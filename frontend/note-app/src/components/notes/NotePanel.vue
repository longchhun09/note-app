<template>
  <div class="note-panel bg-white rounded-lg shadow p-6">
    <div v-if="loading" class="flex justify-center items-center py-12">
      <svg class="animate-spin h-10 w-10 text-blue-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <div v-else-if="error" class="bg-red-50 border border-red-200 text-red-800 px-4 py-3 rounded relative mb-6">
      <strong class="font-bold">Error:</strong>
      <span class="block sm:inline">{{ error }}</span>
    </div>

    <div v-else>
      <div class="flex justify-between items-start mb-6">
        <div class="w-3/4">
          <h2 v-if="isNew" class="text-2xl font-bold text-gray-800">Create New Note</h2>
          <div v-else-if="isEditing">
            <input 
              type="text" 
              v-model="editableNote.title" 
              class="text-2xl font-bold text-gray-800 w-full border-b border-blue-300 focus:border-blue-500 focus:outline-none pb-1"
              placeholder="Note Title"
            />
          </div>
          <h2 v-else class="text-2xl font-bold text-gray-800 mb-1">{{ note?.title }}</h2>
          
          <div v-if="!isEditing && !isNew" class="text-sm text-gray-500 flex space-x-4 mt-2">
            <span>Created: {{ formatDate(note?.createdAt) }}</span>
            <span>Updated: {{ formatDate(note?.updatedAt) }}</span>
          </div>
        </div>

        <div class="flex space-x-2">
          <template v-if="!isEditing && !isNew">
            <Button 
              @click="toggleEditMode" 
              variant="primary"
              text="Edit"
              icon="Edit"
            >
            </Button>
            <Button 
              @click="confirmDelete" 
              variant="danger"
              text="Delete"
              icon="Trash"
            />
          </template>
          <template v-else>
            <Button 
              @click="saveNote" 
              class="bg-green-500 text-white hover:bg-green-600"
              :disabled="saving"
              text="Save"
              icon="Save"
            />
            <Button
              @click="cancelEdit" 
              class="border border-gray-300"
              text="Cancel"
              icon="X"
              variant="secondary"
            >
            </button>
          </template>
        </div>
      </div>

      <div class="mt-4">
        <div v-if="isEditing || isNew" class="space-y-4">
          <div v-if="isNew" class="mb-4">
            <label for="title" class="block text-sm font-medium text-gray-700 mb-1">Title</label>
            <input 
              id="title"
              type="text" 
              v-model="editableNote.title" 
              class="w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
              placeholder="Note Title"
            />
            <p v-if="titleError" class="mt-1 text-sm text-red-600">{{ titleError }}</p>
          </div>
          
          <div>
            <label for="content" class="block text-sm font-medium text-gray-700 mb-1">Content</label>
            <textarea 
              id="content"
              v-model="editableNote.content" 
              rows="12"
              class="w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
              placeholder="Write your note here..."
            ></textarea>
            <p v-if="contentError" class="mt-1 text-sm text-red-600">{{ contentError }}</p>
          </div>
        </div>

        <div v-else class="prose max-w-none">
          <div class="whitespace-pre-line text-gray-700">{{ note?.content }}</div>
        </div>
      </div>
    </div>

        <ConfirmationDialog
          v-model="showDeleteConfirm"
          icon="Trash"
          button-variant="danger"
          @confirm="deleteNote"
          @cancel="showDeleteConfirm = false"
        />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useNoteStore } from '@/stores/noteStore';
import type { Note } from '@/types/Note';
import { formatDate } from '@/utils/dateFormatter';
import ConfirmationDialog from '@/components/dialog/ConfirmationDialog.vue';
import Button from '@/components/common/Button.vue';

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
const titleError = ref('');
const contentError = ref('');

// Computed properties
const isNew = computed(() => props.isNew);

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
    titleError.value = '';
    contentError.value = '';
    
    router.replace({ 
      path: route.path, 
      query: {} 
    });
  }
}

function validateForm() {
  let isValid = true;
  
  titleError.value = '';
  contentError.value = '';
  
  if (!editableNote.value.title.trim()) {
    titleError.value = 'Title is required';
    isValid = false;
  }
  
  if (!editableNote.value.content.trim()) {
    contentError.value = 'Content is required';
    isValid = false;
  }
  
  return isValid;
}

async function saveNote() {
  if (!validateForm()) return;
  
  saving.value = true;
  error.value = '';
  
  try {
    if (props.isNew) {
      await noteStore.createNote({
        title: editableNote.value.title,
        content: editableNote.value.content
      });
      router.push('/notes');
    } else if (note.value) {
      
      await noteStore.updateNote({
        id: note.value.id,
        title: editableNote.value.title,
        content: editableNote.value.content
      });
      
      const updatedNote = noteStore.getNoteById(note.value.id);
      console.log('updatedNote', updatedNote);
      
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
      content: note.value.content
    };
  }
});

onMounted(() => {
  initializeNote();
});
</script>

