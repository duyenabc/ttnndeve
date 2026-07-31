<template>
  <div class="min-h-screen bg-white text-slate-800 flex items-center justify-center p-6 font-sans">
    <div class="w-full max-w-[480px] flex flex-col items-center space-y-8">
      <!-- Logo -->
      <div>
        <img
          src="https://lh3.googleusercontent.com/aida-public/AB6AXuDD4bCjeLffk10Yr5FD077Ll-dPfjpg_tq-VaDHGVkxmLdd39X8yi12uSsFM0YezH0RVWZ9dGRG0jOnTzkLXisAqKj1GNNpRCyAcATBfIcVPIq0HVJpQvFwoPiZlrCiff-eRlLlOqcnf5HbALMFQYpfa3s1AhoTFH03sM-XuN64atNs1z1pHwB7nHvDkaCaB7Y1zCJs2_mfZY91mjiEbwbNtByiVcyZvQVLWFSavxUlSzkUsYFlVQ5C2jlqfE8KsyTk_wlVkwsuLlp2"
          alt="Logo DUE"
          class="h-28 object-contain"
        />
      </div>

      <!-- Header -->
      <div class="text-center space-y-2">
        <h1 class="text-3xl font-extrabold text-slate-900 tracking-tight">Quên mật khẩu?</h1>
        <p class="text-xs font-medium text-slate-500 max-w-sm mx-auto">Vui lòng nhập email đã đăng ký để nhận liên kết đặt lại mật khẩu.</p>
      </div>

      <!-- Dev Link Banner if any -->
      <div v-if="devLink" class="w-full p-4 bg-emerald-50 border border-emerald-200 rounded-2xl text-xs text-emerald-800 space-y-1 shadow-2xs">
        <p class="font-bold flex items-center gap-1">
          <span class="material-symbols-outlined text-[18px]">check_circle</span>
          Link đặt lại mật khẩu thử nghiệm:
        </p>
        <router-link :to="devLink" class="text-blue-600 underline font-mono break-all font-bold">{{ devLink }}</router-link>
      </div>

      <!-- Success State Banner -->
      <div v-if="isSubmitted" class="w-full text-center space-y-4 bg-emerald-50/70 p-6 rounded-2xl border border-emerald-200 shadow-2xs">
        <div class="w-16 h-16 bg-emerald-100 rounded-full flex items-center justify-center mx-auto text-emerald-600">
          <span class="material-symbols-outlined text-4xl">check_circle</span>
        </div>
        <h3 class="text-xl font-extrabold text-slate-900">Kiểm tra Email</h3>
        <p class="text-xs text-slate-600 leading-relaxed font-medium">
          Chúng tôi đã gửi một liên kết đặt lại mật khẩu đến địa chỉ <strong class="text-slate-900 font-bold">{{ email }}</strong>.
          Vui lòng kiểm tra cả hòm thư rác nếu không tìm thấy.
        </p>
        <button @click="isSubmitted = false" class="w-full py-3 bg-[#005EA3] hover:bg-blue-800 text-white font-bold text-xs rounded-xl shadow-md transition">
          Gửi lại yêu cầu
        </button>
      </div>

      <!-- Form Area -->
      <form v-else @submit.prevent="handleSubmit" class="w-full space-y-6">
        <div class="space-y-1.5">
          <label class="block text-xs font-bold text-slate-700 uppercase tracking-wider">EMAIL ĐĂNG KÝ</label>
          <div class="relative">
            <span class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 text-[20px]">mail</span>
            <input
              v-model="email"
              type="email"
              required
              placeholder="username@example.edu.vn"
              class="w-full pl-12 pr-4 py-3.5 bg-slate-50 border border-slate-200 rounded-xl text-sm font-medium text-slate-900 outline-none focus:ring-2 focus:ring-[#005EA3] focus:bg-white transition"
            />
          </div>
        </div>

        <button
          type="submit"
          :disabled="loading"
          class="w-full py-4 bg-[#005EA3] hover:bg-blue-800 text-white font-bold text-sm rounded-xl shadow-md transition active:scale-[0.99] flex items-center justify-center gap-2 disabled:opacity-50"
        >
          <span v-if="loading" class="material-symbols-outlined animate-spin text-[20px]">progress_activity</span>
          <span>{{ loading ? 'Đang xử lý...' : 'Gửi yêu cầu' }}</span>
        </button>

        <div class="text-center pt-2">
          <router-link to="/login" class="text-xs font-bold text-[#005EA3] hover:underline flex items-center justify-center gap-1">
            <span class="material-symbols-outlined text-[16px]">arrow_back</span>
            Quay lại trang Đăng nhập
          </router-link>
        </div>
      </form>

      <!-- Technical Support Footer -->
      <footer class="text-center text-[11px] text-slate-400 leading-relaxed pt-6 border-t border-slate-100 w-full">
        Để được trợ giúp, hãy liên hệ với bộ phận hỗ trợ kỹ thuật của Trung tâm Số và Học liệu:
        <a href="https://www.facebook.com/CenterITC" target="_blank" class="text-[#005EA3] hover:underline font-bold block mt-0.5">https://www.facebook.com/CenterITC</a>
      </footer>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import api from '@/api/api';

const email = ref('sv001@ims.edu.vn');
const devLink = ref('');
const loading = ref(false);
const isSubmitted = ref(false);

async function handleSubmit() {
  loading.value = true;
  try {
    const res = await api.post('/auth/forgot-password', { email: email.value });
    devLink.value = res.data.devResetLink || '';
    isSubmitted.value = true;
  } catch (err) {
    alert(err.response?.data?.message || 'Có lỗi xảy ra khi gửi yêu cầu khôi phục mật khẩu.');
  } finally {
    loading.value = false;
  }
}
</script>
