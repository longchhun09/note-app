<template>
  <div class="note-panel p-6 note-card">
    <div v-if="loading" class="flex justify-center items-center py-12">
      <Loader class="animate-spin h-10 w-10 text-blue-500" />
    </div>

    <div v-else-if="error" class="bg-red-50 border border-red-200 text-red-800 px-4 py-3 rounded relative mb-6">
      <strong class="font-bold">Error:</strong>
      <span class="block sm:inline">{{ error }}</span>
    </div>

    <div v-else>
      <div class="flex justify-between items-start mb-6">
        <div class="w-3/4 ">
          <h2 v-if="isNew" class="text-2xl font-bold text-gray-800">Create New Note</h2>
          <div v-else-if="isEditing">
            <input type="text" v-model="editableNote.title"
              class="text-2xl font-bold text-gray-800 w-full border-b border-blue-300 focus:border-blue-500 focus:outline-none pb-1"
              placeholder="Note Title" />
          </div>
          <h2 v-else class="text-2xl font-bold text-gray-800 mb-1">{{ note?.title }}</h2>

        </div>

        <div class="flex space-x-2">
          <template v-if="!isEditing && !isNew">
            <Button @click="toggleEditMode" variant="primary" text="Edit" icon="Edit">
            </Button>
            <Button @click="confirmDelete" variant="danger" text="Delete" icon="Trash" />
          </template>
          <template v-else>
            <Button type="submit" form="noteForm" class="bg-green-500 text-white hover:bg-green-600" :disabled="saving" text="Save"
              icon="Save" />
            <Button @click="cancelEdit" class="border border-gray-300" text="Cancel" icon="X" variant="secondary">
            </button>
          </template>
        </div>
      </div>

      <div class="mt-4">
        <Form v-if="isEditing || isNew" :validation-schema="validationSchema" :initial-values="editableNote" @submit="saveNote" class="space-y-4" id="noteForm" :key="`form-${JSON.stringify(editableNote)}`">
          <div v-if="isNew" class="mb-4">
            <label for="title" class="block text-sm font-medium text-gray-700 mb-1">Title</label>
            <Field id="title" name="title" type="text"
              class="w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
              placeholder="Note Title" />
            <ErrorMessage name="title" class="mt-1 text-sm text-red-600" />
          </div>

          <div>
            <label for="content" class="block text-sm font-medium text-gray-700 mb-1">Content</label>
            <Field id="content" name="content" as="textarea" rows="12"
              class="w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
              placeholder="Write your note here..."></Field>
            <ErrorMessage name="content" class="mt-1 text-sm text-red-600" /></div>
        </Form>
        <div v-else class="prose max-w-none">
          <div class="whitespace-pre-line text-gray-700">{{ note?.content }}
            <div class="text-sm text-gray-500 flex space-x-4 mt-2 float-right">
              <span>Created: {{ formatDate(note?.createdAt) }}</span>
              <span>Updated: {{ formatDate(note?.updatedAt) }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <ConfirmationDialog v-model="showDeleteConfirm" icon="Trash" button-variant="danger" @confirm="deleteNote"
      @cancel="showDeleteConfirm = false" />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useNoteStore } from '@/stores/noteStore';
import type { Note } from '@/types/Note';
import { formatDate } from '@/utils/dateFormatter.ts';
import ConfirmationDialog from '@/components/common/ConfirmationDialog.vue';
import Button from '@/components/common/Button.vue';
import { Form, Field, ErrorMessage } from 'vee-validate';
import * as yup from 'yup';
import { Loader } from 'lucide-vue-next';

const props = defineProps({
  id: {
    type: Number,
    required: false,
    default: null
  },
  isNew: {
    type: Boolean,
    default: false
  },
  editMode: {
    type: Boolean,
    default: false
  }
});

// Router and store setup
const route = useRoute();
const router = useRouter();
const noteStore = useNoteStore();

// State
const loading = ref(false);
const saving = ref(false);
const error = ref('');
const note = ref<Note | null>(null);
const editableNote = ref<{ title: string, content: string }>({
  title: '',
  content: ''
});
const isEditing = ref(props.editMode);
const showDeleteConfirm = ref(false);

// Computed properties
const isNew = computed(() => props.isNew);

// Validation schema
const validationSchema = yup.object({
  title: yup.string().required('Title is required'),
  content: yup.string().required('Content is required')
});

// Methods
function initializeNote() {
  if (props.isNew) {
    note.value = null;
    editableNote.value = {
      title: '',
      content: ''
    };
    isEditing.value = true;
  } else if (props.id) {
    loading.value = true;
    error.value = '';

    try {
      const foundNote = noteStore.getNoteById(props.id);
      if (foundNote) {
        note.value = foundNote;
        editableNote.value = {
          title: foundNote.title,
          content: foundNote.content || ''
        };
        isEditing.value = props.editMode;
      } else {
        error.value = 'Note not found';
        router.push('/notes');
      }
    } catch (err) {
      error.value = 'Error loading note: ' + (err instanceof Error ? err.message : String(err));
    } finally {
      loading.value = false;
    }
  }
}

function toggleEditMode() {
  if (!isEditing.value) {
    editableNote.value = {
      title: note.value?.title || '',
      content: note.value?.content || ''
    };
    isEditing.value = true;

    router.replace({
      path: route.path,
      query: { ...route.query, edit: 'true' }
    });
  }
}

function cancelEdit() {
  if (props.isNew) {
    router.push('/notes');
  } else {
    editableNote.value = {
      title: note.value?.title || '',
      content: note.value?.content || ''
    };
    isEditing.value = false;

    router.replace({
      path: route.path,
      query: {}
    });
  }
}


async function saveNote(values: { title: string, content: string }) {
  saving.value = true;
  error.value = '';

  try {
    if (props.isNew) {
      await noteStore.createNote({
        title: values.title,
        content: values.content
      }).then(() => {
        router.push('/notes');
      });
    } else if (note.value) {

      await noteStore.updateNote({
        id: note.value.id,
        title: values.title,
        content: values.content
      });

      const updatedNote = noteStore.getNoteById(note.value.id);

      if (updatedNote) {
        note.value = updatedNote;
      }

      isEditing.value = false;

      router.replace({
        path: route.path,
        query: {}
      });
    }
  } catch (err) {
    error.value = 'Error saving note: ' + (err instanceof Error ? err.message : String(err));
  } finally {
    saving.value = false;
  }
}

function confirmDelete() {
  showDeleteConfirm.value = true;
}

async function deleteNote() {
  if (!note.value) return;

  try {
    await noteStore.deleteNote(note.value.id);
    showDeleteConfirm.value = false;
    router.push('/notes');
  } catch (err) {
    error.value = 'Error deleting note: ' + (err instanceof Error ? err.message : String(err));
    showDeleteConfirm.value = false;
  }
}


watch(() => props.id, () => {
  initializeNote();
});

watch(() => props.editMode, (newValue) => {
  isEditing.value = newValue;
  if (newValue && note.value) {
    editableNote.value = {
      title: note.value.title,
      content: note.value.content ?? ''
    };
  }
});

// Add watch for editableNote to update the form
watch(editableNote, (newValue) => {
  // This ensures edits to title in the header sync to the form
  if (isEditing.value) {
    // The form will pick up these values via :initial-values
    editableNote.value = {
      ...editableNote.value
    };
  }
}, { deep: true });

onMounted(() => {
  initializeNote();
});
</script>

<style scoped="scss">
.note-card {
  display: grid;
  font-size: 1.2rem;
  padding: 1rem;
  border: 1vmin solid;
  background-color: #ffe681;
  box-shadow: 1vmin 1vmin 0 0 color-mix(in lab, currentcolor 80%, #0000);
}
</style>