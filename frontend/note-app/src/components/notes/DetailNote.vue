<template>
    <div class="container mx-auto px-4 py-8">
      <div v-if="loading" class="flex justify-center items-center h-64">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
      </div>
  
      <div v-else-if="error" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-4" role="alert">
        <strong class="font-bold">Error!</strong>
        <span class="block sm:inline"> {{ error }}</span>
      </div>
  
      <div v-else-if="note" class="bg-white shadow-md rounded-lg p-6">
        <!-- Back button -->
        <div class="mb-6">
          <button @click="goBack" class="flex items-center text-blue-500 hover:text-blue-700">
            <ArrowLeft class="mr-2" size="18" /> Back to Notes
          </button>
        </div>
  
        <h1 class="text-3xl font-bold text-gray-800 mb-4">{{ note.title }}</h1>
  
        <div class="flex flex-wrap text-sm text-gray-500 mb-6">
          <div class="mr-4">
            <span class="font-semibold">Created:</span> {{ formatDate(note.createdAt) }}
          </div>
          <div>
            <span class="font-semibold">Updated:</span> {{ formatDate(note.updatedAt) }}
          </div>
        </div>
  
        <div class="prose max-w-none mb-8 text-gray-700 whitespace-pre-wrap">
          {{ note.content }}
        </div>
  
        <div class="flex mt-6">
          <button @click="editNote" class="bg-blue-500 text-white px-4 py-2 rounded-md mr-4 hover:bg-blue-600 flex items-center">
            <Edit class="mr-1" size="18" /> Edit
          </button>
          <button @click="confirmDelete" class="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600 flex items-center">
            <Trash class="mr-1" size="18" /> Delete
          </button>
        </div>
      </div>
  
      <div v-else class="bg-yellow-100 border border-yellow-400 text-yellow-700 px-4 py-3 rounded relative mb-4" role="alert">
        <strong class="font-bold">Note not found!</strong>
        <span class="block sm:inline"> The requested note could not be found.</span>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import { useNoteStore } from '@/stores/noteStore';
  import { ArrowLeft, Edit, Trash } from 'lucide-vue-next';
  import { formatDate } from '@/utils/dateFormatter.js';

  const route = useRoute();
  const router = useRouter();
  const noteStore = useNoteStore();
  
  const note = ref(null);
  const loading = ref(true);
  const error = ref(null);
  
  onMounted(() => {
    try {
      loading.value = true;
      const noteId = parseInt(route.params.id, 10);
      const fetchedNote = noteStore.getNoteById(noteId);
      
      if (fetchedNote) {
        note.value = fetchedNote;
      } else {
        error.value = "Note not found";
      }
    } catch (err) {
      console.error('Error fetching note:', err);
      error.value = err.message || "Failed to load note details";
    } finally {
      loading.value = false;
    }
  });

  const goBack = () => {
    router.push('/');
  };
  
  const editNote = () => {
    router.push(`/note/edit/${route.params.id}`);
  };
  
  const confirmDelete = async () => {
    if (confirm('Are you sure you want to delete this note?')) {
      try {
        await noteStore.deleteNote(route.params.id);
        router.push('/');
      } catch (err) {
        error.value = err.message || "Failed to delete note";
      }
    }
  };
  </script>
  
  