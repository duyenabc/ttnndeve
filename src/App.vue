<template>
  <div id="app" class="min-h-screen bg-slate-50 text-slate-900 font-sans antialiased">
    <router-view />
  </div>
</template>

<script setup>
import { onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = useRouter();
const authStore = useAuthStore();

function handleAuthLogout() {
  authStore.logout();
  router.push('/login');
}

onMounted(() => {
  window.addEventListener('auth-logout', handleAuthLogout);
});

onUnmounted(() => {
  window.removeEventListener('auth-logout', handleAuthLogout);
});
</script>

<style>
@import './style.css';
</style>
