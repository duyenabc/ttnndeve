<template>
  <div class="min-h-screen bg-white text-slate-800 flex flex-col lg:flex-row font-sans">
    <AuthHero />

    <main class="flex-1 bg-white flex flex-col items-center justify-between p-6 md:p-12 overflow-y-auto min-h-screen">
      <div class="w-full max-w-md my-auto space-y-7">
        <AuthBrand />

        <div class="text-center space-y-2">
          <h2 class="text-3xl font-extrabold text-slate-900 tracking-tight">Kích hoạt tài khoản</h2>
          <p class="text-sm text-slate-500 font-medium leading-relaxed">
            Chào mừng {{ displayName }} đến với IMS! Hãy đặt mật khẩu để bắt đầu.
          </p>
        </div>

        <form @submit.prevent="handleActivate" class="space-y-6">
          <div class="space-y-2">
            <label class="block text-[11px] font-bold text-slate-500 uppercase tracking-wider">
              Mật khẩu mới
            </label>
            <div class="relative">
              <span class="material-symbols-outlined absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400 text-[20px]">
                lock
              </span>
              <input
                v-model="password"
                :type="showPassword ? 'text' : 'password'"
                required
                placeholder="Nhập mật khẩu mới"
                class="w-full pl-11 pr-11 py-3.5 bg-slate-50 border border-slate-200 rounded-lg text-sm font-medium text-slate-900 outline-none focus:ring-2 focus:ring-[#005EA3] focus:bg-white transition"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 p-1"
              >
                <span class="material-symbols-outlined text-[20px]">
                  {{ showPassword ? 'visibility_off' : 'visibility' }}
                </span>
              </button>
            </div>

            <div class="pt-2 space-y-3">
              <div class="flex justify-between items-center gap-3 text-[10px] font-bold">
                <div class="flex gap-1 flex-1 max-w-[220px]">
                  <div
                    class="h-1.5 flex-1 rounded-full transition-colors"
                    :class="strengthScore >= 1 ? 'bg-[#005EA3]' : 'bg-slate-200'"
                  ></div>
                  <div
                    class="h-1.5 flex-1 rounded-full transition-colors"
                    :class="strengthScore >= 2 ? 'bg-[#005EA3]' : 'bg-slate-200'"
                  ></div>
                  <div
                    class="h-1.5 flex-1 rounded-full transition-colors"
                    :class="strengthScore >= 3 ? 'bg-[#005EA3]' : 'bg-slate-200'"
                  ></div>
                  <div
                    class="h-1.5 flex-1 rounded-full transition-colors"
                    :class="strengthScore >= 4 ? 'bg-[#005EA3]' : 'bg-slate-200'"
                  ></div>
                </div>
                <span
                  class="uppercase tracking-wide"
                  :class="strengthScore >= 4 ? 'text-emerald-700' : strengthScore >= 2 ? 'text-[#005EA3]' : 'text-slate-400'"
                >
                  {{ strengthLabel }}
                </span>
              </div>

              <div class="grid grid-cols-2 gap-x-3 gap-y-2 text-[11px]">
                <div
                  class="flex items-center gap-1.5"
                  :class="password.length >= 8 ? 'text-emerald-700 font-semibold' : 'text-slate-400'"
                >
                  <span class="material-symbols-outlined text-[14px]">
                    {{ password.length >= 8 ? 'check_circle' : 'radio_button_unchecked' }}
                  </span>
                  <span>Tối thiểu 8 ký tự</span>
                </div>
                <div
                  class="flex items-center gap-1.5"
                  :class="hasNumber ? 'text-emerald-700 font-semibold' : 'text-slate-400'"
                >
                  <span class="material-symbols-outlined text-[14px]">
                    {{ hasNumber ? 'check_circle' : 'radio_button_unchecked' }}
                  </span>
                  <span>Bao gồm chữ số</span>
                </div>
                <div
                  class="flex items-center gap-1.5"
                  :class="hasUppercase ? 'text-emerald-700 font-semibold' : 'text-slate-400'"
                >
                  <span class="material-symbols-outlined text-[14px]">
                    {{ hasUppercase ? 'check_circle' : 'radio_button_unchecked' }}
                  </span>
                  <span>Chữ cái viết hoa</span>
                </div>
                <div
                  class="flex items-center gap-1.5"
                  :class="hasSpecial ? 'text-emerald-700 font-semibold' : 'text-slate-400'"
                >
                  <span class="material-symbols-outlined text-[14px]">
                    {{ hasSpecial ? 'check_circle' : 'radio_button_unchecked' }}
                  </span>
                  <span>Ký tự đặc biệt (@, #, $...)</span>
                </div>
                <div
                  class="flex items-center gap-1.5"
                  :class="hasLowercase ? 'text-emerald-700 font-semibold' : 'text-slate-400'"
                >
                  <span class="material-symbols-outlined text-[14px]">
                    {{ hasLowercase ? 'check_circle' : 'radio_button_unchecked' }}
                  </span>
                  <span>Chữ cái viết thường</span>
                </div>
              </div>
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block text-[11px] font-bold text-slate-500 uppercase tracking-wider">
              Xác nhận mật khẩu
            </label>
            <div class="relative">
              <span class="material-symbols-outlined absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400 text-[20px]">
                lock_reset
              </span>
              <input
                v-model="confirmPassword"
                :type="showConfirmPassword ? 'text' : 'password'"
                required
                placeholder="Nhập lại mật khẩu mới"
                class="w-full pl-11 pr-11 py-3.5 bg-slate-50 border border-slate-200 rounded-lg text-sm font-medium text-slate-900 outline-none focus:ring-2 focus:ring-[#005EA3] focus:bg-white transition"
              />
              <button
                type="button"
                @click="showConfirmPassword = !showConfirmPassword"
                class="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 p-1"
              >
                <span class="material-symbols-outlined text-[20px]">
                  {{ showConfirmPassword ? 'visibility_off' : 'visibility' }}
                </span>
              </button>
            </div>
          </div>

          <button
            type="submit"
            :disabled="loading"
            class="w-full py-3.5 bg-[#005EA3] hover:bg-[#003362] text-white font-bold text-sm rounded-lg shadow-sm transition active:scale-[0.99] flex items-center justify-center gap-2 disabled:opacity-50"
          >
            <span v-if="loading" class="material-symbols-outlined animate-spin text-[20px]">progress_activity</span>
            <span>{{ loading ? 'Đang xử lý...' : 'Kích hoạt tài khoản' }}</span>
          </button>
        </form>
      </div>

      <footer class="mt-8 pt-4 text-center text-[11px] text-slate-400 max-w-sm mx-auto leading-relaxed">
        Gặp sự cố? Liên hệ
        <strong class="text-slate-600 font-semibold">Quản trị viên</strong>
        hoặc
        <strong class="text-slate-600 font-semibold">Trung tâm Số và Học liệu</strong>
        để được hỗ trợ kỹ thuật.
      </footer>
    </main>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import api from '@/api/api';
import { useAuthStore } from '@/stores/auth';
import AuthHero from '@/components/auth/AuthHero.vue';
import AuthBrand from '@/components/auth/AuthBrand.vue';

const router = useRouter();
const route = useRoute();
const authStore = useAuthStore();

const password = ref('');
const confirmPassword = ref('');
const showPassword = ref(false);
const showConfirmPassword = ref(false);
const loading = ref(false);

const displayName = computed(
  () => route.query.name || authStore.user?.hoTen || 'bạn'
);

const hasUppercase = computed(() => /[A-Z]/.test(password.value));
const hasLowercase = computed(() => /[a-z]/.test(password.value));
const hasNumber = computed(() => /[0-9]/.test(password.value));
const hasSpecial = computed(() => /[^A-Za-z0-9]/.test(password.value));

const strengthScore = computed(() => {
  let score = 0;
  if (password.value.length >= 8) score += 1;
  if (hasNumber.value) score += 1;
  if (hasUppercase.value && hasLowercase.value) score += 1;
  if (hasSpecial.value) score += 1;
  return score;
});

const strengthLabel = computed(() => {
  if (!password.value) return 'Yếu';
  if (strengthScore.value >= 4) return 'Rất mạnh';
  if (strengthScore.value >= 3) return 'Mạnh';
  if (strengthScore.value >= 2) return 'Trung bình';
  return 'Yếu';
});

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
