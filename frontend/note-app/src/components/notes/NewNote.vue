<template>
<div class="max-w-7xl mx-auto p-5">
    <h2 class="text-2xl font-bold mb-6 text-gray-800">Create New Note</h2>
    <form @submit.prevent="saveNote" class="space-y-6">
      <div class="space-y-2">
        <label for="title" class="block text-sm font-medium text-gray-700">Title</label>
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
        <label for="content" class="block text-sm font-medium text-gray-700">Content</label>
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
          :disabled="isLoading"
          class="px-4 py-2 text-sm font-medium text-white bg-indigo-600 border border-transparent rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
          :class="{ 'opacity-70 cursor-not-allowed': isLoading }"
        >
          <span v-if="isLoading">
            <svg class="animate-spin -ml-1 mr-2 h-4 w-4 text-white inline-block" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            Saving...
          </span>
          <span v-else>Save Note</span>
        </button>
      </div>
    </form>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { useNoteStore } from '@/stores/noteStore';

const router = useRouter();
const noteStore = useNoteStore();

const note = reactive({
  title: '',
  content: ''
});

const isLoading = ref(false);
const errorMessage = ref('');
const errors = reactive({
  title: '',
  content: ''
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
  if (!note.content.trim()) {
    errors.content = 'Content is required';
    isValid = false;
  }
  
  return isValid;
};

const saveNote = async () => {
  if (!validateForm()) {
    return;
  }
  
  try {
    isLoading.value = true;
    errorMessage.value = '';
    
    // Create a new note through the store
    await noteStore.createNote({
      title: note.title.trim(),
      content: note.content.trim()
    });
    
    // Navigate to note list on success
    router.push({ name: 'notes' });
  } catch (error) {
    console.error('Failed to create note:', error);
    errorMessage.value = error.message || 'Failed to create note. Please try again.';
  } finally {
    isLoading.value = false;
  }
};

const cancelForm = () => {
  router.push({ name: 'notes' });
};
</script>

