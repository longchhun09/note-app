import { ApiService } from './apiService';
import type { ApiResponse } from './apiService';
import type { LoginRequest, RegisterRequest, AuthResponse, User, AuthUser } from '../types/Auth';

export class AuthApi {
  private apiService: ApiService;
  private readonly AUTH_ENDPOINTS = {
    LOGIN: '/api/auth/login',
    REGISTER: '/api/auth/register',
    LOGOUT: '/api/auth/logout',
    CHECK: '/api/auth/check'
  };

  constructor(apiService: ApiService) {
    this.apiService = apiService;
  }

  async login(credentials: LoginRequest): Promise<ApiResponse<AuthResponse>> {
    return this.apiService.post<AuthResponse>(this.AUTH_ENDPOINTS.LOGIN, credentials);
  }

  async register(userData: RegisterRequest): Promise<ApiResponse<AuthResponse>> {
    return this.apiService.post<AuthResponse>(this.AUTH_ENDPOINTS.REGISTER, userData);
  }

  async logout(): Promise<ApiResponse<void>> {
    return this.apiService.post<void>(this.AUTH_ENDPOINTS.LOGOUT);
  }

  async checkAuth(): Promise<ApiResponse<AuthUser>> {
    return this.apiService.get<AuthUser>(this.AUTH_ENDPOINTS.CHECK);
  }

  setAuthToken(token: string): void {
    this.apiService.setHeader('Authorization', `Bearer ${token}`);
  }

  removeAuthToken(): void {
    this.apiService.removeHeader('Authorization');
  }
}

