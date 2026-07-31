<template>
  <div>
    <!-- Mobile Backdrop Overlay -->
    <Transition name="fade">
      <div
        v-if="isMobileOpen"
        class="fixed inset-0 bg-slate-900/50 backdrop-blur-xs z-40 lg:hidden"
        @click="closeMobile"
      />
    </Transition>

    <!-- Sidebar Container -->
    <aside
      class="fixed top-16 left-0 h-[calc(100vh-64px)] z-40 bg-white border-r border-slate-200 flex flex-col transition-all duration-300 ease-in-out shadow-xs font-sans select-none"
      :class="[
        // Mobile behavior
        isMobileOpen ? 'translate-x-0 w-64' : '-translate-x-full lg:translate-x-0',
        // Desktop collapse behavior
        isCollapsed ? 'lg:w-20' : 'lg:w-64'
      ]"
    >
      <!-- Class Context / Header Section -->
      <div
        class="p-4 border-b border-slate-100 flex items-center justify-between transition-all"
        :class="isCollapsed ? 'lg:justify-center lg:px-2' : 'px-5'"
      >
        <div v-if="!isCollapsed || isMobileOpen" class="flex flex-col min-w-0 flex-1">
          <div class="flex items-center gap-2">
            <span class="w-2.5 h-2.5 rounded-full bg-emerald-500 shrink-0 animate-pulse" />
            <span class="font-bold text-slate-800 text-sm truncate">Lớp Thực tập K64</span>
          </div>
          <p class="text-[11px] font-medium text-slate-500 mt-0.5 pl-4">Học kỳ 1 - 2024</p>
        </div>

        <div v-else class="flex justify-center w-full" title="Lớp Thực tập K64">
          <div class="w-10 h-10 rounded-xl bg-blue-50 text-blue-800 flex items-center justify-center font-extrabold text-xs border border-blue-100">
            K64
          </div>
        </div>

        <!-- Toggle Collapse Button (Desktop) -->
        <button
          class="hidden lg:flex w-7 h-7 rounded-lg text-slate-400 hover:text-blue-700 hover:bg-slate-100 transition-colors items-center justify-center shrink-0 ml-1"
          :title="isCollapsed ? 'Mở rộng menu' : 'Thu gọn menu'"
          @click="toggleCollapse"
        >
          <span class="material-symbols-outlined text-[20px]">
            {{ isCollapsed ? 'chevron_right' : 'chevron_left' }}
          </span>
        </button>
      </div>

      <!-- Navigation Links -->
      <nav class="flex-1 px-3 py-4 space-y-1.5 overflow-y-auto custom-scrollbar">
        <div v-for="item in navItems" :key="item.id" class="space-y-1">
          <!-- Main Nav Item -->
          <router-link
            :to="item.path"
            class="w-full group relative flex items-center gap-3 px-3.5 py-2.5 rounded-xl text-sm font-medium transition-all duration-200 text-left"
            :class="[
              isItemActive(item)
                ? 'bg-[#005EA3] text-white font-bold shadow-sm'
                : 'text-slate-700 hover:bg-slate-100/80 hover:text-slate-900',
              isCollapsed ? 'lg:justify-center lg:px-0' : ''
            ]"
            @click="closeMobile"
          >
            <!-- Icon -->
            <span
              class="material-symbols-outlined text-[22px] transition-transform group-hover:scale-110 shrink-0"
              :class="isItemActive(item) ? 'text-white' : 'text-slate-500 group-hover:text-slate-700'"
              :style="isItemActive(item) ? { fontVariationSettings: `'FILL' 1` } : {}"
            >
              {{ item.icon }}
            </span>

            <!-- Label -->
            <span
              v-if="!isCollapsed || isMobileOpen"
              class="truncate flex-1 text-[14px]"
            >
              {{ item.label }}
            </span>

            <!-- Badge / Counter -->
            <span
              v-if="item.badge && (!isCollapsed || isMobileOpen)"
              class="ml-auto px-2 py-0.5 rounded-full text-[11px] font-bold transition-all"
              :class="isItemActive(item) ? 'bg-white/20 text-white' : (item.badgeClass || 'bg-blue-100 text-blue-800')"
            >
              {{ item.badge }}
            </span>

            <!-- Tooltip for collapsed mode -->
            <div
              v-if="isCollapsed && !isMobileOpen"
              class="absolute left-full ml-3 px-3 py-1.5 bg-slate-800 text-white text-xs font-semibold rounded-lg shadow-xl whitespace-nowrap opacity-0 group-hover:opacity-100 pointer-events-none transition-opacity z-50"
            >
              {{ item.label }}
            </div>
          </router-link>
        </div>
      </nav>

      <!-- Footer Quick Status -->
      <div class="p-3 border-t border-slate-200 bg-slate-50/50 space-y-2">
        <div v-if="!isCollapsed || isMobileOpen" class="flex items-center justify-between px-2 py-1 text-xs text-slate-500">
          <span class="flex items-center gap-1.5">
            <span class="material-symbols-outlined text-[16px] text-blue-600">verified</span>
            IMS Version 2.4
          </span>
        </div>

        <button
          v-else
          class="w-full py-2 flex justify-center text-slate-400 hover:text-blue-700"
          title="Thông tin hệ thống IMS"
        >
          <span class="material-symbols-outlined text-[20px]">info</span>
        </button>
      </div>
    </aside>

    <!-- Mobile Floating Toggle Button -->
    <button
      class="lg:hidden fixed bottom-6 left-5 z-50 w-12 h-12 rounded-full bg-[#005EA3] text-white shadow-xl flex items-center justify-center hover:bg-blue-800 transition-transform active:scale-95"
      aria-label="Toggle Navigation"
      @click="isMobileOpen = !isMobileOpen"
    >
      <span class="material-symbols-outlined text-[24px]">
        {{ isMobileOpen ? 'close' : 'menu' }}
      </span>
    </button>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted, onUnmounted } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import { useAuthStore } from '@/stores/auth';

  const emit = defineEmits(['toggle-collapse']);

  const props = defineProps({
    classId: {
      type: [String, Number],
      default: '1'
    }
  });

  const route = useRoute();
  const router = useRouter();
  const authStore = useAuthStore();

  const isCollapsed = ref(false);
  const isMobileOpen = ref(false);

  function handleToggleMobileSidebar() {
    isMobileOpen.value = !isMobileOpen.value;
  }

  onMounted(() => {
    window.addEventListener('toggle-mobile-sidebar', handleToggleMobileSidebar);
  });

  onUnmounted(() => {
    window.removeEventListener('toggle-mobile-sidebar', handleToggleMobileSidebar);
  });

  const activeClassId = computed(() => route.params.id || props.classId || '1');
  const userRole = computed(() => authStore.user?.vaiTro || 'SinhVien');

  // Direct flat navigation based on user role
  const navItems = computed(() => {
    if (userRole.value === 'SinhVien') {
      return [
        {
          id: 'dashboard',
          label: 'Dashboard',
          icon: 'dashboard',
          path: '/dashboard'
        },
        {
          id: 'progress',
          label: 'Tiến trình của tôi',
          icon: 'trending_up',
          path: '/student/progress'
        },
        {
          id: 'student-tasks',
          label: 'Nhiệm vụ',
          icon: 'task',
          path: '/student/tasks'
        },
        {
          id: 'scores',
          label: 'Điểm số',
          icon: 'grade',
          path: '/scores'
        },
        {
          id: 'documents',
          label: 'Kho tài liệu & biểu mẫu',
          icon: 'folder_shared',
          path: '/documents'
        }
      ];
    }

    if (userRole.value === 'Admin') {
      return [
        { id: 'dashboard', label: 'Dashboard', icon: 'dashboard', path: '/dashboard' },
        { id: 'accounts', label: 'Quản lý tài khoản', icon: 'manage_accounts', path: '/admin/accounts' },
        { id: 'account-mgmt', label: 'Phân quyền & Vai trò', icon: 'shield', path: '/admin/account-management' },
        { id: 'requests', label: 'Yêu cầu cấp tài khoản', icon: 'pending_actions', path: '/pending-requests' },
        { id: 'documents', label: 'Kho tài liệu', icon: 'folder_shared', path: '/documents' }
      ];
    }

    if (userRole.value === 'GiaoVu') {
      return [
        { id: 'dashboard', label: 'Dashboard', icon: 'dashboard', path: '/dashboard' },
        { id: 'requests', label: 'Yêu cầu của tôi', icon: 'assignment', path: '/giaovu-requests' },
        { id: 'documents', label: 'Kho tài liệu', icon: 'folder_shared', path: '/documents' }
      ];
    }

    // Default for GiangVien — menu lớp theo mockup
    return [
      {
        id: 'dashboard',
        label: 'Dashboard',
        icon: 'dashboard',
        path: `/teacher/classes/${activeClassId.value}/dashboard`
      },
      {
        id: 'students',
        label: 'Sinh viên',
        icon: 'groups',
        path: `/teacher/classes/${activeClassId.value}/students`
      },
      {
        id: 'work',
        label: 'Công việc',
        icon: 'assignment',
        path: `/teacher/classes/${activeClassId.value}/diaries`,
        matchPaths: [
          `/teacher/classes/${activeClassId.value}/diaries`,
          `/teacher/classes/${activeClassId.value}/tasks`,
          `/teacher/classes/${activeClassId.value}/topics`
        ]
      },
      {
        id: 'evaluation',
        label: 'Đánh giá',
        icon: 'fact_check',
        path: `/teacher/classes/${activeClassId.value}/grading`
      },
      {
        id: 'documents',
        label: 'Kho tài liệu & biểu mẫu',
        icon: 'folder_shared',
        path: '/documents'
      },
      {
        id: 'settings',
        label: 'Cấu hình lớp',
        icon: 'settings',
        path: `/teacher/classes/${activeClassId.value}/settings`
      }
    ];
  });

  function isPathActive(path) {
    if (!path) return false;
    return route.path === path || route.path.startsWith(path + '/');
  }

  function isItemActive(item) {
    if (item.matchPaths?.length) {
      return item.matchPaths.some((p) => route.path === p || route.path.startsWith(p + '/'));
    }
    return item.path && isPathActive(item.path);
  }

  function toggleCollapse() {
    isCollapsed.value = !isCollapsed.value;
    emit('toggle-collapse', isCollapsed.value);
  }

  function closeMobile() {
    isMobileOpen.value = false;
  }

  function showClassSettings() {
    router.push('/teacher/classes');
    closeMobile();
  }
</script>

<style scoped>
  .custom-scrollbar::-webkit-scrollbar {
    width: 4px;
  }
  .custom-scrollbar::-webkit-scrollbar-track {
    background: transparent;
  }
  .custom-scrollbar::-webkit-scrollbar-thumb {
    background: #cbd5e1;
    border-radius: 9999px;
  }
  .custom-scrollbar::-webkit-scrollbar-thumb:hover {
    background: #94a3b8;
  }

  .fade-enter-active,
  .fade-leave-active {
    transition: opacity 0.2s ease;
  }
  .fade-enter-from,
  .fade-leave-to {
    opacity: 0;
  }
</style>
