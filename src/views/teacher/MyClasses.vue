<template>
  <div class="ims-scope w-full mx-auto space-y-8">
    <!-- Header -->
    <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
      <div>
        <h1 class="text-[32px] font-bold text-slate-900 tracking-tight mb-1">Danh sÃ¡ch lá»›p hÆ°á»›ng dáº«n</h1>
        <p class="text-sm text-slate-500">Quáº£n lÃ½ vÃ  theo dÃµi cÃ¡c lá»›p thá»±c táº­p do báº¡n phá»¥ trÃ¡ch.</p>
      </div>
      <button
        class="flex items-center gap-2 text-white px-4 py-2.5 rounded-xl font-bold text-sm hover:opacity-90 active:scale-95 transition-all shadow-sm w-fit"
        style="background-color: #005EA3;"
        @click="openCreate"
      >
        <span class="material-symbols-outlined text-[18px]">add</span>
        Táº¡o lá»›p má»›i
      </button>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="bg-white rounded-2xl border border-slate-200 p-12 text-center text-slate-500">
      Äang táº£i danh sÃ¡ch lá»›p...
    </div>

    <!-- Empty State -->
    <div
      v-else-if="!classes.length"
      class="bg-white rounded-2xl border border-slate-200 p-12 text-center text-slate-500"
    >
      <span class="material-symbols-outlined text-[48px] block mb-2 opacity-40">class</span>
      <p class="font-semibold text-base text-slate-800">Báº¡n chÆ°a cÃ³ lá»›p hÆ°á»›ng dáº«n nÃ o</p>
      <p class="text-xs text-slate-500 mt-1">Nháº¥n "Táº¡o lá»›p má»›i" Ä‘á»ƒ báº¯t Ä‘áº§u khá»Ÿi táº¡o lá»›p há»c pháº§n thá»±c táº­p.</p>
    </div>

    <!-- Grouped Class Sections -->
    <div v-else class="space-y-10">
      <section v-for="(group, idx) in groupedClasses" :key="idx" class="space-y-4">
        <div class="border-b border-slate-200 pb-3 flex items-center justify-between">
          <h2 class="font-bold text-base text-slate-800 uppercase tracking-tight flex items-center gap-2">
            <span class="material-symbols-outlined text-blue-600 text-[20px]">school</span>
            {{ group.tenKy }}
          </h2>
          <span class="text-xs font-semibold text-slate-500 bg-slate-100 px-2.5 py-1 rounded-full">
            {{ group.items.length }} lá»›p
          </span>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div
            v-for="c in group.items"
            :key="c.maLop"
            class="group bg-white border border-slate-200 p-6 rounded-2xl hover:shadow-lg transition-all cursor-pointer flex flex-col justify-between relative overflow-hidden"
            @click="goStudents(c.maLop)"
          >
            <div>
              <div class="flex justify-between items-start mb-4">
                <span class="bg-blue-100 text-blue-800 px-2.5 py-1 rounded-md text-xs font-bold uppercase">
                  {{ extractCourseBadge(c.tenLop) }}
                </span>
                <span
                  class="px-2.5 py-0.5 rounded-full text-[11px] font-bold"
                  :class="c.ghiDanhMo ? 'bg-emerald-100 text-emerald-800' : 'bg-slate-100 text-slate-600'"
                >
                  {{ c.ghiDanhMo ? 'Äang má»Ÿ ghi danh' : 'ÄÃ£ Ä‘Ã³ng ghi danh' }}
                </span>
              </div>
              <h3 class="font-bold text-lg text-slate-900 mb-1 group-hover:text-blue-700 transition-colors">
                {{ c.tenLop }}
              </h3>
              <p class="text-xs text-slate-500 mb-4 flex items-center gap-1.5">
                <span class="material-symbols-outlined text-[14px]">key</span>
                MÃ£ tham gia: <strong class="font-mono text-slate-700">{{ c.maThamGia }}</strong>
              </p>
            </div>

            <div class="flex items-center justify-between pt-4 border-t border-slate-100">
              <div class="flex items-center gap-2">
                <span class="material-symbols-outlined text-blue-600 text-[18px]">groups</span>
                <span class="text-xs font-bold text-slate-800">
                  {{ c.soSinhVien }} <span class="font-normal text-slate-500">Sinh viÃªn</span>
                </span>
              </div>
              <div class="flex items-center gap-1 text-xs font-bold text-blue-600 group-hover:translate-x-1 transition-transform">
                Chi tiáº¿t lá»›p <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
              </div>
            </div>
          </div>
        </div>
      </section>
    </div>

    <!-- Toast Notification -->
    <div v-if="toast" class="fixed bottom-6 right-6 z-[90] bg-slate-900 text-white px-4 py-3 rounded-xl shadow-xl text-xs font-bold flex items-center gap-2">
      <span class="material-symbols-outlined text-emerald-400 text-[18px]">check_circle</span>
      {{ toast }}
    </div>

    <!-- Modal Táº¡o Lá»›p Má»›i -->
    <div
      v-if="showCreate"
      class="fixed inset-0 z-[80] flex items-center justify-center bg-black/40 backdrop-blur-xs px-4"
      @click.self="showCreate = false"
    >
      <div class="w-full max-w-[500px] bg-white rounded-2xl shadow-2xl overflow-hidden">
        <!-- Modal Header -->
        <div class="px-6 py-4 border-b border-slate-200 flex justify-between items-center bg-slate-50">
          <div>
            <h3 class="font-bold text-lg text-blue-900">Táº¡o lá»›p má»›i</h3>
            <p class="text-xs text-slate-500">Cáº¥u hÃ¬nh thÃ´ng tin cÆ¡ báº£n cho lá»›p hÆ°á»›ng dáº«n</p>
          </div>
          <button class="material-symbols-outlined text-slate-400 hover:text-slate-700 transition-colors" @click="showCreate = false">
            close
          </button>
        </div>

        <!-- Modal Body -->
        <div class="p-6 space-y-5">
          <div>
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-700 mb-1.5">
              Há»c ká»³ & NÄƒm há»c <span class="text-rose-500">*</span>
            </label>
            <select v-model="form.maKy" class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-medium outline-none focus:ring-2 focus:ring-blue-600">
              <option v-for="k in cycles" :key="k.maKy" :value="k.maKy">{{ k.tenKy }}</option>
            </select>
          </div>

          <div>
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-700 mb-1.5">
              MÃ£ há»c pháº§n <span class="text-rose-500">*</span>
            </label>
            <select v-model="form.maHocPhan" class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-medium outline-none focus:ring-2 focus:ring-blue-600">
              <option value="MIS2012">Thá»±c táº­p nghá» nghiá»‡p â€” MIS2012</option>
              <option value="MIS4012">Thá»±c táº­p tá»‘t nghiá»‡p â€” MIS4012</option>
              <option value="TT">Thá»±c táº­p chung â€” TT</option>
            </select>
          </div>

          <div>
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-700 mb-1.5">
              Sá»‘ thá»© tá»± lá»›p <span class="text-rose-500">*</span>
            </label>
            <input
              v-model.number="form.soThuTuLop"
              type="number"
              min="1"
              placeholder="Nháº­p sá»‘ thá»© tá»± lá»›p (VD: 1, 2, 3)"
              class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-medium outline-none focus:ring-2 focus:ring-blue-600"
            />
          </div>

          <div class="bg-blue-50/70 border border-blue-100 rounded-xl p-4">
            <p class="text-[11px] font-bold text-blue-800 uppercase tracking-wider mb-1 flex items-center gap-1">
              <span class="material-symbols-outlined text-[16px]">visibility</span> Preview tÃªn lá»›p
            </p>
            <p class="font-extrabold text-base text-blue-900 font-mono">
              {{ previewName || 'VD: MIS2012_2026_1' }}
            </p>
          </div>

          <p v-if="formError" class="text-rose-600 text-xs font-bold flex items-center gap-1">
            <span class="material-symbols-outlined text-[16px]">error</span>
            {{ formError }}
          </p>
        </div>

        <!-- Modal Footer -->
        <div class="px-6 py-4 bg-slate-50 border-t border-slate-200 flex justify-end gap-3">
          <button class="px-5 py-2 rounded-xl text-xs font-bold text-slate-600 hover:bg-slate-200 transition-colors" @click="showCreate = false">
            Há»§y bá»
          </button>
          <button
            class="px-6 py-2 rounded-xl text-xs font-bold text-white shadow-md transition-all active:scale-95"
            style="background-color: #005EA3;"
            @click="create"
          >
            Táº¡o lá»›p há»c
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted } from 'vue';
  import { useRouter } from 'vue-router';
  import api from '@/api/api';

  const router = useRouter();
  const classes = ref([]);
  const cycles = ref([]);
  const loading = ref(false);
  const toast = ref('');

  function showToast(m) { toast.value = m; setTimeout(() => (toast.value = ''), 3000); }

  async function reload() {
    loading.value = true;
    try {
      const res = await api.get('/giangvien/classes');
      classes.value = Array.isArray(res.data) ? res.data : (res.data?.items || []);
    } catch {
      classes.value = [];
    } finally { loading.value = false; }
  }

  const groupedClasses = computed(() => {
    const groups = {};
    for (const c of classes.value) {
      const ky = c.tenKy || 'Ká»³ há»c khÃ¡c';
      if (!groups[ky]) {
        groups[ky] = { tenKy: ky, items: [] };
      }
      groups[ky].items.push(c);
    }
    return Object.values(groups);
  });

  function extractCourseBadge(tenLop) {
    if (!tenLop) return 'Lá»šP';
    const match = tenLop.match(/K\d+/i);
    return match ? match[0].toUpperCase() : 'Lá»šP';
  }

  const showCreate = ref(false);
  const form = ref({ maKy: null, maHocPhan: 'MIS2012', soThuTuLop: 1 });
  const formError = ref('');

  const previewName = computed(() => {
    const ky = (cycles.value || []).find((k) => k.maKy === form.value.maKy);
    if (!ky || !form.value.soThuTuLop) return '';
    const mhp = (form.value.maHocPhan || 'MIS2012').trim();
    return `${mhp}_${ky.namHoc || '2026'}_${form.value.soThuTuLop}`;
  });

  async function openCreate() {
    formError.value = '';
    if (!cycles.value.length) {
      try {
        const res = await api.get('/giangvien/cycles');
        cycles.value = Array.isArray(res.data) ? res.data : (res.data?.items || []);
      } catch {
        cycles.value = [];
      }
    }
    form.value = { maKy: cycles.value[0]?.maKy ?? null, maHocPhan: 'MIS2012', soThuTuLop: 1 };
    showCreate.value = true;
  }

  async function create() {
    formError.value = '';
    if (!form.value.soThuTuLop || form.value.soThuTuLop <= 0) {
      formError.value = 'Vui lÃ²ng nháº­p sá»‘ lá»›p há»£p lá»‡';
      return;
    }
    try {
      const res = await api.post('/giangvien/classes', form.value);
      showToast(res.data.message || 'Táº¡o lá»›p má»›i thÃ nh cÃ´ng!');
      showCreate.value = false;
      await reload();
      if (res.data.maLop) {
        goStudents(res.data.maLop);
      }
    } catch (e) {
      formError.value = e.response?.data?.message || 'Táº¡o lá»›p tháº¥t báº¡i';
    }
  }

  function goStudents(maLop) { router.push(`/teacher/classes/${maLop}/students`); }

  onMounted(reload);
</script>

