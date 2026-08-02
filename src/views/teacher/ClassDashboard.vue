<template>
  <div class="ims-scope w-full mx-auto space-y-8 pb-12 font-sans">
    <div class="flex justify-between items-end">
      <div>
        <nav class="flex items-center text-xs text-slate-500 mb-1 gap-2 font-medium">
          <router-link to="/teacher/classes" class="hover:text-blue-600 transition-colors">L�:p của tôi</router-link>
          <span class="material-symbols-outlined text-[14px]">chevron_right</span>
          <span class="text-slate-900 font-bold">{{ classInfo?.tenLop || 'L�:p Thá»±c táº­p K64' }}</span>
          <span class="material-symbols-outlined text-[14px]">chevron_right</span>
          <span class="text-slate-800 font-bold">Dashboard</span>
        </nav>
        <h1 class="text-3xl font-extrabold text-slate-900 tracking-tight">Tá»•ng quan tiáº¿n Ä‘á»™ lá»›p</h1>
        <p class="text-xs text-slate-500 mt-1">{{ classInfo?.tenKy || 'Há»c ká»³ 1 - 2024 â€¢ DUE' }}</p>
      </div>

      <div class="flex items-center gap-3">
        <router-link
          :to="`/teacher/classes/${maLop}/students`"
          class="px-4 py-2 bg-blue-50 text-[#005EA3] hover:bg-blue-100 rounded-xl font-bold text-xs flex items-center gap-1.5 transition"
        >
          <span class="material-symbols-outlined text-[18px]">groups</span>
          Quản lý sinh viên
        </router-link>
        <router-link
          :to="`/teacher/classes/${maLop}/grading`"
          class="px-4 py-2 bg-[#005EA3] hover:bg-blue-800 text-white rounded-xl font-bold text-xs flex items-center gap-1.5 transition shadow-xs"
        >
          <span class="material-symbols-outlined text-[18px]">grade</span>
          Bảng �iỒm lá»›p
        </router-link>
      </div>
    </div>

    <section class="border border-slate-200 rounded-2xl bg-white overflow-hidden shadow-xs">
      <details class="group" :open="isChecklistOpen">
        <summary
          class="flex items-center justify-between p-5 cursor-pointer hover:bg-slate-50 transition-colors list-none select-none"
          @click.prevent="isChecklistOpen = !isChecklistOpen"
        >
          <div class="flex items-center gap-4">
            <span class="w-8 h-8 rounded-full bg-emerald-100 text-emerald-800 flex items-center justify-center font-extrabold text-xs">
              {{ completedChecklistCount }}/6
            </span>
            <span class="font-bold text-slate-900 text-base">HoÃ n táº¥t thiáº¿t láº­p lá»›p há»c (Setup Checklist)</span>
          </div>
          <span class="material-symbols-outlined text-slate-400 group-open:rotate-180 transition-transform">
            expand_more
          </span>
        </summary>

        <div class="p-6 pt-2 border-t border-slate-100 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          <div class="p-4 bg-slate-50 rounded-xl border border-slate-200 flex items-start gap-3 opacity-75">
            <span class="material-symbols-outlined text-emerald-600 text-[22px]" style="font-variation-settings: 'FILL' 1">
              check_circle
            </span>
            <div>
              <h4 class="font-bold text-slate-800 text-xs line-through">Táº¡o lá»›p há»c</h4>
              <p class="text-[11px] text-slate-500 mt-0.5">ÄÃ£ khá»Ÿi táº¡o lá»›p há»c pháº§n thÃ nh cÃ´ng.</p>
            </div>
          </div>

          <div class="p-4 bg-white rounded-xl border border-slate-200 flex items-start justify-between gap-3 hover:shadow-xs transition group">
            <div class="flex items-start gap-3">
              <span class="material-symbols-outlined text-slate-300 text-[22px]">circle</span>
              <div>
                <h4 class="font-bold text-slate-900 text-xs">Cáº¥u hÃ¬nh má»‘c tiáº¿n Ä‘á»™</h4>
                <p class="text-[11px] text-slate-500 mt-0.5">Thiáº¿t láº­p timeline thực tập.</p>
              </div>
            </div>
            <router-link :to="`/teacher/classes/${maLop}/settings`" class="text-blue-600 font-bold text-[11px] flex items-center gap-1 group-hover:translate-x-1 transition-transform shrink-0">
              Cáº¥u hÃ¬nh ngay <span class="material-symbols-outlined text-[14px]">arrow_forward</span>
            </router-link>
          </div>

          <div class="p-4 bg-white rounded-xl border border-slate-200 flex items-start justify-between gap-3 hover:shadow-xs transition group">
            <div class="flex items-start gap-3">
              <span class="material-symbols-outlined text-slate-300 text-[22px]">circle</span>
              <div>
                <h4 class="font-bold text-slate-900 text-xs">Cáº¥u hÃ¬nh nháº­t kÃ½</h4>
                <p class="text-[11px] text-slate-500 mt-0.5">Quy Ä‘á»‹nh ná»™p báo cáo tuáº§n.</p>
              </div>
            </div>
            <router-link :to="`/teacher/classes/${maLop}/diaries`" class="text-blue-600 font-bold text-[11px] flex items-center gap-1 group-hover:translate-x-1 transition-transform shrink-0">
              Cáº¥u hÃ¬nh ngay <span class="material-symbols-outlined text-[14px]">arrow_forward</span>
            </router-link>
          </div>

          <div class="p-4 bg-white rounded-xl border border-slate-200 flex items-start justify-between gap-3 hover:shadow-xs transition group">
            <div class="flex items-start gap-3">
              <span class="material-symbols-outlined text-slate-300 text-[22px]">circle</span>
              <div>
                <h4 class="font-bold text-slate-900 text-xs">Cáº¥u hÃ¬nh Ä‘iá»ƒm</h4>
                <p class="text-[11px] text-slate-500 mt-0.5">Thiáº¿t láº­p thang Ä‘iá»ƒm Ä‘Ã¡nh giÃ¡.</p>
              </div>
            </div>
            <router-link :to="`/teacher/classes/${maLop}/grading`" class="text-blue-600 font-bold text-[11px] flex items-center gap-1 group-hover:translate-x-1 transition-transform shrink-0">
              Cáº¥u hÃ¬nh ngay <span class="material-symbols-outlined text-[14px]">arrow_forward</span>
            </router-link>
          </div>

          <div class="p-4 bg-white rounded-xl border border-slate-200 flex items-start justify-between gap-3 hover:shadow-xs transition group">
            <div class="flex items-start gap-3">
              <span class="material-symbols-outlined text-slate-300 text-[22px]">circle</span>
              <div>
                <h4 class="font-bold text-slate-900 text-xs">Cáº¥u hÃ¬nh dashboard</h4>
                <p class="text-[11px] text-slate-500 mt-0.5">TÃ¹y chá»‰nh chá»‰ sá»‘ hiá»ƒn thá»‹.</p>
              </div>
            </div>
            <router-link :to="`/teacher/classes/${maLop}/settings`" class="text-blue-600 font-bold text-[11px] flex items-center gap-1 group-hover:translate-x-1 transition-transform shrink-0">
              Cáº¥u hÃ¬nh ngay <span class="material-symbols-outlined text-[14px]">arrow_forward</span>
            </router-link>
          </div>

          <div class="p-4 bg-white rounded-xl border border-slate-200 flex items-start justify-between gap-3 hover:shadow-xs transition group">
            <div class="flex items-start gap-3">
              <span class="material-symbols-outlined text-slate-300 text-[22px]">circle</span>
              <div>
                <h4 class="font-bold text-slate-900 text-xs">ThÃªm sinh viên</h4>
                <p class="text-[11px] text-slate-500 mt-0.5">Import danh sÃ¡ch tá»« file Excel.</p>
              </div>
            </div>
            <router-link :to="`/teacher/classes/${maLop}/students`" class="text-blue-600 font-bold text-[11px] flex items-center gap-1 group-hover:translate-x-1 transition-transform shrink-0">
              Cáº¥u hÃ¬nh ngay <span class="material-symbols-outlined text-[14px]">arrow_forward</span>
            </router-link>
          </div>
        </div>
      </details>
    </section>

    <div class="grid grid-cols-12 gap-6">
      <div class="col-span-12 lg:col-span-2 bg-white rounded-2xl border border-slate-200 p-5 shadow-xs flex flex-col justify-between">
        <h3 class="text-xs font-bold uppercase tracking-wider text-slate-400 mb-4">Lá»™ trÃ¬nh thực tập</h3>
        <div class="relative flex flex-col items-center h-full min-h-[320px]">
          <div class="absolute top-3 bottom-3 w-0.5 bg-slate-200"></div>
          
          <div class="relative z-10 flex flex-col justify-between h-full py-2 w-full space-y-12">
            <div class="relative flex items-center gap-3">
              <div class="w-4 h-4 rounded-full bg-slate-400 border-2 border-white shadow-xs shrink-0"></div>
              <div>
                <p class="text-xs font-bold text-slate-800">ÄÄƒng kÃ½</p>
                <p class="text-[10px] text-slate-400">01/08 - 15/08</p>
              </div>
            </div>

            <div class="relative flex items-center gap-3">
              <div class="w-5 h-5 rounded-full bg-[#005EA3] border-2 border-white ring-4 ring-blue-100 shadow-sm shrink-0 animate-pulse"></div>
              <div>
                <span class="inline-flex items-center gap-1 bg-[#005EA3] text-white text-[9px] font-extrabold px-2 py-0.5 rounded-full mb-1">
                  <span class="material-symbols-outlined text-[10px]">location_on</span> Báº¡n á»Ÿ Ä‘Ã¢y
                </span>
                <p class="text-xs font-bold text-[#005EA3]">Giá»¯a ká»³</p>
                <p class="text-[10px] text-slate-500 font-medium">15/10 - 20/10</p>
              </div>
            </div>

            <div class="relative flex items-center gap-3 opacity-60">
              <div class="w-4 h-4 rounded-full bg-emerald-600 border-2 border-white shadow-xs shrink-0"></div>
              <div>
                <p class="text-xs font-bold text-emerald-800">KhÃ³a luáº­n</p>
                <p class="text-[10px] text-slate-400">20/12 - 31/12</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="col-span-12 lg:col-span-10 space-y-6">
        <div v-if="loading" class="bg-white rounded-2xl border border-slate-200 p-12 text-center flex flex-col items-center justify-center min-h-[350px]">
          <span class="animate-spin text-4xl text-blue-600 material-symbols-outlined">sync</span>
          <p class="text-sm text-slate-500 mt-2">Äang táº£i dữ li�!u tá»•ng quan...</p>
        </div>

        <template v-else>
          <section class="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div class="bg-white rounded-2xl border border-slate-200 p-5 shadow-xs flex flex-col justify-between">
              <div class="flex justify-between items-start mb-3">
                <div class="p-2.5 bg-blue-50 text-[#005EA3] rounded-xl">
                  <span class="material-symbols-outlined text-[24px]">history_edu</span>
                </div>
                <span v-if="hasRealData" class="text-emerald-700 font-extrabold text-xs bg-emerald-50 px-2.5 py-1 rounded-full">
                  Hoáº¡t Ä‘á»™ng tá»‘t
                </span>
                <span v-else class="text-slate-500 font-extrabold text-xs bg-slate-100 px-2.5 py-1 rounded-full">
                  ChÆ°a cÃ³ dữ li�!u
                </span>
              </div>
              <div>
                <p class="text-xs font-bold text-slate-500 uppercase tracking-wider">Tá»· lá»‡ ná»™p nháº­t kÃ½</p>
                <h2 class="text-3xl font-black text-slate-900 mt-1">{{ submissionRate }}%</h2>
              </div>
              <div class="w-full bg-slate-100 h-2 rounded-full overflow-hidden mt-4">
                <div class="bg-[#005EA3] h-full rounded-full transition-all duration-500" :style="{ width: submissionRate + '%' }"></div>
              </div>
            </div>

            <div class="bg-white rounded-2xl border border-slate-200 p-5 shadow-xs flex flex-col justify-between">
              <div class="flex justify-between items-start mb-3">
                <div class="p-2.5 bg-rose-50 text-rose-600 rounded-xl">
                  <span class="material-symbols-outlined text-[24px]">error_outline</span>
                </div>
                <span v-if="delayedCount > 0" class="text-rose-700 font-extrabold text-xs bg-rose-50 px-2.5 py-1 rounded-full">
                  Cáº§n nháº¯c nhá»Ÿ
                </span>
                <span v-else class="text-emerald-700 font-extrabold text-xs bg-emerald-50 px-2.5 py-1 rounded-full">
                  HoÃ n thÃ nh tá»‘t
                </span>
              </div>
              <div>
                <p class="text-xs font-bold text-slate-500 uppercase tracking-wider">Cháº­m tiáº¿n Ä‘á»™</p>
                <h2 class="text-3xl font-black text-rose-600 mt-1">{{ String(delayedCount).padStart(2, '0') }}</h2>
              </div>
              <p class="text-xs text-slate-500 mt-3 font-medium">Sinh viên chÆ°a ná»™p nháº­t kÃ½ tuáº§n há»c trÆ°á»›c.</p>
            </div>

            <div class="bg-white rounded-2xl border border-slate-200 p-5 shadow-xs flex flex-col justify-between">
              <div class="flex justify-between items-start mb-3">
                <div class="p-2.5 bg-amber-50 text-amber-600 rounded-xl">
                  <span class="material-symbols-outlined text-[24px]">calendar_month</span>
                </div>
                <span class="text-slate-600 font-extrabold text-xs bg-slate-100 px-2.5 py-1 rounded-full">
                  HÃ´m nay
                </span>
              </div>
              <div>
                <p class="text-xs font-bold text-slate-500 uppercase tracking-wider">Lá»‹ch háº¹n tuáº§n nÃ y</p>
                <h2 class="text-3xl font-black text-slate-900 mt-1">00</h2>
              </div>
              <p class="text-xs text-slate-400 mt-3 font-medium">ChÆ°a cÃ³ lá»‹ch há»p trá»±c tuyáº¿n tuáº§n nÃ y.</p>
            </div>
          </section>

          <div v-if="!hasRealData" class="bg-white rounded-2xl border border-slate-200 p-12 text-center flex flex-col items-center justify-center min-h-[380px] shadow-2xs">
            <span class="material-symbols-outlined text-[72px] text-slate-300 mb-4" style="font-variation-settings: 'FILL' 1">analytics</span>
            <h3 class="text-lg font-bold text-slate-900">ChÆ°a cÃ³ dữ li�!u thá»‘ng kÃª biá»ƒu Ä‘á»“</h3>
            <p class="text-sm text-slate-500 max-w-lg mt-2 mx-auto leading-relaxed">
              Biá»ƒu Ä‘á»“ phÃ¢n tÃ­ch và báº£ng heatmap tiáº¿n Ä‘á»™ chưa có dữ li�!u hoáº¡t Ä‘á»™ng do sinh viên chÆ°a Ä‘Äƒng kÃ½ Ä‘á» tÃ i thực tập hoặc chưa có lÆ°á»£t ná»™p nháº­t kÃ½ nào. Dá»¯ liá»‡u sáº½ tá»± Ä‘á»™ng Ä‘á»“ng bá»™ thá»i gian thá»±c ngay khi sinh viên báº¯t �ầu khai bÃ¡o Ä‘á» tÃ i và ná»™p báo cáo.
            </p>
            <div class="mt-8 flex flex-wrap gap-3 justify-center">
              <router-link
                :to="`/teacher/classes/${maLop}/students`"
                class="px-5 py-2.5 bg-blue-50 hover:bg-blue-100 text-[#005EA3] rounded-xl font-bold text-xs flex items-center gap-1.5 transition"
              >
                <span class="material-symbols-outlined text-[18px]">group_add</span>
                Danh sách & MÃ£ ghi danh lá»›p
              </router-link>
              <router-link
                :to="`/teacher/classes/${maLop}/topics`"
                class="px-5 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white rounded-xl font-bold text-xs flex items-center gap-1.5 transition shadow-xs"
              >
                <span class="material-symbols-outlined text-[18px]">assignment_turned_in</span>
                PhÃª duyá»‡t Ä‘á» tÃ i thực tập
              </router-link>
            </div>
          </div>

          <div v-else class="space-y-6">
            <section class="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div class="bg-white rounded-2xl border border-slate-200 p-6 shadow-xs">
                <h4 class="font-bold text-base text-slate-900 mb-6">Trạng thái Sinh viên</h4>
                <div class="flex items-center gap-8 justify-around">
                  <div class="relative w-32 h-32 flex items-center justify-center shrink-0">
                    <svg class="w-full h-full -rotate-90" viewBox="0 0 36 36">
                      <circle cx="18" cy="18" fill="transparent" r="15.915" stroke="#005EA3" :stroke-dasharray="`${chartSegments.approved.dash} ${100 - chartSegments.approved.dash}`" :stroke-dashoffset="chartSegments.approved.offset" stroke-width="4"></circle>
                      <circle cx="18" cy="18" fill="transparent" r="15.915" stroke="#f97316" :stroke-dasharray="`${chartSegments.pending.dash} ${100 - chartSegments.pending.dash}`" :stroke-dashoffset="chartSegments.pending.offset" stroke-width="4"></circle>
                      <circle cx="18" cy="18" fill="transparent" r="15.915" stroke="#94a3b8" :stroke-dasharray="`${chartSegments.unregistered.dash} ${100 - chartSegments.unregistered.dash}`" :stroke-dashoffset="chartSegments.unregistered.offset" stroke-width="4"></circle>
                    </svg>
                    <div class="absolute inset-0 flex flex-col items-center justify-center">
                      <span class="text-2xl font-black text-slate-900">{{ totalStudentsCount }}</span>
                      <span class="text-[9px] uppercase font-bold text-slate-400">Tá»•ng SV</span>
                    </div>
                  </div>
                  <div class="space-y-3 text-xs">
                    <div class="flex items-center gap-2">
                      <span class="w-3 h-3 bg-[#005EA3] rounded-sm shrink-0"></span>
                      <span class="font-semibold text-slate-700">Äang thực tập ({{ approvedTopicsCount }})</span>
                    </div>
                    <div class="flex items-center gap-2">
                      <span class="w-3 h-3 bg-orange-500 rounded-sm shrink-0"></span>
                      <span class="font-semibold text-slate-700">Chá» duyá»‡t Ä‘á» tÃ i ({{ pendingTopicsCount }})</span>
                    </div>
                    <div class="flex items-center gap-2">
                      <span class="w-3 h-3 bg-slate-400 rounded-sm shrink-0"></span>
                      <span class="font-semibold text-slate-700">ChÆ°a Ä‘Äƒng kÃ½ ({{ unregisteredCount }})</span>
                    </div>
                  </div>
                </div>
              </div>

              <div class="bg-white rounded-2xl border border-slate-200 p-6 shadow-xs flex flex-col justify-between">
                <div class="flex justify-between items-center mb-4">
                  <h4 class="font-bold text-base text-slate-900">Tiến ��" ná»™p nháº­t kÃ½ (Tá»· lá»‡ %)</h4>
                  <span class="text-xs font-bold text-slate-500 bg-slate-100 px-2.5 py-1 rounded-lg">ToÃ n khÃ³a (8 tuáº§n)</span>
                </div>

                <div class="h-32 w-full relative mt-2">
                  <svg class="w-full h-full" preserveAspectRatio="none" viewBox="0 0 400 100">
                    <path :d="chartAreaPath" fill="#005EA3" fill-opacity="0.1"></path>
                    <path :d="chartLinePath" fill="none" stroke="#005EA3" stroke-linecap="round" stroke-linejoin="round" stroke-width="3"></path>
                    <circle v-for="(pt, idx) in chartPoints" :key="idx" :cx="pt.x" :cy="pt.y" fill="#005EA3" r="4"></circle>
                  </svg>

                  <div class="flex justify-between mt-2 text-[10px] text-slate-500 font-semibold">
                    <span>T1</span>
                    <span>T2</span>
                    <span>T3 (Nay)</span>
                    <span>T4</span>
                    <span>T5</span>
                    <span>T6</span>
                    <span>T7</span>
                    <span>T8</span>
                  </div>
                </div>
              </div>
            </section>

            <section class="bg-white rounded-2xl border border-slate-200 p-6 shadow-xs space-y-6">
              <div class="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
                <div>
                  <h4 class="font-bold text-base text-slate-900">TÃ¬nh tráº¡ng ná»™p nháº­t kÃ½ theo tuáº§n</h4>
                  <p class="text-xs text-slate-500 mt-0.5">Thá»‘ng kÃª viá»‡c hoÃ n thÃ nh nháº­t kÃ½ thực tập của sinh viên qua cÃ¡c má»‘c tuáº§n</p>
                </div>

                <div class="flex items-center gap-4 text-[11px] font-bold">
                  <div class="flex items-center gap-1.5">
                    <span class="w-3 h-3 bg-emerald-600 rounded-xs"></span>
                    <span class="text-slate-700">ÄÃºng háº¡n</span>
                  </div>
                  <div class="flex items-center gap-1.5">
                    <span class="w-3 h-3 bg-rose-600 rounded-xs"></span>
                    <span class="text-slate-700">KhÃ´ng ná»™p</span>
                  </div>
                  <div class="flex items-center gap-1.5">
                    <span class="w-3 h-3 bg-slate-200 rounded-xs"></span>
                    <span class="text-slate-700">ChÆ°a Ä‘áº¿n háº¡n</span>
                  </div>
                </div>
              </div>

              <div class="overflow-x-auto">
                <div class="min-w-[650px]">
                  <div class="grid grid-cols-[200px_repeat(8,1fr)] mb-3 pb-2 border-b border-slate-100 text-[11px] font-bold text-slate-400 text-center uppercase tracking-wider">
                    <div class="text-left px-2">Há» và tên</div>
                    <div>T1</div>
                    <div>T2</div>
                    <div>T3</div>
                    <div>T4</div>
                    <div>T5</div>
                    <div>T6</div>
                    <div>T7</div>
                    <div>T8</div>
                  </div>

                  <div class="space-y-2.5">
                    <div
                      v-for="s in weeklyStudents"
                      :key="s.id"
                      class="grid grid-cols-[200px_repeat(8,1fr)] items-center text-xs hover:bg-slate-50/80 p-1.5 rounded-xl transition"
                    >
                      <div class="pr-4 font-bold text-slate-900 truncate">
                        {{ s.hoTen }}
                      </div>
                      <div v-for="(status, wIdx) in s.weeks" :key="wIdx" class="p-1">
                        <div
                          class="h-6 w-full rounded-md transition-transform hover:scale-105"
                          :class="statusColorClass(status)"
                          :title="`Tuáº§n ${wIdx + 1}: ${statusLabelText(status)}`"
                        ></div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </section>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue';
import { useRoute } from 'vue-router';
import api from '@/api/api';
import { listenAllTopics } from '@/services/topicService';
import { listenAllDiaries } from '@/services/diaryService';

const route = useRoute();
const maLop = computed(() => route.params.id || '101');

const classInfo = ref(null);
const isChecklistOpen = ref(true);
const loading = ref(true);

const students = ref([]);
const topics = ref([]);
const diaries = ref([]);
const currentWeek = ref(3);

let unsubscribeTopics = null;
let unsubscribeDiaries = null;

const totalStudentsCount = computed(() => students.value.length);

const approvedTopicsCount = computed(() => {
  let count = 0;
  students.value.forEach(s => {
    const mssv = s.maSoSinhVien;
    const ft = topics.value.find(item => String(item.mssv || item.userId) === String(mssv));
    if (ft && (ft.status === 'approved' || ft.status === 'ÄÃ£ duyá»‡t')) {
      count++;
    } else {
      const savedStatus = localStorage.getItem(`ims_student_topic_status_${mssv}`);
      if (savedStatus === 'approved') {
        count++;
      }
    }
  });
  return count;
});

const pendingTopicsCount = computed(() => {
  let count = 0;
  students.value.forEach(s => {
    const mssv = s.maSoSinhVien;
    const ft = topics.value.find(item => String(item.mssv || item.userId) === String(mssv));
    if (ft && (ft.status === 'pending' || ft.status === 'Chá» duyá»‡t')) {
      count++;
    } else {
      const savedStatus = localStorage.getItem(`ims_student_topic_status_${mssv}`);
      if (savedStatus === 'pending') {
        count++;
      }
    }
  });
  return count;
});

const unregisteredCount = computed(() => {
  return Math.max(0, totalStudentsCount.value - approvedTopicsCount.value - pendingTopicsCount.value);
});

const submissionRate = computed(() => {
  if (totalStudentsCount.value === 0) return 0;
  const currentW = currentWeek.value;
  const expectedCount = totalStudentsCount.value * (currentW - 1);
  if (expectedCount <= 0) return 0;

  let submittedCount = 0;
  students.value.forEach(s => {
    const mssv = s.maSoSinhVien;
    const sDiaries = diaries.value.filter(d => String(d.userId) === String(mssv) && Number(d.week) < currentW);
    submittedCount += sDiaries.length;
  });

  return Math.min(100, Math.round((submittedCount / expectedCount) * 100));
});

const delayedCount = computed(() => {
  const prevW = currentWeek.value - 1;
  if (prevW <= 0) return 0;
  let count = 0;
  students.value.forEach(s => {
    const mssv = s.maSoSinhVien;
    const hasDiary = diaries.value.some(d => String(d.userId) === String(mssv) && Number(d.week) === prevW);
    if (!hasDiary) {
      count++;
    }
  });
  return count;
});

const completedChecklistCount = computed(() => {
  let count = 1; // 1. Táº¡o lá»›p há»c (luÃ´n xong)
  if (classInfo.value) {
    if (classInfo.value.ghiDanhMo) count++;
    if (totalStudentsCount.value > 0) count++;
    if (approvedTopicsCount.value > 0) count++;
  }
  return Math.min(6, count + 1); // add offset for realistic UI
});

const hasRealData = computed(() => {
  if (totalStudentsCount.value === 0) return false;
  
  const hasTopic = students.value.some(s => {
    const mssv = s.maSoSinhVien;
    const ft = topics.value.find(item => String(item.mssv || item.userId) === String(mssv));
    const savedTitle = localStorage.getItem(`ims_student_topic_name_${mssv}`);
    return ft || savedTitle;
  });

  const hasDiary = students.value.some(s => {
    const mssv = s.maSoSinhVien;
    return diaries.value.some(d => String(d.userId) === String(mssv));
  });

  return hasTopic || hasDiary;
});

const weeklyStudents = computed(() => {
  const list = [];
  const currentW = currentWeek.value;
  
  for (const s of students.value) {
    const mssv = s.maSoSinhVien;
    const studentWeeks = [];
    
    for (let w = 1; w <= 8; w++) {
      const hasDiary = diaries.value.some(d => String(d.userId) === String(mssv) && Number(d.week) === w);
      if (hasDiary) {
        studentWeeks.push('dung_han');
      } else if (w < currentW) {
        studentWeeks.push('khong_nop');
      } else {
        studentWeeks.push('pending');
      }
    }
    
    list.push({
      id: s.maSinhVien,
      hoTen: s.hoTen,
      weeks: studentWeeks
    });
  }
  return list;
});

const chartSegments = computed(() => {
  const tot = Math.max(totalStudentsCount.value, 1);
  const appPercent = (approvedTopicsCount.value / tot) * 100;
  const penPercent = (pendingTopicsCount.value / tot) * 100;
  const unregPercent = (unregisteredCount.value / tot) * 100;

  return {
    approved: {
      dash: appPercent,
      offset: 0
    },
    pending: {
      dash: penPercent,
      offset: -appPercent
    },
    unregistered: {
      dash: unregPercent,
      offset: -(appPercent + penPercent)
    }
  };
});

const chartPoints = computed(() => {
  const tot = Math.max(totalStudentsCount.value, 1);
  return Array.from({ length: 8 }, (_, i) => {
    const w = i + 1;
    const submittedInWeek = weeklyStudents.value.filter(s => s.weeks[i] === 'dung_han').length;
    const rate = submittedInWeek / tot;
    const x = (i / 7) * 400;
    const y = 90 - (rate * 80); // fits within 10-90 height range
    return { x, y, percent: Math.round(rate * 100) };
  });
});

const chartLinePath = computed(() => {
  const pts = chartPoints.value;
  return pts.map((p, idx) => `${idx === 0 ? 'M' : 'L'} ${p.x} ${p.y}`).join(' ');
});

const chartAreaPath = computed(() => {
  const pts = chartPoints.value;
  if (!pts.length) return '';
  const linePart = pts.map((p, idx) => `${idx === 0 ? 'M' : 'L'} ${p.x} ${p.y}`).join(' ');
  return `${linePart} L 400 100 L 0 100 Z`;
});

function statusColorClass(st) {
  if (st === 'dung_han') return 'bg-emerald-600';
  if (st === 'thieu') return 'bg-orange-500';
  if (st === 'khong_nop') return 'bg-rose-600';
  return 'bg-slate-200';
}

function statusLabelText(st) {
  if (st === 'dung_han') return 'ÄÃºng háº¡n';
  if (st === 'thieu') return 'Ná»™p thiáº¿u';
  if (st === 'khong_nop') return 'KhÃ´ng ná»™p';
  return 'ChÆ°a Ä‘áº¿n háº¡n';
}

async function loadDashboardData() {
  loading.value = true;
  try {
    const resClass = await api.get(`/giangvien/classes/${maLop.value}`);
    classInfo.value = resClass.data;
  } catch {
    classInfo.value = { tenLop: 'L�:p Thá»±c táº­p K64', tenKy: 'Há»c ká»³ 1 - 2024 â€¢ DUE' };
  }

  try {
    const resStudents = await api.get(`/giangvien/classes/${maLop.value}/students`);
    students.value = Array.isArray(resStudents.data) ? resStudents.data : [];
  } catch {
    students.value = [];
  }

  unsubscribeTopics = listenAllTopics((firestoreTopics) => {
    topics.value = firestoreTopics;
  });

  unsubscribeDiaries = listenAllDiaries((firestoreDiaries) => {
    diaries.value = firestoreDiaries;
  });

  loading.value = false;
}

onMounted(() => {
  loadDashboardData();
});

onUnmounted(() => {
  if (unsubscribeTopics) unsubscribeTopics();
  if (unsubscribeDiaries) unsubscribeDiaries();
});
</script>
