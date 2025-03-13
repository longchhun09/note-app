<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100 py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8 bg-white p-10 rounded-lg shadow-md">
      <div>
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">Sign in to your account</h2>
        <p class="mt-2 text-center text-sm text-gray-600">
          Or
          <router-link to="/register" class="font-medium text-indigo-600 hover:text-indigo-500">
            create a new account
          </router-link>
        </p>
      </div>
      <Form @submit="handleLogin" :validation-schema="schema" class="mt-8 space-y-6" v-slot="{ errors }">
        <div class="rounded-md shadow-sm -space-y-px">
          <div class="mb-4">
            <label for="username" class="sr-only">Username</label>
            <Field
              id="username"
              name="username"
              type="text"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              :class="{ 'border-red-500': errors.username }"
              placeholder="Username"
            />
            <ErrorMessage name="username" class="text-red-500 text-sm mt-1" />
          </div>
          <div>
            <label for="password" class="sr-only">Password</label>
            <Field
              id="password"
              name="password"
              type="password"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              :class="{ 'border-red-500': errors.password }"
              placeholder="Password"
            />
            <ErrorMessage name="password" class="text-red-500 text-sm mt-1" />
          </div>
        </div>
        
        <div v-if="errorMessage" class="p-3 bg-red-100 text-red-700 rounded-md">
          {{ errorMessage }}
        </div>

        <div>
          <button
            type="submit"
            :disabled="isLoading"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
            :class="{ 'opacity-70 cursor-not-allowed': isLoading }"
          >
            <span v-if="isLoading" class="flex items-center">
              <Loader class="animate-spin -ml-1 mr-2 h-4 w-4 text-white" />
              Signing in...
            </span>
            <span v-else class="flex items-center">
              <LogIn class="mr-2" size="16" /> Sign in
            </span>
          </button>
        </div>
      </Form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import { LogIn, Loader } from 'lucide-vue-next';
import { Form, Field, ErrorMessage } from 'vee-validate';
import * as yup from 'yup';

const router = useRouter();
const authStore = useAuthStore();

const isLoading = ref(false);
const errorMessage = ref('');

// Define validation schema
const schema = yup.object({
  username: yup.string().required('Username is required'),
  password: yup.string().required('Password is required')
});

const handleLogin = async (values) => {
  errorMessage.value = '';
  
  try {
    isLoading.value = true;
    await authStore.login({
      username: values.username.trim(),
      password: values.password
    }).then(() => {
      router.push({ name: 'notes' });
    });
  } catch (error) {
    console.error('Login error:', error);
    errorMessage.value = authStore.error || 'Failed to login. Please check your credentials and try again.';
  } finally {
    isLoading.value = false;
  }
};
</script>

