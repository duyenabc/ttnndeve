<template>
  <div class="min-h-screen bg-white text-slate-800 flex flex-col md:flex-row font-sans">
    <!-- Left Hero Section (30% width) -->
    <section class="hidden lg:block w-[30%] relative overflow-hidden bg-slate-900 shrink-0 min-h-screen">
      <img
        src="https://lh3.googleusercontent.com/aida-public/AB6AXuCZRkOs9Q3Dw3ZZBTmD5yzPfsGX_mB5RFGXg6CYzXTQaGFmHrEevcp5Ej2b0wTIM4RPFJI5HHYGFA4wvrU9ZZycMHAIv-Vr3PZHSVEM9aZ694V_B1Ztevu_KlxPeQCq4ycD-vXigkKjNseXDknNuMISayvDjVf13d5dU2SpkUYab9kdcWyy8AMMVMnb9-ldcSq0DBRWhJkxanTB8TLyNsqH0u4f3GHltk02LFsWTjd1U-DkraSv0wTYtFq_G26c4puhx7yKcSAaBRfr3p0"
        alt="University of Economics Da Nang"
        class="absolute inset-0 w-full h-full object-cover"
      />
      <div class="absolute inset-0 bg-blue-950/40 backdrop-blur-[1px]"></div>

      <div class="absolute bottom-0 left-0 right-0 bg-[#003362]/90 p-8 text-white backdrop-blur-md border-t border-white/10 space-y-2">
        <h1 class="font-extrabold text-2xl tracking-tight">Chào mừng đến với IMS</h1>
        <p class="text-xs text-blue-100 leading-relaxed">
          Hệ thống Quản lý Thực tập số, nơi kết nối, theo dõi tiến trình và tối ưu hóa chu kỳ thực tập của bạn.
        </p>
      </div>
    </section>

    <!-- Right Activation Section (70% width) -->
    <main class="flex-1 bg-white flex flex-col items-center justify-between p-6 md:p-12 overflow-y-auto min-h-screen">
      <div class="w-full max-w-md my-auto space-y-8">
        <!-- Logo -->
        <div class="flex justify-center">
          <img
            src="https://lh3.googleusercontent.com/aida-public/AB6AXuBY-V-ETBnj2vtnqINCtqrAlkJPXTnMj8jvTwDdeKgx19ulKNTwGOaoijasxY_lIysvwowlvCSWkbEOvtaXe3IxYuVftunlj8Td9c2fVetw3R8TJqtM9zSD0HNGk9FxaP-vl50Uz0A5QMoCH9NVg8FYqySnOI6ojbC3DeMuzc-vaOK2nloqPZF6au9Gd4xC8NvgGECStxGUiInrUwDZD7CEAvzc3A0bJofi0KNpr0LI-oJsY974iYQcRi9xkYqb5GZS0pao11fyUCxu"
            alt="DUE IMS Logo"
            class="h-28 object-contain"
          />
        </div>

        <!-- Header -->
        <div class="text-center space-y-2">
          <h2 class="text-3xl font-extrabold text-slate-900 tracking-tight">Kích hoạt tài khoản</h2>
          <p class="text-xs text-slate-500 font-medium">Chào mừng bạn đến với IMS! Hãy thiết lập mật khẩu mới để bắt đầu.</p>
        </div>

        <!-- Activation Form -->
        <form @submit.prevent="handleActivate" class="space-y-6">
          <!-- Password Input -->
          <div class="space-y-2">
            <label class="block text-xs font-bold text-slate-700 uppercase tracking-wider">Mật khẩu mới</label>
            <div class="relative">
              <span class="material-symbols-outlined absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400 text-[20px]">lock</span>
              <input
                v-model="password"
                :type="showPassword ? 'text' : 'password'"
                required
                placeholder="Nhập mật khẩu mới"
                class="w-full pl-11 pr-11 py-3.5 bg-slate-50 border border-slate-200 rounded-xl text-sm font-medium text-slate-900 outline-none focus:ring-2 focus:ring-[#005EA3] focus:bg-white transition"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 p-1"
              >
                <span class="material-symbols-outlined text-[20px]">{{ showPassword ? 'visibility_off' : 'visibility' }}</span>
              </button>
            </div>

            <!-- Password Strength Bar & Checklist -->
            <div class="pt-2 space-y-3">
              <div class="flex justify-between items-center text-[10px] font-bold">
                <div class="flex gap-1 flex-1 max-w-[200px]">
                  <div class="h-1.5 flex-1 rounded-full" :class="password.length >= 8 ? 'bg-[#005EA3]' : 'bg-slate-200'"></div>
                  <div class="h-1.5 flex-1 rounded-full" :class="hasUppercase && hasLowercase ? 'bg-[#005EA3]' : 'bg-slate-200'"></div>
                  <div class="h-1.5 flex-1 rounded-full" :class="hasNumber ? 'bg-[#005EA3]' : 'bg-slate-200'"></div>
                  <div class="h-1.5 flex-1 rounded-full" :class="hasSpecial ? 'bg-[#005EA3]' : 'bg-slate-200'"></div>
                </div>
                <span class="uppercase font-bold" :class="isStrong ? 'text-emerald-700' : 'text-slate-500'">
                  {{ isStrong ? 'Rất mạnh' : 'Trung bình' }}
                </span>
              </div>

              <!-- Checklist -->
              <div class="grid grid-cols-2 gap-2 text-[11px]">
                <div class="flex items-center gap-1.5" :class="password.length >= 8 ? 'text-emerald-700 font-bold' : 'text-slate-400'">
                  <span class="material-symbols-outlined text-[14px]">{{ password.length >= 8 ? 'check_circle' : 'radio_button_unchecked' }}</span>
                  <span>Tối thiểu 8 ký tự</span>
                </div>
                <div class="flex items-center gap-1.5" :class="hasUppercase ? 'text-emerald-700 font-bold' : 'text-slate-400'">
                  <span class="material-symbols-outlined text-[14px]">{{ hasUppercase ? 'check_circle' : 'radio_button_unchecked' }}</span>
                  <span>Chữ cái viết hoa</span>
                </div>
                <div class="flex items-center gap-1.5" :class="hasLowercase ? 'text-emerald-700 font-bold' : 'text-slate-400'">
                  <span class="material-symbols-outlined text-[14px]">{{ hasLowercase ? 'check_circle' : 'radio_button_unchecked' }}</span>
                  <span>Chữ cái viết thường</span>
                </div>
                <div class="flex items-center gap-1.5" :class="hasNumber ? 'text-emerald-700 font-bold' : 'text-slate-400'">
                  <span class="material-symbols-outlined text-[14px]">{{ hasNumber ? 'check_circle' : 'radio_button_unchecked' }}</span>
                  <span>Bao gồm chữ số</span>
                </div>
                <div class="flex items-center gap-1.5 col-span-2" :class="hasSpecial ? 'text-emerald-700 font-bold' : 'text-slate-400'">
                  <span class="material-symbols-outlined text-[14px]">{{ hasSpecial ? 'check_circle' : 'radio_button_unchecked' }}</span>
                  <span>Ký tự đặc biệt (@, #, $...)</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Confirm Password Field -->
          <div class="space-y-1.5">
            <label class="block text-xs font-bold text-slate-700 uppercase tracking-wider">Xác nhận mật khẩu</label>
            <div class="relative">
              <span class="material-symbols-outlined absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400 text-[20px]">lock_reset</span>
              <input
                v-model="confirmPassword"
                :type="showConfirmPassword ? 'text' : 'password'"
                required
                placeholder="Nhập lại mật khẩu mới"
                class="w-full pl-11 pr-11 py-3.5 bg-slate-50 border border-slate-200 rounded-xl text-sm font-medium text-slate-900 outline-none focus:ring-2 focus:ring-[#005EA3] focus:bg-white transition"
              />
              <button
                type="button"
                @click="showConfirmPassword = !showConfirmPassword"
                class="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 p-1"
              >
                <span class="material-symbols-outlined text-[20px]">{{ showConfirmPassword ? 'visibility_off' : 'visibility' }}</span>
              </button>
            </div>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="loading"
            class="w-full py-4 bg-[#005EA3] hover:bg-blue-800 text-white font-extrabold text-sm rounded-xl shadow-md transition active:scale-[0.99] uppercase tracking-wider flex items-center justify-center gap-2 disabled:opacity-50"
          >
            <span v-if="loading" class="material-symbols-outlined animate-spin text-[20px]">progress_activity</span>
            <span>{{ loading ? 'Đang xử lý...' : 'Kích hoạt tài khoản' }}</span>
          </button>
        </form>

        <div class="text-center">
          <router-link to="/login" class="text-xs font-bold text-[#005EA3] hover:underline">
            Quay lại trang Đăng nhập
          </router-link>
        </div>
      </div>

      <!-- Footer Help -->
      <footer class="mt-8 pt-6 border-t border-slate-100 text-center text-[11px] text-slate-400 max-w-sm mx-auto leading-relaxed">
        Gặp sự cố? Liên hệ <strong class="text-slate-700">Quản trị viên</strong> hoặc <strong class="text-slate-700">Trung tâm Số và Học liệu</strong> để được hỗ trợ kỹ thuật.
      </footer>
    </main>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';
import api from '@/api/api';
import { useAuthStore } from '@/stores/auth';

const router = useRouter();
const authStore = useAuthStore();

const password = ref('');
const confirmPassword = ref('');
const showPassword = ref(false);
const showConfirmPassword = ref(false);
const loading = ref(false);

const hasUppercase = computed(() => /[A-Z]/.test(password.value));
const hasLowercase = computed(() => /[a-z]/.test(password.value));
const hasNumber = computed(() => /[0-9]/.test(password.value));
const hasSpecial = computed(() => /[^A-Za-z0-9]/.test(password.value));
const isStrong = computed(() => password.value.length >= 8 && hasUppercase.value && hasNumber.value);

async function handleActivate() {
  if (password.value !== confirmPassword.value) {
    alert('Mật khẩu xác nhận không khớp');
    return;
  }
  loading.value = true;
  try {
    const res = await api.post('/auth/activate', { matKhauMoi: password.value });
    authStore.setAuth(res.data);
    alert('Kích hoạt tài khoản thành công!');
    router.push('/dashboard');
  } catch (err) {
    alert(err.response?.data?.message || 'Có lỗi xảy ra khi kích hoạt tài khoản.');
  } finally {
    loading.value = false;
  }
}
</script>
