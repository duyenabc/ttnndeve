<template>
  <div class="ims-scope w-full mx-auto space-y-6 font-sans pb-20">
    <!-- Top Header -->
    <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs flex flex-col md:flex-row md:items-center justify-between gap-4">
      <div>
        <h1 class="font-bold text-2xl text-slate-900 tracking-tight flex items-center gap-2">
          <span class="material-symbols-outlined text-blue-600 text-[28px]">calendar_month</span>
          Lá»‹ch hÆ°á»›ng dáº«n & Cuá»™c há»p
        </h1>
        <p class="text-xs text-slate-500 mt-1">LÃªn lá»‹ch há»p Ä‘á»‹nh ká»³, theo dÃµi má»‘c tiáº¿n Ä‘á»™ vÃ  deadline vá»›i sinh viÃªn hÆ°á»›ng dáº«n.</p>
      </div>

      <button
        @click="openCreateDrawer"
        class="px-5 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white rounded-xl font-bold text-xs shadow-md transition-all active:scale-95 flex items-center gap-2"
      >
        <span class="material-symbols-outlined text-[20px]">add</span>
        Táº¡o lá»‹ch há»p
      </button>
    </div>

    <!-- Main Schedule Layout -->
    <div class="grid grid-cols-1 lg:grid-cols-12 gap-6 items-start">
      <!-- Left Control & Filters Sidebar -->
      <div class="lg:col-span-3 bg-white p-5 rounded-2xl border border-slate-200 shadow-2xs space-y-6">
        <div>
          <h3 class="text-xs font-bold text-slate-400 uppercase tracking-wider mb-3">PhÃ¢n loáº¡i lá»‹ch</h3>
          <div class="space-y-2.5 text-xs">
            <label class="flex items-center justify-between cursor-pointer p-2 rounded-xl hover:bg-slate-50 transition">
              <div class="flex items-center gap-2">
                <input type="checkbox" v-model="layers.meeting" class="rounded border-slate-300 text-blue-600 focus:ring-blue-600" />
                <span class="font-bold text-slate-800">Lá»‹ch há»p Ä‘á»‹nh ká»³</span>
              </div>
              <span class="w-3 h-3 rounded-full bg-emerald-500"></span>
            </label>

            <label class="flex items-center justify-between cursor-pointer p-2 rounded-xl hover:bg-slate-50 transition">
              <div class="flex items-center gap-2">
                <input type="checkbox" v-model="layers.deadline" class="rounded border-slate-300 text-rose-600 focus:ring-rose-600" />
                <span class="font-bold text-slate-800">Deadline ná»™p bÃ i</span>
              </div>
              <span class="w-3 h-3 rounded-full bg-rose-500"></span>
            </label>

            <label class="flex items-center justify-between cursor-pointer p-2 rounded-xl hover:bg-slate-50 transition">
              <div class="flex items-center gap-2">
                <input type="checkbox" v-model="layers.admin" class="rounded border-slate-300 text-amber-600 focus:ring-amber-600" />
                <span class="font-bold text-slate-800">Má»‘c ká»³ GiÃ¡o vá»¥</span>
              </div>
              <span class="w-3 h-3 rounded-full bg-amber-400"></span>
            </label>
          </div>
        </div>

        <div>
          <h3 class="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2">Lá»c theo lá»›p</h3>
          <select v-model="selectedClassFilter" class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-bold text-slate-800 outline-none focus:ring-2 focus:ring-blue-600">
            <option value="ALL">Táº¥t cáº£ cÃ¡c lá»›p</option>
            <option v-for="c in classOptions" :key="c.maLop" :value="c.maLop">{{ c.tenLop }}</option>
          </select>
        </div>
      </div>

      <!-- Right Calendar Container -->
      <div class="lg:col-span-9 bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-2xs">
        <!-- Calendar Header -->
        <div class="p-5 border-b border-slate-200 flex flex-col sm:flex-row sm:items-center justify-between gap-4 bg-slate-50/50">
          <div class="flex items-center gap-4">
            <h2 class="font-bold text-lg text-slate-900">{{ currentViewTitle }}</h2>
            <div class="flex bg-white rounded-xl border border-slate-200 p-0.5 shadow-2xs">
              <button @click="navigateCalendar(-1)" class="p-1 text-slate-500 hover:text-slate-900 rounded-lg">
                <span class="material-symbols-outlined text-[18px]">chevron_left</span>
              </button>
              <button @click="resetToCurrentDate" class="px-3 py-1 text-xs font-bold text-slate-700 hover:bg-slate-100 rounded-lg">
                Hiá»‡n táº¡i
              </button>
              <button @click="navigateCalendar(1)" class="p-1 text-slate-500 hover:text-slate-900 rounded-lg">
                <span class="material-symbols-outlined text-[18px]">chevron_right</span>
              </button>
            </div>
          </div>

          <!-- Month / Week Toggle -->
          <div class="flex bg-slate-200/80 p-1 rounded-xl w-fit text-xs font-bold">
            <button
              @click="currentViewMode = 'month'"
              class="px-4 py-1.5 rounded-lg transition"
              :class="currentViewMode === 'month' ? 'bg-white text-blue-700 shadow-2xs' : 'text-slate-600 hover:text-slate-900'"
            >
              ThÃ¡ng
            </button>
            <button
              @click="currentViewMode = 'week'"
              class="px-4 py-1.5 rounded-lg transition"
              :class="currentViewMode === 'week' ? 'bg-white text-blue-700 shadow-2xs' : 'text-slate-600 hover:text-slate-900'"
            >
              Tuáº§n
            </button>
          </div>
        </div>

        <!-- Month View -->
        <div v-if="currentViewMode === 'month'" class="p-4">
          <div class="grid grid-cols-7 text-center font-bold text-[11px] text-slate-400 uppercase tracking-wider mb-2">
            <div>Chá»§ Nháº­t</div>
            <div>Thá»© Hai</div>
            <div>Thá»© Ba</div>
            <div>Thá»© TÆ°</div>
            <div>Thá»© NÄƒm</div>
            <div>Thá»© SÃ¡u</div>
            <div>Thá»© Báº£y</div>
          </div>

          <div class="grid grid-cols-7 border-t border-l border-slate-200 text-xs">
            <div
              v-for="(day, idx) in monthDays"
              :key="idx"
              class="border-r border-b border-slate-200 min-h-[110px] p-2 flex flex-col justify-between hover:bg-slate-50/60 transition"
              :class="day.isOtherMonth ? 'bg-slate-50/40 text-slate-400' : 'bg-white'"
            >
              <div class="flex justify-between items-center">
                <span
                  class="font-bold text-[11px] w-6 h-6 flex items-center justify-center rounded-full"
                  :class="day.isToday ? 'bg-blue-600 text-white' : 'text-slate-700'"
                >
                  {{ day.dayNumber }}
                </span>
              </div>

              <div class="space-y-1 mt-1.5 flex-1">
                <div
                  v-for="evt in getEventsForDay(day.dateStr)"
                  :key="evt.id"
                  @click.stop="openEventDetail(evt)"
                  class="p-1.5 rounded-lg text-[10px] font-bold border-l-3 truncate cursor-pointer hover:opacity-90 transition shadow-2xs"
                  :class="getEventColorClass(evt.type)"
                >
                  {{ evt.time ? evt.time + ' - ' : '' }}{{ evt.tieuDe }}
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Week View -->
        <div v-else class="p-4 overflow-x-auto">
          <div class="grid grid-cols-8 text-center text-xs font-bold border-b border-slate-200 pb-2 mb-2">
            <div class="text-slate-400 text-[10px] uppercase">Giá»</div>
            <div v-for="wDay in weekDays" :key="wDay.dateStr" class="p-1">
              <span class="text-slate-400 text-[10px] block uppercase">{{ wDay.dayName }}</span>
              <span class="text-sm font-extrabold" :class="wDay.isToday ? 'text-blue-600' : 'text-slate-800'">
                {{ wDay.dayNumber }}
              </span>
            </div>
          </div>

          <div class="divide-y divide-slate-100 text-xs">
            <div v-for="hour in [8, 10, 14, 16]" :key="hour" class="grid grid-cols-8 min-h-[70px] items-start">
              <div class="text-[11px] text-slate-400 font-mono pt-1 text-center font-bold">{{ hour }}:00</div>
              <div
                v-for="wDay in weekDays"
                :key="wDay.dateStr"
                class="border-l border-slate-100 p-1 min-h-[70px]"
              >
                <div
                  v-for="evt in getEventsForDayAndHour(wDay.dateStr, hour)"
                  :key="evt.id"
                  @click.stop="openEventDetail(evt)"
                  class="p-2 rounded-xl text-[11px] font-bold border-l-3 cursor-pointer shadow-2xs hover:scale-98 transition mb-1"
                  :class="getEventColorClass(evt.type)"
                >
                  <p class="truncate">{{ evt.tieuDe }}</p>
                  <p class="text-[9px] font-normal opacity-80 truncate">{{ evt.tenSinhVien || evt.location }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Create Meeting Drawer -->
    <div
      v-if="showCreateDrawer"
      class="fixed inset-0 z-[120] flex justify-end bg-slate-900/50 backdrop-blur-xs"
      @click.self="showCreateDrawer = false"
    >
      <div class="w-full max-w-md bg-white h-full shadow-2xl flex flex-col justify-between p-6 overflow-y-auto font-sans">
        <div class="space-y-5">
          <div class="flex justify-between items-center border-b pb-3">
            <h3 class="font-bold text-slate-900 text-base flex items-center gap-2">
              <span class="material-symbols-outlined text-blue-600">video_call</span>
              Táº¡o lá»‹ch há»p hÆ°á»›ng dáº«n
            </h3>
            <button @click="showCreateDrawer = false" class="text-slate-400 hover:text-slate-600">âœ•</button>
          </div>

          <div class="space-y-4 text-xs">
            <div>
              <label class="block font-bold text-slate-700 mb-1">TÃªn buá»•i há»p (*)</label>
              <input
                v-model="createForm.tieuDe"
                type="text"
                placeholder="VD: [K64] Há»p nhóm Äá»“ Ã¡n 1"
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
              />
            </div>

            <div>
              <label class="block font-bold text-slate-700 mb-1">Sinh viÃªn / NhÃ³m hÆ°á»›ng dáº«n (*)</label>
              <select
                v-model="createForm.sinhVienId"
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium"
              >
                <option v-for="s in studentOptions" :key="s.maSinhVien" :value="s.maSinhVien">
                  {{ s.hoTen }} ({{ s.maSoSinhVien }})
                </option>
              </select>
            </div>

            <div class="grid grid-cols-2 gap-3">
              <div>
                <label class="block font-bold text-slate-700 mb-1">NgÃ y báº¯t Ä‘áº§u (*)</label>
                <input
                  v-model="createForm.startDate"
                  type="date"
                  class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-mono"
                />
              </div>
              <div>
                <label class="block font-bold text-slate-700 mb-1">Giá» báº¯t Ä‘áº§u (*)</label>
                <input
                  v-model="createForm.startTime"
                  type="time"
                  class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-mono"
                />
              </div>
            </div>

            <!-- Recurrence Config Block -->
            <div class="p-4 bg-slate-50 border border-slate-200 rounded-2xl space-y-3">
              <div>
                <label class="block font-bold text-slate-800 mb-1">Láº·p láº¡i cuá»™c há»p</label>
                <select
                  v-model="createForm.recurrence"
                  class="w-full px-3 py-2 bg-white border border-slate-300 rounded-xl font-semibold outline-none focus:ring-2 focus:ring-blue-600"
                >
                  <option value="none">KhÃ´ng láº·p láº¡i</option>
                  <option value="weekly">HÃ ng tuáº§n</option>
                  <option value="monthly">HÃ ng thÃ¡ng</option>
                </select>
              </div>

              <div v-if="createForm.recurrence === 'weekly'" class="space-y-2 pt-1 border-t border-slate-200">
                <p class="font-bold text-slate-700 text-[11px]">Chá»n thá»© trong tuáº§n (*)</p>
                <div class="flex flex-wrap gap-1.5">
                  <button
                    v-for="day in ['T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'CN']"
                    :key="day"
                    type="button"
                    @click="toggleWeeklyDay(day)"
                    class="w-8 h-8 rounded-full border text-xs font-bold transition flex items-center justify-center"
                    :class="selectedWeeklyDays.includes(day) ? 'bg-blue-600 text-white border-blue-600' : 'bg-white text-slate-700 border-slate-300'"
                  >
                    {{ day }}
                  </button>
                </div>
              </div>
            </div>

            <div>
              <label class="block font-bold text-slate-700 mb-1">Link phÃ²ng há»p trá»±c tuyáº¿n</label>
              <input
                v-model="createForm.linkMeeting"
                type="text"
                placeholder="https://meet.google.com/..."
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
              />
            </div>

            <div>
              <label class="block font-bold text-slate-700 mb-1">Ghi chÃº ná»™i dung</label>
              <textarea
                v-model="createForm.noiDung"
                rows="3"
                placeholder="Ná»™i dung trao đổi hoáº·c yêu cầu chuẩn bị..."
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
              ></textarea>
            </div>
          </div>
        </div>

        <div class="pt-4 border-t flex justify-end gap-3 text-xs">
          <button @click="showCreateDrawer = false" class="px-4 py-2 border border-slate-300 rounded-xl font-semibold text-slate-700 hover:bg-slate-100">
            Há»§y
          </button>
          <button @click="submitCreateMeeting" class="px-5 py-2 bg-blue-700 hover:bg-blue-800 text-white font-bold rounded-xl shadow-xs">
            Táº¡o lá»‹ch há»p
          </button>
        </div>
      </div>
    </div>

    <!-- Event Detail Drawer -->
    <div
      v-if="activeEvent"
      class="fixed inset-0 z-[120] flex justify-end bg-slate-900/50 backdrop-blur-xs"
      @click.self="activeEvent = null"
    >
      <div class="w-full max-w-sm bg-white h-full shadow-2xl flex flex-col justify-between p-6 text-xs">
        <div class="space-y-4">
          <div class="flex justify-between items-center border-b pb-3">
            <span class="px-2.5 py-0.5 rounded-full font-bold text-[10px]" :class="getEventColorClass(activeEvent.type)">
              {{ activeEvent.type === 'meeting' ? 'Lá»‹ch há»p' : activeEvent.type === 'deadline' ? 'Deadline' : 'GiÃ¡o vá»¥' }}
            </span>
            <button @click="activeEvent = null" class="text-slate-400 hover:text-slate-600">âœ•</button>
          </div>

          <h3 class="font-extrabold text-base text-slate-900 leading-snug">{{ activeEvent.tieuDe }}</h3>

          <div class="space-y-3 bg-slate-50 p-3.5 rounded-2xl border border-slate-200 text-slate-700">
            <div class="flex items-center gap-2">
              <span class="material-symbols-outlined text-slate-400 text-[18px]">schedule</span>
              <div>
                <p class="font-bold">{{ activeEvent.dateStr }}</p>
                <p class="text-[11px] text-slate-500">{{ activeEvent.time || 'Cáº£ ngÃ y' }}</p>
              </div>
            </div>

            <div v-if="activeEvent.linkMeeting" class="flex items-center gap-2">
              <span class="material-symbols-outlined text-blue-600 text-[18px]">videocam</span>
              <a :href="activeEvent.linkMeeting" target="_blank" class="text-blue-700 font-bold hover:underline truncate">
                Tham gia Google Meet
              </a>
            </div>

            <div v-if="activeEvent.tenSinhVien" class="flex items-center gap-2">
              <span class="material-symbols-outlined text-slate-400 text-[18px]">person</span>
              <span class="font-bold text-slate-900">{{ activeEvent.tenSinhVien }}</span>
            </div>
          </div>

          <div v-if="activeEvent.noiDung" class="space-y-1">
            <p class="font-bold text-slate-800">Ná»™i dung ghi chÃº:</p>
            <p class="text-slate-600 bg-slate-50 p-3 rounded-xl border leading-relaxed">{{ activeEvent.noiDung }}</p>
          </div>
        </div>

        <div class="pt-4 border-t flex justify-end gap-2">
          <button @click="cancelEvent(activeEvent)" class="px-4 py-2 bg-rose-50 hover:bg-rose-100 text-rose-700 font-bold rounded-xl border border-rose-200">
            Há»§y buá»•i há»p
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
  import { ref, computed, onMounted } from 'vue';
  import api from '@/api/api';

  const currentViewMode = ref('month');
  const currentDate = ref(new Date());
  const layers = ref({ meeting: true, deadline: true, admin: true });
  const selectedClassFilter = ref('ALL');
  const classOptions = ref([]);

  const showCreateDrawer = ref(false);
  const activeEvent = ref(null);
  const toastMsg = ref('');
  const studentOptions = ref([]);

  const selectedWeeklyDays = ref(['T2', 'T4']);

  const createForm = ref({
    tieuDe: '',
    sinhVienId: null,
    startDate: '',
    startTime: '09:00',
    recurrence: 'none',
    linkMeeting: '',
    noiDung: ''
  });

  const scheduleEvents = ref([
    {
      id: 101,
      tieuDe: '[K64] 09:00 - Há»p nhóm Äá»“ Ã¡n 1',
      dateStr: '2026-07-28',
      time: '09:00',
      hour: 9,
      type: 'meeting',
      tenSinhVien: 'Nguyá»…n VÄƒn An',
      linkMeeting: 'https://meet.google.com/abc-defg-hij',
      noiDung: 'Kiá»ƒm tra sÆ¡ Ä‘á»“ kiáº¿n trÃºc há»‡ thá»‘ng vÃ  phÃ¢n cÃ´ng cÃ´ng viá»‡c.'
    },
    {
      id: 102,
      tieuDe: '[K64] Deadline: BÃ¡o cÃ¡o tiáº¿n Ä‘á»™ tuáº§n 4',
      dateStr: '2026-07-30',
      time: '23:59',
      hour: 16,
      type: 'deadline',
      noiDung: 'Ná»™p bÃ¡o cÃ¡o qua há»‡ thá»‘ng IMS.'
    },
    {
      id: 103,
      tieuDe: '[K64] Má»‘c: ÄÄƒng kÃ½ báº£o vá»‡ khÃ³a luáº­n',
      dateStr: '2026-08-05',
      time: '14:00',
      hour: 14,
      type: 'admin',
      noiDung: 'Má»‘c thá»i gian tá»•ng há»£p tá»« GiÃ¡o vá»¥ khoa.'
    }
  ]);

  const currentViewTitle = computed(() => {
    const month = currentDate.value.getMonth() + 1;
    const year = currentDate.value.getFullYear();
    return `ThÃ¡ng ${month}, ${year}`;
  });

  const monthDays = computed(() => {
    const year = currentDate.value.getFullYear();
    const month = currentDate.value.getMonth();
    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);

    const startDayOfWeek = firstDay.getDay(); // 0 is Sunday
    const totalDays = lastDay.getDate();

    const days = [];
    const todayStr = new Date().toISOString().split('T')[0];

    // Previous month padding
    const prevMonthLastDay = new Date(year, month, 0).getDate();
    for (let i = startDayOfWeek - 1; i >= 0; i--) {
      const pDay = prevMonthLastDay - i;
      const dateStr = `${year}-${String(month).padStart(2, '0')}-${String(pDay).padStart(2, '0')}`;
      days.push({ dayNumber: pDay, isOtherMonth: true, dateStr });
    }

    // Current month days
    for (let i = 1; i <= totalDays; i++) {
      const dateStr = `${year}-${String(month + 1).padStart(2, '0')}-${String(i).padStart(2, '0')}`;
      days.push({ dayNumber: i, isOtherMonth: false, isToday: dateStr === todayStr, dateStr });
    }

    return days;
  });

  const weekDays = computed(() => {
    const curr = new Date(currentDate.value);
    const first = curr.getDate() - curr.getDay(); // Sunday
    const days = [];
    const todayStr = new Date().toISOString().split('T')[0];
    const names = ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'];

    for (let i = 0; i < 7; i++) {
      const next = new Date(curr.setDate(first + i));
      const dateStr = next.toISOString().split('T')[0];
      days.push({
        dayName: names[i],
        dayNumber: next.getDate(),
        dateStr,
        isToday: dateStr === todayStr
      });
    }
    return days;
  });

  function getEventsForDay(dateStr) {
    return scheduleEvents.value.filter(e => {
      if (e.type === 'meeting' && !layers.value.meeting) return false;
      if (e.type === 'deadline' && !layers.value.deadline) return false;
      if (e.type === 'admin' && !layers.value.admin) return false;
      return e.dateStr === dateStr;
    });
  }

  function getEventsForDayAndHour(dateStr, hour) {
    return getEventsForDay(dateStr).filter(e => e.hour === hour || (hour === 8 && e.hour < 10));
  }

  function getEventColorClass(type) {
    switch (type) {
      case 'meeting':
        return 'bg-emerald-100 text-emerald-800 border-emerald-500';
      case 'deadline':
        return 'bg-rose-100 text-rose-800 border-rose-500';
      case 'admin':
        return 'bg-amber-100 text-amber-800 border-amber-500';
      default:
        return 'bg-blue-100 text-blue-800 border-blue-500';
    }
  }

  function navigateCalendar(dir) {
    const newDate = new Date(currentDate.value);
    if (currentViewMode.value === 'month') {
      newDate.setMonth(newDate.getMonth() + dir);
    } else {
      newDate.setDate(newDate.getDate() + dir * 7);
    }
    currentDate.value = newDate;
  }

  function resetToCurrentDate() {
    currentDate.value = new Date();
  }

  function openCreateDrawer() {
    createForm.value = {
      tieuDe: '',
      sinhVienId: studentOptions.value[0]?.maSinhVien || null,
      startDate: new Date().toISOString().split('T')[0],
      startTime: '09:00',
      recurrence: 'none',
      linkMeeting: 'https://meet.google.com/new',
      noiDung: ''
    };
    showCreateDrawer.value = true;
  }

  function toggleWeeklyDay(day) {
    if (selectedWeeklyDays.value.includes(day)) {
      selectedWeeklyDays.value = selectedWeeklyDays.value.filter(d => d !== day);
    } else {
      selectedWeeklyDays.value.push(day);
    }
  }

  function submitCreateMeeting() {
    if (!createForm.value.tieuDe.trim()) {
      showToast('Vui lÃ²ng nháº­p tÃªn buá»•i há»p!');
      return;
    }

    const st = studentOptions.value.find(s => s.maSinhVien === createForm.value.sinhVienId);

    const newEvt = {
      id: Date.now(),
      tieuDe: createForm.value.tieuDe,
      dateStr: createForm.value.startDate || new Date().toISOString().split('T')[0],
      time: createForm.value.startTime,
      hour: parseInt(createForm.value.startTime?.split(':')[0] || 9, 10),
      type: 'meeting',
      tenSinhVien: st?.hoTen || 'Sinh viÃªn',
      linkMeeting: createForm.value.linkMeeting,
      noiDung: createForm.value.noiDung
    };

    scheduleEvents.value.unshift(newEvt);
    showCreateDrawer.value = false;
    showToast('âœ“ Táº¡o lá»‹ch há»p hÆ°á»›ng dáº«n thÃ nh cÃ´ng!');
  }

  function openEventDetail(evt) {
    activeEvent.value = evt;
  }

  function cancelEvent(evt) {
    scheduleEvents.value = scheduleEvents.value.filter(e => e.id !== evt.id);
    activeEvent.value = null;
    showToast('âœ“ ÄÃ£ há»§y buá»•i há»p!');
  }

  function showToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }

  onMounted(async () => {
    try {
      const cls = await api.get('/giangvien/classes');
      classOptions.value = Array.isArray(cls.data) ? cls.data : (cls.data?.items || []);

      const stList = [];
      for (const c of classOptions.value) {
        const res = await api.get(`/giangvien/classes/${c.maLop}/students`);
        const list = Array.isArray(res.data) ? res.data : (res.data?.items || []);
        list.forEach(s => {
          if (!stList.some(x => x.maSinhVien === s.maSinhVien)) stList.push(s);
        });
      }
      studentOptions.value = stList;
    } catch {
      // Mock fallback
    }
  });
</script>
