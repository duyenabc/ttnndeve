<template>
  <div class="ims-scope space-y-6">
    <!-- Top Greeting & Role Switch Bar -->
    <div class="bg-white rounded-2xl border border-outline-variant p-6 shadow-xs flex flex-wrap items-center justify-between gap-4">
      <div>
        <p class="text-body-sm text-slate-500 font-medium flex items-center gap-2">
          <span class="material-symbols-outlined text-[18px]">calendar_today</span>
          {{ today }}
        </p>
        <h1 class="font-display-lg text-2xl md:text-3xl text-slate-900 font-extrabold mt-1 flex items-center gap-3">
          Xin chào, {{ authStore.user?.hoTen || 'bạn' }}!
          <span :class="roleBadgeClass" class="px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider">
            {{ roleLabel }}
          </span>
        </h1>
        <p class="text-body-md text-slate-600 mt-1">
          {{ roleSubtitle }}
        </p>
      </div>

      <div class="flex items-center gap-3">
        <!-- Role quick switch indicator (helpful for testing & display) -->
        <div class="bg-slate-50 px-3 py-2 rounded-xl border border-slate-200 text-xs font-semibold text-slate-600 flex items-center gap-2">
          <span class="w-2.5 h-2.5 rounded-full bg-emerald-500 animate-pulse"></span>
          Tài khoản: <span class="text-slate-900 font-bold">{{ authStore.user?.maDinhDanh }}</span>
        </div>
      </div>
    </div>

    <!-- ========================================================================= -->
    <!-- 1. ADMIN DASHBOARD (Quản trị hệ thống) -->
    <!-- ========================================================================= -->
    <div v-if="userRole === 'Admin'" class="space-y-6">
      <!-- Admin System Health Metrics -->
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs flex items-center justify-between">
          <div>
            <p class="text-body-xs font-semibold text-slate-500 uppercase tracking-wider">Tài khoản Hoạt động</p>
            <p class="text-3xl font-extrabold text-emerald-600 mt-1">128</p>
            <p class="text-xs text-slate-500 mt-1">Đã xác thực & cấp quyền</p>
          </div>
          <div class="w-12 h-12 rounded-2xl bg-emerald-50 text-emerald-600 flex items-center justify-center font-bold">
            <span class="material-symbols-outlined text-[26px]">manage_accounts</span>
          </div>
        </div>

        <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs flex items-center justify-between">
          <div>
            <p class="text-body-xs font-semibold text-slate-500 uppercase tracking-wider">Yêu cầu Chờ duyệt</p>
            <p class="text-3xl font-extrabold text-amber-600 mt-1">3</p>
            <p class="text-xs text-amber-600 font-medium mt-1">Cần Admin xử lý ngay</p>
          </div>
          <div class="w-12 h-12 rounded-2xl bg-amber-50 text-amber-600 flex items-center justify-center font-bold">
            <span class="material-symbols-outlined text-[26px]">pending_actions</span>
          </div>
        </div>

        <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs flex items-center justify-between">
          <div>
            <p class="text-body-xs font-semibold text-slate-500 uppercase tracking-wider">Tài khoản Bị khóa</p>
            <p class="text-3xl font-extrabold text-rose-600 mt-1">1</p>
            <p class="text-xs text-slate-500 mt-1">Tài khoản tạm ngưng</p>
          </div>
          <div class="w-12 h-12 rounded-2xl bg-rose-50 text-rose-600 flex items-center justify-center font-bold">
            <span class="material-symbols-outlined text-[26px]">block</span>
          </div>
        </div>

        <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs flex items-center justify-between">
          <div>
            <p class="text-body-xs font-semibold text-slate-500 uppercase tracking-wider">Nhật ký Hệ thống</p>
            <p class="text-3xl font-extrabold text-blue-600 mt-1">24</p>
            <p class="text-xs text-slate-500 mt-1">Hoạt động trong 24h qua</p>
          </div>
          <div class="w-12 h-12 rounded-2xl bg-blue-50 text-blue-600 flex items-center justify-center font-bold">
            <span class="material-symbols-outlined text-[26px]">history</span>
          </div>
        </div>
      </div>

      <!-- Admin Quick Action & Requests Management -->
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div class="lg:col-span-2 bg-white rounded-2xl border border-slate-200 p-6 shadow-xs space-y-4">
          <div class="flex items-center justify-between">
            <div>
              <h2 class="font-bold text-title-lg text-slate-900 flex items-center gap-2">
                <span class="material-symbols-outlined text-primary">inbox</span> Yêu cầu Cấp & Phân quyền Tài khoản
              </h2>
              <p class="text-body-xs text-slate-500">Các yêu cầu tạo mới, khóa hoặc khôi phục quyền truy cập</p>
            </div>
            <router-link to="/pending-requests" class="text-primary text-body-sm font-bold hover:underline flex items-center gap-1">
              Quản lý tất cả <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
            </router-link>
          </div>

          <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <router-link to="/pending-requests" class="p-4 bg-slate-50 hover:bg-slate-100 rounded-xl border border-slate-200 block transition">
              <div class="flex items-center justify-between">
                <span class="font-bold text-slate-800 text-sm">Cấp tài khoản mới</span>
                <span class="px-2 py-0.5 bg-amber-100 text-amber-800 rounded-md text-xs font-bold">1 chờ</span>
              </div>
              <p class="text-xs text-slate-500 mt-2">Yêu cầu khởi tạo tài khoản cho sinh viên/giảng viên mới</p>
            </router-link>

            <router-link to="/pending-requests" class="p-4 bg-slate-50 hover:bg-slate-100 rounded-xl border border-slate-200 block transition">
              <div class="flex items-center justify-between">
                <span class="font-bold text-slate-800 text-sm">Mở khóa / Đặt lại MK</span>
                <span class="px-2 py-0.5 bg-amber-100 text-amber-800 rounded-md text-xs font-bold">2 chờ</span>
              </div>
              <p class="text-xs text-slate-500 mt-2">Yêu cầu mở khóa tài khoản hoặc gửi lại liên kết kích hoạt</p>
            </router-link>
          </div>
        </div>

        <div class="bg-white rounded-2xl border border-slate-200 p-6 shadow-xs space-y-4">
          <h2 class="font-bold text-title-lg text-slate-900 flex items-center gap-2">
            <span class="material-symbols-outlined text-indigo-600">admin_panel_settings</span> Phối hợp Quản trị
          </h2>
          <div class="space-y-3">
            <router-link to="/admin/accounts" class="w-full p-3 bg-indigo-50 hover:bg-indigo-100/80 text-indigo-900 rounded-xl font-bold text-xs flex items-center justify-between transition">
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-[20px]">manage_accounts</span> Danh sách Tài khoản Nguời dùng
              </span>
              <span class="material-symbols-outlined text-[18px]">chevron_right</span>
            </router-link>

            <router-link to="/admin/account-management" class="w-full p-3 bg-slate-100 hover:bg-slate-200/80 text-slate-800 rounded-xl font-bold text-xs flex items-center justify-between transition">
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-[20px]">shield</span> Phân quyền & Vai trò
              </span>
              <span class="material-symbols-outlined text-[18px]">chevron_right</span>
            </router-link>
          </div>
        </div>
      </div>
    </div>

    <!-- ========================================================================= -->
    <!-- 2. GIÁO VỤ KHOA DASHBOARD (Quản lý Tiến độ Khoa & Khóa sổ điểm) -->
    <!-- ========================================================================= -->
    <div v-else-if="userRole === 'GiaoVu'" class="space-y-6">
      <!-- Recharts Analytics Banner for Giao Vu -->
      <div class="p-5 bg-gradient-to-r from-blue-900 to-[#005EA3] rounded-2xl text-white shadow-md flex flex-wrap items-center justify-between gap-4">
        <div class="flex items-center gap-4">
          <div class="w-12 h-12 rounded-2xl bg-white/10 flex items-center justify-center shrink-0 backdrop-blur-xs">
            <span class="material-symbols-outlined text-amber-300 text-[28px]">analytics</span>
          </div>
          <div>
            <h3 class="font-extrabold text-lg">Dashboard Thống Kê Tổng Quan Recharts</h3>
            <p class="text-xs text-blue-100 mt-0.5">Phân tích chi tiết biểu đồ trạng thái duyệt đề tài & phân bố điểm của sinh viên toàn Khoa</p>
          </div>
        </div>
        <router-link
          to="/overview-analytics"
          class="px-5 py-2.5 bg-amber-400 hover:bg-amber-300 text-slate-950 font-extrabold rounded-xl text-xs flex items-center gap-2 transition shadow-sm"
        >
          <span>Mở Dashboard Recharts</span>
          <span class="material-symbols-outlined text-[18px]">arrow_forward</span>
        </router-link>
      </div>

      <!-- Alerts Banner for Giao Vu -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div class="p-4 bg-amber-50 border border-amber-200 rounded-2xl flex items-center justify-between">
          <div class="flex items-center gap-3">
            <span class="material-symbols-outlined text-amber-600 text-[28px]">notification_important</span>
            <div>
              <p class="font-bold text-amber-950 text-body-md">Sinh viên trễ hạn nộp nhật ký</p>
              <p class="text-body-sm text-amber-800">8 sinh viên quá hạn 2 tuần chưa nộp báo cáo thực tập</p>
            </div>
          </div>
          <router-link to="/giaovu-requests" class="px-3.5 py-2 bg-amber-600 text-white rounded-xl text-xs font-bold hover:bg-amber-700 transition">
            Đôn đốc ngay
          </router-link>
        </div>

        <div class="p-4 bg-blue-50 border border-blue-200 rounded-2xl flex items-center justify-between">
          <div class="flex items-center gap-3">
            <span class="material-symbols-outlined text-blue-600 text-[28px]">lock_clock</span>
            <div>
              <p class="font-bold text-blue-950 text-body-md">Thời hạn Khóa sổ điểm Khoa</p>
              <p class="text-body-sm text-blue-800">Còn 5 ngày để tất cả Giảng viên chốt điểm (25/06/2026)</p>
            </div>
          </div>
          <router-link to="/giaovu-requests" class="px-3.5 py-2 bg-blue-600 text-white rounded-xl text-xs font-bold hover:bg-blue-700 transition">
            Gửi nhắc nhở
          </router-link>
        </div>
      </div>

      <!-- Department Wide Statistics Chart Component (Exclusively for GiaoVu) -->
      <div class="bg-white rounded-2xl border border-slate-200 p-2 shadow-xs">
        <DashboardSummaryChart />
      </div>
    </div>

    <!-- ========================================================================= -->
    <!-- 3. GIẢNG VIÊN DASHBOARD (Giảng viên hướng dẫn) -->
    <!-- ========================================================================= -->
    <div v-else-if="userRole === 'GiangVien'" class="space-y-6">
      <!-- Recharts Analytics Quick Banner for Teacher -->
      <div class="p-5 bg-gradient-to-r from-blue-900 to-[#005EA3] rounded-2xl text-white shadow-md flex flex-wrap items-center justify-between gap-4">
        <div class="flex items-center gap-4">
          <div class="w-12 h-12 rounded-2xl bg-white/10 flex items-center justify-center shrink-0 backdrop-blur-xs">
            <span class="material-symbols-outlined text-amber-300 text-[28px]">bar_chart</span>
          </div>
          <div>
            <h3 class="font-extrabold text-lg">Báo Cáo & Biểu Đồ Thống Kê Recharts</h3>
            <p class="text-xs text-blue-100 mt-0.5">Biểu đồ trực quan trạng thái duyệt đề tài, phân bố điểm số & xu hướng báo cáo sinh viên</p>
          </div>
        </div>
        <router-link
          to="/overview-analytics"
          class="px-5 py-2.5 bg-amber-400 hover:bg-amber-300 text-slate-950 font-extrabold rounded-xl text-xs flex items-center gap-2 transition shadow-sm"
        >
          <span>Xem Dashboard Thống Kê</span>
          <span class="material-symbols-outlined text-[18px]">arrow_forward</span>
        </router-link>
      </div>

      <!-- 1. Setup Checklist (Collapsible) -->
      <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-xs">
        <details class="group" open>
          <summary class="flex items-center justify-between p-4 cursor-pointer hover:bg-slate-50 transition-colors list-none select-none">
            <div class="flex items-center gap-3">
              <span class="w-8 h-8 rounded-full bg-emerald-100 text-emerald-800 flex items-center justify-center font-black text-xs">1/6</span>
              <span class="font-bold text-sm text-slate-900">Hoàn tất thiết lập lớp học (Setup Checklist)</span>
            </div>
            <span class="material-symbols-outlined text-slate-400 group-open:rotate-180 transition-transform">expand_more</span>
          </summary>
          <div class="p-5 pt-2 border-t border-slate-100 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-3 text-xs">
            <div class="p-3 bg-slate-100 rounded-xl flex items-start gap-2.5 opacity-70">
              <span class="material-symbols-outlined text-emerald-600 text-[18px]">check_circle</span>
              <div>
                <h4 class="font-bold text-slate-800 line-through">Tạo lớp</h4>
                <p class="text-[11px] text-slate-500">Đã khởi tạo lớp học cơ bản.</p>
              </div>
            </div>

            <div v-for="step in [
              { title: 'Cấu hình mốc tiến độ', desc: 'Thiết lập timeline thực tập' },
              { title: 'Cấu hình nhật ký', desc: 'Quy định nộp báo cáo tuần' },
              { title: 'Cấu hình điểm', desc: 'Thiết lập thang điểm đánh giá' },
              { title: 'Cấu hình dashboard', desc: 'Tùy chỉnh chỉ số hiển thị' },
              { title: 'Thêm sinh viên', desc: 'Import danh sách từ file Excel' }
            ]" :key="step.title" class="p-3 bg-white border border-slate-200 rounded-xl flex items-start justify-between gap-2 hover:border-blue-300 transition-colors group">
              <div class="flex items-start gap-2">
                <span class="material-symbols-outlined text-slate-300 text-[18px]">circle</span>
                <div>
                  <h4 class="font-bold text-slate-900">{{ step.title }}</h4>
                  <p class="text-[11px] text-slate-500">{{ step.desc }}</p>
                </div>
              </div>
              <router-link to="/teacher/classes" class="text-blue-700 font-bold text-[11px] hover:underline flex items-center gap-0.5 shrink-0 group-hover:translate-x-0.5 transition-transform">
                Cấu hình <span class="material-symbols-outlined text-[14px]">arrow_forward</span>
              </router-link>
            </div>
          </div>
        </details>
      </div>

      <!-- 2. Bento Layout Grid (Timeline + KPI cards & charts) -->
      <div class="grid grid-cols-1 lg:grid-cols-12 gap-6">
        <!-- Timeline Side Column -->
        <div class="lg:col-span-3 bg-white p-5 rounded-2xl border border-slate-200 shadow-xs flex flex-col justify-between">
          <h3 class="font-bold text-xs uppercase tracking-wider text-slate-400 mb-6 flex items-center gap-1.5">
            <span class="material-symbols-outlined text-[16px] text-blue-600">timeline</span> Timeline Kỳ Thực Tập
          </h3>

          <div class="relative pl-6 space-y-8 my-auto border-l-2 border-slate-200 ml-3">
            <div class="relative">
              <span class="absolute -left-[31px] top-0 w-4 h-4 rounded-full bg-slate-300 border-2 border-white shadow-xs"></span>
              <span class="text-[11px] font-bold text-slate-600">Đăng ký đề tài</span>
              <span class="block text-[10px] text-slate-400">01/08 - 15/08</span>
            </div>

            <div class="relative">
              <span class="absolute -left-[31px] top-0 w-4.5 h-4.5 rounded-full bg-[#005EA3] border-2 border-white shadow-md ring-4 ring-blue-100"></span>
              <div class="inline-flex items-center gap-1 bg-[#005EA3] text-white text-[9px] font-bold px-2 py-0.5 rounded-full mb-1">
                <span class="material-symbols-outlined text-[10px]">location_on</span> Bạn ở đây
              </div>
              <span class="block text-xs font-black text-[#005EA3]">Giữa kỳ & Nhận xét</span>
              <span class="block text-[10px] text-slate-500 font-medium">15/10 - 20/10</span>
            </div>

            <div class="relative">
              <span class="absolute -left-[31px] top-0 w-4 h-4 rounded-full bg-emerald-600 border-2 border-white shadow-xs"></span>
              <span class="text-[11px] font-bold text-emerald-800">Báo cáo & Khóa luận</span>
              <span class="block text-[10px] text-slate-400">20/12 - 31/12</span>
            </div>
          </div>

          <div class="mt-6 pt-4 border-t border-slate-100 text-[11px] text-slate-500 text-center">
            Học kỳ 1 - Năm học 2025-2026
          </div>
        </div>

        <!-- Main Dashboard KPIs & Charts -->
        <div class="lg:col-span-9 space-y-6">
          <!-- Row 1: KPI Cards -->
          <div class="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <!-- Card 1 -->
            <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs space-y-3">
              <div class="flex justify-between items-start">
                <span class="material-symbols-outlined text-blue-700 p-2 bg-blue-50 rounded-xl text-[20px]">history_edu</span>
                <span class="text-emerald-700 font-bold text-xs bg-emerald-50 px-2 py-0.5 rounded-md">+8% vs tuần trước</span>
              </div>
              <div>
                <p class="text-xs font-bold text-slate-500">Tỷ lệ nộp nhật ký</p>
                <h2 class="text-3xl font-black text-slate-900 mt-0.5">92%</h2>
              </div>
              <div class="w-full bg-slate-100 h-2 rounded-full overflow-hidden">
                <div class="bg-[#005EA3] h-full rounded-full" style="width: 92%"></div>
              </div>
            </div>

            <!-- Card 2 -->
            <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs space-y-3">
              <div class="flex justify-between items-start">
                <span class="material-symbols-outlined text-rose-600 p-2 bg-rose-50 rounded-xl text-[20px]">error_outline</span>
                <span class="text-rose-600 font-bold text-xs bg-rose-50 px-2 py-0.5 rounded-md">Cần chú ý</span>
              </div>
              <div>
                <p class="text-xs font-bold text-slate-500">Chậm tiến độ</p>
                <h2 class="text-3xl font-black text-rose-600 mt-0.5">03 <span class="text-xs font-normal text-slate-500">SV</span></h2>
              </div>
              <p class="text-[11px] text-slate-500">Sinh viên quá hạn nộp báo cáo &gt; 2 ngày.</p>
            </div>

            <!-- Card 3 -->
            <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs space-y-3">
              <div class="flex justify-between items-start">
                <span class="material-symbols-outlined text-amber-600 p-2 bg-amber-50 rounded-xl text-[20px]">calendar_month</span>
                <span class="text-slate-600 font-bold text-xs bg-slate-100 px-2 py-0.5 rounded-md">Hôm nay</span>
              </div>
              <div>
                <p class="text-xs font-bold text-slate-500">Lịch hẹn tư vấn</p>
                <h2 class="text-3xl font-black text-slate-900 mt-0.5">02</h2>
              </div>
              <p class="text-[11px] text-blue-700 font-bold flex items-center gap-1">
                <span class="material-symbols-outlined text-[14px]">meeting_room</span> Phòng họp 204 - 14:00
              </p>
            </div>
          </div>

          <!-- Dynamic Charts & Heatmap Conditional Rendering -->
          <div v-if="hasTeacherData" class="space-y-6">
            <!-- Row 2: Charts -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              <!-- Chart 1: Donut breakdown -->
              <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs space-y-4">
                <h4 class="font-bold text-sm text-slate-900">Trạng thái Sinh viên (Lớp mặc định)</h4>
                <div class="flex items-center gap-6 pt-2">
                  <div class="relative w-28 h-28 flex items-center justify-center shrink-0">
                    <svg class="w-full h-full -rotate-90" viewBox="0 0 36 36">
                      <circle cx="18" cy="18" r="15.915" fill="transparent" stroke="#005EA3" stroke-width="4.5" stroke-dasharray="66.6 33.4" stroke-dashoffset="0"></circle>
                      <circle cx="18" cy="18" r="15.915" fill="transparent" stroke="#f97316" stroke-width="4.5" stroke-dasharray="26.6 73.4" stroke-dashoffset="-66.6"></circle>
                      <circle cx="18" cy="18" r="15.915" fill="transparent" stroke="#10b981" stroke-width="4.5" stroke-dasharray="6.8 93.2" stroke-dashoffset="-93.2"></circle>
                    </svg>
                    <div class="absolute inset-0 flex flex-col items-center justify-center">
                      <span class="text-2xl font-black text-slate-900">33</span>
                      <span class="text-[9px] uppercase text-slate-400 font-bold">Tổng SV</span>
                    </div>
                  </div>

                  <div class="space-y-2 text-xs">
                    <div class="flex items-center gap-2">
                      <span class="w-3 h-3 rounded-xs bg-[#005EA3]"></span>
                      <span class="font-semibold text-slate-700">Đang thực tập (30)</span>
                    </div>
                    <div class="flex items-center gap-2">
                      <span class="w-3 h-3 rounded-xs bg-orange-500"></span>
                      <span class="font-semibold text-slate-700">Chờ duyệt đề tài (2)</span>
                    </div>
                    <div class="flex items-center gap-2">
                      <span class="w-3 h-3 rounded-xs bg-emerald-500"></span>
                      <span class="font-semibold text-slate-700">Đã hoàn thành (1)</span>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Chart 2: Weekly Progress Line Trend -->
              <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-xs space-y-4">
                <div class="flex justify-between items-center">
                  <h4 class="font-bold text-sm text-slate-900">Tiến độ nộp nhật ký chung</h4>
                  <span class="text-[11px] font-bold text-blue-700 bg-blue-50 px-2.5 py-1 rounded-lg">4 tuần gần đây</span>
                </div>

                <div class="h-28 w-full relative mt-2">
                  <svg class="w-full h-full" viewBox="0 0 400 100" preserveAspectRatio="none">
                    <path d="M 0 85 L 133 70 L 266 50 L 400 50 L 400 100 L 0 100 Z" fill="#005EA3" fill-opacity="0.1"></path>
                    <path d="M 0 85 L 133 70 L 266 50" fill="none" stroke="#005EA3" stroke-width="3" stroke-linecap="round"></path>
                    <path d="M 266 50 L 400 50" fill="none" stroke="#005EA3" stroke-width="2" stroke-dasharray="4 4" opacity="0.5"></path>
                    <circle cx="0" cy="85" r="4" fill="#005EA3"></circle>
                    <circle cx="133" cy="70" r="4" fill="#005EA3"></circle>
                    <circle cx="266" cy="50" r="5" fill="#005EA3" stroke="white" stroke-width="2"></circle>
                  </svg>
                  <div class="flex justify-between text-[10px] text-slate-400 font-bold mt-1">
                    <span>Tuần 1 (85%)</span>
                    <span>Tuần 2 (88%)</span>
                    <span class="text-blue-700 font-extrabold">Tuần này (92%)</span>
                    <span class="opacity-50">Tuần 4</span>
                  </div>
                </div>
              </div>
            </div>

            <!-- 3. Matrix Heatmap: Tình trạng nộp nhật ký theo tuần -->
            <div class="bg-white rounded-2xl border border-slate-200 p-6 shadow-xs space-y-4">
              <div class="flex flex-col sm:flex-row justify-between sm:items-center gap-3 border-b border-slate-100 pb-4">
                <div>
                  <h4 class="font-bold text-base text-slate-900">Tình trạng nộp nhật ký theo tuần của sinh viên</h4>
                  <p class="text-xs text-slate-500">Theo dõi trực quan việc nộp báo cáo hàng tuần của toàn lớp</p>
                </div>
                <div class="flex items-center gap-3 text-[11px] font-bold">
                  <span class="flex items-center gap-1"><span class="w-3 h-3 bg-emerald-500 rounded-xs"></span> Đúng hạn</span>
                  <span class="flex items-center gap-1"><span class="w-3 h-3 bg-amber-500 rounded-xs"></span> Nộp thiếu/Trễ</span>
                  <span class="flex items-center gap-1"><span class="w-3 h-3 bg-rose-500 rounded-xs"></span> Không nộp</span>
                  <span class="flex items-center gap-1"><span class="w-3 h-3 bg-slate-200 rounded-xs"></span> Chưa đến hạn</span>
                </div>
              </div>

              <div class="overflow-x-auto">
                <div class="min-w-[640px] space-y-2 text-xs">
                  <div class="grid grid-cols-[180px_repeat(8,1fr)] font-bold text-slate-400 text-center pb-1">
                    <div class="text-left pl-2">Họ tên sinh viên</div>
                    <div>T1</div><div>T2</div><div>T3</div><div>T4</div><div>T5</div><div>T6</div><div>T7</div><div>T8</div>
                  </div>

                  <div v-for="st in [
                    { name: 'Nguyễn Văn An', logs: ['ok','ok','ok','fail','pending','pending','pending','pending'] },
                    { name: 'Trần Thị Bình', logs: ['ok','warn','fail','fail','pending','pending','pending','pending'] },
                    { name: 'Lê Văn Cường', logs: ['ok','ok','ok','ok','pending','pending','pending','pending'] },
                    { name: 'Phạm Hải Nam', logs: ['ok','ok','ok','ok','pending','pending','pending','pending'] },
                    { name: 'Vũ Thị Ngọc', logs: ['ok','ok','warn','ok','pending','pending','pending','pending'] }
                  ]" :key="st.name" class="grid grid-cols-[180px_repeat(8,1fr)] items-center hover:bg-slate-50/80 p-1.5 rounded-xl transition-colors">
                    <div class="font-bold text-slate-900 truncate pl-2">{{ st.name }}</div>
                    <div v-for="(status, i) in st.logs" :key="i" class="p-1">
                      <div
                        class="h-6 rounded-md transition-transform hover:scale-105"
                        :class="status === 'ok' ? 'bg-emerald-500' : (status === 'warn' ? 'bg-amber-500' : (status === 'fail' ? 'bg-rose-500' : 'bg-slate-200'))"
                      ></div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Empty State when there is no activity data yet -->
          <div v-else class="bg-white rounded-2xl border border-slate-200 p-12 text-center flex flex-col items-center justify-center min-h-[300px] shadow-2xs">
            <span class="material-symbols-outlined text-[64px] text-slate-300 mb-4" style="font-variation-settings: 'FILL' 1">analytics</span>
            <h3 class="text-base font-bold text-slate-900">Chưa có dữ liệu thống kê biểu đồ</h3>
            <p class="text-xs text-slate-500 max-w-lg mt-2 mx-auto leading-relaxed">
              Biểu đồ phân tích và bảng heatmap tiến độ chưa có dữ liệu hoạt động do các lớp học hiện tại chưa có sinh viên đăng ký đề tài thực tập hoặc chưa có lượt nộp nhật ký nào. Dữ liệu sẽ tự động đồng bộ thời gian thực ngay khi sinh viên bắt đầu khai báo đề tài và nộp báo cáo.
            </p>
          </div>

          <!-- Managed Classes Quick Direct Cards -->
          <div class="bg-white rounded-2xl border border-slate-200 p-6 shadow-xs space-y-4">
            <div class="flex items-center justify-between">
              <div>
                <h2 class="font-bold text-slate-900 text-sm flex items-center gap-2">
                  <span class="material-symbols-outlined text-blue-600">groups</span> Các Lớp Thực tập Giảng viên Phụ trách
                </h2>
                <p class="text-xs text-slate-500">Truy cập nhanh danh sách sinh viên và bảng điểm chi tiết</p>
              </div>
              <router-link to="/teacher/classes" class="text-blue-700 text-xs font-bold hover:underline">
                Tất cả lớp →
              </router-link>
            </div>

            <div class="space-y-3">
              <div v-if="loadingClasses" class="text-center py-6 text-slate-400">
                <span class="animate-spin text-2xl material-symbols-outlined inline-block align-middle">sync</span>
                <span class="ml-2 text-xs">Đang tải danh sách lớp học...</span>
              </div>
              <template v-else-if="teacherClasses.length > 0">
                <div
                  v-for="cls in teacherClasses"
                  :key="cls.maLop"
                  class="p-4 bg-slate-50 rounded-xl border border-slate-200 flex flex-wrap items-center justify-between gap-4"
                >
                  <div>
                    <p class="font-bold text-slate-900 text-sm">{{ cls.tenLop }}</p>
                    <p class="text-xs text-slate-500">{{ cls.tenKy }} • {{ cls.soSinhVien || 0 }} Sinh viên</p>
                  </div>
                  <div class="flex items-center gap-2">
                    <router-link :to="`/teacher/classes/${cls.maLop}`" class="px-3 py-1.5 bg-white border border-slate-300 text-slate-700 rounded-lg text-xs font-bold hover:bg-slate-100">
                      Dashboard lớp
                    </router-link>
                    <router-link :to="`/teacher/classes/${cls.maLop}/students`" class="px-3 py-1.5 bg-white border border-slate-300 text-slate-700 rounded-lg text-xs font-bold hover:bg-slate-100">
                      Danh sách SV
                    </router-link>
                    <router-link :to="`/teacher/classes/${cls.maLop}/grading`" class="px-3 py-1.5 bg-blue-600 text-white rounded-lg text-xs font-bold hover:bg-blue-700">
                      Nhập điểm
                    </router-link>
                  </div>
                </div>
              </template>
              <div v-else class="text-center py-10 bg-slate-50 rounded-xl border border-dashed border-slate-300">
                <span class="material-symbols-outlined text-4xl text-slate-300 mb-2">groups</span>
                <p class="text-xs text-slate-500 font-bold">Bạn chưa quản lý lớp học nào</p>
                <p class="text-[11px] text-slate-400 mt-0.5">Vui lòng liên hệ Giáo vụ khoa để được phân công phụ trách lớp.</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- ========================================================================= -->
    <!-- 4. SINH VIÊN DASHBOARD (Sinh viên thực tập) -->
    <!-- ========================================================================= -->
    <div v-else class="space-y-6">
      <!-- Main Grid Layout -->
      <div class="grid grid-cols-1 lg:grid-cols-12 gap-6">
        <!-- Left Column (Main Tasks, Progress Bento, Recent Feedback) -->
        <div class="lg:col-span-8 space-y-6">
          <!-- Urgent Task Card -->
          <div v-if="submittedCount < 15" class="bg-rose-50/80 border border-rose-200 p-6 rounded-2xl relative overflow-hidden shadow-xs">
            <div class="absolute -right-4 -top-4 text-rose-500 opacity-10 pointer-events-none">
              <span class="material-symbols-outlined text-[110px]">alarm</span>
            </div>
            <h3 class="font-bold text-xs uppercase tracking-wider text-rose-600 mb-3 flex items-center gap-1.5">
              <span class="material-symbols-outlined text-[18px]">error</span>
              Cần làm ngay
            </h3>
            <div class="mb-4">
              <p class="font-bold text-lg text-slate-900">Nộp nhật ký báo cáo Tuần {{ currentWeekNum }}</p>
              <p class="text-xs text-slate-600 mt-1">
                Thời gian còn lại: <span class="text-rose-600 font-bold">2 ngày 04 giờ</span> (Cập nhật nhật ký tuần mới nhất)
              </p>
            </div>
            <router-link
              to="/diaries"
              class="inline-flex items-center gap-2 px-6 py-2.5 bg-rose-600 hover:bg-rose-700 text-white rounded-xl font-bold text-xs transition shadow-xs active:scale-95"
            >
              <span class="material-symbols-outlined text-[18px]">history_edu</span>
              Nộp bài ngay
            </router-link>
          </div>
          <div v-else class="bg-emerald-50/80 border border-emerald-200 p-6 rounded-2xl relative overflow-hidden shadow-xs">
            <div class="absolute -right-4 -top-4 text-emerald-500 opacity-10 pointer-events-none">
              <span class="material-symbols-outlined text-[110px]">check_circle</span>
            </div>
            <h3 class="font-bold text-xs uppercase tracking-wider text-emerald-600 mb-3 flex items-center gap-1.5">
              <span class="material-symbols-outlined text-[18px]">check_circle</span>
              Đã hoàn thành
            </h3>
            <div class="mb-4">
              <p class="font-bold text-lg text-slate-900">Hoàn thành xuất sắc 15/15 tuần báo cáo!</p>
              <p class="text-xs text-slate-600 mt-1">
                Chúc mừng em đã hoàn thành toàn bộ nội dung nộp nhật ký thực tập.
              </p>
            </div>
            <router-link
              to="/diaries"
              class="inline-flex items-center gap-2 px-6 py-2.5 bg-emerald-600 hover:bg-emerald-700 text-white rounded-xl font-bold text-xs transition shadow-xs active:scale-95"
            >
              <span class="material-symbols-outlined text-[18px]">history_edu</span>
              Xem lại nhật ký
            </router-link>
          </div>

          <!-- Bento Grid: Progress Indicators -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <!-- Progress Metrics Card -->
            <div class="bg-white border border-slate-200 p-6 rounded-2xl shadow-2xs flex flex-col justify-between">
              <div>
                <h3 class="font-bold text-xs uppercase tracking-wider text-slate-400 mb-4">Chỉ số tiến độ</h3>
                <div class="space-y-5">
                  <!-- Main Progress Bar -->
                  <div class="space-y-1.5">
                    <div class="flex justify-between items-end">
                      <span class="text-xs font-bold text-slate-800">Tiến độ thực tập</span>
                      <span class="text-2xl font-black text-[#005EA3]">{{ progressPercent }}%</span>
                    </div>
                    <div class="w-full bg-slate-100 h-2.5 rounded-full overflow-hidden">
                      <div class="bg-[#005EA3] h-full rounded-full transition-all duration-700" :style="{ width: progressPercent + '%' }"></div>
                    </div>
                  </div>

                  <!-- Secondary Sub-Metrics -->
                  <div class="space-y-3 pt-2">
                    <div class="space-y-1">
                      <div class="flex justify-between items-center text-xs">
                        <span class="font-semibold text-slate-700">Nhật ký tuần đã nộp</span>
                        <span class="font-bold text-[#005EA3]">{{ submittedCount }}/15</span>
                      </div>
                      <div class="w-full bg-slate-100 h-1.5 rounded-full overflow-hidden">
                        <div class="bg-emerald-600 h-full rounded-full" :style="{ width: (submittedCount / 15 * 100) + '%' }"></div>
                      </div>
                    </div>

                    <div class="space-y-1">
                      <div class="flex justify-between items-center text-xs">
                        <span class="font-semibold text-slate-700">Sự kiện đã tham gia</span>
                        <span class="font-bold text-amber-600">{{ submittedCount >= 1 ? '2/5' : '0/5' }}</span>
                      </div>
                      <div class="w-full bg-slate-100 h-1.5 rounded-full overflow-hidden">
                        <div class="bg-amber-500 h-full rounded-full" :style="{ width: (submittedCount >= 1 ? 40 : 0) + '%' }"></div>
                      </div>
                    </div>

                    <div class="space-y-1">
                      <div class="flex justify-between items-center text-xs">
                        <span class="font-semibold text-slate-700">Giai đoạn hoàn thành</span>
                        <span class="font-bold text-indigo-600">{{ topicStatus === 'approved' ? '2/3' : (topicStatus === 'pending' ? '1/3' : '0/3') }}</span>
                      </div>
                      <div class="w-full bg-slate-100 h-1.5 rounded-full overflow-hidden">
                        <div class="bg-indigo-600 h-full rounded-full" :style="{ width: (topicStatus === 'approved' ? 66 : (topicStatus === 'pending' ? 33 : 0)) + '%' }"></div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Current Week Card -->
            <div class="bg-white border border-slate-200 p-6 rounded-2xl shadow-2xs flex flex-col justify-between">
              <h3 class="font-bold text-xs uppercase tracking-wider text-slate-400 mb-2">Tuần này</h3>
              <div class="flex flex-col justify-center h-full my-4">
                <p class="text-xs font-medium text-slate-500">Trạng thái báo cáo hiện tại</p>
                <p class="text-3xl font-extrabold text-[#005EA3] mt-1">{{ submittedCount }}/15 tuần</p>
                <router-link to="/diaries" class="text-[#005EA3] font-bold text-xs hover:underline mt-3 inline-block">
                  [ Viết nhật ký tuần {{ currentWeekNum }} → ]
                </router-link>
              </div>
              <div class="pt-3 border-t border-slate-100 text-[11px] text-slate-500 flex items-center gap-1.5">
                <span class="material-symbols-outlined text-emerald-600 text-[16px]">check_circle</span>
                {{ submittedCount > 0 ? `Đã hoàn thành nộp nhật ký tuần ${submittedCount}` : 'Chưa có nhật ký nào được nộp' }}
              </div>
            </div>
          </div>

          <!-- Recent Instructor Feedback -->
          <div class="bg-white border border-slate-200 rounded-2xl overflow-hidden shadow-2xs">
            <div class="px-6 py-4 bg-slate-50/80 border-b border-slate-200 flex justify-between items-center">
              <h3 class="font-bold text-sm text-slate-900 flex items-center gap-2">
                <span class="material-symbols-outlined text-[#005EA3]">forum</span>
                Phản hồi mới từ giảng viên
              </h3>
            </div>
            <div class="divide-y divide-slate-100 text-xs">
              <template v-if="feedbacks.length > 0">
                <div v-for="fb in feedbacks" :key="fb.id" class="px-6 py-4 flex items-start gap-4 hover:bg-slate-50/60 transition">
                  <div class="w-10 h-10 rounded-full bg-blue-100 text-[#005EA3] flex items-center justify-center font-bold shrink-0">
                    <span class="material-symbols-outlined text-[20px]">chat</span>
                  </div>
                  <div class="flex-1 min-w-0">
                    <div class="flex items-center gap-2 mb-1">
                      <p class="font-bold text-slate-900">{{ fb.lecturer }}</p>
                      <span class="px-1.5 py-0.5 bg-blue-600 text-white text-[9px] font-black rounded uppercase">Tuần {{ fb.week }}</span>
                    </div>
                    <p class="text-slate-700 italic bg-slate-50 p-2.5 rounded-xl border border-slate-100">
                      "{{ fb.text }}"
                    </p>
                    <div class="flex items-center justify-between mt-2 text-[11px] text-slate-400">
                      <span>{{ fb.time }}</span>
                      <router-link to="/diaries" class="text-[#005EA3] font-bold hover:underline flex items-center gap-1">
                        Xem chi tiết <span class="material-symbols-outlined text-[14px]">arrow_forward</span>
                      </router-link>
                    </div>
                  </div>
                </div>
              </template>
              <template v-else>
                <div class="px-6 py-12 text-center text-slate-500">
                  <span class="material-symbols-outlined text-slate-300 text-5xl mb-2">forum</span>
                  <p class="font-bold">Chưa có phản hồi từ giảng viên</p>
                  <p class="text-xs text-slate-400 mt-1">Các nhận xét, phản hồi cho nhật ký tuần hoặc đề tài của em sẽ hiển thị tại đây khi được phê duyệt.</p>
                </div>
              </template>
            </div>
          </div>
        </div>

        <!-- Right Column (Internship Roadmap Timeline & Upcoming Schedule) -->
        <div class="lg:col-span-4 space-y-6">
          <!-- Timeline / Roadmap Card -->
          <div class="bg-white border border-slate-200 p-6 rounded-2xl shadow-2xs space-y-6">
            <h3 class="font-bold text-sm text-slate-900 flex items-center gap-2">
              <span class="material-symbols-outlined text-[#005EA3]">route</span>
              Lộ trình thực tập
            </h3>

            <div class="relative pl-6 space-y-8">
              <!-- Connecting Line -->
              <div class="absolute left-[9px] top-2 bottom-2 w-0.5 bg-slate-200"></div>

              <!-- Step 1: Registration and Declaration -->
              <div class="relative">
                <div
                  class="absolute -left-[21px] top-0.5 w-3.5 h-3.5 rounded-full border-2 border-white ring-2"
                  :class="topicStatus === 'approved' ? 'bg-emerald-500 ring-emerald-100' : (topicStatus === 'pending' ? 'bg-amber-500 ring-amber-100 animate-pulse' : (topicStatus === 'rejected' ? 'bg-rose-500 ring-rose-100 animate-pulse' : 'bg-slate-300 ring-slate-100'))"
                ></div>
                <div>
                  <h4 class="font-bold text-xs text-slate-900">Giai đoạn: Đăng ký & Khai báo</h4>
                  <p class="text-[11px] text-slate-500 mt-0.5 italic">
                    {{ getDynamicDateStr(-23) }} - {{ getDynamicDateStr(-8) }}
                    <span v-if="topicStatus === 'approved'" class="text-emerald-600 font-bold ml-1">(Hoàn tất)</span>
                    <span v-else-if="topicStatus === 'pending'" class="text-amber-600 font-bold ml-1">(Chờ duyệt đề tài)</span>
                    <span v-else-if="topicStatus === 'rejected'" class="text-rose-600 font-bold ml-1">(Cần sửa đề tài)</span>
                    <span v-else class="text-rose-500 font-medium ml-1">(Chưa hoàn thành)</span>
                  </p>
                </div>
              </div>

              <!-- Step 2: Active -->
              <div class="relative">
                <div class="absolute -left-[21px] top-0.5 w-3.5 h-3.5 rounded-full bg-[#005EA3] border-2 border-white ring-4 ring-blue-100 animate-pulse"></div>
                <div>
                  <h4 class="font-bold text-xs text-[#005EA3]">Giai đoạn: Thực tập tại Doanh nghiệp</h4>
                  <p class="text-xs font-bold text-slate-800 mt-0.5">Bạn đang ở đây</p>
                  <div class="mt-2 inline-flex items-center gap-1.5 px-2.5 py-1 bg-blue-50 text-[#005EA3] rounded-lg text-[11px] font-bold">
                    <span class="material-symbols-outlined text-[14px]">location_on</span>
                    Tuần thứ {{ Math.max(1, Math.min(15, currentWeekNum)) }}/15
                  </div>
                </div>
              </div>

              <!-- Step 3: Pending -->
              <div class="relative opacity-60">
                <div class="absolute -left-[21px] top-0.5 w-3.5 h-3.5 rounded-full bg-slate-300 border-2 border-white"></div>
                <div>
                  <h4 class="font-bold text-xs text-slate-700">Giai đoạn: Đánh giá & Phản biện</h4>
                  <p class="text-[11px] text-slate-500 mt-0.5">{{ getDynamicDateStr(45) }} - {{ getDynamicDateStr(60) }}</p>
                </div>
              </div>
            </div>
          </div>

          <!-- Upcoming Schedule Card -->
          <div class="bg-white border border-slate-200 p-6 rounded-2xl shadow-2xs space-y-4">
            <div class="flex items-center justify-between">
              <h3 class="font-bold text-sm text-slate-900 flex items-center gap-2">
                <span class="material-symbols-outlined text-amber-600">event_upcoming</span>
                Lịch trình & Mốc quan trọng
              </h3>
              <router-link to="/student/calendar" class="text-xs text-[#005EA3] font-bold hover:underline">
                Tất cả →
              </router-link>
            </div>

            <div class="space-y-2.5 text-xs">
              <!-- Item 1: Lịch họp GVHD -->
              <div class="p-3 bg-emerald-50/70 border border-emerald-200 rounded-xl flex items-start gap-2.5">
                <span class="material-symbols-outlined text-emerald-700 text-[18px] mt-0.5">groups</span>
                <div>
                  <p class="font-bold text-slate-900">Lịch họp GVHD TS. Nguyễn Văn A</p>
                  <p class="text-slate-600 mt-0.5">{{ getDynamicDayNameAndDateStr(5) }} • 14:00 Google Meet</p>
                </div>
              </div>

              <!-- Item 2: Hạn nộp Báo cáo Tuần -->
              <div class="p-3 bg-rose-50/70 border border-rose-200 rounded-xl flex items-start gap-2.5">
                <span class="material-symbols-outlined text-rose-700 text-[18px] mt-0.5">alarm</span>
                <div>
                  <p class="font-bold text-slate-900">Hạn nộp Báo cáo Nhật ký Tuần {{ currentWeekNum }}</p>
                  <p class="text-slate-600 mt-0.5">{{ getDynamicDayNameAndDateStr(7) }} • 23:59 PM</p>
                </div>
              </div>

              <!-- Item 3: Hạn nộp Báo cáo Giữa kỳ -->
              <div class="p-3 bg-amber-50/70 border border-amber-200 rounded-xl flex items-start gap-2.5">
                <span class="material-symbols-outlined text-amber-700 text-[18px] mt-0.5">description</span>
                <div>
                  <p class="font-bold text-slate-900">Hạn nộp Báo cáo Tiến độ Giữa kỳ</p>
                  <p class="text-slate-600 mt-0.5">{{ getDynamicDayNameAndDateStr(15) }} • Còn 15 ngày</p>
                </div>
              </div>
            </div>

            <router-link
              to="/student/calendar"
              class="w-full py-2 bg-slate-100 hover:bg-slate-200 text-slate-800 font-bold rounded-xl text-xs transition flex items-center justify-center gap-1.5"
            >
              <span class="material-symbols-outlined text-[16px]">calendar_month</span>
              Mở Lịch thực tập đầy đủ
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, onMounted } from 'vue';
import { useAuthStore } from '@/stores/auth';
import DashboardSummaryChart from '@/components/DashboardSummaryChart.vue';
import { getStudentDiariesFromDb } from '@/services/diaryService';
import api from '@/api/api';

const authStore = useAuthStore();
const userRole = computed(() => authStore.user?.vaiTro || 'SinhVien');

const teacherClasses = ref([]);
const loadingClasses = ref(false);

const hasTeacherData = computed(() => {
  // Show demo dashboard statistics/heatmap only if teacher manages demo class 101 or 103, which have mock students/diaries
  return teacherClasses.value.some(c => c.maLop === 101 || c.maLop === 103);
});

const getDynamicDateStr = (daysFromToday) => {
  const d = new Date();
  d.setDate(d.getDate() + daysFromToday);
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const yyyy = d.getFullYear();
  return `${dd}/${mm}/${yyyy}`;
};

const getDynamicDayNameAndDateStr = (daysFromToday) => {
  const d = new Date();
  d.setDate(d.getDate() + daysFromToday);
  const weekdays = ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
  const dayName = weekdays[d.getDay()];
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const yyyy = d.getFullYear();
  return `${dayName}, ${dd}/${mm}/${yyyy}`;
};

const diaries = ref([]);
const topicStatus = ref('new');
const loading = ref(false);

onMounted(async () => {
  if (userRole.value === 'SinhVien') {
    loading.value = true;
    try {
      const studentId = authStore.user?.maDinhDanh || 'default';
      const fetched = await getStudentDiariesFromDb(studentId);
      diaries.value = fetched || [];

      const savedStatus = localStorage.getItem(`ims_student_topic_status_${studentId}`);
      if (savedStatus) {
        topicStatus.value = savedStatus;
      } else {
        if (studentId === '20241001') {
          topicStatus.value = 'approved';
        } else {
          topicStatus.value = 'new';
        }
      }
    } catch (e) {
      console.error('Error fetching dashboard student data:', e);
    } finally {
      loading.value = false;
    }
  } else if (userRole.value === 'GiangVien') {
    loadingClasses.value = true;
    try {
      const response = await api.get('/giangvien/classes');
      teacherClasses.value = response.data || [];
    } catch (e) {
      console.error('Error fetching teacher classes list:', e);
    } finally {
      loadingClasses.value = false;
    }
  }
});

const submittedCount = computed(() => diaries.value.length);
const progressPercent = computed(() => Math.min(100, Math.round((submittedCount.value / 15) * 100)));
const currentWeekNum = computed(() => submittedCount.value + 1);

const feedbacks = computed(() => {
  const list = [];
  diaries.value.forEach(d => {
    if (d.feedback && d.feedback.trim()) {
      list.push({
        id: d.id,
        week: d.week,
        lecturer: d.lecturerName || 'TS. Nguyễn Văn A',
        text: d.feedback,
        time: d.updatedAt ? new Date(d.updatedAt).toLocaleDateString('vi-VN') : 'Gần đây'
      });
    }
  });
  return list;
});

const roleLabels = {
  SinhVien: 'Sinh viên',
  GiangVien: 'Giảng viên hướng dẫn',
  GiaoVu: 'Giáo vụ khoa',
  Admin: 'Quản trị viên'
};

const roleSubtitles = {
  Admin: 'Bảng điều khiển quản trị hệ thống, tài khoản và phân quyền người dùng',
  GiaoVu: 'Bảng tổng quan theo dõi tiến độ thực tập và quản lý điểm số toàn Khoa',
  GiangVien: 'Bảng điều khiển quản lý các lớp thực tập, chấm nhật ký và duyệt đề tài',
  SinhVien: 'Tổng quan tiến độ thực tập cá nhân, theo dõi nhật ký và bảng điểm'
};

const roleBadgeClasses = {
  Admin: 'bg-rose-100 text-rose-800 border border-rose-300',
  GiaoVu: 'bg-amber-100 text-amber-800 border border-amber-300',
  GiangVien: 'bg-blue-100 text-blue-800 border border-blue-300',
  SinhVien: 'bg-emerald-100 text-emerald-800 border border-emerald-300'
};

const roleLabel = computed(() => roleLabels[userRole.value] || 'Người dùng');
const roleSubtitle = computed(() => roleSubtitles[userRole.value] || 'Hệ thống Quản lý Thực tập DUE');
const roleBadgeClass = computed(() => roleBadgeClasses[userRole.value] || 'bg-slate-100 text-slate-800');

const today = computed(() =>
  new Date().toLocaleDateString('vi-VN', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
);
</script>

