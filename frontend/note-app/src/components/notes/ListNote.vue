<template>
    <div class="h-full flex flex-col overflow-hidden p-4">
        <div class="flex justify-between items-center mb-4">
            <h1 class="text-xl font-bold text-gray-800">My Notes</h1>
            <router-link 
                to="/notes/new" 
                class="bg-blue-500 hover:bg-blue-600 text-white font-medium px-3 py-1.5 rounded-md flex items-center justify-center transition-colors shadow-sm hover:shadow-md text-sm" 
                title="New Note"
            >
                <Plus class="mr-1 h-4 w-4" />
                <span>New Note</span>
            </router-link>
        </div>
        <LoadingState v-if="noteStore.isLoading" />
        <ErrorNoteState v-else-if="noteStore.error" :noteStore  />
        <EmptyState v-else-if="noteStore.notes.length === 0" />
        
        <!-- List -->
        <div v-else class="overflow-y-auto flex-grow">
            <router-link 
                v-for="note in noteStore.notes" 
                :key="note.id" 
                :to="`/notes/${note.id}`"
                custom
                v-slot="{ isActive, navigate }"
            >
                <div 
                    :class="[
                        'bg-white rounded-lg p-3 mb-3 border transition-all duration-200 cursor-pointer relative overflow-hidden',
                        isActive 
                            ? 'border-blue-500 bg-blue-50 shadow-md' 
                            : 'border-gray-200 hover:border-blue-300 hover:shadow'
                    ]"
                    @click="navigate"
                >
                    <h2 class="text-base font-semibold mb-2 text-gray-800 truncate">{{ note.title }}</h2>
                    
                    <p class="text-gray-600 mb-2 text-sm leading-5 h-[3em] overflow-hidden line-clamp-2">{{ getContentPreview(note.content) }}</p>
                    <div class="flex flex-col text-xs text-gray-400 mt-2">
                        <span class="mb-0.5">Updated: {{ formatDate(note.updatedAt) }}</span>
                    </div>
                    <div class="flex items-center mt-2">
                        <router-link
                            :to="`/notes/${note.id}?edit=true`" 
                            class="bg-transparent border-none text-xl cursor-pointer p-1.5 rounded-full flex items-center justify-center transition-colors hover:bg-gray-100" 
                            title="Edit Note"
                            @click.stop
                        >
                            <Edit class="h-4 w-4 text-gray-600" />
                        </router-link>
                        <button 
                            @click.stop="confirmDelete(note.id)" 
                            class="bg-transparent border-none text-xl cursor-pointer p-1.5 rounded-full flex items-center justify-center transition-colors hover:bg-red-50" 
                            title="Delete Note"
                        >
                            <Trash class="h-4 w-4 text-red-500" />
                        </button>
                    </div>
                </div>
            </router-link>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref, computed } from 'vue';
import { useRoute } from 'vue-router';
import { useNoteStore } from '@/stores/noteStore';
import { Plus, Edit, Trash } from 'lucide-vue-next';
import EmptyState from './EmptyState.vue';
import LoadingState from './LoadingState.vue';
import ErrorNoteState from './ErrorNoteState.vue';
import { formatDate } from '@/utils/dateFormatter.js';
const route = useRoute();
const noteStore = useNoteStore();
const noteToDelete = ref<number | null>(null);

const currentNoteId = computed(() => {
  const id = route.params.id;
  return id ? parseInt(id as string) : null;
});

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

