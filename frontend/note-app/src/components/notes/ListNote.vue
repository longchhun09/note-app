<template>
    <div class="h-full flex flex-col overflow-hidden p-4">
        <div class="flex justify-between items-center mb-4">
            <h1 class="text-xl font-bold text-gray-800">My Notes</h1>
            <Button text="New Note" @click="navigateToNewNote" />
        </div>
        
        <div class="flex flex-col mb-4 space-y-2">
            <SearchBar v-model="searchTerm" />
            <SortControls 
                v-model:sortField="sortField" 
                v-model:sortOrder="sortOrder" 
                @sort-change="fetchNotes" 
            />
        </div>
        
        <LoadingState v-if="noteStore.isLoading" />
        <ErrorNoteState v-else-if="noteStore.error" :noteStore />
        <EmptyState v-else-if="noteStore.notes.length === 0" />
        
        <div v-else-if="filteredAndSortedNotes.length === 0" class="flex flex-col items-center justify-center py-8 text-center text-gray-500">
            <Search class="h-10 w-10 mb-2" />
            <p>No notes found matching your search.</p>
        </div>

        <!-- List -->
        <div v-else class="overflow-y-auto flex-grow">
            <router-link v-for="note in filteredAndSortedNotes" :key="note.id" :to="`/notes/${note.id}`" custom
                v-slot="{ isActive, navigate }">
                <div :class="[
                    'bg-white rounded-lg p-3 mb-3 border transition-all duration-200 cursor-pointer relative overflow-hidden',
                    isActive
                        ? 'border-blue-500 bg-blue-50 shadow-md'
                        : 'border-gray-200 hover:border-blue-300 hover:shadow'
                ]" @click="navigate">
                    <h2 class="text-base font-semibold mb-2 text-gray-800 truncate">{{ note.title }}</h2>

                    <p class="text-gray-600 mb-2 text-sm leading-5 h-[3em] overflow-hidden line-clamp-2">{{
                        getContentPreview(note.content) }}</p>
                    <div class="flex flex-col text-xs text-gray-400 mt-2">
                        <span class="mb-0.5">Updated: {{ formatDate(note.updatedAt) }}</span>
                    </div>
                    <div class="flex items-center mt-2">
                        <router-link :to="`/notes/${note.id}?edit=true`"
                            class="bg-transparent border-none text-xl cursor-pointer p-1.5 rounded-full flex items-center justify-center transition-colors hover:bg-gray-100"
                            title="Edit Note" @click.stop>
                            <Edit class="h-4 w-4 text-gray-600" />
                        </router-link>
                        <button @click.stop="showDeleteConfirmation(note.id)"
                            class="bg-transparent border-none text-xl cursor-pointer p-1.5 rounded-full flex items-center justify-center transition-colors hover:bg-red-50"
                            title="Delete Note">
                            <Trash class="h-4 w-4 text-red-500" />
                        </button>
                    </div>
                </div>
            </router-link>
        </div>
        <ConfirmationDialog v-model="showDeleteConfirm" @confirm="confirmDelete" @cancel="showDeleteConfirm = false"
            icon="Trash" button-variant="danger" />
    </div>
</template>
<script setup lang="ts">
import { onMounted, ref, computed, watch } from 'vue';
import { useNoteStore } from '@/stores/noteStore';
import { Edit, Trash, Search } from 'lucide-vue-next';
import LoadingState from './LoadingState.vue';
import ErrorNoteState from './ErrorNoteState.vue';
import EmptyState from './EmptyState.vue';
import { formatDate } from '@/utils/dateFormatter.ts';
import Button from '@/components/common/Button.vue';
import { useNoteNavigation } from '@/utils/noteNavigation';
import ConfirmationDialog from '@/components/common/ConfirmationDialog.vue';
import SearchBar from './SearchBar.vue';
import SortControls from './SortControls.vue';

const noteStore = useNoteStore();
const { navigateToNewNote } = useNoteNavigation();
const showDeleteConfirm = ref(false);
const idToDelete = ref(0);

const searchTerm = ref('');
const sortField = ref<'title' | 'updatedAt'>('updatedAt');
const sortOrder = ref<'asc' | 'desc'>('desc');

const filteredAndSortedNotes = computed(() => {
    return noteStore.notes;
});

onMounted(() => {
    fetchNotes();
});

watch(searchTerm, () => {
    fetchNotes();
});

function fetchNotes() {
    noteStore.fetchFilteredNotes(searchTerm.value, sortField.value, sortOrder.value).then();
}

function getContentPreview(content: string | null): string {
    if (!content) return 'No content';
    return content.length > 100 ? content.substring(0, 100) + '...' : content;
}

function showDeleteConfirmation(id: number) {
    idToDelete.value = id;
    showDeleteConfirm.value = true;
}
function confirmDelete() {
    deleteNote(idToDelete.value);
    showDeleteConfirm.value = false;
}

function deleteNote(id: number) {
    noteStore.deleteNote(id).then();
}
</script>
