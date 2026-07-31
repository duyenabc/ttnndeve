<template>
  <div class="ims-scope w-full mx-auto space-y-6 font-sans pb-24 text-slate-800">
    <!-- Breadcrumb & Header -->
    <div>
      <nav class="flex items-center gap-2 text-[13px] text-slate-500 mb-3">
        <router-link to="/teacher/classes" class="hover:text-blue-700 transition-colors">Lớp của tôi</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-900 font-bold">Công việc</span>
      </nav>
      <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <h1 class="text-[32px] font-bold text-slate-900 tracking-tight">Công việc</h1>
      </div>
    </div>

    <!-- Sub Navigation Tabs -->
    <div class="flex items-center gap-8 border-b border-slate-200 mt-2">
      <router-link
        :to="`/teacher/classes/${classId}/diaries`"
        class="pb-3 text-[#005EA3] border-b-[3px] border-[#005EA3] font-bold text-[15px]"
      >
        Nhật ký
      </router-link>
      <router-link
        :to="`/teacher/classes/${classId}/tasks`"
        class="pb-3 text-slate-500 hover:text-[#005EA3] font-semibold text-[15px] transition-colors"
      >
        Sự kiện
      </router-link>
      <router-link
        :to="`/teacher/classes/${classId}/topics`"
        class="pb-3 text-slate-500 hover:text-[#005EA3] font-semibold text-[15px] transition-colors flex items-center gap-1.5"
      >
        Đề tài
      </router-link>
    </div>

    <!-- Filter Bar -->
    <div class="bg-[#f8f9fa] rounded-xl border border-slate-200 p-5 space-y-5">
      <!-- Time Filter -->
      <div class="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div class="flex items-center gap-4">
          <span class="text-[13px] font-bold text-slate-700 w-24">THỜI GIAN:</span>
          <div class="flex bg-[#e2e8f0] p-1 rounded-lg text-[13px] font-semibold text-slate-600">
            <button
              @click="timeTab = 'thisWeek'"
              class="px-5 py-1.5 rounded-md transition-all"
              :class="timeTab === 'thisWeek' ? 'bg-white text-[#005EA3] shadow-sm font-bold' : 'hover:text-slate-900'"
            >
              Tuần này
            </button>
            <button
              @click="timeTab = 'lastWeek'"
              class="px-5 py-1.5 rounded-md transition-all"
              :class="timeTab === 'lastWeek' ? 'bg-white text-[#005EA3] shadow-sm font-bold' : 'hover:text-slate-900'"
            >
              Tuần trước
            </button>
            <button
              @click="timeTab = 'term'"
              class="px-5 py-1.5 rounded-md transition-all"
              :class="timeTab === 'term' ? 'bg-white text-[#005EA3] shadow-sm font-bold' : 'hover:text-slate-900'"
            >
              Toàn kỳ
            </button>
          </div>
        </div>
        <div class="flex items-center gap-2">
          <span class="text-[13px] text-slate-500">Tùy chỉnh:</span>
          <div class="relative">
            <select
              v-model="selectedWeek"
              class="appearance-none pl-3 pr-8 py-1.5 bg-white border border-slate-300 rounded-md text-[13px] text-slate-700 outline-none focus:ring-1 focus:ring-[#005EA3]"
            >
              <option value="13">Tuần 13 (22/05 - 29/05)</option>
              <option value="12">Tuần 12 (15/05 - 21/05)</option>
              <option value="11">Tuần 11 (08/05 - 14/05)</option>
            </select>
            <span class="material-symbols-outlined absolute right-2 top-1/2 -translate-y-1/2 text-slate-400 text-[18px] pointer-events-none">expand_more</span>
          </div>
        </div>
      </div>

      <!-- Status Filter -->
      <div class="flex items-center gap-4">
        <span class="text-[13px] font-bold text-slate-700 w-24">TRẠNG THÁI:</span>
        <div class="flex flex-wrap items-center gap-3">
          <button
            v-for="st in statusOptions"
            :key="st.value"
            @click="statusFilter = st.value"
            class="px-4 py-1.5 rounded-full text-[13px] transition-all border font-medium"
            :class="statusFilter === st.value
              ? 'bg-[#005EA3] text-white border-[#005EA3] font-bold'
              : 'bg-white text-slate-600 border-slate-300 hover:bg-slate-50'"
          >
            {{ st.label }}
          </button>
        </div>
      </div>
    </div>

    <!-- STUDENT DIARY LIST (Tuần này / Tuần trước / Tùy chỉnh) -->
    <div v-if="timeTab !== 'term'" class="space-y-4">
      <div
        v-for="student in filteredStudentDiaries"
        :key="student.id"
        class="bg-white rounded-lg border border-slate-300 p-5 shadow-sm space-y-4 transition hover:shadow-md relative"
      >
        <div class="flex items-start justify-between">
          <div class="flex items-start gap-4">
            <!-- Avatar -->
            <div
              v-if="!student.avatar"
              class="w-12 h-12 rounded-lg bg-[#e2e8f0] text-slate-700 font-bold flex items-center justify-center text-[15px]"
            >
              {{ student.initials }}
            </div>
            <img v-else :src="student.avatar" class="w-12 h-12 rounded-lg object-cover" />

            <!-- Info -->
            <div class="pt-0.5">
              <div class="flex items-center gap-2">
                <h3 class="font-medium text-slate-900 text-[16px]">{{ student.name }}</h3>
                <span v-if="student.isNew" class="bg-[#e6f0fa] text-[#005EA3] text-[10px] px-1.5 py-0.5 rounded-sm font-bold uppercase tracking-wider">MỚI</span>
              </div>
              <p class="text-[13px] text-slate-500 mt-1">MSSV: {{ student.mssv }} • {{ student.entries.length }}/{{ student.expectedCount || 1 }} nhật ký</p>
              
              <!-- Logs Pills -->
              <div v-if="student.entries.length > 0" class="flex flex-wrap items-center gap-2 mt-3">
                <button
                  v-for="(entry, index) in student.entries"
                  :key="entry.id"
                  @click="student.activeEntryIndex = index"
                  class="px-3 py-1 rounded-full text-[12px] font-medium border transition-all"
                  :class="student.activeEntryIndex === index
                    ? 'bg-white text-[#005EA3] border-[#005EA3]'
                    : 'bg-white text-slate-500 border-slate-300 hover:bg-slate-50'"
                >
                  Nhật ký #{{ index + 1 }} ({{ entry.dateLabel }})
                </button>
              </div>
            </div>
          </div>

          <!-- Status & Chevron -->
          <div class="flex flex-col items-end gap-3 pt-1">
            <span
              class="px-3 py-1 rounded-full text-[12px] font-medium"
              :class="studentStatusBadgeClass(student.status)"
            >
              {{ student.status }}
            </span>
            <button class="text-slate-600 hover:text-slate-900 mt-6">
              <span class="material-symbols-outlined text-[20px]">expand_more</span>
            </button>
          </div>
        </div>

        <!-- AI Summary Card -->
        <div class="bg-[#f0f4f8] border-l-[3px] border-[#005EA3] p-3 rounded-r-md flex items-start gap-2 text-[13px] text-slate-700 ml-[64px]">
          <span class="material-symbols-outlined text-[#005EA3] text-[18px] shrink-0 mt-0.5">auto_awesome</span>
          <div class="leading-relaxed">
            <span class="italic font-bold text-[#005EA3]">Tóm tắt AI {{ currentActiveEntry(student) ? `(Nhật ký #${student.activeEntryIndex + 1})` : '' }}:</span>
            <span class="italic font-medium ml-1 text-slate-800">{{ currentActiveEntry(student)?.aiSummary || student.aiSummary }}</span>
          </div>
        </div>
      </div>
    </div>

    <!-- TERM SUMMARY HEATMAP VIEW (Toàn kỳ) -->
    <div v-else class="bg-white rounded-lg border border-slate-300 overflow-hidden shadow-sm">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse text-[13px]">
          <thead>
            <tr class="bg-[#f8f9fa] border-b border-slate-300 font-bold text-[12px] text-slate-700 uppercase tracking-wide">
              <th class="py-4 px-5">HỌ TÊN</th>
              <th class="py-4 px-5">MÃ SỐ SINH VIÊN</th>
              <th class="py-4 px-5 text-center">TỶ LỆ</th>
              <th class="py-4 px-5 text-center">THIẾU</th>
              <th class="py-4 px-5 text-center">KHÔNG NỘP</th>
              <th class="py-4 px-5">HEATMAP (TUẦN 1-15)</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-200">
            <!-- Mock rows perfectly matching the Figma screenshot -->
            <tr v-for="i in 8" :key="i" class="hover:bg-slate-50 transition">
              <td class="py-4 px-5 font-medium text-[#005EA3]">Võ Đức Trung Quân</td>
              <td class="py-4 px-5 text-slate-600">20246012</td>
              <td class="py-4 px-5 text-center text-slate-700">12/15</td>
              <td class="py-4 px-5 text-center text-red-500">1</td>
              <td class="py-4 px-5 text-center text-red-500">1</td>
              <td class="py-4 px-5">
                <div class="flex items-center gap-1">
                  <div class="w-[18px] h-[18px] bg-[#0e7c25]"></div>
                  <div class="w-[18px] h-[18px] bg-[#0e7c25]"></div>
                  <div class="w-[18px] h-[18px] bg-[#ea580c]"></div>
                  <div class="w-[18px] h-[18px] bg-[#dc2626]"></div>
                  <div class="w-[18px] h-[18px] bg-[#0e7c25]"></div>
                  <div class="w-[18px] h-[18px] bg-[#0e7c25]"></div>
                  <div class="w-[18px] h-[18px] bg-[#0e7c25]"></div>
                  <div class="w-[18px] h-[18px] bg-[#ea580c]"></div>
                  <div class="w-[18px] h-[18px] bg-[#cbd5e1]"></div>
                  <div class="w-[18px] h-[18px] bg-[#cbd5e1]"></div>
                  <div class="w-[18px] h-[18px] bg-[#cbd5e1]"></div>
                  <div class="w-[18px] h-[18px] bg-[#cbd5e1]"></div>
                  <div class="w-[18px] h-[18px] bg-[#cbd5e1]"></div>
                  <div class="w-[18px] h-[18px] bg-[#cbd5e1]"></div>
                  <div class="w-[18px] h-[18px] bg-[#cbd5e1]"></div>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const classId = computed(() => route.params.id || '1');

const timeTab = ref('thisWeek');
const selectedWeek = ref('13');
const statusFilter = ref('ALL');

const statusOptions = [
  { value: 'ALL', label: 'Tất cả' },
  { value: 'Nộp đủ', label: 'Nộp đủ' },
  { value: 'Nộp thiếu', label: 'Nộp thiếu' },
  { value: 'Không nộp', label: 'Không nộp' }
];

const studentDiaries = ref([
  {
    id: 1,
    name: 'Lê Khả Anh Huy',
    initials: 'AH',
    avatar: null,
    mssv: '20246015',
    status: 'Không nộp',
    isNew: false,
    expectedCount: 1,
    entries: [],
    aiSummary: 'Sinh viên chưa nộp nhật ký cho tuần này. Kế hoạch tuần trước là nghiên cứu WebSockets.',
    activeEntryIndex: 0
  },
  {
    id: 2,
    name: 'Võ Đức Trung Quân',
    initials: 'VQ',
    avatar: 'https://i.pravatar.cc/150?u=a042581f4e29026704d',
    mssv: '20246012',
    status: 'Nộp đủ',
    isNew: true,
    expectedCount: 2,
    entries: [
      { id: 101, dateLabel: 'Thứ 3 - 14/05', aiSummary: 'Quân đã tích hợp thành công API và xử lý xong các lỗi hiển thị trên thiết bị di động. Đang tập trung tối ưu hóa hiệu suất CSS.' },
      { id: 102, dateLabel: 'Thứ 5 - 16/05', aiSummary: 'Hoàn thiện giao diện responsive và bắt đầu viết unit test cho các component chính.' }
    ],
    aiSummary: '',
    activeEntryIndex: 0
  },
  {
    id: 3,
    name: 'Đồng Văn Triệu Doãn',
    initials: 'DĐ',
    avatar: 'https://i.pravatar.cc/150?u=a042581f4e29026704e',
    mssv: '20246012',
    status: 'Nộp thiếu',
    isNew: false,
    expectedCount: 2,
    entries: [
      { id: 103, dateLabel: 'Thứ 3 - 14/05', aiSummary: 'Doãn đã tích hợp thành công API và xử lý xong các lỗi hiển thị trên thiết bị di động. Đang tập trung tối ưu hóa hiệu suất CSS.' }
    ],
    aiSummary: '',
    activeEntryIndex: 0
  }
]);

const filteredStudentDiaries = computed(() => {
  return studentDiaries.value.filter(st => {
    const matchStatus = statusFilter.value === 'ALL' || st.status === statusFilter.value;
    return matchStatus;
  });
});

function currentActiveEntry(student) {
  if (!student.entries || !student.entries.length) return null;
  return student.entries[student.activeEntryIndex] || student.entries[0];
}

function studentStatusBadgeClass(status) {
  switch (status) {
    case 'Nộp đủ':
      return 'bg-[#c6f6d5] text-[#276749]';
    case 'Nộp thiếu':
      return 'bg-[#feebc8] text-[#c05621]';
    case 'Không nộp':
      return 'bg-[#fed7d7] text-[#9b2c2c]';
    default:
      return 'bg-slate-100 text-slate-600';
  }
}
</script>
