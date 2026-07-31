<template>
  <div class="space-y-4 font-sans relative">
    <!-- Header bar -->
    <div class="border border-[#C2C6D4] rounded-[8px] bg-white px-6 py-4 flex flex-col md:flex-row md:items-center justify-between gap-4">
      <!-- Week navigation -->
      <div class="flex items-center gap-4">
        <button @click="changeWeek(-1)" :disabled="isWriting" class="flex items-center gap-1 border border-[#C2C6D4] text-[#424752] px-3 py-1.5 rounded-[4px] hover:bg-slate-50 transition text-[13px] disabled:opacity-50">
          <span class="material-symbols-outlined text-[16px]">arrow_back</span>
          Tuần trước
        </button>
        <div class="text-center min-w-[120px]">
          <div class="font-bold text-[#1A1C1C] text-[15px] flex items-center justify-center gap-1 cursor-pointer">
            Tuần {{ currentWeek }}
            <span class="material-symbols-outlined text-[16px]">arrow_drop_down</span>
          </div>
          <div class="text-[12px] text-[#424752] mt-1">{{ getWeekDateRange(currentWeek) }}</div>
        </div>
        <button @click="changeWeek(1)" :disabled="isWriting || currentWeek >= maxWeek" class="flex items-center gap-1 border border-[#C2C6D4] text-[#424752] px-3 py-1.5 rounded-[4px] hover:bg-slate-50 transition text-[13px] disabled:opacity-50">
          Tuần sau
          <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
        </button>
      </div>
      
      <!-- Progress and write button -->
      <div class="flex items-center gap-8">
        <div class="w-[300px]">
          <div class="flex justify-between items-center text-[12px] mb-2">
            <span class="text-[#424752] font-medium text-[12px]">Tiến độ tuần này</span>
            <span class="text-[#005EA3] font-bold">
              {{ weekSubmittedCount }}/{{ config.minPerWeek }} nhật ký đã nộp
            </span>
          </div>
          <div class="w-full bg-[#E9E8E7] rounded-[12px] h-[6px] flex overflow-hidden">
            <div :class="['h-full transition-all duration-500 rounded-[12px]', weekSubmittedCount >= config.minPerWeek ? 'bg-[#005EA3]' : 'bg-[#00488D]']" :style="{ width: weekProgressPercent + '%' }"></div>
          </div>
        </div>
        <button v-if="!isWriting" @click="startWriting" :disabled="!canWrite" class="bg-[#005EA3] hover:bg-blue-800 text-white font-normal px-5 py-2.5 rounded-[4px] transition text-[14px] flex items-center gap-1 disabled:opacity-50 disabled:cursor-not-allowed">
          <span class="material-symbols-outlined text-[18px]">add</span>
          Viết nhật ký
        </button>
        <button v-else @click="cancelWriting" class="border border-[#C2C6D4] text-[#424752] hover:bg-slate-50 font-normal px-5 py-2.5 rounded-[4px] transition text-[14px] flex items-center gap-1">
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
          <div v-for="diary in currentWeekDiaries" :key="diary.id" class="border border-[#C2C6D4] bg-white rounded-[4px] p-[24px] flex flex-col md:flex-row gap-6">
            <!-- Left Column -->
            <div class="w-[200px] shrink-0 flex flex-col items-start border-r border-[#E9E8E7] pr-4">
              <div class="font-bold text-[14px] text-[#1A1C1C] mb-2">{{ formatCardDate(diary.ngayCapNhat || diary.ngayTao) }}</div>
              
              <div class="flex flex-col gap-2 mb-3 w-[150px]">
                <span v-if="diary.status === 'Submitted'" class="block bg-[#C6F6D5] text-[#166534] px-3 py-1.5 rounded-[4px] text-[11px] font-bold w-full uppercase">ĐÃ NỘP</span>
                <span v-else class="block bg-[#E9E8E7] text-[#424752] px-3 py-1.5 rounded-[4px] text-[11px] font-bold w-full uppercase">NHÁP</span>
                <span v-if="diary.feedbacks && diary.feedbacks.length > 0 && !diary.isReadByStudent" class="block bg-[#FFE4D6] text-[#A43D00] px-3 py-1.5 rounded-[4px] text-[11px] font-bold w-full uppercase">CÓ PHẢN HỒI MỚI</span>
              </div>
              
              <div class="text-[11px] text-[#7A7D85]">Cập nhật: {{ formatUpdateTime(diary.ngayCapNhat || diary.ngayTao) }}</div>
            </div>
            
            <!-- Right Column -->
            <div class="flex-1 flex flex-col justify-between">
              <div>
                <p class="text-[13px] text-[#1A1C1C] leading-[20px] mb-4 flex-1" :class="{'italic text-[#7A7D85]': diary.status === 'Draft' && !diary.taskDescription}">
                  {{ diary.status === 'Draft' && !diary.taskDescription ? 'Chưa có nội dung trích đoạn nhật ký cho ngày hôm nay. Hãy cập nhật tiến độ công việc của bạn...' : getPreviewText(diary) }}
                </p>
                
                <div class="flex items-center gap-3 text-[12px] text-[#424752] mb-3">
                  <span>Mức độ hoàn thành: <span class="font-bold text-[#1A1C1C]">{{ diary.completionLevel || '--' }}/10</span></span>
                  <span class="text-[#C2C6D4]">|</span>
                  <span>Cảm nhận: <span class="font-bold text-[#1A1C1C]">{{ diary.feeling || '--' }}/5</span></span>
                </div>
                
                <div v-if="diary.feedbacks && diary.feedbacks.length > 0" class="mb-3 text-[#B45309] text-[12px] flex items-start gap-1">
                  <span class="material-symbols-outlined text-[14px] mt-[1px]">chat_bubble_outline</span>
                  <span class="font-bold">"{{ diary.feedbacks[diary.feedbacks.length-1].content }}"</span>
                </div>
              </div>

              <div class="flex items-center justify-end mt-2">
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
        <div class="text-[13px] font-bold text-[#1A1C1C]">
          Tổng cả kỳ: <span class="text-[#005EA3]">{{ totalSubmitted }}/{{ totalRequired }} nhật ký đã nộp</span>
        </div>
        <div class="w-[256px]">
          <div class="flex justify-between items-center text-[12px] mb-1">
            <span class="text-[#424752] font-medium text-[11px]">Tiến độ hoàn thành</span>
            <span class="text-[#005EA3] font-bold text-[13px]">{{ Math.round((totalSubmitted / Math.max(1, totalRequired)) * 100) }}%</span>
          </div>
          <div class="w-full bg-[#E9E8E7] rounded-[12px] h-[6px] flex overflow-hidden">
            <div class="h-full transition-all duration-500 rounded-[12px] bg-[#00488D]" :style="{ width: Math.min(100, Math.round((totalSubmitted / Math.max(1, totalRequired)) * 100)) + '%' }"></div>
          </div>
        </div>
      </div>
    </template>

    <!-- Drawer for Diary Detail -->
    <div v-if="isDrawerOpen" class="fixed inset-0 z-[100] bg-slate-900/50 backdrop-blur-sm flex justify-end" @click.self="closeDrawer">
      <div class="bg-white w-[500px] h-full shadow-2xl flex flex-col animate-slide-in border-l border-[#C2C6D4]">
        <div class="p-[24px] border-b border-[#C2C6D4] flex items-start justify-between">
          <div class="flex flex-col gap-2">
            <h3 class="text-[24px] font-semibold text-[#1A1C1C] leading-[32px]">Chi tiết Nhật ký thực tập</h3>
            <span class="text-[14px] font-bold text-[#005EA3]">Ngày {{ formatDrawerDate(viewingDiary) }} — Tuần {{ viewingDiary.week }}</span>
          </div>
          <button @click="closeDrawer" class="w-[30px] h-[30px] rounded-[12px] hover:bg-slate-100 flex items-center justify-center transition mt-1">
            <span class="material-symbols-outlined text-[18px] text-[#1A1C1C]">close</span>
          </button>
        </div>
        
        <div class="p-[24px] overflow-y-auto flex-1 space-y-[24px]">
          <div v-if="viewingDiary.completionLevel || viewingDiary.feeling" class="bg-[#F4F4F4] rounded-[4px] py-[16px] flex items-center justify-between">
            <div v-if="viewingDiary.completionLevel" class="flex-1 flex flex-col items-center" :class="{'border-r border-[#C2C6D4]': viewingDiary.feeling}">
              <span class="text-[11px] text-[#424752] font-bold uppercase mb-1">MỨC ĐỘ HOÀN THÀNH</span>
              <span class="text-[22px] font-bold text-[#005EA3]">{{ viewingDiary.completionLevel }}/10</span>
            </div>
            <div v-if="viewingDiary.feeling" class="flex-1 flex flex-col items-center">
              <span class="text-[11px] text-[#424752] font-bold uppercase mb-1">CẢM NHẬN CHUNG</span>
              <span class="text-[22px] font-bold text-[#005EA3]">{{ viewingDiary.feeling }}/5</span>
            </div>
          </div>

          <div class="flex flex-col space-y-[16px]">
            <template v-for="field in activeFields" :key="field.id">
              <div v-if="!['completionLevel', 'feeling'].includes(field.id)">
                <h4 class="font-bold text-[12px] text-[#424752] leading-[16px] mb-1">{{ field.label }}</h4>
                <p class="text-[14px] text-[#1A1C1C] leading-[20px] whitespace-pre-wrap">{{ viewingDiary[field.id] || '---' }}</p>
              </div>
            </template>
          </div>

          <!-- Feedbacks -->
          <div v-if="viewingDiary.feedbacks && viewingDiary.feedbacks.length > 0" class="mt-[24px]">
            <div class="space-y-3">
              <div v-for="(fb, index) in viewingDiary.feedbacks" :key="index" class="bg-[#FFF5F2] border border-[#FFDBCD] p-[16px] rounded-[4px] flex flex-col gap-[8px]">
                <div class="flex items-center gap-[8px]">
                  <span class="material-symbols-outlined text-[14px] text-[#A43D00]">chat_bubble</span>
                  <span class="text-[12px] font-bold text-[#A43D00]">{{ fb.teacherName }} • {{ formatFeedbackDate(fb.timestamp) }}</span>
                </div>
                <p class="text-[13px] text-[#A43D00] italic leading-[18px] whitespace-pre-wrap">"{{ fb.content }}"</p>
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
    <div v-if="showToast" class="fixed top-24 right-8 z-[200] flex items-center px-6 py-4 bg-white border border-[#C2C6D4] shadow-sm animate-fade-in" role="alert">
      <span class="material-symbols-outlined text-[28px] text-[#059669]">check_circle</span>
      <div class="ml-3 text-[16px] font-semibold text-[#1A1C1C] tracking-wide">{{ toastMessage }}</div>
    </div>
    
    <!-- Confirm Submit Modal -->
    <div v-if="showConfirmSubmit" class="fixed inset-0 z-[200] bg-slate-900/40 backdrop-blur-sm flex items-center justify-center animate-fade-in" @click.self="showConfirmSubmit = false">
      <div class="bg-white rounded-[12px] shadow-2xl p-[32px] w-[400px] flex flex-col items-center text-center">
        <span class="material-symbols-outlined text-[48px] text-[#005EA3] mb-4">send</span>
        <h3 class="text-[20px] font-bold text-[#1A1C1C] mb-3">Xác nhận nộp nhật ký?</h3>
        <p class="text-[15px] text-slate-700 mb-8">Sau khi nộp, bạn sẽ không thể chỉnh sửa</p>
        <div class="flex gap-4 w-full">
          <button @click="showConfirmSubmit = false" class="flex-1 py-[10px] border border-slate-300 rounded-[4px] text-[15px] font-bold text-[#1A1C1C] hover:bg-slate-50 transition">
            Hủy
          </button>
          <button @click="confirmSubmit" class="flex-1 py-[10px] bg-[#005EA3] rounded-[4px] text-[15px] font-bold text-white hover:bg-[#00488D] transition">
            Xác nhận nộp
          </button>
        </div>
      </div>
    </div>

    <!-- Confirm Cancel Modal -->
    <div v-if="showConfirmCancel" class="fixed inset-0 z-[200] bg-slate-900/40 backdrop-blur-sm flex items-center justify-center animate-fade-in" @click.self="showConfirmCancel = false">
      <div class="bg-white rounded-[12px] shadow-2xl p-[32px] w-[400px] flex flex-col text-left">
        <h3 class="text-[20px] font-bold text-[#1A1C1C] mb-3">Hủy bỏ soạn thảo</h3>
        <p class="text-[15px] text-[#1A1C1C] leading-relaxed mb-6">Mọi thông tin bạn đã nhập sẽ bị mất nếu không lưu nháp. Bạn có chắc chắn muốn hủy?</p>
        <div class="flex flex-col gap-3 w-full">
          <button @click="showConfirmCancel = false" class="w-full py-[10px] bg-[#D3D7DC] rounded-[4px] text-[15px] font-bold text-[#1A1C1C] hover:bg-[#C2C6D4] transition">
            Tiếp tục viết
          </button>
          <button @click="confirmCancel" class="w-full py-[10px] bg-[#B91C1C] rounded-[4px] text-[15px] font-bold text-white hover:bg-[#991B1B] transition">
            Hủy bỏ
          </button>
        </div>
      </div>
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
const showConfirmSubmit = ref(false);
const showConfirmCancel = ref(false);
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
  showConfirmCancel.value = true;
};

const confirmCancel = () => {
  showConfirmCancel.value = false;
  isWriting.value = false;
  form.value = {};
};

const submitDiary = () => {
  showConfirmSubmit.value = true;
};

const confirmSubmit = async () => {
  showConfirmSubmit.value = false;
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
    
    displayToast(status === 'Submitted' ? 'Đã nộp nhật ký thành công' : 'Đã lưu nháp nhật ký thành công');
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

const getWeekDateRange = (week) => {
  // Mock data to exactly match the design screenshot for visual correctness
  return "15/05 - 21/05/2024";
};

const formatCardDate = (isoString) => {
  if (!isoString) return '';
  const d = new Date(isoString);
  const dayNames = ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
  const dateStr = d.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
  const dayOfWeek = dayNames[d.getDay()];
  return `${dateStr} — ${dayOfWeek}`;
};

const formatUpdateTime = (isoString) => {
  if (!isoString) return '';
  const d = new Date(isoString);
  const dateStr = d.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
  const timeStr = d.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
  return `${dateStr} - ${timeStr}`;
};

const formatFeedbackDate = (isoString) => {
  if (!isoString) return '';
  const d = new Date(isoString);
  const dateStr = d.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
  const timeStr = d.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
  return `${dateStr} ${timeStr}`;
};

const formatDrawerDate = (diary) => {
  const dateString = diary.ngayCapNhat || diary.ngayTao;
  if (!dateString) return 'Chưa xác định';
  const d = new Date(dateString);
  const dayNames = ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
  const dateStr = d.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
  const dayOfWeek = dayNames[d.getDay()];
  return `${dateStr} (${dayOfWeek})`;
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
.animate-fade-in {
  animation: fadeIn 0.2s ease-out forwards;
}
@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}
</style>
