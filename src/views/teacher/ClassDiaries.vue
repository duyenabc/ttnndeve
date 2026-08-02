<template>
  <div class="ims-scope w-full mx-auto space-y-6 font-sans pb-24 text-slate-800">
    <div>
      <nav class="flex items-center gap-2 text-[13px] text-slate-500 mb-3">
        <router-link to="/teacher/classes" class="hover:text-blue-700 transition-colors">Lớp của tôi</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-900 font-bold">Công việc</span>
      </nav>
      <h1 class="text-[32px] font-bold text-slate-900 tracking-tight">Công việc</h1>
    </div>

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
        class="pb-3 text-slate-500 hover:text-[#005EA3] font-semibold text-[15px] transition-colors"
      >
        Đề tài
      </router-link>
    </div>

    <div
      v-if="!config.isEnabled"
      class="bg-amber-50 border border-amber-200 rounded-xl px-5 py-4 text-[14px] text-amber-900"
    >
      Lớp học hiện chưa kích hoạt tính năng Nhật ký thực tập. Vào
      <router-link :to="`/teacher/classes/${classId}/settings`" class="font-bold underline">Cấu hình lớp</router-link>
      để bật.
    </div>

    <template v-else>
      <div class="bg-[#f1f3f5] rounded-xl border border-slate-200/80 p-5 space-y-4">
        <div class="flex flex-col lg:flex-row lg:items-center justify-between gap-4">
          <div class="flex flex-wrap items-center gap-3">
            <span class="text-[12px] font-bold text-slate-600 tracking-wide">THỜI GIAN:</span>
            <div class="flex bg-slate-200/80 p-1 rounded-lg text-[13px] font-semibold text-slate-600">
              <button
                type="button"
                @click="setTimeTab('thisWeek')"
                class="px-4 py-1.5 rounded-md transition-all"
                :class="timeTab === 'thisWeek' ? 'bg-white text-[#005EA3] shadow-sm font-bold' : 'hover:text-slate-900'"
              >
                Tuần này
              </button>
              <button
                type="button"
                @click="setTimeTab('lastWeek')"
                class="px-4 py-1.5 rounded-md transition-all"
                :class="timeTab === 'lastWeek' ? 'bg-white text-[#005EA3] shadow-sm font-bold' : 'hover:text-slate-900'"
              >
                Tuần trước
              </button>
              <button
                type="button"
                @click="setTimeTab('term')"
                class="px-4 py-1.5 rounded-md transition-all"
                :class="timeTab === 'term' ? 'bg-white text-[#005EA3] shadow-sm font-bold' : 'hover:text-slate-900'"
              >
                Toàn kỳ
              </button>
            </div>
          </div>
          <div class="flex items-center gap-2">
            <span class="text-[13px] text-slate-500">Tùy chỉnh:</span>
            <div class="relative min-w-[220px]">
              <select
                v-model.number="selectedWeek"
                class="w-full appearance-none pl-3 pr-8 py-1.5 bg-white border border-slate-300 rounded-md text-[13px] text-slate-700 outline-none focus:ring-1 focus:ring-[#005EA3]"
                @change="timeTab = 'custom'"
              >
                <option v-for="w in weekOptions" :key="w.value" :value="w.value">{{ w.label }}</option>
              </select>
              <span class="material-symbols-outlined absolute right-2 top-1/2 -translate-y-1/2 text-slate-400 text-[18px] pointer-events-none">expand_more</span>
            </div>
          </div>
        </div>

        <div class="flex flex-wrap items-center gap-3">
          <span class="text-[12px] font-bold text-slate-600 tracking-wide">TRẠNG THÁI:</span>
          <button
            v-for="st in statusOptions"
            :key="st.value"
            type="button"
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

      <div v-if="isLoading" class="text-center py-8">
        <span class="material-symbols-outlined animate-spin text-3xl text-[#005EA3]">refresh</span>
        <p class="text-sm text-slate-500 mt-2">Đang tải dữ liệu...</p>
      </div>

      <template v-else>
        <div v-if="timeTab !== 'term'" class="space-y-3">
          <div
            v-for="student in filteredStudentDiaries"
            :key="student.id"
            class="rounded-xl border transition"
            :class="student.hasUnread
              ? 'bg-[#e8f3fc] border-[#b7d4ef]'
              : 'bg-white border-slate-200 hover:border-slate-300'"
          >
            <button
              type="button"
              class="w-full text-left p-4 sm:p-5 flex items-start justify-between gap-3"
              @click="toggleStudent(student.id)"
            >
              <div class="flex items-start gap-3.5 min-w-0 flex-1">
                <div class="w-11 h-11 rounded-full bg-slate-200 text-slate-600 font-bold flex items-center justify-center text-[13px] shrink-0 overflow-hidden">
                  <img v-if="student.avatar" :src="student.avatar" :alt="student.name" class="w-full h-full object-cover" />
                  <span v-else>{{ student.initials }}</span>
                </div>
                <div class="min-w-0">
                  <div class="flex flex-wrap items-center gap-2">
                    <h3 class="font-semibold text-slate-900 text-[15px]">{{ student.name }}</h3>
                    <span
                      v-if="student.hasUnread"
                      class="bg-[#e6f0fa] text-[#005EA3] text-[10px] px-1.5 py-0.5 rounded font-bold uppercase tracking-wider"
                    >Chưa đọc</span>
                  </div>
                  <p class="text-[13px] text-slate-500 mt-0.5">
                    MSSV: {{ student.mssv }} · {{ student.submittedCount }}/{{ config.minPerWeek }} nhật ký
                  </p>
                </div>
              </div>
              <div class="flex items-center gap-2 shrink-0">
                <span
                  class="px-2.5 py-1 rounded-full text-[12px] font-semibold"
                  :class="studentStatusBadgeClass(student.status)"
                >
                  {{ student.status }}
                </span>
                <span class="material-symbols-outlined text-slate-400 text-[22px]">
                  {{ expandedIds.has(student.id) ? 'expand_less' : 'expand_more' }}
                </span>
              </div>
            </button>

            <div v-if="expandedIds.has(student.id)" class="px-4 sm:px-5 pb-5 space-y-3 border-t border-slate-200/70">
              <p v-if="student.entries.length === 0" class="pt-4 text-[13px] text-slate-500">
                Sinh viên chưa nộp nhật ký tuần này.
              </p>

              <template v-else>
                <div class="flex flex-wrap items-center gap-2 pt-4">
                  <button
                    v-for="entry in student.entriesNewestFirst"
                    :key="entry.id"
                    type="button"
                    class="px-2.5 py-1 rounded-full text-[12px] font-medium border transition"
                    :class="[
                      activeEntryId[student.id] === entry.id
                        ? 'bg-[#005EA3] text-white border-[#005EA3]'
                        : entry.isRead
                          ? 'text-slate-600 bg-white border-slate-200 hover:bg-slate-50'
                          : 'text-[#005EA3] bg-blue-50/80 border-blue-100 hover:bg-blue-100',
                    ]"
                    @click.stop="selectEntry(student, entry)"
                  >
                    Nhật ký #{{ entry.displayIndex }} ({{ entry.weekdayLabel }} - {{ entry.dateLabel }})
                  </button>
                </div>

                <div class="bg-[#eef5fb] border border-[#d6e6f5] rounded-lg px-3.5 py-2.5 flex items-start gap-2.5 text-[13px] text-slate-700">
                  <span class="material-symbols-outlined text-[#005EA3] text-[18px] shrink-0 mt-0.5">auto_awesome</span>
                  <p class="leading-relaxed flex-1">
                    <span class="font-semibold text-[#005EA3]">Tóm tắt AI:</span>
                    <span class="ml-1">{{ getAiSummary(student) }}</span>
                  </p>
                </div>

                <div v-if="currentEntry(student)" class="space-y-3">
                  <div
                    v-if="planBanner(student)"
                    class="bg-slate-100 border border-slate-200 rounded-lg px-3.5 py-2.5 text-[13px] text-slate-700"
                  >
                    <span class="font-semibold text-slate-800">Kế hoạch từ {{ planBanner(student).dateLabel }}:</span>
                    <span class="ml-1">{{ planBanner(student).text }}</span>
                  </div>

                  <div class="space-y-2">
                    <div v-for="field in fieldsForEntry(student)" :key="field.id">
                      <h4 class="font-bold text-[11px] text-slate-500 uppercase mb-1">{{ field.label }}</h4>
                      <p class="text-[13px] text-slate-800 whitespace-pre-wrap bg-white p-3 rounded border border-slate-100 min-h-[36px]">
                        {{ fieldValue(currentEntry(student).rawData, field.id) }}
                      </p>
                    </div>
                  </div>

                  <div class="flex flex-wrap items-center gap-2">
                    <button
                      type="button"
                      class="px-3 py-1.5 text-[13px] font-semibold text-[#005EA3] border border-[#005EA3]/40 rounded-md hover:bg-blue-50"
                      @click.stop="toggleFullView(student)"
                    >
                      {{ isFullView(student) ? 'Thu gọn' : 'Xem đầy đủ' }}
                    </button>
                    <button
                      type="button"
                      class="px-3 py-1.5 text-[13px] font-medium text-slate-600 border border-slate-200 rounded-md hover:bg-slate-50"
                      @click.stop="openDiaryDetail(currentEntry(student), student)"
                    >
                      Mở panel chi tiết
                    </button>
                  </div>

                  <div
                    v-if="entryFeedbacks(student).length"
                    class="rounded-lg border border-amber-200 bg-amber-50/80 px-3.5 py-3 space-y-2"
                  >
                    <h4 class="text-[12px] font-bold text-amber-900 uppercase tracking-wide">Nhận xét giảng viên</h4>
                    <div
                      v-for="(fb, i) in entryFeedbacks(student)"
                      :key="i"
                      class="bg-white/80 border border-amber-100 rounded-md p-3"
                    >
                      <div class="flex justify-between text-[11px] mb-1 gap-2">
                        <span class="font-bold text-slate-800">{{ fb.teacherName || 'GVHD' }}</span>
                        <span class="text-slate-500 shrink-0">{{ formatDate(fb.timestamp) }}</span>
                      </div>
                      <p class="text-[13px] text-slate-800 leading-relaxed whitespace-pre-wrap">{{ fb.content }}</p>
                    </div>
                  </div>

                  <div class="pt-2 border-t border-slate-200">
                    <label class="block text-[12px] font-bold text-slate-600 mb-2">Nhận xét / phản hồi trực tiếp</label>
                    <textarea
                      v-model="inlineFeedback[student.id]"
                      rows="2"
                      class="w-full border border-slate-300 rounded-[8px] px-3 py-2 text-[13px] outline-none focus:border-[#005EA3]"
                      placeholder="Nhập nhận xét cho nhật ký này..."
                      @click.stop
                    />
                    <div class="mt-2 flex justify-end">
                      <button
                        type="button"
                        class="px-4 py-2 bg-[#005EA3] text-white font-bold rounded-[8px] text-[13px] hover:bg-blue-800 disabled:opacity-50"
                        :disabled="!inlineFeedback[student.id]?.trim() || isSubmittingFeedback"
                        @click.stop="submitInlineFeedback(student)"
                      >
                        Gửi nhận xét
                      </button>
                    </div>
                  </div>
                </div>
              </template>
            </div>
          </div>

          <div
            v-if="filteredStudentDiaries.length === 0"
            class="text-center py-10 bg-white border border-slate-200 rounded-xl text-slate-500"
          >
            <p>Không có nhật ký nào được viết.</p>
          </div>
        </div>

        <div v-else class="bg-white rounded-xl border border-slate-200 overflow-hidden">
          <div class="overflow-x-auto">
            <table class="w-full text-left border-collapse text-[13px]">
              <thead>
                <tr class="bg-[#f8f9fa] border-b border-slate-200 font-bold text-[11px] text-slate-500 uppercase tracking-wide">
                  <th class="py-3.5 px-5">Họ tên</th>
                  <th class="py-3.5 px-5">Mã số sinh viên</th>
                  <th class="py-3.5 px-5 text-center">Tỷ lệ (tuần đủ)</th>
                  <th class="py-3.5 px-5 text-center">Thiếu</th>
                  <th class="py-3.5 px-5 text-center">Không nộp</th>
                  <th class="py-3.5 px-5">Heatmap (Tuần 1-{{ maxWeek }})</th>
                </tr>
              </thead>
              <tbody class="divide-y divide-slate-100">
                <tr v-for="student in studentDiariesAllTerm" :key="student.id" class="hover:bg-slate-50/80 transition">
                  <td class="py-3.5 px-5 font-medium text-[#005EA3]">{{ student.name }}</td>
                  <td class="py-3.5 px-5 text-slate-600">{{ student.mssv }}</td>
                  <td class="py-3.5 px-5 text-center text-slate-700">{{ student.weeksEnough }}/{{ currentInternshipWeek }}</td>
                  <td class="py-3.5 px-5 text-center font-semibold text-amber-600">{{ student.totalMissing }}</td>
                  <td class="py-3.5 px-5 text-center font-semibold text-red-500">{{ student.totalNone }}</td>
                  <td class="py-3.5 px-5">
                    <div class="flex items-center gap-1 flex-wrap">
                      <button
                        v-for="w in maxWeek"
                        :key="w"
                        type="button"
                        class="w-4 h-4 rounded-sm relative group"
                        :class="getHeatmapColor(student, w)"
                        :title="`Tuần ${w}: ${student.weeklyCounts[w] || 0}/${config.minPerWeek}`"
                        @click="jumpToWeek(w)"
                      />
                    </div>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </template>
    </template>

    <div v-if="isDrawerOpen" class="fixed inset-0 z-[100] bg-slate-900/50 backdrop-blur-sm flex justify-end" @click.self="closeDrawer">
      <div class="bg-white w-[600px] max-w-full h-full shadow-2xl flex flex-col animate-slide-in">
        <div class="p-4 border-b border-slate-200 flex items-center justify-between bg-slate-50">
          <h2 class="font-bold text-[16px] text-slate-800 flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3]">menu_book</span> Chi tiết Nhật ký
          </h2>
          <button type="button" class="text-slate-400 hover:text-slate-600" @click="closeDrawer">✕</button>
        </div>

        <div class="p-6 overflow-y-auto flex-1 space-y-6">
          <div class="bg-blue-50/50 p-4 rounded-lg border border-blue-100 flex items-center gap-4">
            <div class="w-10 h-10 rounded bg-[#e2e8f0] text-slate-700 font-bold flex items-center justify-center text-[14px]">
              {{ viewingStudent?.initials }}
            </div>
            <div>
              <p class="font-bold text-slate-900">{{ viewingStudent?.name }}</p>
              <p class="text-[12px] text-slate-500">MSSV: {{ viewingStudent?.mssv }} · Tuần {{ viewingDiary?.rawData?.week }}</p>
            </div>
          </div>

          <div
            v-if="viewingPlanBanner"
            class="bg-slate-100 border border-slate-200 rounded-lg px-3.5 py-2.5 text-[13px] text-slate-700"
          >
            <span class="font-semibold">Kế hoạch từ {{ viewingPlanBanner.dateLabel }}:</span>
            {{ viewingPlanBanner.text }}
          </div>

          <div class="space-y-4">
            <div v-for="field in activeFields" :key="field.id">
              <h4 class="font-bold text-[12px] text-slate-500 uppercase mb-1">{{ field.label }}</h4>
              <p class="text-[14px] text-slate-800 whitespace-pre-wrap bg-slate-50 p-3 rounded border border-slate-100 min-h-[40px]">
                {{ fieldValue(viewingDiary?.rawData, field.id) }}
              </p>
            </div>
          </div>

          <div class="mt-4 pt-4 border-t border-slate-200">
            <h3 class="font-bold text-[15px] text-slate-900 mb-4">Phản hồi của bạn</h3>
            <div v-if="viewingDiary?.rawData?.feedbacks?.length" class="space-y-3 mb-6">
              <div
                v-for="(fb, i) in viewingDiary.rawData.feedbacks"
                :key="i"
                class="bg-[#F9FAFB] border border-slate-200 rounded-[8px] p-3"
              >
                <div class="flex justify-between text-[11px] mb-1">
                  <span class="font-bold text-slate-700">{{ fb.teacherName }}</span>
                  <span class="text-slate-400">{{ formatDate(fb.timestamp) }}</span>
                </div>
                <p class="text-[13px] text-slate-800 leading-relaxed">{{ fb.content }}</p>
              </div>
            </div>
            <textarea
              v-model="feedbackContent"
              rows="3"
              class="w-full border border-slate-300 rounded-[8px] px-3 py-2 text-[13px] outline-none focus:border-[#005EA3]"
              placeholder="Nhập nhận xét..."
            />
            <div class="mt-3 flex justify-end">
              <button
                type="button"
                class="px-5 py-2 bg-[#005EA3] text-white font-bold rounded-[8px] text-[13px] hover:bg-blue-800 disabled:opacity-50"
                :disabled="!feedbackContent.trim() || isSubmittingFeedback"
                @click="submitFeedback"
              >
                Gửi phản hồi
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="showToast"
      class="fixed top-24 right-8 z-50 flex items-center p-4 text-gray-500 bg-white rounded-lg shadow-lg border border-gray-100"
      role="alert"
    >
      <div class="inline-flex items-center justify-center shrink-0 w-8 h-8 rounded-lg text-green-500 bg-green-100">
        <span class="material-symbols-outlined text-[20px]">check_circle</span>
      </div>
      <div class="ml-3 text-sm font-normal text-gray-800">{{ toastMessage }}</div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, reactive } from 'vue';
import { useRoute } from 'vue-router';
import api from '@/api/api';
import { useAuthStore } from '@/stores/auth';

const route = useRoute();
const authStore = useAuthStore();
const classId = computed(() => route.params.id || 'L001');

const timeTab = ref('thisWeek');
const selectedWeek = ref(1);
const statusFilter = ref('ALL');
const maxWeek = ref(15);
const internshipStart = ref(addDays(alignToMonday(new Date()), -7));

const statusOptions = [
  { value: 'ALL', label: 'Tất cả' },
  { value: 'Nộp đủ', label: 'Nộp đủ' },
  { value: 'Nộp thiếu', label: 'Nộp thiếu' },
  { value: 'Không nộp', label: 'Không nộp' },
  { value: 'Đang trong hạn', label: 'Đang trong hạn' },
];

const STATUS_SORT = { 'Không nộp': 0, 'Nộp thiếu': 1, 'Nộp đủ': 2, 'Đang trong hạn': 3 };

const allStudents = ref([]);
const allDiaries = ref([]);
const isLoading = ref(true);
const config = ref({
  isEnabled: true,
  minPerWeek: 2,
  deadlineDay: 0,
  deadlineTime: '23:59',
  fields: [],
});

const expandedIds = ref(new Set());
const fullViewIds = ref(new Set());
const activeEntryId = reactive({});
const inlineFeedback = reactive({});

const isDrawerOpen = ref(false);
const viewingDiary = ref(null);
const viewingStudent = ref(null);
const viewingPlanBanner = ref(null);
const feedbackContent = ref('');
const isSubmittingFeedback = ref(false);
const showToast = ref(false);
const toastMessage = ref('');

const activeFields = computed(() => (config.value.fields || []).filter((f) => f.isEnabled));
const previewFields = computed(() => activeFields.value.slice(0, 3));

const currentInternshipWeek = computed(() => weekNumberForDate(new Date()));

const weekOptions = computed(() => {
  const opts = [];
  for (let w = 1; w <= maxWeek.value; w++) {
    const start = addDays(internshipStart.value, (w - 1) * 7);
    const end = addDays(start, 6);
    opts.push({
      value: w,
      label: `Tuần ${w} (${fmtShort(start)} - ${fmtShort(end)})`,
    });
  }
  return opts;
});

onMounted(async () => {
  await loadData();
  selectedWeek.value = currentInternshipWeek.value;
});

async function loadData() {
  isLoading.value = true;
  try {
    const confRes = await api.get(`/giangvien/classes/${classId.value}/diary-config`);
    if (confRes.data) {
      config.value = {
        isEnabled: confRes.data.isEnabled !== false,
        minPerWeek: confRes.data.minPerWeek || 2,
        deadlineDay: confRes.data.deadlineDay ?? 0,
        deadlineTime: confRes.data.deadlineTime || '23:59',
        fields: confRes.data.fields?.length ? confRes.data.fields : [],
      };
      if (confRes.data.internshipStart) {
        internshipStart.value = alignToMonday(new Date(confRes.data.internshipStart));
      }
    }

    const stRes = await api.get(`/giangvien/classes/${classId.value}/students`);
    allStudents.value = Array.isArray(stRes.data) ? stRes.data : stRes.data?.items || [];

    const diariesRes = await api.get('/diaries', { params: { classId: classId.value } });
    allDiaries.value = diariesRes.data || [];
  } catch (error) {
    console.error('Lỗi khi tải dữ liệu nhật ký:', error);
  } finally {
    isLoading.value = false;
  }
}

function setTimeTab(tab) {
  timeTab.value = tab;
  if (tab === 'thisWeek') selectedWeek.value = currentInternshipWeek.value;
  if (tab === 'lastWeek') selectedWeek.value = Math.max(1, currentInternshipWeek.value - 1);
}

function jumpToWeek(w) {
  selectedWeek.value = w;
  timeTab.value = 'custom';
}

function isPastDeadline(weekNum) {
  const deadline = deadlineForWeek(weekNum);
  return Date.now() > deadline.getTime();
}

function deadlineForWeek(weekNum) {
  const weekStart = addDays(internshipStart.value, (weekNum - 1) * 7); // Monday
  // Find calendar day in Mon..Sun matching deadlineDay (0=Sun..6=Sat)
  let deadlineDate = new Date(weekStart);
  for (let i = 0; i < 7; i++) {
    const d = addDays(weekStart, i);
    if (d.getDay() === Number(config.value.deadlineDay ?? 0)) {
      deadlineDate = d;
      break;
    }
  }
  const [hh, mm] = String(config.value.deadlineTime || '23:59').split(':').map(Number);
  deadlineDate.setHours(hh || 23, mm || 59, 59, 999);
  return deadlineDate;
}

function matchStudentDiary(d, st) {
  const stIds = [st.maGhiDanh, st.maSoSinhVien, st.id, st.maNguoiDung].filter(Boolean).map(String);
  return stIds.includes(String(d.userId));
}

const studentDiaries = computed(() => {
  const week = String(selectedWeek.value);
  const pastDeadline = isPastDeadline(selectedWeek.value);

  return allStudents.value.map((st) => {
    const stId = String(st.maGhiDanh || st.maSoSinhVien || st.id);
    // Chronological (oldest → newest) for plan comparison
    const stDiaries = allDiaries.value
      .filter((d) => matchStudentDiary(d, st) && String(d.week) === week && d.status === 'Submitted')
      .sort((a, b) => new Date(a.ngayTao || a.ngayCapNhat) - new Date(b.ngayTao || b.ngayCapNhat));

    const entries = stDiaries.map((d, index) => {
      const dObj = new Date(d.ngayTao || d.ngayCapNhat);
      return {
        id: d.id,
        dateLabel: dObj.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit' }),
        weekdayLabel: weekdayVi(dObj),
        displayIndex: index + 1,
        index,
        rawData: d,
        isRead: !!d.isReadByTeacher,
      };
    });
    const entriesNewestFirst = [...entries].reverse();

    const submittedCount = entries.length;
    let status = 'Đang trong hạn';
    if (pastDeadline) {
      if (submittedCount >= config.value.minPerWeek) status = 'Nộp đủ';
      else if (submittedCount > 0) status = 'Nộp thiếu';
      else status = 'Không nộp';
    } else if (submittedCount >= config.value.minPerWeek) {
      status = 'Nộp đủ';
    }

    const nameParts = (st.hoTen || 'Sinh Viên').trim().split(/\s+/);
    const initials =
      nameParts.length >= 2
        ? (nameParts[0][0] + nameParts[nameParts.length - 1][0]).toUpperCase()
        : (nameParts[0] || 'SV').substring(0, 2).toUpperCase();

    return {
      id: stId,
      name: st.hoTen || 'Chưa cập nhật tên',
      initials,
      avatar: st.anhDaiDien || st.avatar || null,
      mssv: st.maSoSinhVien || 'N/A',
      status,
      submittedCount,
      hasUnread: entries.some((e) => !e.isRead),
      entries,
      entriesNewestFirst,
      allChronological: stDiaries,
    };
  });
});

const filteredStudentDiaries = computed(() => {
  const list = studentDiaries.value.filter(
    (st) => statusFilter.value === 'ALL' || st.status === statusFilter.value
  );
  return list.sort((a, b) => (STATUS_SORT[a.status] ?? 9) - (STATUS_SORT[b.status] ?? 9));
});

const studentDiariesAllTerm = computed(() => {
  const upTo = currentInternshipWeek.value;
  return allStudents.value.map((st) => {
    const weeklyCounts = {};
    let weeksEnough = 0;
    let totalMissing = 0;
    let totalNone = 0;

    for (let w = 1; w <= maxWeek.value; w++) {
      const count = allDiaries.value.filter(
        (d) => matchStudentDiary(d, st) && String(d.week) === String(w) && d.status === 'Submitted'
      ).length;
      weeklyCounts[w] = count;
      if (w > upTo) continue;
      if (!isPastDeadline(w)) continue;
      if (count >= config.value.minPerWeek) weeksEnough++;
      else if (count === 0) totalNone++;
      else totalMissing++;
    }

    return {
      id: String(st.maGhiDanh || st.maSoSinhVien || st.id),
      name: st.hoTen,
      mssv: st.maSoSinhVien,
      weeklyCounts,
      weeksEnough,
      totalMissing,
      totalNone,
    };
  });
});

function getHeatmapColor(student, week) {
  const count = student.weeklyCounts[week] || 0;
  if (week > currentInternshipWeek.value) return 'bg-[#cbd5e1]';
  if (!isPastDeadline(week)) return 'bg-[#cbd5e1]';
  if (count >= config.value.minPerWeek) return 'bg-[#0e7c25]';
  if (count > 0) return 'bg-[#ea580c]';
  return 'bg-[#dc2626]';
}

function toggleStudent(id) {
  const next = new Set(expandedIds.value);
  if (next.has(id)) next.delete(id);
  else {
    next.add(id);
    const st = studentDiaries.value.find((s) => s.id === id);
    if (st?.entriesNewestFirst?.length) {
      selectEntry(st, st.entriesNewestFirst[0]);
    }
  }
  expandedIds.value = next;
}

function currentEntry(student) {
  const id = activeEntryId[student.id];
  return (
    student.entries.find((e) => e.id === id) ||
    student.entriesNewestFirst?.[0] ||
    student.entries[student.entries.length - 1] ||
    null
  );
}

function fullViewKey(student) {
  const e = currentEntry(student);
  return e ? `${student.id}:${e.id}` : '';
}

function isFullView(student) {
  const key = fullViewKey(student);
  return key ? fullViewIds.value.has(key) : false;
}

function fieldsForEntry(student) {
  return isFullView(student) ? activeFields.value : previewFields.value;
}

function toggleFullView(student) {
  const key = fullViewKey(student);
  if (!key) return;
  const next = new Set(fullViewIds.value);
  if (next.has(key)) next.delete(key);
  else next.add(key);
  fullViewIds.value = next;
  const entry = currentEntry(student);
  if (entry) selectEntry(student, entry);
}

function entryFeedbacks(student) {
  const entry = currentEntry(student);
  const list = entry?.rawData?.feedbacks;
  return Array.isArray(list) ? list : [];
}

function planBanner(student) {
  const entry = currentEntry(student);
  if (!entry) return null;
  const chron = student.allChronological || [];
  const idx = chron.findIndex((d) => d.id === entry.id);
  if (idx <= 0) return null;
  const prev = chron[idx - 1];
  if (!prev?.nextPlan?.trim()) return null;
  const d = new Date(prev.ngayTao || prev.ngayCapNhat);
  return {
    dateLabel: d.toLocaleDateString('vi-VN'),
    text: prev.nextPlan.trim(),
  };
}

async function selectEntry(student, entry) {
  activeEntryId[student.id] = entry.id;
  if (!entry.isRead) {
    try {
      await api.put(`/diaries/${entry.id}/read`);
      entry.isRead = true;
      entry.rawData.isReadByTeacher = true;
      const src = allDiaries.value.find((d) => d.id === entry.id);
      if (src) src.isReadByTeacher = true;
    } catch {
      /* ignore */
    }
  }
}

function getAiSummary(student) {
  if (!student.entries.length) {
    return 'Sinh viên chưa nộp nhật ký cho tuần này.';
  }
  const entry = currentEntry(student)?.rawData;
  const work = entry?.taskDescription || entry?.newKnowledge || 'Đã nộp nhật ký';
  const firstName = (student.name || '').trim().split(/\s+/).pop() || 'Sinh viên';
  const lines = [
    `${firstName}: ${student.submittedCount}/${config.value.minPerWeek} nhật ký tuần ${selectedWeek.value} (${student.status}).`,
    `Nội dung gần nhất: ${String(work).slice(0, 160)}`,
  ];
  if (entry?.nextPlan) lines.push(`Kế hoạch tiếp theo: ${String(entry.nextPlan).slice(0, 120)}`);
  return lines.join(' ');
}

function fieldValue(raw, fieldId) {
  if (!raw) return '---';
  const map = {
    taskDescription: raw.taskDescription,
    newKnowledge: raw.newKnowledge,
    completionLevel: raw.completionLevel != null ? String(raw.completionLevel) : null,
    issues: raw.issues,
    solutions: raw.solutions,
    feedback: raw.feedback || raw.mentorFeedback,
    supportNeeded: raw.supportNeeded,
    feeling: raw.feeling != null ? String(raw.feeling) : null,
    nextPlan: raw.nextPlan,
    proofFile: evidenceLabel(raw.evidence),
    proofDescription: evidenceDesc(raw.evidence),
  };
  return map[fieldId] || raw[fieldId] || '---';
}

function evidenceLabel(evidence) {
  try {
    const e = typeof evidence === 'string' ? JSON.parse(evidence) : evidence;
    return e?.fileName || e?.name || (e ? 'Đã đính kèm' : '---');
  } catch {
    return evidence || '---';
  }
}

function evidenceDesc(evidence) {
  try {
    const e = typeof evidence === 'string' ? JSON.parse(evidence) : evidence;
    return e?.description || '---';
  } catch {
    return '---';
  }
}

function studentStatusBadgeClass(status) {
  switch (status) {
    case 'Nộp đủ':
      return 'bg-[#c6f6d5] text-[#276749]';
    case 'Nộp thiếu':
      return 'bg-[#feebc8] text-[#c05621]';
    case 'Không nộp':
      return 'bg-[#fed7d7] text-[#9b2c2c]';
    case 'Đang trong hạn':
      return 'bg-slate-200 text-slate-700';
    default:
      return 'bg-slate-100 text-slate-600';
  }
}

function formatDate(isoString) {
  if (!isoString) return '';
  const d = new Date(isoString);
  return (
    d.toLocaleDateString('vi-VN') +
    ' ' +
    d.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' })
  );
}

function openDiaryDetail(entry, student) {
  viewingStudent.value = student;
  viewingDiary.value = entry;
  viewingPlanBanner.value = planBanner(student);
  feedbackContent.value = '';
  isDrawerOpen.value = true;
  selectEntry(student, entry);
}

function closeDrawer() {
  isDrawerOpen.value = false;
  viewingDiary.value = null;
  viewingStudent.value = null;
  viewingPlanBanner.value = null;
}

async function submitInlineFeedback(student) {
  const entry = currentEntry(student);
  const content = inlineFeedback[student.id]?.trim();
  if (!entry) return;
  if (!content) {
    toastMessage.value = 'Nội dung nhận xét không được để trống';
    showToast.value = true;
    setTimeout(() => {
      showToast.value = false;
    }, 3000);
    return;
  }
  isSubmittingFeedback.value = true;
  try {
    const res = await api.put(`/diaries/${entry.id}/feedback`, {
      teacherName: authStore.user?.hoTen || 'GVHD',
      content,
    });
    inlineFeedback[student.id] = '';
    toastMessage.value = res.data?.message || 'Đã gửi nhận xét thành công';
    showToast.value = true;
    setTimeout(() => {
      showToast.value = false;
    }, 3000);
    await loadData();
  } catch (err) {
    toastMessage.value = err.response?.data?.message || 'Gửi nhận xét thất bại';
    showToast.value = true;
  } finally {
    isSubmittingFeedback.value = false;
  }
}

async function submitFeedback() {
  if (!viewingDiary.value) return;
  if (!feedbackContent.value.trim()) {
    toastMessage.value = 'Nội dung nhận xét không được để trống';
    showToast.value = true;
    setTimeout(() => {
      showToast.value = false;
    }, 3000);
    return;
  }
  isSubmittingFeedback.value = true;
  try {
    const res = await api.put(`/diaries/${viewingDiary.value.id}/feedback`, {
      teacherName: authStore.user?.hoTen || 'GVHD',
      content: feedbackContent.value,
    });
    toastMessage.value = res.data?.message || 'Đã gửi phản hồi thành công';
    showToast.value = true;
    setTimeout(() => {
      showToast.value = false;
    }, 3000);
    await loadData();
    closeDrawer();
  } catch (err) {
    toastMessage.value = err.response?.data?.message || 'Có lỗi xảy ra khi gửi phản hồi';
    showToast.value = true;
  } finally {
    isSubmittingFeedback.value = false;
  }
}

function alignToMonday(d) {
  const x = new Date(d);
  x.setHours(0, 0, 0, 0);
  const day = x.getDay();
  const diff = day === 0 ? -6 : 1 - day;
  x.setDate(x.getDate() + diff);
  return x;
}

function addDays(d, n) {
  const x = new Date(d);
  x.setDate(x.getDate() + n);
  return x;
}

function weekNumberForDate(date) {
  const start = internshipStart.value;
  const ms = alignToMonday(date) - start;
  const w = Math.floor(ms / (7 * 24 * 60 * 60 * 1000)) + 1;
  return Math.min(maxWeek.value, Math.max(1, w));
}

function fmtShort(d) {
  return d.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit' });
}

function weekdayVi(d) {
  const map = ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
  return map[d.getDay()] || '';
}
</script>

<style scoped>
.animate-slide-in {
  animation: slideIn 0.3s forwards;
}
@keyframes slideIn {
  from {
    transform: translateX(100%);
  }
  to {
    transform: translateX(0);
  }
}
</style>
