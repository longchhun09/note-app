export interface Note {
  id: number;
  title: string;
  content: string | null;
  createdAt: string; // ISO string representation of DateTime
  updatedAt: string; // ISO string representation of DateTime
}

export interface CreateNoteRequest {
  title: string;
  content?: string | null;
}

export interface UpdateNoteRequest {
  id: number;
  title: string;
  content?: string | null;
}
