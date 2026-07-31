<template>
  <div class="ims-scope w-full mx-auto space-y-6 font-sans pb-24">
    <!-- Breadcrumbs & Header -->
    <div>
      <nav class="flex items-center gap-2 text-xs text-slate-500 mb-2">
        <router-link to="/teacher/classes" class="hover:text-blue-700 transition-colors">Lá»›p cá»§a tÃ´i</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-700 font-medium">ÄÃ¡nh giÃ¡</span>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="font-bold text-slate-900">Sá»• Ä‘iá»ƒm tá»•ng há»£p</span>
      </nav>
      <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 class="text-2xl sm:text-[28px] font-bold text-slate-900 tracking-tight">Sá»• Ä‘iá»ƒm & ÄÃ¡nh giÃ¡ Thá»±c táº­p</h1>
          <p class="text-xs text-slate-500 mt-0.5">
            Thang Ä‘iá»ƒm 10 â€¢ Äiá»ƒm tá»•ng káº¿t tá»± Ä‘á»™ng tÃ­nh theo trá»ng sá»‘ 3 cá»™t: QuÃ¡ trÃ¬nh, Giá»¯a ká»³, Cuá»‘i ká»³
          </p>
        </div>

        <div class="flex flex-wrap items-center gap-2 sm:gap-2.5">
          <router-link
            :to="`/teacher/classes/${classId}/settings`"
            class="px-3 py-2 rounded-xl border border-slate-300 text-slate-700 hover:bg-slate-100 font-bold text-xs flex items-center gap-1.5 transition"
            title="Sá»­a trá»ng sá»‘ & thang Ä‘iá»ƒm"
          >
            <span class="material-symbols-outlined text-[18px] text-blue-600">tune</span>
            Cáº¥u hÃ¬nh trá»ng sá»‘
          </router-link>

          <button
            @click="showExportModal = true"
            class="px-3 py-2 rounded-xl border border-slate-300 text-slate-700 hover:bg-slate-100 font-bold text-xs flex items-center gap-1.5 transition shadow-2xs bg-white"
          >
            <span class="material-symbols-outlined text-[18px] text-emerald-600">file_download</span>
            Xuáº¥t BÃ¡o cÃ¡o (Excel/PDF)
          </button>

          <button
            @click="saveScores"
            :disabled="locked"
            class="px-4 py-2 bg-[#005EA3] hover:bg-blue-800 text-white font-bold text-xs rounded-xl shadow-md transition active:scale-95 disabled:opacity-50 flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[18px]">save</span>
            LÆ°u báº£ng Ä‘iá»ƒm
          </button>
        </div>
      </div>
    </div>

    <!-- Sub Navigation Tabs -->
    <div class="flex items-center gap-4 sm:gap-8 border-b border-slate-200 overflow-x-auto whitespace-nowrap custom-scrollbar">
      <router-link
        :to="`/teacher/classes/${classId}/grading`"
        class="pb-3 text-blue-700 border-b-2 border-blue-700 font-bold text-sm shrink-0"
      >
        Sá»• Ä‘iá»ƒm tá»•ng há»£p
      </router-link>
      <router-link
        :to="`/teacher/classes/${classId}/attendance`"
        class="pb-3 text-slate-500 hover:text-blue-700 font-semibold text-sm transition-colors shrink-0"
      >
        Äiá»ƒm danh buá»•i há»p
      </router-link>
      <router-link
        :to="`/teacher/classes/${classId}/violations`"
        class="pb-3 text-slate-500 hover:text-blue-700 font-semibold text-sm transition-colors flex items-center gap-1.5 shrink-0"
      >
        Theo dÃµi vi pháº¡m
        <span class="bg-rose-100 text-rose-800 text-[10px] px-1.5 py-0.2 rounded-full font-bold">4</span>
      </router-link>
    </div>

    <!-- Weight Breakdown Banner -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <div
        v-for="cat in categories"
        :key="cat.id"
        class="p-4 rounded-2xl border bg-white shadow-2xs flex items-center justify-between"
        :class="cat.borderColor"
      >
        <div class="space-y-0.5">
          <span class="text-[10px] font-extrabold uppercase tracking-wider" :class="cat.textColor">
            {{ cat.code }} ({{ cat.weight }}% Tá»”NG MÃ”N)
          </span>
          <h3 class="font-bold text-slate-900 text-sm">{{ cat.name }}</h3>
          <p class="text-[11px] text-slate-500">{{ cat.subItems.length }} cá»™t thÃ nh pháº§n</p>
        </div>
        <div class="w-10 h-10 rounded-xl flex items-center justify-center font-black text-sm shrink-0" :class="cat.badgeBg">
          {{ cat.weight }}%
        </div>
      </div>
    </div>

    <!-- Controls & Search -->
    <div class="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs flex flex-col md:flex-row justify-between items-stretch md:items-center gap-4">
      <div class="relative flex-1 max-w-md">
        <span class="material-symbols-outlined absolute left-3 top-2.5 text-slate-400 text-[18px]">search</span>
        <input
          v-model="searchQuery"
          type="text"
          placeholder="TÃ¬m kiáº¿m MSSV, tÃªn sinh viÃªn..."
          class="w-full pl-10 pr-4 py-2 bg-slate-50 border border-slate-300 rounded-xl text-xs outline-none focus:ring-2 focus:ring-blue-600"
        />
      </div>

      <div class="flex items-center gap-3 text-xs">
        <span class="text-slate-500">Äang hiá»ƒn thá»‹ <strong class="text-slate-900">{{ filteredStudents.length }}</strong> sinh viÃªn</span>
        <button
          @click="autoCalculate"
          class="px-3.5 py-2 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold rounded-xl transition flex items-center gap-1.5"
        >
          <span class="material-symbols-outlined text-[16px] text-blue-600">calculate</span>
          TÃ­nh láº¡i toÃ n bá»™
        </button>
      </div>
    </div>

    <!-- Main Gradebook Grid Table -->
    <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-2xs">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse text-xs">
          <thead>
            <!-- Group Header Row -->
            <tr class="bg-slate-100/80 border-b border-slate-200 font-bold uppercase text-[10px] text-slate-600">
              <th class="px-4 py-3 sticky left-0 bg-slate-100 z-10 w-48 border-r border-slate-200">Sinh viÃªn</th>
              <th class="px-3 py-3 font-mono text-slate-500 w-24 border-r border-slate-200">MSSV</th>

              <!-- Dynamic Category Headers -->
              <th
                v-for="(cat, cIdx) in categories"
                :key="cat.id || cIdx"
                :colspan="(cat.subItems?.length || 0) + 1"
                class="px-3 py-2 text-center border-r-2"
                :class="[
                  cIdx === 0 ? 'bg-emerald-100/50 text-emerald-900 border-emerald-300' :
                  cIdx === 1 ? 'bg-sky-100/50 text-sky-900 border-sky-300' :
                  'bg-indigo-100/50 text-indigo-900 border-indigo-300'
                ]"
              >
                {{ cat.name }} (Trá»ng sá»‘ {{ cat.weight }}%)
              </th>

              <th class="px-4 py-3 text-center bg-slate-200/60 text-slate-900 font-black w-28">Tá»”NG MÃ”N</th>
              <th class="px-4 py-3 text-center w-24">Káº¾T QUáº¢</th>
            </tr>

            <!-- Column Sub-header Row -->
            <tr class="bg-slate-50 border-b border-slate-200 text-[11px] font-bold text-slate-600">
              <th class="px-4 py-2 sticky left-0 bg-slate-50 border-r border-slate-200"></th>
              <th class="px-3 py-2 border-r border-slate-200"></th>

              <!-- Dynamic Sub Items & Category Sub-totals -->
              <template v-for="(cat, cIdx) in categories" :key="'sub_hdr_' + (cat.id || cIdx)">
                <th
                  v-for="sub in cat.subItems"
                  :key="sub.id"
                  class="px-2 py-2 text-center font-medium border-r border-slate-100 min-w-[110px]"
                >
                  <div class="flex flex-col items-center gap-0.5">
                    <div class="flex items-center gap-1">
                      <span class="block truncate max-w-[90px] font-bold text-slate-800" :title="sub.name">{{ sub.name }}</span>
                      <button @click="openSubConfig(sub)" class="text-slate-400 hover:text-blue-700 p-0.5" title="Cáº¥u hÃ¬nh Ä‘iá»ƒm sub item">
                        <span class="material-symbols-outlined text-[13px]">tune</span>
                      </button>
                    </div>
                    <div class="flex items-center gap-1 text-[10px]">
                      <span class="text-blue-700 font-bold">({{ sub.ratio }}%)</span>
                      <span class="text-slate-500 font-medium">Thang {{ sub.maxScore || 10 }}</span>
                    </div>
                    <span v-if="sub.minPassScore" class="text-[9px] bg-amber-100 text-amber-900 font-extrabold px-1 rounded">
                      â‰¥ {{ sub.minPassScore }}Ä‘ Ä‘áº¡t
                    </span>
                  </div>
                </th>
                <th
                  class="px-3 py-2 text-center font-bold border-r-2"
                  :class="[
                    cIdx === 0 ? 'bg-emerald-50/80 text-emerald-900 border-emerald-300' :
                    cIdx === 1 ? 'bg-sky-50/80 text-sky-900 border-sky-300' :
                    'bg-indigo-50/80 text-indigo-900 border-indigo-300'
                  ]"
                >
                  Ä.{{ cat.code || ('TP' + (cIdx + 1)) }}
                </th>
              </template>

              <th class="px-4 py-2 text-center bg-slate-100 font-black text-slate-900">
                (Äiá»ƒm 10)
              </th>
              <th class="px-4 py-2 text-center">Tráº¡ng thÃ¡i</th>
            </tr>
          </thead>

          <tbody class="divide-y divide-slate-100">
            <tr
              v-for="st in filteredStudents"
              :key="st.id"
              class="hover:bg-slate-50/80 transition"
            >
              <!-- Student Info -->
              <td class="px-4 py-3 font-bold text-slate-900 sticky left-0 bg-white border-r border-slate-200">
                {{ st.name }}
              </td>
              <td class="px-3 py-3 font-mono text-slate-600 border-r border-slate-200">
                {{ st.mssv }}
              </td>

              <!-- Dynamic Category Scores -->
              <template v-for="(cat, cIdx) in categories" :key="'st_cat_' + (cat.id || cIdx)">
                <td
                  v-for="sub in cat.subItems"
                  :key="sub.id"
                  class="px-2 py-2 text-center border-r border-slate-100"
                >
                  <div class="relative flex items-center justify-center">
                    <input
                      v-model.number="st.scores[sub.id]"
                      type="number"
                      step="0.1"
                      min="0"
                      :max="sub.maxScore || 10"
                      class="w-16 px-1.5 py-1 text-center border rounded-lg font-semibold outline-none focus:ring-2 focus:ring-blue-500 transition"
                      :class="sub.minPassScore && st.scores[sub.id] < sub.minPassScore ? 'bg-rose-50 border-rose-300 text-rose-800 font-extrabold' : 'bg-white border-slate-300 text-slate-800'"
                    />
                  </div>
                </td>
                <td
                  class="px-3 py-3 text-center font-bold border-r-2"
                  :class="[
                    cIdx === 0 ? 'bg-emerald-50/50 text-emerald-900 border-emerald-300' :
                    cIdx === 1 ? 'bg-sky-50/50 text-sky-900 border-sky-300' :
                    'bg-indigo-50/50 text-indigo-900 border-indigo-300'
                  ]"
                >
                  {{ getCategoryScore(st, cat) }}
                </td>
              </template>

              <!-- Final Calculated Score -->
              <td class="px-4 py-3 text-center bg-slate-100/70 font-black text-slate-900 text-sm">
                {{ getFinalScore(st) }}
              </td>

              <!-- Status Tag -->
              <td class="px-4 py-3 text-center">
                <span
                  class="px-2.5 py-0.5 rounded-full text-[10px] font-extrabold"
                  :class="getFinalScore(st) >= 4.0 ? 'bg-emerald-100 text-emerald-800' : 'bg-rose-100 text-rose-800'"
                >
                  {{ getFinalScore(st) >= 4.0 ? 'Äáº T' : 'KHÃ”NG Äáº T' }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Modal: Cáº¥u hÃ¬nh nhanh Ä‘iá»ƒm Sub Item tá»« Sá»• Ä‘iá»ƒm -->
    <div
      v-if="editingSubItem"
      class="fixed inset-0 z-[120] bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-4"
      @click.self="editingSubItem = null"
    >
      <div class="bg-white rounded-2xl shadow-2xl max-w-lg w-full overflow-hidden border border-slate-200 font-sans">
        <div class="px-6 py-4 bg-[#005EA3] text-white flex justify-between items-center">
          <div>
            <h3 class="font-bold text-base">Cáº¥u hÃ¬nh Ä‘iá»ƒm: {{ editingSubItem.name }}</h3>
            <p class="text-[11px] text-blue-100">Äiá»u chá»‰nh thang Ä‘iá»ƒm, Ä‘iá»ƒm liá»‡t vÃ  tá»· lá»‡ trá»±c tiáº¿p</p>
          </div>
          <button @click="editingSubItem = null" class="text-white/80 hover:text-white font-bold text-lg">âœ•</button>
        </div>

        <div class="p-6 space-y-4 text-xs">
          <div>
            <label class="block font-bold text-slate-700 mb-1">TÃªn cá»™t Sub Item</label>
            <input v-model="editingSubItem.name" type="text" class="w-full px-3 py-2 border rounded-xl font-semibold" />
          </div>

          <div class="grid grid-cols-3 gap-3">
            <div>
              <label class="block font-bold text-slate-700 mb-1">Thang Ä‘iá»ƒm</label>
              <input v-model.number="editingSubItem.maxScore" type="number" min="1" max="100" class="w-full px-3 py-2 border rounded-xl font-bold" />
            </div>
            <div>
              <label class="block font-bold text-slate-700 mb-1">Tá»· lá»‡ % nhÃ³m</label>
              <input v-model.number="editingSubItem.ratio" type="number" min="0" max="100" class="w-full px-3 py-2 border rounded-xl font-bold text-blue-800" />
            </div>
            <div>
              <label class="block font-bold text-slate-700 mb-1">Äiá»ƒm liá»‡t (â‰¥)</label>
              <input v-model.number="editingSubItem.minPassScore" type="number" step="0.5" min="0" max="10" class="w-full px-3 py-2 border rounded-xl font-bold text-rose-700" />
            </div>
          </div>

          <div>
            <label class="block font-bold text-slate-700 mb-1">PhÆ°Æ¡ng thá»©c cháº¥m Ä‘iá»ƒm</label>
            <select v-model="editingSubItem.gradingMethod" class="w-full px-3 py-2 border rounded-xl font-semibold">
              <option value="direct">Nháº­p Ä‘iá»ƒm trá»±c tiáº¿p</option>
              <option value="rubric">Cháº¥m theo Rubric tiÃªu chÃ­</option>
              <option value="submission">Tá»± Ä‘á»™ng tá»« BÃ i ná»™p Sá»± kiá»‡n</option>
              <option value="diary">Tá»± Ä‘á»™ng tá»« Sá»• Nháº­t kÃ½</option>
              <option value="attendance">Tá»± Ä‘á»™ng tá»« Äiá»ƒm danh</option>
            </select>
          </div>
        </div>

        <div class="px-6 py-3 bg-slate-50 border-t border-slate-200 flex justify-end gap-2">
          <button @click="editingSubItem = null" class="px-4 py-2 border border-slate-300 font-bold rounded-xl text-xs">Há»§y</button>
          <button @click="saveSubConfig" class="px-5 py-2 bg-[#005EA3] text-white font-bold rounded-xl text-xs shadow-xs">Cáº­p nháº­t</button>
        </div>
      </div>
    </div>

    <!-- Modal Xuáº¥t bÃ¡o cÃ¡o Ä‘iá»ƒm (Excel & PDF) -->
    <div
      v-if="showExportModal"
      class="fixed inset-0 z-[100] bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-4"
    >
      <div class="bg-white rounded-3xl max-w-lg w-full overflow-hidden shadow-2xl border border-slate-100">
        <div class="p-6 bg-gradient-to-br from-slate-900 to-blue-950 text-white flex items-center justify-between">
          <div class="flex items-center gap-3">
            <div class="w-10 h-10 rounded-2xl bg-white/10 flex items-center justify-center font-bold">
              <span class="material-symbols-outlined text-[24px] text-emerald-400">output</span>
            </div>
            <div>
              <h3 class="font-bold text-base">Xuáº¥t BÃ¡o cÃ¡o & Báº£ng Ä‘iá»ƒm Thá»±c táº­p</h3>
              <p class="text-xs text-slate-300">Xuáº¥t dá»¯ liá»‡u chÃ­nh thá»©c cho Lá»›p {{ classId }}</p>
            </div>
          </div>
          <button @click="showExportModal = false" class="text-slate-400 hover:text-white transition">
            <span class="material-symbols-outlined text-[20px]">close</span>
          </button>
        </div>

        <div class="p-6 space-y-5 text-xs text-slate-700">
          <div class="bg-blue-50 p-4 rounded-2xl border border-blue-100 flex items-start gap-3">
            <span class="material-symbols-outlined text-blue-700 text-[20px] shrink-0 mt-0.5">info</span>
            <div class="space-y-1">
              <p class="font-bold text-blue-900">BÃ¡o cÃ¡o tá»•ng há»£p bao gá»“m:</p>
              <ul class="list-disc list-inside text-slate-600 space-y-0.5">
                <li>Danh sÃ¡ch sinh viÃªn, MSSV vÃ  cÃ¡c cá»™t Ä‘iá»ƒm QuÃ¡ trÃ¬nh, Giá»¯a ká»³, Cuá»‘i ká»³.</li>
                <li>Tá»•ng Ä‘iá»ƒm há»‡ 10, quy Ä‘á»•i Ä‘iá»ƒm chá»¯ (A, B+, B, C, D, F) vÃ  ÄÃ¡nh giÃ¡ Äáº¡t/KhÃ´ng Ä‘áº¡t.</li>
                <li>Thá»‘ng kÃª tá»· lá»‡ Äáº¡t/KhÃ´ng Ä‘áº¡t vÃ  khung kÃ½ xÃ¡c nháº­n cá»§a Giáº£ng viÃªn & Bá»™ mÃ´n.</li>
              </ul>
            </div>
          </div>

          <div class="grid grid-cols-2 gap-3">
            <div class="p-3 bg-slate-50 rounded-xl border border-slate-200 text-center">
              <span class="text-[11px] font-bold text-slate-500 uppercase block">Tá»•ng sinh viÃªn</span>
              <span class="text-xl font-black text-slate-900">{{ filteredStudents.length }}</span>
            </div>
            <div class="p-3 bg-emerald-50 rounded-xl border border-emerald-200 text-center">
              <span class="text-[11px] font-bold text-emerald-800 uppercase block">Tá»· lá»‡ Äáº¡t (Dá»± kiáº¿n)</span>
              <span class="text-xl font-black text-emerald-700">
                {{ Math.round((filteredStudents.filter(st => parseFloat(getFinalScore(st)) >= 4.0).length / (filteredStudents.length || 1)) * 100) }}%
              </span>
            </div>
          </div>

          <div class="space-y-2 pt-2">
            <label class="font-bold text-slate-800 block">Chá»n Ä‘á»‹nh dáº¡ng xuáº¥t file:</label>
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <button
                @click="exportExcel"
                class="p-4 rounded-2xl border-2 border-emerald-500/30 bg-emerald-50/50 hover:bg-emerald-100/80 transition-all flex flex-col items-center justify-center gap-2 text-emerald-900 font-bold group"
              >
                <div class="w-10 h-10 rounded-xl bg-emerald-600 text-white flex items-center justify-center group-hover:scale-105 transition-transform shadow-xs">
                  <span class="material-symbols-outlined text-[22px]">table_chart</span>
                </div>
                <span>Táº£i file Excel (.xlsx)</span>
                <span class="text-[10px] text-emerald-700 font-normal">Äáº§y Ä‘á»§ cÃ´ng thá»©c & dá»¯ liá»‡u thÃ´</span>
              </button>

              <button
                @click="exportPdf"
                class="p-4 rounded-2xl border-2 border-rose-500/30 bg-rose-50/50 hover:bg-rose-100/80 transition-all flex flex-col items-center justify-center gap-2 text-rose-900 font-bold group"
              >
                <div class="w-10 h-10 rounded-xl bg-rose-600 text-white flex items-center justify-center group-hover:scale-105 transition-transform shadow-xs">
                  <span class="material-symbols-outlined text-[22px]">picture_as_pdf</span>
                </div>
                <span>In / Táº£i PDF (.pdf)</span>
                <span class="text-[10px] text-rose-700 font-normal">Chuáº©n máº«u vÄƒn báº£n A4 cÃ³ khung kÃ½</span>
              </button>
            </div>
          </div>
        </div>

        <div class="px-6 py-4 bg-slate-50 border-t border-slate-200 flex justify-end">
          <button @click="showExportModal = false" class="px-5 py-2.5 border border-slate-300 font-bold rounded-xl text-xs hover:bg-slate-100 transition">
            ÄÃ³ng
          </button>
        </div>
      </div>
    </div>

    <!-- Toast Notification -->
    <div
      v-if="toastMsg"
      class="fixed bottom-6 right-6 z-[120] bg-slate-900 text-white px-4 py-3 rounded-xl shadow-xl text-xs font-bold flex items-center gap-2"
    >
      <span class="material-symbols-outlined text-emerald-400 text-[18px]">check_circle</span>
      {{ toastMsg }}
    </div>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted, onUnmounted } from 'vue';
  import { useRoute } from 'vue-router';
  import * as XLSX from 'xlsx';
  import api from '@/api/api';

  const route = useRoute();
  const classId = computed(() => route.params.id || '1');

  const searchQuery = ref('');
  const locked = ref(false);
  const toastMsg = ref('');
  const editingSubItem = ref(null);
  const showExportModal = ref(false);

  // 3 Category Configuration aligned with ClassSettings.vue
  const categories = ref([
    {
      id: 'cat_qt',
      code: 'QT',
      name: 'ÄÃ¡nh giÃ¡ QuÃ¡ trÃ¬nh',
      weight: 20,
      borderColor: 'border-emerald-200',
      textColor: 'text-emerald-700',
      badgeBg: 'bg-emerald-100 text-emerald-800',
      subItems: [
        { id: 's1_1', name: 'ChuyÃªn cáº§n', maxScore: 10, ratio: 40, minPassScore: 4.0, gradingMethod: 'attendance' },
        { id: 's1_2', name: 'Nháº­t kÃ½ thá»±c táº­p', maxScore: 10, ratio: 60, minPassScore: 5.0, gradingMethod: 'diary' }
      ]
    },
    {
      id: 'cat_gk',
      code: 'GK',
      name: 'ÄÃ¡nh giÃ¡ Giá»¯a ká»³',
      weight: 30,
      borderColor: 'border-sky-200',
      textColor: 'text-sky-700',
      badgeBg: 'bg-sky-100 text-sky-800',
      subItems: [
        { id: 's2_1', name: 'BÃ¡o cÃ¡o tiáº¿n Ä‘á»™ (GV)', maxScore: 10, ratio: 50, minPassScore: 4.0, gradingMethod: 'submission' },
        { id: 's2_2', name: 'ÄÃ¡nh giÃ¡ Doanh nghiá»‡p (GK)', maxScore: 10, ratio: 50, minPassScore: 4.0, gradingMethod: 'direct' }
      ]
    },
    {
      id: 'cat_ck',
      code: 'CK',
      name: 'ÄÃ¡nh giÃ¡ Cuá»‘i ká»³',
      weight: 50,
      borderColor: 'border-indigo-200',
      textColor: 'text-indigo-700',
      badgeBg: 'bg-indigo-100 text-indigo-800',
      subItems: [
        { id: 's3_1', name: 'BÃ¡o cÃ¡o / KhÃ³a luáº­n', maxScore: 10, ratio: 40, minPassScore: 4.0, gradingMethod: 'rubric' },
        { id: 's3_2', name: 'ÄÃ¡nh giÃ¡ Doanh nghiá»‡p (CK)', maxScore: 10, ratio: 40, minPassScore: 5.0, gradingMethod: 'direct' },
        { id: 's3_3', name: 'Báº£o vá»‡ / BÃ¡o cÃ¡o', maxScore: 10, ratio: 20, minPassScore: 4.0, gradingMethod: 'rubric' }
      ]
    }
  ]);

  function ensureStudentScores() {
    students.value.forEach(st => {
      if (!st.scores) st.scores = {};
      categories.value.forEach((cat, cIdx) => {
        (cat.subItems || []).forEach((sub, sIdx) => {
          if (st.scores[sub.id] === undefined) {
            st.scores[sub.id] = null;
          }
        });
      });
    });
  }

  function loadSavedStructure() {
    try {
      const saved = localStorage.getItem(`ims_grading_structure_${classId.value}`) || localStorage.getItem(`ims_grading_structure_global`);
      if (saved) {
        const parsed = JSON.parse(saved);
        if (Array.isArray(parsed) && parsed.length > 0) {
          categories.value = parsed.map((grp, idx) => ({
            id: grp.id || (idx === 0 ? 'cat_qt' : idx === 1 ? 'cat_gk' : 'cat_ck'),
            code: grp.code || (idx === 0 ? 'QT' : idx === 1 ? 'GK' : 'CK'),
            name: grp.name ? grp.name.replace(/^\d+\.\s*/, '') : (idx === 0 ? 'ÄÃ¡nh giÃ¡ QuÃ¡ trÃ¬nh' : idx === 1 ? 'ÄÃ¡nh giÃ¡ Giá»¯a ká»³' : 'ÄÃ¡nh giÃ¡ Cuá»‘i ká»³'),
            weight: grp.weight || (idx === 0 ? 20 : idx === 1 ? 30 : 50),
            borderColor: idx === 0 ? 'border-emerald-200' : idx === 1 ? 'border-sky-200' : 'border-indigo-200',
            textColor: idx === 0 ? 'text-emerald-700' : idx === 1 ? 'text-sky-700' : 'text-indigo-700',
            badgeBg: idx === 0 ? 'bg-emerald-100 text-emerald-800' : idx === 1 ? 'bg-sky-100 text-sky-800' : 'bg-indigo-100 text-indigo-800',
            subItems: grp.subItems || []
          }));
          ensureStudentScores();
        }
      }
    } catch (e) {
      console.error('Failed to load saved structure in ClassGrading', e);
    }
  }

  function handleStructureUpdated(e) {
    if (e.detail && e.detail.groups) {
      const parsed = e.detail.groups;
      categories.value = parsed.map((grp, idx) => ({
        id: grp.id || (idx === 0 ? 'cat_qt' : idx === 1 ? 'cat_gk' : 'cat_ck'),
        code: grp.code || (idx === 0 ? 'QT' : idx === 1 ? 'GK' : 'CK'),
        name: grp.name ? grp.name.replace(/^\d+\.\s*/, '') : (idx === 0 ? 'ÄÃ¡nh giÃ¡ QuÃ¡ trÃ¬nh' : idx === 1 ? 'ÄÃ¡nh giÃ¡ Giá»¯a ká»³' : 'ÄÃ¡nh giÃ¡ Cuá»‘i ká»³'),
        weight: grp.weight || (idx === 0 ? 20 : idx === 1 ? 30 : 50),
        borderColor: idx === 0 ? 'border-emerald-200' : idx === 1 ? 'border-sky-200' : 'border-indigo-200',
        textColor: idx === 0 ? 'text-emerald-700' : idx === 1 ? 'text-sky-700' : 'text-indigo-700',
        badgeBg: idx === 0 ? 'bg-emerald-100 text-emerald-800' : idx === 1 ? 'bg-sky-100 text-sky-800' : 'bg-indigo-100 text-indigo-800',
        subItems: grp.subItems || []
      }));
      ensureStudentScores();
      showToast('ÄÃ£ tá»± Ä‘á»™ng cáº­p nháº­t Sá»• Ä‘iá»ƒm theo cÆ¡ cáº¥u trá»ng sá»‘ má»›i!');
    }
  }

  const loading = ref(true);

  async function loadGrades() {
    try {
      loading.value = true;
      const res = await api.get(`/giangvien/classes/${classId.value}/grades`);
      students.value = res.data.map(item => ({
        id: item.maSinhVien,
        name: item.hoTen,
        mssv: item.maSoSinhVien,
        scores: item.scores || {}
      }));
      ensureStudentScores();
    } catch (err) {
      console.error(err);
      showToast('KhÃ´ng thá»ƒ táº£i báº£ng Ä‘iá»ƒm tá»« há»‡ thá»‘ng');
    } finally {
      loading.value = false;
    }
  }

  onMounted(() => {
    loadSavedStructure();
    window.addEventListener('grading-structure-updated', handleStructureUpdated);
    loadGrades();
  });

  onUnmounted(() => {
    window.removeEventListener('grading-structure-updated', handleStructureUpdated);
  });

  function openSubConfig(sub) {
    editingSubItem.value = JSON.parse(JSON.stringify(sub));
  }

  function saveSubConfig() {
    if (!editingSubItem.value) return;
    for (const cat of categories.value) {
      const idx = cat.subItems.findIndex(s => s.id === editingSubItem.value.id);
      if (idx !== -1) {
        cat.subItems[idx] = { ...editingSubItem.value };
        break;
      }
    }
    editingSubItem.value = null;
    const dataStr = JSON.stringify(categories.value);
    localStorage.setItem(`ims_grading_structure_${classId.value}`, dataStr);
    localStorage.setItem(`ims_grading_structure_global`, dataStr);
    window.dispatchEvent(new CustomEvent('grading-structure-updated', {
      detail: { classId: classId.value, groups: categories.value }
    }));
    showToast('ÄÃ£ cáº­p nháº­t cáº¥u hÃ¬nh Ä‘iá»ƒm Sub Item vÃ  tá»± Ä‘á»™ng Ä‘á»“ng bá»™!');
  }

  const catQT = computed(() => categories.value[0]);
  const catGK = computed(() => categories.value[1]);
  const catCK = computed(() => categories.value[2]);

  const students = ref([]);

  const filteredStudents = computed(() => {
    if (!searchQuery.value.trim()) return students.value;
    const q = searchQuery.value.toLowerCase();
    return students.value.filter(
      st => st.name.toLowerCase().includes(q) || st.mssv.toLowerCase().includes(q)
    );
  });

  // Calculate score for 1 category (0 - 10)
  function getCategoryScore(student, category) {
    let sum = 0;
    let ratioSum = 0;
    for (const sub of category.subItems) {
      const val = student.scores[sub.id];
      if (val !== undefined && val !== null && val !== '') {
        sum += val * (sub.ratio / 100);
        ratioSum += sub.ratio;
      }
    }
    if (ratioSum === 0) return '-';
    // Normalize to 10
    const catScore = (sum / (ratioSum / 100));
    return catScore.toFixed(1);
  }

  // Calculate final weighted total grade (0 - 10)
  function getFinalScore(student) {
    let total = 0;
    let weightSum = 0;

    for (const cat of categories.value) {
      const catScoreStr = getCategoryScore(student, cat);
      const catScore = parseFloat(catScoreStr);
      if (!isNaN(catScore)) {
        total += catScore * ((cat.weight || 0) / 100);
        weightSum += (cat.weight || 0);
      }
    }

    if (weightSum === 0) return '-';
    return (total / (weightSum / 100)).toFixed(2);
  }

  function getLetterGrade(score) {
    const num = parseFloat(score);
    if (isNaN(num)) return '-';
    if (num >= 8.5) return 'A';
    if (num >= 7.8) return 'B+';
    if (num >= 7.0) return 'B';
    if (num >= 6.3) return 'C+';
    if (num >= 5.5) return 'C';
    if (num >= 4.8) return 'D+';
    if (num >= 4.0) return 'D';
    return 'F';
  }

  function getPassStatus(score) {
    const num = parseFloat(score);
    if (isNaN(num)) return '-';
    return num >= 4.0 ? 'Äáº¡t' : 'KhÃ´ng Ä‘áº¡t';
  }

  function autoCalculate() {
    showToast('ÄÃ£ tÃ­nh toÃ¡n láº¡i toÃ n bá»™ Ä‘iá»ƒm theo tá»· lá»‡ trá»ng sá»‘!');
  }

  async function saveScores() {
    try {
      const grades = students.value.map(st => ({
        maSinhVien: st.id,
        scores: st.scores
      }));
      await api.post(`/giangvien/classes/${classId.value}/grades`, { grades });
      showToast('ÄÃ£ lÆ°u toÃ n bá»™ thÃ´ng tin báº£ng Ä‘iá»ƒm thÃ nh cÃ´ng!');
    } catch (err) {
      console.error(err);
      showToast('CÃ³ lá»—i xáº£y ra khi lÆ°u báº£ng Ä‘iá»ƒm!');
    }
  }

  function exportExcel() {
    const data = [
      ["TRÆ¯á»œNG Äáº I Há»ŒC KINH Táº¾ - Äáº I Há»ŒC ÄÃ€ Náº´NG"],
      ["KHOA CÃ”NG NGHá»† THÃ”NG TIN - Bá»˜ MÃ”N Há»† THá»NG THÃ”NG TIN"],
      ["Báº¢NG ÄIá»‚M Tá»”NG Há»¢P THá»°C Táº¬P DOANH NGHIá»†P"],
      [`MÃ£ lá»›p thá»±c táº­p: Lá»šP ${classId.value} | Há»c ká»³ I - NÄƒm há»c 2025-2026`],
      [`Giáº£ng viÃªn hÆ°á»›ng dáº«n: TS. Nguyá»…n VÄƒn A`],
      [""],
      ["STT", "MSSV", "Há» vÃ  tÃªn", "Ä.QuÃ¡ trÃ¬nh (20%)", "Ä.Giá»¯a ká»³ (30%)", "Ä.Cuá»‘i ká»³ (50%)", "Tá»•ng Ä‘iá»ƒm (Thang 10)", "Äiá»ƒm chá»¯", "ÄÃ¡nh giÃ¡"]
    ];

    filteredStudents.value.forEach((st, idx) => {
      const qtScore = getCategoryScore(st, categories.value[0]);
      const gkScore = getCategoryScore(st, categories.value[1]);
      const ckScore = getCategoryScore(st, categories.value[2]);
      const finalScore = getFinalScore(st);
      const letterGrade = getLetterGrade(finalScore);
      const passStatus = getPassStatus(finalScore);

      data.push([
        idx + 1,
        st.mssv,
        st.name,
        qtScore,
        gkScore,
        ckScore,
        finalScore,
        letterGrade,
        passStatus
      ]);
    });

    const passedCount = filteredStudents.value.filter(st => parseFloat(getFinalScore(st)) >= 4.0).length;
    const passRate = ((passedCount / (filteredStudents.value.length || 1)) * 100).toFixed(1);

    data.push([""]);
    data.push(["Tá»”NG Há»¢P:", `Tá»•ng SV: ${filteredStudents.value.length}`, `Sá»‘ SV Äáº¡t: ${passedCount}`, `Tá»· lá»‡ Äáº¡t: ${passRate}%`]);
    data.push(["", "", "", "", "", "", "NgÃ y xuáº¥t file:", new Date().toLocaleDateString('vi-VN')]);

    const ws = XLSX.utils.aoa_to_sheet(data);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "BangDiemThucTap");
    XLSX.writeFile(wb, `BaoCao_BangDiem_ThucTap_Lop_${classId.value}.xlsx`);

    showToast('âœ“ ÄÃ£ xuáº¥t thÃ nh cÃ´ng file Excel Báº£ng Ä‘iá»ƒm Thá»±c táº­p (.xlsx)');
    showExportModal.value = false;
  }

  function exportPdf() {
    const printWindow = window.open('', '_blank');
    if (!printWindow) {
      showToast('Vui lÃ²ng cho phÃ©p trÃ¬nh duyá»‡t báº­t popup Ä‘á»ƒ má»Ÿ báº£n In PDF');
      return;
    }

    const rowsHtml = filteredStudents.value.map((st, idx) => {
      const qtScore = getCategoryScore(st, categories.value[0]);
      const gkScore = getCategoryScore(st, categories.value[1]);
      const ckScore = getCategoryScore(st, categories.value[2]);
      const finalScore = getFinalScore(st);
      const letterGrade = getLetterGrade(finalScore);
      const passStatus = getPassStatus(finalScore);
      const isPassed = parseFloat(finalScore) >= 4.0;

      return `
        <tr>
          <td style="text-align: center; padding: 6px;">${idx + 1}</td>
          <td style="text-align: center; font-family: monospace; font-weight: bold; padding: 6px;">${st.mssv}</td>
          <td style="padding: 6px; font-weight: 600;">${st.name}</td>
          <td style="text-align: center; padding: 6px;">${qtScore}</td>
          <td style="text-align: center; padding: 6px;">${gkScore}</td>
          <td style="text-align: center; padding: 6px;">${ckScore}</td>
          <td style="text-align: center; font-weight: bold; padding: 6px; font-size: 13px;">${finalScore}</td>
          <td style="text-align: center; font-weight: bold; padding: 6px;">${letterGrade}</td>
          <td style="text-align: center; font-weight: bold; padding: 6px; color: ${isPassed ? '#047857' : '#b91c1c'};">${passStatus}</td>
        </tr>
      `;
    }).join('');

    const passedCount = filteredStudents.value.filter(st => parseFloat(getFinalScore(st)) >= 4.0).length;
    const passRate = ((passedCount / (filteredStudents.value.length || 1)) * 100).toFixed(1);

    printWindow.document.write(`
      <!DOCTYPE html>
      <html>
      <head>
        <title>BÃ¡o cÃ¡o Báº£ng Ä‘iá»ƒm Thá»±c táº­p - Lá»›p ${classId.value}</title>
        <meta charset="utf-8" />
        <style>
          body { font-family: "Times New Roman", Times, serif; margin: 20mm 15mm; color: #111; line-height: 1.3; }
          .header-table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }
          .header-table td { vertical-align: top; }
          .title { text-align: center; text-transform: uppercase; font-size: 16px; font-weight: bold; margin: 15px 0 5px 0; }
          .subtitle { text-align: center; font-size: 13px; font-style: italic; margin-bottom: 20px; }
          .data-table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 12px; }
          .data-table th, .data-table td { border: 1px solid #333; }
          .data-table th { background-color: #f0f0f0; font-weight: bold; text-align: center; padding: 8px 4px; }
          .stats-box { margin-top: 20px; border: 1px solid #ddd; padding: 10px; font-size: 12px; background-color: #fafafa; }
          .signature-section { margin-top: 40px; width: 100%; text-align: center; font-size: 12px; }
          .signature-section td { width: 50%; vertical-align: top; }
          @media print {
            body { margin: 0; }
            .no-print { display: none; }
          }
        </style>
      </head>
      <body>
        <button class="no-print" onclick="window.print()" style="position: fixed; top: 15px; right: 15px; padding: 10px 20px; background: #005EA3; color: white; font-weight: bold; border: none; border-radius: 6px; cursor: pointer;">
          ðŸ–¨ï¸ In / Táº£i PDF
        </button>

        <table class="header-table">
          <tr>
            <td style="text-align: center; width: 45%;">
              <strong>Äáº I Há»ŒC ÄÃ€ Náº´NG</strong><br/>
              <strong>TRÆ¯á»œNG Äáº I Há»ŒC KINH Táº¾</strong><br/>
              --------------------
            </td>
            <td style="text-align: center; width: 55%;">
              <strong>Cá»˜NG HÃ’A XÃƒ Há»˜I CHá»¦ NGHÄ¨A VIá»†T NAM</strong><br/>
              <strong>Äá»™c láº­p - Tá»± do - Háº¡nh phÃºc</strong><br/>
              --------------------
            </td>
          </tr>
        </table>

        <div class="title">Báº¢NG ÄIá»‚M Tá»”NG Há»¢P THá»°C Táº¬P DOANH NGHIá»†P</div>
        <div class="subtitle">Lá»›p hÆ°á»›ng dáº«n: <strong>Lá»šP ${classId.value}</strong> â€¢ Há»c ká»³ I - NÄƒm há»c 2025-2026</div>

        <table class="data-table">
          <thead>
            <tr>
              <th style="width: 35px;">STT</th>
              <th style="width: 80px;">MSSV</th>
              <th>Há» vÃ  tÃªn sinh viÃªn</th>
              <th style="width: 80px;">QuÃ¡ trÃ¬nh<br/>(20%)</th>
              <th style="width: 80px;">Giá»¯a ká»³<br/>(30%)</th>
              <th style="width: 80px;">Cuá»‘i ká»³<br/>(50%)</th>
              <th style="width: 90px;">Tá»•ng Ä‘iá»ƒm<br/>(Há»‡ 10)</th>
              <th style="width: 60px;">Äiá»ƒm chá»¯</th>
              <th style="width: 80px;">ÄÃ¡nh giÃ¡</th>
            </tr>
          </thead>
          <tbody>
            ${rowsHtml}
          </tbody>
        </table>

        <div class="stats-box">
          <strong>Tá»”NG Há»¢P TÃŒNH HÃŒNH Lá»šP:</strong><br/>
          - Tá»•ng sá»‘ sinh viÃªn Ä‘Ã¡nh giÃ¡: <strong>${filteredStudents.value.length}</strong> sinh viÃªn<br/>
          - Sá»‘ sinh viÃªn Äáº T (â‰¥ 4.0): <strong>${passedCount}</strong> / ${filteredStudents.value.length} (${passRate}%)<br/>
          - Sá»‘ sinh viÃªn KHÃ”NG Äáº T (&lt; 4.0): <strong>${filteredStudents.value.length - passedCount}</strong> sinh viÃªn
        </div>

        <table class="signature-section">
          <tr>
            <td>
              <br/>
              <strong>TRÆ¯á»œNG Bá»˜ MÃ”N</strong><br/>
              <span style="font-style: italic; font-size: 11px;">(KÃ½ vÃ  ghi rÃµ há» tÃªn)</span>
              <br/><br/><br/><br/>
            </td>
            <td>
              <em>ÄÃ  Náºµng, ngÃ y ${new Date().getDate()} thÃ¡ng ${new Date().getMonth() + 1} nÄƒm ${new Date().getFullYear()}</em><br/>
              <strong>GIáº¢NG VIÃŠN HÆ¯á»šNG DáºªN</strong><br/>
              <span style="font-style: italic; font-size: 11px;">(KÃ½ vÃ  ghi rÃµ há» tÃªn)</span>
              <br/><br/><br/><br/>
              <strong>TS. Nguyá»…n VÄƒn A</strong>
            </td>
          </tr>
        </table>
      </body>
      </html>
    `);
    printWindow.document.close();
    showExportModal.value = false;
    showToast('ÄÃ£ má»Ÿ giao diá»‡n In / Xuáº¥t bÃ¡o cÃ¡o PDF thÃ nh cÃ´ng!');
  }

  function showToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }
</script>
