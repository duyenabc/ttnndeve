<template>
  <div class="ims-scope w-full mx-auto space-y-6 font-sans pb-20">
    <!-- Breadcrumbs & Header -->
    <div>
      <nav class="flex items-center gap-2 text-[13px] text-slate-500 mb-3">
        <router-link to="/teacher/classes" class="hover:text-blue-700 transition-colors">Lớp của tôi</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-900 font-bold">Công việc</span>
      </nav>
      <h1 class="text-[32px] font-bold text-slate-900 tracking-tight">Công việc</h1>
    </div>

    <!-- Sub Navigation Tabs -->
    <div class="flex items-center gap-8 border-b border-slate-200 mt-2">
      <router-link
        :to="`/teacher/classes/${classId}/diaries`"
        class="pb-3 text-slate-500 hover:text-[#005EA3] font-semibold text-[15px] transition-colors"
      >
        Nhật ký
      </router-link>
      <router-link
        :to="`/teacher/classes/${classId}/tasks`"
        class="pb-3 text-slate-500 hover:text-[#005EA3] font-semibold text-[15px] transition-colors"
      >
        Sự kiện
      </router-link>
      <router-link
        :to="`/teacher/classes/${classId}/topics`"
        class="pb-3 text-[#005EA3] border-b-[3px] border-[#005EA3] font-bold text-[15px] flex items-center gap-1.5"
      >
        Đề tài
        <span v-if="pendingTopicsCount > 0" class="bg-blue-100 text-blue-800 text-[10px] px-1.5 py-0.5 rounded-full font-bold">
          {{ pendingTopicsCount }}
        </span>
      </router-link>
    </div>

    <!-- Search & Lock Registration Bar -->
    <div class="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs space-y-4">
      <div class="flex flex-col md:flex-row justify-between items-stretch md:items-center gap-4">
        <div class="relative flex-1">
          <span class="material-symbols-outlined absolute left-3 top-2.5 text-slate-400 text-[18px]">search</span>
          <input
            v-model="searchQuery"
            type="text"
            placeholder="TÃ¬m kiáº¿m tên Ä‘á» tÃ i, tên sinh viên, MSSV..."
            class="w-full pl-10 pr-4 py-2 bg-slate-50 border border-slate-300 rounded-xl text-xs outline-none focus:ring-2 focus:ring-blue-600"
          />
        </div>

        <button
          @click="openLockModal"
          class="px-4 py-2 rounded-xl text-white font-bold text-xs flex items-center justify-center gap-2 transition active:scale-95 shadow-xs shrink-0"
          :class="isRegistrationLocked ? 'bg-blue-700 hover:bg-blue-800' : 'bg-slate-700 hover:bg-slate-800'"
        >
          <span class="material-symbols-outlined text-[16px]">
            {{ isRegistrationLocked ? 'lock_open' : 'lock' }}
          </span>
          {{ isRegistrationLocked ? 'Má»Ÿ khÃ³a Ä‘Äƒng kÃ½' : 'KhÃ³a Ä‘Äƒng kÃ½' }}
        </button>
      </div>

      <!-- Filter Pills -->
      <div class="flex flex-wrap items-center gap-2">
        <button
          v-for="status in statusOptions"
          :key="status.value"
          @click="selectedStatus = status.value"
          class="px-3.5 py-1.5 rounded-xl text-xs font-bold transition flex items-center gap-1"
          :class="selectedStatus === status.value
            ? 'bg-blue-700 text-white shadow-2xs'
            : 'bg-slate-100 text-slate-600 hover:bg-slate-200'"
        >
          {{ status.label }}
          <span v-if="status.count" class="bg-white/20 text-white px-1.5 rounded-full text-[10px]">
            {{ status.count }}
          </span>
        </button>
      </div>
    </div>

    <!-- Topics Table -->
    <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-2xs">
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse text-xs">
          <thead>
            <tr class="bg-slate-50 border-b border-slate-200 font-bold uppercase text-[10px] text-slate-500">
              <th class="px-6 py-3.5 w-[30%]">TÃªn Ä‘á» tÃ i</th>
              <th class="px-6 py-3.5">Sinh viên / Nhóm</th>
              <th class="px-6 py-3.5">ÄÆ¡n vá»‹ thực tập</th>
              <th class="px-6 py-3.5 text-center">NgÃ y Ä‘Äƒng kÃ½</th>
              <th class="px-6 py-3.5">Trạng thái duyá»‡t</th>
              <th class="px-6 py-3.5 text-center">Thao tÃ¡c</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100">
            <tr v-if="loadingStudents">
              <td colspan="6" class="p-10 text-center text-slate-500 font-medium">
                Äang táº£i danh sÃ¡ch Ä‘á» tÃ i sinh viên...
              </td>
            </tr>
            <tr v-else-if="!hasRegisteredTopics">
              <td colspan="6" class="p-16 text-center text-slate-500">
                <span class="material-symbols-outlined text-[56px] block mb-3 text-slate-300">assignment_late</span>
                <div class="text-sm font-bold text-slate-700">ChÆ°a cÃ³ Ä‘á» tÃ i nào Ä‘Æ°á»£c Ä‘Äƒng kÃ½</div>
                <p class="text-slate-400 text-xs mt-1 max-w-md mx-auto leading-relaxed">
                  Các sinh viên trong lá»›p há»c nÃ y hi�!n tại chưa có Ä‘á» tÃ i thực tập nào Ä‘Æ°á»£c Ä‘Äƒng kÃ½.
                </p>
              </td>
            </tr>
            <tr v-else-if="!filteredTopics.length">
              <td colspan="6" class="p-12 text-center text-slate-500">
                <span class="material-symbols-outlined text-[48px] block mb-2 opacity-40">assignment</span>
                ChÆ°a tÃ¬m tháº¥y Ä‘á» tÃ i nào khá»›p vá»›i bá»™ lá»c hi�!n tại.
              </td>
            </tr>
            <tr
              v-else
              v-for="topic in filteredTopics"
              :key="topic.id"
              class="hover:bg-slate-50/80 transition group"
            >
              <td class="px-6 py-4">
                <span
                  @click="openDetail(topic)"
                  class="font-bold text-slate-900 text-sm hover:text-blue-700 transition cursor-pointer"
                >
                  {{ topic.title }}
                </span>
              </td>
              <td class="px-6 py-4">
                <div class="flex items-center gap-2">
                  <div class="font-bold text-slate-900">{{ topic.student }}</div>
                  <span
                    v-if="topic.groupName"
                    class="px-2 py-0.5 rounded-md text-[10px] font-extrabold flex items-center gap-1"
                    :class="topic.isGroup ? 'bg-[#005EA3]/10 text-[#005EA3] border border-[#005EA3]/20' : 'bg-slate-100 text-slate-600'"
                  >
                    <span v-if="topic.isGroup" class="material-symbols-outlined text-[12px]">groups</span>
                    {{ topic.groupName }}
                  </span>
                </div>
                <span class="text-[10px] font-mono text-slate-500">MSSV Ä‘áº¡i diá»‡n: {{ topic.mssv }}</span>
              </td>
              <td class="px-6 py-4 text-slate-700 font-medium">
                {{ topic.company }}
              </td>
              <td class="px-6 py-4 text-center font-mono text-slate-600">
                {{ topic.date }}
              </td>
              <td class="px-6 py-4">
                <div class="flex items-center gap-1.5">
                  <span
                    class="inline-flex items-center px-2.5 py-0.5 rounded-full text-[11px] font-bold"
                    :class="getStatusBadgeClass(topic.status)"
                  >
                    {{ topic.status }}
                  </span>
                  <span v-if="topic.isUnlockedEdit" class="px-2 py-0.5 bg-purple-100 text-purple-800 text-[10px] font-bold rounded-md">
                    Äang má»Ÿ khÃ³a sửa
                  </span>
                </div>
              </td>
              <td class="px-6 py-4 text-center relative">
                <div class="flex items-center justify-center gap-2">
                  <button
                    @click="openDetail(topic)"
                    class="px-3 py-1.5 bg-blue-50 text-blue-700 hover:bg-blue-100 font-bold rounded-xl transition"
                  >
                    Xem & Duyá»‡t
                  </button>
                  <button
                    v-if="topic.status === 'ÄÃ£ duyá»‡t'"
                    @click="openUnlockDrawer(topic)"
                    class="p-1.5 text-slate-500 hover:text-purple-700 hover:bg-purple-50 rounded-lg transition"
                    title="Má»Ÿ khÃ³a cho sinh viên sửa láº¡i"
                  >
                    <span class="material-symbols-outlined text-[18px]">lock_open</span>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Detail Slide-over Panel (E05.3) -->
    <div
      v-if="selectedTopic"
      class="fixed inset-0 z-[110] flex justify-end bg-slate-900/50 backdrop-blur-xs"
      @click.self="selectedTopic = null"
    >
      <div class="w-full max-w-lg bg-white h-full shadow-2xl flex flex-col justify-between p-6 overflow-y-auto font-sans">
        <div class="space-y-5">
          <div class="flex justify-between items-start border-b border-slate-100 pb-3">
            <div>
              <span
                class="px-2.5 py-0.5 rounded-full text-[11px] font-bold"
                :class="getStatusBadgeClass(selectedTopic.status)"
              >
                {{ selectedTopic.status }}
              </span>
              <h2 class="font-bold text-slate-900 text-base mt-2 leading-snug">{{ selectedTopic.title }}</h2>
            </div>
            <button @click="selectedTopic = null" class="text-slate-400 hover:text-slate-600">âœ•</button>
          </div>

          <!-- Student & Intern Info Grid -->
          <div class="grid grid-cols-2 gap-2.5 p-3.5 bg-slate-50 rounded-2xl text-xs text-slate-700 border border-slate-200">
            <div>
              <span class="text-[10px] text-slate-400 font-bold uppercase block">Nhóm / HÃ¬nh thá»©c</span>
              <span class="font-bold text-[#005EA3] flex items-center gap-1">
                <span v-if="selectedTopic.isGroup" class="material-symbols-outlined text-[14px]">groups</span>
                {{ selectedTopic.groupName || 'CÃ¡ nhÃ¢n' }}
              </span>
            </div>
            <div>
              <span class="text-[10px] text-slate-400 font-bold uppercase block">NgÃ y ná»™p</span>
              <span class="font-mono text-slate-800">{{ selectedTopic.date }}</span>
            </div>
            <div>
              <span class="text-[10px] text-slate-400 font-bold uppercase block">ÄÆ¡n vá»‹ thực tập</span>
              <span class="font-bold text-slate-800">{{ selectedTopic.company }}</span>
            </div>
            <div>
              <span class="text-[10px] text-slate-400 font-bold uppercase block">V�9 trí thực tập</span>
              <span class="font-bold text-slate-800">{{ selectedTopic.position || 'Thá»±c táº­p sinh' }}</span>
            </div>
          </div>

          <!-- Group Members Section -->
          <div v-if="selectedTopic.members && selectedTopic.members.length > 0" class="p-3 bg-blue-50/50 rounded-2xl border border-blue-100 space-y-2">
            <div class="flex items-center justify-between text-xs">
              <span class="font-bold text-slate-900 flex items-center gap-1.5">
                <span class="material-symbols-outlined text-[#005EA3] text-[16px]">group</span>
                ThÃ nh viÃªn thá»±c hi�!n chung Ä‘á» tÃ i nÃ y:
              </span>
              <span class="text-[10px] font-bold text-[#005EA3]">
                {{ selectedTopic.members.length }} sinh viên
              </span>
            </div>
            <div class="space-y-1.5">
              <div
                v-for="m in selectedTopic.members"
                :key="m.mssv"
                class="flex items-center justify-between p-2 bg-white rounded-xl border border-slate-200 text-xs"
              >
                <div>
                  <span class="font-bold text-slate-800">{{ m.name }}</span>
                  <span class="text-[10px] text-slate-500 font-mono ml-2">MSSV: {{ m.mssv }}</span>
                </div>
                <span
                  class="px-2 py-0.5 text-[10px] font-bold rounded-md"
                  :class="m.role === 'TrÆ°á»Ÿng nhóm' ? 'bg-[#005EA3] text-white' : 'bg-slate-100 text-slate-600'"
                >
                  {{ m.role }}
                </span>
              </div>
            </div>
          </div>

          <!-- Visual Approval History (Revision Timeline) -->
          <div v-if="topicHistoryList && topicHistoryList.length > 0" class="space-y-3">
            <h4 class="font-bold text-slate-900 text-xs uppercase tracking-wider flex items-center gap-1.5">
              <span class="material-symbols-outlined text-[#005EA3] text-[18px]">history</span>
              Lá»‹ch sá»­ vÃ²ng duyá»‡t (PhiÃªn báº£n)
            </h4>
            <div class="border-l-2 border-slate-200 pl-4 ml-2 space-y-4">
              <div v-for="(hist, idx) in topicHistoryList" :key="idx" class="relative">
                <!-- Timeline Dot -->
                <div class="absolute -left-[22px] top-1 w-2.5 h-2.5 rounded-full border-2 border-white" :class="hist.status === 'ÄÃ£ duyá»‡t' ? 'bg-emerald-500 ring-2 ring-emerald-100' : (hist.status === 'Tá»« chá»‘i' ? 'bg-rose-500 ring-2 ring-rose-100' : 'bg-amber-500 ring-2 ring-amber-100')"></div>
                
                <div class="bg-slate-50 border border-slate-200 rounded-xl p-3 text-xs space-y-1.5">
                  <div class="flex items-center justify-between">
                    <span class="font-bold text-slate-800 text-[11px]">{{ hist.round }} ({{ hist.date }})</span>
                    <span class="px-2 py-0.5 rounded-md text-[10px] font-bold" :class="hist.statusClass">
                      {{ hist.status }}
                    </span>
                  </div>
                  <p class="text-slate-800 font-semibold leading-tight text-[11px]">{{ hist.title }}</p>
                  <p v-if="hist.file" class="text-[10px] text-slate-500 font-mono flex items-center gap-1">
                    <span class="material-symbols-outlined text-[12px]">attachment</span> {{ hist.file }}
                  </p>
                  <p class="text-slate-600 bg-white p-2 rounded border border-slate-100 text-[11px] leading-relaxed">
                    <strong>Pháº£n há»“i:</strong> {{ hist.feedback }}
                  </p>
                </div>
              </div>
            </div>
          </div>

          <!-- Description -->
          <div class="space-y-1 text-xs">
            <h4 class="font-bold text-slate-900">MÃ´ táº£ Ä‘á» tÃ i</h4>
            <p class="text-slate-600 leading-relaxed bg-slate-50 p-3 rounded-xl border border-slate-200">
              {{ selectedTopic.description }}
            </p>
          </div>

          <!-- Proposal File Attachment -->
          <div v-if="selectedTopic.file || selectedTopic.fileUrl" class="p-3 border border-slate-200 rounded-xl bg-slate-50 flex items-center justify-between hover:bg-slate-100 transition cursor-pointer">
            <div class="flex items-center gap-2">
              <span class="material-symbols-outlined text-rose-600">
                {{ (selectedTopic.file || '').toLowerCase().endsWith('.pdf') ? 'picture_as_pdf' : 'description' }}
              </span>
              <div>
                <span class="font-bold text-xs text-slate-800 block">{{ selectedTopic.file || 'Proposal_DeTai.pdf' }}</span>
                <span class="text-[10px] text-slate-400 font-mono">File Ä‘á» cÆ°Æ¡ng chi tiáº¿t Ä‘Ã­nh kÃ¨m</span>
              </div>
            </div>
            <button @click="downloadTeacherTopicFile(selectedTopic)" class="text-blue-700 font-bold text-xs hover:underline flex items-center gap-1 cursor-pointer">
              <span class="material-symbols-outlined text-[14px]">download</span>
              Táº£i file
            </button>
          </div>

          <!-- Feedback / Reason Input -->
          <div class="space-y-1 text-xs">
            <label class="font-bold text-slate-900 block">Ã kiáº¿n pháº£n há»“i / LÃ½ do tá»« chá»‘i, chỉnh sửa *</label>
            <textarea
              v-model="feedbackText"
              rows="4"
              placeholder="Nháº­p Ã½ kiáº¿n pháº£n há»“i cá»¥ thá»ƒ cho sinh viên..."
              class="w-full px-3.5 py-2.5 rounded-xl border text-xs outline-none focus:ring-2 focus:ring-blue-600 transition"
              :class="feedbackError ? 'border-rose-500 bg-rose-50/20' : 'border-slate-300'"
            ></textarea>
            <p v-if="feedbackError" class="text-[11px] text-rose-600 font-bold mt-0.5">
              âš ï¸ {{ feedbackErrorText || 'Vui lÃ²ng nháº­p ghi chÃº pháº£n há»“i cá»¥ thá»ƒ!' }}
            </p>
          </div>
        </div>

        <!-- Action Footer -->
        <div class="pt-4 border-t border-slate-100 grid grid-cols-3 gap-2">
          <button
            @click="handleStatusUpdate('ÄÃ£ duyá»‡t')"
            class="py-2.5 bg-emerald-600 hover:bg-emerald-700 text-white font-bold rounded-xl text-xs shadow-xs transition"
          >
            Duyá»‡t
          </button>
          <button
            @click="handleStatusUpdate('YÃªu cáº§u chỉnh sửa')"
            class="py-2.5 bg-amber-600 hover:bg-amber-700 text-white font-bold rounded-xl text-xs shadow-xs transition"
          >
            YÃªu cáº§u sửa
          </button>
          <button
            @click="handleStatusUpdate('Tá»« chá»‘i')"
            class="py-2.5 bg-rose-600 hover:bg-rose-700 text-white font-bold rounded-xl text-xs shadow-xs transition"
          >
            Tá»« chá»‘i
          </button>
        </div>
      </div>
    </div>

    <!-- Unlock Edit Drawer Modal -->
    <div
      v-if="unlockDrawerTopic"
      class="fixed inset-0 z-[120] flex justify-end bg-slate-900/50 backdrop-blur-xs"
      @click.self="unlockDrawerTopic = null"
    >
      <div class="w-full max-w-md bg-white h-full shadow-2xl flex flex-col justify-between p-6">
        <div class="space-y-4">
          <div class="flex justify-between items-center border-b pb-3">
            <h3 class="font-bold text-base text-slate-900">Má»Ÿ khÃ³a chỉnh sửa Ä‘á» tÃ i</h3>
            <button @click="unlockDrawerTopic = null" class="text-slate-400 hover:text-slate-600">âœ•</button>
          </div>

          <p class="text-xs text-slate-600">
            Sinh viên <strong>{{ unlockDrawerTopic.student }}</strong> sáº½ Ä‘Æ°á»£c phÃ©p cáº­p nháº­t láº¡i tên Ä‘á» tÃ i và file Ä‘á» cÆ°Æ¡ng Ä‘Ã­nh kÃ¨m.
          </p>

          <div class="space-y-1 text-xs">
            <label class="font-bold text-slate-900 block">LÃ½ do má»Ÿ láº¡i (*)</label>
            <textarea
              v-model="unlockReason"
              @input="unlockReasonError = false"
              rows="5"
              placeholder="Nháº­p lÃ½ do má»Ÿ láº¡i cho sinh viên chỉnh sửa..."
              class="w-full px-3.5 py-2.5 rounded-xl border text-xs outline-none focus:ring-2 focus:ring-blue-600 transition"
              :class="unlockReasonError ? 'border-rose-500 bg-rose-50/20' : 'border-slate-300'"
            ></textarea>
            <p v-if="unlockReasonError" class="text-[11px] text-rose-600 font-bold mt-0.5">
              âš ï¸ Vui lÃ²ng nháº­p lÃ½ do má»Ÿ láº¡i
            </p>
          </div>
        </div>

        <div class="pt-4 border-t flex justify-end gap-3 text-xs">
          <button @click="unlockDrawerTopic = null" class="px-4 py-2 border border-slate-300 rounded-xl font-semibold text-slate-700 hover:bg-slate-100">
            Há»§y
          </button>
          <button @click="confirmUnlockEdit" class="px-5 py-2 bg-purple-700 hover:bg-purple-800 text-white font-bold rounded-xl shadow-xs">
            XÃ¡c nháº­n má»Ÿ khÃ³a
          </button>
        </div>
      </div>
    </div>

    <!-- Confirmation Lock Modal -->
    <div
      v-if="showLockConfirmModal"
      class="fixed inset-0 z-[120] bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-4"
      @click.self="showLockConfirmModal = false"
    >
      <div class="bg-white rounded-2xl shadow-2xl max-w-md w-full p-6 space-y-4">
        <h3 class="font-bold text-lg text-slate-900">
          {{ isRegistrationLocked ? 'Má»Ÿ láº¡i quyá»n Ä‘Äƒng kÃ½ Ä‘á» tÃ i?' : 'KhÃ³a quyá»n Ä‘Äƒng kÃ½ Ä‘á» tÃ i của lá»›p?' }}
        </h3>
        <p class="text-xs text-slate-600 leading-relaxed">
          {{ isRegistrationLocked
            ? 'Sinh viên cÃ³ thá»ƒ tiáº¿p tá»¥c chỉnh sửa, lÆ°u nhÃ¡p và gá»­i duyá»‡t Ä‘á» tÃ i trá»±c tuyáº¿n.'
            : 'Sinh viên chÆ°a Ä‘Äƒng kÃ½ hoặc Ä‘á» tÃ i Ä‘ang á»Ÿ tráº¡ng thÃ¡i NhÃ¡p/YÃªu cáº§u sửa sáº½ không thá»ƒ ná»™p bÃ i ná»¯a.' }}
        </p>

        <div class="flex justify-end gap-3 pt-2">
          <button @click="showLockConfirmModal = false" class="px-4 py-2 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold text-xs rounded-xl">
            Há»§y
          </button>
          <button @click="confirmToggleLock" class="px-5 py-2 bg-blue-700 hover:bg-blue-800 text-white font-bold text-xs rounded-xl shadow-xs">
            XÃ¡c nháº­n
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
  import { ref, computed, onMounted, onUnmounted } from 'vue';
  import { useRoute } from 'vue-router';
  import api from '@/api/api';
  import { sendRealtimeNotification } from '@/services/notificationService';
  import { listenAllTopics, updateTopicStatusInDb } from '@/services/topicService';

  const route = useRoute();
  const loadingStudents = ref(true);
  const classId = computed(() => route.params.id || '1');

  const searchQuery = ref('');
  const selectedStatus = ref('ALL');
  const isRegistrationLocked = ref(false);
  const showLockConfirmModal = ref(false);

  const selectedTopic = ref(null);
  const unlockDrawerTopic = ref(null);
  const unlockReason = ref('');
  const unlockReasonError = ref(false);
  const feedbackText = ref('');
  const feedbackError = ref(false);
  const feedbackErrorText = ref('');
  const toastMsg = ref('');

  const topicHistoryList = computed(() => {
    if (!selectedTopic.value) return [];
    
    // If the topic already has a structured list, use it
    if (selectedTopic.value.historyList) {
      return selectedTopic.value.historyList;
    }
    
    const list = [];
    
    // For demo/aesthetic completeness, provide structured logs
    if (selectedTopic.value.mssv === '20241001') {
      list.push({
        round: 'VÃ²ng 2',
        date: '12/10/2024 09:15',
        status: 'ÄÃ£ duyá»‡t',
        statusClass: 'bg-emerald-100 text-emerald-800 border border-emerald-200',
        title: 'XÃ¢y dá»±ng Há»‡ thá»‘ng Quản lý Thá»±c táº­p Doanh nghi�!p (IMS) dá»±a trÃªn Kiáº¿n trÃºc Microservices',
        file: 'De_cuong_chi_tiet_De_tai_IMS.pdf',
        feedback: 'Äá» tÃ i ráº¥t tá»‘t, bÃ¡m sÃ¡t thá»±c tiá»…n tại doanh nghiá»‡p FPT. Khuyáº¿n khÃ­ch hoÃ n thÃ nh Ä‘Ãºng tiáº¿n Ä‘á»™.'
      });
      list.push({
        round: 'VÃ²ng 1',
        date: '10/10/2024 14:30',
        status: 'YÃªu cáº§u chỉnh sửa',
        statusClass: 'bg-amber-100 text-amber-800 border border-amber-200',
        title: 'NghiÃªn cá»©u kiáº¿n trÃºc Microservices trong phÃ¡t triá»ƒn pháº§n má»m',
        file: 'DeCuong_BanDau_Microservices.pdf',
        feedback: 'TÃªn Ä‘á» tÃ i quÃ¡ rá»™ng, cáº§n thu háº¹p pháº¡m vi Ã¡p dá»¥ng vào má»™t dá»± Ã¡n thá»±c táº¿ cá»¥ thá»ƒ.'
      });
    } else {
      // General fallback based on current status
      if (selectedTopic.value.status === 'ÄÃ£ duyá»‡t') {
        list.push({
          round: 'VÃ²ng 1',
          date: selectedTopic.value.date || 'HÃ´m nay',
          status: 'ÄÃ£ duyá»‡t',
          statusClass: 'bg-emerald-100 text-emerald-800 border border-emerald-200',
          title: selectedTopic.value.title,
          file: selectedTopic.value.file || 'Proposal_v1.pdf',
          feedback: selectedTopic.value.feedback || 'Äá» tÃ i há»£p lá»‡, Ä‘Æ°á»£c phÃª duyá»‡t thá»±c hi�!n.'
        });
      } else if (selectedTopic.value.status === 'Tá»« chá»‘i') {
        list.push({
          round: 'VÃ²ng 1',
          date: selectedTopic.value.date || 'HÃ´m nay',
          status: 'Tá»« chá»‘i',
          statusClass: 'bg-rose-100 text-rose-800 border border-rose-200',
          title: selectedTopic.value.title,
          file: selectedTopic.value.file || 'Proposal_v1.pdf',
          feedback: selectedTopic.value.feedback || 'Äá» tÃ i không phÃ¹ há»£p vá»›i má»¥c tiÃªu thực tập.'
        });
      } else if (selectedTopic.value.status === 'YÃªu cáº§u chỉnh sửa') {
        list.push({
          round: 'VÃ²ng 1',
          date: selectedTopic.value.date || 'HÃ´m nay',
          status: 'YÃªu cáº§u chỉnh sửa',
          statusClass: 'bg-amber-100 text-amber-800 border border-amber-200',
          title: selectedTopic.value.title,
          file: selectedTopic.value.file || 'Proposal_v1.pdf',
          feedback: selectedTopic.value.feedback || 'Cáº§n bá»• sung chi tiáº¿t sÆ¡ Ä‘á»“ thiết kế há»‡ thá»‘ng.'
        });
      } else if (selectedTopic.value.status === 'Chá» duyá»‡t') {
        list.push({
          round: 'VÃ²ng 1',
          date: selectedTopic.value.date || 'HÃ´m nay',
          status: 'Chá» duyá»‡t',
          statusClass: 'bg-blue-100 text-blue-800 border border-blue-200',
          title: selectedTopic.value.title,
          file: selectedTopic.value.file || 'Proposal_v1.pdf',
          feedback: 'Äang Ä‘á»£i giảng viên cháº¥m duyá»‡t.'
        });
      }
    }
    
    return list;
  });

  const statusOptions = computed(() => {
    return [
      { value: 'ALL', label: 'Táº¥t cáº£' },
      { value: 'Chá» duyá»‡t', label: 'Chá» duyá»‡t', count: topics.value.filter(t => t.status === 'Chá» duyá»‡t').length },
      { value: 'ÄÃ£ duyá»‡t', label: 'ÄÃ£ duyá»‡t', count: topics.value.filter(t => t.status === 'ÄÃ£ duyá»‡t').length },
      { value: 'YÃªu cáº§u chỉnh sửa', label: 'YÃªu cáº§u sửa', count: topics.value.filter(t => t.status === 'YÃªu cáº§u chỉnh sửa').length },
      { value: 'Tá»« chá»‘i', label: 'Tá»« chá»‘i', count: topics.value.filter(t => t.status === 'Tá»« chá»‘i').length },
      { value: 'NhÃ¡p', label: 'NhÃ¡p', count: topics.value.filter(t => t.status === 'NhÃ¡p').length },
      { value: 'ChÆ°a ná»™p', label: 'ChÆ°a ná»™p', count: topics.value.filter(t => t.status === 'ChÆ°a ná»™p').length }
    ];
  });

  function downloadTeacherTopicFile(topic) {
    if (!topic) return;
    if (topic.fileUrl) {
      const link = document.createElement('a');
      link.href = topic.fileUrl;
      link.download = topic.file || 'Proposal_DeTai.pdf';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      showToast('âœ“ ÄÃ£ táº£i xuá»‘ng file Ä‘á» cÆ°Æ¡ng thÃ nh cÃ´ng!');
    } else {
      showToast('âœ“ Äang táº£i xuá»‘ng file Ä‘á» cÆ°Æ¡ng: ' + (topic.file || 'Proposal_DeTai.pdf'));
    }
  }

  const hasRegisteredTopics = computed(() => {
    return topics.value.some(t => t.status !== 'ChÆ°a ná»™p' && t.title && t.title !== 'â€” ChÆ°a Ä‘Äƒng kÃ½ Ä‘á» tÃ i â€”');
  });

  const topics = ref([]);
  let unsubscribeTopics = null;

  async function loadClassStudents() {
    loadingStudents.value = true;
    try {
      const res = await api.get(`/giangvien/classes/${classId.value}/students`);
      const studentList = Array.isArray(res.data) ? res.data : (res.data?.items || []);
      
      topics.value = studentList.map((s, idx) => {
        return {
          id: s.maGhiDanh || (1001 + idx),
          title: 'â€” ChÆ°a Ä‘Äƒng kÃ½ Ä‘á» tÃ i â€”',
          student: s.hoTen || 'Sinh viên',
          isGroup: !!s.tenNhom,
          groupName: s.tenNhom || 'CÃ¡ nhÃ¢n',
          members: [{ name: s.hoTen, mssv: s.maSoSinhVien, role: s.tenNhom ? 'ThÃ nh viÃªn' : 'CÃ¡ nhÃ¢n' }],
          mssv: s.maSoSinhVien || '',
          company: s.donViThucTap || 'ChÆ°a khai bÃ¡o',
          position: s.viTriThucTap || 'â€”',
          date: 'â€”',
          status: 'ChÆ°a ná»™p',
          file: null,
          description: 'Sinh viên chÆ°a ná»™p Ä‘á» tÃ i thực tập. Háº¡n Ä‘Äƒng kÃ½ cÃ²n hiá»‡u lá»±c.'
        };
      });

      // 1. Load from localStorage fallbacks for these students
      topics.value.forEach(t => {
        const mssv = t.mssv;
        if (!mssv) return;

        const savedStatus = localStorage.getItem(`ims_student_topic_status_${mssv}`);
        const savedTitle = localStorage.getItem(`ims_student_topic_name_${mssv}`);
        const savedDesc = localStorage.getItem(`ims_student_topic_desc_${mssv}`);
        const savedOutline = localStorage.getItem(`ims_student_topic_outline_${mssv}`);
        const savedDate = localStorage.getItem(`ims_student_topic_date_${mssv}`);

        if (savedStatus) {
          if (savedStatus === 'pending') t.status = 'Chá» duyá»‡t';
          else if (savedStatus === 'approved') t.status = 'ÄÃ£ duyá»‡t';
          else if (savedStatus === 'rejected') t.status = 'YÃªu cáº§u chỉnh sửa';
          else if (savedStatus === 'denied') t.status = 'Tá»« chá»‘i';
          else if (savedStatus === 'draft') t.status = 'NhÃ¡p';
          else if (savedStatus === 'new') t.status = 'ChÆ°a ná»™p';
        }
        if (savedTitle) t.title = savedTitle;
        if (savedDesc) t.description = savedDesc;
        if (savedOutline) t.file = savedOutline;
        const savedOutlineData = localStorage.getItem(`ims_student_topic_outline_data_${mssv}`);
        if (savedOutlineData) t.fileUrl = savedOutlineData;
        if (savedDate) t.date = savedDate;
      });

    } catch (err) {
      console.error('Error loading class roster for topics:', err);
    } finally {
      // 2. Real-time Firebase topic sync
      unsubscribeTopics = listenAllTopics((firestoreTopics) => {
        firestoreTopics.forEach(ft => {
          const mssv = ft.mssv || ft.userId;
          if (!mssv) return;

          let t = topics.value.find(item => String(item.mssv) === String(mssv));
          if (!t) {
            t = {
              id: topics.value.length + 1001,
              title: ft.title || 'â€” ChÆ°a Ä‘Äƒng kÃ½ Ä‘á» tÃ i â€”',
              student: ft.studentName || 'Sinh viên má»›i',
              isGroup: ft.group && ft.group !== 'ChÆ°a cÃ³ nhóm',
              groupName: ft.group || 'CÃ¡ nhÃ¢n',
              members: [{ name: ft.studentName || 'Sinh viên má»›i', mssv: mssv, role: 'CÃ¡ nhÃ¢n' }],
              mssv: mssv,
              company: 'ChÆ°a khai bÃ¡o',
              position: 'â€”',
              date: ft.date || 'â€”',
              status: 'ChÆ°a ná»™p',
              file: ft.file || null,
              description: ft.description || 'Sinh viên chÆ°a ná»™p Ä‘á» tÃ i thực tập.'
            };
            topics.value.push(t);
          }

          if (ft.status) {
            const st = String(ft.status).trim();
            const lower = st.toLowerCase();
            if (st === 'Pending' || lower === 'pending' || lower === 'chá» duyá»‡t') t.status = 'Chá» duyá»‡t';
            else if (st === 'Approved' || lower === 'approved' || lower === 'đã duyá»‡t') t.status = 'ÄÃ£ duyá»‡t';
            else if (st === 'Requested Changes' || st === 'RequestedChanges' || lower === 'requested_changes' || lower === 'requested changes' || lower === 'yêu cầu chỉnh sửa' || lower === 'yêu cầu sửa' || (lower === 'rejected' && st !== 'Rejected' && st !== 'rejected_final')) t.status = 'YÃªu cáº§u chỉnh sửa';
            else if (st === 'Rejected' || lower === 'rejected_final' || lower === 'denied' || lower === 'tá»« chá»‘i') t.status = 'Tá»« chá»‘i';
            else if (st === 'Draft' || lower === 'draft' || lower === 'nhÃ¡p') t.status = 'NhÃ¡p';
            else if (st === 'New' || lower === 'new' || lower === 'chÆ°a ná»™p') t.status = 'ChÆ°a ná»™p';
            else t.status = st;
          }
          if (ft.title) t.title = ft.title;
          if (ft.description) t.description = ft.description;
          if (ft.file || ft.fileName) t.file = ft.file || ft.fileName;
          if (ft.fileUrl || ft.fileData) t.fileUrl = ft.fileUrl || ft.fileData;
          if (ft.date) t.date = ft.date;
          if (ft.feedback) t.feedback = ft.feedback;
        });
      });
      
      loadingStudents.value = false;
    }
  }

  onUnmounted(() => {
    if (unsubscribeTopics) {
      unsubscribeTopics();
    }
  });

  onMounted(() => {
    loadClassStudents();
  });

  const filteredTopics = computed(() => {
    const list = topics.value.filter(t => {
      const matchStatus = selectedStatus.value === 'ALL' || t.status === selectedStatus.value;
      const q = searchQuery.value.toLowerCase().trim();
      const matchSearch =
        !q ||
        t.title.toLowerCase().includes(q) ||
        t.student.toLowerCase().includes(q) ||
        t.mssv.includes(q);
      return matchStatus && matchSearch;
    });

    // Prioritize 'Chá» duyá»‡t' (Pending) status
    return list.sort((a, b) => {
      if (a.status === 'Chá» duyá»‡t' && b.status !== 'Chá» duyá»‡t') return -1;
      if (b.status === 'Chá» duyá»‡t' && a.status !== 'Chá» duyá»‡t') return 1;
      return 0;
    });
  });

  function getStatusBadgeClass(status) {
    switch (status) {
      case 'Chá» duyá»‡t':
        return 'bg-blue-100 text-blue-800 animate-pulse';
      case 'ÄÃ£ duyá»‡t':
        return 'bg-emerald-100 text-emerald-800';
      case 'YÃªu cáº§u chỉnh sửa':
        return 'bg-amber-100 text-amber-800';
      case 'Tá»« chá»‘i':
        return 'bg-rose-100 text-rose-800';
      case 'NhÃ¡p':
        return 'bg-slate-100 text-slate-800 border border-slate-300';
      default:
        return 'bg-slate-100 text-slate-600';
    }
  }

  function openLockModal() {
    showLockConfirmModal.value = true;
  }

  function confirmToggleLock() {
    isRegistrationLocked.value = !isRegistrationLocked.value;
    showLockConfirmModal.value = false;
    showToast(isRegistrationLocked.value ? 'âœ“ ÄÃ£ khÃ³a quyá»n Ä‘Äƒng kÃ½ Ä‘á» tÃ i của lá»›p' : 'âœ“ ÄÃ£ má»Ÿ láº¡i quyá»n Ä‘Äƒng kÃ½ Ä‘á» tÃ i cho lá»›p');
  }

  function openDetail(topic) {
    selectedTopic.value = topic;
    feedbackText.value = '';
    feedbackError.value = false;
  }

  function openUnlockDrawer(topic) {
    unlockDrawerTopic.value = topic;
    unlockReason.value = '';
  }

  function confirmUnlockEdit() {
    unlockReasonError.value = false;
    if (!unlockReason.value.trim()) {
      unlockReasonError.value = true;
      showToast('Vui lÃ²ng nháº­p lÃ½ do má»Ÿ láº¡i');
      return;
    }
    if (unlockDrawerTopic.value) {
      unlockDrawerTopic.value.isUnlockedEdit = true;
      unlockDrawerTopic.value.status = 'YÃªu cáº§u chỉnh sửa';
      const mssv = unlockDrawerTopic.value.mssv || '20241001';

      localStorage.setItem(`ims_student_topic_status_${mssv}`, 'rejected');
      localStorage.setItem('ims_student_topic_status', 'rejected');
      localStorage.setItem(`ims_student_topic_feedback_${mssv}`, unlockReason.value);
      
      // Update in Firestore
      updateTopicStatusInDb(mssv, {
        status: 'rejected',
        feedback: unlockReason.value,
        isUnlockedEdit: true
      });

      // Dispatch real-time notification to student via Firebase
      sendRealtimeNotification({
        role: 'SinhVien',
        userId: mssv,
        title: 'Giáº£ng viÃªn đã má»Ÿ khÃ³a sửa Ä‘á» tÃ i',
        content: `Giáº£ng viÃªn đã má»Ÿ quyá»n cho phÃ©p báº¡n chỉnh sửa láº¡i Ä‘á» tÃ i. LÃ½ do: ${unlockReason.value}`,
        icon: 'lock_open',
        bgClass: 'bg-blue-100 text-blue-700',
        link: '/student/progress'
      });

      showToast(`ÄÃ£ má»Ÿ khÃ³a cho sinh viên ${unlockDrawerTopic.value.student} sửa láº¡i Ä‘á» tÃ i!`);
      unlockDrawerTopic.value = null;
    }
  }

  function handleStatusUpdate(newStatus) {
    feedbackError.value = false;
    feedbackErrorText.value = '';

    if (newStatus !== 'ÄÃ£ duyá»‡t' && !feedbackText.value.trim()) {
      feedbackError.value = true;
      if (newStatus === 'YÃªu cáº§u chỉnh sửa' || newStatus === 'YÃªu cáº§u sửa') {
        feedbackErrorText.value = 'Vui lÃ²ng nháº­p ghi chÃº cá»¥ thá»ƒ cáº§n chỉnh sửa';
        showToast('Vui lÃ²ng nháº­p ghi chÃº cá»¥ thá»ƒ cáº§n chỉnh sửa');
      } else if (newStatus === 'Tá»« chá»‘i') {
        feedbackErrorText.value = 'Vui lÃ²ng nháº­p lÃ½ do tá»« chá»‘i';
        showToast('Vui lÃ²ng nháº­p lÃ½ do tá»« chá»‘i');
      } else {
        feedbackErrorText.value = 'Vui lÃ²ng nháº­p Ã½ kiáº¿n pháº£n há»“i!';
        showToast('Vui lÃ²ng nháº­p Ã½ kiáº¿n pháº£n há»“i');
      }
      return;
    }

    if (selectedTopic.value) {
      selectedTopic.value.status = newStatus;
      const mssv = selectedTopic.value.mssv || '20241001';

      // Map teacher's status back to student's status key
      let mappedStatus = 'new';
      if (newStatus === 'ÄÃ£ duyá»‡t') mappedStatus = 'approved';
      else if (newStatus === 'Chá» duyá»‡t') mappedStatus = 'pending';
      else if (newStatus === 'YÃªu cáº§u chỉnh sửa') mappedStatus = 'rejected';
      else if (newStatus === 'Tá»« chá»‘i') mappedStatus = 'denied';

      localStorage.setItem(`ims_student_topic_status_${mssv}`, mappedStatus);
      localStorage.setItem('ims_student_topic_status', mappedStatus);

      if (feedbackText.value) {
        localStorage.setItem(`ims_student_topic_feedback_${mssv}`, feedbackText.value);
      } else {
        localStorage.removeItem(`ims_student_topic_feedback_${mssv}`);
      }

      // Update in Firestore
      updateTopicStatusInDb(mssv, {
        status: mappedStatus,
        feedback: feedbackText.value || '',
        isUnlockedEdit: false
      });

      // Dispatch real-time Firebase notification to student
      sendRealtimeNotification({
        role: 'SinhVien',
        userId: mssv,
        title: `Äá» tÃ i thực tập: ${newStatus}`,
        content: `Giáº£ng viÃªn vá»«a cáº­p nháº­t tráº¡ng thÃ¡i Ä‘á» tÃ i "${selectedTopic.value.title}" sang "${newStatus}".${feedbackText.value ? ' Pháº£n há»“i: ' + feedbackText.value : ''}`,
        icon: newStatus === 'ÄÃ£ duyá»‡t' ? 'verified' : (newStatus === 'YÃªu cáº§u chỉnh sửa' ? 'rate_review' : 'cancel'),
        bgClass: newStatus === 'ÄÃ£ duyá»‡t' ? 'bg-emerald-100 text-emerald-700' : (newStatus === 'YÃªu cáº§u chỉnh sửa' ? 'bg-amber-100 text-amber-800' : 'bg-rose-100 text-rose-700'),
        link: '/student/progress'
      });

      showToast(`âœ“ ÄÃ£ cáº­p nháº­t tráº¡ng thÃ¡i Ä‘á» tÃ i sang "${newStatus}" và gá»­i thÃ´ng bÃ¡o thá»i gian thá»±c!`);
      selectedTopic.value = null;
    }
  }

  function showToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }
</script>
