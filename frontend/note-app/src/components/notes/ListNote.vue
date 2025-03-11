<template>
    <div class="max-w-7xl mx-auto p-5">
        <h1 class="text-3xl font-bold mb-8 text-gray-800">My Notes</h1>
        <LoadingState v-if="noteStore.isLoading" />
        <ErrorNoteState v-else-if="noteStore.error" :noteStore  />
        <EmptyState v-else-if="noteStore.notes.length === 0" />

        <!-- List -->
        <div v-else class="grid grid-cols-1 sm:grid-cols-2 gap-5 mt-5">
            <div 
                v-for="note in noteStore.notes" 
                :key="note.id" 
                class="bg-white rounded-lg p-5 shadow-md hover:shadow-xl hover:-translate-y-1 transition-all duration-200 cursor-pointer relative overflow-hidden" 
                @click="viewNote(note.id)"
            >
                <h2 class="text-xl font-semibold mb-2.5 text-gray-800 truncate">{{ note.title }}</h2>
                <p class="text-gray-600 mb-4 text-[0.95rem] leading-6 h-[4.5em] overflow-hidden line-clamp-3">{{ getContentPreview(note.content) }}</p>
                <div class="flex flex-col text-xs text-gray-400 mt-4">
                    <span class="mb-1">Created: {{ formatDate(note.createdAt) }}</span>
                    <span>Updated: {{ formatDate(note.updatedAt) }}</span>
                </div>
                <div class="absolute top-2.5 right-2.5 flex gap-1.5 opacity-0 transition-opacity duration-200 group-hover:opacity-100">
                    <button 
                        @click.stop="editNote(note.id)" 
                        class="bg-transparent border-none text-xl cursor-pointer p-1.5 rounded-full flex items-center justify-center transition-colors hover:bg-gray-100" 
                        title="Edit Note"
                    >
                        ✏️
                    </button>
                    <button 
                        @click.stop="confirmDelete(note.id)" 
                        class="bg-transparent border-none text-xl cursor-pointer p-1.5 rounded-full flex items-center justify-center transition-colors hover:bg-red-50" 
                        title="Delete Note"
                    >
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

