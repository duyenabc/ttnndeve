<template>
  <footer class="bg-sky-700 text-white w-full mt-auto font-sans">
    <div class="w-full px-6 lg:px-10 pt-16 pb-4">
      <div class="flex flex-col lg:flex-row gap-12 lg:gap-x-12 items-start justify-center max-w-[1280px] mx-auto">
        <div class="flex-1 w-full min-w-0">
          <div class="w-full inline-flex items-center justify-start">
            <img
              alt="Trường Đại học Kinh tế - ĐH Đà Nẵng"
              class="w-56 h-16 object-contain object-left"
              src="https://lh3.googleusercontent.com/aida-public/AB6AXuBAI8djrAd-nZQ5MYlWMJpXo45oC7yEgroVX6knYldfG57Tf2Rbt03V-3ZVOMAhcbLGBMcS4HzqK9ag1JgEyj2btMyxiioLkRfKGqkJni6tddzWXuuoisK1fx4pOCH5dkuF58C-y4Xjdlk88uvBZn13VKmgi1t_gP9uuDa8arV9bL4ClNpQNzK7KGAItjaF66dD56SttjZJl2Kw58MhWgooivjmf_Bb1rKSJGcDmIeqn2-Wmxib7S3YlyN9R3xLtDH4Kc0mz6KHwRLZ"
            />
          </div>
          <div class="w-full pt-6">
            <p class="text-sm leading-snug text-white/70 w-full">
              Chúng tôi tin tưởng rằng giáo dục đại học đóng vai trò then chốt trong
              “kiến tạo xã hội tương lai”. Hệ thống quản lý thực tập (IMS) được thiết kế
              nhằm tối ưu hóa quy trình hướng dẫn và theo sát lộ trình phát triển của sinh viên.
            </p>
          </div>
          <div class="w-full pt-6">
            <router-link
              to="/documents"
              class="text-sm font-semibold leading-tight text-white hover:text-orange-300 transition-colors"
            >
              Read More »
            </router-link>
          </div>
        </div>

        <div class="flex-1 w-full min-w-0 pb-6 lg:pb-20">
          <div class="w-full pb-6">
            <div class="w-full pb-2">
              <p class="text-base font-bold tracking-wider leading-normal text-white uppercase">
                Liên hệ
              </p>
              <div class="w-10 h-0.5 bg-orange-400/50 mt-2" />
            </div>
          </div>
          <div class="w-full space-y-4">
            <div class="w-full inline-flex items-start justify-start">
              <svg width="16" height="22" viewBox="0 0 16 22" fill="none" aria-hidden="true" class="shrink-0 mt-0.5">
                <path
                  d="M8 12C8.55 12 9.02083 11.8042 9.4125 11.4125C9.80417 11.0208 10 10.55 10 10C10 9.45 9.80417 8.97917 9.4125 8.5875C9.02083 8.19583 8.55 8 8 8C7.45 8 6.97917 8.19583 6.5875 8.5875C6.19583 8.97917 6 9.45 6 10C6 10.55 6.19583 11.0208 6.5875 11.4125C6.97917 11.8042 7.45 12 8 12ZM8 19.35C10.0333 17.4833 11.5417 15.7875 12.525 14.2625C13.5083 12.7375 14 11.3833 14 10.2C14 8.38333 13.4208 6.89583 12.2625 5.7375C11.1042 4.57917 9.68333 4 8 4C6.31667 4 4.89583 4.57917 3.7375 5.7375C2.57917 6.89583 2 8.38333 2 10.2C2 11.3833 2.49167 12.7375 3.475 14.2625C4.45833 15.7875 5.96667 17.4833 8 19.35ZM8 22C5.31667 19.7167 3.3125 17.5958 1.9875 15.6375C0.6625 13.6792 0 11.8667 0 10.2C0 7.7 0.804167 5.70833 2.4125 4.225C4.02083 2.74167 5.88333 2 8 2C10.1167 2 11.9792 2.74167 13.5875 4.225C15.1958 5.70833 16 7.7 16 10.2C16 11.8667 15.3375 13.6792 14.0125 15.6375C12.6875 17.5958 10.6833 19.7167 8 22Z"
                  fill="#F37021"
                />
              </svg>
              <p class="pl-3 text-sm leading-snug text-white/70">
                Trung tâm Số và Học Liệu, Trường Đại học Kinh tế, ĐH Đà Nẵng.
              </p>
            </div>
            <a href="tel:+842363525656" class="w-full inline-flex items-center justify-start group">
              <svg width="18" height="18" viewBox="0 0 18 18" fill="none" aria-hidden="true" class="shrink-0">
                <path
                  d="M16.95 18C14.8667 18 12.8083 17.5458 10.775 16.6375C8.74167 15.7292 6.89167 14.4417 5.225 12.775C3.55833 11.1083 2.27083 9.25833 1.3625 7.225C0.454167 5.19167 0 3.13333 0 1.05C0 0.75 0.1 0.5 0.3 0.3C0.5 0.1 0.75 0 1.05 0H5.1C5.33333 0 5.54167 0.0791667 5.725 0.2375C5.90833 0.395833 6.01667 0.583333 6.05 0.8L6.7 4.3C6.73333 4.56667 6.725 4.79167 6.675 4.975C6.625 5.15833 6.53333 5.31667 6.4 5.45L3.975 7.9C4.30833 8.51667 4.70417 9.1125 5.1625 9.6875C5.62083 10.2625 6.125 10.8167 6.675 11.35C7.19167 11.8667 7.73333 12.3458 8.3 12.7875C8.86667 13.2292 9.46667 13.6333 10.1 14L12.45 11.65C12.6 11.5 12.7958 11.3875 13.0375 11.3125C13.2792 11.2375 13.5167 11.2167 13.75 11.25L17.2 11.95C17.4333 12.0167 17.625 12.1375 17.775 12.3125C17.925 12.4875 18 12.6833 18 12.9V16.95C18 17.25 17.9 17.5 17.7 17.7C17.5 17.9 17.25 18 16.95 18Z"
                  fill="#F37021"
                />
              </svg>
              <p class="pl-3 text-sm leading-tight text-white group-hover:text-orange-300 transition-colors">
                (84.236) 3525656
              </p>
            </a>
            <a href="mailto:i.t.c@due.edu.vn" class="w-full inline-flex items-center justify-start group">
              <svg width="20" height="16" viewBox="0 0 20 16" fill="none" aria-hidden="true" class="shrink-0">
                <path
                  d="M2 16C1.45 16 0.979167 15.8042 0.5875 15.4125C0.195833 15.0208 0 14.55 0 14V2C0 1.45 0.195833 0.979167 0.5875 0.5875C0.979167 0.195833 1.45 0 2 0H18C18.55 0 19.0208 0.195833 19.4125 0.5875C19.8042 0.979167 20 1.45 20 2V14C20 14.55 19.8042 15.0208 19.4125 15.4125C19.0208 15.8042 18.55 16 18 16H2ZM10 9L2 4V14H18V4L10 9ZM10 7L18 2H2L10 7Z"
                  fill="#F37021"
                />
              </svg>
              <p class="pl-3 text-sm leading-tight text-white group-hover:text-orange-300 transition-colors">
                i.t.c@due.edu.vn
              </p>
            </a>
          </div>
        </div>

        <div class="flex-1 w-full min-w-0 pb-6 lg:pb-16">
          <div class="w-full pb-6">
            <div class="w-full pb-2">
              <p class="text-base font-bold tracking-wider leading-normal text-white uppercase">
                Quản lý thực tập
              </p>
              <div class="w-10 h-0.5 bg-orange-400/50 mt-2" />
            </div>
          </div>
          <div class="w-full space-y-3">
            <div v-for="link in managementLinks" :key="link.to + link.label" class="w-full">
              <router-link
                :to="link.to"
                class="text-sm leading-tight text-white/70 hover:text-white transition-colors"
              >
                {{ link.label }}
              </router-link>
            </div>
          </div>
        </div>

        <div class="flex-1 w-full min-w-0 relative pb-16 lg:pb-44">
          <div class="w-full pb-6">
            <div class="w-full pb-2">
              <p class="text-base font-bold tracking-wider leading-normal text-white uppercase">
                Kết nối
              </p>
              <div class="w-10 h-0.5 bg-orange-400/50 mt-2" />
            </div>
          </div>
          <div class="flex flex-wrap gap-3">
            <a
              href="https://facebook.com"
              title="Facebook"
              target="_blank"
              rel="noopener noreferrer"
              class="social-btn"
            >
              <svg viewBox="0 0 24 24" class="w-5 h-5 fill-current" aria-hidden="true">
                <path d="M22 12.07C22 6.48 17.52 2 11.93 2S1.86 6.48 1.86 12.07c0 5.02 3.66 9.18 8.44 9.93v-7.02H7.9v-2.91h2.4V9.86c0-2.37 1.4-3.68 3.56-3.68 1.03 0 2.11.18 2.11.18v2.32h-1.19c-1.17 0-1.54.73-1.54 1.48v1.77h2.62l-.42 2.91h-2.2V22c4.78-.75 8.44-4.91 8.44-9.93z" />
              </svg>
            </a>
            <a
              href="https://youtube.com"
              title="YouTube"
              target="_blank"
              rel="noopener noreferrer"
              class="social-btn"
            >
              <svg viewBox="0 0 24 24" class="w-5 h-5 fill-current" aria-hidden="true">
                <path d="M23.5 6.19a3.02 3.02 0 0 0-2.12-2.14C19.5 3.55 12 3.55 12 3.55s-7.5 0-9.38.5A3.02 3.02 0 0 0 .5 6.19C0 8.07 0 12 0 12s0 3.93.5 5.81a3.02 3.02 0 0 0 2.12 2.14c1.88.5 9.38.5 9.38.5s7.5 0 9.38-.5a3.02 3.02 0 0 0 2.12-2.14C24 15.93 24 12 24 12s0-3.93-.5-5.81zM9.55 15.57V8.43L15.82 12l-6.27 3.57z" />
              </svg>
            </a>
            <a
              href="https://tiktok.com"
              title="TikTok"
              target="_blank"
              rel="noopener noreferrer"
              class="social-btn"
            >
              <svg viewBox="0 0 24 24" class="w-5 h-5 fill-current" aria-hidden="true">
                <path d="M19.59 6.69a4.83 4.83 0 0 1-3.77-4.25V2h-3.45v13.67a2.89 2.89 0 0 1-2.88 2.5 2.89 2.89 0 0 1-2.89-2.89 2.89 2.89 0 0 1 2.89-2.89c.28 0 .54.04.79.1v-3.5a6.37 6.37 0 0 0-.79-.05A6.34 6.34 0 0 0 3.15 15.28 6.34 6.34 0 0 0 9.49 21.6a6.34 6.34 0 0 0 6.34-6.34V8.77a8.16 8.16 0 0 0 4.76 1.52V6.84a4.85 4.85 0 0 1-.99-.15z" />
              </svg>
            </a>
            <a
              href="https://linkedin.com"
              title="LinkedIn"
              target="_blank"
              rel="noopener noreferrer"
              class="social-btn"
            >
              <svg viewBox="0 0 24 24" class="w-5 h-5 fill-current" aria-hidden="true">
                <path d="M20.45 20.45h-3.55v-5.57c0-1.33-.03-3.04-1.85-3.04-1.85 0-2.14 1.45-2.14 2.94v5.67H9.35V9h3.41v1.56h.05c.48-.9 1.64-1.85 3.37-1.85 3.6 0 4.27 2.37 4.27 5.46v6.28zM5.34 7.43a2.06 2.06 0 1 1 0-4.12 2.06 2.06 0 0 1 0 4.12zM7.12 20.45H3.56V9h3.56v11.45zM22.23 0H1.77C.79 0 0 .77 0 1.73v20.54C0 23.23.79 24 1.77 24h20.45c.98 0 1.78-.77 1.78-1.73V1.73C24 .77 23.2 0 22.22 0h.01z" />
              </svg>
            </a>
          </div>

          <button
            type="button"
            class="absolute bottom-0 right-0 w-10 h-10 bg-blue-900 border border-white/10 rounded flex items-center justify-center hover:bg-[#F37021] transition-colors"
            title="Lên đầu trang"
            @click="scrollToTop"
          >
            <span class="material-symbols-outlined text-white text-[20px]">keyboard_arrow_up</span>
          </button>
        </div>
      </div>
    </div>

    <div class="px-8 py-6 bg-blue-900">
      <p class="text-xs leading-tight text-center text-white/90">
        © Bản quyền Trường Đại học Kinh tế, Đại học Đà Nẵng | Phát triển bởi Trung tâm Số và Học Liệu
      </p>
    </div>
  </footer>
</template>

<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();
const route = useRoute();

const userRole = computed(() => authStore.user?.vaiTro || 'SinhVien');

const classId = computed(() => {
  const id = route.params.id;
  return typeof id === 'string' && id ? id : null;
});

const managementLinks = computed(() => {
  const cid = classId.value;
  if (userRole.value === 'GiangVien') {
    return [
      { to: '/dashboard', label: 'Dashboard tổng quan' },
      { to: cid ? `/teacher/classes/${cid}/diaries` : '/teacher/classes', label: 'Công việc' },
      { to: cid ? `/teacher/classes/${cid}/grading` : '/teacher/classes', label: 'Đánh giá' },
      { to: '/documents', label: 'Kho tài liệu & biểu mẫu' },
      { to: cid ? `/teacher/classes/${cid}/settings` : '/teacher/classes', label: 'Cấu hình lớp' },
    ];
  }
  if (userRole.value === 'Admin') {
    return [
      { to: '/dashboard', label: 'Dashboard tổng quan' },
      { to: '/admin/accounts', label: 'Quản lý tài khoản' },
      { to: '/pending-requests', label: 'Yêu cầu chờ xử lý' },
      { to: '/documents', label: 'Kho tài liệu & biểu mẫu' },
    ];
  }
  if (userRole.value === 'GiaoVu') {
    return [
      { to: '/dashboard', label: 'Dashboard tổng quan' },
      { to: '/giaovu-requests', label: 'Yêu cầu của tôi' },
      { to: '/documents', label: 'Kho tài liệu & biểu mẫu' },
    ];
  }
  return [
    { to: '/dashboard', label: 'Dashboard tổng quan' },
    { to: '/student/progress', label: 'Tiến trình của tôi' },
    { to: '/student/classes', label: 'Lớp thực tập của tôi' },
    { to: '/documents', label: 'Kho tài liệu & biểu mẫu' },
  ];
});

function scrollToTop() {
  window.scrollTo({ top: 0, behavior: 'smooth' });
}
</script>

<style scoped>
.social-btn {
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 0.5rem;
  border: 1px solid rgb(255 255 255 / 0.2);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  color: #fff;
  transition: background-color 0.15s ease, border-color 0.15s ease;
}
.social-btn:hover {
  background-color: rgb(255 255 255 / 0.1);
  border-color: rgb(255 255 255 / 0.4);
}
</style>
