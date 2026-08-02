<template>
  <div class="ims-scope max-w-2xl mx-auto font-sans space-y-6 pb-20">
    <nav class="flex items-center text-xs text-slate-500 gap-1.5">
      <router-link to="/dashboard" class="hover:text-[#005EA3] transition font-medium">Bảng tin</router-link>
      <span class="material-symbols-outlined text-[14px]">chevron_right</span>
      <span class="text-slate-900 font-bold">Mật khẩu & Bảo mật</span>
    </nav>

    <div class="bg-white rounded-2xl border border-slate-200 shadow-2xs p-8 sm:p-10 space-y-8">
      <div>
        <h1 class="text-2xl font-extrabold text-slate-900 tracking-tight flex items-center gap-2">
          <span class="material-symbols-outlined text-[#005EA3] text-[28px]">lock_reset</span>
          Đổi mật khẩu
        </h1>
        <p class="text-xs text-slate-500 mt-1 font-medium">Vui lòng thiết lập mật khẩu mạnh để bảo vệ tài khoản cá nhân của bạn.</p>
      </div>

      <form @submit.prevent="handleSubmit" class="space-y-6 text-xs">
        <div class="space-y-1.5">
          <label class="block font-bold text-slate-700 uppercase tracking-wider">Mật khẩu hiện tại <span class="text-rose-600">*</span></label>
          <div class="relative">
            <span class="material-symbols-outlined absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400 text-[20px]">lock</span>
            <input
              v-model="oldPass"
              :type="showOldPass ? 'text' : 'password'"
              required
              placeholder="Nhập mật khẩu hiện tại..."
              class="w-full pl-11 pr-11 py-3.5 bg-slate-50 border border-slate-200 rounded-xl text-sm font-medium text-slate-900 outline-none focus:ring-2 focus:ring-[#005EA3] focus:bg-white transition"
            />
            <button
              type="button"
              @click="showOldPass = !showOldPass"
              class="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 p-1"
            >
              <span class="material-symbols-outlined text-[20px]">{{ showOldPass ? 'visibility_off' : 'visibility' }}</span>
            </button>
          </div>
        </div>

        <div class="space-y-1.5">
          <label class="block font-bold text-slate-700 uppercase tracking-wider">Mật khẩu mới <span class="text-rose-600">*</span></label>
          <div class="relative">
            <span class="material-symbols-outlined absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400 text-[20px]">lock_open</span>
            <input
              v-model="newPass"
              :type="showNewPass ? 'text' : 'password'"
              required
              placeholder="Nhập mật khẩu mới..."
              class="w-full pl-11 pr-11 py-3.5 bg-slate-50 border border-slate-200 rounded-xl text-sm font-medium text-slate-900 outline-none focus:ring-2 focus:ring-[#005EA3] focus:bg-white transition"
            />
            <button
              type="button"
              @click="showNewPass = !showNewPass"
              class="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 p-1"
            >
              <span class="material-symbols-outlined text-[20px]">{{ showNewPass ? 'visibility_off' : 'visibility' }}</span>
            </button>
          </div>

          <div class="pt-2 space-y-1.5">
            <div class="flex justify-between items-center text-[10px] font-bold">
              <span class="text-slate-400 uppercase tracking-wider">ĐỘ MẠNH MẬT KHẨU</span>
              <span :class="isStrong ? 'text-emerald-600' : 'text-amber-600'">
                {{ isStrong ? 'RẤT MẠNH' : 'TRUNG BÌNH' }}
              </span>
            </div>
            <div class="h-1.5 w-full bg-slate-100 rounded-full overflow-hidden">
              <div
                class="h-full transition-all duration-300"
                :class="isStrong ? 'bg-emerald-600 w-full' : (newPass.length > 0 ? 'bg-amber-500 w-2/3' : 'w-0')"
              ></div>
            </div>
          </div>
        </div>

        <div class="space-y-1.5">
          <label class="block font-bold text-slate-700 uppercase tracking-wider">Xác nhận mật khẩu mới <span class="text-rose-600">*</span></label>
          <div class="relative">
            <span class="material-symbols-outlined absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400 text-[20px]">verified_user</span>
            <input
              v-model="confirmPass"
              :type="showConfirmPass ? 'text' : 'password'"
              required
              placeholder="Nhập lại mật khẩu mới..."
              class="w-full pl-11 pr-11 py-3.5 bg-slate-50 border border-slate-200 rounded-xl text-sm font-medium text-slate-900 outline-none focus:ring-2 focus:ring-[#005EA3] focus:bg-white transition"
            />
            <button
              type="button"
              @click="showConfirmPass = !showConfirmPass"
              class="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 p-1"
            >
              <span class="material-symbols-outlined text-[20px]">{{ showConfirmPass ? 'visibility_off' : 'visibility' }}</span>
            </button>
          </div>
        </div>

        <div class="bg-slate-50 p-6 rounded-2xl border border-slate-200 space-y-3">
          <h3 class="text-[10px] font-extrabold text-slate-400 uppercase tracking-wider">TIÊU CHUẨN MẬT KHẨU AN TOÀN</h3>
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-3 text-xs">
            <div class="flex items-center gap-2" :class="hasLength ? 'text-emerald-700 font-bold' : 'text-slate-500'">
              <span class="material-symbols-outlined text-[16px]">{{ hasLength ? 'check_circle' : 'radio_button_unchecked' }}</span>
              <span>Tối thiểu 8 ký tự</span>
            </div>
            <div class="flex items-center gap-2" :class="hasUpper ? 'text-emerald-700 font-bold' : 'text-slate-500'">
              <span class="material-symbols-outlined text-[16px]">{{ hasUpper ? 'check_circle' : 'radio_button_unchecked' }}</span>
              <span>Có chữ cái viết hoa (A-Z)</span>
            </div>
            <div class="flex items-center gap-2" :class="hasLower ? 'text-emerald-700 font-bold' : 'text-slate-500'">
              <span class="material-symbols-outlined text-[16px]">{{ hasLower ? 'check_circle' : 'radio_button_unchecked' }}</span>
              <span>Có chữ cái viết thường (a-z)</span>
            </div>
            <div class="flex items-center gap-2" :class="hasNum ? 'text-emerald-700 font-bold' : 'text-slate-500'">
              <span class="material-symbols-outlined text-[16px]">{{ hasNum ? 'check_circle' : 'radio_button_unchecked' }}</span>
              <span>Bao gồm chữ số (0-9)</span>
            </div>
            <div class="flex items-center gap-2 sm:col-span-2" :class="hasSpecial ? 'text-emerald-700 font-bold' : 'text-slate-500'">
              <span class="material-symbols-outlined text-[16px]">{{ hasSpecial ? 'check_circle' : 'radio_button_unchecked' }}</span>
              <span>Ký tự đặc biệt (@, #, $, !...)</span>
            </div>
          </div>
        </div>

        <div class="pt-4 flex items-center justify-end gap-3 border-t border-slate-100">
          <button
            type="button"
            @click="router.push('/dashboard')"
            class="px-6 py-2.5 rounded-xl border border-slate-300 font-bold text-slate-700 hover:bg-slate-100 transition"
          >
            Hủy bỏ
          </button>
          <button
            type="submit"
            :disabled="loading"
            class="px-8 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-md transition active:scale-95 flex items-center gap-2 disabled:opacity-50"
          >
            <span v-if="loading" class="material-symbols-outlined animate-spin text-[18px]">progress_activity</span>
            <span>Lưu thay đổi</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';
import api from '@/api/api';

const router = useRouter();
const oldPass = ref('');
const newPass = ref('');
const confirmPass = ref('');

const showOldPass = ref(false);
const showNewPass = ref(false);
const showConfirmPass = ref(false);
const loading = ref(false);

const hasLength = computed(() => newPass.value.length >= 8);
const hasUpper = computed(() => /[A-Z]/.test(newPass.value));
const hasLower = computed(() => /[a-z]/.test(newPass.value));
const hasNum = computed(() => /[0-9]/.test(newPass.value));
const hasSpecial = computed(() => /[^A-Za-z0-9]/.test(newPass.value));
const isStrong = computed(() => hasLength.value && hasUpper.value && hasNum.value && hasSpecial.value);

async function handleSubmit() {
  if (newPass.value !== confirmPass.value) {
    alert('Mật khẩu xác nhận không khớp.');
    return;
  }
  loading.value = true;
  try {
    await api.post('/auth/change-password', { matKhauHienTai: oldPass.value, matKhauMoi: newPass.value });
    alert('Đổi mật khẩu thành công!');
    router.push('/dashboard');
  } catch (err) {
    alert(err.response?.data?.message || 'Có lỗi xảy ra khi đổi mật khẩu.');
  } finally {
    loading.value = false;
  }
}
</script>
