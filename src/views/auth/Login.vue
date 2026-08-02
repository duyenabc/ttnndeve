<template>
  <div class="h-screen w-screen bg-white text-slate-800 flex flex-col lg:flex-row font-sans antialiased selection:bg-[#005EA3] selection:text-white overflow-hidden">
    <AuthHero />

    <main class="flex-1 bg-white flex flex-col items-center justify-center px-6 py-8 sm:px-10 h-full overflow-y-auto w-full">
      <div class="w-full max-w-[400px] flex flex-col items-center my-auto space-y-8">
        <AuthBrand />

        <div class="text-center space-y-1.5">
          <h1 class="text-4xl sm:text-5xl font-black text-[#F7B011] tracking-tight leading-none">IMS</h1>
          <p class="text-sm sm:text-base font-medium text-slate-600">
            Hệ thống Quản lý Thực tập
          </p>
        </div>

        <Transition name="fade">
          <div
            v-if="errorMessage"
            class="w-full p-3 rounded-xl bg-rose-50 border border-rose-200 text-rose-700 text-xs flex items-start gap-2"
          >
            <span class="material-symbols-outlined text-[18px] text-rose-600 shrink-0">error</span>
            <p class="leading-snug">{{ errorMessage }}</p>
          </div>
        </Transition>

        <form @submit.prevent="handleLogin" class="w-full space-y-4" novalidate>
          <div class="relative w-full min-w-0 box-border">
            <span
              class="material-symbols-outlined absolute left-3.5 top-1/2 -translate-y-1/2 z-[1] text-slate-400 text-[20px] pointer-events-none"
              :class="{ 'text-[#005EA3]': form.maDinhDanh }"
            >
              person
            </span>
            <input
              id="input-username"
              v-model="form.maDinhDanh"
              type="text"
              required
              autocomplete="username"
              placeholder="Mã định danh / Tên đăng nhập"
              :class="[
                'block w-full box-border pl-11 pr-12 py-3.5 bg-white border rounded-lg text-sm font-medium text-slate-900 placeholder-slate-400 outline-none transition-all duration-200',
                inputErrors.maDinhDanh
                  ? 'border-rose-500 focus:ring-2 focus:ring-rose-200'
                  : 'border-slate-300 focus:border-[#005EA3] focus:ring-2 focus:ring-blue-100',
              ]"
              @input="inputErrors.maDinhDanh = false"
            />
          </div>

          <div class="relative w-full min-w-0 box-border">
            <span
              class="material-symbols-outlined absolute left-3.5 top-1/2 -translate-y-1/2 z-[1] text-slate-400 text-[20px] pointer-events-none"
              :class="{ 'text-[#005EA3]': form.matKhau }"
            >
              lock
            </span>
            <input
              id="input-password"
              v-model="form.matKhau"
              :type="showPassword ? 'text' : 'password'"
              required
              autocomplete="current-password"
              placeholder="Mật khẩu"
              :class="[
                'block w-full box-border pl-11 pr-12 py-3.5 bg-white border rounded-lg text-sm font-medium text-slate-900 placeholder-slate-400 outline-none transition-all duration-200',
                inputErrors.matKhau
                  ? 'border-rose-500 focus:ring-2 focus:ring-rose-200'
                  : 'border-slate-300 focus:border-[#005EA3] focus:ring-2 focus:ring-blue-100',
              ]"
              @input="inputErrors.matKhau = false"
            />
            <button
              type="button"
              @click="showPassword = !showPassword"
              class="absolute right-2 top-1/2 z-[1] -translate-y-1/2 inline-flex h-8 w-8 items-center justify-center border-0 bg-transparent p-0 text-slate-400 hover:text-slate-600 outline-none shadow-none appearance-none cursor-pointer"
              :title="showPassword ? 'Ẩn mật khẩu' : 'Hiện mật khẩu'"
            >
              <span class="material-symbols-outlined text-[20px] leading-none">
                {{ showPassword ? 'visibility_off' : 'visibility' }}
              </span>
            </button>
          </div>

          <button
            type="submit"
            :disabled="loading"
            class="flex w-full box-border py-3.5 bg-[#005EA3] hover:bg-[#003362] text-white font-bold text-sm rounded-lg border border-[#005EA3] shadow-sm transition-all duration-200 active:scale-[0.99] items-center justify-center gap-2 disabled:opacity-50 cursor-pointer"
          >
            <span v-if="loading" class="material-symbols-outlined animate-spin text-[18px]">progress_activity</span>
            <span>{{ loading ? 'Đang xử lý...' : 'Đăng nhập' }}</span>
          </button>
        </form>

        <router-link
          to="/forgot-password"
          class="text-sm font-medium text-[#005EA3] hover:underline transition"
        >
          Quên mật khẩu?
        </router-link>

        <footer class="text-center text-[11px] text-slate-400 leading-relaxed pt-2 max-w-sm">
          Để được trợ giúp, hãy liên hệ với bộ phận hỗ trợ kỹ thuật của Trung tâm Số và Học liệu:
          <a
            href="https://www.facebook.com/CenterITC"
            target="_blank"
            rel="noopener noreferrer"
            class="text-[#005EA3] hover:underline font-medium block mt-0.5"
          >
            https://www.facebook.com/CenterITC
          </a>
        </footer>
      </div>
    </main>

    <Transition name="toast">
      <div
        v-if="toastMessage"
        class="fixed bottom-4 right-4 bg-slate-900 text-white text-xs font-bold px-3.5 py-2.5 rounded-xl shadow-2xl flex items-center gap-2 z-50 border border-slate-700"
      >
        <span class="material-symbols-outlined text-amber-400 text-[16px]">info</span>
        <span>{{ toastMessage }}</span>
      </div>
    </Transition>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';
import AuthHero from '@/components/auth/AuthHero.vue';
import AuthBrand from '@/components/auth/AuthBrand.vue';

const router = useRouter();
const authStore = useAuthStore();

const form = ref({ maDinhDanh: '', matKhau: '' });
const showPassword = ref(false);
const loading = ref(false);
const errorMessage = ref('');
const toastMessage = ref('');

const inputErrors = ref({
  maDinhDanh: false,
  matKhau: false,
});

function triggerToast(msg) {
  toastMessage.value = msg;
  setTimeout(() => {
    toastMessage.value = '';
  }, 2500);
}

function resetErrors() {
  errorMessage.value = '';
  inputErrors.value.maDinhDanh = false;
  inputErrors.value.matKhau = false;
}

async function handleLogin() {
  resetErrors();

  let hasError = false;
  if (!form.value.maDinhDanh || !form.value.maDinhDanh.trim()) {
    inputErrors.value.maDinhDanh = true;
    hasError = true;
  }
  if (!form.value.matKhau) {
    inputErrors.value.matKhau = true;
    hasError = true;
  }

  if (hasError) {
    errorMessage.value = 'Vui lòng nhập đầy đủ Mã định danh và Mật khẩu!';
    return;
  }

  loading.value = true;
  try {
    const res = await authStore.login(form.value);
    triggerToast('✓ Đăng nhập thành công! Đang chuyển hướng...');
    setTimeout(() => {
      if (res.redirectTo) {
        router.push(res.redirectTo);
      } else {
        router.push('/dashboard');
      }
    }, 300);
  } catch (err) {
    const status = err?.response?.status;
    const data = err?.response?.data;
    const isNetwork =
      err?.code === 'ERR_NETWORK' ||
      err?.code === 'ECONNABORTED' ||
      status === 503;
    const looksLikeWrongApi =
      typeof data === 'string' ||
      (typeof data === 'object' && data !== null && !data.message && status === 404);
    errorMessage.value =
      data?.message ||
      (isNetwork
        ? 'Không kết nối được API (mạng/timeout). Nếu dùng Render: đợi service thức dậy rồi thử lại. Local: chạy backend :5071. URL đúng: https://ims-api-ftzr.onrender.com/api'
        : looksLikeWrongApi
          ? 'Sai địa chỉ API. Đặt VITE_API_BASE_URL=https://ims-api-ftzr.onrender.com/api rồi build lại (không dùng ims-backend / ims-api Express).'
          : 'Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại!');
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
/* Edge/IE: ẩn nút reveal mật khẩu mặc định */
#input-password::-ms-reveal,
#input-password::-ms-clear {
  display: none;
}

.fade-enter-active,
.fade-leave-active {
  transition: all 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(-6px);
}

.toast-enter-active,
.toast-leave-active {
  transition: all 0.25s ease;
}
.toast-enter-from,
.toast-leave-to {
  opacity: 0;
  transform: translateY(12px) scale(0.95);
}
</style>
