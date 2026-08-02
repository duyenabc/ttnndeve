<template>
  <div class="ims-scope min-h-screen bg-surface text-on-surface">
    <header class="fixed top-0 left-0 w-full z-[60] flex justify-between items-center px-3 sm:px-6 lg:px-container-padding h-16 bg-[#005EA3] border-b border-white/10 font-body-md">
      <div class="flex items-center gap-3 sm:gap-8">
        <button
          @click="toggleMobileSidebar"
          class="lg:hidden text-white p-1.5 rounded-lg hover:bg-white/10 transition-colors flex items-center justify-center cursor-pointer"
          aria-label="Mở Menu"
          title="Mở Menu"
        >
          <span class="material-symbols-outlined text-[26px]">menu</span>
        </button>
        <span class="font-display-lg text-display-lg font-black text-white">IMS</span>
        <nav class="hidden md:flex gap-6 h-16 items-center">
          <router-link to="/dashboard" class="text-white/80 hover:bg-white/10 transition-colors px-3 h-full flex items-center font-title-sm text-title-sm">Bảng tin</router-link>
          
          <template v-if="userRole === 'SinhVien'">
            <router-link to="/student/classes" class="text-white/80 hover:bg-white/10 transition-colors px-3 h-full flex items-center font-title-sm text-title-sm">Lớp của tôi</router-link>
            <router-link to="/student/calendar" class="text-white/80 hover:bg-white/10 transition-colors px-3 h-full flex items-center font-title-sm text-title-sm">Lịch cá nhân</router-link>
          </template>

          <template v-else-if="userRole === 'GiangVien'">
            <router-link to="/teacher/classes" class="text-white/80 hover:bg-white/10 transition-colors px-3 h-full flex items-center font-title-sm text-title-sm">Lớp của tôi</router-link>
            <router-link to="/teacher/schedule" class="text-white/80 hover:bg-white/10 transition-colors px-3 h-full flex items-center font-title-sm text-title-sm">Lịch hướng dẫn</router-link>
          </template>

          <template v-else-if="userRole === 'GiaoVu'">
            <router-link to="/giaovu-requests" class="text-white/80 hover:bg-white/10 transition-colors px-3 h-full flex items-center font-title-sm text-title-sm">Yêu cầu của tôi</router-link>
            <router-link to="/documents" class="text-white/80 hover:bg-white/10 transition-colors px-3 h-full flex items-center font-title-sm text-title-sm">Kho tài liệu</router-link>
          </template>

          <template v-else-if="userRole === 'Admin'">
            <router-link to="/admin/accounts" class="text-white/80 hover:bg-white/10 transition-colors px-3 h-full flex items-center font-title-sm text-title-sm">Quản lý tài khoản</router-link>
            <router-link to="/pending-requests" class="text-white/80 hover:bg-white/10 transition-colors px-3 h-full flex items-center font-title-sm text-title-sm">Yêu cầu chờ xử lý</router-link>
          </template>
        </nav>
      </div>

      <div class="flex items-center gap-4">
        <div class="relative">
          <button
            class="material-symbols-outlined text-white p-2 rounded-full hover:bg-white/10 transition-colors relative cursor-pointer select-none flex items-center justify-center"
            aria-label="Thông báo"
            @click.stop="toggleNotifications"
          >
            notifications
            <span
              v-if="unreadCount > 0"
              class="absolute -top-0.5 -right-0.5 flex min-w-[20px] h-[20px] px-1 items-center justify-center rounded-full bg-rose-500 text-[11px] font-extrabold text-white shadow-md ring-2 ring-[#005EA3] animate-pulse"
              title="Thông báo chưa đọc"
            >
              {{ unreadCount > 99 ? '99+' : unreadCount }}
            </span>
          </button>

          <div
            v-if="notificationsOpen"
            class="absolute top-12 -right-12 sm:right-0 w-[320px] sm:w-[390px] bg-white rounded-3xl shadow-2xl border border-slate-200 z-[90] overflow-hidden font-sans text-xs transition-all"
            @click.stop
          >
            <div class="px-5 py-3.5 bg-gradient-to-r from-slate-900 to-blue-950 text-white flex items-center justify-between">
              <div class="flex items-center gap-2">
                <span class="material-symbols-outlined text-amber-400 text-[20px]">notifications_active</span>
                <span class="font-bold text-sm">Thông báo</span>
                <span v-if="unreadCount > 0" class="px-2 py-0.5 rounded-full bg-rose-500 text-white font-extrabold text-[10px]">
                  {{ unreadCount }} chưa đọc
                </span>
              </div>
              <button
                v-if="unreadCount > 0"
                @click="markAllAsRead"
                class="text-[11px] text-blue-200 hover:text-white font-semibold underline underline-offset-2 transition cursor-pointer"
              >
                Đánh dấu tất cả đã đọc
              </button>
            </div>

            <div class="flex items-center border-b border-slate-100 bg-slate-50/80 px-4 py-2 gap-2 text-[11px] font-bold">
              <button
                type="button"
                @click="activeFilterTab = 'all'"
                class="px-3 py-1 rounded-full transition cursor-pointer"
                :class="activeFilterTab === 'all' ? 'bg-[#005EA3] text-white shadow-2xs' : 'text-slate-600 hover:bg-slate-200/60'"
              >
                Tất cả ({{ notificationList.length }})
              </button>
              <button
                type="button"
                @click="activeFilterTab = 'unread'"
                class="px-3 py-1 rounded-full transition cursor-pointer flex items-center gap-1.5"
                :class="activeFilterTab === 'unread' ? 'bg-[#005EA3] text-white shadow-2xs' : 'text-slate-600 hover:bg-slate-200/60'"
              >
                Chưa đọc
                <span
                  class="w-4 h-4 rounded-full text-[10px] flex items-center justify-center"
                  :class="activeFilterTab === 'unread' ? 'bg-white text-[#005EA3] font-black' : 'bg-rose-500 text-white font-bold'"
                >
                  {{ unreadCount }}
                </span>
              </button>
            </div>

            <div class="max-h-[360px] overflow-y-auto divide-y divide-slate-100">
              <div
                v-for="item in filteredNotificationList"
                :key="item.id"
                class="p-3.5 hover:bg-slate-50 transition-colors cursor-pointer flex items-start gap-3 relative group"
                :class="{ 'bg-blue-50/50 border-l-4 border-l-blue-600': !item.read, 'opacity-75': item.read }"
                @click="handleNotificationClick(item)"
              >
                <div
                  class="w-9 h-9 rounded-2xl flex items-center justify-center shrink-0 shadow-2xs font-bold mt-0.5"
                  :class="item.bgClass || 'bg-blue-100 text-blue-700'"
                >
                  <span class="material-symbols-outlined text-[18px]">{{ item.icon || 'info' }}</span>
                </div>

                <div class="flex-1 space-y-1">
                  <div class="flex items-start justify-between gap-1 pr-2">
                    <p class="font-extrabold text-slate-800 leading-snug" :class="{ 'text-blue-950': !item.read }">
                      {{ item.title }}
                    </p>
                  </div>
                  <p class="text-slate-600 leading-normal text-[11px] line-clamp-2">{{ item.content }}</p>
                  <div class="flex items-center justify-between pt-1">
                    <p class="text-[10px] text-slate-400 font-medium flex items-center gap-1">
                      <span class="material-symbols-outlined text-[12px]">schedule</span>
                      {{ formatTimeAgo(item.createdAt) || item.time }}
                    </p>

                    <button
                      type="button"
                      @click="toggleItemRead(item, $event)"
                      class="text-[10px] font-bold px-2 py-0.5 rounded-full border transition cursor-pointer"
                      :class="item.read ? 'bg-slate-100 text-slate-500 border-slate-200 hover:bg-blue-50 hover:text-blue-600' : 'bg-blue-100 text-blue-700 border-blue-200 hover:bg-emerald-100 hover:text-emerald-700'"
                      :title="item.read ? 'Đánh dấu chưa đọc' : 'Đánh dấu đã đọc'"
                    >
                      {{ item.read ? 'Đã đọc' : 'Chưa đọc' }}
                    </button>
                  </div>
                </div>
              </div>

              <div v-if="filteredNotificationList.length === 0" class="py-10 text-center text-slate-400 space-y-2">
                <span class="material-symbols-outlined text-[36px] text-slate-300">notifications_off</span>
                <p class="text-xs font-semibold">
                  {{ activeFilterTab === 'unread' ? 'Không có thông báo chưa đọc nào' : 'Không có thông báo nào' }}
                </p>
              </div>
            </div>

            <div class="px-4 py-2.5 bg-slate-50 border-t border-slate-100 text-center flex items-center justify-between text-[11px] text-slate-500">
              <span>Đang hiển thị thông báo thời gian thực</span>
              <button
                v-if="notificationList.length > 0"
                @click="clearAllNotifications"
                class="text-rose-600 hover:text-rose-800 font-bold transition cursor-pointer"
              >
                Xóa tất cả
              </button>
            </div>
          </div>
        </div>

        <div class="relative">
          <button
            class="flex items-center gap-2 cursor-pointer p-1 pr-2 rounded-full hover:bg-white/10 transition-colors select-none"
            @click.stop="toggleUserMenu"
          >
            <span class="material-symbols-outlined text-white text-[32px]" style="font-variation-settings:'FILL' 1;">account_circle</span>
            <span class="hidden sm:block text-white text-body-md font-medium max-w-[150px] truncate">{{ authStore.user?.hoTen || 'Người dùng' }}</span>
            <span class="material-symbols-outlined text-white text-[18px]">expand_more</span>
          </button>

          <div
            v-if="menuOpen"
            class="absolute top-12 right-0 w-[240px] bg-white rounded-2xl shadow-2xl border border-slate-200 z-[90] overflow-hidden py-2 font-sans text-xs"
            @click.stop
          >
            <div class="px-4 py-3 border-b border-slate-100 bg-slate-50/80 flex items-center gap-3">
              <div class="w-9 h-9 rounded-full bg-[#005EA3] text-white flex items-center justify-center font-bold text-sm shrink-0">
                {{ (authStore.user?.hoTen || 'U').charAt(0) }}
              </div>
              <div class="overflow-hidden">
                <p class="font-extrabold text-slate-900 truncate">{{ authStore.user?.hoTen || 'Người dùng' }}</p>
                <p class="text-[10px] text-blue-700 font-bold uppercase tracking-wider mt-0.5">{{ roleLabel }}</p>
              </div>
            </div>

            <div class="py-1">
              <router-link
                to="/profile"
                class="flex items-center gap-3 px-4 py-2.5 text-slate-700 hover:bg-slate-100 hover:text-[#005EA3] transition font-bold"
                @click="menuOpen = false"
              >
                <span class="material-symbols-outlined text-[18px] text-slate-500">person</span>
                Hồ sơ cá nhân
              </router-link>

              <router-link
                v-if="userRole === 'SinhVien'"
                to="/scores"
                class="flex items-center gap-3 px-4 py-2.5 text-slate-700 hover:bg-slate-100 hover:text-[#005EA3] transition font-bold"
                @click="menuOpen = false"
              >
                <span class="material-symbols-outlined text-[18px] text-amber-500">grade</span>
                Điểm số & Bảng điểm
              </router-link>

              <router-link
                to="/change-password"
                class="flex items-center gap-3 px-4 py-2.5 text-slate-700 hover:bg-slate-100 hover:text-[#005EA3] transition font-bold"
                @click="menuOpen = false"
              >
                <span class="material-symbols-outlined text-[18px] text-slate-500">lock_reset</span>
                Đổi mật khẩu
              </router-link>
            </div>

            <div class="h-[1px] bg-slate-100 my-1 mx-3"></div>

            <button
              type="button"
              class="w-full flex items-center gap-3 px-4 py-2.5 text-rose-600 hover:bg-rose-50 transition font-bold text-left cursor-pointer"
              @click="askLogout"
            >
              <span class="material-symbols-outlined text-[18px] text-rose-500">logout</span>
              Đăng xuất
            </button>
          </div>
        </div>
      </div>
    </header>

    <!-- E15.2.2 xác nhận đăng xuất -->
    <div
      v-if="showLogoutConfirm"
      class="fixed inset-0 z-[100] flex items-center justify-center bg-slate-900/60 backdrop-blur-xs px-4"
      @click.self="showLogoutConfirm = false"
    >
      <div class="w-full max-w-[400px] bg-white rounded-2xl shadow-2xl p-6 border border-slate-200">
        <div class="flex items-center gap-3 mb-4">
          <div class="w-10 h-10 rounded-full bg-rose-100 text-rose-600 flex items-center justify-center">
            <span class="material-symbols-outlined text-[24px]">logout</span>
          </div>
          <h3 class="font-extrabold text-lg text-slate-900">Xác nhận Đăng xuất</h3>
        </div>
        <p class="text-xs text-slate-600 leading-relaxed font-medium mb-6">Bạn có chắc chắn muốn đăng xuất khỏi hệ thống IMS?</p>
        <div class="flex justify-end gap-3 text-xs font-bold">
          <button
            class="px-5 py-2.5 rounded-xl border border-slate-300 text-slate-700 hover:bg-slate-100 transition"
            @click="showLogoutConfirm = false"
          >
            Hủy
          </button>
          <button
            class="px-6 py-2.5 rounded-xl text-white shadow-md hover:bg-blue-800 transition"
            style="background-color:#005ea3;"
            @click="confirmLogout"
          >
            Đăng xuất
          </button>
        </div>
      </div>
    </div>

    <!-- E15.7 sắp hết phiên -->
    <div
      v-if="showTimeoutWarning"
      class="fixed inset-0 z-[120] flex items-center justify-center bg-slate-900/70 backdrop-blur-xs p-4 font-sans"
    >
      <div class="bg-white w-full max-w-[500px] rounded-2xl shadow-2xl overflow-hidden border border-slate-200 text-center">
        <div class="p-8 pb-4 flex flex-col items-center">
          <div class="w-20 h-20 bg-rose-100 rounded-full flex items-center justify-center mb-4 text-rose-600">
            <span class="material-symbols-outlined text-5xl">timer</span>
          </div>
          <h2 class="text-2xl font-extrabold text-slate-900 tracking-tight">Phiên đăng nhập sắp hết hạn</h2>
        </div>

        <div class="p-8 pt-0 space-y-6">
          <p class="text-xs text-slate-600 leading-relaxed font-medium px-4">
            Bạn đã không hoạt động trong một thời gian dài. Phiên làm việc của bạn sẽ kết thúc sau 5 phút nữa. Vui lòng chọn tiếp tục để giữ phiên đăng nhập.
          </p>

          <div class="bg-slate-50 rounded-2xl p-5 inline-flex items-center gap-6 border border-slate-200">
            <div class="relative w-16 h-16">
              <svg class="w-full h-full -rotate-90">
                <circle class="text-slate-200" cx="32" cy="32" fill="transparent" r="28" stroke="currentColor" stroke-width="4"></circle>
                <circle class="text-[#005EA3] transition-all duration-1000" cx="32" cy="32" fill="transparent" r="28" stroke="currentColor" stroke-dasharray="176" stroke-dashoffset="20" stroke-linecap="round" stroke-width="4"></circle>
              </svg>
              <div class="absolute inset-0 flex items-center justify-center">
                <span class="material-symbols-outlined text-[#005EA3] text-xl">hourglass_empty</span>
              </div>
            </div>
            <div class="text-left">
              <p class="text-[10px] font-extrabold text-slate-400 uppercase tracking-wider">THỜI GIAN CÒN LẠI</p>
              <p class="text-3xl font-black text-[#005EA3] tracking-tight font-mono">04:47</p>
            </div>
          </div>
        </div>

        <div class="p-6 bg-slate-50 border-t border-slate-200 flex flex-col sm:flex-row gap-3 text-xs font-bold">
          <button
            class="flex-1 px-6 py-3 rounded-xl border border-slate-300 text-slate-700 hover:bg-slate-100 transition flex items-center justify-center gap-2"
            @click="endSessionNow"
          >
            <span class="material-symbols-outlined text-[18px]">logout</span>
            Đăng xuất
          </button>
          <button
            class="flex-1 px-6 py-3 rounded-xl bg-[#005EA3] text-white shadow-md hover:bg-blue-800 transition flex items-center justify-center gap-2"
            @click="continueSession"
          >
            <span class="material-symbols-outlined text-[18px]">refresh</span>
            Tiếp tục làm việc
          </button>
        </div>
      </div>
    </div>

    <AppSidebar @toggle-collapse="isSidebarCollapsed = $event" />

    <main :class="['pt-16 min-h-screen flex flex-col justify-between transition-all duration-300 ease-in-out', isSidebarCollapsed ? 'lg:ml-20' : 'lg:ml-sidebar-width']">
      <div class="p-3 sm:p-6 lg:p-container-padding mx-auto w-full flex-1">
        <router-view />
      </div>

      <div class="w-full shrink-0">
        <AppFooter />
      </div>
    </main>

    <div
      v-if="realtimeToast"
      class="fixed bottom-6 right-6 z-[200] max-w-sm bg-slate-900/95 text-white p-4 rounded-2xl shadow-2xl border border-slate-700 backdrop-blur-md flex items-start gap-3.5 font-sans text-xs cursor-pointer hover:bg-slate-900 transition animate-bounce"
      @click="handleToastClick"
    >
      <div class="w-9 h-9 rounded-xl bg-blue-500/20 text-blue-400 flex items-center justify-center shrink-0 font-bold mt-0.5">
        <span class="material-symbols-outlined text-[20px]">{{ realtimeToast.icon || 'notifications_active' }}</span>
      </div>
      <div class="flex-1 space-y-1">
        <div class="flex items-center justify-between">
          <p class="font-extrabold text-white text-xs leading-snug">{{ realtimeToast.title }}</p>
          <span class="text-[10px] text-emerald-400 font-bold bg-emerald-950/80 px-2 py-0.5 rounded-full border border-emerald-500/30">Mới</span>
        </div>
        <p class="text-slate-300 text-[11px] leading-relaxed line-clamp-2">{{ realtimeToast.content }}</p>
        <p class="text-[10px] text-blue-300 font-semibold pt-0.5 flex items-center gap-1">
          <span>Bấm để chuyển tới liên kết</span>
          <span class="material-symbols-outlined text-[12px]">arrow_forward</span>
        </p>
      </div>
      <button @click.stop="realtimeToast = null" class="text-slate-400 hover:text-white font-bold p-1 cursor-pointer">✕</button>
    </div>
  </div>
</template>

<script setup>
  import { computed, ref, watch, onMounted, onUnmounted } from 'vue';
  import { useAuthStore } from '@/stores/auth';
  import { useRoute, useRouter } from 'vue-router';
  import AppFooter from '@/components/AppFooter.vue';
  import AppSidebar from '@/components/AppSidebar.vue';
  import {
    subscribeNotifications,
    markNotificationAsReadInDb,
    toggleNotificationReadInDb,
    markAllNotificationsAsReadInDb,
    clearAllNotificationsInDb
  } from '@/services/notificationService';

  const authStore = useAuthStore();
  const route = useRoute();
  const router = useRouter();
  const menuOpen = ref(false);
  const notificationsOpen = ref(false);
  const showLogoutConfirm = ref(false);
  const showTimeoutWarning = ref(false);
  const isSidebarCollapsed = ref(false);

  const notificationList = ref([]);
  const realtimeToast = ref(null);
  const activeFilterTab = ref('all'); // 'all' | 'unread'
  let unsubscribeNotifications = null;
  let previousNotificationIds = new Set();

  // Dynamic role check
  const userRole = computed(() => authStore.user?.vaiTro || 'SinhVien');

  const unreadCount = computed(() => notificationList.value.filter((n) => !n.read).length);

  const filteredNotificationList = computed(() => {
    if (activeFilterTab.value === 'unread') {
      return notificationList.value.filter((n) => !n.read);
    }
    return notificationList.value;
  });

  async function toggleItemRead(item, event) {
    if (event) event.stopPropagation();
    const newReadState = !item.read;
    item.read = newReadState;
    await toggleNotificationReadInDb(item.id, !newReadState);
  }

  // Poll notifications from .NET API
  function initRealtimeNotifications() {
    if (unsubscribeNotifications) {
      unsubscribeNotifications();
    }

    const userId = authStore.user?.mssv || authStore.user?.maGV || 'ALL';
    unsubscribeNotifications = subscribeNotifications(
      userRole.value,
      userId,
      (items) => {
        if (previousNotificationIds.size > 0) {
          const newUnread = items.filter((it) => !previousNotificationIds.has(it.id) && !it.read);
          if (newUnread.length > 0) {
            realtimeToast.value = newUnread[0];
            setTimeout(() => {
              if (realtimeToast.value === newUnread[0]) realtimeToast.value = null;
            }, 6000);
          }
        }
        previousNotificationIds = new Set(items.map((it) => it.id));
        notificationList.value = items;
      }
    );
  }

  watch(
    () => userRole.value,
    () => {
      initRealtimeNotifications();
    },
    { immediate: true }
  );

  function formatTimeAgo(dateStr) {
    if (!dateStr) return 'Vừa xong';
    const diffMs = Date.now() - new Date(dateStr).getTime();
    const diffMins = Math.floor(diffMs / (1000 * 60));
    if (diffMins < 1) return 'Vừa xong';
    if (diffMins < 60) return `${diffMins} phút trước`;
    const diffHours = Math.floor(diffMins / 60);
    if (diffHours < 24) return `${diffHours} giờ trước`;
    const diffDays = Math.floor(diffHours / 24);
    return `${diffDays} ngày trước`;
  }

  function toggleNotifications() {
    notificationsOpen.value = !notificationsOpen.value;
    menuOpen.value = false;
  }

  function toggleUserMenu() {
    menuOpen.value = !menuOpen.value;
    notificationsOpen.value = false;
  }

  async function markAllAsRead() {
    const userId = authStore.user?.mssv || authStore.user?.maGV || 'ALL';
    notificationList.value.forEach((n) => (n.read = true));
    await markAllNotificationsAsReadInDb(userRole.value, userId);
  }

  async function clearAllNotifications() {
    const userId = authStore.user?.mssv || authStore.user?.maGV || 'ALL';
    notificationList.value = [];
    await clearAllNotificationsInDb(userRole.value, userId);
  }

  async function handleNotificationClick(item) {
    item.read = true;
    notificationsOpen.value = false;
    await markNotificationAsReadInDb(item.id);
    if (item.link) {
      router.push(item.link);
    }
  }

  function handleToastClick() {
    if (realtimeToast.value) {
      if (realtimeToast.value.link) {
        router.push(realtimeToast.value.link);
      }
      realtimeToast.value = null;
    }
  }

  function handleGlobalClick() {
    notificationsOpen.value = false;
    menuOpen.value = false;
  }

  function toggleMobileSidebar() {
    window.dispatchEvent(new CustomEvent('toggle-mobile-sidebar'));
  }

  // E15.7: 60 phút không hoạt động → cảnh báo; sau thêm 5 phút → tự đăng xuất
  const INACTIVITY_MS = 60 * 60 * 1000;
  const GRACE_MS = 5 * 60 * 1000;
  let warnTimer = null;
  let logoutTimer = null;

  function clearTimers() {
    clearTimeout(warnTimer);
    clearTimeout(logoutTimer);
  }
  function scheduleTimers() {
    clearTimers();
    warnTimer = setTimeout(() => {
      showTimeoutWarning.value = true;
      logoutTimer = setTimeout(() => {
        showTimeoutWarning.value = false;
        authStore.logout({ redirect: false });
        router.replace('/login?expired=1');
      }, GRACE_MS);
    }, INACTIVITY_MS);
  }
  function onActivity() {
    if (showTimeoutWarning.value) return; // đang chờ người dùng quyết định
    scheduleTimers();
  }
  function continueSession() {
    showTimeoutWarning.value = false;
    scheduleTimers();
  }
  function endSessionNow() {
    showTimeoutWarning.value = false;
    clearTimers();
    authStore.logout();
  }

  const roleLabels = {
    SinhVien: 'Sinh viên',
    GiangVien: 'Giảng viên',
    GiaoVu: 'Giáo vụ khoa',
    Admin: 'Quản trị viên'
  };
  const roleLabel = computed(() => roleLabels[userRole.value] || 'Người dùng');

  const askLogout = () => {
    menuOpen.value = false;
    showLogoutConfirm.value = true;
  };

  const confirmLogout = () => {
    showLogoutConfirm.value = false;
    authStore.logout();
  };

  // E15.2.6 đăng xuất đồng bộ giữa các tab + E15.7 hết phiên
  const onStorage = (e) => {
    if (e.key === 'auth-event' && e.newValue?.startsWith('logout')) {
      authStore.clearLocal();
      router.replace('/login');
    }
  };
  const onSessionExpired = () => {
    router.replace('/login?expired=1');
  };
  const activityEvents = ['mousedown', 'keydown', 'scroll', 'touchstart'];
  onMounted(() => {
    window.addEventListener('storage', onStorage);
    window.addEventListener('auth:session-expired', onSessionExpired);
    window.addEventListener('click', handleGlobalClick);
    activityEvents.forEach((ev) => window.addEventListener(ev, onActivity, { passive: true }));
    scheduleTimers();
    initRealtimeNotifications();
  });
  onUnmounted(() => {
    if (unsubscribeNotifications) unsubscribeNotifications();
    window.removeEventListener('storage', onStorage);
    window.removeEventListener('auth:session-expired', onSessionExpired);
    window.removeEventListener('click', handleGlobalClick);
    activityEvents.forEach((ev) => window.removeEventListener(ev, onActivity));
    clearTimers();
  });
</script>
