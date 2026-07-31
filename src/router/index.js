import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

import MainLayout from '@/layouts/MainLayout.vue';
import Login from '@/views/auth/Login.vue';
import Register from '@/views/Register.vue';
import Activate from '@/views/Activate.vue';
import ForgotPassword from '@/views/ForgotPassword.vue';
import ResetPassword from '@/views/ResetPassword.vue';
import ChangePassword from '@/views/ChangePassword.vue';

import Dashboard from '@/views/Dashboard.vue';
import Scores from '@/views/Scores.vue';
import Diaries from '@/views/Diaries.vue';
import Profile from '@/views/Profile.vue';
import Documents from '@/views/Documents.vue';
import PendingRequests from '@/views/PendingRequests.vue';
import GiaoVuRequests from '@/views/GiaoVuRequests.vue';
import AdminAccounts from '@/views/AdminAccounts.vue';
import AccountManagement from '@/views/admin/AccountManagement.vue';
import OverviewAnalytics from '@/views/OverviewAnalytics.vue';

import StudentClasses from '@/views/student/StudentClasses.vue';
import StudentProgress from '@/views/student/StudentProgress.vue';
import StudentTasks from '@/views/student/StudentTasks.vue';
import StudentTaskDetail from '@/views/student/StudentTaskDetail.vue';
import StudentCalendar from '@/views/student/StudentCalendar.vue';

import MyClasses from '@/views/teacher/MyClasses.vue';
import ClassDashboard from '@/views/teacher/ClassDashboard.vue';
import ClassStudents from '@/views/teacher/ClassStudents.vue';
import ClassGrading from '@/views/teacher/ClassGrading.vue';
import ClassTasks from '@/views/teacher/ClassTasks.vue';
import ClassSettings from '@/views/teacher/ClassSettings.vue';
import ClassDiaries from '@/views/teacher/ClassDiaries.vue';
import ClassTopics from '@/views/teacher/ClassTopics.vue';
import ClassAttendance from '@/views/teacher/ClassAttendance.vue';
import ClassViolations from '@/views/teacher/ClassViolations.vue';
import TeacherSchedule from '@/views/teacher/TeacherSchedule.vue';
import TeacherStudents from '@/views/TeacherStudents.vue';

const routes = [
  { path: '/login', name: 'Login', component: Login, meta: { guestOnly: true } },
  { path: '/register', name: 'Register', component: Register, meta: { guestOnly: true } },
  { path: '/activate', name: 'Activate', component: Activate },
  { path: '/forgot-password', name: 'ForgotPassword', component: ForgotPassword, meta: { guestOnly: true } },
  { path: '/reset-password', name: 'ResetPassword', component: ResetPassword, meta: { guestOnly: true } },

  {
    path: '/',
    component: MainLayout,
    meta: { requiresAuth: true },
    children: [
      { path: '', redirect: '/dashboard' },
      { path: 'dashboard', name: 'Dashboard', component: Dashboard },
      { path: 'profile', name: 'Profile', component: Profile },
      { path: 'documents', name: 'Documents', component: Documents },
      { path: 'change-password', name: 'ChangePassword', component: ChangePassword },

      // SinhVien routes
      { path: 'scores', name: 'Scores', component: Scores, meta: { roles: ['SinhVien'] } },
      { path: 'diaries', name: 'Diaries', component: Diaries, meta: { roles: ['SinhVien'] } },
      { path: 'student/classes', name: 'StudentClasses', component: StudentClasses, meta: { roles: ['SinhVien'] } },
      { path: 'student/progress', name: 'StudentProgress', component: StudentProgress, meta: { roles: ['SinhVien'] } },
      { path: 'student/tasks', name: 'StudentTasks', component: StudentTasks, meta: { roles: ['SinhVien'] } },
      { path: 'student/tasks/:id', name: 'StudentTaskDetail', component: StudentTaskDetail, meta: { roles: ['SinhVien'] } },
      { path: 'student/calendar', name: 'StudentCalendar', component: StudentCalendar, meta: { roles: ['SinhVien'] } },
      { path: 'calendar', redirect: '/student/calendar' },
      { path: 'tasks', redirect: '/student/tasks' },
      { path: 'my-progress', redirect: '/student/progress' },

      // Admin routes
      { path: 'pending-requests', name: 'PendingRequests', component: PendingRequests, meta: { roles: ['Admin'] } },
      { path: 'admin/accounts', name: 'AdminAccounts', component: AdminAccounts, meta: { roles: ['Admin'] } },
      { path: 'admin/account-management', name: 'AccountManagement', component: AccountManagement, meta: { roles: ['Admin'] } },

      // Shared Overview Analytics route for GiangVien, GiaoVu & Admin
      { path: 'overview-analytics', name: 'OverviewAnalytics', component: OverviewAnalytics, meta: { roles: ['GiangVien', 'GiaoVu', 'Admin'] } },

      // GiaoVu routes
      { path: 'giaovu-requests', name: 'GiaoVuRequests', component: GiaoVuRequests, meta: { roles: ['GiaoVu'] } },

      // GiangVien routes
      { path: 'teacher/classes', name: 'TeacherClasses', component: MyClasses, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/classes/:id/dashboard', name: 'ClassDashboard', component: ClassDashboard, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/classes/:id/students', name: 'ClassStudents', component: ClassStudents, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/classes/:id/tasks', name: 'ClassTasks', component: ClassTasks, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/classes/:id/diaries', name: 'ClassDiaries', component: ClassDiaries, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/classes/:id/topics', name: 'ClassTopics', component: ClassTopics, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/classes/:id/grading', name: 'ClassGrading', component: ClassGrading, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/classes/:id/attendance', name: 'ClassAttendance', component: ClassAttendance, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/classes/:id/violations', name: 'ClassViolations', component: ClassViolations, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/classes/:id/settings', name: 'ClassSettings', component: ClassSettings, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/schedule', name: 'TeacherSchedule', component: TeacherSchedule, meta: { roles: ['GiangVien'] } },
      { path: 'teacher/students', name: 'TeacherStudents', component: TeacherStudents, meta: { roles: ['GiangVien'] } }
    ]
  },
  { path: '/:pathMatch(.*)*', redirect: '/dashboard' }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from) => {
  const authStore = useAuthStore();

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return '/login';
  }

  if (to.meta.guestOnly && authStore.isAuthenticated) {
    return '/dashboard';
  }

  // Role-based access control guard
  if (to.meta.roles && to.meta.roles.length > 0) {
    const userRole = authStore.userRole || authStore.user?.vaiTro;
    if (!userRole || !to.meta.roles.includes(userRole)) {
      return '/dashboard';
    }
  }
});

export default router;
