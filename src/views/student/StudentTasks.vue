<template>
  <div class="ims-scope max-w-6xl mx-auto font-sans space-y-6 pb-20">
    <!-- Breadcrumb & Header -->
    <div class="space-y-3">
      <nav class="flex items-center text-xs text-slate-500 gap-1.5">
        <router-link to="/student/classes" class="hover:text-[#005EA3] transition font-medium">Lớp của tôi</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="hover:text-[#005EA3] transition font-medium">Lớp Thực tập K64</span>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-900 font-bold">Nhiệm vụ & Bài nộp</span>
      </nav>

      <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs">
        <div>
          <h1 class="text-2xl font-extrabold text-slate-900 tracking-tight flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3] text-[28px]">assignment</span>
            Danh sách Nhiệm vụ & Bài nộp
          </h1>
          <p class="text-xs text-slate-500 mt-1">Theo dõi các bài tập, báo cáo và deadline do Giảng viên hướng dẫn giao.</p>
        </div>

        <div class="flex items-center gap-2">
          <span class="px-3 py-1 bg-rose-50 text-rose-700 text-xs font-bold rounded-xl border border-rose-200 flex items-center gap-1">
            <span class="w-2 h-2 rounded-full bg-rose-600 animate-ping"></span>
            3 Nhiệm vụ cần làm
          </span>
        </div>
      </div>
    </div>

    <!-- Section 1: Cần thực hiện (Pending Tasks) -->
    <div class="bg-white rounded-2xl border border-slate-200 shadow-2xs overflow-hidden space-y-4 p-6">
      <div class="flex items-center justify-between border-b border-slate-100 pb-4">
        <h3 class="font-bold text-sm text-slate-900 flex items-center gap-2">
          <span class="w-2.5 h-6 rounded-full bg-rose-600"></span>
          Cần thực hiện
          <span class="px-2 py-0.5 bg-rose-100 text-rose-800 text-xs font-bold rounded-full">3</span>
        </h3>
      </div>

      <div class="divide-y divide-slate-100 text-xs">
        <!-- Task Row 1: Overdue -->
        <div class="py-4 flex flex-col md:flex-row md:items-center justify-between gap-4 hover:bg-rose-50/40 p-3 rounded-xl transition">
          <div class="flex items-start gap-3 md:w-1/2">
            <span class="px-2.5 py-1 bg-rose-100 text-rose-800 font-black rounded-lg text-[10px] uppercase shrink-0 mt-0.5">
              QUÁ HẠN
            </span>
            <div>
              <router-link to="/student/tasks/1" class="font-bold text-slate-900 text-sm hover:text-[#005EA3] transition">
                Báo cáo thực tập tuần 4
              </router-link>
              <p class="text-slate-500 mt-1 line-clamp-1">
                Tổng hợp các công việc đã thực hiện trong tuần, các khó khăn gặp phải và giải pháp đề xuất.
              </p>
            </div>
          </div>

          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 md:w-1/2">
            <div class="text-slate-500">
              <p class="text-[11px]">Hạn chót:</p>
              <p class="font-bold text-rose-600 font-mono">15/08/2026, 23:59 PM</p>
            </div>

            <button
              @click="openSubmitModal('Báo cáo thực tập tuần 4')"
              class="px-5 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-xs transition active:scale-95 shrink-0"
            >
              Nộp bài ngay
            </button>
          </div>
        </div>

        <!-- Task Row 2: Due Today -->
        <div class="py-4 flex flex-col md:flex-row md:items-center justify-between gap-4 hover:bg-amber-50/40 p-3 rounded-xl transition">
          <div class="flex items-start gap-3 md:w-1/2">
            <span class="px-2.5 py-1 bg-amber-100 text-amber-900 font-black rounded-lg text-[10px] uppercase shrink-0 mt-0.5">
              HÔM NAY
            </span>
            <div>
              <h4 class="font-bold text-slate-900 text-sm">Nhật ký kỹ thuật - Tháng 10</h4>
              <p class="text-slate-500 mt-1 line-clamp-1">
                Ghi chép chi tiết các kỹ năng công nghệ mới học được và áp dụng vào dự án thực tế tại doanh nghiệp.
              </p>
            </div>
          </div>

          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 md:w-1/2">
            <div class="text-slate-500">
              <p class="text-[11px]">Hạn chót:</p>
              <p class="font-bold text-amber-700 font-mono">Hôm nay, 20:15 PM</p>
            </div>

            <button
              @click="openSubmitModal('Nhật ký kỹ thuật - Tháng 10')"
              class="px-5 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-xs transition active:scale-95 shrink-0"
            >
              Nộp bài ngay
            </button>
          </div>
        </div>

        <!-- Task Row 3: Upcoming -->
        <div class="py-4 flex flex-col md:flex-row md:items-center justify-between gap-4 hover:bg-slate-50 p-3 rounded-xl transition">
          <div class="flex items-start gap-3 md:w-1/2">
            <span class="px-2.5 py-1 bg-emerald-100 text-emerald-800 font-black rounded-lg text-[10px] uppercase shrink-0 mt-0.5">
              CÒN 2 NGÀY
            </span>
            <div>
              <h4 class="font-bold text-slate-900 text-sm">Phiếu đánh giá giữa kỳ</h4>
              <p class="text-slate-500 mt-1 line-clamp-1">
                Xin xác nhận và đánh giá từ quản lý trực tiếp tại đơn vị thực tập về thái độ và năng lực chuyên môn.
              </p>
            </div>
          </div>

          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 md:w-1/2">
            <div class="text-slate-500">
              <p class="text-[11px]">Hạn chót:</p>
              <p class="font-bold text-slate-800 font-mono">25/08/2026, 18:45 PM</p>
            </div>

            <button
              @click="openSubmitModal('Phiếu đánh giá giữa kỳ')"
              class="px-5 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-xs transition active:scale-95 shrink-0"
            >
              Nộp bài ngay
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Section 2: Đã hoàn thành (Completed Tasks) -->
    <div class="bg-white rounded-2xl border border-slate-200 shadow-2xs overflow-hidden space-y-4 p-6">
      <div class="flex items-center justify-between border-b border-slate-100 pb-4">
        <h3 class="font-bold text-sm text-slate-900 flex items-center gap-2">
          <span class="w-2.5 h-6 rounded-full bg-emerald-500"></span>
          Đã hoàn thành
          <span class="px-2 py-0.5 bg-emerald-100 text-emerald-800 text-xs font-bold rounded-full">12</span>
        </h3>
      </div>

      <div class="divide-y divide-slate-100 text-xs">
        <!-- Completed Row 1 -->
        <div class="py-3.5 flex flex-col md:flex-row md:items-center justify-between gap-4 hover:bg-slate-50 p-2 rounded-xl transition">
          <div class="flex items-start gap-3 md:w-1/2">
            <span class="px-2.5 py-1 bg-emerald-100 text-emerald-800 font-black rounded-lg text-[10px] uppercase shrink-0 mt-0.5">
              ĐÃ NỘP
            </span>
            <div>
              <h4 class="font-bold text-slate-900 text-sm">Báo cáo thực tập tuần 3</h4>
              <p class="text-slate-500 mt-0.5 line-clamp-1">
                Báo cáo chi tiết về tiến độ công việc và các kết quả đạt được trong tuần thứ 3.
              </p>
            </div>
          </div>

          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 md:w-1/2">
            <div class="text-slate-500">
              <p class="text-[11px]">Đã nộp lúc:</p>
              <p class="font-semibold text-slate-800 font-mono">10/08/2026, 08:45 AM</p>
            </div>

            <button
              @click="viewSubmission('Báo cáo thực tập tuần 3')"
              class="px-4 py-2 border border-slate-300 text-slate-700 font-bold rounded-xl hover:bg-slate-100 transition shrink-0"
            >
              Xem bài nộp
            </button>
          </div>
        </div>

        <!-- Completed Row 2 -->
        <div class="py-3.5 flex flex-col md:flex-row md:items-center justify-between gap-4 hover:bg-slate-50 p-2 rounded-xl transition">
          <div class="flex items-start gap-3 md:w-1/2">
            <span class="px-2.5 py-1 bg-amber-100 text-amber-800 font-black rounded-lg text-[10px] uppercase shrink-0 mt-0.5">
              NỘP TRỄ
            </span>
            <div>
              <h4 class="font-bold text-slate-900 text-sm">Hồ sơ năng lực cá nhân</h4>
              <p class="text-slate-500 mt-0.5 line-clamp-1">
                Bản tự đánh giá năng lực và định hướng phát triển nghề nghiệp trong tương lai.
              </p>
            </div>
          </div>

          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 md:w-1/2">
            <div class="text-slate-500">
              <p class="text-[11px]">Đã nộp lúc:</p>
              <p class="font-semibold text-slate-800 font-mono">05/08/2026, 07:17 AM</p>
            </div>

            <button
              @click="viewSubmission('Hồ sơ năng lực cá nhân')"
              class="px-4 py-2 border border-slate-300 text-slate-700 font-bold rounded-xl hover:bg-slate-100 transition shrink-0"
            >
              Xem bài nộp
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Submission Modal -->
    <div
      v-if="showSubmitModal"
      class="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-slate-900/50 backdrop-blur-xs font-sans"
      @click.self="showSubmitModal = false"
    >
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-lg p-6 space-y-5">
        <div class="flex justify-between items-center border-b pb-3">
          <h3 class="font-bold text-slate-900 text-base flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3]">upload_file</span>
            Nộp bài: {{ activeTaskTitle }}
          </h3>
          <button @click="showSubmitModal = false" class="text-slate-400 hover:text-slate-600">✕</button>
        </div>

        <div class="space-y-4 text-xs">
          <div>
            <label class="block font-bold text-slate-700 mb-1">Ghi chú gửi Giảng viên</label>
            <textarea
              v-model="submissionNote"
              rows="3"
              placeholder="Nhập ghi chú hoặc mô tả bài nộp của bạn..."
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
            ></textarea>
          </div>

          <div>
            <label class="block font-bold text-slate-700 mb-1">Đính kèm tệp đính kèm (*)</label>
            <div class="border-2 border-dashed border-slate-300 p-6 rounded-2xl text-center hover:bg-blue-50/50 transition cursor-pointer">
              <span class="material-symbols-outlined text-slate-400 text-[36px]">cloud_upload</span>
              <p class="font-bold text-slate-700 mt-1">Kéo thả tệp hoặc bấm chọn tệp</p>
              <p class="text-[11px] text-slate-400 mt-0.5">Chấp nhận PDF, DOCX, ZIP (Tối đa 50MB)</p>
            </div>
          </div>
        </div>

        <div class="pt-4 border-t flex justify-end gap-3 text-xs">
          <button @click="showSubmitModal = false" class="px-4 py-2 border border-slate-300 rounded-xl font-bold text-slate-700 hover:bg-slate-100">
            Hủy
          </button>
          <button @click="submitTask" class="px-5 py-2 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-xs">
            Xác nhận nộp bài
          </button>
        </div>
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
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';

  const router = useRouter();
  const showSubmitModal = ref(false);
  const activeTaskTitle = ref('');
  const submissionNote = ref('');
  const toastMsg = ref('');

  function openSubmitModal(title) {
    activeTaskTitle.value = title;
    submissionNote.value = '';
    showSubmitModal.value = true;
  }

  function submitTask() {
    showSubmitModal.value = false;
    toastMsg.value = `✓ Nộp bài "${activeTaskTitle.value}" thành công!`;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }

  function viewSubmission(title) {
    router.push('/student/tasks/1');
  }
</script>
