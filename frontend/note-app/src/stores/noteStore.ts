import { defineStore } from 'pinia';
import type { Note, CreateNoteRequest, UpdateNoteRequest } from '@/types/Note';
import { noteApi } from '../services';

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
        const response = await noteApi.getAllNotes();
        this.notes = response.data;
      } catch (error: any) {
        const errorMessage = error.message || 'Failed to fetch notes';
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
        const response = await noteApi.createNote(note);
        this.notes.push(response.data);
        return response.data;
      } catch (error: any) {
        const errorMessage = error.message || 'Failed to create note';
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
        await noteApi.updateNote(note);
        const index = this.notes.findIndex(n => n.id === note.id);        
        if (index !== -1) {
          const existingNote = this.notes[index];
          const updatedNote: Note = {
            ...existingNote,
            title: note.title,
            content: note.content ?? existingNote.content,
            updatedAt: new Date().toISOString()
          };
          
          this.notes[index] = updatedNote;
          
          return updatedNote;
        }
        
      } catch (error: any) {
        const errorMessage = error.message || 'Failed to update note';
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
        await noteApi.deleteNote(id);
        this.notes = this.notes.filter(note => note.id !== id);
      } catch (error: any) {
        const errorMessage = error.message || 'Failed to delete note';
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

