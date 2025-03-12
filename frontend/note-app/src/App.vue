<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useAuthStore } from './stores/authStore';
import { ref, computed, onMounted, onUnmounted } from 'vue';
const authStore = useAuthStore();
let isAuthenticate = ref(false);
const isDropdownOpen = ref(false);
const isMobileMenuOpen = ref(false);

authStore.checkAuth().then(authenticated => {
  isAuthenticate.value = authenticated;
});

function logOut() {
  authStore.logout().then(() => {
    isAuthenticate.value = false;
    isDropdownOpen.value = false;
  });
}

const userInitials = computed(() => {
  if (!authStore.user || !authStore.user.username) return '?';

  const username = authStore.user.username;
  if (username.includes(' ')) {
    const names = username.split(' ');
    return (names[0][0] + names[names.length - 1][0]).toUpperCase();
  } else {
    return username.substring(0, 2).toUpperCase();
  }
});

const handleClickOutside = (event: MouseEvent) => {
  const target = event.target as HTMLElement;
  if (isDropdownOpen.value && !target.closest('.user-dropdown')) {
    isDropdownOpen.value = false;
  }
};

onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside);
});
</script>

<template>
  <div :class="{ 'home': isAuthenticate }">
    <div class="w-full min-h-screen flex flex-col">
      <nav v-if="isAuthenticate" class="border-b border-gray-200 shadow-sm transition-all duration-300">
        <div class="w-full px-4">
          <div class="flex justify-between h-16">
            <div class="flex items-center flex-shrink-0">
              <router-link to="/"
                class="px-3 py-2 rounded-md text-sm font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-100 transition-all duration-200">
                Home
              </router-link>
            </div>

            <div class="hidden sm:flex sm:items-center">
              <template v-if="isAuthenticate">
                <div class="ml-4 flex items-center md:ml-6">
                  <div class="ml-3 relative user-dropdown">
                    <div>
                      <button @click="isDropdownOpen = !isDropdownOpen"
                        class="flex items-center max-w-xs rounded-full text-sm focus:outline-none transition-all duration-150 group"
                        id="user-menu" aria-haspopup="true">
                        <div
                          class="h-9 w-9 rounded-full bg-blue-600 flex items-center justify-center text-white font-medium shadow group-hover:bg-blue-700 transition-colors duration-200">
                          {{ userInitials }}
                        </div>
                        <span v-if="authStore.user"
                          class="ml-2 text-gray-700 font-medium hidden lg:block group-hover:text-gray-900">
                          {{ authStore.user.username }}
                        </span>
                        <svg class="ml-1 h-5 w-5 text-gray-400 group-hover:text-gray-600"
                          xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                          <path fill-rule="evenodd"
                            d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                            clip-rule="evenodd" />
                        </svg>
                      </button>
                    </div>

                    <transition enter-active-class="transition ease-out duration-100"
                      enter-from-class="transform opacity-0 scale-95" enter-to-class="transform opacity-100 scale-100"
                      leave-active-class="transition ease-in duration-75"
                      leave-from-class="transform opacity-100 scale-100" leave-to-class="transform opacity-0 scale-95">
                      <div v-if="isDropdownOpen"
                        class="origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5 z-50"
                        role="menu" aria-orientation="vertical" aria-labelledby="user-menu">
                        <router-link to="/profile"
                          class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 transition-colors duration-150"
                          role="menuitem">
                          Your Profile
                        </router-link>
                        <a @click="logOut"
                          class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-red-600 cursor-pointer transition-colors duration-150"
                          role="menuitem">
                          Logout
                        </a>
                      </div>
                    </transition>
                  </div>
                </div>
              </template>
            </div>

            <div class="flex items-center sm:hidden">
              <button v-if="isAuthenticate" @click="isMobileMenuOpen = !isMobileMenuOpen"
                class="inline-flex items-center justify-center p-2 rounded-md text-gray-500 hover:text-gray-700 hover:bg-gray-100 focus:outline-none transition-colors duration-150"
                aria-expanded="false">
                <span class="sr-only">Open main menu</span>
                <svg v-if="!isMobileMenuOpen" class="block h-6 w-6" fill="none" viewBox="0 0 24 24"
                  stroke="currentColor" aria-hidden="true">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
                </svg>
                <svg v-if="isMobileMenuOpen" class="block h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"
                  aria-hidden="true">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
            </div>
          </div>
        </div>

        <transition enter-active-class="transition-all duration-300 ease-out" enter-from-class="opacity-0 max-h-0"
          enter-to-class="opacity-100 max-h-64" leave-active-class="transition-all duration-200 ease-in"
          leave-from-class="opacity-100 max-h-64" leave-to-class="opacity-0 max-h-0">
          <div v-if="isAuthenticate && isMobileMenuOpen" class="sm:hidden overflow-hidden">
            <div class="pt-2 pb-3 space-y-1">
              <router-link to="/"
                class="block px-3 py-2 rounded-md text-base font-medium text-gray-700 hover:text-gray-900 hover:bg-gray-50 transition-colors duration-150">
                Home
              </router-link>
            </div>
            <div class="pt-4 pb-3 border-t border-gray-200">
              <div class="flex items-center px-4">
                <div class="flex-shrink-0">
                  <div
                    class="h-10 w-10 rounded-full bg-blue-600 flex items-center justify-center text-white font-medium">
                    {{ userInitials }}
                  </div>
                </div>
                <div class="ml-3">
                  <div v-if="authStore.user" class="text-base font-medium text-gray-800">{{ authStore.user.username }}
                  </div>
                </div>
              </div>
              <div class="mt-3 space-y-1">
                <router-link to="/profile"
                  class="block px-4 py-2 text-base font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-50 transition-colors duration-150">
                  Your Profile
                </router-link>
                <router-link to="/settings"
                  class="block px-4 py-2 text-base font-medium text-gray-600 hover:text-gray-900 hover:bg-gray-50 transition-colors duration-150">
                  Settings
                </router-link>
                <a @click="logOut"
                  class="block px-4 py-2 text-base font-medium text-gray-600 hover:text-red-600 hover:bg-gray-50 transition-colors duration-150 cursor-pointer">
                  Logout
                </a>
              </div>
            </div>
          </div>
        </transition>
      </nav>

      <main :class="{ 'flex-1': isAuthenticate }">
        <RouterView />
      </main>
    </div>
  </div>

</template>
