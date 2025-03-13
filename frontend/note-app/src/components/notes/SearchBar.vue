<template>
  <div class="relative">
    <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
      <Search class="h-4 w-4 text-gray-400" />
    </div>
    <input 
      type="text" 
      v-model="searchValue" 
      placeholder="Search notes..." 
      class="pl-10 pr-4 py-2 w-full border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
    />
  </div>
  
  <div v-if="showHelperText" class="mt-1 text-sm text-amber-600">
      Please enter at least 3 characters to search
    </div>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import { Search } from 'lucide-vue-next';

const model = defineModel<string>();
const searchValue = ref(model.value || '');
let timeout: number | null = null;

const showHelperText = computed(() => {
  return searchValue.value.length > 0 && searchValue.value.length < 3;
});

watch(searchValue, (value) => {
  if (timeout) clearTimeout(timeout);
  
  timeout = setTimeout(() => {
    if (value.length === 0 || value.length >= 3) {
      model.value = value;
    }
  }, 300);
});

watch(model, (newValue) => {
  if (newValue !== searchValue.value) {
    searchValue.value = newValue || '';
  }
});
</script>

