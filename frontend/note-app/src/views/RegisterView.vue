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
            <form class="mt-8 space-y-6" @submit.prevent="handleRegister">
                <div class="rounded-md shadow-sm -space-y-px">
                    <div class="mb-4">
                        <label for="username" class="sr-only">Username</label>
                        <input id="username" v-model="formData.username" name="username" type="text" required
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            :class="{ 'border-red-500': errors.username }" placeholder="Username" />
                        <p v-if="errors.username" class="text-red-500 text-sm mt-1">{{ errors.username }}</p>
                    </div>
                    <div class="mb-4">
                        <label for="email" class="sr-only">Email</label>
                        <input id="email" v-model="formData.email" name="email" type="text" required
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            :class="{ 'border-red-500': errors.email }" placeholder="Email" />
                        <p v-if="errors.email" class="text-red-500 text-sm mt-1">{{ errors.email }}</p>
                    </div>
                    <div class="mb-4">
                        <label for="password" class="sr-only">Password</label>
                        <input id="password" v-model="formData.password" name="password" type="password" required
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            :class="{ 'border-red-500': errors.password }" placeholder="Password" />
                        <p v-if="errors.password" class="text-red-500 text-sm mt-1">{{ errors.password }}</p>
                    </div>
                    <div>
                        <label for="confirmPassword" class="sr-only">Confirm Password</label>
                        <input id="confirmPassword" v-model="formData.confirmPassword" name="confirmPassword"
                            type="password" required
                            class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                            :class="{ 'border-red-500': errors.confirmPassword }" placeholder="Confirm Password" />
                        <p v-if="errors.confirmPassword" class="text-red-500 text-sm mt-1">{{ errors.confirmPassword }}
                        </p>
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
            </form>
        </div>
    </div>
</template>

<script setup>
import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/authStore';
import { UserPlus } from 'lucide-vue-next';

const router = useRouter();
const authStore = useAuthStore();

const formData = reactive({
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
});

const errors = reactive({
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
});

const isLoading = ref(false);
const errorMessage = ref('');

const validateForm = () => {
    let isValid = true;

    // Reset errors
    errors.username = '';
    errors.email = '';
    errors.password = '';
    errors.confirmPassword = '';

    // Validate username
    if (!formData.username.trim()) {
        errors.username = 'Username is required';
        isValid = false;
    }

    // Validate password
    if (!formData.password) {
        errors.password = 'Password is required';
        isValid = false;
    } else if (formData.password.length < 6) {
        errors.password = 'Password must be at least 6 characters';
        isValid = false;
    }

    // Validate confirm password
    if (!formData.confirmPassword) {
        errors.confirmPassword = 'Please confirm your password';
        isValid = false;
    } else if (formData.password !== formData.confirmPassword) {
        errors.confirmPassword = 'Passwords do not match';
        isValid = false;
    }

    return isValid;
};

const handleRegister = async () => {
    if (!validateForm()) {
        return;
    }

    errorMessage.value = '';

    try {
        isLoading.value = true;
        await authStore.register({
            username: formData.username.trim(),
            password: formData.password,
            email: formData.email,
            confirmPassword: formData.confirmPassword
        });

        // Navigate to login page after successful registration
        router.push({ name: 'login' });
    } catch (error) {
        console.error('Registration error:', error);
        errorMessage.value = authStore.error || 'Failed to register. Please try again.';
    } finally {
        isLoading.value = false;
    }
};
</script>
