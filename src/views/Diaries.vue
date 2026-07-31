<template>
  <div class="ims-scope max-w-6xl mx-auto font-sans space-y-6 pb-20">
    <!-- Breadcrumb & Header -->
    <div class="space-y-3">
      <nav class="flex items-center text-xs text-slate-500 gap-1.5">
        <router-link to="/student/classes" class="hover:text-[#005EA3] transition font-medium">Lớp của tôi</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="hover:text-[#005EA3] transition font-medium">Lớp Thực tập K64</span>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-900 font-bold">Nhật ký thực tập</span>
      </nav>

      <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs">
        <div>
          <h1 class="text-2xl font-extrabold text-slate-900 tracking-tight flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3] text-[28px]">menu_book</span>
            Nhật ký thực tập
          </h1>
          <p class="text-xs text-slate-500 mt-1">Ghi chép công việc thực tập hàng tuần và nhận phản hồi từ Giảng viên hướng dẫn.</p>
        </div>

        <button
          @click="openNewJournalModal"
          :disabled="isPastDeadline"
          :title="isPastDeadline ? 'Đã hết hạn nộp nhật ký tuần này' : 'Viết nhật ký mới'"
          class="px-5 py-2.5 bg-[#005EA3] hover:bg-blue-800 disabled:opacity-50 disabled:cursor-not-allowed text-white font-bold rounded-xl text-xs shadow-md transition-all active:scale-95 flex items-center gap-2 w-fit"
        >
          <span class="material-symbols-outlined text-[18px]">edit_note</span>
          Viết nhật ký mới
        </button>
      </div>
    </div>

    <!-- Weekly Navigation & Progress Card -->
    <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs flex flex-col md:flex-row items-center justify-between gap-6">
      <div class="flex items-center gap-3">
        <button
          @click="switchWeek(selectedWeek - 1)"
          :disabled="selectedWeek <= 1"
          class="p-2 hover:bg-slate-100 disabled:opacity-30 rounded-xl border border-slate-200 text-slate-700 transition"
        >
          <span class="material-symbols-outlined text-[20px]">arrow_back</span>
        </button>

        <div class="text-center px-4 py-1.5 bg-slate-50 border border-slate-200 rounded-xl">
          <p class="font-extrabold text-slate-900 text-sm">Tuần {{ selectedWeek }}</p>
          <p class="text-[11px] text-slate-500 font-medium">{{ currentWeekRange }}</p>
        </div>

        <button
          @click="switchWeek(selectedWeek + 1)"
          :disabled="selectedWeek >= 12"
          class="p-2 hover:bg-slate-100 disabled:opacity-30 rounded-xl border border-slate-200 text-slate-700 transition"
        >
          <span class="material-symbols-outlined text-[20px]">arrow_forward</span>
        </button>
      </div>

      <div class="flex-1 max-w-md w-full space-y-1.5">
        <div class="flex justify-between items-center text-xs">
          <span class="font-semibold text-slate-600">Tiến độ nộp nhật ký</span>
          <span class="font-bold text-[#005EA3]">{{ submittedCount }}/3 bài tuần này</span>
        </div>
        <div class="w-full bg-slate-100 h-2.5 rounded-full overflow-hidden">
          <div
            class="bg-[#005EA3] h-full rounded-full transition-all duration-500"
            :style="{ width: (submittedCount / 3) * 100 + '%' }"
          ></div>
        </div>
      </div>
    </div>

    <!-- Journal Items List -->
    <div class="space-y-4">
      <!-- Empty State when no journals for current week -->
      <div v-if="!currentJournals.length" class="bg-white border border-slate-200 rounded-2xl p-10 text-center space-y-4 shadow-2xs">
        <div class="w-16 h-16 bg-blue-50 text-[#005EA3] rounded-full flex items-center justify-center mx-auto border border-blue-100">
          <span class="material-symbols-outlined text-[32px]">menu_book</span>
        </div>
        <div class="space-y-1.5 max-w-md mx-auto">
          <h3 class="font-extrabold text-slate-900 text-base">Chưa có nhật ký thực tập cho Tuần {{ selectedWeek }}</h3>
          <p class="text-xs text-slate-500 leading-relaxed">
            Bạn chưa viết bài nhật ký nào cho tuần này. Bấm vào nút bên dưới để tạo bài nhật ký mới và nộp cho Giảng viên hướng dẫn.
          </p>
        </div>
        <button
          @click="openNewJournalModal"
          :disabled="isPastDeadline"
          :title="isPastDeadline ? 'Đã hết hạn nộp nhật ký tuần này' : 'Viết nhật ký'"
          class="px-5 py-2.5 bg-[#005EA3] hover:bg-blue-800 disabled:opacity-50 disabled:cursor-not-allowed text-white font-bold rounded-xl text-xs shadow-md transition-all active:scale-95 inline-flex items-center gap-2"
        >
          <span class="material-symbols-outlined text-[18px]">edit_note</span>
          Viết nhật ký Tuần {{ selectedWeek }}
        </button>
      </div>

      <div
        v-for="item in currentJournals"
        :key="item.id"
        class="bg-white border border-slate-200 rounded-2xl p-5 hover:shadow-md transition space-y-3"
      >
        <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-3 border-b border-slate-100 pb-3">
          <div class="flex items-center gap-3">
            <span class="font-extrabold text-slate-900 text-sm">{{ item.date }}</span>
            <span
              v-if="item.status === 'submitted'"
              class="px-2.5 py-0.5 rounded-full bg-emerald-100 text-emerald-800 text-[10px] font-bold uppercase"
            >
              Đã nộp
            </span>
            <span
              v-else
              class="px-2.5 py-0.5 rounded-full bg-slate-200 text-slate-700 text-[10px] font-bold uppercase"
            >
              Nháp
            </span>

            <span
              v-if="item.feedback"
              class="px-2.5 py-0.5 rounded-full bg-amber-100 text-amber-900 text-[10px] font-bold uppercase flex items-center gap-1"
            >
              <span class="material-symbols-outlined text-[12px]">chat</span> Có phản hồi
            </span>
          </div>

          <span class="text-[11px] text-slate-400">Cập nhật: {{ item.updatedAt }}</span>
        </div>

        <p class="text-xs text-slate-800 leading-relaxed font-medium line-clamp-3">
          {{ item.content }}
        </p>

        <!-- Attached Images Grid in Journal Card -->
        <div v-if="item.images && item.images.length" class="space-y-1.5 pt-1">
          <p class="font-bold text-slate-500 uppercase text-[10px] tracking-wider flex items-center gap-1">
            <span class="material-symbols-outlined text-[14px] text-[#005EA3]">photo_library</span>
            Hình ảnh minh chứng ({{ item.images.length }} hình)
          </p>
          <div class="grid grid-cols-2 sm:grid-cols-4 md:grid-cols-5 gap-3.5 p-3 bg-slate-50/80 rounded-xl border border-slate-200/80">
            <div
              v-for="(img, imgIdx) in item.images"
              :key="imgIdx"
              class="relative group aspect-square rounded-xl overflow-hidden border border-slate-200 shadow-2xs bg-white cursor-pointer hover:border-blue-500 transition-all p-1"
              @click="openImagePreview(img)"
            >
              <img :src="img.url || img" class="w-full h-full object-cover rounded-lg block transition-transform duration-300 group-hover:scale-105" />
              <div class="absolute inset-0 bg-slate-900/30 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center rounded-lg">
                <span class="material-symbols-outlined text-white text-[20px]">zoom_in</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Ratings summary & AI Auto-Summary -->
        <div class="flex flex-wrap items-center justify-between gap-4 text-xs pt-1">
          <div class="flex items-center gap-4 text-slate-600 text-[11px]">
            <span>Hoàn thành: <strong class="text-slate-900">{{ item.completionRating }}/10</strong></span>
            <span>|</span>
            <span>Cảm nhận: <strong class="text-slate-900">{{ item.feelingRating }}/5</strong></span>
          </div>

          <div class="flex items-center gap-3">
            <button
              v-if="item.status === 'draft' && !isPastDeadline"
              @click="editDraft(item)"
              class="text-amber-600 font-bold text-xs hover:underline flex items-center gap-1"
            >
              Tiếp tục chỉnh sửa <span class="material-symbols-outlined text-[16px]">edit</span>
            </button>
            <button
              @click="openDetailDrawer(item)"
              class="text-[#005EA3] font-bold text-xs hover:underline flex items-center gap-1"
            >
              Xem chi tiết <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
            </button>
          </div>
        </div>

        <!-- Instructor Feedback Block if exists -->
        <div v-if="item.feedback" class="p-3 bg-amber-50/80 border border-amber-200 rounded-xl text-xs text-amber-950 space-y-1">
          <div class="flex items-center gap-1.5 font-bold text-amber-900">
            <span class="material-symbols-outlined text-[16px]">record_voice_over</span>
            Phản hồi từ {{ item.feedbackTeacher }}:
          </div>
          <p class="italic text-[11px]">"{{ item.feedback }}"</p>
        </div>
      </div>
    </div>

    <!-- DETAIL DRAWER -->
    <div
      v-if="showDrawer"
      class="fixed inset-0 z-[110] flex justify-end bg-slate-900/40 backdrop-blur-xs font-sans"
      @click.self="showDrawer = false"
    >
      <div class="bg-white w-full max-w-lg h-full overflow-y-auto p-6 space-y-6 shadow-2xl flex flex-col justify-between">
        <div class="space-y-6">
          <div class="flex justify-between items-center border-b pb-4">
            <div>
              <h3 class="font-extrabold text-slate-900 text-base">Chi tiết Nhật ký thực tập</h3>
              <p class="text-xs text-[#005EA3] font-bold mt-0.5">{{ selectedDrawerItem?.date }} — Tuần {{ selectedWeek }}</p>
            </div>
            <button @click="showDrawer = false" class="text-slate-400 hover:text-slate-600">✕</button>
          </div>

          <div class="grid grid-cols-2 gap-4 text-xs">
            <div class="p-3 bg-slate-50 rounded-xl border text-center">
              <span class="text-slate-400 font-bold block">Mức độ hoàn thành</span>
              <span class="text-lg font-black text-[#005EA3] mt-0.5 block">{{ selectedDrawerItem?.completionRating }}/10</span>
            </div>
            <div class="p-3 bg-slate-50 rounded-xl border text-center">
              <span class="text-slate-400 font-bold block">Cảm nhận chung</span>
              <span class="text-lg font-black text-[#005EA3] mt-0.5 block">{{ selectedDrawerItem?.feelingRating }}/5</span>
            </div>
          </div>

          <div class="space-y-4 text-xs">
            <div>
              <label class="font-bold text-slate-400 uppercase text-[10px] tracking-wider block">Mô tả nhiệm vụ</label>
              <p class="text-slate-800 font-medium mt-1 leading-relaxed bg-slate-50 p-3 rounded-xl border border-slate-100">
                {{ selectedDrawerItem?.content }}
              </p>
            </div>

            <div>
              <label class="font-bold text-slate-400 uppercase text-[10px] tracking-wider block">Kiến thức / Kỹ năng mới</label>
              <p class="text-slate-800 font-medium mt-1 leading-relaxed bg-slate-50 p-3 rounded-xl border border-slate-100">
                Sử dụng Chrome DevTools để debug layout trên mobile, kỹ thuật lazy loading cho hình ảnh, tối ưu hóa API call.
              </p>
            </div>

            <div>
              <label class="font-bold text-slate-400 uppercase text-[10px] tracking-wider block">Khó khăn & Giải pháp</label>
              <p class="text-slate-800 font-medium mt-1 leading-relaxed bg-slate-50 p-3 rounded-xl border border-slate-100">
                Một số component cũ chưa responsive → Đã refactor sang Tailwind CSS Flex/Grid.
              </p>
            </div>

            <div>
              <label class="font-bold text-slate-400 uppercase text-[10px] tracking-wider block mb-1">Minh chứng đính kèm</label>
              <div v-if="selectedDrawerItem?.images && selectedDrawerItem.images.length" class="space-y-2 mb-3">
                <span class="text-xs font-bold text-slate-700 block">Hình ảnh minh chứng ({{ selectedDrawerItem.images.length }} hình)</span>
                <div class="grid grid-cols-2 sm:grid-cols-3 gap-3.5 p-3 bg-slate-50 rounded-xl border border-slate-200">
                  <div
                    v-for="(img, imgIdx) in selectedDrawerItem.images"
                    :key="imgIdx"
                    class="relative group aspect-square rounded-xl overflow-hidden border border-slate-200 shadow-2xs bg-white cursor-pointer hover:border-blue-500 transition-all p-1"
                    @click="openImagePreview(img)"
                  >
                    <img :src="img.url || img" class="w-full h-full object-cover rounded-lg block transition-transform duration-300 group-hover:scale-105" />
                    <div class="absolute inset-0 bg-slate-900/30 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center rounded-lg">
                      <span class="material-symbols-outlined text-white text-[20px]">zoom_in</span>
                    </div>
                  </div>
                </div>
              </div>

              <div class="p-3 border border-slate-200 rounded-xl flex items-center justify-between bg-slate-50">
                <div class="flex items-center gap-2">
                  <span class="material-symbols-outlined text-[#005EA3]">description</span>
                  <span class="font-bold text-slate-800 text-xs">Bao_cao_tuan_{{ selectedWeek }}.pdf</span>
                </div>
                <button class="text-[#005EA3] font-bold text-xs hover:underline">Tải về</button>
              </div>
            </div>
          </div>
        </div>

        <button @click="showDrawer = false" class="w-full py-2.5 bg-slate-100 font-bold text-xs text-slate-700 rounded-xl hover:bg-slate-200">
          Đóng
        </button>
      </div>
    </div>

    <!-- NEW JOURNAL MODAL (Viết nhật ký mới) -->
    <div
      v-if="showNewModal"
      id="new-journal-modal-container"
      class="fixed inset-0 z-[120] flex items-center justify-center p-4 bg-slate-900/60 backdrop-blur-xs font-sans"
      @click.self="showNewModal = false"
    >
      <div 
        id="new-journal-modal-card"
        class="bg-white rounded-2xl shadow-2xl w-full max-w-3xl max-h-[92vh] flex flex-col overflow-hidden border border-slate-100 animate-in fade-in-50 zoom-in-95 duration-200"
      >
        <!-- Modal Header -->
        <div id="new-journal-modal-header" class="flex justify-between items-center px-6 py-4 border-b border-slate-100 bg-slate-50/50 shrink-0">
          <div>
            <h3 id="new-journal-modal-title" class="font-extrabold text-slate-900 text-base flex items-center gap-2">
              <span class="material-symbols-outlined text-[#005EA3] text-[22px]">edit_note</span>
              Viết nhật ký thực tập mới
            </h3>
            <p class="text-[11px] text-slate-500 mt-0.5 font-medium">Báo cáo tiến độ: <span class="text-[#005EA3] font-extrabold">Tuần {{ selectedWeek }}</span> ({{ currentWeekRange }})</p>
          </div>
          <button @click="showNewModal = false" class="text-slate-400 hover:text-slate-600 hover:bg-slate-100 p-1.5 rounded-full transition flex items-center justify-center">
            <span class="material-symbols-outlined text-[18px]">close</span>
          </button>
        </div>

        <!-- Scrollable Modal Body -->
        <div id="new-journal-modal-body" class="overflow-y-auto p-6 space-y-5 flex-1 scroll-smooth">
          
          <!-- Validation Alert Banner -->
          <div 
            v-if="showValidationAlert" 
            id="validation-alert-banner"
            class="p-3.5 bg-rose-50 border border-rose-200 rounded-xl flex items-start gap-3 text-xs text-rose-950 animate-in fade-in slide-in-from-top-1 duration-200"
          >
            <span class="material-symbols-outlined text-rose-600 shrink-0 text-[18px]">warning</span>
            <div class="space-y-0.5">
              <p class="font-bold">Lỗi nhập liệu! Vui lòng kiểm tra lại:</p>
              <ul class="list-disc list-inside text-[11px] text-rose-800 space-y-0.5">
                <li v-if="validationErrors.taskDesc">Mô tả nhiệm vụ không được bỏ trống và phải tối thiểu 10 ký tự.</li>
                <li v-if="validationErrors.skills">Kiến thức / kỹ năng mới không được bỏ trống.</li>
                <li v-if="validationErrors.nextPlan">Kế hoạch tuần kế tiếp không được bỏ trống.</li>
              </ul>
            </div>
          </div>

          <!-- Section 1: Overview and Feeling Ratings -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-5 p-4 bg-blue-50/40 border border-blue-100 rounded-2xl">
            <!-- Completion Segmented Control -->
            <div id="completion-rating-wrapper" class="space-y-1.5">
              <label class="block font-bold text-slate-800 text-[11px] uppercase tracking-wider">Mức độ hoàn thành công việc (*)</label>
              <div class="flex flex-wrap items-center gap-1">
                <button
                  v-for="num in 10"
                  :key="'comp-btn-' + num"
                  type="button"
                  @click="formCompletion = num"
                  class="w-8 h-8 rounded-lg font-extrabold text-xs flex items-center justify-center transition-all border"
                  :class="formCompletion === num 
                    ? 'bg-[#005EA3] text-white border-[#005EA3] shadow-xs scale-105' 
                    : 'bg-white hover:bg-slate-50 text-slate-700 border-slate-200/80'"
                >
                  {{ num }}
                </button>
              </div>
              <p class="text-[10px] text-slate-500 font-medium pt-0.5">
                {{ completionLabel }}
              </p>
            </div>

            <!-- Feeling Grid Controls -->
            <div id="feeling-rating-wrapper" class="space-y-1.5">
              <label class="block font-bold text-slate-800 text-[11px] uppercase tracking-wider">Cảm nhận tuần này (*)</label>
              <div class="grid grid-cols-5 gap-1.5">
                <button
                  v-for="f in feelings"
                  :key="'feel-btn-' + f.val"
                  type="button"
                  @click="formFeeling = f.val"
                  class="p-1.5 rounded-xl border text-center transition-all flex flex-col items-center gap-0.5"
                  :class="formFeeling === f.val 
                    ? 'bg-blue-100/60 border-[#005EA3] text-[#005EA3] font-bold shadow-xs scale-102' 
                    : 'bg-white hover:bg-slate-50 border-slate-200/80 text-slate-500'"
                >
                  <span class="text-base">{{ f.emoji }}</span>
                  <span class="text-[9px] truncate w-full">{{ f.label }}</span>
                </button>
              </div>
              <p class="text-[10px] text-slate-400 font-medium text-right pt-0.5">Hãy chọn biểu tượng phù hợp với cảm xúc của em.</p>
            </div>
          </div>

          <!-- Section 2: Core Log Textareas -->
          <div class="space-y-4">
            
            <!-- Task Description -->
            <div id="task-desc-wrapper" class="space-y-1">
              <div class="flex items-center justify-between">
                <label class="block font-bold text-slate-800 text-[11px] uppercase tracking-wider flex items-center gap-1">
                  <span class="material-symbols-outlined text-[#005EA3] text-[16px]">task_alt</span>
                  Mô tả nhiệm vụ đã thực hiện (*)
                </label>
                <span class="text-[10px] font-mono text-slate-400">
                  {{ formTaskDesc.length }} ký tự (Min 10)
                </span>
              </div>
              <textarea
                id="task-desc-textarea"
                v-model="formTaskDesc"
                rows="3"
                placeholder="Ghi chi tiết các công việc cụ thể đã làm trong tuần này (ví dụ: xây dựng giao diện đăng ký, viết API lấy danh sách sinh viên, tối ưu hóa database...)"
                class="w-full px-3.5 py-2.5 rounded-xl border outline-none text-xs transition-all placeholder:text-slate-400 leading-relaxed"
                :class="validationErrors.taskDesc ? 'border-rose-400 focus:ring-2 focus:ring-rose-100' : 'border-slate-300 focus:ring-2 focus:ring-blue-100 focus:border-[#005EA3]'"
              ></textarea>
              <p v-if="validationErrors.taskDesc" class="text-[10px] text-rose-600 flex items-center gap-1 font-semibold">
                <span class="material-symbols-outlined text-[13px]">error</span>
                Vui lòng điền mô tả nhiệm vụ chi tiết hơn (tối thiểu 10 ký tự).
              </p>
            </div>

            <!-- Skills Learned -->
            <div id="skills-learned-wrapper" class="space-y-1">
              <div class="flex items-center justify-between">
                <label class="block font-bold text-slate-800 text-[11px] uppercase tracking-wider flex items-center gap-1">
                  <span class="material-symbols-outlined text-[#005EA3] text-[16px]">school</span>
                  Kiến thức / Kỹ năng mới học được (*)
                </label>
                <span class="text-[10px] font-mono text-slate-400">
                  {{ formSkills.length }} ký tự
                </span>
              </div>
              <textarea
                id="skills-learned-textarea"
                v-model="formSkills"
                rows="2"
                placeholder="Nghiên cứu quy trình nghiệp vụ mới, học cách tối ưu code, kỹ năng debug, giải thuật mới hoặc kỹ năng giao tiếp, làm việc nhóm..."
                class="w-full px-3.5 py-2.5 rounded-xl border outline-none text-xs transition-all placeholder:text-slate-400 leading-relaxed"
                :class="validationErrors.skills ? 'border-rose-400 focus:ring-2 focus:ring-rose-100' : 'border-slate-300 focus:ring-2 focus:ring-blue-100 focus:border-[#005EA3]'"
              ></textarea>
              <p v-if="validationErrors.skills" class="text-[10px] text-rose-600 flex items-center gap-1 font-semibold">
                <span class="material-symbols-outlined text-[13px]">error</span>
                Vui lòng không để trống mục kiến thức/kỹ năng tích lũy.
              </p>
            </div>

            <!-- Optional Difficulties & Solutions Grid -->
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div id="difficulties-wrapper" class="space-y-1">
                <label class="block font-bold text-slate-800 text-[11px] uppercase tracking-wider flex items-center gap-1">
                  <span class="material-symbols-outlined text-slate-500 text-[16px]">report_problem</span>
                  Khó khăn gặp phải (Tùy chọn)
                </label>
                <textarea
                  id="difficulties-textarea"
                  v-model="formDifficulties"
                  rows="2"
                  placeholder="Lỗi kỹ thuật, vướng mắc nghiệp vụ, thiếu tài liệu hướng dẫn..."
                  class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none text-xs transition-all focus:ring-2 focus:ring-blue-100 focus:border-[#005EA3] placeholder:text-slate-400 leading-relaxed"
                ></textarea>
              </div>

              <div id="solutions-wrapper" class="space-y-1">
                <label class="block font-bold text-slate-800 text-[11px] uppercase tracking-wider flex items-center gap-1">
                  <span class="material-symbols-outlined text-slate-500 text-[16px]">psychology</span>
                  Cách xử lý / Hướng giải quyết (Tùy chọn)
                </label>
                <textarea
                  id="solutions-textarea"
                  v-model="formSolutions"
                  rows="2"
                  placeholder="Tra cứu tài liệu, tự debug, thảo luận nhóm, nhờ Mentor hoặc Giảng viên hướng dẫn..."
                  class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none text-xs transition-all focus:ring-2 focus:ring-blue-100 focus:border-[#005EA3] placeholder:text-slate-400 leading-relaxed"
                ></textarea>
              </div>
            </div>

            <!-- Next Week Plan -->
            <div id="next-plan-wrapper" class="space-y-1">
              <div class="flex items-center justify-between">
                <label class="block font-bold text-slate-800 text-[11px] uppercase tracking-wider flex items-center gap-1">
                  <span class="material-symbols-outlined text-[#005EA3] text-[16px]">next_plan</span>
                  Kế hoạch làm việc tiếp theo (*)
                </label>
                <span class="text-[10px] font-mono text-slate-400">
                  {{ formNextPlan.length }} ký tự
                </span>
              </div>
              <textarea
                id="next-plan-textarea"
                v-model="formNextPlan"
                rows="2"
                placeholder="Dự kiến các mục tiêu, nhiệm vụ và kết quả cần đạt được trong tuần kế tiếp..."
                class="w-full px-3.5 py-2.5 rounded-xl border outline-none text-xs transition-all placeholder:text-slate-400 leading-relaxed"
                :class="validationErrors.nextPlan ? 'border-rose-400 focus:ring-2 focus:ring-rose-100' : 'border-slate-300 focus:ring-2 focus:ring-blue-100 focus:border-[#005EA3]'"
              ></textarea>
              <p v-if="validationErrors.nextPlan" class="text-[10px] text-rose-600 flex items-center gap-1 font-semibold">
                <span class="material-symbols-outlined text-[13px]">error</span>
                Vui lòng điền kế hoạch làm việc dự kiến cho tuần tới.
              </p>
            </div>

          </div>

          <!-- Section 3: Document Upload Area -->
          <div id="attachments-wrapper" class="space-y-2">
            <label class="block font-bold text-slate-800 text-[11px] uppercase tracking-wider flex items-center gap-1">
              <span class="material-symbols-outlined text-slate-500 text-[16px]">attach_file</span>
              Minh chứng thực tập (Hình ảnh, Tài liệu đính kèm)
            </label>
            
            <input
              ref="fileInputRef"
              type="file"
              multiple
              accept="image/*,.pdf,.doc,.docx"
              class="hidden"
              @change="onFilesSelected"
            />

            <!-- Drag & Drop Zone -->
            <div
              id="drag-drop-zone"
              @click="triggerFileInput"
              @dragover.prevent
              @drop.prevent="onFileDrop"
              class="border-2 border-dashed border-slate-300 hover:border-blue-500 p-5 rounded-xl text-center bg-slate-50/50 hover:bg-blue-50/20 transition cursor-pointer group flex flex-col items-center justify-center gap-1.5"
            >
              <span class="material-symbols-outlined text-slate-400 group-hover:text-blue-600 text-[28px] transition-colors">cloud_upload</span>
              <div>
                <p class="font-bold text-slate-700 text-xs">Kéo thả hình ảnh / file minh chứng hoặc bấm để chọn</p>
                <p class="text-[10px] text-slate-400 mt-0.5">Chấp nhận định dạng ảnh (PNG, JPG, JPEG), PDF, DOCX (Tối đa 50MB)</p>
              </div>
            </div>

            <!-- Uploaded Images Preview Grid -->
            <div v-if="uploadedImages.length > 0" class="space-y-1.5 pt-1">
              <div class="flex items-center justify-between">
                <span class="font-bold text-slate-800 text-[11px] flex items-center gap-1">
                  <span class="material-symbols-outlined text-[15px] text-[#005EA3]">image</span>
                  Hình ảnh đã tải lên ({{ uploadedImages.length }})
                </span>
                <button type="button" @click="uploadedImages = []" class="text-rose-600 hover:underline text-[10px] font-semibold">
                  Xóa tất cả
                </button>
              </div>

              <div class="grid grid-cols-2 sm:grid-cols-4 gap-3 p-3 bg-slate-50 rounded-xl border border-slate-200/80">
                <div
                  v-for="(img, idx) in uploadedImages"
                  :key="'up-img-' + idx"
                  class="relative group aspect-square rounded-lg overflow-hidden border border-slate-200 shadow-2xs bg-white flex items-center justify-center p-0.5"
                >
                  <img :src="img.url" :alt="img.name" class="w-full h-full object-cover rounded-md block" />
                  
                  <button
                    type="button"
                    @click.stop="removeImage(idx)"
                    class="absolute top-1 right-1 bg-slate-900/80 hover:bg-rose-600 text-white w-5 h-5 rounded-full flex items-center justify-center text-[10px] shadow-sm transition-all active:scale-90 z-10"
                    title="Xóa"
                  >
                    ✕
                  </button>
                  <span class="absolute bottom-0 left-0 right-0 bg-slate-950/75 text-white text-[8px] px-1 py-0.5 rounded-b truncate text-center font-mono">
                    {{ img.name }}
                  </span>
                </div>
              </div>
            </div>

            <!-- Uploaded Documents List -->
            <div v-if="uploadedDocs.length > 0" class="space-y-1.5 pt-1">
              <span class="font-bold text-slate-800 text-[11px] block">File tài liệu đính kèm ({{ uploadedDocs.length }})</span>
              <div class="grid grid-cols-1 sm:grid-cols-2 gap-2">
                <div
                  v-for="(doc, dIdx) in uploadedDocs"
                  :key="'up-doc-' + dIdx"
                  class="flex items-center justify-between p-2 bg-slate-50 border border-slate-200/80 rounded-xl text-xs"
                >
                  <div class="flex items-center gap-2 truncate pr-1">
                    <span class="material-symbols-outlined text-[#005EA3] text-[18px]">description</span>
                    <span class="font-bold text-slate-700 truncate text-[11px]">{{ doc.name }}</span>
                    <span class="text-[9px] text-slate-400">({{ doc.size }})</span>
                  </div>
                  <button type="button" @click="removeDoc(dIdx)" class="text-rose-500 hover:text-rose-700 font-bold px-1 text-[11px] transition-colors">
                    ✕
                  </button>
                </div>
              </div>
            </div>

          </div>

        </div>

        <!-- Modal Footer -->
        <div id="new-journal-modal-footer" class="px-6 py-4 border-t border-slate-100 flex items-center justify-between bg-slate-50/50 shrink-0">
          <button 
            id="btn-cancel-modal"
            type="button" 
            @click="showNewModal = false" 
            class="px-4 py-2 border border-slate-300 rounded-xl font-bold text-slate-700 hover:bg-slate-100 text-xs transition"
          >
            Hủy bỏ
          </button>
          
          <div class="flex items-center gap-2.5">
            <button 
              id="btn-save-draft"
              type="button" 
              @click="saveDraft" 
              class="px-4 py-2 border border-[#005EA3]/30 text-[#005EA3] rounded-xl font-bold hover:bg-[#005EA3]/5 text-xs transition"
            >
              Lưu bản nháp
            </button>
            <button 
              id="btn-submit-journal"
              type="button" 
              @click="submitJournal" 
              class="px-5 py-2 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-xs text-xs transition-all active:scale-98 flex items-center gap-1.5"
            >
              <span class="material-symbols-outlined text-[16px]">send</span>
              Nộp nhật ký
            </button>
          </div>
        </div>

      </div>
    </div>

    <!-- IMAGE LIGHTBOX PREVIEW MODAL -->
    <div
      v-if="previewImageUrl"
      class="fixed inset-0 z-[150] flex items-center justify-center p-4 bg-slate-950/80 backdrop-blur-md"
      @click.self="previewImageUrl = null"
    >
      <div class="relative max-w-4xl max-h-[90vh] bg-slate-900 p-2 rounded-2xl shadow-2xl border border-slate-700 flex flex-col items-center">
        <button
          @click="previewImageUrl = null"
          class="absolute -top-3 -right-3 bg-white text-slate-900 w-8 h-8 rounded-full font-black text-sm shadow-xl flex items-center justify-center hover:bg-slate-200 transition"
        >
          ✕
        </button>
        <img :src="previewImageUrl" class="max-w-full max-h-[80vh] object-contain rounded-xl" />
      </div>
    </div>

    <!-- Toast Notification -->
    <div
      v-if="toastMsg"
      class="fixed bottom-6 right-6 z-[130] bg-slate-900 text-white px-4 py-3 rounded-xl shadow-xl text-xs font-bold flex items-center gap-2"
    >
      <span class="material-symbols-outlined text-emerald-400 text-[18px]">check_circle</span>
      {{ toastMsg }}
    </div>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted, onUnmounted, watch } from 'vue';
  import { useAuthStore } from '@/stores/auth';
  import { sendRealtimeNotification } from '@/services/notificationService';
  import { listenStudentDiaries, addDiaryEntryToDb } from '@/services/diaryService';

  const authStore = useAuthStore();
  const selectedWeek = ref(5);
  const currentSystemWeek = 5;
  const isPastDeadline = computed(() => selectedWeek.value < currentSystemWeek);
  const editingJournalId = ref(null);
  const showDrawer = ref(false);
  const selectedDrawerItem = ref(null);
  const showNewModal = ref(false);
  const toastMsg = ref('');
  const fileInputRef = ref(null);
  const previewImageUrl = ref(null);

  // Form states
  const formCompletion = ref(8);
  const formFeeling = ref(4);
  const formTaskDesc = ref('');
  const formSkills = ref('');
  const formDifficulties = ref('');
  const formSolutions = ref('');
  const formNextPlan = ref('');
  const uploadedImages = ref([]);
  const uploadedDocs = ref([]);

  const validationErrors = ref({
    taskDesc: false,
    skills: false,
    nextPlan: false
  });
  const showValidationAlert = ref(false);

  const feelings = [
    { val: 1, emoji: '😞', label: 'Rất tệ' },
    { val: 2, emoji: '😟', label: 'Áp lực' },
    { val: 3, emoji: '😐', label: 'Bình thường' },
    { val: 4, emoji: '🙂', label: 'Tốt' },
    { val: 5, emoji: '🤩', label: 'Tuyệt vời' }
  ];

  const completionLabel = computed(() => {
    const val = formCompletion.value;
    if (val <= 3) return '🔴 Mức độ hoàn thành thấp (Chậm tiến độ, nhiều vướng mắc)';
    if (val <= 6) return '🟡 Mức độ trung bình (Hoàn thành một phần, cần nỗ lực thêm)';
    if (val <= 8) return '🟢 Mức độ tốt (Hoàn thành đúng hạn, kết quả đạt yêu cầu)';
    return '🔵 Mức độ xuất sắc (Hoàn thành vượt tiến độ hoặc chất lượng rất cao)';
  });

  const weekRanges = {
    5: '15/07 - 21/07/2026',
    4: '08/07 - 14/07/2026',
    3: '01/07 - 07/07/2026'
  };

  const currentWeekRange = computed(() => weekRanges[selectedWeek.value] || '22/07 - 28/07/2026');

  const currentStudentId = computed(() => {
    const user = authStore.user || JSON.parse(localStorage.getItem('user') || '{}');
    return String(user?.maDinhDanh || user?.username || 'default');
  });

  const journalsData = ref({});
  let unsubscribeFirestore = null;

  function loadUserJournals() {
    const sid = currentStudentId.value;
    const saved = localStorage.getItem(`ims_diaries_${sid}`);
    if (saved) {
      try {
        journalsData.value = JSON.parse(saved);
      } catch (e) {
        console.error(e);
        journalsData.value = {};
      }
    } else {
      journalsData.value = {};
    }

    // Attach Firestore real-time listener scoped strictly to current user ID
    if (unsubscribeFirestore) unsubscribeFirestore();

    // Prevent unauthenticated calls during login/transition or if user is guest/not authenticated
    if (!authStore.isAuthenticated || !sid || sid === 'default' || sid === '---') {
      return;
    }

    unsubscribeFirestore = listenStudentDiaries(sid, (firestoreEntries) => {
      const grouped = {};
      firestoreEntries.forEach(item => {
        const w = item.week || 5;
        if (!grouped[w]) grouped[w] = [];
        grouped[w].push(item);
      });
      journalsData.value = grouped;
      persistUserJournals();
    });
  }

  function persistUserJournals() {
    const sid = currentStudentId.value;
    localStorage.setItem(`ims_diaries_${sid}`, JSON.stringify(journalsData.value));
  }

  onMounted(() => {
    loadUserJournals();
  });

  onUnmounted(() => {
    if (unsubscribeFirestore) unsubscribeFirestore();
  });

  watch(currentStudentId, () => {
    loadUserJournals();
  });

  const currentJournals = computed(() => journalsData.value[selectedWeek.value] || []);
  const submittedCount = computed(() => currentJournals.value.filter(j => j.status === 'submitted').length);

  function switchWeek(week) {
    if (week >= 1 && week <= 12) {
      selectedWeek.value = week;
    }
  }

  function openDetailDrawer(item) {
    selectedDrawerItem.value = item;
    showDrawer.value = true;
  }

  function openImagePreview(img) {
    previewImageUrl.value = typeof img === 'string' ? img : img.url;
  }

  function triggerFileInput() {
    fileInputRef.value?.click();
  }

  function onFilesSelected(e) {
    const files = Array.from(e.target.files || []);
    processFiles(files);
  }

  function onFileDrop(e) {
    const files = Array.from(e.dataTransfer.files || []);
    processFiles(files);
  }

  function processFiles(files) {
    files.forEach(file => {
      if (file.type.startsWith('image/')) {
        const url = URL.createObjectURL(file);
        uploadedImages.value.push({
          name: file.name,
          size: (file.size / (1024 * 1024)).toFixed(1) + ' MB',
          url
        });
      } else {
        uploadedDocs.value.push({
          name: file.name,
          size: (file.size / (1024 * 1024)).toFixed(1) + ' MB'
        });
      }
    });
  }

  function removeImage(idx) {
    uploadedImages.value.splice(idx, 1);
  }

  function removeDoc(idx) {
    uploadedDocs.value.splice(idx, 1);
  }

  function openNewJournalModal() {
    editingJournalId.value = null;
    formTaskDesc.value = '';
    formSkills.value = '';
    formDifficulties.value = '';
    formSolutions.value = '';
    formNextPlan.value = '';
    uploadedImages.value = [];
    uploadedDocs.value = [];
    validationErrors.value = {
      taskDesc: false,
      skills: false,
      nextPlan: false
    };
    showValidationAlert.value = false;
    showNewModal.value = true;
  }

  function editDraft(item) {
    editingJournalId.value = item.id || item.createdAt;
    formTaskDesc.value = item.content || '';
    formSkills.value = item.skills || '';
    formNextPlan.value = item.nextPlan || '';
    formDifficulties.value = item.difficulty || '';
    formSolutions.value = item.solution || '';
    formCompletion.value = item.completionRating || 8;
    formFeeling.value = item.feelingRating || 4;
    uploadedImages.value = (item.images || []).map(url => ({ url, name: 'Ảnh đính kèm' }));
    uploadedDocs.value = [];
    validationErrors.value = { taskDesc: false, skills: false, nextPlan: false };
    showValidationAlert.value = false;
    showNewModal.value = true;
  }

  function saveDraft() {
    const todayStr = new Date().toLocaleDateString('vi-VN');
    const sid = currentStudentId.value;
    const user = authStore.user || {};
    
    const draftEntry = {
      id: editingJournalId.value || Date.now(),
      userId: String(sid),
      studentName: user.hoTen || 'Sinh viên',
      week: Number(selectedWeek.value),
      date: `${todayStr} — Thứ ${new Date().getDay() || 7}`,
      status: 'draft',
      updatedAt: `${todayStr} - ${new Date().toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' })}`,
      content: formTaskDesc.value,
      completionRating: formCompletion.value,
      feelingRating: formFeeling.value,
      images: uploadedImages.value.map(i => i.url),
      feedback: null
    };

    if (!journalsData.value[selectedWeek.value]) {
      journalsData.value[selectedWeek.value] = [];
    }

    if (editingJournalId.value) {
      const idx = journalsData.value[selectedWeek.value].findIndex(j => (j.id || j.createdAt) === editingJournalId.value);
      if (idx > -1) {
        journalsData.value[selectedWeek.value][idx] = draftEntry;
      } else {
        journalsData.value[selectedWeek.value].unshift(draftEntry);
      }
    } else {
      journalsData.value[selectedWeek.value].unshift(draftEntry);
    }

    persistUserJournals();
    showNewModal.value = false;
    triggerToast('Đã lưu nháp nhật ký thành công!');
  }

  async function submitJournal() {
    // Validate required fields
    validationErrors.value.taskDesc = !formTaskDesc.value.trim() || formTaskDesc.value.trim().length < 10;
    validationErrors.value.skills = !formSkills.value.trim();
    validationErrors.value.nextPlan = !formNextPlan.value.trim();

    if (validationErrors.value.taskDesc || validationErrors.value.skills || validationErrors.value.nextPlan) {
      showValidationAlert.value = true;
      const bodyEl = document.getElementById('new-journal-modal-body');
      if (bodyEl) {
        bodyEl.scrollTo({ top: 0, behavior: 'smooth' });
      }
      return;
    }

    showValidationAlert.value = false;

    const todayStr = new Date().toLocaleDateString('vi-VN');
    const sid = currentStudentId.value;
    const user = authStore.user || {};
    const sName = user.hoTen || 'Sinh viên';
    const sMssv = user.maDinhDanh || sid;

    const newEntry = {
      userId: String(sid),
      studentName: sName,
      week: Number(selectedWeek.value),
      date: `${todayStr} — Thứ ${new Date().getDay() || 7}`,
      status: 'submitted',
      updatedAt: `${todayStr} - ${new Date().toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' })}`,
      content: formTaskDesc.value,
      completionRating: formCompletion.value,
      feelingRating: formFeeling.value,
      images: uploadedImages.value.map(i => i.url),
      feedback: null
    };

    try {
      await addDiaryEntryToDb(newEntry);
    } catch (e) {
      console.error('Failed to save to Firestore:', e);
    }

    if (!journalsData.value[selectedWeek.value]) {
      journalsData.value[selectedWeek.value] = [];
    }
    
    if (editingJournalId.value) {
      const idx = journalsData.value[selectedWeek.value].findIndex(j => (j.id || j.createdAt) === editingJournalId.value);
      if (idx > -1) {
        journalsData.value[selectedWeek.value][idx] = newEntry;
      } else {
        journalsData.value[selectedWeek.value].unshift(newEntry);
      }
    } else {
      journalsData.value[selectedWeek.value].unshift(newEntry);
    }
    persistUserJournals();

    // Dispatch real-time Firebase notification to teacher
    sendRealtimeNotification({
      role: 'GiangVien',
      userId: 'GV001',
      title: `Nhật ký thực tập Tuần ${selectedWeek.value} mới`,
      content: `Sinh viên ${sName} (MSSV: ${sMssv}) vừa nộp Nhật ký Tuần ${selectedWeek.value}: "${formTaskDesc.value.substring(0, 80)}..."`,
      icon: 'menu_book',
      bgClass: 'bg-blue-100 text-blue-700',
      link: '/teacher/classes/L01/diaries'
    });

    showNewModal.value = false;
    triggerToast('Đã nộp nhật ký thành công!');
  }

  function triggerToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }
</script>
