import { defineStore } from 'pinia';
import axios, { AxiosError } from 'axios';
import type { Note, CreateNoteRequest, UpdateNoteRequest } from '@/types/Note';

// TODO: refactor static api 
const API_URL = 'http://localhost:5289/api/notes';

export const useNoteStore = defineStore('noteStore', {
  state: () => ({
    notes: [] as Note[],
    isLoading: false,
    error: null as string | null,
  }),
  
  actions: {
    async fetchNotes() {
      this.isLoading = true;
      this.error = null;
      
      try {
        const response = await axios.get(API_URL);
        this.notes = response.data;
      } catch (error) {
        const axiosError = error as AxiosError;
        const errorMessage = axiosError.response?.data?.message || axiosError.message || 'Failed to fetch notes';
        console.error('Error fetching notes:', errorMessage);
        this.error = errorMessage;
        throw error;
      } finally {
        this.isLoading = false;
      }
    },
    async createNote(note: CreateNoteRequest) {
      this.isLoading = true;
      this.error = null;
      
      try {
        const response = await axios.post(API_URL, note);
        this.notes.push(response.data);
        return response.data;
      } catch (error) {
        const axiosError = error as AxiosError;
        const errorMessage = axiosError.response?.data?.message || axiosError.message || 'Failed to create note';
        console.error('Error creating note:', errorMessage);
        this.error = errorMessage;
        throw error;
      } finally {
        this.isLoading = false;
      }
    },
    async updateNote(note: UpdateNoteRequest) {
      this.isLoading = true;
      this.error = null;
      
      try {
        const response = await axios.put(`${API_URL}/${note.id}`, note);
        const index = this.notes.findIndex(n => n.id === note.id);
        if (index !== -1) {
          this.notes[index] = response.data;
        }
        return response.data;
      } catch (error) {
        const axiosError = error as AxiosError;
        const errorMessage = axiosError.response?.data?.message || axiosError.message || 'Failed to update note';
        console.error('Error updating note:', errorMessage);
        this.error = errorMessage;
        throw error;
      } finally {
        this.isLoading = false;
      }
    },
    async deleteNote(id: number) {
      this.isLoading = true;
      this.error = null;
      
      try {
        await axios.delete(`${API_URL}/${id}`);
        this.notes = this.notes.filter(note => note.id !== id);
      } catch (error) {
        const axiosError = error as AxiosError;
        const errorMessage = axiosError.response?.data?.message || axiosError.message || 'Failed to delete note';
        console.error('Error deleting note:', errorMessage);
        this.error = errorMessage;
        throw error;
      } finally {
        this.isLoading = false;
      }
    },

    getNoteById(id: number): Note | undefined {
      return this.notes.find(note => note.id === id);
    }
  }
});

