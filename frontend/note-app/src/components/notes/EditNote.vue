<template>
<div class="bg-white rounded-lg p-5 shadow-md hover:shadow-xl hover:-translate-y-1 transition-all duration-200 cursor-pointer relative overflow-hidden" 
>
    <div v-if="isLoadingNote" class="flex justify-center items-center py-20">
      <svg class="animate-spin h-10 w-10 text-indigo-600" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <div v-else-if="initialLoadError" class="p-6 bg-red-100 text-red-700 rounded-md">
      <h3 class="text-lg font-medium mb-2">Error Loading Note</h3>
      <p>{{ initialLoadError }}</p>
      <button 
        @click="goBack" 
        class="mt-4 px-4 py-2 bg-gray-100 hover:bg-gray-200 text-gray-800 rounded-md flex items-center"
      >
        <ArrowLeft class="mr-2" size="18" /> Back to Notes
      </button>
    </div>

    <form v-else @submit.prevent="updateNote" class="space-y-6">
      <div class="space-y-2">
        <input
          type="text"
          id="title"
          v-model="note.title"
          class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
          :class="{ 'border-red-500': errors.title }"
          placeholder="Enter note title"
        />
        <p v-if="errors.title" class="text-red-500 text-sm mt-1">{{ errors.title }}</p>
      </div>

      <div class="space-y-2">
        <textarea
          id="content"
          v-model="note.content"
          rows="6"
          class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
          :class="{ 'border-red-500': errors.content }"
          placeholder="Enter note content"
        ></textarea>
        <p v-if="errors.content" class="text-red-500 text-sm mt-1">{{ errors.content }}</p>
      </div>

      <div v-if="errorMessage" class="p-3 bg-red-100 text-red-700 rounded-md">
        {{ errorMessage }}
      </div>

      <div class="flex items-center justify-between pt-4">
        <button
          type="button"
          @click="cancelForm"
          class="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
        >
          Cancel
        </button>
        <button
          type="submit"
          :disabled="isSubmitting"
          class="px-4 py-2 text-sm font-medium text-white bg-indigo-600 border border-transparent rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
          :class="{ 'opacity-70 cursor-not-allowed': isSubmitting }"
        >
          <span v-if="isSubmitting">
            <svg class="animate-spin -ml-1 mr-2 h-4 w-4 text-white inline-block" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            Updating...
          </span>
          <span v-else>Update Note</span>
        </button>
      </div>
    </form>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useNoteStore } from '@/stores/noteStore';
import { ArrowLeft } from 'lucide-vue-next';

const router = useRouter();
const route = useRoute();
const noteStore = useNoteStore();
const noteId = parseInt(route.params.id);

const note = reactive({
  id: noteId,
  title: '',
  content: ''
});

const isLoadingNote = ref(true);
const isSubmitting = ref(false);
const errorMessage = ref('');
const initialLoadError = ref('');
const errors = reactive({
  title: '',
  content: ''
});

onMounted(async () => {
  try {
    let existingNote = noteStore.getNoteById(noteId);
    
    if (!existingNote) {
      await noteStore.fetchNotes();
      existingNote = noteStore.getNoteById(noteId);
    }
    
    if (existingNote) {
      note.title = existingNote.title;
      note.content = existingNote.content || '';
    } else {
      initialLoadError.value = `Note with ID ${noteId} not found.`;
    }
  } catch (error) {
    console.error('Failed to load note:', error);
    initialLoadError.value = error.message || 'Failed to load note. Please try again.';
  } finally {
    isLoadingNote.value = false;
  }
});

const validateForm = () => {
  let isValid = true;
  
  // Reset errors
  errors.title = '';
  errors.content = '';
  
  // Validate title
  if (!note.title.trim()) {
    errors.title = 'Title is required';
    isValid = false;
  }
  
  // Validate content
  if (!note.content?.trim()) {
    errors.content = 'Content is required';
    isValid = false;
  }
  
  return isValid;
};

const updateNote = async () => {
  if (!validateForm()) {
    return;
  }
  
  try {
    isSubmitting.value = true;
    errorMessage.value = '';
    
    await noteStore.updateNote({
      id: noteId,
      title: note.title.trim(),
      content: note.content.trim()
    });
    
    router.push({ name: 'notes' });
  } catch (error) {
    console.error('Failed to update note:', error);
    errorMessage.value = error.message || 'Failed to update note. Please try again.';
  } finally {
    isSubmitting.value = false;
  }
};

const cancelForm = () => {
  router.push({ name: 'note-detail', params: { id: noteId } });
};

const goBack = () => {
  router.push({ name: 'notes' });
};
</script>

