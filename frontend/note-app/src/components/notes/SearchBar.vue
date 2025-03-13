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
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import { Search } from 'lucide-vue-next';

const props = defineProps<{
  modelValue: string;
}>();

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void;
}>();

const searchValue = ref(props.modelValue);

watch(searchValue, (newValue) => {
  emit('update:modelValue', newValue);
});

watch(() => props.modelValue, (newValue) => {
  if (newValue !== searchValue.value) {
    searchValue.value = newValue;
  }
});
</script>

