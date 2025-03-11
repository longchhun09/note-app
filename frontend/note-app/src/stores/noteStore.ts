import { defineStore } from 'pinia';
import axios from 'axios';
import type { Note } from '@/types/Note';

// TODO: refactor static api 
const API_URL = 'http://localhost:5289/api/notes';

export const useNoteStore = defineStore('note', {
  state: () => ({
    notes: [] as Note[]
  }),

  actions: {
    async fetchNotes() {
      try {
        const response = await axios.get(API_URL);
        this.notes = response.data;
      } catch (error) {
        console.error('Error fetching notes:', error);
        throw error;
      }
    },

    async createNote(note: Omit<Note, 'id' | 'createdAt' | 'updatedAt'>) {
      try {
        const response = await axios.post(API_URL, note);
        this.notes.push(response.data);
        return response.data;
      } catch (error) {
        console.error('Error creating note:', error);
        throw error;
      }
    },

    async updateNote(note: Note) {
      try {
        const response = await axios.put(`${API_URL}/${note.id}`, note);
        const index = this.notes.findIndex(n => n.id === note.id);
        if (index !== -1) {
          this.notes[index] = response.data;
        }
        return response.data;
      } catch (error) {
        console.error('Error updating note:', error);
        throw error;
      }
    },

    async deleteNote(id: string) {
      try {
        await axios.delete(`${API_URL}/${id}`);
        this.notes = this.notes.filter(note => note.id !== id);
      } catch (error) {
        console.error('Error deleting note:', error);
        throw error;
      }
    }
  }
});

