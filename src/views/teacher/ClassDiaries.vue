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
              <option value="15">Tuần 15</option>
              <option value="14">Tuần 14</option>
              <option value="13">Tuần 13</option>
              <option value="12">Tuần 12</option>
              <option value="11">Tuần 11</option>
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

    <div v-if="isLoading" class="text-center py-8">
      <span class="material-symbols-outlined animate-spin text-3xl text-[#005EA3]">refresh</span>
      <p class="text-sm text-slate-500 mt-2">Đang tải dữ liệu...</p>
    </div>

    <template v-else>
      <!-- STUDENT DIARY LIST (Tuần này / Tuần trước / Tùy chỉnh) -->
      <div v-if="timeTab !== 'term'" class="space-y-4">
        <!-- Week plan alignment banner -->
        <div class="bg-blue-50 border border-blue-200 rounded-lg p-3 text-sm text-blue-800 flex items-center gap-2 mb-4">
          <span class="material-symbols-outlined text-[20px]">info</span>
          <strong>Lưu ý:</strong> Hãy đối chiếu các nhật ký tuần này với "Kế hoạch tuần tiếp theo" mà sinh viên đã ghi trong tuần trước để đánh giá mức độ hoàn thành.
        </div>

        <div
          v-for="student in filteredStudentDiaries"
          :key="student.id"
          class="bg-white rounded-lg border border-slate-300 p-5 shadow-sm space-y-4 transition hover:shadow-md relative"
        >
          <div class="flex items-start justify-between">
            <div class="flex items-start gap-4">
              <!-- Avatar -->
              <div class="w-12 h-12 rounded-lg bg-[#e2e8f0] text-slate-700 font-bold flex items-center justify-center text-[15px]">
                {{ student.initials }}
              </div>

              <!-- Info -->
              <div class="pt-0.5">
                <div class="flex items-center gap-2">
                  <h3 class="font-medium text-slate-900 text-[16px]">{{ student.name }}</h3>
                  <span v-if="student.isNew" class="bg-[#e6f0fa] text-[#005EA3] text-[10px] px-1.5 py-0.5 rounded-sm font-bold uppercase tracking-wider">MỚI</span>
                </div>
                <p class="text-[13px] text-slate-500 mt-1">MSSV: {{ student.mssv }} • {{ student.entries.length }}/{{ config.minPerWeek }} nhật ký (Tuần {{ selectedWeek }})</p>
                
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

            <!-- Status & Action -->
            <div class="flex flex-col items-end gap-3 pt-1">
              <span
                class="px-3 py-1 rounded-full text-[12px] font-medium"
                :class="studentStatusBadgeClass(student.status)"
              >
                {{ student.status }}
              </span>
              <button 
                v-if="student.entries.length > 0" 
                @click="openDiaryDetail(currentActiveEntry(student))" 
                class="text-[#005EA3] hover:underline text-[13px] font-bold mt-4 flex items-center gap-1"
              >
                Xem chi tiết
                <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
              </button>
            </div>
          </div>

          <!-- Preview Content -->
          <div v-if="student.entries.length > 0" class="bg-[#f0f4f8] border-l-[3px] border-[#005EA3] p-3 rounded-r-md flex items-start gap-2 text-[13px] text-slate-700 ml-[64px]">
            <span class="material-symbols-outlined text-[#005EA3] text-[18px] shrink-0 mt-0.5">menu_book</span>
            <div class="leading-relaxed">
              <span class="italic font-bold text-[#005EA3]">Trích đoạn Nhật ký #{{ student.activeEntryIndex + 1 }}:</span>
              <span class="ml-1 text-slate-800">{{ getPreviewText(currentActiveEntry(student).rawData) }}</span>
            </div>
          </div>
        </div>

        <div v-if="filteredStudentDiaries.length === 0" class="text-center py-10 bg-white border border-slate-200 rounded-lg text-slate-500">
          <p>Không có sinh viên nào khớp với bộ lọc.</p>
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
              <tr v-for="student in studentDiariesAllTerm" :key="student.id" class="hover:bg-slate-50 transition">
                <td class="py-4 px-5 font-medium text-[#005EA3]">{{ student.name }}</td>
                <td class="py-4 px-5 text-slate-600">{{ student.mssv }}</td>
                <td class="py-4 px-5 text-center text-slate-700">{{ student.totalSubmitted }}/{{ config.minPerWeek * 15 }}</td>
                <td class="py-4 px-5 text-center text-amber-500">{{ student.totalMissing }}</td>
                <td class="py-4 px-5 text-center text-red-500">{{ student.totalNone }}</td>
                <td class="py-4 px-5">
                  <div class="flex items-center gap-1">
                    <div v-for="w in 15" :key="w" class="w-[18px] h-[18px] tooltip-trigger relative group" :class="getHeatmapColor(student, w)">
                      <!-- Tooltip -->
                      <div class="absolute bottom-full left-1/2 -translate-x-1/2 mb-1 hidden group-hover:block bg-slate-800 text-white text-[10px] py-1 px-2 rounded whitespace-nowrap z-10">
                        Tuần {{ w }}: {{ student.weeklyCounts[w] || 0 }} / {{ config.minPerWeek }}
                      </div>
                    </div>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </template>

    <!-- Drawer for Diary Detail and Feedback -->
    <div v-if="isDrawerOpen" class="fixed inset-0 z-[100] bg-slate-900/50 backdrop-blur-sm flex justify-end" @click.self="closeDrawer">
      <div class="bg-white w-[600px] h-full shadow-2xl flex flex-col animate-slide-in">
        <div class="p-4 border-b border-slate-200 flex items-center justify-between bg-slate-50">
          <h2 class="font-bold text-[16px] text-slate-800 flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3]">menu_book</span> Chi tiết Nhật ký
          </h2>
          <button @click="closeDrawer" class="text-slate-400 hover:text-slate-600">✕</button>
        </div>
        
        <div class="p-6 overflow-y-auto flex-1 space-y-6">
          <div class="bg-blue-50/50 p-4 rounded-lg border border-blue-100 flex items-center gap-4">
            <div class="w-10 h-10 rounded bg-[#e2e8f0] text-slate-700 font-bold flex items-center justify-center text-[14px]">
              {{ viewingStudent.initials }}
            </div>
            <div>
              <p class="font-bold text-slate-900">{{ viewingStudent.name }}</p>
              <p class="text-[12px] text-slate-500">MSSV: {{ viewingStudent.mssv }} • Lớp {{ classId }}</p>
            </div>
          </div>

          <div class="space-y-4">
            <div v-for="field in activeFields" :key="field.id">
              <h4 class="font-bold text-[12px] text-slate-500 uppercase mb-1">{{ field.label }}</h4>
              <p class="text-[14px] text-slate-800 whitespace-pre-wrap bg-slate-50 p-3 rounded border border-slate-100 min-h-[40px]">{{ viewingDiary.rawData[field.id] || '---' }}</p>
            </div>
          </div>

          <!-- Feedbacks List -->
          <div class="mt-8 pt-4 border-t border-slate-200">
            <h3 class="font-bold text-[15px] text-slate-900 mb-4 flex items-center gap-1">
              <span class="material-symbols-outlined text-[18px]">forum</span> Phản hồi của bạn
            </h3>
            
            <div v-if="viewingDiary.rawData.feedbacks && viewingDiary.rawData.feedbacks.length > 0" class="space-y-3 mb-6">
              <div v-for="(fb, i) in viewingDiary.rawData.feedbacks" :key="i" class="bg-[#F9FAFB] border border-slate-200 rounded-[8px] p-3">
                <div class="flex justify-between text-[11px] mb-1">
                  <span class="font-bold text-slate-700">{{ fb.teacherName }}</span>
                  <span class="text-slate-400">{{ formatDate(fb.timestamp) }}</span>
                </div>
                <p class="text-[13px] text-slate-800 leading-relaxed">{{ fb.content }}</p>
              </div>
            </div>

            <!-- Write Feedback -->
            <div>
              <label class="block text-[12px] font-bold text-slate-600 mb-2">Thêm phản hồi mới:</label>
              <textarea
                v-model="feedbackContent"
                rows="3"
                class="w-full border border-slate-300 rounded-[8px] px-3 py-2 text-[13px] outline-none focus:border-[#005EA3]"
                placeholder="Nhập nhận xét, góp ý hoặc đánh giá cho sinh viên..."
              ></textarea>
              <div class="mt-3 flex justify-end">
                <button 
                  @click="submitFeedback" 
                  :disabled="!feedbackContent.trim() || isSubmittingFeedback" 
                  class="px-5 py-2 bg-[#005EA3] text-white font-bold rounded-[8px] text-[13px] hover:bg-blue-800 transition disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-1"
                >
                  <span v-if="isSubmittingFeedback" class="material-symbols-outlined animate-spin text-[16px]">refresh</span>
                  <span v-else class="material-symbols-outlined text-[16px]">send</span>
                  Gửi phản hồi
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Toast -->
    <div v-if="showToast" class="fixed top-24 right-8 z-50 flex items-center p-4 mb-4 text-gray-500 bg-white rounded-lg shadow-lg border border-gray-100" role="alert">
      <div class="inline-flex items-center justify-center shrink-0 w-8 h-8 rounded-lg text-green-500 bg-green-100">
        <span class="material-symbols-outlined text-[20px]">check_circle</span>
      </div>
      <div class="ml-3 text-sm font-normal text-gray-800">{{ toastMessage }}</div>
    </div>

  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';
import api from '@/api/api';

const route = useRoute();
const classId = computed(() => route.params.id || '101');

const timeTab = ref('thisWeek');
const selectedWeek = ref('13'); // Defaults to Week 13 for demo
const statusFilter = ref('ALL');

const statusOptions = [
  { value: 'ALL', label: 'Tất cả' },
  { value: 'Nộp đủ', label: 'Nộp đủ' },
  { value: 'Nộp thiếu', label: 'Nộp thiếu' },
  { value: 'Không nộp', label: 'Không nộp' }
];

const allStudents = ref([]);
const allDiaries = ref([]);
const isLoading = ref(true);
const config = ref({ minPerWeek: 2, fields: [] });
const activeFields = computed(() => config.value.fields.filter(f => f.isEnabled));

// Drawer state
const isDrawerOpen = ref(false);
const viewingDiary = ref(null);
const viewingStudent = ref(null);
const feedbackContent = ref('');
const isSubmittingFeedback = ref(false);

const showToast = ref(false);
const toastMessage = ref('');

onMounted(async () => {
  await loadData();
});

const loadData = async () => {
  isLoading.value = true;
  try {
    // 1. Fetch config
    const confRes = await api.get(`/giangvien/classes/${classId.value}/diary-config`);
    if (confRes.data) {
      config.value = {
        minPerWeek: confRes.data.minPerWeek || 2,
        fields: confRes.data.fields || []
      };
    }

    // 2. Fetch students
    const stRes = await api.get(`/giangvien/classes/${classId.value}/students`);
    allStudents.value = Array.isArray(stRes.data) ? stRes.data : (stRes.data?.items || []);

    // 3. Fetch diaries for class
    const diariesRes = await api.get('/diaries', { params: { classId: classId.value } });
    allDiaries.value = diariesRes.data || [];
  } catch (error) {
    console.error('Lỗi khi tải dữ liệu nhật ký:', error);
  } finally {
    isLoading.value = false;
  }
};

// Processed list for current week
const studentDiaries = computed(() => {
  return allStudents.value.map(st => {
    const stId = String(st.maGhiDanh || st.maSoSinhVien || st.id);
    
    // Filter diaries by student AND week (only Submitted ones)
    const stDiaries = allDiaries.value.filter(d => 
      (String(d.userId) === stId || String(d.userId) === String(st.maSoSinhVien)) &&
      String(d.week) === selectedWeek.value &&
      d.status === 'Submitted'
    );

    // Sort descending by date
    stDiaries.sort((a, b) => new Date(b.ngayTao) - new Date(a.ngayTao));

    const entries = stDiaries.map(d => {
      const dObj = new Date(d.ngayTao);
      return {
        id: d.id,
        dateLabel: dObj.toLocaleDateString('vi-VN'),
        rawData: d
      };
    });

    let status = 'Không nộp';
    if (entries.length >= config.value.minPerWeek) status = 'Nộp đủ';
    else if (entries.length > 0) status = 'Nộp thiếu';

    const nameParts = (st.hoTen || 'Sinh Viên').trim().split(' ');
    const initials = nameParts.length >= 2 
      ? (nameParts[0][0] + nameParts[nameParts.length - 1][0]).toUpperCase() 
      : nameParts[0].substring(0, 2).toUpperCase();

    return {
      id: stId,
      name: st.hoTen || 'Chưa cập nhật tên',
      initials,
      mssv: st.maSoSinhVien || 'N/A',
      status,
      isNew: entries.length > 0 && (new Date() - new Date(stDiaries[0].ngayTao) < 24*60*60*1000),
      entries,
      activeEntryIndex: 0
    };
  });
});

const filteredStudentDiaries = computed(() => {
  return studentDiaries.value.filter(st => {
    return statusFilter.value === 'ALL' || st.status === statusFilter.value;
  });
});

// Processed list for Term Heatmap
const studentDiariesAllTerm = computed(() => {
  return allStudents.value.map(st => {
    const stId = String(st.maGhiDanh || st.maSoSinhVien || st.id);
    
    const weeklyCounts = {};
    let totalSubmitted = 0;
    
    for (let w = 1; w <= 15; w++) {
      const count = allDiaries.value.filter(d => 
        (String(d.userId) === stId || String(d.userId) === String(st.maSoSinhVien)) &&
        String(d.week) === String(w) &&
        d.status === 'Submitted'
      ).length;
      weeklyCounts[w] = count;
      totalSubmitted += count;
    }
    
    let totalMissing = 0;
    let totalNone = 0;
    for (let w = 1; w <= 15; w++) {
      if (weeklyCounts[w] === 0) totalNone++;
      else if (weeklyCounts[w] < config.value.minPerWeek) totalMissing++;
    }

    const nameParts = (st.hoTen || 'Sinh Viên').trim().split(' ');
    const initials = nameParts.length >= 2 
      ? (nameParts[0][0] + nameParts[nameParts.length - 1][0]).toUpperCase() 
      : nameParts[0].substring(0, 2).toUpperCase();

    return {
      id: stId,
      name: st.hoTen,
      mssv: st.maSoSinhVien,
      initials,
      weeklyCounts,
      totalSubmitted,
      totalMissing,
      totalNone
    };
  });
});

function getHeatmapColor(student, week) {
  const count = student.weeklyCounts[week] || 0;
  if (count >= config.value.minPerWeek) return 'bg-[#0e7c25]'; // green
  if (count > 0) return 'bg-[#ea580c]'; // orange (missing)
  // For weeks in the past that are 0, red. Otherwise grey. We mock current week as 13.
  if (week <= Number(selectedWeek.value)) return 'bg-[#dc2626]'; // red (not submitted)
  return 'bg-[#cbd5e1]'; // grey (future)
}

function currentActiveEntry(student) {
  if (!student.entries || !student.entries.length) return null;
  return student.entries[student.activeEntryIndex] || student.entries[0];
}

function getPreviewText(rawData) {
  if (!rawData) return 'Không có nội dung';
  return rawData.taskDescription || rawData.newKnowledge || 'Đã nộp nhật ký';
}

function studentStatusBadgeClass(status) {
  switch (status) {
    case 'Nộp đủ': return 'bg-[#c6f6d5] text-[#276749]';
    case 'Nộp thiếu': return 'bg-[#feebc8] text-[#c05621]';
    case 'Không nộp': return 'bg-[#fed7d7] text-[#9b2c2c]';
    default: return 'bg-slate-100 text-slate-600';
  }
}

const formatDate = (isoString) => {
  if (!isoString) return '';
  const d = new Date(isoString);
  return d.toLocaleDateString('vi-VN') + ' ' + d.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
};

function openDiaryDetail(entry) {
  const student = studentDiaries.value.find(s => s.entries.some(e => e.id === entry.id));
  viewingStudent.value = student;
  viewingDiary.value = entry;
  feedbackContent.value = '';
  isDrawerOpen.value = true;
}

function closeDrawer() {
  isDrawerOpen.value = false;
  viewingDiary.value = null;
  viewingStudent.value = null;
}

async function submitFeedback() {
  if (!feedbackContent.value.trim() || !viewingDiary.value) return;
  isSubmittingFeedback.value = true;
  
  try {
    const diaryId = viewingDiary.value.id;
    await api.put(`/diaries/${diaryId}/feedback`, {
      teacherName: 'GVHD', // Should be authStore.user.hoTen
      content: feedbackContent.value
    });
    
    toastMessage.value = 'Đã gửi phản hồi thành công';
    showToast.value = true;
    setTimeout(() => showToast.value = false, 3000);
    
    // Refresh to see the new feedback
    await loadData();
    closeDrawer();
  } catch (e) {
    alert('Có lỗi xảy ra khi gửi phản hồi');
  } finally {
    isSubmittingFeedback.value = false;
  }
}

watch(timeTab, (newVal) => {
  if (newVal === 'thisWeek') selectedWeek.value = '13';
  if (newVal === 'lastWeek') selectedWeek.value = '12';
});

</script>

<style scoped>
.animate-slide-in {
  animation: slideIn 0.3s forwards;
}
@keyframes slideIn {
  from { transform: translateX(100%); }
  to { transform: translateX(0); }
}
</style>
