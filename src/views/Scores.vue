<template>
  <div class="ims-scope max-w-6xl mx-auto font-sans space-y-6 pb-20">
    <nav class="flex items-center text-xs text-slate-500 gap-1.5">
      <router-link to="/student/classes" class="hover:text-[#005EA3] transition font-medium">Lớp của tôi</router-link>
      <span class="material-symbols-outlined text-[14px]">chevron_right</span>
      <span class="hover:text-[#005EA3] transition font-medium">Lớp Thực tập K64</span>
      <span class="material-symbols-outlined text-[14px]">chevron_right</span>
      <span class="text-slate-900 font-bold">Bảng điểm thực tập</span>
    </nav>

    <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs space-y-4">
      <div class="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-slate-100 pb-4">
        <div>
          <h1 class="text-2xl font-extrabold text-slate-900 tracking-tight flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3] text-[28px]">grade</span>
            Kết quả học tập & Bảng điểm
          </h1>
          <p class="text-xs text-slate-500 mt-1">Bảng tổng hợp điểm số các thành phần thực tập được đồng bộ trực tiếp từ hệ thống giảng dạy.</p>
        </div>

        <div class="flex items-center gap-3">
          <button
            @click="exportScores"
            class="px-4 py-2 bg-slate-100 hover:bg-slate-200 text-slate-800 font-bold text-xs rounded-xl border border-slate-300 transition flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[18px]">download</span>
            Xuất file PDF / Excel
          </button>
        </div>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 bg-slate-50 p-4 rounded-xl border border-slate-200 text-xs font-medium text-slate-700">
        <div class="flex items-center gap-3">
          <div class="w-10 h-10 rounded-full bg-[#005EA3]/10 text-[#005EA3] flex items-center justify-center font-bold text-sm shrink-0 uppercase">
            {{ userInitials }}
          </div>
          <div>
            <p class="text-[10px] text-slate-400 uppercase font-bold">Họ và tên sinh viên</p>
            <p class="font-extrabold text-slate-900 text-sm uppercase">{{ currentUser.hoTen || 'Sinh viên' }}</p>
            <p class="text-[11px] text-slate-500 font-mono">MSSV: {{ currentUser.maDinhDanh || currentUser.username || '---' }}</p>
          </div>
        </div>

        <div>
          <p class="text-[10px] text-slate-400 uppercase font-bold">Lớp sinh hoạt / Khoa</p>
          <p class="font-bold text-slate-900 mt-0.5">{{ currentUser.lopSinhHoat || 'Lớp 49K21.1' }} • Khoa CNTT</p>
        </div>

        <div>
          <p class="text-[10px] text-slate-400 uppercase font-bold">Lớp học phần thực tập</p>
          <p class="font-bold text-slate-900 mt-0.5">Thực tập tốt nghiệp K64 (Nhóm 01)</p>
        </div>
      </div>
    </div>

    <div class="bg-white rounded-2xl border border-slate-200 shadow-2xs overflow-hidden space-y-4 p-4 sm:p-6">
      <div class="flex items-center justify-between border-b border-slate-100 pb-3">
        <h3 class="font-bold text-sm text-slate-900 flex items-center gap-2">
          <span class="material-symbols-outlined text-[#005EA3]">table_chart</span>
          Chi tiết bảng điểm Matrix
        </h3>
        <span class="text-xs text-slate-400 italic hidden sm:inline">Tự động đồng bộ từ Sổ điểm Giảng viên</span>
      </div>

      <div class="overflow-x-auto border border-slate-200 rounded-xl">
        <table class="w-full text-center border-collapse text-xs">
          <thead>
            <tr class="bg-slate-100 text-slate-800 font-bold border-b border-slate-200">
              <th v-for="cat in categories" :key="cat.code" class="py-3 px-3 border-r border-slate-200 text-[#005EA3]" :colspan="(cat.subItems?.length || 1) + 1">
                <div class="flex items-center justify-center gap-1.5">
                  {{ cat.name }} ({{ cat.weight }}%)
                  <span class="text-[10px] bg-blue-100 text-[#005EA3] px-1.5 py-0.5 rounded">Trọng số {{ cat.weight }}%</span>
                </div>
              </th>
              <th class="py-3 px-4 text-slate-900 bg-amber-50/80 min-w-[100px]" rowspan="2">Điểm tổng kết</th>
            </tr>

            <tr class="bg-slate-50 text-slate-700 font-bold border-b border-slate-200 text-[11px]">
              <template v-for="cat in categories" :key="'sub_' + cat.code">
                <th v-for="sub in cat.subItems" :key="sub.id" class="py-2.5 px-3 border-r border-slate-200 min-w-[110px]">
                  <div class="flex flex-col items-center">
                    <span class="font-bold text-slate-800 truncate max-w-[120px]">{{ sub.name }}</span>
                    <div class="text-[10px] text-slate-500 font-normal flex items-center gap-1 mt-0.5">
                      <span class="text-blue-700 font-bold">{{ sub.ratio }}%</span>
                      <span v-if="sub.minPassScore">• Liệt: ≥{{ sub.minPassScore }}đ</span>
                    </div>
                  </div>
                </th>
                <th class="py-2.5 px-3 border-r border-slate-200 bg-blue-50/50 text-[#005EA3] min-w-[80px]">
                  Điểm {{ cat.code }}
                </th>
              </template>
            </tr>
          </thead>

          <tbody class="divide-y divide-slate-100 font-semibold text-slate-800">
            <tr>
              <template v-for="cat in categories" :key="'val_' + cat.code">
                <td v-for="sub in cat.subItems" :key="'val_sub_' + sub.id" class="py-4 px-3 border-r border-slate-200">
                  <span v-if="sub.score !== null && sub.score !== undefined" :class="sub.minPassScore && sub.score < sub.minPassScore ? 'text-rose-700 font-extrabold' : ''">
                    {{ sub.score }}
                  </span>
                  <span v-else class="text-slate-400 italic">
                    Chưa có
                  </span>
                </td>
                <td class="py-4 px-3 border-r border-slate-200 bg-blue-50/40 text-[#005EA3] font-bold">
                  {{ getCatScoreFormatted(cat) }}
                </td>
              </template>

              <td class="py-4 px-4 font-black text-base text-[#005EA3] bg-amber-50/80">
                {{ finalScoreFormatted }}
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="pt-4 flex flex-col sm:flex-row items-center justify-between gap-4 text-xs">
        <div class="flex items-center gap-2">
          <span class="px-3 py-1 bg-emerald-100 text-emerald-800 rounded-lg font-bold">
            Xếp loại: {{ getPerformanceLabel(finalScoreValue) }}
          </span>
          <span class="px-3 py-1 bg-blue-100 text-[#005EA3] rounded-lg font-bold">
            Thang điểm 4: {{ finalScoreValue ? (finalScoreValue * 0.4).toFixed(1) + ' / 4.0' : '-' }}
          </span>
        </div>

        <router-link to="/student/tasks" class="text-[#005EA3] font-bold hover:underline flex items-center gap-1">
          Xem chi tiết nhận xét & bài nộp <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted, onUnmounted } from 'vue';
  import { useAuthStore } from '@/stores/auth';

  const authStore = useAuthStore();
  const currentUser = computed(() => authStore.user || JSON.parse(localStorage.getItem('user') || '{}'));

  const userInitials = computed(() => {
    const name = currentUser.value?.hoTen || 'SV';
    const parts = name.trim().split(' ').filter(Boolean);
    if (parts.length >= 2) {
      return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
    }
    return name.substring(0, 2).toUpperCase();
  });

  const defaultCategories = [
    {
      code: 'QT',
      name: 'Đánh giá Quá trình',
      weight: 20,
      subItems: [
        { id: 's1_1', name: 'Chuyên cần', ratio: 40, minPassScore: 4.0, score: null },
        { id: 's1_2', name: 'Nhật ký thực tập', ratio: 60, minPassScore: 5.0, score: null }
      ]
    },
    {
      code: 'GK',
      name: 'Đánh giá Giữa kỳ',
      weight: 30,
      subItems: [
        { id: 's2_1', name: 'Báo cáo giữa kỳ (GV)', ratio: 50, minPassScore: 4.0, score: null },
        { id: 's2_2', name: 'Đánh giá Doanh nghiệp (GK)', ratio: 50, minPassScore: 4.0, score: null }
      ]
    },
    {
      code: 'CK',
      name: 'Đánh giá Cuối kỳ',
      weight: 50,
      subItems: [
        { id: 's3_1', name: 'Báo cáo / Khóa luận', ratio: 40, minPassScore: 4.0, score: null },
        { id: 's3_2', name: 'Đánh giá Doanh nghiệp (CK)', ratio: 40, minPassScore: 5.0, score: null },
        { id: 's3_3', name: 'Bảo vệ / Báo cáo', ratio: 20, minPassScore: 4.0, score: null }
      ]
    }
  ];

  const categories = ref(JSON.parse(JSON.stringify(defaultCategories)));

  function loadSavedStructure() {
    try {
      const studentId = currentUser.value?.maDinhDanh || currentUser.value?.username || '';
      // Try to read student-specific scores if saved by teacher
      const savedUserScores = localStorage.getItem(`ims_student_scores_${studentId}`);
      let userScoreMap = null;
      if (savedUserScores) {
        try { userScoreMap = JSON.parse(savedUserScores); } catch (e) { console.error(e); }
      }

      const saved = localStorage.getItem('ims_grading_structure_1') || localStorage.getItem('ims_grading_structure_global');
      if (saved) {
        const parsed = JSON.parse(saved);
        if (Array.isArray(parsed) && parsed.length > 0) {
          categories.value = parsed.map((grp, idx) => ({
            code: grp.code || (idx === 0 ? 'QT' : idx === 1 ? 'GK' : 'CK'),
            name: grp.name ? grp.name.replace(/^\d+\.\s*/, '') : (idx === 0 ? 'Quá trình' : idx === 1 ? 'Giữa kỳ' : 'Cuối kỳ'),
            weight: grp.weight || (idx === 0 ? 20 : idx === 1 ? 30 : 50),
            subItems: (grp.subItems || []).map((s, sIdx) => {
              const subKey = s.id || `s${idx+1}_${sIdx+1}`;
              const val = userScoreMap && userScoreMap[subKey] !== undefined ? userScoreMap[subKey] : (s.score !== undefined ? s.score : null);
              return {
                id: subKey,
                name: s.name,
                ratio: s.ratio,
                minPassScore: s.minPassScore,
                score: val
              };
            })
          }));
        }
      } else if (userScoreMap) {
        categories.value = categories.value.map(cat => ({
          ...cat,
          subItems: cat.subItems.map(s => ({
            ...s,
            score: userScoreMap[s.id] !== undefined ? userScoreMap[s.id] : null
          }))
        }));
      }
    } catch (e) {
      console.error('Error loading structure in Scores.vue', e);
    }
  }

  function handleStructureUpdated(e) {
    if (e.detail && e.detail.groups) {
      const parsed = e.detail.groups;
      const studentId = currentUser.value?.maDinhDanh || currentUser.value?.username || '';
      const savedUserScores = localStorage.getItem(`ims_student_scores_${studentId}`);
      let userScoreMap = null;
      if (savedUserScores) {
        try { userScoreMap = JSON.parse(savedUserScores); } catch (err) { console.error(err); }
      }

      categories.value = parsed.map((grp, idx) => ({
        code: grp.code || (idx === 0 ? 'QT' : idx === 1 ? 'GK' : 'CK'),
        name: grp.name ? grp.name.replace(/^\d+\.\s*/, '') : (idx === 0 ? 'Quá trình' : idx === 1 ? 'Giữa kỳ' : 'Cuối kỳ'),
        weight: grp.weight || (idx === 0 ? 20 : idx === 1 ? 30 : 50),
        subItems: (grp.subItems || []).map((s, sIdx) => {
          const subKey = s.id || `s${idx+1}_${sIdx+1}`;
          const val = userScoreMap && userScoreMap[subKey] !== undefined ? userScoreMap[subKey] : null;
          return {
            id: subKey,
            name: s.name,
            ratio: s.ratio,
            minPassScore: s.minPassScore,
            score: val
          };
        })
      }));
    }
  }

  onMounted(() => {
    loadSavedStructure();
    window.addEventListener('grading-structure-updated', handleStructureUpdated);
  });

  onUnmounted(() => {
    window.removeEventListener('grading-structure-updated', handleStructureUpdated);
  });

  function getCatScoreValue(cat) {
    let sum = 0;
    let ratioSum = 0;
    let count = 0;
    for (const sub of cat.subItems || []) {
      if (sub.score !== null && sub.score !== undefined) {
        sum += Number(sub.score) * ((sub.ratio || 0) / 100);
        ratioSum += (sub.ratio || 0);
        count++;
      }
    }
    if (count === 0 || ratioSum === 0) return null;
    return (sum / (ratioSum / 100));
  }

  function getCatScoreFormatted(cat) {
    const val = getCatScoreValue(cat);
    return val !== null && val !== undefined ? val.toFixed(2) : '-';
  }

  const finalScoreValue = computed(() => {
    let total = 0;
    let wSum = 0;
    let count = 0;
    for (const cat of categories.value) {
      const s = getCatScoreValue(cat);
      if (s !== null && s !== undefined) {
        total += s * ((cat.weight || 0) / 100);
        wSum += (cat.weight || 0);
        count++;
      }
    }
    if (count === 0 || wSum === 0) return null;
    return (total / (wSum / 100));
  });

  const finalScoreFormatted = computed(() => {
    return finalScoreValue.value !== null && finalScoreValue.value !== undefined ? finalScoreValue.value.toFixed(2) : '-';
  });

  function getPerformanceLabel(val) {
    if (val === null || val === undefined) return 'Chưa đánh giá';
    if (val >= 9.0) return 'Xuất Sắc (A+)';
    if (val >= 8.0) return 'Giỏi (A)';
    if (val >= 7.0) return 'Khá (B)';
    if (val >= 5.0) return 'Trung Bình (C)';
    return 'Yếu / Chưa đạt';
  }

  function exportScores() {
    alert('Đang xuất bảng điểm chi tiết định dạng PDF...');
  }
</script>

