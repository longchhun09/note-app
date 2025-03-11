<template>
    <div class="notes-list-container">
        <h1 class="page-title">My Notes</h1>
        <LoadingState v-if="noteStore.isLoading" />
        <ErrorNoteState v-else-if="noteStore.error" :noteStore  />
        <EmptyState v-else-if="noteStore.notes.length === 0" />

        <!-- List -->
        <div v-else class="notes-grid">
            <div v-for="note in noteStore.notes" :key="note.id" class="note-card" @click="viewNote(note.id)">
                <h2 class="note-title">{{ note.title }}</h2>
                <p class="note-preview">{{ getContentPreview(note.content) }}</p>
                <div class="note-dates">
                    <span class="created-date">Created: {{ formatDate(note.createdAt) }}</span>
                    <span class="updated-date">Updated: {{ formatDate(note.updatedAt) }}</span>
                </div>
                <div class="note-actions">
                    <button @click.stop="editNote(note.id)" class="edit-button" title="Edit Note">
                        ✏️
                    </button>
                    <button @click.stop="confirmDelete(note.id)" class="delete-button" title="Delete Note">
                        🗑️
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useNoteStore } from '@/stores/noteStore';
import EmptyState from './EmptyState.vue';
import LoadingState from './LoadingState.vue';
import ErrorNoteState from './ErrorNoteState.vue';

const router = useRouter();
const noteStore = useNoteStore();
const showDeleteConfirm = ref(false);
const noteToDelete = ref<number | null>(null);

onMounted(() => {
    fetchNotes();
});

async function fetchNotes() {
    try {
        await noteStore.fetchNotes();
    } catch (error) {
        console.error('Failed to fetch notes:', error);
    }
}

function getContentPreview(content: string | null): string {
    if (!content) return 'No content';
    return content.length > 100 ? content.substring(0, 100) + '...' : content;
}

function formatDate(dateString: string): string {
    const date = new Date(dateString);
    return new Intl.DateTimeFormat('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    }).format(date);
}

function viewNote(id: number) {
    router.push(`/note/${id}`);
}

function editNote(id: number) {
    router.push(`/edit/${id}`);
}

function confirmDelete(id: number) {
    if (confirm('Are you sure you want to delete this note?')) {
        deleteNote(id);
    }
}

async function deleteNote(id: number) {
    try {
        await noteStore.deleteNote(id);
    } catch (error) {
        console.error('Failed to delete note:', error);
    }
}
</script>

<style scoped>
.notes-list-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
}

.page-title {
    font-size: 2rem;
    margin-bottom: 30px;
    color: #333;
}

.create-button {
    background-color: #5cb85c;
    color: white;
    border: none;
    padding: 10px 20px;
    border-radius: 5px;
    cursor: pointer;
    font-weight: bold;
    text-decoration: none;
    margin-top: 15px;
}

/* Notes Grid */
.notes-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 20px;
    margin-top: 20px;
}

.note-card {
    background-color: white;
    border-radius: 8px;
    padding: 20px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s, box-shadow 0.2s;
    cursor: pointer;
    position: relative;
    overflow: hidden;
}

.note-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.15);
}

.note-title {
    font-size: 1.5rem;
    margin-bottom: 10px;
    color: #333;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.note-preview {
    color: #666;
    margin-bottom: 15px;
    font-size: 0.95rem;
    line-height: 1.5;
    height: 4.5em;
    overflow: hidden;
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
}

.note-dates {
    display: flex;
    flex-direction: column;
    font-size: 0.8rem;
    color: #888;
    margin-top: 15px;
}

.created-date,
.updated-date {
    margin-bottom: 5px;
}

.note-actions {
    position: absolute;
    top: 10px;
    right: 10px;
    display: flex;
    gap: 5px;
    opacity: 0;
    transition: opacity 0.2s;
}

.note-card:hover .note-actions {
    opacity: 1;
}

.edit-button,
.delete-button {
    background-color: transparent;
    border: none;
    font-size: 1.2rem;
    cursor: pointer;
    padding: 5px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: background-color 0.2s;
}

.edit-button:hover {
    background-color: rgba(0, 0, 0, 0.05);
}

.delete-button:hover {
    background-color: rgba(217, 83, 79, 0.1);
}

/* Responsive adjustments */
@media (max-width: 768px) {
    .notes-grid {
        grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
    }
}

@media (max-width: 480px) {
    .notes-grid {
        grid-template-columns: 1fr;
    }
}
</style>