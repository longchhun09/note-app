<template>
  <div class="flex flex-col items-center justify-center h-full p-8 text-center">
    <div class="max-w-md mx-auto">
      <h1 class="text-3xl font-bold text-gray-800 mb-6">Welcome to Your Notes</h1>
      
      <div class="bg-white p-6 rounded-lg shadow-md">
        <div class="mb-6">
          <div class="w-20 h-20 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <PenSquare class="h-10 w-10 text-blue-500" />
          </div>
          <p class="text-gray-700 mb-4">
            Select a note from the list or create a new one to get started.
          </p>
        </div>
        
        <div class="space-y-3 text-left mb-6">
          <div class="flex items-start">
            <div class="flex-shrink-0 h-6 w-6 text-blue-500 mr-2">
              <Plus class="h-5 w-5" />
            </div>
            <p class="text-sm text-gray-600">
              <span class="font-semibold">Create new notes</span> by clicking the "+ New Note" button
            </p>
          </div>
          
          <div class="flex items-start">
            <div class="flex-shrink-0 h-6 w-6 text-blue-500 mr-2">
              <PenLine class="h-5 w-5" />
            </div>
            <p class="text-sm text-gray-600">
              <span class="font-semibold">Edit your notes</span> by selecting them and clicking the edit button
            </p>
          </div>
          
          <div class="flex items-start">
            <div class="flex-shrink-0 h-6 w-6 text-blue-500 mr-2">
              <Trash2 class="h-5 w-5" />
            </div>
            <p class="text-sm text-gray-600">
              <span class="font-semibold">Delete notes</span> you no longer need with the delete button
            </p>
          </div>
        </div>
        
        <div class="p-4 bg-blue-50 rounded-md">
          <p class="text-sm text-blue-700">
            <span class="font-bold">Pro tip:</span> Keep your notes organized with clear titles and concise content!
          </p>
        </div>
      </div>
      
      <div class="mt-8">
        <button
          @click="createNewNote"
          class="inline-flex items-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
        >
          <Plus class="-ml-1 mr-2 h-5 w-5" />
          Create New Note
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import { PenSquare, Plus, PenLine, Trash2 } from 'lucide-vue-next';

const router = useRouter();

function createNewNote() {
  router.push('/notes/new');
}
</script>

