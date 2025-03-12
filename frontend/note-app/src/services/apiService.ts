import axios, { type AxiosInstance, type AxiosRequestConfig, type AxiosResponse, type AxiosRequestHeaders } from 'axios';

export interface ApiServiceConfig {
  baseURL: string;
  timeout?: number;
  headers?: Record<string, string>;
}

export interface ApiResponse<T> {
  data: T;
  status: number;
  statusText: string;
  headers: Record<string, string>;
}

export class ApiService {
  private instance: AxiosInstance;
  private headers: Record<string, string> = {};

  constructor(config: ApiServiceConfig) {
    this.instance = axios.create({
      baseURL: config.baseURL,
      timeout: config.timeout || 30000,
      headers: config.headers || {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      }
    });

    this.setupInterceptors();
  }

  private setupInterceptors(): void {
    this.instance.interceptors.request.use(
      (config) => {
        config.headers = { ...config.headers, ...this.headers } as AxiosRequestHeaders;
        return config;
      },
      (error) => Promise.reject(error)
    );

    this.instance.interceptors.response.use(
      (response) => response,
      (error) => Promise.reject(error)
    );
  }

  public setHeader(key: string, value: string): void {
    this.headers[key] = value;
  }

  public removeHeader(key: string): void {
    delete this.headers[key];
  }

  public async get<T>(endpoint: string, params?: Record<string, any>, config?: AxiosRequestConfig): Promise<ApiResponse<T>> {
    try {
      const response = await this.instance.get<T>(endpoint, { 
        params, 
        ...config 
      });
      
      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
        headers: response.headers as Record<string, string>
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  public async post<T>(endpoint: string, data?: any, config?: AxiosRequestConfig): Promise<ApiResponse<T>> {
    try {
      const response = await this.instance.post<T>(endpoint, data, config);
      
      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
        headers: response.headers as Record<string, string>
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  public async put<T>(endpoint: string, data?: any, config?: AxiosRequestConfig): Promise<ApiResponse<T>> {
    try {
      const response = await this.instance.put<T>(endpoint, data, config);
      
      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
        headers: response.headers as Record<string, string>
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  public async delete<T>(endpoint: string, config?: AxiosRequestConfig): Promise<ApiResponse<T>> {
    try {
      const response = await this.instance.delete<T>(endpoint, config);
      
      return {
        data: response.data,
        status: response.status,
        statusText: response.statusText,
        headers: response.headers as Record<string, string>
      };
    } catch (error) {
      throw this.handleError(error);
    }
  }

  private handleError(error: any): Error {
    if (axios.isAxiosError(error)) {
      const serverError = error.response?.data;
      if (serverError) {
        return new Error(
          serverError.message || 'An unexpected error occurred'
        );
      }
      return new Error(error.message || 'Network error');
    }
    return error instanceof Error ? error : new Error('An unexpected error occurred');
  }
}

