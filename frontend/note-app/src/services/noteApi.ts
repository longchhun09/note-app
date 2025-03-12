import { ApiService } from './apiService';
import type { ApiResponse } from './apiService';

import type { Note, CreateNoteRequest, UpdateNoteRequest } from '@/types/Note';

export class NoteApi {
  private apiService: ApiService;
  private endpoint = '/api/notes';

  constructor(apiService: ApiService) {
    this.apiService = apiService;
  }

  public async getAllNotes(): Promise<ApiResponse<Note[]>> {
    return this.apiService.get<Note[]>(this.endpoint);
  }

  public async getNoteById(id: number): Promise<ApiResponse<Note>> {
    return this.apiService.get<Note>(`${this.endpoint}/${id}`);
  }

  public async createNote(note: CreateNoteRequest): Promise<ApiResponse<Note>> {
    return this.apiService.post<Note>(this.endpoint, note);
  }

  public async updateNote(note: UpdateNoteRequest): Promise<ApiResponse<Note>> {
    return this.apiService.put<Note>(`${this.endpoint}/${note.id}`, note);
  }

  public async deleteNote(id: number): Promise<ApiResponse<void>> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}

