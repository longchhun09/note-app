import { defineStore } from 'pinia';
import axios, { AxiosError } from 'axios';
import type { AuthState, LoginRequest, RegisterRequest, AuthResponse, User } from '@/types/Auth';

const API_URL = 'http://localhost:5289/api/auth';

export const useAuthStore = defineStore('authStore', {
  state: (): AuthState => ({
    user: null,
    token: localStorage.getItem('token'),
    isAuthenticated: !!localStorage.getItem('token'),
    isLoading: false,
    error: null,
  }),
  
  getters: {
    getUser: (state) => state.user,
    getToken: (state) => state.token,
    getIsAuthenticated: (state) => state.isAuthenticated,
  },
  
  actions: {
    async login(credentials: LoginRequest) {
      this.isLoading = true;
      this.error = null;
      
      try {
        const response = await axios.post<AuthResponse>(`${API_URL}/login`, credentials);
        this.handleAuthSuccess(response.data);
        return response.data;
      } catch (error) {
        return this.handleAuthError(error as AxiosError, 'Login failed');
      } finally {
        this.isLoading = false;
      }
    },
    
    async register(userInfo: RegisterRequest) {
      this.isLoading = true;
      this.error = null;
      
      try {
        const response = await axios.post<AuthResponse>(`${API_URL}/register`, userInfo);
        this.handleAuthSuccess(response.data);
        return response.data;
      } catch (error) {
        return this.handleAuthError(error as AxiosError, 'Registration failed');
      } finally {
        this.isLoading = false;
      }
    },
    
    async logout() {
      this.isLoading = true;
      
      try {
        // Call logout endpoint if available
        if (this.token) {
          await axios.post(`${API_URL}/logout`, {}, {
            headers: { Authorization: `Bearer ${this.token}` }
          });
        }
      } catch (error) {
        console.error('Logout error:', error);
      } finally {
        // Clear state regardless of API call success
        this.clearAuthState();
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
      
      try {
        // Configure axios with token
        axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`;
        
        // Call an endpoint to validate the token
        const response = await axios.get(`${API_URL}/check`);
        
        if (response.status === 200) {
          // If we have user data in the response, update it
          if (response.data.username) {
            this.user = {
              id: response.data.userId,
              username: response.data.username,
              email: response.data.email
            };
          }
          this.isAuthenticated = true;
          return true;
        } else {
          this.clearAuthState();
          return false;
        }
      } catch (error) {
        this.clearAuthState();
        console.error('Authentication check failed:', error);
        return false;
      } finally {
        this.isLoading = false;
      }
    },
    
    setAuthHeader() {
      if (this.token) {
        axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`;
      } else {
        delete axios.defaults.headers.common['Authorization'];
      }
    },
    
    handleAuthSuccess(data: AuthResponse) {
      this.token = data.token;
      this.user = {
        id: data.userId,
        username: data.username
      };
      this.isAuthenticated = true;
      localStorage.setItem('token', data.token);
      this.setAuthHeader();
    },
    
    handleAuthError(error: AxiosError, defaultMessage: string) {
      const response = error.response as any;
      const errorMessage = response?.data?.message || error.message || defaultMessage;
      console.error(`Auth error: ${errorMessage}`, error);
      this.error = errorMessage;
      throw error;
    },
    
    clearAuthState() {
      this.user = null;
      this.token = null;
      this.isAuthenticated = false;
      localStorage.removeItem('token');
      delete axios.defaults.headers.common['Authorization'];
    }
  }
});
