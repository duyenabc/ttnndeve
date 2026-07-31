<template>
  <div class="font-sans text-[#333333] max-w-[1200px] w-full p-8 relative pb-24 mx-auto">
    <!-- Breadcrumb -->
    <div class="flex items-center gap-2 text-[14px] text-gray-500 mb-6">
      <router-link to="/teacher/classes" class="hover:text-blue-600 cursor-pointer">Lớp của tôi</router-link>
      <span class="material-symbols-outlined text-[16px]">chevron_right</span>
      <span class="cursor-pointer hover:text-blue-600">Lớp Thực tập K64</span>
      <span class="material-symbols-outlined text-[16px]">chevron_right</span>
      <span class="font-semibold text-blue-600">Cấu hình lớp học</span>
    </div>

    <!-- Title -->
    <h1 class="text-[24px] font-bold text-gray-900 mb-8">Cấu hình lớp học</h1>

    <!-- Accordion 1: Cấu hình giai đoạn -->
    <div class="mb-8">
      <div class="flex items-center gap-2 mb-4 cursor-pointer w-fit" @click="isStageOpen = !isStageOpen">
        <span class="material-symbols-outlined text-[20px] font-bold select-none">{{ isStageOpen ? 'expand_more' : 'chevron_right' }}</span>
        <h2 class="text-[18px] font-bold">Cấu hình giai đoạn</h2>
      </div>
      <div v-show="isStageOpen" class="border-t border-gray-200 pt-6">
        
        <!-- Giai đoạn cứng -->
        <div class="mb-8">
          <h3 class="text-[#005EA3] font-bold text-[14px] flex items-center gap-2 mb-4">
            <span class="material-symbols-outlined text-[18px]">lock</span>
            Giai đoạn cứng (Giáo vụ)
          </h3>
          <div class="flex gap-4">
            <div class="flex-1 border border-gray-300 rounded p-4 bg-[#F9FAFB]">
              <div class="text-[12px] font-bold text-gray-600 mb-1">ĐĂNG KÝ THỰC TẬP</div>
              <div class="text-[14px] text-gray-800">01/08/2024 - 15/08/2024</div>
            </div>
            <div class="flex-1 border border-gray-300 rounded p-4 bg-[#F9FAFB]">
              <div class="text-[12px] font-bold text-gray-600 mb-1">THỰC TẬP TẠI DOANH NGHIỆP</div>
              <div class="text-[14px] text-gray-800">01/09/2024 - 31/12/2024</div>
            </div>
          </div>
        </div>

        <!-- Giai đoạn linh hoạt -->
        <div class="mb-8">
          <h3 class="text-[#005EA3] font-bold text-[14px] flex items-center gap-2 mb-4">
            <span class="material-symbols-outlined text-[18px]">calendar_today</span>
            Giai đoạn linh hoạt
          </h3>
          <div class="border border-gray-300 rounded p-5">
            <div class="font-bold text-[14px] mb-3">Nộp báo cáo giữa kỳ</div>
            <div class="flex items-center gap-4 mb-2">
              <input type="text" value="10/15/2024" class="border border-gray-400 text-gray-700 rounded px-3 py-2.5 w-[300px] outline-none focus:border-[#005EA3]" />
              <span class="text-gray-600 text-[14px]">đến</span>
              <input type="text" value="10/20/2024" class="border border-gray-400 text-gray-700 rounded px-3 py-2.5 w-[300px] outline-none focus:border-[#005EA3]" />
            </div>
            <div class="text-[12px] text-gray-500 italic">Giảng viên có thể điều chỉnh ngày trong phạm vi cho phép</div>
          </div>
        </div>

        <!-- Mốc thời gian nội bộ -->
        <div class="mb-4">
          <div class="flex items-center justify-between mb-4">
            <h3 class="font-bold text-[14px]">Mốc thời gian nội bộ</h3>
            <button class="border border-[#005EA3] text-[#005EA3] font-semibold text-[13px] px-4 py-2 rounded flex items-center gap-1 hover:bg-blue-50 transition select-none">
              <span class="material-symbols-outlined text-[16px]">add</span>
              Thêm giai đoạn nội bộ
            </button>
          </div>
          <div class="border border-gray-200 bg-[#F4F7FA] rounded p-5 relative">
            <button class="absolute top-3 right-3 text-gray-500 hover:text-red-500 transition">
              <span class="material-symbols-outlined text-[20px]">delete</span>
            </button>
            <div class="flex gap-6">
              <div class="flex-1">
                <label class="block text-[12px] font-bold mb-1.5 text-gray-700">TÊN GIAI ĐOẠN <span class="text-red-500">*</span></label>
                <input type="text" value="Gặp mặt đầu kỳ" class="w-full border border-gray-300 text-gray-800 rounded px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white" />
              </div>
              <div class="flex-[1.2]">
                <label class="block text-[12px] font-bold mb-1.5 text-gray-700">THỜI GIAN</label>
                <div class="flex items-center gap-2">
                  <input type="text" value="09/05/2024" class="w-full border border-gray-300 text-gray-800 rounded px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white" />
                  <span class="text-gray-500">-</span>
                  <input type="text" value="09/10/2024" class="w-full border border-gray-300 text-gray-800 rounded px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white" />
                </div>
              </div>
            </div>
          </div>
        </div>

      </div>
    </div>

    <!-- Accordion 2: Cấu hình Nhật ký thực tập -->
    <div class="mb-6">
      <div class="flex items-center gap-2 mb-4 cursor-pointer w-fit" @click="isDiaryOpen = !isDiaryOpen">
        <span class="material-symbols-outlined text-[20px] font-bold select-none">{{ isDiaryOpen ? 'expand_less' : 'chevron_right' }}</span>
        <h2 class="text-[18px] font-bold">Cấu hình Nhật ký thực tập</h2>
      </div>
      <div v-show="isDiaryOpen" class="border-t border-gray-200 pt-6">
        <div class="flex items-center justify-between mb-6">
          <div>
            <div class="font-bold text-[14px] mb-1">Bật/Tắt nhật ký</div>
            <div class="text-[13px] text-gray-500">Cho phép sinh viên nộp nhật ký thực tập hàng tuần</div>
          </div>
          <!-- Toggle switch -->
          <label class="relative inline-flex items-center cursor-pointer mr-2">
            <input type="checkbox" v-model="diaryConfig.isEnabled" class="sr-only peer">
            <div class="w-12 h-6 bg-gray-300 rounded-full peer peer-checked:after:translate-x-[26px] peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-[20px] after:w-[20px] after:transition-all peer-checked:bg-[#005EA3]"></div>
          </label>
        </div>

        <div v-if="diaryConfig.isEnabled" class="bg-[#F9FAFB] border border-gray-200 rounded-lg p-5">
          <div class="grid grid-cols-2 gap-8 mb-8">
            <div>
              <label class="block text-[13px] font-bold mb-2 text-gray-700">Số nhật ký tối thiểu/tuần <span class="text-red-500">*</span></label>
              <input type="number" min="1" max="7" v-model.number="diaryConfig.minPerWeek" class="w-full border border-gray-300 text-gray-800 rounded px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white" />
              <p class="text-[12px] text-gray-500 mt-1">Tối thiểu 1 nhật ký/tuần.</p>
            </div>
            <div>
              <label class="block text-[13px] font-bold mb-2 text-gray-700">Deadline chốt sổ mỗi tuần <span class="text-red-500">*</span></label>
              <div class="flex gap-2">
                <select v-model="diaryConfig.deadlineDay" class="w-1/2 border border-gray-300 text-gray-800 rounded px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white">
                  <option v-for="opt in dayOptions" :key="opt.value" :value="opt.value">{{ opt.label }}</option>
                </select>
                <input type="time" v-model="diaryConfig.deadlineTime" class="w-1/2 border border-gray-300 text-gray-800 rounded px-3 py-2.5 outline-none focus:border-[#005EA3] bg-white" />
              </div>
            </div>
          </div>

          <h3 class="font-bold text-[14px] text-gray-800 mb-4 border-b border-gray-200 pb-2">Trường hiển thị trong form nhật ký</h3>
          <div class="space-y-4">
            <div v-for="field in diaryConfig.fields" :key="field.id" class="flex items-center justify-between bg-white border border-gray-200 rounded p-3">
              <div class="flex items-center gap-3">
                <input type="checkbox" v-model="field.isEnabled" :id="field.id" class="w-4 h-4 text-[#005EA3] bg-gray-100 border-gray-300 rounded focus:ring-[#005EA3]" />
                <label :for="field.id" class="text-[14px] text-gray-700 cursor-pointer">{{ field.label }}</label>
              </div>
              <div class="flex items-center gap-2" v-if="field.isEnabled">
                <span class="text-[12px] text-gray-500">Bắt buộc điền:</span>
                <label class="relative inline-flex items-center cursor-pointer">
                  <input type="checkbox" v-model="field.isRequired" class="sr-only peer">
                  <div class="w-9 h-5 bg-gray-300 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-[16px] after:w-[16px] after:transition-all peer-checked:bg-red-500"></div>
                </label>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Toast Notification -->
    <div v-if="showToast" class="fixed top-24 right-8 z-50 flex items-center p-4 mb-4 text-gray-500 bg-white rounded-lg shadow-lg border border-gray-100" role="alert">
      <div :class="['inline-flex items-center justify-center shrink-0 w-8 h-8 rounded-lg', toastType === 'success' ? 'text-green-500 bg-green-100' : 'text-red-500 bg-red-100']">
        <span class="material-symbols-outlined text-[20px]">{{ toastType === 'success' ? 'check_circle' : 'error' }}</span>
      </div>
      <div class="ml-3 text-sm font-normal text-gray-800">{{ toastMessage }}</div>
    </div>

    <!-- Footer -->
    <div class="fixed bottom-0 left-0 right-0 lg:left-60 bg-white border-t border-gray-200 p-4 px-8 flex items-center justify-between z-10 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)]">
      <div class="flex items-center gap-2 text-[13px] text-gray-600">
        <span class="material-symbols-outlined text-[18px]">info</span>
        Mọi thay đổi cần được lưu để có hiệu lực toàn hệ thống.
      </div>
      <div class="flex items-center gap-3">
        <button @click="cancelChanges" class="border border-gray-300 text-gray-700 font-bold px-6 py-2 rounded hover:bg-gray-50 transition text-[13px]">
          Hủy thay đổi
        </button>
        <button @click="saveConfig" :disabled="isSaving" class="bg-[#005EA3] text-white font-bold px-6 py-2 rounded hover:bg-blue-800 transition flex items-center gap-2 text-[13px] disabled:opacity-70 disabled:cursor-not-allowed">
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
const classId = route.params.id || '101'; // Default for testing if not provided

const isStageOpen = ref(true);
const isDiaryOpen = ref(true);

const isSaving = ref(false);
const showToast = ref(false);
const toastMessage = ref('');
const toastType = ref('success'); // 'success' or 'error'

const diaryConfig = ref({
  isEnabled: true,
  minPerWeek: 2,
  deadlineDay: 0,
  deadlineTime: '23:59',
  fields: []
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
  { id: 'proofDescription', label: 'Mô tả minh chứng', isEnabled: true, isRequired: false }
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

onMounted(async () => {
  try {
    const res = await api.get(`/giangvien/classes/${classId}/diary-config`);
    if (res.data) {
      diaryConfig.value = { ...res.data };
    } else {
      // Fallback to defaults
      diaryConfig.value.fields = JSON.parse(JSON.stringify(defaultFields));
    }
  } catch (error) {
    console.error('Lỗi khi tải cấu hình nhật ký', error);
    diaryConfig.value.fields = JSON.parse(JSON.stringify(defaultFields));
  }
});

const validateConfig = () => {
  if (diaryConfig.value.isEnabled) {
    if (!diaryConfig.value.minPerWeek || diaryConfig.value.minPerWeek < 1) {
      diaryConfig.value.minPerWeek = 1;
      displayToast('Đã tự động điều chỉnh: Tối thiểu 1 nhật ký/tuần', 'error');
      return false;
    }
    const hasEnabledField = diaryConfig.value.fields.some(f => f.isEnabled);
    if (!hasEnabledField) {
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
    await api.put(`/giangvien/classes/${classId}/diary-config`, diaryConfig.value);
    displayToast('Lưu cấu hình lớp thực tập thành công', 'success');
  } catch (error) {
    displayToast('Lưu cấu hình lớp thực tập thất bại, vui lòng thử lại sau!', 'error');
  } finally {
    isSaving.value = false;
  }
};

const cancelChanges = async () => {
  // Re-fetch to reset
  isSaving.value = true;
  try {
    const res = await api.get(`/giangvien/classes/${classId}/diary-config`);
    if (res.data) {
      diaryConfig.value = { ...res.data };
    }
    displayToast('Đã khôi phục cài đặt trước đó', 'success');
  } catch (error) {
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
