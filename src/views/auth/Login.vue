<template>
  <div class="h-screen w-screen bg-slate-100 text-slate-800 flex flex-col lg:flex-row font-sans antialiased selection:bg-[#005EA3] selection:text-white overflow-hidden">
    
    <!-- Left Hero Brand Banner (Desktop >= 1024px) -->
    <aside class="hidden lg:flex lg:w-5/12 xl:w-4/12 relative overflow-hidden bg-[#00284D] shrink-0 h-full flex-col justify-between p-6 xl:p-8 text-white shadow-2xl z-10">
      <!-- Background Image with Gradient Overlay -->
      <img
        src="https://lh3.googleusercontent.com/aida-public/AB6AXuAEp0YQXdxN9bS-TIAXe_isYZaRUHRpQWLih43XYLrdR5a2sHAfXURYkn41SIlhZ_qQoX5r78WVWN914nSgiqPhlWZPdjl9SQAKlXCd33L0E0ehj0lTxD9If7tj3PZRiJSmzXVIqcttWPJwiEJ5R1t7130ImIlf8ZqZzteiqF4o_D8gVqEl_1XLvv0dfxJxIHy1c3fnjKtY76LeFITpiq-fNEmjg3eCz3UpJZDWRYtKPceOl4UYsItBmCQIPChzV6Tb-8aoQhho4Q4epDw"
        alt="Trường Đại học Kinh tế - ĐHĐN"
        class="absolute inset-0 w-full h-full object-cover opacity-25"
      />
      <div class="absolute inset-0 bg-gradient-to-b from-[#00284D]/95 via-[#003B6D]/85 to-[#001D38]"></div>

      <!-- Top Branding in Banner -->
      <div class="relative z-10 space-y-4">
        <div class="inline-flex items-center gap-2.5 bg-white/95 backdrop-blur-md px-3.5 py-2 rounded-xl shadow-md border border-white/30">
          <img
            src="https://lh3.googleusercontent.com/aida-public/AB6AXuDD4bCjeLffk10Yr5FD077Ll-dPfjpg_tq-VaDHGVkxmLdd39X8yi12uSsFM0YezH0RVWZ9dGRG0jOnTzkLXisAqKj1GNNpRCyAcATBfIcVPIq0HVJpQvFwoPiZlrCiff-eRlLlOqcnf5HbALMFQYpfa3s1AhoTFH03sM-XuN64atNs1z1pHwB7nHvDkaCaB7Y1zCJs2_mfZY91mjiEbwbNtByiVcyZvQVLWFSavxUlSzkUsYFlVQ5C2jlqfE8KsyTk_wlVkwsuLlp2"
            alt="DUE Logo"
            class="h-7 w-auto object-contain"
          />
          <span class="text-[11px] font-black text-[#003362] uppercase tracking-wider">ĐẠI HỌC KINH TẾ - ĐHĐN</span>
        </div>

        <div class="space-y-2 pt-1">
          <p class="text-[11px] font-bold text-amber-400 uppercase tracking-widest">Hệ thống Quản lý Thực tập Doanh nghiệp</p>
          <h2 class="text-2xl xl:text-3xl font-black text-white tracking-tight leading-tight">
            CỔNG THÔNG TIN THỰC TẬP <span class="text-amber-400">(IMS)</span>
          </h2>
          <p class="text-xs text-blue-100/90 leading-relaxed font-normal">
            Kết nối Sinh viên - Giảng viên hướng dẫn - Giáo vụ Khoa - Doanh nghiệp trên một nền tảng chuyển đổi số tập trung.
          </p>
        </div>
      </div>

      <!-- Bottom Information Card -->
      <div class="relative z-10 bg-white/10 backdrop-blur-md p-4 rounded-xl border border-white/20 space-y-1.5">
        <div class="flex items-center gap-2 text-amber-300 font-bold text-xs uppercase tracking-wider">
          <span class="material-symbols-outlined text-[16px]">verified_user</span>
          Hỗ trợ Kỹ thuật & Kích hoạt
        </div>
        <p class="text-[11px] text-blue-100 leading-relaxed">
          Sử dụng mã định danh cá nhân do trường cấp để truy cập. Liên hệ Trung tâm Số & Học liệu (Phòng A102) nếu cần trợ giúp.
        </p>
      </div>
    </aside>

    <!-- Right Main Authentication View (Tối ưu chiều cao vừa vặn 100vh không bị cuộn) -->
    <main class="flex-1 bg-slate-50 flex items-center justify-center p-3 sm:p-5 md:p-6 lg:p-8 h-full overflow-y-auto lg:overflow-hidden w-full">
      
      <!-- Compact Responsive Authentication Card -->
      <div class="w-full max-w-sm sm:max-w-md md:max-w-lg lg:max-w-xl bg-white rounded-2xl sm:rounded-3xl border border-slate-200 shadow-xl shadow-slate-200/50 p-4 sm:p-6 md:p-7 space-y-3.5 sm:space-y-4 my-auto max-h-[96vh] flex flex-col justify-between">
        
        <!-- Header Logo & Title Bar -->
        <div class="text-center space-y-1.5 shrink-0">
          <div class="flex justify-center">
            <div class="p-2 bg-slate-50 border border-slate-100 rounded-xl shadow-xs inline-block">
              <img
                src="https://lh3.googleusercontent.com/aida-public/AB6AXuDD4bCjeLffk10Yr5FD077Ll-dPfjpg_tq-VaDHGVkxmLdd39X8yi12uSsFM0YezH0RVWZ9dGRG0jOnTzkLXisAqKj1GNNpRCyAcATBfIcVPIq0HVJpQvFwoPiZlrCiff-eRlLlOqcnf5HbALMFQYpfa3s1AhoTFH03sM-XuN64atNs1z1pHwB7nHvDkaCaB7Y1zCJs2_mfZY91mjiEbwbNtByiVcyZvQVLWFSavxUlSzkUsYFlVQ5C2jlqfE8KsyTk_wlVkwsuLlp2"
                alt="Logo DUE"
                class="h-10 sm:h-12 md:h-14 w-auto object-contain"
              />
            </div>
          </div>
          <div>
            <h1 class="text-xl sm:text-2xl font-black text-[#F7B011] tracking-tight leading-none">IMS</h1>
            <p class="text-[10px] sm:text-xs font-bold text-[#003362] uppercase tracking-wider mt-0.5">
              Hệ thống Quản lý Thực tập Doanh nghiệp
            </p>
            <p class="text-[10px] sm:text-[11px] text-slate-500 font-medium">
              Trường Đại học Kinh tế - Đại học Đà Nẵng
            </p>
          </div>
        </div>

        <!-- Error Alert Banner -->
        <Transition name="fade">
          <div v-if="errorMessage" class="p-2.5 sm:p-3 rounded-xl bg-rose-50 border border-rose-200 text-rose-700 text-xs flex items-start gap-2 shadow-xs animate-shake shrink-0">
            <span class="material-symbols-outlined text-[18px] text-rose-600 shrink-0 mt-0.5">error</span>
            <div class="space-y-0.5 flex-1">
              <p class="font-bold text-rose-800 text-xs">Đăng nhập thất bại!</p>
              <p class="text-rose-700 leading-tight text-[11px] sm:text-xs">{{ errorMessage }}</p>
            </div>
          </div>
        </Transition>

        <!-- Login Form -->
        <form @submit.prevent="handleLogin" class="space-y-3 shrink-0" novalidate>
          
          <!-- Username / Identifier Input -->
          <div class="space-y-1">
            <div class="flex items-center justify-between">
              <label for="input-username" class="block text-[11px] font-bold text-slate-700 uppercase tracking-wider">
                Mã định danh / Tên đăng nhập <span class="text-rose-500">*</span>
              </label>
              <span class="text-[10px] text-slate-400 font-medium hidden sm:inline">Mã SV, Cán bộ hoặc Email</span>
            </div>
            <div class="relative">
              <span class="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 text-[18px] pointer-events-none transition-colors" :class="{ 'text-[#005EA3]': form.maDinhDanh }">
                person
              </span>
              <input
                id="input-username"
                v-model="form.maDinhDanh"
                type="text"
                required
                placeholder="VD: 231121521101, gv001, admin..."
                :class="[
                  'w-full pl-9 pr-3 py-2.5 bg-slate-50 border rounded-xl text-xs sm:text-sm font-semibold text-slate-900 placeholder-slate-400 outline-none transition-all duration-200',
                  inputErrors.maDinhDanh ? 'border-rose-500 focus:ring-2 focus:ring-rose-200' : 'border-slate-200 focus:border-[#005EA3] focus:ring-2 focus:ring-blue-100 focus:bg-white'
                ]"
                @input="inputErrors.maDinhDanh = false"
              />
            </div>
            <p v-if="inputErrors.maDinhDanh" class="text-[10px] text-rose-600 font-medium flex items-center gap-1">
              <span class="material-symbols-outlined text-[13px]">warning</span>
              Vui lòng nhập mã sinh viên, mã cán bộ hoặc tên đăng nhập!
            </p>
          </div>

          <!-- Password Input -->
          <div class="space-y-1">
            <div class="flex items-center justify-between">
              <label for="input-password" class="block text-[11px] font-bold text-slate-700 uppercase tracking-wider">
                Mật khẩu <span class="text-rose-500">*</span>
              </label>
              <router-link to="/forgot-password" class="text-[11px] font-bold text-[#005EA3] hover:underline transition">
                Quên mật khẩu?
              </router-link>
            </div>
            <div class="relative">
              <span class="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 text-[18px] pointer-events-none transition-colors" :class="{ 'text-[#005EA3]': form.matKhau }">
                lock
              </span>
              <input
                id="input-password"
                v-model="form.matKhau"
                :type="showPassword ? 'text' : 'password'"
                required
                placeholder="Nhập mật khẩu (Mặc định: Test@1234)..."
                :class="[
                  'w-full pl-9 pr-10 py-2.5 bg-slate-50 border rounded-xl text-xs sm:text-sm font-semibold text-slate-900 placeholder-slate-400 outline-none transition-all duration-200',
                  inputErrors.matKhau ? 'border-rose-500 focus:ring-2 focus:ring-rose-200' : 'border-slate-200 focus:border-[#005EA3] focus:ring-2 focus:ring-blue-100 focus:bg-white'
                ]"
                @input="inputErrors.matKhau = false"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute right-2.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 p-1 rounded-lg transition cursor-pointer"
                :title="showPassword ? 'Ẩn mật khẩu' : 'Hiện mật khẩu'"
              >
                <span class="material-symbols-outlined text-[18px]">{{ showPassword ? 'visibility_off' : 'visibility' }}</span>
              </button>
            </div>
            <p v-if="inputErrors.matKhau" class="text-[10px] text-rose-600 font-medium flex items-center gap-1">
              <span class="material-symbols-outlined text-[13px]">warning</span>
              Vui lòng nhập mật khẩu!
            </p>
          </div>

          <!-- Remember Me & Account Activation Options -->
          <div class="flex items-center justify-between text-[11px] font-bold gap-2">
            <label class="flex items-center gap-1.5 cursor-pointer text-slate-600 hover:text-slate-800 transition select-none">
              <input type="checkbox" v-model="rememberMe" class="w-3.5 h-3.5 rounded border-slate-300 text-[#005EA3] focus:ring-[#005EA3]" />
              <span>Ghi nhớ đăng nhập</span>
            </label>
            <router-link to="/activate" class="text-[#005EA3] hover:underline flex items-center gap-1 font-bold">
              <span class="material-symbols-outlined text-[14px]">how_to_reg</span>
              Kích hoạt tài khoản
            </router-link>
          </div>

          <!-- Login Submit Button -->
          <button
            type="submit"
            :disabled="loading"
            class="w-full py-3 bg-[#005EA3] hover:bg-[#003362] text-white font-black text-xs sm:text-sm rounded-xl shadow-md shadow-blue-900/15 transition-all duration-200 active:scale-[0.99] flex items-center justify-center gap-2 disabled:opacity-50 cursor-pointer uppercase tracking-wider mt-1"
          >
            <span v-if="loading" class="material-symbols-outlined animate-spin text-[18px]">progress_activity</span>
            <span>{{ loading ? 'Đang xử lý...' : 'ĐĂNG NHẬP HỆ THỐNG' }}</span>
            <span v-if="!loading" class="material-symbols-outlined text-[18px]">arrow_forward</span>
          </button>
        </form>

        <!-- Technical Support Contact Link -->
        <p class="text-[10px] sm:text-[11px] text-slate-500 text-center leading-tight shrink-0">
          Hỗ trợ kỹ thuật:
          <a href="https://www.facebook.com/CenterITC" target="_blank" class="text-[#005EA3] hover:underline font-bold inline-flex items-center gap-0.5 ml-0.5">
            Trung tâm Số & Học liệu DUE
            <span class="material-symbols-outlined text-[11px]">open_in_new</span>
          </a>
        </p>

        <!-- Quick Demo Accounts Grid (Thử nghiệm tài khoản nhanh - Compact 2 hoặc 3 cột) -->
        <div class="pt-2.5 border-t border-slate-100 space-y-2 shrink-0">
          <div class="flex items-center justify-between gap-1">
            <p class="text-[10px] font-extrabold text-slate-400 uppercase tracking-wider flex items-center gap-1">
              <span class="material-symbols-outlined text-amber-500 text-[13px]">touch_app</span>
              Thử nghiệm nhanh (Firestore Live)
            </p>
            <span class="text-[9px] font-bold text-emerald-700 bg-emerald-50 px-2 py-0.5 rounded-full border border-emerald-200 flex items-center gap-1">
              <span class="w-1 h-1 rounded-full bg-emerald-500 animate-pulse"></span>
              Sẵn sàng
            </span>
          </div>

          <!-- Compact 2 or 3 Column Demo Buttons Grid -->
          <div class="grid grid-cols-2 sm:grid-cols-3 gap-1.5 text-[11px]">
            <button
              type="button"
              @click="fillDemo('231121521101', 'Test@1234', 'Sinh viên (Có đề tài)')"
              class="p-1.5 sm:p-2 rounded-lg border border-slate-200 hover:border-[#005EA3] hover:bg-blue-50/50 text-slate-800 font-bold text-left transition flex items-center gap-1.5 group cursor-pointer"
            >
              <span class="text-sm group-hover:scale-110 transition-transform">🎓</span>
              <div class="overflow-hidden leading-tight">
                <span class="block truncate text-slate-800 font-bold">SV (Có đề tài)</span>
                <span class="text-[9px] text-slate-400 font-mono">231121521101</span>
              </div>
            </button>

            <button
              type="button"
              @click="fillDemo('20241021', 'Test@1234', 'Sinh viên (Mới/Trống)')"
              class="p-1.5 sm:p-2 rounded-lg border border-slate-200 hover:border-[#005EA3] hover:bg-blue-50/50 text-slate-800 font-bold text-left transition flex items-center gap-1.5 group cursor-pointer"
            >
              <span class="text-sm group-hover:scale-110 transition-transform">🎓</span>
              <div class="overflow-hidden leading-tight">
                <span class="block truncate text-slate-800 font-bold">SV (Mới/Trống)</span>
                <span class="text-[9px] text-slate-400 font-mono">20241021</span>
              </div>
            </button>

            <button
              type="button"
              @click="fillDemo('gv001', 'Test@1234', 'Giảng viên Hướng dẫn')"
              class="p-1.5 sm:p-2 rounded-lg border border-slate-200 hover:border-[#005EA3] hover:bg-blue-50/50 text-slate-800 font-bold text-left transition flex items-center gap-1.5 group cursor-pointer"
            >
              <span class="text-sm group-hover:scale-110 transition-transform">👨‍🏫</span>
              <div class="overflow-hidden leading-tight">
                <span class="block truncate text-slate-800 font-bold">Giảng viên HD</span>
                <span class="text-[9px] text-slate-400 font-mono">gv001</span>
              </div>
            </button>

            <button
              type="button"
              @click="fillDemo('gv003', 'Test@1234', 'Giảng viên (Lớp sạch)')"
              class="p-1.5 sm:p-2 rounded-lg border border-blue-200 bg-blue-50/30 hover:border-[#005EA3] hover:bg-blue-50 text-slate-800 font-bold text-left transition flex items-center gap-1.5 group cursor-pointer"
            >
              <span class="text-sm group-hover:scale-110 transition-transform">✨</span>
              <div class="overflow-hidden leading-tight">
                <span class="block truncate text-[#003362] font-bold">GV (Lớp Sạch)</span>
                <span class="text-[9px] text-slate-400 font-mono">gv003</span>
              </div>
            </button>

            <button
              type="button"
              @click="fillDemo('gvu001', 'Test@1234', 'Giáo vụ Khoa')"
              class="p-1.5 sm:p-2 rounded-lg border border-slate-200 hover:border-[#005EA3] hover:bg-blue-50/50 text-slate-800 font-bold text-left transition flex items-center gap-1.5 group cursor-pointer"
            >
              <span class="text-sm group-hover:scale-110 transition-transform">📚</span>
              <div class="overflow-hidden leading-tight">
                <span class="block truncate text-slate-800 font-bold">Giáo vụ Khoa</span>
                <span class="text-[9px] text-slate-400 font-mono">gvu001</span>
              </div>
            </button>

            <button
              type="button"
              @click="fillDemo('admin', 'Admin@123', 'Quản trị viên')"
              class="p-1.5 sm:p-2 rounded-lg border border-slate-200 hover:border-[#005EA3] hover:bg-blue-50/50 text-slate-800 font-bold text-left transition flex items-center gap-1.5 group cursor-pointer"
            >
              <span class="text-sm group-hover:scale-110 transition-transform">👑</span>
              <div class="overflow-hidden leading-tight">
                <span class="block truncate text-slate-800 font-bold">Quản trị viên</span>
                <span class="text-[9px] text-slate-400 font-mono">admin</span>
              </div>
            </button>
          </div>
        </div>

      </div>
    </main>

    <!-- Toast Notification -->
    <Transition name="toast">
      <div v-if="toastMessage" class="fixed bottom-4 right-4 bg-slate-900 text-white text-xs font-bold px-3.5 py-2.5 rounded-xl shadow-2xl flex items-center gap-2 z-50 border border-slate-700">
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

const router = useRouter();
const authStore = useAuthStore();

const form = ref({ maDinhDanh: '231121521101', matKhau: 'Test@1234' });
const showPassword = ref(false);
const rememberMe = ref(true);
const loading = ref(false);
const errorMessage = ref('');
const toastMessage = ref('');

const inputErrors = ref({
  maDinhDanh: false,
  matKhau: false
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

function fillDemo(user, pass, roleLabel) {
  resetErrors();
  form.value.maDinhDanh = user;
  form.value.matKhau = pass;
  triggerToast(`Đã chọn tài khoản mẫu: ${roleLabel}`);
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
    errorMessage.value = err.response?.data?.message || 'Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại!';
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
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

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  20%, 60% { transform: translateX(-4px); }
  40%, 80% { transform: translateX(4px); }
}

.animate-shake {
  animation: shake 0.4s ease-in-out;
}
</style>
