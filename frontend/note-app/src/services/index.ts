import { ApiService } from './apiService';
import { AuthApi } from './authApi';
import { NoteApi } from './noteApi';

const apiService = new ApiService({
  baseURL: 'http://localhost:5289'
});

const authApi = new AuthApi(apiService);
const noteApi = new NoteApi(apiService);

export { apiService, authApi, noteApi };

