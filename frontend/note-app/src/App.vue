<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useAuthStore } from './stores/authStore';
import { ref } from 'vue';

const authStore = useAuthStore();
let isAuthenticate = ref(false);

authStore.checkAuth().then(authenticated => {
  isAuthenticate.value = authenticated;
});

function logOut() {
  authStore.logout();
  isAuthenticate.value = false;
}
</script>

<template>
  <div class="bg-white w-full">
    <header class="leading-normal max-h-screen lg:flex lg:items-center lg:pr-4">
      <div class="flex items-center justify-between p-4">
        <div class="flex items-center">
          <template v-if="isAuthenticate">
            <router-link to="/" class="text-gray-600 hover:text-gray-800">Home</router-link>
            <a @click="logOut" class="text-gray-600 hover:text-gray-800">Logout</a>
          </template>
        </div>
      </div>
    </header>
    <RouterView />
  </div>

</template>
