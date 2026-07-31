<template>
  <div class="ims-scope w-full mx-auto space-y-6">
    <!-- Top Header -->
    <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-xs flex flex-col md:flex-row md:items-center justify-between gap-4">
      <div>
        <h1 class="font-bold text-2xl text-slate-900 tracking-tight flex items-center gap-2">
          <span class="material-symbols-outlined text-blue-600 text-[28px]">groups</span>
          Quản lý Sinh viên & Há»“ sÆ¡ Chi tiáº¿t
        </h1>
        <p class="text-xs text-slate-500 mt-1">Danh sách táº¥t cáº£ sinh viên thuá»™c cÃ¡c lá»›p thực tập do báº¡n trá»±c tiáº¿p hư�:ng dẫn.</p>
      </div>

      <div class="flex items-center gap-3">
        <span class="bg-blue-50 text-blue-800 text-xs font-bold px-3 py-1.5 rounded-xl border border-blue-200">
          Tá»•ng sá»‘: {{ students.length }} sinh viên
        </span>
      </div>
    </div>

    <!-- Stats Bar -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
      <div class="bg-white p-4 rounded-2xl border border-slate-200 shadow-xs flex items-center justify-between">
        <div>
          <p class="text-[11px] font-bold text-slate-400 uppercase tracking-wider">Tá»•ng Sinh viên</p>
          <p class="text-2xl font-black text-slate-900 mt-0.5">{{ students.length }}</p>
          <p class="text-[11px] text-slate-500 mt-0.5">Thuá»™c {{ classCount }} lá»›p hư�:ng dẫn</p>
        </div>
        <div class="w-10 h-10 rounded-xl bg-blue-50 text-blue-600 flex items-center justify-center font-bold">
          <span class="material-symbols-outlined text-[22px]">person</span>
        </div>
      </div>

      <div class="bg-white p-4 rounded-2xl border border-slate-200 shadow-xs flex items-center justify-between">
        <div>
          <p class="text-[11px] font-bold text-slate-400 uppercase tracking-wider">ÄÃ£ cÃ³ Doanh nghi�!p</p>
          <p class="text-2xl font-black text-emerald-600 mt-0.5">{{ withCompanyCount }}</p>
          <p class="text-[11px] text-emerald-600 font-semibold mt-0.5">{{ Math.round((withCompanyCount / (students.length || 1)) * 100) }}% hoÃ n thÃ nh</p>
        </div>
        <div class="w-10 h-10 rounded-xl bg-emerald-50 text-emerald-600 flex items-center justify-center font-bold">
          <span class="material-symbols-outlined text-[22px]">domain</span>
        </div>
      </div>

      <div class="bg-white p-4 rounded-2xl border border-slate-200 shadow-xs flex items-center justify-between">
        <div>
          <p class="text-[11px] font-bold text-slate-400 uppercase tracking-wider">ÄÃ£ ghÃ©p Nhóm</p>
          <p class="text-2xl font-black text-indigo-600 mt-0.5">{{ withGroupCount }}</p>
          <p class="text-[11px] text-indigo-600 font-semibold mt-0.5">Nhóm thực tập tá»± quáº£n</p>
        </div>
        <div class="w-10 h-10 rounded-xl bg-indigo-50 text-indigo-600 flex items-center justify-center font-bold">
          <span class="material-symbols-outlined text-[22px]">diversity_3</span>
        </div>
      </div>

      <div class="bg-white p-4 rounded-2xl border border-slate-200 shadow-xs flex items-center justify-between">
        <div>
          <p class="text-[11px] font-bold text-slate-400 uppercase tracking-wider">ÄÃºng tiáº¿n Ä‘á»™</p>
          <p class="text-2xl font-black text-amber-600 mt-0.5">{{ onTimeCount }}</p>
          <p class="text-[11px] text-amber-600 font-semibold mt-0.5">Ná»™p nháº­t kÃ½ hÃ ng tuáº§n</p>
        </div>
        <div class="w-10 h-10 rounded-xl bg-amber-50 text-amber-600 flex items-center justify-center font-bold">
          <span class="material-symbols-outlined text-[22px]">verified</span>
        </div>
      </div>
    </div>

    <!-- Filters & Table Card -->
    <div class="bg-white rounded-2xl border border-slate-200 p-5 shadow-xs space-y-4">
      <div class="flex flex-col md:flex-row items-stretch md:items-center justify-between gap-3">
        <div class="relative flex-1">
          <span class="material-symbols-outlined text-[20px] absolute left-3 top-1/2 -translate-y-1/2 text-slate-400">search</span>
          <input
            v-model="search"
            placeholder="TÃ¬m kiáº¿m há» tên, MSSV, lá»›p, doanh nghiá»‡p..."
            class="w-full pl-10 pr-4 py-2 border border-slate-300 rounded-xl text-xs font-medium focus:ring-2 focus:ring-blue-600 outline-none"
          />
        </div>

        <div class="flex items-center gap-2">
          <select v-model="statusFilter" class="px-3.5 py-2 border border-slate-300 rounded-xl text-xs font-semibold text-slate-800 outline-none">
            <option value="all">Táº¥t cáº£ tráº¡ng thÃ¡i</option>
            <option value="DangThucTap">Äang thực tập</option>
            <option value="ChoGhiDanh">Chá» ghi danh</option>
            <option value="DungThucTap">Dá»«ng thực tập</option>
          </select>
        </div>
      </div>

      <!-- Students Table -->
      <div class="border border-slate-200 rounded-2xl overflow-x-auto">
        <table class="w-full text-left text-xs border-collapse">
          <thead>
            <tr class="bg-slate-50 border-b border-slate-200 text-slate-600 font-bold uppercase tracking-wider">
              <th class="p-4">Sinh viên</th>
              <th class="p-4">L�:p thực tập</th>
              <th class="p-4">Nhóm</th>
              <th class="p-4">ÄÆ¡n vá»‹ thực tập</th>
              <th class="p-4">Trạng thái</th>
              <th class="p-4 text-right">Há»“ sÆ¡ chi tiáº¿t</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100">
            <tr v-if="loading" class="text-center">
              <td colspan="6" class="p-8 text-slate-500">Äang táº£i danh sÃ¡ch sinh viên...</td>
            </tr>
            <tr v-else-if="!filteredStudents.length" class="text-center">
              <td colspan="6" class="p-8 text-slate-500">KhÃ´ng tÃ¬m tháº¥y sinh viên thá»a mÃ£n Ä‘iá»u kiá»‡n.</td>
            </tr>
            <tr v-for="s in filteredStudents" :key="s.maSinhVien" class="hover:bg-slate-50 transition-colors">
              <td class="p-4">
                <p class="font-bold text-slate-900 text-sm hover:text-blue-600 cursor-pointer" @click="openProfile(s)">
                  {{ s.hoTen }}
                </p>
                <p class="text-[11px] text-slate-400 font-mono">MSSV: {{ s.maSoSinhVien || s.maDinhDanh }}</p>
              </td>

              <td class="p-4 font-semibold text-slate-700">
                {{ s.tenLop || 'MIS2012_01' }}
              </td>

              <td class="p-4 font-semibold text-slate-700">
                <span v-if="s.tenNhom" class="px-2 py-0.5 bg-indigo-50 text-indigo-700 border border-indigo-200 rounded font-mono font-bold text-[11px]">
                  {{ s.tenNhom }}
                </span>
                <span v-else class="text-slate-400">-</span>
              </td>

              <td class="p-4">
                <p class="font-bold text-slate-800">{{ s.donViThucTap || 'ChÆ°a cáº­p nháº­t' }}</p>
                <p class="text-[11px] text-slate-400">{{ s.viTriThucTap || '--' }}</p>
              </td>

              <td class="p-4">
                <span
                  class="px-2.5 py-1 rounded-full text-[11px] font-bold inline-block"
                  :class="statusCls[s.trangThaiThucTap] || 'bg-emerald-100 text-emerald-800'"
                >
                  {{ statusLabels[s.trangThaiThucTap] || 'Äang thực tập' }}
                </span>
              </td>

              <td class="p-4 text-right">
                <button
                  class="px-3 py-1.5 rounded-xl bg-blue-50 text-blue-700 border border-blue-200 hover:bg-blue-100 font-bold text-xs transition-colors inline-flex items-center gap-1"
                  @click="openProfile(s)"
                >
                  <span class="material-symbols-outlined text-[16px]">badge</span>
                  Xem há»“ sÆ¡
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Drawer Há»“ sÆ¡ Chi tiáº¿t Sinh viên -->
    <div v-if="detail" class="fixed inset-0 z-[85] flex justify-end bg-black/40 backdrop-blur-xs" @click.self="detail = null">
      <div class="w-full max-w-[520px] h-full bg-white shadow-2xl flex flex-col justify-between overflow-hidden">
        <!-- Drawer Header -->
        <div class="p-6 border-b border-slate-200 flex justify-between items-start bg-slate-50">
          <div>
            <span class="px-2.5 py-0.5 rounded-full text-[10px] font-extrabold uppercase bg-blue-100 text-blue-800 mb-2 inline-block">
              Há»’ SÆ  THá»°C Táº¬P SINH VIÃŠN
            </span>
            <h3 class="font-extrabold text-xl text-slate-900">{{ detail.hoTen }}</h3>
            <p class="text-xs text-slate-500 font-mono mt-0.5">MSSV: {{ detail.maSoSinhVien || detail.maDinhDanh }}</p>
          </div>
          <button class="material-symbols-outlined text-slate-400 hover:text-slate-700 transition-colors" @click="detail = null">
            close
          </button>
        </div>

        <!-- Drawer Body -->
        <div class="p-6 overflow-y-auto space-y-6 flex-1 text-xs">
          <!-- Personal Info -->
          <div class="space-y-3">
            <h4 class="font-bold text-slate-900 uppercase tracking-wider text-[11px] text-blue-900 border-b border-slate-100 pb-1">
              Thông tin Há»c pháº§n & LiÃªn há»‡
            </h4>
            <div class="grid grid-cols-2 gap-3 text-slate-700">
              <div><span class="text-slate-400 block text-[10px] uppercase">L�:p sinh hoạt:</span> <strong>{{ detail.lopSinhHoat || '22CNTT1' }}</strong></div>
              <div><span class="text-slate-400 block text-[10px] uppercase">L�:p thực tập:</span> <strong>{{ detail.tenLop || 'MIS2012_01' }}</strong></div>
              <div><span class="text-slate-400 block text-[10px] uppercase">Email:</span> <strong class="truncate block">{{ detail.email || '-' }}</strong></div>
              <div><span class="text-slate-400 block text-[10px] uppercase">Sá»‘ Ä‘iá»‡n thoáº¡i:</span> <strong>{{ detail.soDienThoai || '-' }}</strong></div>
            </div>
          </div>

          <!-- Enterprise Info -->
          <div class="space-y-3">
            <h4 class="font-bold text-slate-900 uppercase tracking-wider text-[11px] text-blue-900 border-b border-slate-100 pb-1">
              Doanh nghi�!p & CÃ¡n bá»™ hư�:ng dẫn
            </h4>
            <div class="bg-slate-50 p-4 rounded-2xl border border-slate-200 space-y-2.5">
              <div>
                <span class="text-slate-400 text-[10px] uppercase block font-bold">ÄÆ¡n vá»‹ thực tập:</span>
                <p class="font-extrabold text-slate-900 text-sm">{{ detail.donViThucTap || 'ChÆ°a cáº­p nháº­t' }}</p>
              </div>
              <div class="grid grid-cols-2 gap-2 pt-1 border-t border-slate-200/60">
                <div>
                  <span class="text-slate-400 text-[10px] uppercase block">V�9 trí thực tập:</span>
                  <p class="font-bold text-slate-800">{{ detail.viTriThucTap || '--' }}</p>
                </div>
                <div>
                  <span class="text-slate-400 text-[10px] uppercase block">Mentor doanh nghiá»‡p:</span>
                  <p class="font-bold text-slate-800">{{ detail.mentor || 'ChÆ°a phÃ¢n cÃ´ng' }}</p>
                </div>
              </div>
            </div>
          </div>

          <!-- Project Topic -->
          <div class="space-y-2">
            <h4 class="font-bold text-slate-900 uppercase tracking-wider text-[11px] text-blue-900 border-b border-slate-100 pb-1">
              Äá» tÃ i ÄÄƒng kÃ½
            </h4>
            <div class="p-4 bg-blue-50/70 border border-blue-100 rounded-2xl">
              <p class="font-extrabold text-blue-900 text-sm mb-1">{{ detail.deTai?.tenDeTai || 'ChÆ°a Ä‘Äƒng kÃ½ tên Ä‘á» tÃ i' }}</p>
              <p class="text-slate-600 text-[11px] leading-relaxed">{{ detail.deTai?.moTa || 'MÃ´ táº£ ná»™i dung Ä‘á» tÃ i thực tập tại Ä‘Æ¡n vá»‹.' }}</p>
            </div>
          </div>

          <!-- Grades Breakdown -->
          <div class="space-y-3">
            <h4 class="font-bold text-slate-900 uppercase tracking-wider text-[11px] text-blue-900 border-b border-slate-100 pb-1">
              Bảng �iỒm ÄÃ¡nh giÃ¡ Chi tiáº¿t
            </h4>
            <div class="space-y-2">
              <div v-for="d in (detail.diem || defaultGrades)" :key="d.maCotDiem" class="flex justify-between items-center p-3 bg-slate-50 rounded-xl border border-slate-200">
                <span class="font-semibold text-slate-800">{{ d.tenCot }}</span>
                <span class="font-black text-sm text-blue-700 bg-blue-50 px-2.5 py-0.5 rounded-lg border border-blue-200">{{ d.diemSo ?? '-' }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Footer -->
        <div class="p-4 bg-slate-50 border-t border-slate-200 flex justify-end">
          <button class="px-5 py-2 bg-slate-200 hover:bg-slate-300 font-bold text-slate-700 rounded-xl text-xs transition-colors" @click="detail = null">
            ÄÃ³ng
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import api from '@/api/api';

const students = ref([]);
const loading = ref(false);
const search = ref('');
const statusFilter = ref('all');
const classCount = ref(1);

const statusLabels = {
  DangThucTap: 'Äang thực tập',
  ChoGhiDanh: 'Chá» ghi danh',
  DungThucTap: 'Dá»«ng thực tập',
  HoanThanh: 'HoÃ n thÃ nh'
};

const statusCls = {
  DangThucTap: 'bg-emerald-100 text-emerald-800 border border-emerald-200',
  ChoGhiDanh: 'bg-amber-100 text-amber-800 border border-amber-200',
  DungThucTap: 'bg-rose-100 text-rose-800 border border-rose-200',
  HoanThanh: 'bg-blue-100 text-blue-800 border border-blue-200'
};

const defaultGrades = [
  { maCotDiem: 1, tenCot: 'ChuyÃªn cáº§n & Nháº­t kÃ½ tuáº§n (10%)', diemSo: 9.0 },
  { maCotDiem: 2, tenCot: 'ÄÃ¡nh giÃ¡ tá»« Doanh nghi�!p (40%)', diemSo: 8.5 },
  { maCotDiem: 3, tenCot: 'ÄÃ¡nh giÃ¡ tá»« GV HÆ°á»›ng dáº«n (30%)', diemSo: 8.8 },
  { maCotDiem: 4, tenCot: 'Báo cáo / KhÃ³a luáº­n tá»‘t nghiá»‡p (20%)', diemSo: 8.5 }
];

const withCompanyCount = computed(() => students.value.filter(s => s.donViThucTap && s.donViThucTap !== 'ChÆ°a cáº­p nháº­t').length);
const withGroupCount = computed(() => students.value.filter(s => s.tenNhom).length);
const onTimeCount = computed(() => students.value.filter(s => s.tinhTrangTienDo === 'DungTienDo' || !s.tinhTrangTienDo).length);

const filteredStudents = computed(() => {
  let list = students.value;

  if (statusFilter.value !== 'all') {
    list = list.filter(s => s.trangThaiThucTap === statusFilter.value);
  }

  if (search.value) {
    const q = search.value.toLowerCase().trim();
    list = list.filter(s =>
      (s.hoTen && s.hoTen.toLowerCase().includes(q)) ||
      (s.maSoSinhVien && s.maSoSinhVien.toLowerCase().includes(q)) ||
      (s.maDinhDanh && s.maDinhDanh.toLowerCase().includes(q)) ||
      (s.tenLop && s.tenLop.toLowerCase().includes(q)) ||
      (s.donViThucTap && s.donViThucTap.toLowerCase().includes(q))
    );
  }

  return list;
});

const detail = ref(null);

async function openProfile(student) {
  try {
    const res = await api.get(`/giangvien/students/${student.maSinhVien || student.maGhiDanh}`);
    detail.value = res.data || student;
  } catch {
    detail.value = student;
  }
}

onMounted(async () => {
  loading.value = true;
  try {
    const cls = await api.get('/giangvien/classes');
    const classList = Array.isArray(cls.data) ? cls.data : (cls.data?.items || []);
    classCount.value = classList.length || 1;
    const all = [];

    for (const c of classList) {
      const res = await api.get(`/giangvien/classes/${c.maLop}/students`);
      const list = Array.isArray(res.data) ? res.data : (res.data?.items || []);
      list.forEach((s) => {
        if (!all.some(x => (x.maSinhVien || x.maGhiDanh) === (s.maSinhVien || s.maGhiDanh))) {
          all.push({ ...s, tenLop: c.tenLop });
        }
      });
    }

    students.value = all;
  } catch (err) {
    console.error(err);
  } finally {
    loading.value = false;
  }
});
</script>
