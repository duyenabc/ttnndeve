<template>
  <div class="font-sans text-slate-800 max-w-[1100px] w-full relative pb-28 mx-auto">
    <nav class="flex items-center gap-2 text-[13px] text-slate-500 mb-3">
      <router-link to="/teacher/classes" class="hover:text-[#005EA3] transition-colors">Lớp của tôi</router-link>
      <span class="material-symbols-outlined text-[14px]">chevron_right</span>
      <span class="text-slate-600">Lớp Thực tập K64</span>
      <span class="material-symbols-outlined text-[14px]">chevron_right</span>
      <span class="font-semibold text-slate-800">Cấu hình lớp học</span>
    </nav>

    <h1 class="text-[28px] font-bold text-slate-900 mb-8 tracking-tight">Cấu hình lớp học</h1>

    <section class="mb-8">
      <button
        type="button"
        class="flex items-center gap-2 mb-4 w-fit"
        @click="isStageOpen = !isStageOpen"
      >
        <span class="material-symbols-outlined text-[22px] text-slate-700">
          {{ isStageOpen ? 'expand_more' : 'chevron_right' }}
        </span>
        <h2 class="text-[18px] font-bold text-slate-900">Cấu hình giai đoạn</h2>
      </button>

      <div v-show="isStageOpen" class="border-t border-slate-200 pt-6 space-y-8">
        <div>
          <h3 class="text-[#005EA3] font-bold text-[14px] flex items-center gap-2 mb-4">
            <span class="material-symbols-outlined text-[18px]">lock</span>
            Giai đoạn cứng (Giáo vụ)
          </h3>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div class="border border-slate-300 rounded-lg p-4 bg-[#F9FAFB]">
              <div class="text-[11px] font-bold text-slate-500 uppercase tracking-wide mb-1">Đăng ký thực tập</div>
              <div class="text-[14px] text-slate-800 font-medium">01/08/2024 - 15/08/2024</div>
            </div>
            <div class="border border-slate-300 rounded-lg p-4 bg-[#F9FAFB]">
              <div class="text-[11px] font-bold text-slate-500 uppercase tracking-wide mb-1">Thực tập tại doanh nghiệp</div>
              <div class="text-[14px] text-slate-800 font-medium">01/09/2024 - 31/12/2024</div>
            </div>
          </div>
        </div>

        <div>
          <h3 class="text-[#005EA3] font-bold text-[14px] flex items-center gap-2 mb-4">
            <span class="material-symbols-outlined text-[18px]">calendar_today</span>
            Giai đoạn linh hoạt
          </h3>
          <div class="border border-slate-300 rounded-lg p-5">
            <div class="font-bold text-[14px] mb-3 text-slate-800">Nộp báo cáo giữa kỳ</div>
            <div class="flex flex-wrap items-center gap-3 mb-2">
              <input
                v-model="flexiblePhase.start"
                type="text"
                class="border border-slate-300 text-slate-700 rounded-md px-3 py-2.5 w-full sm:w-[220px] outline-none focus:border-[#005EA3] focus:ring-1 focus:ring-[#005EA3]"
              />
              <span class="text-slate-500 text-[14px]">đến</span>
              <input
                v-model="flexiblePhase.end"
                type="text"
                class="border border-slate-300 text-slate-700 rounded-md px-3 py-2.5 w-full sm:w-[220px] outline-none focus:border-[#005EA3] focus:ring-1 focus:ring-[#005EA3]"
              />
            </div>
            <p class="text-[12px] text-slate-500 italic">
              Giảng viên có thể điều chỉnh ngày trong phạm vi cho phép
            </p>
          </div>
        </div>

        <div>
          <div class="flex flex-wrap items-center justify-between gap-3 mb-4">
            <h3 class="font-bold text-[14px] text-slate-800">Mốc thời gian nội bộ</h3>
            <button
              type="button"
              class="border border-[#005EA3] text-[#005EA3] font-semibold text-[13px] px-4 py-2 rounded-md flex items-center gap-1 hover:bg-blue-50 transition"
              @click="addInternalPhase"
            >
              <span class="material-symbols-outlined text-[16px]">add</span>
              Thêm giai đoạn nội bộ
            </button>
          </div>

          <div
            v-for="(phase, index) in internalPhases"
            :key="phase.id"
            class="border border-slate-200 bg-[#F4F7FA] rounded-lg p-5 relative mb-3"
          >
            <button
              type="button"
              class="absolute top-3 right-3 text-slate-400 hover:text-red-500 transition"
              title="Xóa giai đoạn"
              @click="removeInternalPhase(index)"
            >
              <span class="material-symbols-outlined text-[20px]">delete</span>
            </button>
            <div class="flex flex-col md:flex-row gap-5 pr-8">
              <div class="flex-1">
                <label class="block text-[11px] font-bold mb-1.5 text-slate-600 uppercase tracking-wide">
                  Tên giai đoạn <span class="text-red-500">*</span>
                </label>
                <input
                  v-model="phase.name"
                  type="text"
                  class="w-full border border-slate-300 text-slate-800 rounded-md px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white"
                />
              </div>
              <div class="flex-[1.2]">
                <label class="block text-[11px] font-bold mb-1.5 text-slate-600 uppercase tracking-wide">
                  Thời gian
                </label>
                <div class="flex items-center gap-2">
                  <input
                    v-model="phase.start"
                    type="text"
                    class="w-full border border-slate-300 text-slate-800 rounded-md px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white"
                  />
                  <span class="text-slate-400 shrink-0">-</span>
                  <input
                    v-model="phase.end"
                    type="text"
                    class="w-full border border-slate-300 text-slate-800 rounded-md px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <section class="mb-6">
      <button
        type="button"
        class="flex items-center gap-2 mb-4 w-fit"
        @click="isDiaryOpen = !isDiaryOpen"
      >
        <span class="material-symbols-outlined text-[22px] text-slate-700">
          {{ isDiaryOpen ? 'expand_less' : 'chevron_right' }}
        </span>
        <h2 class="text-[18px] font-bold text-slate-900">Cấu hình Nhật ký thực tập</h2>
      </button>

      <div v-show="isDiaryOpen" class="border-t border-slate-200 pt-6">
        <div class="flex items-center justify-between gap-4 mb-6">
          <div>
            <div class="font-bold text-[14px] mb-1 text-slate-800">Bật/Tắt nhật ký</div>
            <div class="text-[13px] text-slate-500">
              Cho phép sinh viên nộp nhật ký thực tập hàng tuần
            </div>
          </div>
          <label class="relative inline-flex items-center cursor-pointer shrink-0">
            <input v-model="diaryConfig.isEnabled" type="checkbox" class="sr-only peer" />
            <div
              class="w-12 h-6 bg-slate-300 rounded-full peer peer-checked:after:translate-x-[26px] after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:rounded-full after:h-[20px] after:w-[20px] after:transition-all peer-checked:bg-[#005EA3]"
            ></div>
          </label>
        </div>

        <div v-if="diaryConfig.isEnabled" class="bg-[#F9FAFB] border border-slate-200 rounded-lg p-5">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
            <div>
              <label class="block text-[13px] font-bold mb-2 text-slate-700">
                Số nhật ký tối thiểu/tuần <span class="text-red-500">*</span>
              </label>
              <input
                v-model.number="diaryConfig.minPerWeek"
                type="number"
                min="1"
                max="7"
                class="w-full border border-slate-300 text-slate-800 rounded-md px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white"
              />
              <p class="text-[12px] text-slate-500 mt-1">Tối thiểu 1 nhật ký/tuần.</p>
            </div>
            <div>
              <label class="block text-[13px] font-bold mb-2 text-slate-700">
                Deadline chốt sổ mỗi tuần <span class="text-red-500">*</span>
              </label>
              <div class="flex gap-2">
                <select
                  v-model="diaryConfig.deadlineDay"
                  class="w-1/2 border border-slate-300 text-slate-800 rounded-md px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white"
                >
                  <option v-for="opt in dayOptions" :key="opt.value" :value="opt.value">
                    {{ opt.label }}
                  </option>
                </select>
                <input
                  v-model="diaryConfig.deadlineTime"
                  type="time"
                  class="w-1/2 border border-slate-300 text-slate-800 rounded-md px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white"
                />
              </div>
            </div>
          </div>

          <h3 class="font-bold text-[14px] text-slate-800 mb-4 border-b border-slate-200 pb-2">
            Trường hiển thị trong form nhật ký
          </h3>
          <div class="space-y-3">
            <div
              v-for="field in diaryConfig.fields"
              :key="field.id"
              class="flex items-center justify-between bg-white border border-slate-200 rounded-md p-3 gap-3"
            >
              <div class="flex items-center gap-3">
                <input
                  :id="field.id"
                  v-model="field.isEnabled"
                  type="checkbox"
                  class="w-4 h-4 text-[#005EA3] border-slate-300 rounded focus:ring-[#005EA3]"
                />
                <label :for="field.id" class="text-[14px] text-slate-700 cursor-pointer">
                  {{ field.label }}
                </label>
              </div>
              <div v-if="field.isEnabled" class="flex items-center gap-2 shrink-0">
                <span class="text-[12px] text-slate-500">Bắt buộc:</span>
                <label class="relative inline-flex items-center cursor-pointer">
                  <input v-model="field.isRequired" type="checkbox" class="sr-only peer" />
                  <div
                    class="w-9 h-5 bg-slate-300 rounded-full peer peer-checked:after:translate-x-full after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:rounded-full after:h-[16px] after:w-[16px] after:transition-all peer-checked:bg-red-500"
                  ></div>
                </label>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <div
      v-if="showToast"
      class="fixed top-24 right-8 z-50 flex items-center p-4 text-slate-500 bg-white rounded-lg shadow-lg border border-slate-100"
      role="alert"
    >
      <div
        :class="[
          'inline-flex items-center justify-center shrink-0 w-8 h-8 rounded-lg',
          toastType === 'success' ? 'text-green-500 bg-green-100' : 'text-red-500 bg-red-100',
        ]"
      >
        <span class="material-symbols-outlined text-[20px]">
          {{ toastType === 'success' ? 'check_circle' : 'error' }}
        </span>
      </div>
      <div class="ml-3 text-sm font-normal text-slate-800">{{ toastMessage }}</div>
    </div>

    <div
      class="fixed bottom-0 left-0 right-0 lg:left-64 bg-white border-t border-slate-200 px-6 py-3.5 flex flex-col sm:flex-row items-stretch sm:items-center justify-between gap-3 z-10 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)]"
    >
      <div class="flex items-center gap-2 text-[13px] text-slate-500">
        <span class="material-symbols-outlined text-[18px]">info</span>
        Mọi thay đổi cần được lưu để có hiệu lực toàn hệ thống.
      </div>
      <div class="flex items-center gap-3 justify-end">
        <button
          type="button"
          class="border border-slate-300 text-slate-700 font-bold px-5 py-2 rounded-md hover:bg-slate-50 transition text-[13px]"
          @click="cancelChanges"
        >
          Hủy thay đổi
        </button>
        <button
          type="button"
          :disabled="isSaving"
          class="bg-[#005EA3] text-white font-bold px-5 py-2 rounded-md hover:bg-[#003362] transition flex items-center gap-2 text-[13px] disabled:opacity-70"
          @click="saveConfig"
        >
          <span v-if="isSaving" class="material-symbols-outlined text-[18px] animate-spin">refresh</span>
          <span v-else class="material-symbols-outlined text-[18px]">save</span>
          Lưu cấu hình
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import api from '@/api/api';

const route = useRoute();
const classId = route.params.id || '101';

const isStageOpen = ref(true);
const isDiaryOpen = ref(true);
const isSaving = ref(false);
const showToast = ref(false);
const toastMessage = ref('');
const toastType = ref('success');

const flexiblePhase = ref({ start: '10/15/2024', end: '10/20/2024' });
const internalPhases = ref([
  { id: 1, name: 'Gặp mặt đầu kỳ', start: '09/05/2024', end: '09/10/2024' },
]);

const diaryConfig = ref({
  isEnabled: true,
  minPerWeek: 2,
  deadlineDay: 0,
  deadlineTime: '23:59',
  fields: [],
});

const defaultFields = [
  { id: 'taskDescription', label: 'Mô tả nhiệm vụ đã thực hiện', isEnabled: true, isRequired: true },
  { id: 'newKnowledge', label: 'Kiến thức/kỹ năng mới', isEnabled: true, isRequired: false },
  { id: 'completionLevel', label: 'Mức độ hoàn thành (thang 1-10)', isEnabled: true, isRequired: true },
  { id: 'issues', label: 'Khó khăn/vướng mắc', isEnabled: true, isRequired: false },
  { id: 'solutions', label: 'Cách xử lý/hướng giải quyết', isEnabled: true, isRequired: false },
  { id: 'feedback', label: 'Phản hồi từ người hướng dẫn (nếu có)', isEnabled: true, isRequired: false },
  { id: 'supportNeeded', label: 'Cần GVHD hỗ trợ gì không', isEnabled: true, isRequired: false },
  { id: 'feeling', label: 'Cảm nhận chung (thang 1-5)', isEnabled: true, isRequired: true },
  { id: 'nextPlan', label: 'Kế hoạch làm việc tiếp theo', isEnabled: true, isRequired: true },
  { id: 'proofFile', label: 'Minh chứng công việc (file, tối đa 100MB)', isEnabled: true, isRequired: false },
  { id: 'proofDescription', label: 'Mô tả minh chứng', isEnabled: true, isRequired: false },
];

const dayOptions = [
  { value: 1, label: 'Thứ 2' },
  { value: 2, label: 'Thứ 3' },
  { value: 3, label: 'Thứ 4' },
  { value: 4, label: 'Thứ 5' },
  { value: 5, label: 'Thứ 6' },
  { value: 6, label: 'Thứ 7' },
  { value: 0, label: 'Chủ nhật' },
];

let nextPhaseId = 2;

function addInternalPhase() {
  internalPhases.value.push({
    id: nextPhaseId++,
    name: '',
    start: '',
    end: '',
  });
}

function removeInternalPhase(index) {
  internalPhases.value.splice(index, 1);
}

onMounted(async () => {
  try {
    const res = await api.get(`/giangvien/classes/${classId}/diary-config`);
    if (res.data) {
      diaryConfig.value = { ...res.data };
    } else {
      diaryConfig.value.fields = JSON.parse(JSON.stringify(defaultFields));
    }
  } catch {
    diaryConfig.value.fields = JSON.parse(JSON.stringify(defaultFields));
  }
});

const validateConfig = () => {
  if (diaryConfig.value.isEnabled) {
    const min = Number(diaryConfig.value.minPerWeek);
    if (!min || min < 1 || min > 7) {
      displayToast('Tối thiểu 1 nhật ký/tuần', 'error');
      return false;
    }
    if (!diaryConfig.value.fields.some((f) => f.isEnabled)) {
      displayToast('Form nhật ký phải có ít nhất 1 trường thông tin được bật', 'error');
      return false;
    }
  }
  return true;
};

const saveConfig = async () => {
  if (!validateConfig()) return;
  isSaving.value = true;
  try {
    const res = await api.put(`/giangvien/classes/${classId}/diary-config`, diaryConfig.value);
    if (res.data?.config) diaryConfig.value = { ...res.data.config };
    displayToast(res.data?.message || 'Lưu cấu hình lớp thực tập thành công', 'success');
  } catch (err) {
    displayToast(
      err.response?.data?.message || 'Lưu cấu hình lớp thực tập thất bại, vui lòng thử lại sau!',
      'error'
    );
  } finally {
    isSaving.value = false;
  }
};

const cancelChanges = async () => {
  isSaving.value = true;
  try {
    const res = await api.get(`/giangvien/classes/${classId}/diary-config`);
    if (res.data) diaryConfig.value = { ...res.data };
    displayToast('Đã khôi phục cài đặt trước đó', 'success');
  } catch {
    /* ignore */
  } finally {
    isSaving.value = false;
  }
};

const displayToast = (msg, type) => {
  toastMessage.value = msg;
  toastType.value = type;
  showToast.value = true;
  setTimeout(() => {
    showToast.value = false;
  }, 3000);
};
</script>
