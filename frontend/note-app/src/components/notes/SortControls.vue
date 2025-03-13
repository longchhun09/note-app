<template>
  <div class="flex space-x-2">
    <span class="text-sm text-gray-600 self-center">Sort by:</span>
    <button 
      @click="setSortField('title')" 
      class="text-sm py-1 px-2 rounded-md transition-colors"
      :class="sortField === 'title' ? 'bg-blue-100 text-blue-700' : 'bg-gray-100 text-gray-700 hover:bg-gray-200'"
    >
      Title
    </button>
    <button 
      @click="setSortField('updatedAt')" 
      class="text-sm py-1 px-2 rounded-md transition-colors"
      :class="sortField === 'updatedAt' ? 'bg-blue-100 text-blue-700' : 'bg-gray-100 text-gray-700 hover:bg-gray-200'"
    >
      Date
    </button>
    <button 
      @click="toggleSortOrder()" 
      class="text-sm py-1 px-2 rounded-md bg-gray-100 text-gray-700 hover:bg-gray-200"
    >
      <SortAsc v-if="sortOrder === 'asc'" class="h-4 w-4" />
      <SortDesc v-else class="h-4 w-4" />
    </button>
  </div>
</template>

<script setup lang="ts">
import { SortAsc, SortDesc } from 'lucide-vue-next';

const props = defineProps<{
  sortField: 'title' | 'updatedAt';
  sortOrder: 'asc' | 'desc';
}>();

const emit = defineEmits<{
  (e: 'update:sortField', value: 'title' | 'updatedAt'): void;
  (e: 'update:sortOrder', value: 'asc' | 'desc'): void;
  (e: 'sort-change'): void;
}>();

function setSortField(field: 'title' | 'updatedAt') {
  if (field !== props.sortField) {
    emit('update:sortField', field);
    emit('sort-change');
  }
}

function toggleSortOrder() {
  const newOrder = props.sortOrder === 'asc' ? 'desc' : 'asc';
  emit('update:sortOrder', newOrder);
  emit('sort-change');
}
</script>

