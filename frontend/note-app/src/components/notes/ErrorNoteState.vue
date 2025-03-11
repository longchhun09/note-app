<template>
    <div class="error-container">
        <div class="error-icon">❌</div>
        <p class="error-message">{{ noteStore.error }}</p>
        <button @click="fetchNotes" class="retry-button">Retry</button>
    </div>
</template>
<script setup lang="ts">

const props = defineProps({
    noteStore: { type: Object }
});
const emit = defineEmits(['retry']);
function fetchNotes() {
    emit('retry')
}
</script>

<style lang="css">
.error-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 300px;
    background-color: #fff3f3;
    border-radius: 10px;
    padding: 20px;
}

.error-icon {
    font-size: 3rem;
    margin-bottom: 15px;
}

.error-message {
    color: #d9534f;
    margin-bottom: 20px;
    text-align: center;
}

.retry-button {
    background-color: #d9534f;
    color: white;
    border: none;
    padding: 10px 20px;
    border-radius: 5px;
    cursor: pointer;
    font-weight: bold;
}
</style>