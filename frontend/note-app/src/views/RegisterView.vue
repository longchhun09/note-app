<template>
    <div class="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
        <div class="max-w-md w-full space-y-8 bg-white p-10 rounded-lg shadow-md">
            <div>
                <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">Create an account</h2>
                <p class="mt-2 text-center text-sm text-gray-600">
                    Or
                    <router-link to="/login" class="font-medium text-indigo-600 hover:text-indigo-500">
                        sign in to your account
                    </router-link>
                </p>
            </div>
            <Form @submit="handleRegister" :validation-schema="schema" v-slot="{ errors }" class="mt-8 space-y-6">
                <div class="rounded-md shadow-sm -space-y-px">
                    <div class="mb-4">
                        <label for="username" class="sr-only">Username</label>
                        <Field id="username" name="username" type="text"
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            :class="{ 'border-red-500': errors.username }" placeholder="Username" />
                        <ErrorMessage name="username" class="text-red-500 text-sm mt-1" />
                    </div>
                    <div class="mb-4">
                        <label for="email" class="sr-only">Email</label>
                        <Field id="email" name="email" type="text"
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            :class="{ 'border-red-500': errors.email }" placeholder="Email" />
                        <ErrorMessage name="email" class="text-red-500 text-sm mt-1" />
                    </div>
                    <div class="mb-4">
                        <label for="password" class="sr-only">Password</label>
                        <Field id="password" name="password" type="password"
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            :class="{ 'border-red-500': errors.password }" placeholder="Password" />
                        <ErrorMessage name="password" class="text-red-500 text-sm mt-1" />
                    </div>
                    <div>
                        <label for="confirmPassword" class="sr-only">Confirm Password</label>
                        <Field id="confirmPassword" name="confirmPassword" type="password"
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            :class="{ 'border-red-500': errors.confirmPassword }" placeholder="Confirm Password" />
                        <ErrorMessage name="confirmPassword" class="text-red-500 text-sm mt-1" />
                    </div>
                </div>

                <div v-if="errorMessage" class="p-3 bg-red-100 text-red-700 rounded-md">
                    {{ errorMessage }}
                </div>

                <div>
                    <button type="submit" :disabled="isLoading"
                        class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                        :class="{ 'opacity-70 cursor-not-allowed': isLoading }">
                        <span v-if="isLoading" class="flex items-center">
                            <svg class="animate-spin -ml-1 mr-2 h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg"
                                fill="none" viewBox="0 0 24 24">
                                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor"
                                    stroke-width="4"></circle>
                                <path class="opacity-75" fill="currentColor"
                                    d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z">
                                </path>
                            </svg>
                            Registering...
                        </span>
                        <span v-else class="flex items-center">
                            <UserPlus class="mr-2" size="16" /> Register
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
import { UserPlus } from 'lucide-vue-next';
import { Form, Field, ErrorMessage } from 'vee-validate';
import * as yup from 'yup';

const router = useRouter();
const authStore = useAuthStore();

const isLoading = ref(false);
const errorMessage = ref('');

const schema = yup.object({
    username: yup.string().required('Username is required'),
    email: yup.string().required('Email is required').email('Please enter a valid email'),
    password: yup.string()
        .required('Password is required')
        .min(6, 'Password must be at least 6 characters'),
    confirmPassword: yup.string()
        .required('Please confirm your password')
        .oneOf([yup.ref('password')], 'Passwords do not match')
});

const handleRegister = async (values) => {
    errorMessage.value = '';
    isLoading.value = true;
    
    try {
        await authStore.register({
            username: values.username.trim(),
            password: values.password,
            email: values.email,
            confirmPassword: values.confirmPassword
        });
        
        router.push({ name: 'login' });
    } catch (error) {
        console.error('Registration error:', error);
        errorMessage.value = authStore.error || 'Failed to register. Please try again.';
    } finally {
        isLoading.value = false;
    }
};
</script>
