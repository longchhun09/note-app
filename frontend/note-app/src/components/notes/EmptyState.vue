<template>
    <div class="empty-container">
        <div class="empty-icon">📝</div>
        <p>You don't have any notes yet.</p>
        <router-link to="/create" class="create-button">Create your first note</router-link>
    </div>
</template>
<script setup lang="ts">
import { useRouter } from 'vue-router';
const router = useRouter();

</script>
<style scoped="scss">
.empty-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 300px;
    background-color: #f9f9f9;
    border-radius: 10px;
    padding: 20px;
}

.empty-icon {
    font-size: 3rem;
    margin-bottom: 15px;
}
</style>