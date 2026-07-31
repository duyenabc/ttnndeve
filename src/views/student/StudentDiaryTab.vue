<template>
  <div class="space-y-4 font-sans relative">
    <!-- Header bar -->
    <div class="border border-slate-200 rounded-[12px] bg-white p-4 flex flex-col md:flex-row md:items-center justify-between gap-4">
      <!-- Week navigation -->
      <div class="flex items-center gap-4">
        <button @click="changeWeek(-1)" :disabled="isWriting" class="flex items-center gap-1 border border-slate-300 text-slate-500 px-3 py-1.5 rounded-[8px] hover:bg-slate-50 transition text-[13px] disabled:opacity-50">
          <span class="material-symbols-outlined text-[16px]">arrow_back</span>
          Tuần trước
        </button>
        <div class="text-center">
          <div class="font-bold text-slate-900 text-[16px] flex items-center justify-center gap-1">
            Tuần {{ currentWeek }}
          </div>
          <div class="text-[12px] text-slate-500">Hạn nộp: {{ config.deadlineDayName }} {{ config.deadlineTime }}</div>
        </div>
        <button @click="changeWeek(1)" :disabled="isWriting || currentWeek >= maxWeek" class="flex items-center gap-1 border border-slate-300 text-slate-700 px-3 py-1.5 rounded-[8px] hover:bg-slate-50 transition text-[13px] disabled:opacity-50">
          Tuần sau
          <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
        </button>
      </div>
      
      <!-- Progress and write button -->
      <div class="flex items-center gap-6">
        <div class="w-[240px]">
          <div class="flex justify-between items-center text-[12px] font-bold mb-1">
            <span class="text-slate-500 font-normal text-[11px]">Tiến độ tuần này</span>
            <span :class="weekSubmittedCount >= config.minPerWeek ? 'text-[#005EA3]' : 'text-amber-600'">
              {{ weekSubmittedCount }}/{{ config.minPerWeek }} nhật ký đã nộp
            </span>
          </div>
          <div class="w-full bg-slate-200 rounded-full h-[6px] flex overflow-hidden">
            <div :class="['h-full transition-all duration-500', weekSubmittedCount >= config.minPerWeek ? 'bg-[#005EA3]' : 'bg-amber-500']" :style="{ width: weekProgressPercent + '%' }"></div>
          </div>
        </div>
        <button v-if="!isWriting" @click="startWriting" :disabled="!canWrite" class="bg-[#005EA3] hover:bg-blue-800 text-white font-bold px-4 py-2 rounded-[8px] transition text-[13px] flex items-center gap-1 disabled:opacity-50 disabled:cursor-not-allowed">
          <span class="material-symbols-outlined text-[16px]">add</span>
          Viết nhật ký
        </button>
        <button v-else @click="cancelWriting" class="border border-slate-300 text-slate-700 hover:bg-slate-50 font-bold px-4 py-2 rounded-[8px] transition text-[13px] flex items-center gap-1">
          Hủy viết
        </button>
      </div>
    </div>

    <div v-if="isLoading" class="text-center py-8">
      <span class="material-symbols-outlined animate-spin text-3xl text-[#005EA3]">refresh</span>
      <p class="text-sm text-slate-500 mt-2">Đang tải dữ liệu nhật ký...</p>
    </div>

    <template v-else>
      <!-- Writing Form -->
      <div v-if="isWriting" class="bg-white rounded-[12px] p-8 shadow-sm border border-slate-200 pb-20">
        <div class="mb-8">
          <h2 class="font-bold text-[24px] text-slate-900 mb-2">Viết nhật ký tuần hiện tại</h2>
          <p class="text-[13px] text-slate-500 font-medium">Tuần thực tập: Tuần {{ currentWeek }} (Hạn nộp: {{ config.deadlineDayName }} {{ config.deadlineTime }})</p>
        </div>

        <div class="grid grid-cols-2 gap-8 mb-8">
          <div v-if="activeFields.find(f => f.id === 'completionLevel')">
            <label class="block text-[13px] font-bold text-slate-800 mb-3">Mức độ hoàn thành</label>
            <div class="flex items-center gap-1">
              <span v-for="i in 10" :key="i" @click="form.completionLevel = i" 
                    class="material-symbols-outlined cursor-pointer text-[28px] select-none transition" 
                    :class="(form.completionLevel >= i) ? 'text-amber-400 font-variation-fill' : 'text-slate-300'">star</span>
            </div>
          </div>
          <div v-if="activeFields.find(f => f.id === 'feeling')">
            <label class="block text-[13px] font-bold text-slate-800 mb-3">Cảm nhận chung</label>
            <div class="flex items-center gap-1">
              <span v-for="i in 5" :key="i" @click="form.feeling = i" 
                    class="material-symbols-outlined cursor-pointer text-[28px] select-none transition" 
                    :class="(form.feeling >= i) ? 'text-amber-400 font-variation-fill' : 'text-slate-300'">star</span>
            </div>
          </div>
        </div>
        
        <div class="space-y-6">
          <template v-for="field in activeFields" :key="field.id">
            <div v-if="!['completionLevel', 'feeling'].includes(field.id)">
              <label class="block text-[13px] font-bold text-slate-800 mb-2">
                {{ field.label }} <span v-if="field.isRequired" class="text-slate-500">(*)</span>
              </label>
              
              <textarea
                v-if="['taskDescription', 'newKnowledge', 'issues', 'solutions', 'nextPlan', 'supportNeeded'].includes(field.id)"
                v-model="form[field.id]"
                rows="4"
                class="w-full border border-slate-200 rounded-[8px] px-4 py-3 text-[13px] outline-none focus:border-[#005EA3] bg-white placeholder:italic text-slate-700"
                :placeholder="field.id === 'taskDescription' ? 'Ghi rõ các đầu việc cụ thể bạn đã xử lý trong tuần này...' :
                             field.id === 'newKnowledge' ? 'Những công nghệ, quy trình hoặc kỹ năng mềm bạn đã tích lũy...' :
                             field.id === 'issues' ? 'Nêu rõ những khó khăn bạn đang gặp phải...' :
                             field.id === 'solutions' ? 'Bạn đã hoặc dự định giải quyết những khó khăn đó như thế nào...' :
                             field.id === 'nextPlan' ? 'Dự kiến công việc cho tuần kế tiếp...' : `Nhập ${field.label.toLowerCase()}...`"
              ></textarea>
              <input
                v-else-if="field.id === 'proofFile'"
                type="file"
                class="w-full text-[13px]"
              />
              <input
                v-else
                v-model="form[field.id]"
                type="text"
                class="w-full border border-slate-200 rounded-[8px] px-4 py-3 text-[13px] outline-none focus:border-[#005EA3] bg-white placeholder:italic text-slate-700"
              />
            </div>
          </template>
        </div>

        <!-- Sticky Footer for Actions -->
        <div class="fixed bottom-0 left-0 right-0 lg:left-64 bg-white border-t border-slate-200 p-4 px-8 flex justify-end gap-3 z-30 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)]">
          <button @click="cancelWriting" class="px-6 py-2.5 text-slate-600 font-bold text-[13px] hover:text-slate-900 transition">
            Hủy bỏ
          </button>
          <button @click="saveDraft" class="px-6 py-2.5 border border-slate-300 text-slate-700 font-bold rounded-[8px] text-[13px] hover:bg-slate-50 transition">
            Lưu nháp
          </button>
          <button @click="submitDiary" :disabled="!isValidForm" class="px-6 py-2.5 bg-[#005EA3] text-white font-bold rounded-[8px] text-[13px] hover:bg-blue-800 transition disabled:opacity-50 flex items-center gap-2">
            <span class="material-symbols-outlined text-[18px]">send</span>
            Nộp nhật ký
          </button>
        </div>
      </div>

      <!-- Diary List -->
      <div v-else class="space-y-4">
        <template v-if="currentWeekDiaries.length > 0">
          <div v-for="diary in currentWeekDiaries" :key="diary.id" class="border border-slate-200 bg-white rounded-[12px] p-5 flex flex-col md:flex-row gap-6 hover:shadow-md transition">
            <div class="w-[200px] shrink-0 border-r border-slate-100 pr-4">
              <div class="font-bold text-[14px] text-slate-900 mb-1">{{ formatDate(diary.ngayCapNhat || diary.ngayTao) }}</div>
              <div class="text-[11px] text-slate-500 mb-2 italic">Cập nhật lần cuối</div>
              <div class="flex flex-wrap gap-2 mb-2">
                <span v-if="diary.status === 'Submitted'" class="inline-block bg-[#E6F4EA] text-[#137333] px-2 py-0.5 rounded text-[10px] font-bold">ĐÃ NỘP</span>
                <span v-else class="inline-block bg-slate-200 text-slate-600 px-2 py-0.5 rounded text-[10px] font-bold">NHÁP</span>
                <span v-if="diary.feedbacks && diary.feedbacks.length > 0 && !diary.isReadByStudent" class="inline-block bg-[#FCE8E6] text-[#C5221F] px-2 py-0.5 rounded text-[10px] font-bold">CÓ PHẢN HỒI MỚI</span>
              </div>
            </div>
            <div class="flex-1 flex flex-col">
              <p class="text-[13px] text-slate-700 leading-relaxed mb-3 flex-1 line-clamp-3">
                {{ getPreviewText(diary) }}
              </p>
              
              <div class="flex items-center gap-4 mb-3 text-[12px] text-slate-600 font-medium">
                <span v-if="diary.completionLevel" class="flex items-center gap-1">
                  <span class="material-symbols-outlined text-[16px] text-amber-500">task_alt</span>
                  Mức độ: {{ diary.completionLevel }}/10
                </span>
                <span v-if="diary.feeling" class="flex items-center gap-1">
                  <span class="material-symbols-outlined text-[16px] text-amber-500">mood</span>
                  Cảm nhận: {{ diary.feeling }}/5
                </span>
              </div>
              
              <div v-if="diary.feedbacks && diary.feedbacks.length > 0" class="mb-3">
                <div class="text-[#D93025] font-bold text-[12px] flex items-start gap-1">
                  <span class="material-symbols-outlined text-[16px] mt-0.5">chat_bubble_outline</span>
                  Có phản hồi mới từ Giảng viên
                </div>
              </div>

              <div class="flex items-center justify-end mt-auto">
                <button @click="openDrawer(diary)" class="text-[#005EA3] font-bold text-[13px] flex items-center gap-1 hover:underline">
                  <template v-if="diary.status === 'Draft'">
                    <span class="material-symbols-outlined text-[16px]">edit_note</span>
                    Tiếp tục viết
                  </template>
                  <template v-else>
                    Xem chi tiết
                    <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
                  </template>
                </button>
              </div>
            </div>
          </div>
        </template>
        <template v-else>
          <div class="border border-slate-200 bg-white rounded-[12px] p-8 text-center text-slate-500">
            <span class="material-symbols-outlined text-4xl mb-2 text-slate-300">menu_book</span>
            <p class="text-sm">Chưa có nhật ký nào trong tuần này.</p>
          </div>
        </template>
      </div>

      <!-- Footer progress -->
      <div v-if="!isWriting" class="mt-8 pt-[24px] border-t border-[#C2C6D4] flex items-center justify-between">
        <div class="text-[14px] font-bold text-[#1A1C1C]">
          Tổng cả kỳ: {{ totalSubmitted }}/{{ totalRequired }} nhật ký đã nộp
        </div>
        <div class="w-[256px]">
          <div class="flex justify-between items-center text-[12px] mb-1">
            <span class="text-[#424752] font-medium text-[12px]">Tiến độ hoàn thành</span>
            <span class="text-[#00488D] font-bold">{{ Math.round((totalSubmitted / Math.max(1, totalRequired)) * 100) }}%</span>
          </div>
          <div class="w-full bg-[#E9E8E7] rounded-[12px] h-[6px] flex overflow-hidden">
            <div class="h-full transition-all duration-500 rounded-[12px]" :class="totalSubmitted >= totalRequired ? 'bg-[#005503]' : 'bg-[#00488D]'" :style="{ width: Math.min(100, Math.round((totalSubmitted / Math.max(1, totalRequired)) * 100)) + '%' }"></div>
          </div>
        </div>
      </div>
    </template>

    <!-- Drawer for Diary Detail -->
    <div v-if="isDrawerOpen" class="fixed inset-0 z-[100] bg-slate-900/50 backdrop-blur-sm flex justify-end" @click.self="closeDrawer">
      <div class="bg-white w-[500px] h-full shadow-2xl flex flex-col animate-slide-in border-l border-[#C2C6D4]">
        <div class="p-[24px] border-b border-[#C2C6D4] flex items-start justify-between">
          <div class="flex flex-col gap-1">
            <h3 class="text-[24px] font-semibold text-[#1A1C1C] leading-[32px]">Chi tiết nhật ký thực tập</h3>
            <span class="text-[14px] font-bold text-[#00488D]">Tuần {{ viewingDiary.week }} - {{ viewingDiary.status === 'Submitted' ? 'Đã nộp' : 'Nháp' }}</span>
          </div>
          <button @click="closeDrawer" class="w-[30px] h-[30px] rounded-[12px] hover:bg-slate-100 flex items-center justify-center transition">
            <span class="material-symbols-outlined text-[14px] text-[#1A1C1C]">close</span>
          </button>
        </div>
        
        <div class="p-[24px] overflow-y-auto flex-1 space-y-[24px]">
          <div class="grid grid-cols-2 gap-4">
            <div v-if="viewingDiary.completionLevel" class="bg-amber-50 p-3 rounded-[8px]">
              <div class="text-[11px] text-amber-800 font-bold uppercase mb-1">Mức độ hoàn thành</div>
              <div class="text-[18px] font-bold text-amber-600">{{ viewingDiary.completionLevel }}/10</div>
            </div>
            <div v-if="viewingDiary.feeling" class="bg-amber-50 p-3 rounded-[8px]">
              <div class="text-[11px] text-amber-800 font-bold uppercase mb-1">Cảm nhận chung</div>
              <div class="text-[18px] font-bold text-amber-600">{{ viewingDiary.feeling }}/5</div>
            </div>
          </div>

          <template v-for="field in activeFields" :key="field.id">
            <div v-if="!['completionLevel', 'feeling'].includes(field.id)">
              <h4 class="font-semibold text-[12px] text-[#424752] uppercase tracking-[0.6px] leading-[16px] mb-1">{{ field.label }}</h4>
              <p class="text-[14px] text-[#1A1C1C] leading-[20px] whitespace-pre-wrap">{{ viewingDiary[field.id] || '---' }}</p>
            </div>
          </template>

          <!-- Feedbacks -->
          <div v-if="viewingDiary.feedbacks && viewingDiary.feedbacks.length > 0" class="mt-[24px] pt-[24px] border-t border-dashed border-[#C2C6D4]">
            <h4 class="font-semibold text-[12px] text-[#424752] uppercase tracking-[0.6px] mb-4">Nhận xét của Giảng viên</h4>
            <div class="space-y-3">
              <div v-for="(fb, index) in viewingDiary.feedbacks" :key="index" class="bg-[rgba(255,219,205,0.3)] border border-[#FFDBCD] p-[16px] rounded-[4px] flex flex-col gap-[8px]">
                <div class="flex items-center gap-[8px]">
                  <span class="material-symbols-outlined text-[12px] text-[#A43D00]">chat_bubble</span>
                  <span class="text-[12px] font-bold text-[#7D2D00]">{{ fb.teacherName }} • {{ formatDate(fb.timestamp) }}</span>
                </div>
                <p class="text-[13px] text-[#7D2D00] italic leading-[18px] whitespace-pre-wrap">"{{ fb.content }}"</p>
              </div>
            </div>
          </div>
        </div>
        
        <div class="p-[24px] border-t border-[#C2C6D4] flex justify-end">
          <button @click="closeDrawer" class="px-5 py-2 border border-slate-300 text-slate-700 font-bold rounded-[8px] hover:bg-slate-50 text-[13px] transition">
            Đóng
          </button>
        </div>
      </div>
    </div>
    
    <!-- Toast Notification -->
    <div v-if="showToast" class="fixed top-24 right-8 z-50 flex items-center p-4 mb-4 text-gray-500 bg-white rounded-lg shadow-lg border border-gray-100" role="alert">
      <div class="inline-flex items-center justify-center shrink-0 w-8 h-8 rounded-lg text-green-500 bg-green-100">
        <span class="material-symbols-outlined text-[20px]">check_circle</span>
      </div>
      <div class="ml-3 text-sm font-normal text-gray-800">{{ toastMessage }}</div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import api from '@/api/api';
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();
const props = defineProps({
  classId: { type: String, required: true }
});

const isLoading = ref(true);
const currentWeek = ref(1);
const maxWeek = ref(15);
const diaries = ref([]);
const config = ref({
  isEnabled: false,
  minPerWeek: 2,
  deadlineDayName: 'Chủ nhật',
  deadlineTime: '23:59',
  fields: []
});

const isWriting = ref(false);
const form = ref({});
const isDrawerOpen = ref(false);
const viewingDiary = ref({});
const showToast = ref(false);
const toastMessage = ref('');

onMounted(async () => {
  await loadConfig();
  await loadDiaries();
  isLoading.value = false;
});

const loadConfig = async () => {
  try {
    const res = await api.get(`/giangvien/classes/${props.classId}/diary-config`);
    if (res.data) {
      config.value = {
        isEnabled: res.data.isEnabled,
        minPerWeek: res.data.minPerWeek || 2,
        deadlineDayName: getDayName(res.data.deadlineDay),
        deadlineTime: res.data.deadlineTime || '23:59',
        fields: res.data.fields || []
      };
    }
  } catch (e) {
    console.error('Error loading config', e);
  }
};

const loadDiaries = async () => {
  try {
    const res = await api.get('/diaries', {
      params: { classId: props.classId, userId: authStore.user?.id }
    });
    diaries.value = res.data || [];
  } catch (e) {
    console.error('Error loading diaries', e);
  }
};

const getDayName = (day) => {
  const map = { 1: 'Thứ 2', 2: 'Thứ 3', 3: 'Thứ 4', 4: 'Thứ 5', 5: 'Thứ 6', 6: 'Thứ 7', 0: 'Chủ nhật' };
  return map[day] || 'Chủ nhật';
};

const activeFields = computed(() => config.value.fields.filter(f => f.isEnabled));

const currentWeekDiaries = computed(() => {
  return diaries.value.filter(d => Number(d.week) === currentWeek.value);
});

const weekSubmittedCount = computed(() => {
  return currentWeekDiaries.value.filter(d => d.status === 'Submitted').length;
});

const weekProgressPercent = computed(() => {
  if (!config.value.minPerWeek) return 0;
  return Math.min(100, Math.round((weekSubmittedCount.value / config.value.minPerWeek) * 100));
});

const totalSubmitted = computed(() => diaries.value.filter(d => d.status === 'Submitted').length);
const totalRequired = computed(() => config.value.minPerWeek * maxWeek.value);

const canWrite = computed(() => {
  return config.value.isEnabled && weekSubmittedCount.value < config.value.minPerWeek * 2; // Allow double writing if they want, but disable eventually
});

const isValidForm = computed(() => {
  for (const field of activeFields.value) {
    if (field.isRequired && !form.value[field.id]) {
      return false;
    }
  }
  return true;
});

const changeWeek = (delta) => {
  const newWeek = currentWeek.value + delta;
  if (newWeek > 0 && newWeek <= maxWeek.value) {
    currentWeek.value = newWeek;
  }
};

const startWriting = () => {
  isWriting.value = true;
  form.value = {};
};

const cancelWriting = () => {
  isWriting.value = false;
  form.value = {};
};

const submitDiary = async () => {
  if (!confirm('Sau khi nộp, bạn không thể tự sửa. Xác nhận nộp?')) return;
  await saveDiary('Submitted');
};

const saveDraft = async () => {
  await saveDiary('Draft');
};

const saveDiary = async (status) => {
  try {
    let payload = {
      ...form.value,
      classId: props.classId,
      userId: authStore.user?.maNguoiDung || authStore.user?.id,
      week: currentWeek.value,
      status: status
    };
    
    if (!payload.feedbacks) payload.feedbacks = [];
    
    // Remove undefined fields which Firestore rejects
    payload = JSON.parse(JSON.stringify(payload));
    
    if (payload.id) {
      await api.put(`/diaries/${payload.id}`, payload);
    } else {
      await api.post('/diaries', payload);
    }
    
    displayToast(status === 'Submitted' ? 'Đã nộp nhật ký thành công' : 'Đã lưu nháp');
    isWriting.value = false;
    await loadDiaries();
  } catch (e) {
    console.error('Save diary error:', e);
    alert('Lưu nhật ký thất bại, vui lòng thử lại sau!');
  }
};

const getPreviewText = (diary) => {
  // Use taskDescription or fallback to something else
  if (diary.taskDescription) return diary.taskDescription;
  const anyKey = Object.keys(diary).find(k => k !== 'id' && k !== 'week' && k !== 'status' && k !== 'ngayTao' && k !== 'userId' && k !== 'classId');
  return diary[anyKey] || 'Nội dung nhật ký...';
};

const formatDate = (isoString) => {
  if (!isoString) return '';
  const d = new Date(isoString);
  return d.toLocaleDateString('vi-VN') + ' ' + d.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
};

const openDrawer = (diary) => {
  if (diary.status === 'Draft') {
    isWriting.value = true;
    form.value = { ...diary };
  } else {
    viewingDiary.value = diary;
    isDrawerOpen.value = true;
    
    // Mark as read if not already
    if (!diary.isReadByStudent && diary.feedbacks && diary.feedbacks.length > 0) {
      // In a real app we would call API to mark as read
      diary.isReadByStudent = true;
    }
  }
};

const closeDrawer = () => {
  isDrawerOpen.value = false;
  viewingDiary.value = {};
};

const displayToast = (msg) => {
  toastMessage.value = msg;
  showToast.value = true;
  setTimeout(() => showToast.value = false, 3000);
};

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
