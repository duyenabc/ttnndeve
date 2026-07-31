<template>
  <div class="ims-scope w-full mx-auto space-y-6 font-sans pb-24">
    <!-- Main Task & Event Management View -->
    <div v-if="!activeSubmissionEvent" class="space-y-6">
      <!-- Breadcrumbs & Header -->
      <div>
        <nav class="flex items-center gap-2 text-xs text-slate-500 mb-2">
          <router-link to="/teacher/classes" class="hover:text-blue-700 transition-colors">Lá»›p cá»§a tÃ´i</router-link>
          <span class="material-symbols-outlined text-[14px]">chevron_right</span>
          <span class="text-slate-700 font-medium">CÃ´ng viá»‡c</span>
          <span class="material-symbols-outlined text-[14px]">chevron_right</span>
          <span class="font-bold text-slate-900">Sá»± kiá»‡n & BÃ i ná»™p</span>
        </nav>
        <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <h1 class="text-[28px] font-bold text-slate-900 tracking-tight">Quáº£n lÃ½ CÃ´ng viá»‡c & Sá»± kiá»‡n Ná»™p bÃ i</h1>
        </div>
      </div>

      <!-- Sub Navigation Tabs -->
      <div class="flex items-center gap-8 border-b border-slate-200">
        <router-link
          :to="`/teacher/classes/${classId}/diaries`"
          class="pb-3 text-slate-500 hover:text-blue-700 font-semibold text-sm transition-colors"
        >
          Nháº­t kÃ½ thá»±c táº­p
        </router-link>
        <router-link
          :to="`/teacher/classes/${classId}/tasks`"
          class="pb-3 text-blue-700 border-b-2 border-blue-700 font-bold text-sm"
        >
          Sá»± kiá»‡n & BÃ i ná»™p
        </router-link>
        <router-link
          :to="`/teacher/classes/${classId}/topics`"
          class="pb-3 text-slate-500 hover:text-blue-700 font-semibold text-sm transition-colors flex items-center gap-1.5"
        >
          PhÃª duyá»‡t Ä‘á» tÃ i
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
            Äang hiá»ƒn thá»‹ <span class="font-bold text-slate-900">{{ filteredEvents.length }}</span> sá»± kiá»‡n
          </div>
          <button
            @click="openCreateModal"
            class="bg-[#005EA3] hover:bg-blue-800 text-white px-5 py-2.5 rounded-xl flex items-center gap-2 font-bold text-xs shadow-md transition-all active:scale-95 shrink-0"
          >
            <span class="material-symbols-outlined text-[20px]">add</span>
            ThÃªm sá»± kiá»‡n
          </button>
        </div>
      </div>

      <!-- Event List Table -->
      <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-2xs">
        <div class="overflow-x-auto">
          <table class="w-full text-left border-collapse">
            <thead>
              <tr class="bg-slate-50 border-b border-slate-200 text-[11px] font-bold uppercase tracking-wider text-slate-500">
                <th class="px-6 py-3.5">TÃªn sá»± kiá»‡n</th>
                <th class="px-6 py-3.5">Háº¡n chÃ³t</th>
                <th class="px-6 py-3.5">ÄÃ£ ná»™p</th>
                <th class="px-6 py-3.5">Cháº¥m Ä‘iá»ƒm</th>
                <th class="px-6 py-3.5">Tráº¡ng thÃ¡i</th>
                <th class="px-6 py-3.5 text-right">Thao tÃ¡c</th>
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
                      title="Theo dÃµi bÃ i ná»™p & cháº¥m Ä‘iá»ƒm"
                    >
                      <span class="material-symbols-outlined text-[16px]">assignment_turned_in</span>
                      Theo dÃµi ná»™p
                    </button>
                    <button
                      @click="editEvent(event)"
                      class="p-1.5 text-slate-500 hover:text-blue-700 hover:bg-slate-100 rounded-lg transition-colors"
                      title="Thiáº¿t láº­p / Chá»‰nh sá»­a"
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
        <button @click="activeSubmissionEvent = null" class="hover:text-blue-700 font-medium">CÃ´ng viá»‡c</button>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <button @click="activeSubmissionEvent = null" class="hover:text-blue-700 font-medium">Sá»± kiá»‡n</button>
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
            <span class="flex items-center gap-1"><span class="material-symbols-outlined text-[16px]">schedule</span> Háº¡n ná»™p: {{ activeSubmissionEvent.deadline }}</span>
            <span class="flex items-center gap-1"><span class="material-symbols-outlined text-[16px]">groups</span> SÄ© sá»‘ lá»›p: {{ activeSubmissionEvent.total }} sinh viÃªn</span>
          </p>
        </div>

        <button
          @click="activeSubmissionEvent = null"
          class="px-4 py-2 border border-slate-300 text-slate-700 hover:bg-slate-100 font-bold text-xs rounded-xl transition flex items-center gap-1.5 self-start md:self-auto"
        >
          <span class="material-symbols-outlined text-[18px]">arrow_back</span>
          Quay láº¡i danh sÃ¡ch
        </button>
      </div>

      <!-- Filters Row -->
      <div class="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs flex flex-col md:flex-row justify-between items-stretch md:items-center gap-4">
        <div class="relative flex-1 max-w-md">
          <span class="material-symbols-outlined absolute left-3 top-2.5 text-slate-400 text-[18px]">search</span>
          <input
            v-model="submissionSearch"
            type="text"
            placeholder="TÃ¬m kiáº¿m theo tÃªn sinh viÃªn, MSSV..."
            class="w-full pl-10 pr-4 py-2 bg-slate-50 border border-slate-300 rounded-xl text-xs outline-none focus:ring-2 focus:ring-blue-600"
          />
        </div>

        <div class="flex flex-wrap items-center gap-3 text-xs">
          <select v-model="submissionStatusFilter" class="px-3 py-2 bg-slate-50 border border-slate-300 rounded-xl font-semibold text-slate-700 outline-none focus:ring-2 focus:ring-blue-600">
            <option value="ALL">Táº¥t cáº£ tráº¡ng thÃ¡i ná»™p</option>
            <option value="Ä Ã£ ná»™p">Ä Ã£ ná»™p</option>
            <option value="Ná»™p trá»…">Ná»™p trá»…</option>
            <option value="ChÆ°a ná»™p">ChÆ°a ná»™p</option>
          </select>

          <select v-model="gradingStatusFilter" class="px-3 py-2 bg-slate-50 border border-slate-300 rounded-xl font-semibold text-slate-700 outline-none focus:ring-2 focus:ring-blue-600">
            <option value="ALL">Tráº¡ng thÃ¡i cháº¥m Ä‘iá»ƒm</option>
            <option value="Ä Ã£ cháº¥m">Ä Ã£ cháº¥m</option>
            <option value="ChÆ°a cháº¥m">ChÆ°a cháº¥m</option>
          </select>
        </div>
      </div>

      <!-- Submission Table -->
      <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-2xs">
        <div class="overflow-x-auto">
          <table class="w-full text-left border-collapse text-xs">
            <thead>
              <tr class="bg-slate-50 border-b border-slate-200 text-[11px] font-bold uppercase tracking-wider text-slate-500">
                <th class="px-6 py-3.5">Há»  tÃªn sinh viÃªn</th>
                <th class="px-6 py-3.5">MSSV</th>
                <th class="px-6 py-3.5">Thá» i gian ná»™p</th>
                <th class="px-6 py-3.5">Tráº¡ng thÃ¡i ná»™p</th>
                <th class="px-6 py-3.5">Tráº¡ng thÃ¡i cháº¥m</th>
                <th class="px-6 py-3.5 text-center">Ä iá»ƒm sá»‘</th>
                <th class="px-6 py-3.5 text-right">Thao tÃ¡c</th>
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
                    {{ sub.isGraded ? 'Ä Ã£ cháº¥m' : 'ChÆ°a cháº¥m' }}
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
                    Cháº¥m bÃ i
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
          <button @click="selectedSubForReview = null" class="hover:bg-white/10 p-1.5 rounded-lg transition" title="Ä Ã³ng">
            <span class="material-symbols-outlined text-[20px]">arrow_back</span>
          </button>
          <div>
            <h2 class="font-bold text-sm leading-tight">Cháº¥m Ä‘iá»ƒm bÃ i ná»™p - {{ selectedSubForReview.studentName }} (MSSV: {{ selectedSubForReview.mssv }})</h2>
            <p class="text-[11px] text-blue-100">{{ activeSubmissionEvent?.title || 'BÃ¡o cÃ¡o thá»±c táº­p' }} â€¢ Ná»™p lÃºc: {{ selectedSubForReview.submittedAt }}</p>
          </div>
        </div>

        <div class="flex items-center gap-3">
          <button
            @click="navigateSubmission(-1)"
            class="px-3.5 py-1.5 bg-white/10 hover:bg-white/20 font-bold text-xs rounded-lg transition flex items-center gap-1"
          >
            <span class="material-symbols-outlined text-[16px]">arrow_back</span> Bài trước
          </button>
          <button
            @click="navigateSubmission(1)"
            class="px-3.5 py-1.5 bg-white/10 hover:bg-white/20 font-bold text-xs rounded-lg transition flex items-center gap-1"
          >
            Bài tiếp theo <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
          </button>
          <span class="text-blue-200 text-xs font-mono">{{ currentSubIndex + 1 }}/{{ filteredSubmissions.length }}</span>
          <span class="w-px h-4 bg-white/20"></span>
          <button @click="showToast('Đã lưu nháp!')" class="px-3.5 py-1.5 bg-white/10 hover:bg-white/20 font-bold text-xs rounded-lg transition">Lưu nháp</button>
          <button @click="saveRubricGrade" class="px-4 py-1.5 bg-emerald-500 hover:bg-emerald-600 font-bold text-xs rounded-lg shadow-xs transition">Lưu &amp; Bài tiếp →</button>
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
              <button @click="showToast('Ä ang táº£i file gá»‘c Ä‘Ã­nh kÃ¨m...')" class="px-3 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg font-bold flex items-center gap-1">
                <span class="material-symbols-outlined text-[16px]">download</span> Táº£i file gá»‘c
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
                <h3 class="text-lg font-black uppercase text-slate-900">Báo Cáo Tiến Độ Thực Tập Tuần 4</h3>
                <p class="text-slate-500 font-semibold">Sinh viên: {{ selectedSubForReview.studentName }} • MSSV: {{ selectedSubForReview.mssv }}</p>
              </div>

              <div class="space-y-3 leading-relaxed">
                <p class="font-bold text-sm text-slate-900">1. Nội dung công việc thực hiện trong tuần:</p>
                <p class="text-slate-700 bg-slate-50 p-3 rounded-lg border border-slate-200">
                  <span
                    v-for="(seg, i) in docSegments"
                    :key="i"
                    :class="seg.highlighted ? 'bg-amber-200 text-amber-900 rounded cursor-help' : ''"
                    :title="seg.comment || ''"
                  >{{ seg.text }}</span>
                </p>

                <p class="font-bold text-sm text-slate-900">2. Các kết quả đã đạt được:</p>
                <ul class="list-disc pl-5 space-y-1 text-slate-700">
                  <li>Hoàn thành 15 test case tự động trên Selenium với tỷ lệ thành công 95%.</li>
                  <li>Thu thập ý kiến đánh giá tiến độ từ Anh Mentor hướng dẫn tại doanh nghiệp.</li>
                  <li>Cập nhật bổ sung sơ đồ kiến trúc hệ thống vào nhật ký thực tập.</li>
                </ul>

                <p class="font-bold text-sm text-slate-900">3. Khó khăn &amp; Giải pháp đề xuất:</p>
                <p class="text-slate-700 bg-amber-50/60 p-3 rounded-lg border border-amber-200">
                  Gáº·p má»™t sá»‘ váº¥n Ä‘á»  vá»  thá» i gian pháº£n há»“i API khi cháº¡y test Ä‘á»“ng thá» i. Em Ä‘ang tá»± tÃ¬m hiá»ƒu thÃªm vá»  tá»‘i Æ°u hÃ³a connection pool vÃ  bá»™ nhá»› Ä‘á»‡m Redis.
                </p>
              </div>
            </div>
            <div v-else class="flex items-center justify-center h-full text-slate-500">
              <div class="text-center">
                <span class="material-symbols-outlined text-[64px] block mb-2 opacity-50">description</span>
                <p>File không hỗ trợ xem trước trực tiếp.</p>
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
              Nhận xét inline
              <span v-if="highlightComments.length" class="ml-1 bg-amber-400 text-white text-[10px] px-1.5 py-0.5 rounded-full">{{ highlightComments.length }}</span>
            </button>
            <button
              @click="reviewTab = 'overall'"
              class="px-5 py-3 flex items-center gap-1.5 transition"
              :class="reviewTab === 'overall' ? 'border-b-2 border-blue-700 text-blue-700 bg-white' : 'text-slate-500 hover:text-slate-800'"
            >
              <span class="material-symbols-outlined text-[16px]">rate_review</span>
              Nhận xét tổng thể &amp; Điểm
            </button>
          </div>

          <div class="flex-1 overflow-y-auto custom-scrollbar">
            <!-- TAB 1: Inline Highlight Comments -->
            <div v-if="reviewTab === 'comments'" class="p-5 space-y-4">
              <p class="text-xs text-slate-500 bg-amber-50 border border-amber-200 rounded-xl p-3 flex items-start gap-2">
                <span class="material-symbols-outlined text-amber-500 text-[18px] shrink-0">info</span>
                Bôi đen một đoạn văn bản trong tài liệu bên trái, sau đó nhấn nút <strong class="text-slate-800">"+ Comment"</strong> để tạo nhận xét gắn vào đoạn đó.
              </p>

              <!-- Pending new comment input -->
              <div v-if="pendingHighlight" class="border-2 border-amber-400 rounded-xl p-4 bg-amber-50 space-y-2 text-xs">
                <p class="font-bold text-amber-900 flex items-center gap-1">
                  <span class="material-symbols-outlined text-[15px]">format_quote</span>
                  Đoạn trích dẫn:
                </p>
                <p class="italic text-slate-600 bg-white px-3 py-2 rounded-lg border border-amber-200 line-clamp-2">"{{ pendingHighlight.text }}"</p>
                <textarea
                  v-model="pendingHighlight.draft"
                  rows="3"
                  placeholder="Nhập nhận xét của bạn (tối đa 1000 ký tự)..."
                  maxlength="1000"
                  class="w-full px-3 py-2 border border-amber-300 rounded-lg outline-none focus:ring-2 focus:ring-amber-400 text-xs resize-none"
                ></textarea>
                <div class="flex justify-end gap-2">
                  <button @click="pendingHighlight = null" class="px-3 py-1.5 text-slate-500 hover:bg-slate-100 rounded-lg font-bold transition">Hủy</button>
                  <button @click="confirmHighlightComment" class="px-4 py-1.5 bg-amber-500 hover:bg-amber-600 text-white font-bold rounded-lg transition">Lưu nhận xét</button>
                </div>
              </div>

              <!-- Empty state -->
              <div v-if="highlightComments.length === 0 && !pendingHighlight" class="text-center py-10 text-slate-400">
                <span class="material-symbols-outlined text-[40px] block mb-2">format_quote</span>
                <p class="text-xs">Chưa có nhận xét inline nào.<br>Bôi đen văn bản để bắt đầu.</p>
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
                  <span class="text-[11px] font-extrabold uppercase text-blue-800 tracking-wider">Tổng điểm bài nộp (Thang 10)</span>
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
                  <span class="text-[10px] text-slate-500 font-bold block">TỔNG ĐIỂM RUBRIC</span>
                  <span class="text-base font-black text-emerald-700">{{ calculatedRubricScore }} / 4.5 pts</span>
                </div>
              </div>

              <!-- Rubric Matrix -->
              <div class="space-y-3">
                <h3 class="font-bold text-slate-900 text-sm flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-blue-600 text-[18px]">fact_check</span>
                  Ma trận Rubric Chấm điểm
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
                  Nhận xét tổng thể (tối đa 2000 ký tự)
                </h3>
                <textarea
                  v-model="reviewComment"
                  rows="5"
                  maxlength="2000"
                  placeholder="Nhập nhận xét tổng thể chi tiết cho sinh viên..."
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
              {{ isEditing ? 'Thiáº¿t láº­p & Cáº¥u hÃ¬nh Sá»± kiá»‡n Ná»™p bÃ i' : 'ThÃªm sá»± kiá»‡n ná»™p bÃ i ná»™i bá»™ má»›i' }}
            </h2>
            <p class="text-[11px] text-blue-100">Thiáº¿t láº­p thá» i gian, thÃ´ng sá»‘ ká»¹ thuáº­t vÃ  ma tráº­n cháº¥m Ä‘iá»ƒm Rubric</p>
          </div>
          <button @click="showCreateModal = false" class="text-white/80 hover:text-white text-lg">âœ•</button>
        </div>

        <!-- Form Body (Scrollable) -->
        <div class="p-6 overflow-y-auto custom-scrollbar space-y-6 text-xs flex-1">
          <!-- SECTION 1: THÃ”NG TIN CHUNG -->
          <section class="border border-slate-200 rounded-2xl p-4 bg-white space-y-4">
            <div
              @click="toggleSection('general')"
              class="flex items-center justify-between cursor-pointer border-b pb-2 text-slate-900 font-bold text-sm"
            >
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-blue-600">info</span> 1. ThÃ´ng tin chung sá»± kiá»‡n
              </span>
              <span class="material-symbols-outlined text-slate-400">
                {{ sectionOpen.general ? 'expand_less' : 'expand_more' }}
              </span>
            </div>

            <div v-if="sectionOpen.general" class="space-y-4 pt-1">
              <div>
                <label class="block font-bold text-slate-700 mb-1">
                  TÃªn sá»± kiá»‡n <span class="text-rose-500">*</span>
                </label>
                <input
                  v-model="eventForm.title"
                  type="text"
                  required
                  placeholder="Nháº­p tÃªn sá»± kiá»‡n ná»™p tÃ i liá»‡u..."
                  class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium"
                />
                <!-- Quick Suggestion Pills -->
                <div class="flex items-center gap-2 mt-2">
                  <span class="text-[10px] text-slate-400 font-bold">Gá»£i Ã½ nhanh:</span>
                  <button
                    type="button"
                    @click="eventForm.title = 'Ná»™p Ä‘á»  cÆ°Æ¡ng chi tiáº¿t'"
                    class="px-2.5 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg text-[10px] font-bold"
                  >
                    Ná»™p Ä‘á»  cÆ°Æ¡ng
                  </button>
                  <button
                    type="button"
                    @click="eventForm.title = 'Ná»™p báº£n tháº£o bÃ¡o cÃ¡o giá»¯a ká»³'"
                    class="px-2.5 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg text-[10px] font-bold"
                  >
                    Ná»™p báº£n tháº£o
                  </button>
                  <button
                    type="button"
                    @click="eventForm.title = 'BÃ¡o cÃ¡o tá»•ng káº¿t thá»±c táº­p cuá»‘i ká»³'"
                    class="px-2.5 py-1 bg-slate-100 hover:bg-slate-200 text-slate-700 rounded-lg text-[10px] font-bold"
                  >
                    Ná»™p bÃ¡o cÃ¡o cuá»‘i cÃ¹ng
                  </button>
                </div>
              </div>

              <div>
                <label class="block font-bold text-slate-700 mb-1">MÃ´ táº£ & HÆ°á»›ng dáº«n cho sinh viÃªn</label>
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
                    placeholder="Nháº­p hÆ°á»›ng dáº«n chi tiáº¿t vá»  cáº¥u trÃºc bÃ i ná»™p, quy cÃ¡ch Ä‘áº·t tÃªn file..."
                    class="w-full p-3 border-none outline-none focus:ring-0 text-xs"
                  ></textarea>
                </div>
              </div>
            </div>
          </section>

          <!-- SECTION 2: THÃ”NG Sá»  Ká»¸ THUáº¬T -->
          <section class="border border-slate-200 rounded-2xl p-4 bg-white space-y-4">
            <div
              @click="toggleSection('specs')"
              class="flex items-center justify-between cursor-pointer border-b pb-2 text-slate-900 font-bold text-sm"
            >
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-blue-600">tune</span> 2. ThÃ´ng sá»‘ ká»¹ thuáº­t file ná»™p
              </span>
              <span class="material-symbols-outlined text-slate-400">
                {{ sectionOpen.specs ? 'expand_less' : 'expand_more' }}
              </span>
            </div>

            <div v-if="sectionOpen.specs" class="space-y-4 pt-1">
              <div>
                <label class="block font-bold text-slate-700 mb-1.5">Ä á»‹nh dáº¡ng file cho phÃ©p</label>
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
                    <input type="checkbox" class="rounded text-blue-600" /> KhÃ´ng giá»›i háº¡n
                  </label>
                </div>
              </div>

              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label class="block font-bold text-slate-700 mb-1">Dung lÆ°á»£ng tá»‘i Ä‘a (MB)</label>
                  <input
                    v-model.number="eventForm.maxSize"
                    type="number"
                    min="1"
                    max="100"
                    class="w-full px-3.5 py-2 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
                  />
                </div>
                <div>
                  <label class="block font-bold text-slate-700 mb-1">Sá»‘ lÆ°á»£ng file tá»‘i Ä‘a</label>
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

          <!-- SECTION 3: THá»œI GIAN & THIáº¾T Láº¬P -->
          <section class="border border-slate-200 rounded-2xl p-4 bg-white space-y-4">
            <div
              @click="toggleSection('time')"
              class="flex items-center justify-between cursor-pointer border-b pb-2 text-slate-900 font-bold text-sm"
            >
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-blue-600">calendar_month</span> 3. Thá» i gian & Cáº¥u hÃ¬nh ná»™p
              </span>
              <span class="material-symbols-outlined text-slate-400">
                {{ sectionOpen.time ? 'expand_less' : 'expand_more' }}
              </span>
            </div>

            <div v-if="sectionOpen.time" class="space-y-4 pt-1">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label class="block font-bold text-slate-700 mb-1">Thá» i gian má»Ÿ ná»™p</label>
                  <input
                    v-model="eventForm.openTime"
                    type="datetime-local"
                    class="w-full px-3.5 py-2 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
                  />
                </div>
                <div>
                  <label class="block font-bold text-slate-700 mb-1">Háº¡n chÃ³t ná»™p bÃ i (*)</label>
                  <input
                    v-model="eventForm.deadlineTime"
                    type="datetime-local"
                    required
                    class="w-full px-3.5 py-2 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-bold text-blue-900"
                  />
                </div>
              </div>

              <div>
                <label class="block font-bold text-slate-700 mb-1">Giai Ä‘oáº¡n thá»±c táº­p</label>
                <select v-model="eventForm.stage" class="w-full px-3.5 py-2 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium">
                  <option>Giai Ä‘oáº¡n 1: TÃ¬m kiáº¿m & Ä á»  xuáº¥t</option>
                  <option>Giai Ä‘oáº¡n 2: Triá»ƒn khai ná»™i dung</option>
                  <option>Giai Ä‘oáº¡n 3: HoÃ n thiá»‡n & BÃ¡o cÃ¡o</option>
                </select>
              </div>

              <div class="flex items-center gap-3 pt-1">
                <input v-model="eventForm.allowLate" type="checkbox" id="lateToggle" class="rounded text-blue-600 focus:ring-blue-600" />
                <label for="lateToggle" class="font-bold text-slate-700 cursor-pointer">Cho phÃ©p ná»™p trá»… (Ghi nháº­n Ä‘iá»ƒm trá»« trá»…)</label>
              </div>
            </div>
          </section>

          <!-- SECTION 4: Cáº¤U HÃŒNH RUBRIC CHáº¤M Ä Iá»‚M -->
          <section class="border border-slate-200 rounded-2xl p-4 bg-white space-y-4">
            <div
              @click="toggleSection('grading')"
              class="flex items-center justify-between cursor-pointer border-b pb-2 text-slate-900 font-bold text-sm"
            >
              <span class="flex items-center gap-2">
                <span class="material-symbols-outlined text-blue-600">grading</span> 4. PhÆ°Æ¡ng thá»©c cháº¥m Ä‘iá»ƒm & Ma tráº­n Rubric
              </span>
              <span class="material-symbols-outlined text-slate-400">
                {{ sectionOpen.grading ? 'expand_less' : 'expand_more' }}
              </span>
            </div>

            <div v-if="sectionOpen.grading" class="space-y-4 pt-1">
              <div class="flex items-center justify-between bg-slate-50 p-3 rounded-xl border border-slate-200">
                <label class="flex items-center gap-2 font-bold text-slate-800 cursor-pointer">
                  <input v-model="eventForm.isGraded" type="checkbox" class="rounded text-blue-600" />
                  Sá»± kiá»‡n nÃ y cÃ³ tÃ­nh Ä‘iá»ƒm
                </label>

                <!-- Method Switcher -->
                <div v-if="eventForm.isGraded" class="flex bg-slate-200 p-1 rounded-lg text-xs font-bold">
                  <button
                    type="button"
                    @click="eventForm.gradingMethod = 'direct'"
                    class="px-3 py-1 rounded-md transition"
                    :class="eventForm.gradingMethod === 'direct' ? 'bg-white text-blue-700 shadow-2xs' : 'text-slate-600'"
                  >
                    Ä iá»ƒm trá»±c tiáº¿p
                  </button>
                  <button
                    type="button"
                    @click="eventForm.gradingMethod = 'rubric'"
                    class="px-3 py-1 rounded-md transition"
                    :class="eventForm.gradingMethod === 'rubric' ? 'bg-white text-blue-700 shadow-2xs' : 'text-slate-600'"
                  >
                    Ma tráº­n Rubric
                  </button>
                </div>
              </div>

              <!-- Interactive Rubric Matrix Builder -->
              <div v-if="eventForm.isGraded && eventForm.gradingMethod === 'rubric'" class="space-y-4 border border-blue-200 p-4 bg-blue-50/20 rounded-2xl">
                <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-2">
                  <h4 class="font-bold text-slate-900 text-xs flex items-center gap-1.5">
                    <span class="material-symbols-outlined text-blue-600 text-[18px]">table_chart</span>
                    Thiáº¿t láº­p ma tráº­n Rubric
                  </h4>

                  <div class="flex items-center gap-2">
                    <label class="text-[11px] font-bold text-slate-600">CÃ¡ch tÃ­nh:</label>
                    <select v-model="eventForm.rubricCalcMethod" class="px-2.5 py-1 bg-white border border-slate-300 rounded-lg text-[11px] font-bold text-slate-800">
                      <option value="avg">Trung bÃ¬nh cá»™ng cÃ¡c tiÃªu chÃ­</option>
                      <option value="sum">Tá»•ng Ä‘iá»ƒm cÃ¡c tiÃªu chÃ­</option>
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
                          <th class="p-3 w-1/3 border-r border-slate-200">TiÃªu chÃ­ Ä‘Ã¡nh giÃ¡</th>
                          <th
                            v-for="(lvl, lIdx) in rubricLevels"
                            :key="lIdx"
                            class="p-3 border-r border-slate-200 min-w-[130px] relative group"
                          >
                            <span>Má»©c {{ lIdx + 1 }}</span>
                            <button
                              v-if="rubricLevels.length > 2"
                              type="button"
                              @click="removeRubricLevelColumn(lIdx)"
                              class="absolute top-1 right-1 text-rose-500 hover:text-rose-700 font-black text-xs hidden group-hover:block"
                              title="XÃ³a má»©c"
                            >
                              âœ•
                            </button>
                          </th>
                          <th class="p-3 text-center w-28">Thao tÃ¡c</th>
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
                              placeholder="Nháº­p tÃªn tiÃªu chÃ­ / háº¡ng má»¥c con..."
                              class="w-full p-1.5 border border-slate-300 rounded-lg outline-none focus:ring-1 focus:ring-blue-600 font-bold text-slate-800"
                            />
                            <div class="flex items-center gap-2 text-[10px]">
                              <div class="flex items-center gap-1 bg-slate-100 px-1.5 py-0.5 rounded border border-slate-200">
                                <span class="font-bold text-slate-600">Tá»· lá»‡:</span>
                                <input v-model.number="crit.weight" type="number" min="0" max="100" class="w-10 bg-white border rounded text-center font-bold text-blue-700" />
                                <span class="font-bold text-slate-600">%</span>
                              </div>
                              <div class="flex items-center gap-1 bg-amber-50 px-1.5 py-0.5 rounded border border-amber-200">
                                <span class="font-bold text-amber-900">Ä iá»ƒm liá»‡t:</span>
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
                              placeholder="MÃ´ táº£ má»©c..."
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
                                title="NhÃ¢n báº£n"
                              >
                                <span class="material-symbols-outlined text-[18px]">content_copy</span>
                              </button>
                              <button
                                type="button"
                                @click="removeCriterionRow(rIdx)"
                                class="p-1 text-slate-500 hover:text-rose-600"
                                title="XÃ³a tiÃªu chÃ­"
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
                    <span class="material-symbols-outlined text-[16px]">add</span> ThÃªm tiÃªu chÃ­
                  </button>

                  <button
                    type="button"
                    @click="addRubricLevelColumn"
                    class="px-3.5 py-1.5 bg-white border border-slate-300 hover:bg-slate-50 font-bold rounded-xl text-xs flex items-center gap-1 text-slate-700 shadow-2xs"
                  >
                    <span class="material-symbols-outlined text-[16px]">view_column</span> ThÃªm cá»™t Má»©c
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
            Hủy bỏ
          </button>
          <button
            type="button"
            @click="saveEventForm"
            class="px-6 py-2 bg-[#005EA3] hover:bg-blue-800 text-white font-bold text-xs rounded-xl shadow-md transition flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[18px]">save</span>
            Lưu & Áp dụng sự kiện
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
  const rawDocText = 'Trong tuần qua tại doanh nghiệp, em đã tiếp tục nghiên cứu cấu trúc hệ thống backend, xây dựng bộ test case kiểm thử tự động cho các API giao dịch chính và hoàn thành báo cáo giữa kỳ.';

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
    showToast('Đã lưu nhận xét inline thành công!');
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
      const confirmed = confirm('Bạn chưa lưu điểm/nhận xét. Bỏ qua và chuyển bài?');
      if (!confirmed) return;
    }
    const list = filteredSubmissions.value;
    const nextIdx = currentSubIndex.value + dir;
    if (nextIdx < 0 || nextIdx >= list.length) {
      showToast('Đã đến đầu/cuối danh sách bài nộp!');
      return;
    }
    currentSubIndex.value = nextIdx;
    openRubricReview(list[nextIdx]);
  }


  const rubricCriteria = ref([
    {
      id: 1,
      title: 'Láº­p káº¿ hoáº¡ch thá»±c hiá»‡n',
      selectedIndex: 3,
      levels: [
        { label: 'KhÃ´ng há»£p lÃ½', points: 0 },
        { label: 'ChÆ°a há»£p lÃ½', points: 1.0 },
        { label: 'KhÃ¡ há»£p lÃ½', points: 1.25 },
        { label: 'HoÃ n toÃ n há»£p lÃ½', points: 1.5 }
      ]
    },
    {
      id: 2,
      title: 'Giai Ä‘oáº¡n thá»±c hiá»‡n',
      selectedIndex: 2,
      levels: [
        { label: 'Cháº­m trá»… nhiá» u', points: 0 },
        { label: 'Ä Ãºng tiáº¿n Ä‘á»™ Ã­t', points: 0.5 },
        { label: 'CÃ³ cháº­m trá»… nháº¹', points: 0.75 },
        { label: 'Xuáº¥t sáº¯c', points: 1.5 }
      ]
    },
    {
      id: 3,
      title: 'Ná»™i dung bÃ¡o cÃ¡o',
      selectedIndex: 3,
      levels: [
        { label: 'Thiáº¿u nhiá» u ná»™i dung', points: 0 },
        { label: 'KhÃ¡ Ä‘áº§y Ä‘á»§', points: 0.5 },
        { label: 'Ä áº§y Ä‘á»§ yÃªu cáº§u', points: 1.0 },
        { label: 'Phong phÃº sÃ¢u sáº¯c', points: 1.5 }
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
    { value: 'ALL', label: 'Táº¥t cáº£' },
    { value: 'Ä ang má»Ÿ', label: 'Ä ang má»Ÿ' },
    { value: 'Sáº¯p tá»›i', label: 'Sáº¯p tá»›i' },
    { value: 'Ä Ã£ Ä‘Ã³ng', label: 'Ä Ã£ Ä‘Ã³ng' }
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
      showToast('KhÃ´ng thá»ƒ táº£i danh sÃ¡ch sá»± kiá»‡n');
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
    stage: 'Giai đoạn 2: Triển khai nội dung',
    allowLate: true,
    isGraded: true,
    gradingMethod: 'rubric',
    rubricCalcMethod: 'avg'
  });

  // Dynamic Rubric Levels & Criteria state for editor
  const rubricLevels = ref(['Má»©c 1', 'Má»©c 2', 'Má»©c 3']);
  const editableCriteria = ref([
    {
      id: 1,
      title: 'Láº­p káº¿ hoáº¡ch thá»±c hiá»‡n',
      levels: [
        { label: 'KhÃ´ng há»£p lÃ½', points: 0 },
        { label: 'ChÆ°a há»£p lÃ½', points: 0.5 },
        { label: 'HoÃ n toÃ n há»£p lÃ½', points: 1.5 }
      ]
    },
    {
      id: 2,
      title: 'Ná»™i dung bÃ¡o cÃ¡o',
      levels: [
        { label: 'Thiáº¿u ná»™i dung', points: 0 },
        { label: 'Ä áº§y Ä‘á»§ yÃªu cáº§u', points: 1.0 },
        { label: 'Phong phÃº sÃ¢u sáº¯c', points: 1.5 }
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
        (gradingStatusFilter.value === 'Ä Ã£ cháº¥m' && s.isGraded) ||
        (gradingStatusFilter.value === 'ChÆ°a cháº¥m' && !s.isGraded);
      const matchSearch =
        !submissionSearch.value ||
        s.studentName.toLowerCase().includes(submissionSearch.value.toLowerCase()) ||
        s.mssv.includes(submissionSearch.value);
      return matchStatus && matchGraded && matchSearch;
    });
  });

  function getStatusBadgeClass(status) {
    switch (status) {
      case 'Ä ang má»Ÿ':
        return 'bg-emerald-100 text-emerald-800';
      case 'Sáº¯p tá»›i':
        return 'bg-amber-100 text-amber-800';
      case 'Ä Ã£ Ä‘Ã³ng':
        return 'bg-slate-100 text-slate-600';
      default:
        return 'bg-slate-100 text-slate-600';
    }
  }

  function subStatusClass(status) {
    switch (status) {
      case 'Ä Ã£ ná»™p':
        return 'bg-emerald-100 text-emerald-800';
      case 'Ná»™p trá»…':
        return 'bg-amber-100 text-amber-800';
      case 'ChÆ°a ná»™p':
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
      showToast('KhÃ´ng thá»ƒ táº£i bÃ i ná»™p cho sá»± kiá»‡n nÃ y');
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
        const scoreVal = parseFloat(calculatedRubricScore.value) || currentDirectScore.value;
        await api.post(`/giangvien/submissions/${selectedSubForReview.value.id}/grade`, {
          score: scoreVal
        });
        selectedSubForReview.value.isGraded = true;
        selectedSubForReview.value.score = scoreVal.toFixed(1);
        showToast(`ÄÃ£ lÆ°u Ä‘iá»ƒm ${selectedSubForReview.value.score} cho ${selectedSubForReview.value.studentName}!`);
        selectedSubForReview.value = null;
      } catch (err) {
        console.error(err);
        showToast('CÃ³ lá»—i xáº£y ra khi lÆ°u Ä‘iá»ƒm!');
      }
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
      stage: 'Giai Ä‘oáº¡n 2: Triá»ƒn khai ná»™i dung',
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
      description: 'Ná»™p file bÃ¡o cÃ¡o Ä‘á»‹nh dáº¡ng PDF hoáº·c DOCX.',
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
      title: 'TiÃªu chÃ­ má»›i',
      levels: rubricLevels.value.map((_, idx) => ({
        label: `MÃ´ táº£ má»©c ${idx + 1}`,
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
      title: source.title + ' (Báº£n sao)',
      levels: JSON.parse(JSON.stringify(source.levels))
    });
  }

  function addRubricLevelColumn() {
    rubricLevels.value.push(`Má»©c ${rubricLevels.value.length + 1}`);
    editableCriteria.value.forEach(crit => {
      crit.levels.push({
        label: 'MÃ´ táº£ má»©c má»›i',
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
      showToast('Vui lÃ²ng nháº­p tÃªn sá»± kiá»‡n!');
      return;
    }

    try {
      if (isEditing.value) {
        const idx = events.value.findIndex(e => e.id === eventForm.value.id);
        if (idx !== -1) {
          events.value[idx].title = eventForm.value.title;
          events.value[idx].stage = eventForm.value.stage;
        }
        showToast('âœ“ ÄÃ£ cáº­p nháº­t thiáº¿t láº­p sá»± kiá»‡n thÃ nh cÃ´ng!');
      } else {
        const res = await api.post(`/giangvien/classes/${classId.value}/events`, eventForm.value);
        events.value.unshift(res.data);
        showToast('âœ“ ÄÃ£ táº¡o vÃ  thiáº¿t láº­p sá»± kiá»‡n má»›i thÃ nh cÃ´ng!');
      }
      showCreateModal.value = false;
    } catch (err) {
      console.error(err);
      showToast('CÃ³ lá»—i xáº£y ra khi lÆ°u sá»± kiá»‡n!');
    }
  }

  function showToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }
</script>
