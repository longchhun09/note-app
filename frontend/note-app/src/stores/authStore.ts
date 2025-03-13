import { defineStore } from 'pinia';
import type { LoginRequest, RegisterRequest } from '@/types/Auth';
import type { AuthState } from '@/types/Auth';
import { authApi } from '@/services';
import router from '@/router';

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    user: null,
    token: localStorage.getItem('token') || null,
    isAuthenticated: !!localStorage.getItem('token'),
    isLoading: false,
    error: null
  }),

  getters: {
    currentUser: (state) => state.user
  },

  actions: {
    async login(credentials: LoginRequest) {
      this.isLoading = true;
      this.error = null;

      try {
        const response = await authApi.login(credentials);
        
        if (response.data) {
          this.token = response.data.token;
          this.user = {
            username: response.data.username
          };
          this.isAuthenticated = true;
          localStorage.setItem('token', response.data.token);
          authApi.setAuthToken(response.data.token);
          return response.data;
        }
        return null;
      } catch (error: any) {
        this.handleAuthError(error);
        throw error;
      } finally {
        this.isLoading = false;
      }
    },

    async register(userData: RegisterRequest) {
      this.isLoading = true;
      this.error = null;

      try {
        const response = await authApi.register(userData);
        
        if (response.data) {
          this.token = response.data.token;
          this.user = {
            username: response.data.username
          };
          this.isAuthenticated = true;
          localStorage.setItem('token', response.data.token);
          authApi.setAuthToken(response.data.token);
          return response.data;
        }
        return null;
      } catch (error: any) {
        this.handleAuthError(error);
        throw error;
      } finally {
        this.isLoading = false;
      }
    },

    async logout() {
      this.isLoading = true;
      this.error = null;

      try {
        await authApi.logout();
        console.log('Logged out');
        
        this.clearAuthState();
        router.push('/login');
      } catch (error: any) {
        this.error = error.message || 'Failed to logout';
      } finally {
        this.isLoading = false;
      }
    },

    async checkAuth() {
      if (!this.token) {
        this.clearAuthState();
        return false;
      }

      this.isLoading = true;
      this.error = null;
      authApi.setAuthToken(this.token);

      try {
        const response = await authApi.checkAuth();
        
        if (response.data && response.data.isAuthenticated && response.data.username) {
          this.user = {
            username: response.data.username,
          };
          this.isAuthenticated = response.data.isAuthenticated;
          return true;
        }
        
        this.clearAuthState();
        return false;
      } catch (error: any) {
        this.clearAuthState();
        return false;
      } finally {
        this.isLoading = false;
      }
    },

    handleAuthError(error: any) {
      this.error = error.message || 'Authentication failed';
      this.clearAuthState();
    },

    clearAuthState() {      
      this.user = null;
      this.token = null;
      this.isAuthenticated = false;
      localStorage.removeItem('token');
      authApi.removeAuthToken();
    }
  }
});
