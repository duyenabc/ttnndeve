<template>
  <div class="ims-scope max-w-5xl mx-auto font-sans space-y-6 pb-20">
    <nav class="flex items-center text-xs text-slate-500 gap-1.5">
      <router-link to="/dashboard" class="hover:text-[#005EA3] transition font-medium">Bảng tin</router-link>
      <span class="material-symbols-outlined text-[14px]">chevron_right</span>
      <span class="text-slate-900 font-bold">Hồ sơ cá nhân</span>
    </nav>

    <div>
      <h1 class="text-2xl font-extrabold text-slate-900 tracking-tight flex items-center gap-2">
        <span class="material-symbols-outlined text-[#005EA3] text-[28px]">badge</span>
        Hồ sơ cá nhân
      </h1>
      <p class="text-xs text-slate-500 mt-1">Quản lý thông tin tài khoản định danh và cập nhật ảnh đại diện sinh viên/giảng viên.</p>
    </div>

    <div class="bg-white rounded-2xl border border-slate-200 shadow-2xs overflow-hidden">
      <div class="relative h-40 bg-gradient-to-r from-[#003362] to-[#005EA3]">
        <div class="absolute -bottom-12 left-8 sm:left-12">
          <div class="relative group">
            <div class="w-32 h-32 rounded-full border-4 border-white overflow-hidden bg-slate-100 shadow-md">
              <img
                src="https://lh3.googleusercontent.com/aida-public/AB6AXuC4myu-vncusDzoKcxkvT6dcy4Et59c5PgMRJRY2s9xbDIJ5XKWSCxg0Q0zvsvC7CZ1jcUr-O2aDID3-wp6WgKLJa8-x8qhfqeCn0XsKUqIUmKvXRPvV0oUDCD4MC6r1m5Lwj5n-o8Jxu1mVlBD5o8kTM48j4be1z4ipdktMvxUJI48Vf2ThZ4KCOP5by9B0TF39b5UTgrGpCOEd4lkX86tKc-wO7q8mycbeVsH2Dc7uDZxpE_Pl3DYOcrDW0sERh0ive5McJboIbxS"
                alt="Profile Avatar"
                class="w-full h-full object-cover"
              />
            </div>
            <button
              @click="triggerAvatarUpload"
              class="absolute bottom-1 right-1 bg-[#005EA3] text-white w-9 h-9 rounded-full flex items-center justify-center border-2 border-white shadow-md hover:scale-105 active:scale-95 transition"
              title="Đổi ảnh đại diện"
            >
              <span class="material-symbols-outlined text-[18px]">edit</span>
            </button>
            <input ref="avatarInputRef" type="file" accept="image/*" class="hidden" @change="onAvatarSelected" />
          </div>
        </div>
      </div>

      <div class="pt-16 p-8 sm:p-12 space-y-8">
        <div>
          <h3 class="text-lg font-extrabold text-slate-900">Thông tin cơ bản</h3>
          <p class="text-xs text-slate-500 font-medium">Thông tin này được quản lý và đồng bộ trực tiếp từ phòng Đào tạo và Khoa.</p>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6 text-xs">
          <div class="space-y-1.5">
            <label class="block font-bold text-slate-400 uppercase tracking-wider text-[10px]">Họ và tên</label>
            <div class="px-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-slate-900 font-extrabold text-sm uppercase">
              {{ profile?.hoTen || authStore.user?.hoTen || 'Người dùng' }}
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block font-bold text-slate-400 uppercase tracking-wider text-[10px]">Mã sinh viên / Mã định danh</label>
            <div class="px-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-slate-900 font-bold font-mono text-sm">
              {{ profile?.maDinhDanh || authStore.user?.maDinhDanh || authStore.user?.username || '---' }}
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block font-bold text-slate-400 uppercase tracking-wider text-[10px]">Email công vụ (@due.edu.vn)</label>
            <div class="px-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-slate-900 font-medium text-sm">
              {{ profile?.email || authStore.user?.email || '---' }}
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block font-bold text-slate-400 uppercase tracking-wider text-[10px]">Lớp sinh hoạt / Chuyên ngành</label>
            <div class="px-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-slate-900 font-medium text-sm">
              {{ profile?.lopSinhHoat || 'Lớp 49K21.1 • Hệ thông tin quản lý' }}
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block font-bold text-slate-400 uppercase tracking-wider text-[10px]">Khoa trực thuộc</label>
            <div class="px-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-slate-900 font-medium text-sm">
              {{ profile?.tenKhoa || 'Khoa Thống kê - Tin học' }}
            </div>
          </div>

          <div class="space-y-1.5">
            <label class="block font-bold text-slate-400 uppercase tracking-wider text-[10px]">Số điện thoại liên hệ</label>
            <div class="px-4 py-3 bg-slate-50 border border-slate-200 rounded-xl text-slate-900 font-medium text-sm">
              {{ profile?.soDienThoai || '0905 123 456' }}
            </div>
          </div>
        </div>

        <div class="p-4 bg-blue-50/70 border border-blue-200 rounded-xl flex items-start gap-3 text-xs">
          <span class="material-symbols-outlined text-[#005EA3] shrink-0 text-[20px]">info</span>
          <p class="text-slate-700 leading-relaxed font-medium">
            <strong class="text-slate-900 font-bold">Lưu ý:</strong> Hiện tại hệ thống chỉ cho phép tự cập nhật ảnh đại diện. Nếu thông tin định danh (Họ tên, Mã SV, Khoa) có sai sót, vui lòng liên hệ Ban chủ nhiệm khoa để yêu cầu điều chỉnh.
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import api from '@/api/api';

const profile = ref(null);
const avatarInputRef = ref(null);

function triggerAvatarUpload() {
  avatarInputRef.value?.click();
}

function onAvatarSelected(e) {
  const file = e.target.files[0];
  if (file) {
    alert(`Đã chọn ảnh đại diện mới: ${file.name}. Đang tải lên...`);
  }
}

onMounted(async () => {
  try {
    const res = await api.get('/auth/me');
    profile.value = res.data;
  } catch (err) {
    console.error(err);
  }
});
</script>
