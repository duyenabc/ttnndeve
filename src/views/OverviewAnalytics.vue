<template>
  <div class="ims-scope space-y-6 pb-12 font-sans">
    <!-- Header & Filter Bar -->
    <div class="bg-white rounded-2xl border border-slate-200 p-6 shadow-xs space-y-4">
      <div class="flex flex-wrap items-center justify-between gap-4">
        <div>
          <div class="flex items-center gap-2.5">
            <span class="w-10 h-10 rounded-2xl bg-blue-50 text-[#005EA3] flex items-center justify-center shrink-0 shadow-xs border border-blue-100">
              <span class="material-symbols-outlined text-[24px]">analytics</span>
            </span>
            <div>
              <div class="flex items-center gap-2">
                <h1 class="font-extrabold text-2xl text-slate-900 tracking-tight">Thống Kê Tổng Quan Thực Tập</h1>
                <span class="px-2.5 py-0.5 rounded-full bg-blue-100 text-[#005EA3] font-bold text-[11px] uppercase tracking-wider border border-blue-200">
                  Recharts Analytics
                </span>
              </div>
              <p class="text-xs text-slate-500 font-medium mt-0.5">
                Báo cáo phân tích tiến độ duyệt đề tài, cơ cấu điểm số và đánh giá toàn diện dành cho Giảng viên & Giáo vụ
              </p>
            </div>
          </div>
        </div>

        <!-- Role Badge & Action Buttons -->
        <div class="flex items-center gap-3">
          <div class="bg-slate-100 px-3 py-1.5 rounded-xl text-xs font-bold text-slate-700 flex items-center gap-2 border border-slate-200">
            <span class="w-2 h-2 rounded-full bg-emerald-500 animate-pulse"></span>
            <span>Vai trò: {{ userRoleLabel }}</span>
          </div>

          <button
            @click="exportReport"
            class="px-4 py-2 bg-[#005EA3] text-white hover:bg-blue-800 rounded-xl font-bold text-xs flex items-center gap-2 shadow-xs transition cursor-pointer"
          >
            <span class="material-symbols-outlined text-[18px]">download</span>
            Xuất Báo Cáo Excel
          </button>
        </div>
      </div>

      <!-- Filters Row -->
      <div class="pt-3 border-t border-slate-100 flex flex-wrap items-center justify-between gap-3 text-xs">
        <div class="flex flex-wrap items-center gap-3">
          <div class="flex items-center gap-2">
            <label class="font-bold text-slate-600 shrink-0">Học kỳ / Năm học:</label>
            <select
              v-model="selectedSemester"
              @change="updateChartData"
              class="px-3 py-1.5 bg-slate-50 border border-slate-200 rounded-xl font-semibold text-slate-800 focus:bg-white focus:ring-2 focus:ring-[#005EA3] focus:outline-none transition"
            >
              <option value="all">Tất cả các kỳ thực tập</option>
              <option value="hk1-2026">HK1 - Năm học 2026-2027</option>
              <option value="hk2-2025">HK2 - Năm học 2025-2026</option>
            </select>
          </div>

          <div class="flex items-center gap-2">
            <label class="font-bold text-slate-600 shrink-0">Lớp thực tập:</label>
            <select
              v-model="selectedClass"
              @change="updateChartData"
              class="px-3 py-1.5 bg-slate-50 border border-slate-200 rounded-xl font-semibold text-slate-800 focus:bg-white focus:ring-2 focus:ring-[#005EA3] focus:outline-none transition"
            >
              <option value="all">Tất cả lớp phụ trách (3 lớp)</option>
              <option value="mis2012">MIS2012_01 - Lớp Hệ thống thông tin</option>
              <option value="khdl3011">KHDL3011_02 - Lớp Khoa học dữ liệu</option>
              <option value="tmdt1001">TMDT1001_01 - Lớp Thương mại điện tử</option>
            </select>
          </div>
        </div>

        <button
          @click="refreshData"
          class="px-3 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold rounded-xl flex items-center gap-1.5 transition cursor-pointer"
        >
          <span class="material-symbols-outlined text-[16px]" :class="{ 'animate-spin': isRefreshing }">refresh</span>
          Tải lại dữ liệu
        </button>
      </div>
    </div>

    <!-- Executive KPI Metric Cards -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
      <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs space-y-3">
        <div class="flex justify-between items-start">
          <span class="material-symbols-outlined text-blue-700 p-2.5 bg-blue-50 rounded-xl text-[22px]">groups</span>
          <span class="text-blue-700 font-bold text-xs bg-blue-50 px-2 py-0.5 rounded-md">100% active</span>
        </div>
        <div>
          <p class="text-xs font-bold text-slate-500 uppercase tracking-wider">Tổng Sinh Viên</p>
          <h2 class="text-3xl font-black text-slate-900 mt-1">{{ filteredMetrics.totalStudents }} <span class="text-sm font-normal text-slate-400">SV</span></h2>
        </div>
        <p class="text-[11px] text-slate-500 font-medium">Đã phân công Giảng viên hướng dẫn</p>
      </div>

      <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs space-y-3">
        <div class="flex justify-between items-start">
          <span class="material-symbols-outlined text-emerald-600 p-2.5 bg-emerald-50 rounded-xl text-[22px]">verified</span>
          <span class="text-emerald-700 font-bold text-xs bg-emerald-50 px-2 py-0.5 rounded-md">+5.2% vs tuần trước</span>
        </div>
        <div>
          <p class="text-xs font-bold text-slate-500 uppercase tracking-wider">Tỷ Lệ Đề Tài Đã Duyệt</p>
          <h2 class="text-3xl font-black text-emerald-600 mt-1">{{ filteredMetrics.approvedRate }}%</h2>
        </div>
        <div class="w-full bg-slate-100 h-2 rounded-full overflow-hidden">
          <div class="bg-emerald-500 h-full rounded-full transition-all duration-500" :style="{ width: filteredMetrics.approvedRate + '%' }"></div>
        </div>
      </div>

      <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs space-y-3">
        <div class="flex justify-between items-start">
          <span class="material-symbols-outlined text-amber-600 p-2.5 bg-amber-50 rounded-xl text-[22px]">grade</span>
          <span class="text-amber-800 font-bold text-xs bg-amber-50 px-2 py-0.5 rounded-md">Xếp loại Giỏi</span>
        </div>
        <div>
          <p class="text-xs font-bold text-slate-500 uppercase tracking-wider">Điểm Trung Bình</p>
          <h2 class="text-3xl font-black text-slate-900 mt-1">{{ filteredMetrics.averageGrade }} <span class="text-sm font-normal text-slate-400">/ 10</span></h2>
        </div>
        <p class="text-[11px] text-slate-500 font-medium">Trung bình điểm hướng dẫn & phản biện</p>
      </div>

      <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs space-y-3">
        <div class="flex justify-between items-start">
          <span class="material-symbols-outlined text-purple-600 p-2.5 bg-purple-50 rounded-xl text-[22px]">workspace_premium</span>
          <span class="text-purple-700 font-bold text-xs bg-purple-50 px-2 py-0.5 rounded-md">Đạt yêu cầu</span>
        </div>
        <div>
          <p class="text-xs font-bold text-slate-500 uppercase tracking-wider">Tỷ Lệ Sinh Viên Đạt</p>
          <h2 class="text-3xl font-black text-purple-700 mt-1">{{ filteredMetrics.passRate }}%</h2>
        </div>
        <p class="text-[11px] text-slate-500 font-medium">{{ filteredMetrics.passedCount }} / {{ filteredMetrics.totalStudents }} SV đạt điểm &gt;= 5.0</p>
      </div>
    </div>

    <!-- Recharts Main Visualizations Grid -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Chart 1: Topic Approval Status (Recharts Pie & Bar) -->
      <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-xs space-y-4 flex flex-col justify-between">
        <div class="flex items-center justify-between">
          <div>
            <h3 class="font-extrabold text-base text-slate-900 flex items-center gap-2">
              <span class="material-symbols-outlined text-emerald-600 text-[20px]">donut_small</span>
              Biểu Đồ Trạng Thái Duyệt Đề Tài
            </h3>
            <p class="text-xs text-slate-500">Thống kê tỷ lệ phê duyệt đề tài đăng ký của sinh viên</p>
          </div>

          <div class="flex items-center bg-slate-100 p-1 rounded-xl gap-1">
            <button
              @click="topicChartType = 'pie'"
              class="px-2.5 py-1 rounded-lg text-xs font-bold transition cursor-pointer"
              :class="topicChartType === 'pie' ? 'bg-white text-slate-900 shadow-xs' : 'text-slate-500 hover:text-slate-800'"
            >
              Donut Chart
            </button>
            <button
              @click="topicChartType = 'bar'"
              class="px-2.5 py-1 rounded-lg text-xs font-bold transition cursor-pointer"
              :class="topicChartType === 'bar' ? 'bg-white text-slate-900 shadow-xs' : 'text-slate-500 hover:text-slate-800'"
            >
              Bar Chart
            </button>
          </div>
        </div>

        <!-- Container for Recharts Render -->
        <div ref="topicChartContainer" class="w-full h-80 relative flex items-center justify-center">
          <div v-if="!topicChartMounted" class="text-slate-400 text-xs font-medium flex items-center gap-2">
            <span class="material-symbols-outlined animate-spin">sync</span>
            Đang tải biểu đồ Recharts...
          </div>
        </div>

        <!-- Legend Pills -->
        <div class="grid grid-cols-2 sm:grid-cols-3 gap-2 pt-2 border-t border-slate-100 text-xs font-semibold">
          <div v-for="item in topicStatusData" :key="item.name" class="flex items-center gap-2 bg-slate-50 p-2 rounded-xl border border-slate-100">
            <span class="w-3 h-3 rounded-full shrink-0" :style="{ backgroundColor: item.color }"></span>
            <div class="min-w-0">
              <p class="text-slate-600 text-[11px] truncate">{{ item.name }}</p>
              <p class="font-bold text-slate-900 text-xs">{{ item.value }} SV ({{ ((item.value / filteredMetrics.totalStudents) * 100).toFixed(1) }}%)</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Chart 2: Student Grade Distribution (Recharts Bar & Composed) -->
      <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-xs space-y-4 flex flex-col justify-between">
        <div class="flex items-center justify-between">
          <div>
            <h3 class="font-extrabold text-base text-slate-900 flex items-center gap-2">
              <span class="material-symbols-outlined text-blue-600 text-[20px]">bar_chart</span>
              Biểu Đồ Phân Bố Điểm Số Sinh Viên
            </h3>
            <p class="text-xs text-slate-500">Phân hạng kết quả đánh giá thực tập theo thang điểm 10</p>
          </div>

          <span class="px-3 py-1 bg-amber-50 text-amber-800 font-bold text-xs rounded-xl border border-amber-200">
            Thang điểm 10
          </span>
        </div>

        <!-- Container for Recharts Grade Render -->
        <div ref="gradeChartContainer" class="w-full h-80 relative flex items-center justify-center">
          <div v-if="!gradeChartMounted" class="text-slate-400 text-xs font-medium flex items-center gap-2">
            <span class="material-symbols-outlined animate-spin">sync</span>
            Đang tải biểu đồ Recharts...
          </div>
        </div>

        <!-- Grade Legend Summary -->
        <div class="grid grid-cols-2 sm:grid-cols-5 gap-2 pt-2 border-t border-slate-100 text-xs font-semibold">
          <div v-for="item in gradeDistributionData" :key="item.range" class="text-center bg-slate-50 p-2 rounded-xl border border-slate-100">
            <p class="text-[10px] text-slate-500 uppercase font-bold">{{ item.range }}</p>
            <p class="font-extrabold text-slate-900 text-sm mt-0.5" :style="{ color: item.color }">{{ item.count }} SV</p>
            <p class="text-[10px] text-slate-400">{{ ((item.count / filteredMetrics.totalStudents) * 100).toFixed(0) }}%</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Secondary Visualizations: Weekly Journal Submission Trend Chart (Recharts Area/Line) -->
    <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-xs space-y-4">
      <div class="flex flex-wrap items-center justify-between gap-4">
        <div>
          <h3 class="font-extrabold text-base text-slate-900 flex items-center gap-2">
            <span class="material-symbols-outlined text-purple-600 text-[20px]">show_chart</span>
            Xu Hướng Nộp Báo Cáo & Nhận Xét Hàng Tuần (Tuần 1 - Tuần 10)
          </h3>
          <p class="text-xs text-slate-500">So sánh tỷ lệ nộp nhật ký đúng hạn của sinh viên và tỷ lệ phản hồi của Giảng viên qua từng tuần</p>
        </div>

        <div class="flex items-center gap-4 text-xs font-bold">
          <div class="flex items-center gap-1.5">
            <span class="w-3 h-3 rounded-xs bg-[#005EA3]"></span>
            <span class="text-slate-700">Tỷ lệ nộp đúng hạn (%)</span>
          </div>
          <div class="flex items-center gap-1.5">
            <span class="w-3 h-3 rounded-xs bg-emerald-500"></span>
            <span class="text-slate-700">Tỷ lệ GV đã nhận xét (%)</span>
          </div>
        </div>
      </div>

      <!-- Container for Recharts Weekly Trend Render -->
      <div ref="weeklyChartContainer" class="w-full h-72 relative flex items-center justify-center">
        <div v-if="!weeklyChartMounted" class="text-slate-400 text-xs font-medium flex items-center gap-2">
          <span class="material-symbols-outlined animate-spin">sync</span>
          Đang tải biểu đồ Recharts...
        </div>
      </div>
    </div>

    <!-- Detailed Student Topic & Grade Data Table -->
    <div class="bg-white rounded-2xl border border-slate-200 shadow-xs overflow-hidden">
      <div class="p-5 border-b border-slate-100 flex flex-wrap items-center justify-between gap-4">
        <div>
          <h3 class="font-extrabold text-slate-900 text-base flex items-center gap-2">
            <span class="material-symbols-outlined text-blue-700 text-[20px]">table_chart</span>
            Danh Sách Chi Tiết Đề Tài & Điểm Đánh Giá Sinh Viên
          </h3>
          <p class="text-xs text-slate-500">Bảng tra cứu trực tiếp thông tin đề tài thực tập, trạng thái phê duyệt và kết quả chấm điểm</p>
        </div>

        <div class="flex flex-wrap items-center gap-3">
          <!-- Search box -->
          <div class="relative w-64">
            <span class="material-symbols-outlined absolute left-3 top-2.5 text-slate-400 text-[18px]">search</span>
            <input
              v-model="searchQuery"
              type="text"
              placeholder="Tìm kiếm MSSV, Họ tên, Đề tài..."
              class="w-full pl-9 pr-3 py-2 bg-slate-50 border border-slate-200 rounded-xl text-xs font-medium text-slate-800 focus:bg-white focus:ring-2 focus:ring-[#005EA3] focus:outline-none"
            />
          </div>

          <!-- Status Filter -->
          <select
            v-model="statusFilter"
            class="px-3 py-2 bg-slate-50 border border-slate-200 rounded-xl text-xs font-medium text-slate-800 focus:bg-white focus:ring-2 focus:ring-[#005EA3] focus:outline-none"
          >
            <option value="all">Tất cả trạng thái</option>
            <option value="Đã duyệt">Đã duyệt</option>
            <option value="Chờ duyệt">Chờ duyệt</option>
            <option value="Yêu cầu chỉnh sửa">Yêu cầu chỉnh sửa</option>
            <option value="Từ chối">Từ chối</option>
          </select>
        </div>
      </div>

      <!-- Table -->
      <div class="overflow-x-auto">
        <table class="w-full text-left text-xs border-collapse">
          <thead>
            <tr class="bg-slate-50 text-slate-600 font-extrabold uppercase tracking-wider text-[11px] border-b border-slate-200">
              <th class="py-3.5 px-4 text-center">STT</th>
              <th class="py-3.5 px-4">Sinh viên</th>
              <th class="py-3.5 px-4">Lớp SH</th>
              <th class="py-3.5 px-4">Tên đề tài thực tập</th>
              <th class="py-3.5 px-4">Doanh nghiệp</th>
              <th class="py-3.5 px-4 text-center">Trạng thái đề tài</th>
              <th class="py-3.5 px-4 text-center">Điểm số</th>
              <th class="py-3.5 px-4 text-center">Xếp loại</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100 font-medium text-slate-800">
            <tr v-for="(st, index) in paginatedStudents" :key="st.mssv" class="hover:bg-slate-50/80 transition-colors">
              <td class="py-3.5 px-4 text-center font-bold text-slate-400">{{ (currentPage - 1) * pageSize + index + 1 }}</td>
              <td class="py-3.5 px-4">
                <div class="font-bold text-slate-900">{{ st.student }}</div>
                <div class="text-[11px] text-slate-500 font-mono">MSSV: {{ st.mssv }}</div>
              </td>
              <td class="py-3.5 px-4 font-semibold text-slate-600">{{ st.classCode }}</td>
              <td class="py-3.5 px-4 max-w-xs">
                <p class="font-bold text-slate-900 line-clamp-2">{{ st.topic }}</p>
              </td>
              <td class="py-3.5 px-4 font-medium text-slate-700">{{ st.company }}</td>
              <td class="py-3.5 px-4 text-center">
                <span :class="getStatusBadgeClass(st.status)" class="px-2.5 py-1 rounded-full text-[11px] font-extrabold inline-block">
                  {{ st.status }}
                </span>
              </td>
              <td class="py-3.5 px-4 text-center font-black text-slate-900 text-sm">
                {{ st.score !== null ? st.score.toFixed(1) : '--' }}
              </td>
              <td class="py-3.5 px-4 text-center">
                <span :class="getGradeBadgeClass(st.score)" class="px-2.5 py-1 rounded-full text-[10px] font-bold inline-block">
                  {{ getGradeTier(st.score) }}
                </span>
              </td>
            </tr>

            <tr v-if="filteredStudents.length === 0">
              <td colspan="8" class="py-8 text-center text-slate-400 font-medium">
                Không tìm thấy dữ liệu sinh viên phù hợp với bộ lọc.
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Pagination -->
      <div class="p-4 bg-slate-50 border-t border-slate-100 flex items-center justify-between text-xs text-slate-600 font-medium">
        <div>Hiển thị {{ (currentPage - 1) * pageSize + 1 }} - {{ Math.min(currentPage * pageSize, filteredStudents.length) }} trong tổng số {{ filteredStudents.length }} sinh viên</div>
        <div class="flex items-center gap-1">
          <button
            @click="currentPage--"
            :disabled="currentPage === 1"
            class="px-3 py-1.5 rounded-lg border bg-white disabled:opacity-40 font-bold hover:bg-slate-100 transition cursor-pointer"
          >
            Trước
          </button>
          <span class="px-3 font-extrabold text-slate-800">Trang {{ currentPage }} / {{ totalPages }}</span>
          <button
            @click="currentPage++"
            :disabled="currentPage >= totalPages"
            class="px-3 py-1.5 rounded-lg border bg-white disabled:opacity-40 font-bold hover:bg-slate-100 transition cursor-pointer"
          >
            Sau
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted, onUnmounted, watch } from 'vue';
  import { useAuthStore } from '@/stores/auth';
  import React from 'react';
  import { createRoot } from 'react-dom/client';
  import {
    PieChart,
    Pie,
    Cell,
    Tooltip,
    Legend,
    ResponsiveContainer,
    BarChart,
    Bar,
    XAxis,
    YAxis,
    CartesianGrid,
    AreaChart,
    Area
  } from 'recharts';

  const authStore = useAuthStore();
  const userRoleLabel = computed(() => {
    const r = authStore.user?.vaiTro || 'GiangVien';
    if (r === 'GiangVien') return 'Giảng viên Hướng dẫn';
    if (r === 'GiaoVu') return 'Giáo vụ Khoa';
    if (r === 'Admin') return 'Quản trị hệ thống';
    return r;
  });

  const selectedSemester = ref('all');
  const selectedClass = ref('all');
  const isRefreshing = ref(false);
  const topicChartType = ref('pie'); // 'pie' | 'bar'

  const searchQuery = ref('');
  const statusFilter = ref('all');
  const currentPage = ref(1);
  const pageSize = 8;

  // React Roots
  const topicChartContainer = ref(null);
  const gradeChartContainer = ref(null);
  const weeklyChartContainer = ref(null);

  let topicChartRoot = null;
  let gradeChartRoot = null;
  let weeklyChartRoot = null;

  const topicChartMounted = ref(false);
  const gradeChartMounted = ref(false);
  const weeklyChartMounted = ref(false);

  // Raw student dataset
  const baseStudents = [
    { mssv: '20241001', student: 'Nguyễn Văn An', classCode: '47K14.1', topic: 'Xây dựng Hệ thống Quản lý Thực tập tại DUE', company: 'Công ty Cổ phần Công nghệ FPT', status: 'Đã duyệt', score: 9.2 },
    { mssv: '20241002', student: 'Trần Thị Bình', classCode: '47K14.1', topic: 'Phân tích dữ liệu hành vi người dùng trên nền tảng e-Commerce', company: 'Tiki Corporation', status: 'Đã duyệt', score: 8.8 },
    { mssv: '20241003', student: 'Lê Hoàng Cường', classCode: '47K14.2', topic: 'Thiết kế hệ thống CRM hỗ trợ chăm sóc khách hàng tự động', company: 'Viettel Telecom', status: 'Chờ duyệt', score: 7.5 },
    { mssv: '20241004', student: 'Phạm Minh Đức', classCode: '47K14.2', topic: 'Ứng dụng AI trong dự báo doanh thu chuỗi bán lẻ', company: 'VinGroup Technology', status: 'Yêu cầu chỉnh sửa', score: 8.2 },
    { mssv: '20241005', student: 'Vũ Hải Đăng', classCode: '47K14.1', topic: 'Giải pháp Bảo mật thông tin trong Ngân hàng Số', company: 'Ngân hàng MBBank', status: 'Đã duyệt', score: 9.5 },
    { mssv: '20241006', student: 'Đỗ Thu Hà', classCode: '47K14.3', topic: 'Tối ưu hóa chuỗi cung ứng bằng thuật toán học máy', company: 'Giao Hàng Tiết Kiệm', status: 'Đã duyệt', score: 8.0 },
    { mssv: '20241007', student: 'Hoàng Anh Tuấn', classCode: '47K14.3', topic: 'Thiết kế Mobile App Quản lý tài chính cá nhân', company: 'MOMO Wallet', status: 'Từ chối', score: 4.5 },
    { mssv: '20241008', student: 'Ngô Thị Mai', classCode: '47K14.1', topic: 'Xây dựng Dashboard phân tích chỉ số KPI Doanh nghiệp', company: 'VNPT Software', status: 'Đã duyệt', score: 8.7 },
    { mssv: '20241009', student: 'Bùi Gia Bảo', classCode: '47K14.2', topic: 'Giải pháp Chuyển đổi số cho Doanh nghiệp vừa và nhỏ', company: 'MISA Software', status: 'Chờ duyệt', score: 7.8 },
    { mssv: '20241010', student: 'Đặng Kim Liên', classCode: '47K14.3', topic: 'Đánh giá rủi ro tín dụng bằng Mô hình Logistic Regression', company: 'Techcombank', status: 'Đã duyệt', score: 9.0 },
    { mssv: '20241011', student: 'Trịnh Quốc Bảo', classCode: '47K14.1', topic: 'Nghiên cứu ứng dụng Blockchain trong Truy xuất nguồn gốc', company: 'FPT Software Đà Nẵng', status: 'Đã duyệt', score: 8.5 },
    { mssv: '20241012', student: 'Dương Khánh Linh', classCode: '47K14.2', topic: 'Thiết kế Website Quản lý Kho hàng Thông minh', company: 'Sun Group', status: 'Yêu cầu chỉnh sửa', score: 6.8 },
    { mssv: '20241013', student: 'Nguyễn Thành Nam', classCode: '47K14.3', topic: 'Xây dựng chatbot tư vấn sản phẩm tự động', company: 'Shopee Việt Nam', status: 'Đã duyệt', score: 9.1 },
    { mssv: '20241014', student: 'Trần Đức Anh', classCode: '47K14.1', topic: 'Phát triển ứng dụng Web GIS theo dõi tuyến giao hàng', company: 'Grab Vietnam', status: 'Đã duyệt', score: 8.4 },
    { mssv: '20241015', student: 'Lê Phương Thảo', classCode: '47K14.2', topic: 'Nghiên cứu áp dụng thuật toán KMeans phân cụm khách hàng', company: 'Lazada Vietnam', status: 'Chờ duyệt', score: 7.2 },
    { mssv: '20241016', student: 'Phan Nhật Minh', classCode: '47K14.3', topic: 'Xây dựng quy trình tự động hóa RPA cho ngành kế toán', company: 'PwC Việt Nam', status: 'Đã duyệt', score: 9.4 },
    { mssv: '20241017', student: 'Võ Văn Kiệt', classCode: '47K14.1', topic: 'Phân tích hiệu quả chiến dịch Marketing trên TikTok Ads', company: 'Thế Giới Di Động', status: 'Yêu cầu chỉnh sửa', score: 6.2 },
    { mssv: '20241018', student: 'Đặng Ngọc Bích', classCode: '47K14.2', topic: 'Hệ thống khuyến nghị khóa học trực tuyến', company: 'Topica Edtech', status: 'Đã duyệt', score: 8.9 },
    { mssv: '20241019', student: 'Nguyễn Thị Hương', classCode: '47K14.3', topic: 'Nghiên cứu trải nghiệm người dùng UI/UX ứng dụng đặt xe', company: 'Be Group', status: 'Đã duyệt', score: 8.1 },
    { mssv: '20241020', student: 'Hoàng Minh Trí', classCode: '47K14.1', topic: 'Xây dựng hạ tầng Cloud Server trên AWS cho Startup', company: 'CMC Telecom', status: 'Từ chối', score: 3.8 },
    { mssv: '20241021', student: 'Nguyễn Đức Hoàng', classCode: '47K14.1', topic: 'Chưa đăng ký đề tài', company: 'Chưa khai báo', status: 'Chưa nộp', score: null },
    { mssv: '20241022', student: 'Phạm Thu Phương', classCode: '47K14.2', topic: 'Chưa đăng ký đề tài', company: 'Chưa khai báo', status: 'Chưa nộp', score: null },
    { mssv: '20241023', student: 'Lê Minh Khánh', classCode: '47K14.3', topic: 'Chưa đăng ký đề tài', company: 'Chưa khai báo', status: 'Chưa nộp', score: null }
  ];

  // Active list filtered by class dropdown
  const classStudentsList = computed(() => {
    return baseStudents.filter((st) => {
      if (selectedClass.value === 'mis2012') return st.classCode === '47K14.1';
      if (selectedClass.value === 'khdl3011') return st.classCode === '47K14.2';
      if (selectedClass.value === 'tmdt1001') return st.classCode === '47K14.3';
      return true;
    });
  });

  // Topic Approval Data - dynamically calculated
  const topicStatusData = computed(() => {
    const list = classStudentsList.value;
    const counts = {
      'Đã duyệt': 0,
      'Chờ duyệt': 0,
      'Yêu cầu sửa': 0,
      'Từ chối': 0,
      'Chưa nộp': 0
    };

    list.forEach((st) => {
      if (st.status === 'Đã duyệt') counts['Đã duyệt']++;
      else if (st.status === 'Chờ duyệt') counts['Chờ duyệt']++;
      else if (st.status === 'Yêu cầu chỉnh sửa') counts['Yêu cầu sửa']++;
      else if (st.status === 'Từ chối') counts['Từ chối']++;
      else counts['Chưa nộp']++;
    });

    return [
      { name: 'Đã duyệt', value: counts['Đã duyệt'], color: '#10b981' },
      { name: 'Chờ duyệt', value: counts['Chờ duyệt'], color: '#f59e0b' },
      { name: 'Yêu cầu sửa', value: counts['Yêu cầu sửa'], color: '#3b82f6' },
      { name: 'Từ chối', value: counts['Từ chối'], color: '#ef4444' },
      { name: 'Chưa nộp', value: counts['Chưa nộp'], color: '#94a3b8' }
    ];
  });

  // Grade Distribution Data - dynamically calculated
  const gradeDistributionData = computed(() => {
    const list = classStudentsList.value;
    const counts = {
      excellent: 0,
      good: 0,
      fair: 0,
      average: 0,
      poor: 0
    };

    list.forEach((st) => {
      if (st.score !== null && st.score !== undefined) {
        if (st.score >= 9.0) counts.excellent++;
        else if (st.score >= 8.0) counts.good++;
        else if (st.score >= 7.0) counts.fair++;
        else if (st.score >= 5.0) counts.average++;
        else counts.poor++;
      }
    });

    return [
      { range: 'Xuất sắc (9.0-10)', count: counts.excellent, color: '#059669' },
      { range: 'Giỏi (8.0-8.9)', count: counts.good, color: '#2563eb' },
      { range: 'Khá (7.0-7.9)', count: counts.fair, color: '#0891b2' },
      { range: 'Trung bình (5.0-6.9)', count: counts.average, color: '#d97706' },
      { range: 'Yếu/Kém (<5.0)', count: counts.poor, color: '#dc2626' }
    ];
  });

  // Weekly Trend Data
  const weeklyData = computed(() => {
    return [
      { week: 'Tuần 1', submitRate: 65, reviewRate: 50 },
      { week: 'Tuần 2', submitRate: 72, reviewRate: 65 },
      { week: 'Tuần 3', submitRate: 80, reviewRate: 78 },
      { week: 'Tuần 4', submitRate: 88, reviewRate: 82 },
      { week: 'Tuần 5', submitRate: 92, reviewRate: 88 },
      { week: 'Tuần 6', submitRate: 95, reviewRate: 90 },
      { week: 'Tuần 7', submitRate: 91, reviewRate: 89 },
      { week: 'Tuần 8', submitRate: 96, reviewRate: 94 },
      { week: 'Tuần 9', submitRate: 98, reviewRate: 96 },
      { week: 'Tuần 10', submitRate: 100, reviewRate: 98 }
    ];
  });

  // Overall Metrics - dynamically calculated
  const filteredMetrics = computed(() => {
    const list = classStudentsList.value;
    const totalStudents = list.length;
    const approvedCount = list.filter((st) => st.status === 'Đã duyệt').length;
    const approvedRate = totalStudents ? ((approvedCount / totalStudents) * 100).toFixed(1) : '0.0';

    const scoredStudents = list.filter((st) => st.score !== null && st.score !== undefined);
    const sumScores = scoredStudents.reduce((acc, st) => acc + st.score, 0);
    const averageGrade = scoredStudents.length ? (sumScores / scoredStudents.length).toFixed(2) : '0.00';

    const passedCount = scoredStudents.filter((st) => st.score >= 5.0).length;
    const passRate = totalStudents ? ((passedCount / totalStudents) * 100).toFixed(1) : '0.0';

    return {
      totalStudents,
      approvedCount,
      approvedRate,
      averageGrade,
      passedCount,
      passRate
    };
  });

  // Table Filtered Students
  const filteredStudents = computed(() => {
    return classStudentsList.value.filter((st) => {
      const matchSearch =
        searchQuery.value === '' ||
        st.student.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        st.mssv.includes(searchQuery.value) ||
        st.topic.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        st.company.toLowerCase().includes(searchQuery.value.toLowerCase());

      const matchStatus = statusFilter.value === 'all' || st.status === statusFilter.value;

      return matchSearch && matchStatus;
    });
  });

  const totalPages = computed(() => Math.ceil(filteredStudents.value.length / pageSize) || 1);

  const paginatedStudents = computed(() => {
    const start = (currentPage.value - 1) * pageSize;
    return filteredStudents.value.slice(start, start + pageSize);
  });

  function getStatusBadgeClass(status) {
    if (status === 'Đã duyệt') return 'bg-emerald-100 text-emerald-800 border border-emerald-200';
    if (status === 'Chờ duyệt') return 'bg-amber-100 text-amber-800 border border-amber-200';
    if (status === 'Yêu cầu chỉnh sửa') return 'bg-blue-100 text-blue-800 border border-blue-200';
    return 'bg-rose-100 text-rose-800 border border-rose-200';
  }

  function getGradeBadgeClass(score) {
    if (score === null) return 'bg-slate-100 text-slate-600';
    if (score >= 9.0) return 'bg-emerald-100 text-emerald-800';
    if (score >= 8.0) return 'bg-blue-100 text-blue-800';
    if (score >= 7.0) return 'bg-cyan-100 text-cyan-800';
    if (score >= 5.0) return 'bg-amber-100 text-amber-800';
    return 'bg-rose-100 text-rose-800';
  }

  function getGradeTier(score) {
    if (score === null) return 'Chưa chấm';
    if (score >= 9.0) return 'Xuất sắc';
    if (score >= 8.0) return 'Giỏi';
    if (score >= 7.0) return 'Khá';
    if (score >= 5.0) return 'Trung bình';
    return 'Yếu/Kém';
  }

  // Render Topic Chart with Recharts
  function renderTopicChart() {
    if (!topicChartContainer.value) return;
    if (!topicChartRoot) {
      topicChartRoot = createRoot(topicChartContainer.value);
    }

    let chartElem;
    if (topicChartType.value === 'pie') {
      chartElem = React.createElement(
        ResponsiveContainer,
        { width: '100%', height: 320 },
        React.createElement(
          PieChart,
          null,
          React.createElement(
            Pie,
            {
              data: topicStatusData.value,
              cx: '50%',
              cy: '50%',
              innerRadius: 65,
              outerRadius: 105,
              paddingAngle: 4,
              dataKey: 'value',
              label: ({ name, percent }) => `${name} ${(percent * 100).toFixed(0)}%`
            },
            topicStatusData.value.map((entry, idx) =>
              React.createElement(Cell, { key: `topic-cell-${idx}`, fill: entry.color })
            )
          ),
          React.createElement(Tooltip, {
            formatter: (val) => [`${val} sinh viên`, 'Số lượng']
          }),
          React.createElement(Legend, { verticalAlign: 'bottom', height: 36 })
        )
      );
    } else {
      chartElem = React.createElement(
        ResponsiveContainer,
        { width: '100%', height: 320 },
        React.createElement(
          BarChart,
          { data: topicStatusData.value, margin: { top: 20, right: 30, left: 0, bottom: 20 } },
          React.createElement(CartesianGrid, { strokeDasharray: '3 3', vertical: false, stroke: '#e2e8f0' }),
          React.createElement(XAxis, { dataKey: 'name', tick: { fontSize: 11, fontWeight: 600 } }),
          React.createElement(YAxis, { tick: { fontSize: 11 } }),
          React.createElement(Tooltip, { formatter: (val) => [`${val} sinh viên`, 'Số lượng'] }),
          React.createElement(
            Bar,
            { dataKey: 'value', radius: [8, 8, 0, 0] },
            topicStatusData.value.map((entry, idx) =>
              React.createElement(Cell, { key: `bar-cell-${idx}`, fill: entry.color })
            )
          )
        )
      );
    }

    topicChartRoot.render(chartElem);
    topicChartMounted.value = true;
  }

  // Render Grade Chart with Recharts
  function renderGradeChart() {
    if (!gradeChartContainer.value) return;
    if (!gradeChartRoot) {
      gradeChartRoot = createRoot(gradeChartContainer.value);
    }

    const chartElem = React.createElement(
      ResponsiveContainer,
      { width: '100%', height: 320 },
      React.createElement(
        BarChart,
        { data: gradeDistributionData.value, margin: { top: 20, right: 30, left: 0, bottom: 20 } },
        React.createElement(CartesianGrid, { strokeDasharray: '3 3', vertical: false, stroke: '#e2e8f0' }),
        React.createElement(XAxis, { dataKey: 'range', tick: { fontSize: 10, fontWeight: 700 } }),
        React.createElement(YAxis, { tick: { fontSize: 11 } }),
        React.createElement(Tooltip, {
          formatter: (val) => [`${val} sinh viên`, 'Số lượng SV']
        }),
        React.createElement(
          Bar,
          { dataKey: 'count', radius: [8, 8, 0, 0], barSize: 45 },
          gradeDistributionData.value.map((entry, idx) =>
            React.createElement(Cell, { key: `grade-cell-${idx}`, fill: entry.color })
          )
        )
      )
    );

    gradeChartRoot.render(chartElem);
    gradeChartMounted.value = true;
  }

  // Render Weekly Trend Chart with Recharts
  function renderWeeklyChart() {
    if (!weeklyChartContainer.value) return;
    if (!weeklyChartRoot) {
      weeklyChartRoot = createRoot(weeklyChartContainer.value);
    }

    const chartElem = React.createElement(
      ResponsiveContainer,
      { width: '100%', height: 280 },
      React.createElement(
        AreaChart,
        { data: weeklyData.value, margin: { top: 10, right: 30, left: 0, bottom: 10 } },
        React.createElement(CartesianGrid, { strokeDasharray: '3 3', stroke: '#f1f5f9' }),
        React.createElement(XAxis, { dataKey: 'week', tick: { fontSize: 11, fontWeight: 600 } }),
        React.createElement(YAxis, { domain: [0, 100], tickFormatter: (v) => `${v}%`, tick: { fontSize: 11 } }),
        React.createElement(Tooltip, { formatter: (val) => [`${val}%`, 'Tỷ lệ'] }),
        React.createElement(Area, {
          type: 'monotone',
          dataKey: 'submitRate',
          name: 'Nộp đúng hạn',
          stroke: '#005EA3',
          fill: '#005EA3',
          fillOpacity: 0.15,
          strokeWidth: 3
        }),
        React.createElement(Area, {
          type: 'monotone',
          dataKey: 'reviewRate',
          name: 'GV đã nhận xét',
          stroke: '#10b981',
          fill: '#10b981',
          fillOpacity: 0.15,
          strokeWidth: 3
        })
      )
    );

    weeklyChartRoot.render(chartElem);
    weeklyChartMounted.value = true;
  }

  function updateChartData() {
    renderTopicChart();
    renderGradeChart();
    renderWeeklyChart();
  }

  function refreshData() {
    isRefreshing.value = true;
    setTimeout(() => {
      updateChartData();
      isRefreshing.value = false;
    }, 400);
  }

  function exportReport() {
    alert('✓ Đã xuất báo cáo thống kê tiến độ & điểm thực tập Recharts ra file Excel thành công!');
  }

  watch(topicChartType, () => {
    renderTopicChart();
  });

  onMounted(() => {
    setTimeout(() => {
      renderTopicChart();
      renderGradeChart();
      renderWeeklyChart();
    }, 100);
  });

  onUnmounted(() => {
    if (topicChartRoot) topicChartRoot.unmount();
    if (gradeChartRoot) gradeChartRoot.unmount();
    if (weeklyChartRoot) weeklyChartRoot.unmount();
  });
</script>

<style scoped>
  .ims-scope {
    color-scheme: light;
  }
</style>
