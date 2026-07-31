<template>
  <div class="ims-scope w-full mx-auto space-y-6 font-sans pb-20">
    <!-- Breadcrumb & Header -->
    <div>
      <nav class="flex items-center gap-2 text-xs text-slate-500 mb-2">
        <router-link to="/teacher/classes" class="hover:text-blue-700 transition-colors">Lá»›p cá»§a tÃ´i</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-700 font-medium">ÄÃ¡nh giÃ¡</span>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="font-bold text-slate-900">Äiá»ƒm danh</span>
      </nav>
      <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <h1 class="text-[28px] font-bold text-slate-900 tracking-tight">Quáº£n lÃ½ Äiá»ƒm danh Buá»•i há»p</h1>
      </div>
    </div>

    <!-- Sub Navigation Tabs -->
    <div class="flex items-center gap-8 border-b border-slate-200">
      <router-link
        :to="`/teacher/classes/${classId}/grading`"
        class="pb-3 text-slate-500 hover:text-blue-700 font-semibold text-sm transition-colors"
      >
        Sá»• Ä‘iá»ƒm tá»•ng há»£p
      </router-link>
      <router-link
        :to="`/teacher/classes/${classId}/attendance`"
        class="pb-3 text-blue-700 border-b-2 border-blue-700 font-bold text-sm"
      >
        Äiá»ƒm danh buá»•i há»p
      </router-link>
      <router-link
        :to="`/teacher/classes/${classId}/violations`"
        class="pb-3 text-slate-500 hover:text-blue-700 font-semibold text-sm transition-colors flex items-center gap-1.5"
      >
        Theo dÃµi vi pháº¡m
        <span class="bg-rose-100 text-rose-800 text-[10px] px-1.5 py-0.2 rounded-full font-bold">4</span>
      </router-link>
    </div>

    <!-- Session Select Bar -->
    <div class="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs flex flex-col md:flex-row justify-between items-stretch md:items-center gap-4">
      <div class="flex items-center gap-3">
        <label class="text-xs font-bold text-slate-700 whitespace-nowrap">Chá»n buá»•i há»p:</label>
        <select v-model="selectedMeeting" class="px-3.5 py-2 rounded-xl border border-slate-300 text-xs font-bold text-slate-800 outline-none focus:ring-2 focus:ring-blue-600 min-w-[280px]">
          <option value="1">Buá»•i há»p hÆ°á»›ng dáº«n #04 - 20/10/2024</option>
          <option value="2">Buá»•i há»p hÆ°á»›ng dáº«n #03 - 13/10/2024</option>
          <option value="3">Buá»•i há»p Ä‘á»™t xuáº¥t - 05/10/2024</option>
        </select>
        <span class="text-xs text-slate-500 font-medium hidden sm:inline">Tá»• chá»©c: 5 buá»•i</span>
      </div>

      <button
        @click="showCreateMeetingModal = true"
        class="px-4 py-2 border border-blue-600 text-blue-700 hover:bg-blue-50 font-bold text-xs rounded-xl transition flex items-center justify-center gap-2 active:scale-95"
      >
        <span class="material-symbols-outlined text-[18px]">add</span>
        Táº¡o buá»•i há»p Ä‘á»™t xuáº¥t
      </button>
    </div>

    <!-- Student Attendance Table -->
    <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-2xs">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse text-xs">
          <thead>
            <tr class="bg-slate-50 border-b border-slate-200 font-bold uppercase text-[10px] text-slate-500">
              <th class="px-6 py-3.5">Há» vÃ  tÃªn</th>
              <th class="px-6 py-3.5">MSSV</th>
              <th class="px-6 py-3.5">Tráº¡ng thÃ¡i thá»±c táº­p</th>
              <th class="px-6 py-3.5 text-center">Váº¯ng (P / KP)</th>
              <th class="px-6 py-3.5">Äiá»ƒm danh buá»•i há»p</th>
              <th class="px-6 py-3.5">Ghi chÃº váº¯ng / LÃ½ do</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100">
            <tr
              v-for="st in students"
              :key="st.id"
              class="hover:bg-slate-50/80 transition"
              :class="{ 'opacity-50 bg-slate-50': st.status === 'Dá»«ng thá»±c táº­p' }"
            >
              <td class="px-6 py-4">
                <div class="flex items-center gap-2">
                  <span class="font-bold text-slate-900 text-sm">{{ st.name }}</span>
                  <button
                    @click="openQuickNote(st)"
                    class="p-1 text-slate-400 hover:text-blue-700 transition"
                    title="Ghi chÃº nhanh"
                  >
                    <span class="material-symbols-outlined text-[16px]">edit_square</span>
                  </button>
                </div>
              </td>
              <td class="px-6 py-4 font-mono font-medium text-slate-600">
                {{ st.mssv }}
              </td>
              <td class="px-6 py-4">
                <span
                  class="px-2.5 py-0.5 rounded-full text-[10px] font-bold"
                  :class="st.status === 'Äang thá»±c táº­p' ? 'bg-emerald-100 text-emerald-800' : 'bg-slate-200 text-slate-700'"
                >
                  {{ st.status }}
                </span>
              </td>
              <td class="px-6 py-4 text-center font-bold text-slate-700">
                {{ st.absentPermitted }} / <span class="text-rose-600">{{ st.absentUnpermitted }}</span>
              </td>
              <td class="px-6 py-4">
                <div class="flex items-center gap-3">
                  <label class="flex items-center gap-1.5 cursor-pointer">
                    <input
                      type="radio"
                      :name="`attendance-${st.id}`"
                      value="present"
                      v-model="st.attendance"
                      :disabled="st.status === 'Dá»«ng thá»±c táº­p'"
                      class="text-emerald-600 focus:ring-emerald-500"
                    />
                    <span class="font-bold text-emerald-700">CÃ³ máº·t</span>
                  </label>
                  <label class="flex items-center gap-1.5 cursor-pointer">
                    <input
                      type="radio"
                      :name="`attendance-${st.id}`"
                      value="excused"
                      v-model="st.attendance"
                      :disabled="st.status === 'Dá»«ng thá»±c táº­p'"
                      class="text-amber-600 focus:ring-amber-500"
                    />
                    <span class="font-bold text-amber-700">CÃ³ phÃ©p</span>
                  </label>
                  <label class="flex items-center gap-1.5 cursor-pointer">
                    <input
                      type="radio"
                      :name="`attendance-${st.id}`"
                      value="unexcused"
                      v-model="st.attendance"
                      :disabled="st.status === 'Dá»«ng thá»±c táº­p'"
                      class="text-rose-600 focus:ring-rose-500"
                    />
                    <span class="font-bold text-rose-700">KhÃ´ng phÃ©p</span>
                  </label>
                </div>
              </td>
              <td class="px-6 py-4">
                <input
                  v-model="st.reason"
                  type="text"
                  placeholder="ThÃªm lÃ½ do váº¯ng..."
                  :disabled="st.status === 'Dá»«ng thá»±c táº­p'"
                  class="w-full px-3 py-1.5 bg-slate-50 border border-slate-200 rounded-xl text-xs outline-none focus:ring-2 focus:ring-blue-600"
                />
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Floating Footer Bar -->
    <div class="fixed bottom-0 left-0 right-0 z-50 bg-white border-t border-slate-200 p-4 shadow-lg flex justify-between items-center px-8">
      <span class="text-xs text-slate-500 italic">Tá»± Ä‘á»™ng lÆ°u nhÃ¡p: 10:45:22</span>
      <button
        @click="saveAttendance"
        class="px-8 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl text-xs shadow-md transition active:scale-95"
      >
        LÆ°u Ä‘iá»ƒm danh
      </button>
    </div>

    <!-- Modal Táº¡o buá»•i há»p Ä‘á»™t xuáº¥t -->
    <div
      v-if="showCreateMeetingModal"
      class="fixed inset-0 z-[100] flex items-center justify-center bg-slate-900/50 backdrop-blur-xs p-4"
      @click.self="showCreateMeetingModal = false"
    >
      <div class="bg-white rounded-2xl shadow-2xl max-w-md w-full p-6 space-y-4 border border-slate-200">
        <div class="flex justify-between items-center border-b pb-3">
          <h3 class="font-bold text-base text-slate-900">Táº¡o buá»•i há»p Ä‘á»™t xuáº¥t</h3>
          <button @click="showCreateMeetingModal = false" class="text-slate-400 hover:text-slate-600">âœ•</button>
        </div>

        <div class="space-y-3 text-xs">
          <div>
            <label class="block font-bold text-slate-700 mb-1">TÃªn buá»•i há»p</label>
            <input
              v-model="newMeetingTitle"
              type="text"
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-semibold"
            />
          </div>
          <div>
            <label class="block font-bold text-slate-700 mb-1">Link phÃ²ng há»p (Google Meet / Teams)</label>
            <input
              v-model="newMeetingLink"
              type="text"
              placeholder="https://meet.google.com/..."
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
            />
          </div>
        </div>

        <div class="flex justify-end gap-3 pt-3 border-t">
          <button @click="showCreateMeetingModal = false" class="px-4 py-2 text-slate-600 font-bold hover:bg-slate-100 rounded-xl">Há»§y</button>
          <button @click="createMeeting" class="px-6 py-2 bg-[#005EA3] text-white font-bold rounded-xl shadow-md">Táº¡o ngay</button>
        </div>
      </div>
    </div>

    <!-- Toast Notification -->
    <div
      v-if="toastMsg"
      class="fixed bottom-20 right-6 z-[120] bg-slate-900 text-white px-4 py-3 rounded-xl shadow-xl text-xs font-bold flex items-center gap-2"
    >
      <span class="material-symbols-outlined text-emerald-400 text-[18px]">check_circle</span>
      {{ toastMsg }}
    </div>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted } from 'vue';
  import { useRoute } from 'vue-router';
  import api from '@/api/api';

  const route = useRoute();
  const classId = computed(() => route.params.id || '1');

  const selectedMeeting = ref('1');
  const showCreateMeetingModal = ref(false);
  const newMeetingTitle = ref('Há»p Ä‘á»™t xuáº¥t - 27/07/2026');
  const newMeetingLink = ref('');
  const toastMsg = ref('');

  const students = ref([]);
  const loading = ref(true);

  onMounted(async () => {
    try {
      const res = await api.get(`/giangvien/classes/${classId.value}/attendance`);
      students.value = res.data.map(item => ({
        id: item.maSinhVien,
        name: item.hoTen,
        mssv: item.maSoSinhVien,
        status: item.trangThaiThucTap === 'DangThucTap' ? 'Äang thá»±c táº­p' : 'Dá»«ng thá»±c táº­p',
        absentPermitted: item.absentPermitted || 0,
        absentUnpermitted: item.absentUnpermitted || 0,
        attendance: item.attendance || 'present',
        reason: item.reason || ''
      }));
    } catch (err) {
      console.error(err);
      showToast('KhÃ´ng thá»ƒ táº£i dá»¯ liá»‡u Ä‘iá»ƒm danh');
    } finally {
      loading.value = false;
    }
  });

  function openQuickNote(st) {
    showToast(`Má»Ÿ ghi chÃº nhanh cho ${st.name}`);
  }

  async function saveAttendance() {
    try {
      const records = students.value.map(s => ({
        maSinhVien: s.id,
        attendance: s.attendance,
        reason: s.reason
      }));
      await api.post(`/giangvien/classes/${classId.value}/attendance`, { records });
      showToast('LÆ°u dá»¯ liá»‡u Ä‘iá»ƒm danh thÃ nh cÃ´ng!');
    } catch (err) {
      showToast('CÃ³ lá»—i xáº£y ra khi lÆ°u Ä‘iá»ƒm danh!');
    }
  }

  function createMeeting() {
    showCreateMeetingModal.value = false;
    showToast(`ÄÃ£ táº¡o "${newMeetingTitle.value}" thÃ nh cÃ´ng!`);
  }

  function showToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }
</script>
