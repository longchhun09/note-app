<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div class="bg-white">
    <header class="leading-normal max-h-screen lg:flex lg:items-center lg:pr-4">
    <div class="lg:flex lg:items-start lg:flex-wrap">
      <nav class="w-full text-xs text-center mt-8 lg:text-left lg:-ml-4 lg:text-base lg:py-4 lg:mt-4">
        <RouterLink 
          to="/" 
          class="inline-block px-4 first:border-0 border-l border-gray-300 hover:text-blue-600 transition-colors"
          :class="{ 'text-blue-700 font-semibold': $route.path === '/' }"
        >
          Notes
        </RouterLink>
        <RouterLink 
          to="/user" 
          class="inline-block px-4 border-l border-gray-300 hover:text-blue-600 transition-colors"
          :class="{ 'text-blue-700 font-semibold': $route.path === '/user' }"
        >
          Users
        </RouterLink>
      </nav>
    </div>
  </header>

  <RouterView />
  </div>
  
</template>
