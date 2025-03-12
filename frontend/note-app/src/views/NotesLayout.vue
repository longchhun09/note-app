<template>
  <div class="notes-layout flex h-screen w-screen">
    <div class="notes-sidebar w-80 border-r border-gray-200 overflow-y-auto">
      <ListNote />
    </div>
    <div class="notes-content flex-1 p-6 overflow-y-auto">
      <router-view />
    </div>
  </div>
</template>

<script setup lang="ts">
import ListNote from '@/components/notes/ListNote.vue';
</script>

<style scoped>
.notes-layout {
  height: calc(100vh - 60px);
}

@media (max-width: 768px) {
  .notes-layout {
    flex-direction: column;
  }
  
  .notes-sidebar {
    width: 100%;
    height: 40vh;
    border-right: none;
    border-bottom: 1px solid #eee;
  }
  
  .notes-content {
    height: 60vh;
  }
}
</style>

