<template>
  <div class="ims-scope w-full mx-auto space-y-6 font-sans pb-24">
    <!-- Main Task & Event Management View -->
    <div v-if="!activeSubmissionEvent" class="space-y-6">
      <!-- Breadcrumbs & Header -->
      <div>
        <nav class="flex items-center gap-2 text-xs text-slate-500 mb-2">
          <router-link to="/teacher/classes" class="hover:text-blue-700 transition-colors">Lớp của tôi</router-link>
          <span class="material-symbols-outlined text-[14px]">chevron_right</span>
          <span class="text-slate-700 font-medium">Công việc</span>
          <span class="material-symbols-outlined text-[14px]">chevron_right</span>
          <span class="font-bold text-slate-900">Sự kiện & Bài nộp</span>
        </nav>
        <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <h1 class="text-[28px] font-bold text-slate-900 tracking-tight">Quản lý Công việc & Sự kiện Nộp bài</h1>
        </div>
      </div>

      <!-- Sub Navigation Tabs -->
      <div class="flex items-center gap-8 border-b border-slate-200">
        <router-link
          :to="`/teacher/classes/${classId}/diaries`"
          class="pb-3 text-slate-500 hover:text-blue-700 font-semibold text-sm transition-colors"
        >
          Nhật ký thực tập
        </router-link>
        <router-link
          :to="`/teacher/classes/${classId}/tasks`"
          class="pb-3 text-blue-700 border-b-2 border-blue-700 font-bold text-sm"
        >
          Sự kiện & Bài nộp
        </router-link>
        <router-link
          :to="`/teacher/classes/${classId}/topics`"
          class="pb-3 text-slate-500 hover:text-blue-700 font-semibold text-sm transition-colors flex items-center gap-1.5"
        >
          Phê duyệt đ�? tài
          <span v-if="pendingTopicsCount > 0" class="bg-blue-100 text-blue-800 text-[10px] px-1.5 py-0.2 rounded-full font-bold">
            {{ pendingTopicsCount }}
          </span>
        </router-link>
      </div>

      <!-- Filter Bar & Create Event Button -->
      <div class="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs flex flex-col md:flex-row justify-between items-stretch md:items-center gap-4">
        <div class="flex items-center gap-2 bg-slate-100 p-1 rounded-xl border border-slate-200 w-fit text-xs font-bold">
          <button
            v-for="filter in statusFilters"
            :key="filter.value"
            @click="activeFilter = filter.value"
            class="px-4 py-1.5 rounded-lg transition-all"
            :class="activeFilter === filter.value
              ? 'bg-blue-700 text-white shadow-2xs'
              : 'text-slate-600 hover:text-slate-900'"
          >
            {{ filter.label }}
          </button>
        </div>

        <div class="flex items-center gap-4">
          <div class="text-slate-500 text-xs font-medium">
            �?ang hiển thị <span class="font-bold text-slate-900">{{ filteredEvents.length }}</span> sự kiện
          </div>
          <button
            @click="openCreateModal"
            class="bg-[#005EA3] hover:bg-blue-800 text-white px-5 py-2.5 rounded-xl flex items-center gap-2 font-bold text-xs shadow-md transition-all active:scale-95 shrink-0"
          >
            <span class="material-symbols-outlined text-[20px]">add</span>
            Thêm sự kiện
          </button>
        </div>
      </div>

      <!-- Event List Table -->
      <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-2xs">
        <div class="overflow-x-auto">
          <table class="w-full text-left border-collapse">
            <thead>
              <tr class="bg-slate-50 border-b border-slate-200 text-[11px] font-bold uppercase tracking-wider text-slate-500">
                <th class="px-6 py-3.5">Tên sự kiện</th>
                <th class="px-6 py-3.5">Hạn chót</th>
                <th class="px-6 py-3.5">�?ã nộp</th>
                <th class="px-6 py-3.5">Chấm điểm</th>
                <th class="px-6 py-3.5">Trạng thái</th>
                <th class="px-6 py-3.5 text-right">Thao tác</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-slate-100 text-xs">
              <tr
                v-for="event in filteredEvents"
                :key="event.id"
                class="hover:bg-slate-50/80 transition-colors group"
              >
                <td class="px-6 py-4">
                  <div class="flex flex-col">
                    <span
                      @click="viewSubmissions(event)"
                      class="font-bold text-slate-900 text-sm hover:text-blue-700 transition-colors cursor-pointer"
                    >
                      {{ event.title }}
                    </span>
                    <span class="text-[11px] text-slate-500 mt-0.5">{{ event.stage }}</span>
                  </div>
                </td>
                <td class="px-6 py-4 font-mono font-medium text-slate-700">
                  {{ event.deadline }}
                </td>
                <td class="px-6 py-4">
                  <div class="flex items-center gap-2">
                    <span class="font-bold text-blue-700">{{ event.submitted }}</span>/{{ event.total }}
                    <div class="w-16 h-1.5 bg-slate-100 rounded-full overflow-hidden shrink-0">
                      <div
                        class="bg-blue-600 h-full rounded-full"
                        :style="{ width: `${(event.submitted / event.total) * 100}%` }"
                      />
                    </div>
                  </div>
                </td>
                <td class="px-6 py-4 font-medium text-slate-600">
                  {{ event.graded }}/{{ event.submitted }}
                </td>
                <td class="px-6 py-4">
                  <span
                    class="inline-flex items-center px-2.5 py-0.5 rounded-full text-[11px] font-bold"
                    :class="getStatusBadgeClass(event.status)"
                  >
                    {{ event.status }}
                  </span>
                </td>
                <td class="px-6 py-4 text-right">
                  <div class="flex justify-end gap-2">
                    <button
                      @click="viewSubmissions(event)"
                      class="px-3 py-1.5 bg-blue-50 text-blue-700 hover:bg-blue-100 font-bold rounded-xl transition flex items-center gap-1 text-xs"
                      title="Theo dõi bài nộp & chấm điểm"
                    >
                      <span class="material-symbols-outlined text-[16px]">assignment_turned_in</span>
                      Theo dõi nộp
                    </button>
                    <button
                      @click="editEvent(event)"
                      class="p-1.5 text-slate-500 hover:text-blue-700 hover:bg-slate-100 rounded-lg transition-colors"
                      title="Thiết lập / Chỉnh sửa"
                    >
                      <span class="material-symbols-outlined text-[18px]">settings</span>
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Submission Tracking Detail View (E06.3) -->
    <div v-else-if="activeSubmissionEvent" class="space-y-6">
      <!-- Breadcrumb Navigation -->
      <nav class="flex items-center gap-2 text-xs text-slate-500">
        <button @click="activeSubmissionEvent = null" class="hover:text-blue-700 font-medium">Công việc</button>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <button @click="activeSubmissionEvent = null" class="hover:text-blue-700 font-medium">Sự kiện</button>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="font-bold text-slate-900">{{ activeSubmissionEvent.title }}</span>
      </nav>

      <!-- Event Header Info -->
      <div class="bg-white rounded-2xl border border-slate-200 p-6 shadow-2xs flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <div class="flex items-center gap-2 mb-1">
            <span class="px-2.5 py-0.5 bg-blue-100 text-blue-800 text-[11px] font-extrabold rounded-full">
              {{ activeSubmissionEvent.stage }}
            </span>
          </div>
          <h1 class="text-2xl font-bold text-slate-900 tracking-tight">{{ activeSubmissionEvent.title }}</h1>
          <p class="text-xs text-slate-500 mt-1 flex flex-wrap items-center gap-4">
            <span class="flex items-center gap-1"><span class="material-symbols-outlined text-[16px]">schedule</span> Hạn nộp: {{ activeSubmissionEvent.deadline }}</span>
            <span class="flex items-center gap-1"><span class="material-symbols-outlined text-[16px]">groups</span> Sĩ số lớp: {{ activeSubmissionEvent.total }} sinh viên</span>
          </p>
        </div>

        <button
          @click="activeSubmissionEvent = null"
          class="px-4 py-2 border border-slate-300 text-slate-700 hover:bg-slate-100 font-bold text-xs rounded-xl transition flex items-center gap-1.5 self-start md:self-auto"
        >
          <span class="material-symbols-outlined text-[18px]">arrow_back</span>
          Quay lại danh sách
        </button>
      </div>

      <!-- Filters Row -->
      <div class="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs flex flex-col md:flex-row justify-between items-stretch md:items-center gap-4">
        <div class="relative flex-1 max-w-md">
          <span class="material-symbols-outlined absolute left-3 top-2.5 text-slate-400 text-[18px]">search</span>
          <input
            v-model="submissionSearch"
            type="text"
            placeholder="Tìm kiếm theo tên sinh viên, MSSV..."
            class="w-full pl-10 pr-4 py-2 bg-slate-50 border border-slate-300 rounded-xl text-xs outline-none focus:ring-2 focus:ring-blue-600"
          />
        </div>

        <div class="flex flex-wrap items-center gap-3 text-xs">
          <select v-model="submissionStatusFilter" class="px-3 py-2 bg-slate-50 border border-slate-300 rounded-xl font-semibold text-slate-700 outline-none focus:ring-2 focus:ring-blue-600">
            <option value="ALL">Tất cả trạng thái nộp</option>
            <option value="� ã nộp">� ã nộp</option>
            <option value="Nộp trễ">Nộp trễ</option>
            <option value="Chưa nộp">Chưa nộp</option>
          </select>

          <select v-model="gradingStatusFilter" class="px-3 py-2 bg-slate-50 border border-slate-300 rounded-xl font-semibold text-slate-700 outline-none focus:ring-2 focus:ring-blue-600">
            <option value="ALL">Trạng thái chấm điểm</option>
            <option value="� ã chấm">� ã chấm</option>
            <option value="Chưa chấm">Chưa chấm</option>
          </select>
        </div>
      </div>

      <!-- Submission Table -->
      <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-2xs">
        <div class="overflow-x-auto">
          <table class="w-full text-left border-collapse text-xs">
            <thead>
              <tr class="bg-slate-50 border-b border-slate-200 text-[11px] font-bold uppercase tracking-wider text-slate-500">
                <th class="px-6 py-3.5">H�  tên sinh viên</th>
                <th class="px-6 py-3.5">MSSV</th>
                <th class="px-6 py-3.5">Th� i gian nộp</th>
                <th class="px-6 py-3.5">Trạng thái nộp</th>
                <th class="px-6 py-3.5">Trạng thái chấm</th>
                <th class="px-6 py-3.5 text-center">� iểm số</th>
                <th class="px-6 py-3.5 text-right">Thao tác</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-slate-100">
              <tr
                v-for="sub in filteredSubmissions"
                :key="sub.id"
                class="hover:bg-slate-50/80 transition cursor-pointer"
                @click="openRubricReview(sub)"
              >
                <td class="px-6 py-4 font-bold text-slate-900 text-sm">
                  {{ sub.studentName }}
                </td>
                <td class="px-6 py-4 font-mono text-slate-600">
                  {{ sub.mssv }}
                </td>
                <td class="px-6 py-4 font-mono text-slate-600">
                  {{ sub.submittedAt || '-' }}
                </td>
                <td class="px-6 py-4">
                  <span
                    class="px-2.5 py-0.5 rounded-full text-[11px] font-bold"
                    :class="subStatusClass(sub.status)"
                  >
                    {{ sub.status }}
                  </span>
                </td>
                <td class="px-6 py-4">
                  <span
                    class="px-2.5 py-0.5 rounded-full text-[11px] font-bold"
                    :class="sub.isGraded ? 'bg-blue-100 text-blue-800' : 'bg-amber-100 text-amber-800'"
                  >
                    {{ sub.isGraded ? '� ã chấm' : 'Chưa chấm' }}
                  </span>
                </td>
                <td class="px-6 py-4 text-center font-bold text-sm" :class="sub.score !== '-' ? 'text-blue-700 font-black' : 'text-slate-400'">
                  {{ sub.score }}
                </td>
                <td class="px-6 py-4 text-right">
                  <button
                    @click.stop="openRubricReview(sub)"
                    class="px-3 py-1.5 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-xl transition shadow-2xs flex items-center gap-1 ml-auto"
                  >
                    <span class="material-symbols-outlined text-[16px]">rate_review</span>
                    Chấm bài
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- FULL-PAGE / SIDE-BY-SIDE RUBRIC EVALUATION MODAL (E07.5) -->
    <div
      v-if="selectedSubForReview"
      class="fixed inset-0 z-[110] bg-white flex flex-col font-sans"
    >
      <!-- Top Header -->
      <header class="h-14 bg-[#005EA3] text-white px-6 flex items-center justify-between shrink-0 shadow-md">
        <div class="flex items-center gap-4">
          <button @click="selectedSubForReview = null" class="hover:bg-white/10 p-1.5 rounded-lg transition" title="� óng">
            <span class="material-symbols-outlined text-[20px]">arrow_back</span>
          </button>
          <div>
            <h2 class="font-bold text-sm leading-tight">Chấm điểm bài nộp - {{ selectedSubForReview.studentName }} (MSSV: {{ selectedSubForReview.mssv }})</h2>
            <p class="text-[11px] text-blue-100">{{ activeSubmissionEvent?.title || 'Báo cáo thực tập' }} • Nộp lúc: {{ selectedSubForReview.submittedAt }}</p>
          </div>
        </div>

        <div class="flex items-center gap-3">
          <button
            @click="navigateSubmission(-1)"
            class="px-3.5 py-1.5 bg-white/10 hover:bg-white/20 font-bold text-xs rounded-lg transition flex items-center gap-1"
          >
            <span class="material-symbols-outlined text-[16px]">arrow_back</span> B�i tr??c
          </button>
          <button
            @click="navigateSubmission(1)"
            class="px-3.5 py-1.5 bg-white/10 hover:bg-white/20 font-bold text-xs rounded-lg transition flex items-center gap-1"
          >
            B�i ti?p theo <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
          </button>
          <span class="text-blue-200 text-xs font-mono">{{ currentSubIndex + 1 }}/{{ filteredSubmissions.length }}</span>
          <span class="w-px h-4 bg-white/20"></span>
          <button @click="showToast('?� l?u nh�p!')" class="px-3.5 py-1.5 bg-white/10 hover:bg-white/20 font-bold text-xs rounded-lg transition">L?u nh�p</button>
          <button @click="saveRubricGrade" class="px-4 py-1.5 bg-emerald-500 hover:bg-emerald-600 font-bold text-xs rounded-lg shadow-xs transition">L?u &amp; B�i ti?p ?</button>
        </div>
      </header>

      <!-- Main Split View Container -->
      <div class="flex-1 flex overflow-hidden">
        <!-- Left Side: Document Viewer (58%) -->
        <section class="w-full lg:w-[58%] bg-slate-100 border-r border-slate-300 flex flex-col relative">
          <!-- Document Viewer Toolbar -->
          <div class="h-11 bg-white border-b border-slate-200 flex items-center justify-between px-4 text-xs shrink-0">
            <div class="flex items-center gap-3">
              <div class="flex items-center bg-slate-100 rounded-lg p-0.5 border border-slate-200">
                <button @click="zoomLevel = Math.max(50, zoomLevel - 10)" class="px-2 py-0.5 text-slate-600 hover:bg-white rounded font-bold">-</button>
                <span class="px-2 text-[11px] font-bold text-slate-700">{{ zoomLevel }}%</span>
                <button @click="zoomLevel = Math.min(150, zoomLevel + 10)" class="px-2 py-0.5 text-slate-600 hover:bg-white rounded font-bold">+</button>
              </div>
              <span class="text-slate-400">|</span>
              <div class="flex items-center gap-1 font-semibold text-slate-600">
                <span>Trang {{ currentPage }} / 12</span>
              </div>
            </div>

            <div class="flex items-center gap-2">
              <button @click="showToast('� ang tải file gốc đính kèm...')" class="px-3 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg font-bold flex items-center gap-1">
                <span class="material-symbols-outlined text-[16px]">download</span> Tải file gốc
              </button>
            </div>
          </div>

          <!-- Document Canvas Content (Scrollable) -->
          <div
            class="flex-1 overflow-y-auto p-6 flex justify-center bg-slate-200/60 custom-scrollbar relative"
            @mouseup="onTextSelect"
          >
            <!-- Floating "+ Comment" button on text selection -->
            <div
              v-if="isSupportedFile && floatingBtn.visible"
              class="fixed z-[200] flex items-center gap-1 px-3 py-1.5 bg-slate-900 text-white text-xs font-bold rounded-xl shadow-xl cursor-pointer hover:bg-blue-700 transition"
              :style="{ top: floatingBtn.y + 'px', left: floatingBtn.x + 'px' }"
              @mousedown.prevent="addHighlightComment"
            >
              <span class="material-symbols-outlined text-[15px]">add_comment</span> + Comment
            </div>

            <div
              v-if="isSupportedFile"
              class="bg-white shadow-xl p-10 max-w-2xl w-full min-h-[900px] border border-slate-300 space-y-6 text-slate-800 text-xs transition-transform origin-top select-text"
              :style="{ transform: `scale(${zoomLevel / 100})` }"
            >
              <div class="text-center space-y-1 border-b border-slate-200 pb-4">
                <h3 class="text-lg font-black uppercase text-slate-900">B�o C�o Ti?n ?? Th?c T?p Tu?n 4</h3>
                <p class="text-slate-500 font-semibold">Sinh vi�n: {{ selectedSubForReview.studentName }} � MSSV: {{ selectedSubForReview.mssv }}</p>
              </div>

              <div class="space-y-3 leading-relaxed">
                <p class="font-bold text-sm text-slate-900">1. N?i dung c�ng vi?c th?c hi?n trong tu?n:</p>
                <p class="text-slate-700 bg-slate-50 p-3 rounded-lg border border-slate-200">
                  <span
                    v-for="(seg, i) in docSegments"
                    :key="i"
                    :class="seg.highlighted ? 'bg-amber-200 text-amber-900 rounded cursor-help' : ''"
                    :title="seg.comment || ''"
                  >{{ seg.text }}</span>
                </p>

                <p class="font-bold text-sm text-slate-900">2. C�c k?t qu? ?� ??t ???c:</p>
                <ul class="list-disc pl-5 space-y-1 text-slate-700">
                  <li>Ho�n th�nh 15 test case t? ??ng tr�n Selenium v?i t? l? th�nh c�ng 95%.</li>
                  <li>Thu th?p � ki?n ?�nh gi� ti?n ?? t? Anh Mentor h??ng d?n t?i doanh nghi?p.</li>
                  <li>C?p nh?t b? sung s? ?? ki?n tr�c h? th?ng v�o nh?t k� th?c t?p.</li>
                </ul>

                <p class="font-bold text-sm text-slate-900">3. Kh� kh?n &amp; Gi?i ph�p ?? xu?t:</p>
                <p class="text-slate-700 bg-amber-50/60 p-3 rounded-lg border border-amber-200">
                  Gặp một số vấn đ�  v�  th� i gian phản hồi API khi chạy test đồng th� i. Em đang tự tìm hiểu thêm v�  tối ưu hóa connection pool và bộ nhớ đệm Redis.
                </p>
              </div>
            </div>
            <div v-else class="flex items-center justify-center h-full text-slate-500">
              <div class="text-center">
                <span class="material-symbols-outlined text-[64px] block mb-2 opacity-50">description</span>
                <p>File kh�ng h? tr? xem tr??c tr?c ti?p.</p>
              </div>
            </div>
          </div>
        </section>

        <!-- Right Side: Review Panel (42%) with Tabs -->
        <section class="w-full lg:w-[42%] bg-white flex flex-col border-l border-slate-200 overflow-hidden">
          <!-- Tab bar -->
          <div class="flex border-b border-slate-200 bg-slate-50 text-xs font-bold shrink-0">
            <button
              v-if="isSupportedFile"
              @click="reviewTab = 'comments'"
              class="px-5 py-3 flex items-center gap-1.5 transition"
              :class="reviewTab === 'comments' ? 'border-b-2 border-blue-700 text-blue-700 bg-white' : 'text-slate-500 hover:text-slate-800'"
            >
              <span class="material-symbols-outlined text-[16px]">comment</span>
              Nh?n x�t inline
              <span v-if="highlightComments.length" class="ml-1 bg-amber-400 text-white text-[10px] px-1.5 py-0.5 rounded-full">{{ highlightComments.length }}</span>
            </button>
            <button
              @click="reviewTab = 'overall'"
              class="px-5 py-3 flex items-center gap-1.5 transition"
              :class="reviewTab === 'overall' ? 'border-b-2 border-blue-700 text-blue-700 bg-white' : 'text-slate-500 hover:text-slate-800'"
            >
              <span class="material-symbols-outlined text-[16px]">rate_review</span>
              Nh?n x�t t?ng th? &amp; ?i?m
            </button>
          </div>

          <div class="flex-1 overflow-y-auto custom-scrollbar">
            <!-- TAB 1: Inline Highlight Comments -->
            <div v-if="reviewTab === 'comments'" class="p-5 space-y-4">
              <p class="text-xs text-slate-500 bg-amber-50 border border-amber-200 rounded-xl p-3 flex items-start gap-2">
                <span class="material-symbols-outlined text-amber-500 text-[18px] shrink-0">info</span>
                B�i ?en m?t ?o?n v?n b?n trong t�i li?u b�n tr�i, sau ?� nh?n n�t <strong class="text-slate-800">"+ Comment"</strong> ?? t?o nh?n x�t g?n v�o ?o?n ?�.
              </p>

              <!-- Pending new comment input -->
              <div v-if="pendingHighlight" class="border-2 border-amber-400 rounded-xl p-4 bg-amber-50 space-y-2 text-xs">
                <p class="font-bold text-amber-900 flex items-center gap-1">
                  <span class="material-symbols-outlined text-[15px]">format_quote</span>
                  ?o?n tr�ch d?n:
                </p>
                <p class="italic text-slate-600 bg-white px-3 py-2 rounded-lg border border-amber-200 line-clamp-2">"{{ pendingHighlight.text }}"</p>
                <textarea
                  v-model="pendingHighlight.draft"
                  rows="3"
                  placeholder="Nh?p nh?n x�t c?a b?n (t?i ?a 1000 k� t?)..."
                  maxlength="1000"
                  class="w-full px-3 py-2 border border-amber-300 rounded-lg outline-none focus:ring-2 focus:ring-amber-400 text-xs resize-none"
                ></textarea>
                <div class="flex justify-end gap-2">
                  <button @click="pendingHighlight = null" class="px-3 py-1.5 text-slate-500 hover:bg-slate-100 rounded-lg font-bold transition">H?y</button>
                  <button @click="confirmHighlightComment" class="px-4 py-1.5 bg-amber-500 hover:bg-amber-600 text-white font-bold rounded-lg transition">L?u nh?n x�t</button>
                </div>
              </div>

              <!-- Empty state -->
              <div v-if="highlightComments.length === 0 && !pendingHighlight" class="text-center py-10 text-slate-400">
                <span class="material-symbols-outlined text-[40px] block mb-2">format_quote</span>
                <p class="text-xs">Ch?a c� nh?n x�t inline n�o.<br>B�i ?en v?n b?n ?? b?t ??u.</p>
              </div>

              <!-- List of saved highlights -->
              <div
                v-for="(hc, idx) in highlightComments"
                :key="hc.id"
                class="border border-slate-200 rounded-xl p-4 bg-white shadow-2xs text-xs space-y-2 group"
              >
                <div class="flex items-start justify-between">
                  <p class="italic text-slate-500 line-clamp-1 flex-1">
                    <span class="material-symbols-outlined text-amber-400 text-[14px] align-middle">format_quote</span>
                    "{{ hc.text }}"
                  </p>
                  <button @click="removeHighlight(idx)" class="ml-2 text-slate-300 hover:text-rose-500 opacity-0 group-hover:opacity-100 transition">
                    <span class="material-symbols-outlined text-[16px]">close</span>
                  </button>
                </div>
                <p class="text-slate-800 font-medium leading-relaxed">{{ hc.comment }}</p>
                <p class="text-slate-400 text-[10px]">{{ hc.timestamp }}</p>
              </div>
            </div>

            <!-- TAB 2: Overall Review + Rubric + Score -->
            <div v-if="reviewTab === 'overall'" class="p-5 space-y-5">
              <!-- Score Box -->
              <div class="bg-blue-50/60 border border-blue-200 p-4 rounded-2xl flex items-center justify-between">
                <div>
                  <span class="text-[11px] font-extrabold uppercase text-blue-800 tracking-wider">T?ng ?i?m b�i n?p (Thang 10)</span>
                  <div class="flex items-baseline gap-1 mt-0.5">
                    <input
                      v-model.number="currentDirectScore"
                      type="number"
                      step="0.1"
                      min="0"
                      max="10"
                      @input="hasUnsavedChanges = true"
                      class="w-20 text-2xl font-black text-blue-800 bg-white border border-blue-300 rounded-xl text-center outline-none focus:ring-2 focus:ring-blue-600"
                    />
                    <span class="text-lg font-bold text-blue-900">/ 10</span>
                  </div>
                </div>
                <div class="text-right">
                  <span class="text-[10px] text-slate-500 font-bold block">T?NG ?I?M RUBRIC</span>
                  <span class="text-base font-black text-emerald-700">{{ calculatedRubricScore }} / 4.5 pts</span>
                </div>
              </div>

              <!-- Rubric Matrix -->
              <div class="space-y-3">
                <h3 class="font-bold text-slate-900 text-sm flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-blue-600 text-[18px]">fact_check</span>
                  Ma tr?n Rubric Ch?m ?i?m
                </h3>
                <div class="border border-slate-200 rounded-xl overflow-x-auto text-xs bg-white shadow-2xs">
                  <table class="w-full border-collapse">
                    <tbody class="divide-y divide-slate-100">
                      <tr v-for="(criterion, rIdx) in rubricCriteria" :key="criterion.id" class="hover:bg-slate-50/50">
                        <td class="p-3 font-bold text-slate-800 w-1/3 border-r border-slate-200 bg-slate-50/80">{{ criterion.title }}</td>
                        <td
                          v-for="(level, cIdx) in criterion.levels"
                          :key="cIdx"
                          @click="selectRubricLevel(rIdx, cIdx); hasUnsavedChanges = true"
                          class="p-2.5 border-r border-slate-100 cursor-pointer transition relative hover:bg-blue-50/40"
                          :class="criterion.selectedIndex === cIdx ? 'bg-emerald-50/80 border-2 border-emerald-500 font-bold text-emerald-900' : 'text-slate-600'"
                        >
                          <div class="flex flex-col space-y-1">
                            <span class="text-[11px] leading-tight">{{ level.label }}</span>
                            <span class="text-[10px] font-extrabold" :class="criterion.selectedIndex === cIdx ? 'text-emerald-800' : 'text-slate-400'">{{ level.points }} pts</span>
                          </div>
                          <span v-if="criterion.selectedIndex === cIdx" class="material-symbols-outlined absolute top-1 right-1 text-emerald-600 text-[14px]">check_circle</span>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>

              <!-- Overall feedback -->
              <div class="space-y-2">
                <h3 class="font-bold text-slate-900 text-sm flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-blue-600 text-[18px]">chat_bubble</span>
                  Nh?n x�t t?ng th? (t?i ?a 2000 k� t?)
                </h3>
                <textarea
                  v-model="reviewComment"
                  rows="5"
                  maxlength="2000"
                  placeholder="Nh?p nh?n x�t t?ng th? chi ti?t cho sinh vi�n..."
                  class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs outline-none focus:ring-2 focus:ring-blue-600 resize-none"
                  @input="hasUnsavedChanges = true"
                ></textarea>
                <p class="text-right text-[10px] text-slate-400">{{ reviewComment.length }}/2000</p>
              </div>
            </div>
          </div>
        </section>
      </div>
    </div>



    <!-- MOODLE-STYLE COMPREHENSIVE EVENT SETUP MODAL / RUBRIC MATRIX EDITOR -->
    <div
      v-if="showCreateModal"
      class="fixed inset-0 z-[120] bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-4 overflow-y-auto"
      @click.self="showCreateModal = false"
    >
      <div class="bg-white rounded-2xl shadow-2xl max-w-4xl w-full my-8 overflow-hidden border border-slate-200 font-sans flex flex-col max-h-[90vh]">
        <!-- Header -->
        <div class="px-6 py-4 bg-[#005EA3] text-white flex justify-between items-center shrink-0">
          <div>
            <h2 class="font-bold text-base">
              {{ isEditing ? 'Thiết lập & Cấu hình Sự kiện Nộp bài' : 'Thêm sự kiện nộp bài nội bộ mới' }}
            </h2>
            <p class="text-[11px] text-blue-100">Thiết lập th� i gian, thông số kỹ thuật và ma trận chấm điểm Rubric</p>
          </div>
          <button @click="showCreateModal = false" class="text-white/80 hover:text-white text-lg">✕</button>
        </div>

        <!-- Form Body (Scrollable) -->
        <div class="p-6 overflow-y-auto custom-scrollbar space-y-6 text-xs flex-1">
          <!-- SECTION 1: THÔNG TIN CHUNG -->
          <section class="border border-slate-200 rounded-2xl p-4 bg-white space-y-4">
            <div
              @click="toggleSection('general')"
              class="flex items-center justify-between cursor-pointer border-b pb-2 text-slate-900 font-bold text-sm"
            >
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-blue-600">info</span> 1. Thông tin chung sự kiện
              </span>
              <span class="material-symbols-outlined text-slate-400">
                {{ sectionOpen.general ? 'expand_less' : 'expand_more' }}
              </span>
            </div>

            <div v-if="sectionOpen.general" class="space-y-4 pt-1">
              <div>
                <label class="block font-bold text-slate-700 mb-1">
                  Tên sự kiện <span class="text-rose-500">*</span>
                </label>
                <input
                  v-model="eventForm.title"
                  type="text"
                  required
                  placeholder="Nhập tên sự kiện nộp tài liệu..."
                  class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium"
                />
                <!-- Quick Suggestion Pills -->
                <div class="flex items-center gap-2 mt-2">
                  <span class="text-[10px] text-slate-400 font-bold">Gợi ý nhanh:</span>
                  <button
                    type="button"
                    @click="eventForm.title = 'Nộp đ�  cương chi tiết'"
                    class="px-2.5 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg text-[10px] font-bold"
                  >
                    Nộp đ�  cương
                  </button>
                  <button
                    type="button"
                    @click="eventForm.title = 'Nộp bản thảo báo cáo giữa kỳ'"
                    class="px-2.5 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg text-[10px] font-bold"
                  >
                    Nộp bản thảo
                  </button>
                  <button
                    type="button"
                    @click="eventForm.title = 'Báo cáo tổng kết thực tập cuối kỳ'"
                    class="px-2.5 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg text-[10px] font-bold"
                  >
                    Nộp báo cáo cuối cùng
                  </button>
                </div>
              </div>

              <div>
                <label class="block font-bold text-slate-700 mb-1">Mô tả & Hướng dẫn cho sinh viên</label>
                <!-- Editor Toolbar Mockup -->
                <div class="border border-slate-300 rounded-xl overflow-hidden">
                  <div class="bg-slate-50 border-b border-slate-200 px-3 py-1.5 flex items-center gap-3 text-slate-600">
                    <span class="material-symbols-outlined text-[18px] cursor-pointer hover:text-blue-700">format_bold</span>
                    <span class="material-symbols-outlined text-[18px] cursor-pointer hover:text-blue-700">format_italic</span>
                    <span class="material-symbols-outlined text-[18px] cursor-pointer hover:text-blue-700">format_list_bulleted</span>
                    <span class="material-symbols-outlined text-[18px] cursor-pointer hover:text-blue-700">link</span>
                    <span class="material-symbols-outlined text-[18px] cursor-pointer hover:text-blue-700">image</span>
                  </div>
                  <textarea
                    v-model="eventForm.description"
                    rows="3"
                    placeholder="Nhập hướng dẫn chi tiết v�  cấu trúc bài nộp, quy cách đặt tên file..."
                    class="w-full p-3 border-none outline-none focus:ring-0 text-xs"
                  ></textarea>
                </div>
              </div>
            </div>
          </section>

          <!-- SECTION 2: THÔNG S�  KỸ THUẬT -->
          <section class="border border-slate-200 rounded-2xl p-4 bg-white space-y-4">
            <div
              @click="toggleSection('specs')"
              class="flex items-center justify-between cursor-pointer border-b pb-2 text-slate-900 font-bold text-sm"
            >
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-blue-600">tune</span> 2. Thông số kỹ thuật file nộp
              </span>
              <span class="material-symbols-outlined text-slate-400">
                {{ sectionOpen.specs ? 'expand_less' : 'expand_more' }}
              </span>
            </div>

            <div v-if="sectionOpen.specs" class="space-y-4 pt-1">
              <div>
                <label class="block font-bold text-slate-700 mb-1.5">� ịnh dạng file cho phép</label>
                <div class="flex flex-wrap items-center gap-4 text-xs">
                  <label class="flex items-center gap-1.5 font-semibold text-slate-700 cursor-pointer">
                    <input type="checkbox" checked class="rounded text-blue-600" /> .pdf
                  </label>
                  <label class="flex items-center gap-1.5 font-semibold text-slate-700 cursor-pointer">
                    <input type="checkbox" checked class="rounded text-blue-600" /> .docx
                  </label>
                  <label class="flex items-center gap-1.5 font-semibold text-slate-700 cursor-pointer">
                    <input type="checkbox" class="rounded text-blue-600" /> .xlsx
                  </label>
                  <label class="flex items-center gap-1.5 font-semibold text-slate-700 cursor-pointer">
                    <input type="checkbox" class="rounded text-blue-600" /> .pptx
                  </label>
                  <label class="flex items-center gap-1.5 font-semibold text-slate-700 cursor-pointer">
                    <input type="checkbox" class="rounded text-blue-600" /> Không giới hạn
                  </label>
                </div>
              </div>

              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label class="block font-bold text-slate-700 mb-1">Dung lượng tối đa (MB)</label>
                  <input
                    v-model.number="eventForm.maxSize"
                    type="number"
                    min="1"
                    max="100"
                    class="w-full px-3.5 py-2 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
                  />
                </div>
                <div>
                  <label class="block font-bold text-slate-700 mb-1">Số lượng file tối đa</label>
                  <input
                    v-model.number="eventForm.maxFiles"
                    type="number"
                    min="1"
                    max="10"
                    class="w-full px-3.5 py-2 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
                  />
                </div>
              </div>
            </div>
          </section>

          <!-- SECTION 3: THỜI GIAN & THIẾT LẬP -->
          <section class="border border-slate-200 rounded-2xl p-4 bg-white space-y-4">
            <div
              @click="toggleSection('time')"
              class="flex items-center justify-between cursor-pointer border-b pb-2 text-slate-900 font-bold text-sm"
            >
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-blue-600">calendar_month</span> 3. Th� i gian & Cấu hình nộp
              </span>
              <span class="material-symbols-outlined text-slate-400">
                {{ sectionOpen.time ? 'expand_less' : 'expand_more' }}
              </span>
            </div>

            <div v-if="sectionOpen.time" class="space-y-4 pt-1">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label class="block font-bold text-slate-700 mb-1">Th� i gian mở nộp</label>
                  <input
                    v-model="eventForm.openTime"
                    type="datetime-local"
                    class="w-full px-3.5 py-2 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
                  />
                </div>
                <div>
                  <label class="block font-bold text-slate-700 mb-1">Hạn chót nộp bài (*)</label>
                  <input
                    v-model="eventForm.deadlineTime"
                    type="datetime-local"
                    required
                    class="w-full px-3.5 py-2 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-bold text-blue-900"
                  />
                </div>
              </div>

              <div>
                <label class="block font-bold text-slate-700 mb-1">Giai đoạn thực tập</label>
                <select v-model="eventForm.stage" class="w-full px-3.5 py-2 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium">
                  <option>Giai đoạn 1: Tìm kiếm & � �  xuất</option>
                  <option>Giai đoạn 2: Triển khai nội dung</option>
                  <option>Giai đoạn 3: Hoàn thiện & Báo cáo</option>
                </select>
              </div>

              <div class="flex items-center gap-3 pt-1">
                <input v-model="eventForm.allowLate" type="checkbox" id="lateToggle" class="rounded text-blue-600 focus:ring-blue-600" />
                <label for="lateToggle" class="font-bold text-slate-700 cursor-pointer">Cho phép nộp trễ (Ghi nhận điểm trừ trễ)</label>
              </div>
            </div>
          </section>

          <!-- SECTION 4: CẤU HÌNH RUBRIC CHẤM � IỂM -->
          <section class="border border-slate-200 rounded-2xl p-4 bg-white space-y-4">
            <div
              @click="toggleSection('grading')"
              class="flex items-center justify-between cursor-pointer border-b pb-2 text-slate-900 font-bold text-sm"
            >
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-blue-600">grading</span> 4. Phương thức chấm điểm & Ma trận Rubric
              </span>
              <span class="material-symbols-outlined text-slate-400">
                {{ sectionOpen.grading ? 'expand_less' : 'expand_more' }}
              </span>
            </div>

            <div v-if="sectionOpen.grading" class="space-y-4 pt-1">
              <div class="flex items-center justify-between bg-slate-50 p-3 rounded-xl border border-slate-200">
                <label class="flex items-center gap-2 font-bold text-slate-800 cursor-pointer">
                  <input v-model="eventForm.isGraded" type="checkbox" class="rounded text-blue-600" />
                  Sự kiện này có tính điểm
                </label>

                <!-- Method Switcher -->
                <div v-if="eventForm.isGraded" class="flex bg-slate-200 p-1 rounded-lg text-xs font-bold">
                  <button
                    type="button"
                    @click="eventForm.gradingMethod = 'direct'"
                    class="px-3 py-1 rounded-md transition"
                    :class="eventForm.gradingMethod === 'direct' ? 'bg-white text-blue-700 shadow-2xs' : 'text-slate-600'"
                  >
                    � iểm trực tiếp
                  </button>
                  <button
                    type="button"
                    @click="eventForm.gradingMethod = 'rubric'"
                    class="px-3 py-1 rounded-md transition"
                    :class="eventForm.gradingMethod === 'rubric' ? 'bg-white text-blue-700 shadow-2xs' : 'text-slate-600'"
                  >
                    Ma trận Rubric
                  </button>
                </div>
              </div>

              <!-- Interactive Rubric Matrix Builder -->
              <div v-if="eventForm.isGraded && eventForm.gradingMethod === 'rubric'" class="space-y-4 border border-blue-200 p-4 bg-blue-50/20 rounded-2xl">
                <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-2">
                  <h4 class="font-bold text-slate-900 text-xs flex items-center gap-1.5">
                    <span class="material-symbols-outlined text-blue-600 text-[18px]">table_chart</span>
                    Thiết lập ma trận Rubric
                  </h4>

                  <div class="flex items-center gap-2">
                    <label class="text-[11px] font-bold text-slate-600">Cách tính:</label>
                    <select v-model="eventForm.rubricCalcMethod" class="px-2.5 py-1 bg-white border border-slate-300 rounded-lg text-[11px] font-bold text-slate-800">
                      <option value="avg">Trung bình cộng các tiêu chí</option>
                      <option value="sum">Tổng điểm các tiêu chí</option>
                    </select>
                  </div>
                </div>

                <!-- Rubric Matrix Table -->
                <div class="border border-slate-300 rounded-xl overflow-hidden bg-white shadow-2xs">
                  <div class="overflow-x-auto">
                    <table class="w-full text-left border-collapse text-xs">
                      <thead>
                        <tr class="bg-slate-100 border-b border-slate-300 font-bold text-slate-700 text-[11px]">
                          <th class="p-3 w-12 text-center border-r border-slate-200">STT</th>
                          <th class="p-3 w-1/3 border-r border-slate-200">Tiêu chí đánh giá</th>
                          <th
                            v-for="(lvl, lIdx) in rubricLevels"
                            :key="lIdx"
                            class="p-3 border-r border-slate-200 min-w-[130px] relative group"
                          >
                            <span>Mức {{ lIdx + 1 }}</span>
                            <button
                              v-if="rubricLevels.length > 2"
                              type="button"
                              @click="removeRubricLevelColumn(lIdx)"
                              class="absolute top-1 right-1 text-rose-500 hover:text-rose-700 font-black text-xs hidden group-hover:block"
                              title="Xóa mức"
                            >
                              ✕
                            </button>
                          </th>
                          <th class="p-3 text-center w-28">Thao tác</th>
                        </tr>
                      </thead>
                      <tbody class="divide-y divide-slate-200">
                        <tr v-for="(crit, rIdx) in editableCriteria" :key="crit.id" class="hover:bg-slate-50">
                          <td class="p-3 text-center border-r border-slate-200 font-bold text-slate-500">
                            {{ rIdx + 1 }}
                          </td>
                          <td class="p-3 border-r border-slate-200 space-y-2">
                            <input
                              v-model="crit.title"
                              type="text"
                              placeholder="Nhập tên tiêu chí / hạng mục con..."
                              class="w-full p-1.5 border border-slate-300 rounded-lg outline-none focus:ring-1 focus:ring-blue-600 font-bold text-slate-800"
                            />
                            <div class="flex items-center gap-2 text-[10px]">
                              <div class="flex items-center gap-1 bg-slate-100 px-1.5 py-0.5 rounded border border-slate-200">
                                <span class="font-bold text-slate-600">Tỷ lệ:</span>
                                <input v-model.number="crit.weight" type="number" min="0" max="100" class="w-10 bg-white border rounded text-center font-bold text-blue-700" />
                                <span class="font-bold text-slate-600">%</span>
                              </div>
                              <div class="flex items-center gap-1 bg-amber-50 px-1.5 py-0.5 rounded border border-amber-200">
                                <span class="font-bold text-amber-900">� iểm liệt:</span>
                                <input v-model.number="crit.minPass" type="number" step="0.5" min="0" max="10" class="w-10 bg-white border rounded text-center font-bold text-rose-700" />
                              </div>
                            </div>
                          </td>
                          <td
                            v-for="(lvl, cIdx) in crit.levels"
                            :key="cIdx"
                            class="p-2.5 border-r border-slate-200 space-y-1 bg-slate-50/50"
                          >
                            <input
                              v-model="lvl.label"
                              type="text"
                              placeholder="Mô tả mức..."
                              class="w-full p-1 border border-slate-200 rounded text-[11px] outline-none focus:ring-1 focus:ring-blue-600"
                            />
                            <div class="flex items-center justify-between gap-1 mt-1">
                              <input
                                v-model.number="lvl.points"
                                type="number"
                                step="0.25"
                                class="w-14 p-1 border border-slate-300 rounded text-center text-xs font-bold text-emerald-700 outline-none"
                              />
                              <span class="text-[10px] font-bold text-emerald-800">pts</span>
                            </div>
                          </td>
                          <td class="p-3 text-center">
                            <div class="flex items-center justify-center gap-1">
                              <button
                                type="button"
                                @click="duplicateCriterion(rIdx)"
                                class="p-1 text-slate-500 hover:text-blue-700"
                                title="Nhân bản"
                              >
                                <span class="material-symbols-outlined text-[18px]">content_copy</span>
                              </button>
                              <button
                                type="button"
                                @click="removeCriterionRow(rIdx)"
                                class="p-1 text-slate-500 hover:text-rose-600"
                                title="Xóa tiêu chí"
                              >
                                <span class="material-symbols-outlined text-[18px]">delete</span>
                              </button>
                            </div>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>

                <div class="flex justify-between items-center pt-1">
                  <button
                    type="button"
                    @click="addCriterionRow"
                    class="px-3.5 py-1.5 bg-white border border-slate-300 hover:bg-slate-50 font-bold rounded-xl text-xs flex items-center gap-1 text-slate-700 shadow-2xs"
                  >
                    <span class="material-symbols-outlined text-[16px]">add</span> Thêm tiêu chí
                  </button>

                  <button
                    type="button"
                    @click="addRubricLevelColumn"
                    class="px-3.5 py-1.5 bg-white border border-slate-300 hover:bg-slate-50 font-bold rounded-xl text-xs flex items-center gap-1 text-slate-700 shadow-2xs"
                  >
                    <span class="material-symbols-outlined text-[16px]">view_column</span> Thêm cột Mức
                  </button>
                </div>
              </div>
            </div>
          </section>
        </div>

        <!-- Footer -->
        <div class="px-6 py-4 bg-slate-50 border-t border-slate-200 flex justify-end gap-3 shrink-0">
          <button
            type="button"
            @click="showCreateModal = false"
            class="px-5 py-2 border border-slate-300 text-slate-700 font-bold text-xs rounded-xl hover:bg-slate-100"
          >
            H?y b?
          </button>
          <button
            type="button"
            @click="saveEventForm"
            class="px-6 py-2 bg-[#005EA3] hover:bg-blue-800 text-white font-bold text-xs rounded-xl shadow-md transition flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[18px]">save</span>
            L?u & �p d?ng s? ki?n
          </button>
        </div>
      </div>
    </div>

    <!-- Toast Notification -->
    <div
      v-if="toastMsg"
      class="fixed top-6 left-1/2 -translate-x-1/2 z-[150] bg-slate-900 text-white px-5 py-3 rounded-xl shadow-2xl text-xs font-bold flex items-center gap-2"
    >
      <span class="material-symbols-outlined text-emerald-400 text-[18px]">check_circle</span>
      {{ toastMsg }}
    </div>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted, onUnmounted } from 'vue';
  import { useRoute } from 'vue-router';
  import api from '@/api/api';
  import { listenAllTopics } from '@/services/topicService';

  const route = useRoute();
  const classId = computed(() => route.params.id || '1');

  const activeFilter = ref('ALL');
  const showCreateModal = ref(false);
  const isEditing = ref(false);
  const toastMsg = ref('');

  // Section collapse toggles in Event setup form
  const sectionOpen = ref({
    general: true,
    specs: true,
    time: true,
    grading: true
  });

  function toggleSection(sec) {
    sectionOpen.value[sec] = !sectionOpen.value[sec];
  }

  // Submissions state
  const activeSubmissionEvent = ref(null);
  const submissionSearch = ref('');
  const submissionStatusFilter = ref('ALL');
  const gradingStatusFilter = ref('ALL');
  const selectedSubForReview = ref(null);
  
  const supportedExtensions = ['pdf', 'doc', 'docx'];
  const isSupportedFile = computed(() => {
    if (!selectedSubForReview.value) return false;
    const ext = selectedSubForReview.value.fileExtension || 'pdf';
    return supportedExtensions.includes(ext.toLowerCase());
  });

  // Rubric Review state
  const zoomLevel = ref(100);
  const currentPage = ref(4);
  const currentDirectScore = ref(8.5);
  const reviewComment = ref('');
  const reviewTab = ref('comments'); // 'comments' | 'overall'
  const hasUnsavedChanges = ref(false);

  // Highlight comment state (TC05-18)
  const highlightComments = ref([]);
  const pendingHighlight = ref(null);
  const floatingBtn = ref({ visible: false, x: 0, y: 0, text: '' });

  // Document paragraph segments for highlight rendering
  const docSegments = ref([]);
  const rawDocText = 'Trong tu?n qua t?i doanh nghi?p, em ?� ti?p t?c nghi�n c?u c?u tr�c h? th?ng backend, x�y d?ng b? test case ki?m th? t? ??ng cho c�c API giao d?ch ch�nh v� ho�n th�nh b�o c�o gi?a k?.';

  function onTextSelect() {
    const selection = window.getSelection();
    if (!selection || selection.rangeCount === 0 || selection.toString().trim().length === 0) {
      floatingBtn.value.visible = false;
      return;
    }
    const selectedText = selection.toString().trim().slice(0, 200);
    const range = selection.getRangeAt(0);
    const rect = range.getBoundingClientRect();
    floatingBtn.value = {
      visible: true,
      x: Math.max(0, rect.left + rect.width / 2 - 55),
      y: Math.max(0, rect.top - 44),
      text: selectedText
    };
  }

  function addHighlightComment() {
    const text = floatingBtn.value.text;
    floatingBtn.value.visible = false;
    if (!text) return;
    pendingHighlight.value = { text, draft: '' };
    reviewTab.value = 'comments';
    window.getSelection()?.removeAllRanges();
  }

  function confirmHighlightComment() {
    if (!pendingHighlight.value?.draft?.trim()) return;
    const { text, draft } = pendingHighlight.value;
    highlightComments.value.push({
      id: Date.now(),
      text,
      comment: draft.trim(),
      timestamp: new Date().toLocaleString('vi-VN')
    });
    // Mark matched segment as highlighted
    const newSegs = [];
    for (const seg of docSegments.value) {
      if (!seg.highlighted && seg.text.includes(text)) {
        const idx = seg.text.indexOf(text);
        if (idx > 0) newSegs.push({ text: seg.text.slice(0, idx), highlighted: false, comment: '' });
        newSegs.push({ text, highlighted: true, comment: draft.trim() });
        const after = seg.text.slice(idx + text.length);
        if (after) newSegs.push({ text: after, highlighted: false, comment: '' });
      } else {
        newSegs.push(seg);
      }
    }
    docSegments.value = newSegs;
    pendingHighlight.value = null;
    hasUnsavedChanges.value = true;
    showToast('?� l?u nh?n x�t inline th�nh c�ng!');
  }

  function removeHighlight(idx) {
    const removed = highlightComments.value.splice(idx, 1)[0];
    const merged = [];
    for (const seg of docSegments.value) {
      if (seg.highlighted && seg.text === removed.text) {
        const last = merged[merged.length - 1];
        if (last && !last.highlighted) { last.text += seg.text; }
        else merged.push({ text: seg.text, highlighted: false, comment: '' });
      } else {
        merged.push(seg);
      }
    }
    docSegments.value = merged;
  }

  const currentSubIndex = ref(0);

  function navigateSubmission(dir) {
    // TC05-20: Warn when navigating with unsaved changes
    if (hasUnsavedChanges.value) {
      const confirmed = confirm('B?n ch?a l?u ?i?m/nh?n x�t. B? qua v� chuy?n b�i?');
      if (!confirmed) return;
    }
    const list = filteredSubmissions.value;
    const nextIdx = currentSubIndex.value + dir;
    if (nextIdx < 0 || nextIdx >= list.length) {
      showToast('?� ??n ??u/cu?i danh s�ch b�i n?p!');
      return;
    }
    currentSubIndex.value = nextIdx;
    openRubricReview(list[nextIdx]);
  }


  const rubricCriteria = ref([
    {
      id: 1,
      title: 'Lập kế hoạch thực hiện',
      selectedIndex: 3,
      levels: [
        { label: 'Không hợp lý', points: 0 },
        { label: 'Chưa hợp lý', points: 1.0 },
        { label: 'Khá hợp lý', points: 1.25 },
        { label: 'Hoàn toàn hợp lý', points: 1.5 }
      ]
    },
    {
      id: 2,
      title: 'Giai đoạn thực hiện',
      selectedIndex: 2,
      levels: [
        { label: 'Chậm trễ nhi� u', points: 0 },
        { label: '� úng tiến độ ít', points: 0.5 },
        { label: 'Có chậm trễ nhẹ', points: 0.75 },
        { label: 'Xuất sắc', points: 1.5 }
      ]
    },
    {
      id: 3,
      title: 'Nội dung báo cáo',
      selectedIndex: 3,
      levels: [
        { label: 'Thiếu nhi� u nội dung', points: 0 },
        { label: 'Khá đầy đủ', points: 0.5 },
        { label: '� ầy đủ yêu cầu', points: 1.0 },
        { label: 'Phong phú sâu sắc', points: 1.5 }
      ]
    }
  ]);

  const calculatedRubricScore = computed(() => {
    let total = 0;
    rubricCriteria.value.forEach(c => {
      if (c.selectedIndex !== null && c.levels[c.selectedIndex]) {
        total += c.levels[c.selectedIndex].points;
      }
    });
    return total.toFixed(2);
  });

  const statusFilters = [
    { value: 'ALL', label: 'Tất cả' },
    { value: '� ang mở', label: '� ang mở' },
    { value: 'Sắp tới', label: 'Sắp tới' },
    { value: '� ã đóng', label: '� ã đóng' }
  ];

  const events = ref([]);

  // Submissions
  const submissions = ref([]);
  const pendingTopicsCount = ref(0);
  let unsubscribeTopics = null;

  onUnmounted(() => {
    if (unsubscribeTopics) {
      unsubscribeTopics();
    }
  });

  onMounted(async () => {
    // Listen to all topics to count pending approvals
    unsubscribeTopics = listenAllTopics((firestoreTopics) => {
      const pendingList = firestoreTopics.filter(t => t.status === 'pending');
      pendingTopicsCount.value = pendingList.length;
    });

    try {
      const res = await api.get(`/giangvien/classes/${classId.value}/events`);
      events.value = res.data;
    } catch (err) {
      console.error(err);
      showToast('Không thể tải danh sách sự kiện');
    }
  });

  // Event Setup Form state
  const eventForm = ref({
    id: null,
    title: '',
    description: '',
    maxSize: 10,
    maxFiles: 1,
    openTime: '2024-10-01T08:00',
    deadlineTime: '2024-10-15T23:59',
    stage: 'Giai ?o?n 2: Tri?n khai n?i dung',
    allowLate: true,
    isGraded: true,
    gradingMethod: 'rubric',
    rubricCalcMethod: 'avg'
  });

  // Dynamic Rubric Levels & Criteria state for editor
  const rubricLevels = ref(['Mức 1', 'Mức 2', 'Mức 3']);
  const editableCriteria = ref([
    {
      id: 1,
      title: 'Lập kế hoạch thực hiện',
      levels: [
        { label: 'Không hợp lý', points: 0 },
        { label: 'Chưa hợp lý', points: 0.5 },
        { label: 'Hoàn toàn hợp lý', points: 1.5 }
      ]
    },
    {
      id: 2,
      title: 'Nội dung báo cáo',
      levels: [
        { label: 'Thiếu nội dung', points: 0 },
        { label: '� ầy đủ yêu cầu', points: 1.0 },
        { label: 'Phong phú sâu sắc', points: 1.5 }
      ]
    }
  ]);

  const filteredEvents = computed(() => {
    if (activeFilter.value === 'ALL') return events.value;
    return events.value.filter(e => e.status === activeFilter.value);
  });

  const filteredSubmissions = computed(() => {
    return submissions.value.filter(s => {
      const matchStatus = submissionStatusFilter.value === 'ALL' || s.status === submissionStatusFilter.value;
      const matchGraded =
        gradingStatusFilter.value === 'ALL' ||
        (gradingStatusFilter.value === '� ã chấm' && s.isGraded) ||
        (gradingStatusFilter.value === 'Chưa chấm' && !s.isGraded);
      const matchSearch =
        !submissionSearch.value ||
        s.studentName.toLowerCase().includes(submissionSearch.value.toLowerCase()) ||
        s.mssv.includes(submissionSearch.value);
      return matchStatus && matchGraded && matchSearch;
    });
  });

  function getStatusBadgeClass(status) {
    switch (status) {
      case '� ang mở':
        return 'bg-emerald-100 text-emerald-800';
      case 'Sắp tới':
        return 'bg-amber-100 text-amber-800';
      case '� ã đóng':
        return 'bg-slate-100 text-slate-600';
      default:
        return 'bg-slate-100 text-slate-600';
    }
  }

  function subStatusClass(status) {
    switch (status) {
      case '� ã nộp':
        return 'bg-emerald-100 text-emerald-800';
      case 'Nộp trễ':
        return 'bg-amber-100 text-amber-800';
      case 'Chưa nộp':
        return 'bg-rose-100 text-rose-800';
      default:
        return 'bg-slate-100 text-slate-600';
    }
  }

  async function viewSubmissions(event) {
    activeSubmissionEvent.value = event;
    try {
      const res = await api.get(`/giangvien/events/${event.id}/submissions`);
      submissions.value = res.data.map((s, idx) => ({
        ...s,
        fileExtension: idx === 1 ? 'zip' : 'pdf'
      }));
    } catch (err) {
      console.error(err);
      showToast('Không thể tải bài nộp cho sự kiện này');
    }
  }

  function openRubricReview(sub) {
    selectedSubForReview.value = sub;
    currentSubIndex.value = filteredSubmissions.value.findIndex(s => s.id === sub.id);
    currentDirectScore.value = sub.score !== '-' ? parseFloat(sub.score) : 8.5;
    reviewComment.value = '';
    reviewTab.value = isSupportedFile.value ? 'comments' : 'overall';
    highlightComments.value = [];
    pendingHighlight.value = null;
    floatingBtn.value.visible = false;
    docSegments.value = [{ text: rawDocText, highlighted: false, comment: '' }];
    hasUnsavedChanges.value = false;
  }

  async function saveRubricGrade() {
    if (!selectedSubForReview.value) return;
    try {
      const scoreVal = parseFloat(calculatedRubricScore.value) || currentDirectScore.value;
        await api.post(`/giangvien/submissions/${selectedSubForReview.value.id}/grade`, {
          score: scoreVal
        });
        selectedSubForReview.value.isGraded = true;
        selectedSubForReview.value.score = scoreVal.toFixed(1);
        showToast(`�?ã lưu điểm ${selectedSubForReview.value.score} cho ${selectedSubForReview.value.studentName}!`);
        selectedSubForReview.value = null;
      } catch (err) {
        console.error(err);
        showToast('Có lỗi xảy ra khi lưu điểm!');
      }
    }

  function openCreateModal() {
    isEditing.value = false;
    eventForm.value = {
      id: null,
      title: '',
      description: '',
      maxSize: 10,
      maxFiles: 1,
      openTime: '2024-10-01T08:00',
      deadlineTime: '2024-10-25T23:59',
      stage: 'Giai đoạn 2: Triển khai nội dung',
      allowLate: true,
      isGraded: true,
      gradingMethod: 'rubric',
      rubricCalcMethod: 'avg'
    };
    showCreateModal.value = true;
  }

  function editEvent(event) {
    isEditing.value = true;
    eventForm.value = {
      id: event.id,
      title: event.title,
      description: 'Nộp file báo cáo định dạng PDF hoặc DOCX.',
      maxSize: 10,
      maxFiles: 1,
      openTime: '2024-10-01T08:00',
      deadlineTime: '2024-10-15T23:59',
      stage: event.stage,
      allowLate: true,
      isGraded: true,
      gradingMethod: 'rubric',
      rubricCalcMethod: 'avg'
    };
    showCreateModal.value = true;
  }

  function addCriterionRow() {
    editableCriteria.value.push({
      id: Date.now(),
      title: 'Tiêu chí mới',
      levels: rubricLevels.value.map((_, idx) => ({
        label: `Mô tả mức ${idx + 1}`,
        points: (idx + 1) * 0.5
      }))
    });
  }

  function removeCriterionRow(idx) {
    if (editableCriteria.value.length > 1) {
      editableCriteria.value.splice(idx, 1);
    }
  }

  function duplicateCriterion(idx) {
    const source = editableCriteria.value[idx];
    editableCriteria.value.splice(idx + 1, 0, {
      id: Date.now(),
      title: source.title + ' (Bản sao)',
      levels: JSON.parse(JSON.stringify(source.levels))
    });
  }

  function addRubricLevelColumn() {
    rubricLevels.value.push(`Mức ${rubricLevels.value.length + 1}`);
    editableCriteria.value.forEach(crit => {
      crit.levels.push({
        label: 'Mô tả mức mới',
        points: (crit.levels.length + 1) * 0.5
      });
    });
  }

  function removeRubricLevelColumn(lvlIdx) {
    if (rubricLevels.value.length > 2) {
      rubricLevels.value.splice(lvlIdx, 1);
      editableCriteria.value.forEach(crit => {
        crit.levels.splice(lvlIdx, 1);
      });
    }
  }

  async function saveEventForm() {
    if (!eventForm.value.title.trim()) {
      showToast('Vui lòng nhập tên sự kiện!');
      return;
    }

    try {
      if (isEditing.value) {
        const idx = events.value.findIndex(e => e.id === eventForm.value.id);
        if (idx !== -1) {
          events.value[idx].title = eventForm.value.title;
          events.value[idx].stage = eventForm.value.stage;
        }
        showToast('✓ �?ã cập nhật thiết lập sự kiện thành công!');
      } else {
        const res = await api.post(`/giangvien/classes/${classId.value}/events`, eventForm.value);
        events.value.unshift(res.data);
        showToast('✓ �?ã tạo và thiết lập sự kiện mới thành công!');
      }
      showCreateModal.value = false;
    } catch (err) {
      console.error(err);
      showToast('Có lỗi xảy ra khi lưu sự kiện!');
    }
  }

  function showToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }
</script>
