<template>
  <div v-if="modelValue" class="fixed inset-0 flex items-center justify-center z-50">
    <div class="fixed inset-0 bg-black bg-opacity-50" @click="onCancel"></div>
    <div class="bg-white rounded-lg p-6 max-w-sm mx-auto relative z-10">
      <h3 class="text-lg font-bold mb-4">{{ title }}</h3>
      <p class="mb-6">{{ message }}</p>
      <div class="flex justify-end space-x-3">
        <button 
          @click="onCancel" 
          class="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
        >
          {{ cancelButtonText }}
        </button>
        <Button 
          @click="onConfirm" 
          :text="confirmButtonText"
          :variant="buttonVariant"
          :icon="icon"
        ></Button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import Button from '@/components/common/Button.vue';

interface Props {
  modelValue: boolean;
  title?: string;
  message?: string;
  confirmButtonText?: string;
  cancelButtonText?: string;
  icon?: string;
  buttonVariant?: 'primary' | 'secondary' | 'danger' | 'success';
}

const props = withDefaults(defineProps<Props>(), {
  title: 'Confirmation',
  message: 'Are you sure you want to proceed?',
  confirmButtonText: 'Confirm',
  cancelButtonText: 'Cancel',
  buttonVariant: 'primary'
});

const emit = defineEmits<{
  (e: 'update:modelValue', value: boolean): void;
  (e: 'confirm'): void;
  (e: 'cancel'): void;
}>();

const onConfirm = () => {
  emit('confirm');
  emit('update:modelValue', false);
};

const onCancel = () => {
  emit('cancel');
  emit('update:modelValue', false);
};
</script>
