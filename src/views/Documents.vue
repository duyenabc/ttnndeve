<template>
  <div class="ims-scope w-full mx-auto space-y-6 font-sans pb-24">
    <!-- Breadcrumb & Top Header -->
    <div class="bg-white rounded-2xl border border-slate-200 p-6 shadow-2xs flex flex-col md:flex-row md:items-center justify-between gap-4">
      <div>
        <nav class="flex items-center gap-2 text-xs text-slate-500 mb-2">
          <router-link to="/teacher/classes" class="hover:text-blue-700 transition-colors">Lá»›p cá»§a tÃ´i</router-link>
          <span class="material-symbols-outlined text-[14px]">chevron_right</span>
          <span class="font-bold text-slate-900">Kho tÃ i liá»‡u & biá»ƒu máº«u</span>
        </nav>
        <h1 class="text-[28px] font-bold text-slate-900 tracking-tight">Kho TÃ i liá»‡u & Biá»ƒu máº«u IMS</h1>
        <p class="text-xs text-slate-500 mt-1">
          Tra cá»©u, Ä‘Äƒng táº£i biá»ƒu máº«u chÃ­nh thá»©c, quy cháº¿ thá»±c táº­p vÃ  tÃ i liá»‡u hÆ°á»›ng dáº«n dÃ nh cho Giáº£ng viÃªn & Sinh viÃªn
        </p>
      </div>

      <button
        @click="showUploadDrawer = true"
        class="bg-[#005EA3] hover:bg-blue-800 text-white px-5 py-2.5 rounded-xl flex items-center gap-2 font-bold text-xs shadow-md transition-all active:scale-95 shrink-0 self-start md:self-auto"
      >
        <span class="material-symbols-outlined text-[20px]">upload</span>
        ÄÄƒng táº£i tÃ i liá»‡u
      </button>
    </div>

    <!-- Filter & Search Bar -->
    <div class="bg-white rounded-2xl border border-slate-200 p-4 shadow-2xs space-y-4">
      <div class="flex flex-col md:flex-row justify-between items-stretch md:items-center gap-4">
        <!-- Source Toggle -->
        <div class="flex bg-slate-100 p-1 rounded-xl border border-slate-200 w-fit text-xs font-bold">
          <button
            @click="sourceFilter = 'ALL'"
            class="px-4 py-1.5 rounded-lg transition"
            :class="sourceFilter === 'ALL' ? 'bg-white text-blue-700 shadow-2xs' : 'text-slate-600 hover:text-slate-900'"
          >
            Táº¤T Cáº¢
          </button>
          <button
            @click="sourceFilter = 'Khoa'"
            class="px-4 py-1.5 rounded-lg transition"
            :class="sourceFilter === 'Khoa' ? 'bg-white text-blue-700 shadow-2xs' : 'text-slate-600 hover:text-slate-900'"
          >
            Tá»ª KHOA
          </button>
          <button
            @click="sourceFilter = 'Báº¡n'"
            class="px-4 py-1.5 rounded-lg transition"
            :class="sourceFilter === 'Báº¡n' ? 'bg-white text-blue-700 shadow-2xs' : 'text-slate-600 hover:text-slate-900'"
          >
            Tá»ª Báº N
          </button>
        </div>

        <div class="flex items-center gap-3">
          <!-- Search Input -->
          <div class="relative min-w-[260px]">
            <span class="material-symbols-outlined absolute left-3 top-2.5 text-slate-400 text-[18px]">search</span>
            <input
              v-model="searchQuery"
              type="text"
              placeholder="TÃ¬m kiáº¿m tÃªn tÃ i liá»‡u, biá»ƒu máº«u..."
              class="w-full pl-10 pr-4 py-2 bg-slate-50 border border-slate-300 rounded-xl text-xs outline-none focus:ring-2 focus:ring-blue-600"
            />
          </div>

          <!-- Status Dropdown -->
          <select
            v-model="statusFilter"
            class="px-3 py-2 bg-slate-50 border border-slate-300 rounded-xl text-xs font-bold text-slate-700 outline-none focus:ring-2 focus:ring-blue-600"
          >
            <option value="ALL">Táº¥t cáº£ tráº¡ng thÃ¡i</option>
            <option value="Äang lÆ°u hÃ nh">Äang lÆ°u hÃ nh</option>
            <option value="Ngá»«ng sá»­ dá»¥ng">Ngá»«ng sá»­ dá»¥ng</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Document Cards Grid -->
    <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
      <div
        v-for="doc in filteredDocuments"
        :key="doc.id"
        class="bg-white rounded-2xl border border-slate-200 p-5 shadow-2xs hover:shadow-md transition-all flex flex-col justify-between relative group overflow-hidden h-[260px]"
        :class="doc.status === 'Ngá»«ng sá»­ dá»¥ng' ? 'opacity-75 bg-slate-50' : ''"
      >
        <!-- Top Badges -->
        <div class="flex justify-between items-center z-10">
          <span class="px-2.5 py-0.5 rounded-full text-[10px] font-extrabold" :class="getSourceBadgeClass(doc.source)">
            {{ doc.source }}
          </span>
          <span class="px-2.5 py-0.5 rounded-full text-[10px] font-extrabold" :class="doc.status === 'Äang lÆ°u hÃ nh' ? 'bg-emerald-100 text-emerald-800' : 'bg-slate-200 text-slate-700'">
            {{ doc.status }}
          </span>
        </div>

        <!-- File Icon & Title -->
        <div class="flex-1 flex flex-col items-center justify-center text-center my-2">
          <div class="w-14 h-14 rounded-2xl flex items-center justify-center font-black text-sm mb-3 shadow-2xs" :class="getFileIconClass(doc.type)">
            <span class="material-symbols-outlined text-[32px]">{{ getFileIconSymbol(doc.type) }}</span>
          </div>
          <h3 class="font-bold text-slate-900 text-sm line-clamp-2 px-1 hover:text-blue-700 transition cursor-pointer">
            {{ doc.name }}
          </h3>
          <p class="text-[11px] text-slate-500 mt-1 font-medium">{{ doc.size }} â€¢ {{ doc.date }}</p>
        </div>

        <!-- Hover Action Slide Footer -->
        <div class="absolute bottom-0 left-0 right-0 h-12 bg-white/95 backdrop-blur-xs border-t border-slate-200 flex items-center justify-around translate-y-full group-hover:translate-y-0 transition-transform duration-200 z-20">
          <button
            @click="downloadDoc(doc)"
            class="p-2 text-slate-600 hover:text-blue-700 rounded-lg transition"
            title="Táº£i xuá»‘ng"
          >
            <span class="material-symbols-outlined text-[20px]">download</span>
          </button>
          <button
            @click="toggleHideDoc(doc)"
            class="p-2 text-slate-600 hover:text-blue-700 rounded-lg transition"
            :title="doc.status === 'Äang lÆ°u hÃ nh' ? 'áº¨n tÃ i liá»‡u' : 'Hiá»‡n tÃ i liá»‡u'"
          >
            <span class="material-symbols-outlined text-[20px]">
              {{ doc.status === 'Äang lÆ°u hÃ nh' ? 'visibility' : 'visibility_off' }}
            </span>
          </button>
          <button
            @click="confirmDeleteDoc(doc)"
            class="p-2 text-slate-600 hover:text-rose-600 rounded-lg transition"
            title="XÃ³a tÃ i liá»‡u"
          >
            <span class="material-symbols-outlined text-[20px]">delete</span>
          </button>
        </div>
      </div>
    </div>

    <!-- Upload Drawer (E04.1) -->
    <div
      v-if="showUploadDrawer"
      class="fixed inset-0 z-[110] flex justify-end bg-slate-900/50 backdrop-blur-xs"
      @click.self="showUploadDrawer = false"
    >
      <div class="w-full max-w-lg bg-white h-full shadow-2xl flex flex-col justify-between p-6 overflow-y-auto font-sans">
        <div class="space-y-5">
          <div class="flex justify-between items-center border-b pb-3">
            <h3 class="font-bold text-slate-900 text-base flex items-center gap-2">
              <span class="material-symbols-outlined text-blue-600">upload_file</span> ÄÄƒng táº£i tÃ i liá»‡u & biá»ƒu máº«u
            </h3>
            <button @click="showUploadDrawer = false" class="text-slate-400 hover:text-slate-600">âœ•</button>
          </div>

          <form @submit.prevent="submitUpload" class="space-y-4 text-xs">
            <div>
              <label class="block font-bold text-slate-700 mb-1">TÃªn tÃ i liá»‡u & biá»ƒu máº«u (TÃ¹y chá»n)</label>
              <input
                v-model="uploadForm.name"
                type="text"
                placeholder="Tá»± Ä‘á»™ng láº¥y tÃªn file náº¿u Ä‘á»ƒ trá»‘ng..."
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
              />
            </div>

            <div>
              <label class="block font-bold text-slate-700 mb-1">Chá»n táº­p tin Ä‘Ã­nh kÃ¨m (Báº®T BUá»˜C)</label>
              <!-- Dropzone Area -->
              <div
                @click="triggerFileInput"
                class="border-2 border-dashed border-slate-300 hover:border-blue-600 bg-slate-50 hover:bg-blue-50/30 rounded-2xl p-6 text-center transition cursor-pointer space-y-2"
              >
                <div class="w-12 h-12 rounded-full bg-blue-100 text-blue-700 flex items-center justify-center mx-auto">
                  <span class="material-symbols-outlined text-[28px]">cloud_upload</span>
                </div>
                <div>
                  <p class="font-bold text-slate-800">KÃ©o tháº£ hoáº·c <span class="text-blue-700 underline">Click Ä‘á»ƒ chá»n</span></p>
                  <p class="text-[11px] text-slate-400 mt-0.5">Há»— trá»£: .pdf, .docx, .xlsx (Tá»‘i Ä‘a 10MB)</p>
                </div>
                <input ref="fileInputRef" type="file" class="hidden" accept=".pdf,.doc,.docx,.xls,.xlsx" @change="onFileSelected" />
              </div>

              <!-- File Preview & Validation Message -->
              <div v-if="selectedFile" class="mt-3 p-3 bg-blue-50 border border-blue-200 rounded-xl flex items-center justify-between">
                <div class="flex items-center gap-2 truncate">
                  <span class="material-symbols-outlined text-blue-600">description</span>
                  <div class="truncate">
                    <p class="font-bold text-slate-900 truncate">{{ selectedFile.name }}</p>
                    <p class="text-[10px] text-slate-500">{{ (selectedFile.size / (1024*1024)).toFixed(2) }} MB</p>
                  </div>
                </div>
                <button type="button" @click="selectedFile = null" class="text-slate-400 hover:text-rose-600 font-bold ml-2">âœ•</button>
              </div>
            </div>
          </form>
        </div>

        <div class="pt-4 border-t flex justify-end gap-3 text-xs">
          <button @click="showUploadDrawer = false" class="px-4 py-2 border border-slate-300 rounded-xl font-semibold text-slate-700 hover:bg-slate-100">
            Há»§y
          </button>
          <button @click="submitUpload" class="px-5 py-2 bg-blue-700 hover:bg-blue-800 text-white font-bold rounded-xl shadow-xs">
            ÄÄƒng táº£i ngay
          </button>
        </div>
      </div>
    </div>

    <!-- Confirm Delete Modal -->
    <div
      v-if="docToDelete"
      class="fixed inset-0 z-[120] bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-4"
      @click.self="docToDelete = null"
    >
      <div class="bg-white rounded-2xl shadow-2xl max-w-sm w-full p-6 space-y-4 text-xs">
        <div class="flex items-center gap-3">
          <div class="w-10 h-10 rounded-full bg-rose-100 text-rose-600 flex items-center justify-center shrink-0 font-bold">
            <span class="material-symbols-outlined text-[22px]">delete</span>
          </div>
          <div>
            <h3 class="font-bold text-base text-slate-900">XÃ³a vÄ©nh viá»…n tÃ i liá»‡u?</h3>
            <p class="text-slate-500 mt-0.5">HÃ nh Ä‘á»™ng nÃ y khÃ´ng thá»ƒ hoÃ n tÃ¡c.</p>
          </div>
        </div>

        <div class="flex justify-end gap-2 pt-2">
          <button @click="docToDelete = null" class="px-4 py-2 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold rounded-xl">
            Há»§y
          </button>
          <button @click="executeDeleteDoc" class="px-5 py-2 bg-rose-600 hover:bg-rose-700 text-white font-bold rounded-xl shadow-xs">
            XÃ³a vÄ©nh viá»…n
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
  import { ref, computed } from 'vue';

  const searchQuery = ref('');
  const sourceFilter = ref('ALL');
  const statusFilter = ref('ALL');
  const showUploadDrawer = ref(false);
  const docToDelete = ref(null);
  const selectedFile = ref(null);
  const fileInputRef = ref(null);
  const toastMsg = ref('');

  const uploadForm = ref({
    name: ''
  });

  const documents = ref([
    {
      id: 1,
      name: 'Huong-dan-viet-bao-cao.pdf',
      size: '2.4 MB',
      date: '12/07/2026',
      source: 'Khoa',
      status: 'Äang lÆ°u hÃ nh',
      type: 'pdf'
    },
    {
      id: 2,
      name: 'Mau-phieu-cham-diem.docx',
      size: '45 KB',
      date: '15/07/2026',
      source: 'Báº¡n',
      status: 'Äang lÆ°u hÃ nh',
      type: 'doc'
    },
    {
      id: 3,
      name: 'Danh-sach-doanh-nghiep-2026.xlsx',
      size: '1.2 MB',
      date: '05/06/2026',
      source: 'Báº¡n',
      status: 'Ngá»«ng sá»­ dá»¥ng',
      type: 'xls'
    },
    {
      id: 4,
      name: 'Quy-dinh-thuc-tap-K64.pdf',
      size: '3.1 MB',
      date: '20/07/2026',
      source: 'Khoa',
      status: 'Äang lÆ°u hÃ nh',
      type: 'pdf'
    }
  ]);

  const filteredDocuments = computed(() => {
    return documents.value.filter(doc => {
      const matchSource = sourceFilter.value === 'ALL' || doc.source === sourceFilter.value;
      const matchStatus = statusFilter.value === 'ALL' || doc.status === statusFilter.value;
      const matchSearch = !searchQuery.value || doc.name.toLowerCase().includes(searchQuery.value.toLowerCase());
      return matchSource && matchStatus && matchSearch;
    });
  });

  function getSourceBadgeClass(source) {
    return source === 'Khoa' ? 'bg-blue-100 text-blue-800' : 'bg-amber-100 text-amber-800';
  }

  function getFileIconClass(type) {
    switch (type) {
      case 'pdf':
        return 'bg-rose-100 text-rose-700';
      case 'doc':
      case 'docx':
        return 'bg-blue-100 text-blue-700';
      case 'xls':
      case 'xlsx':
        return 'bg-emerald-100 text-emerald-700';
      default:
        return 'bg-slate-100 text-slate-700';
    }
  }

  function getFileIconSymbol(type) {
    switch (type) {
      case 'pdf':
        return 'picture_as_pdf';
      case 'doc':
      case 'docx':
        return 'description';
      case 'xls':
      case 'xlsx':
        return 'table_chart';
      default:
        return 'description';
    }
  }

  function triggerFileInput() {
    if (fileInputRef.value) {
      fileInputRef.value.click();
    }
  }

  function onFileSelected(e) {
    const file = e.target.files[0];
    if (file) {
      selectedFile.value = file;
    }
  }

  function submitUpload() {
    if (!selectedFile.value && !uploadForm.value.name.trim()) {
      showToast('Vui lÃ²ng chá»n tá»‡p tin hoáº·c nháº­p tÃªn tÃ i liá»‡u!');
      return;
    }

    const newDoc = {
      id: Date.now(),
      name: uploadForm.value.name.trim() || selectedFile.value?.name || 'Tai-lieu-moi.docx',
      size: selectedFile.value ? (selectedFile.value.size / (1024*1024)).toFixed(2) + ' MB' : '1.5 MB',
      date: new Date().toLocaleDateString('vi-VN'),
      source: 'Báº¡n',
      status: 'Äang lÆ°u hÃ nh',
      type: 'doc'
    };

    documents.value.unshift(newDoc);
    showUploadDrawer.value = false;
    uploadForm.value.name = '';
    selectedFile.value = null;
    showToast('âœ“ ÄÃ£ Ä‘Äƒng táº£i tÃ i liá»‡u thÃ nh cÃ´ng!');
  }

  function downloadDoc(doc) {
    showToast(`Äang táº£i xuá»‘ng tÃ i liá»‡u "${doc.name}"...`);
  }

  function toggleHideDoc(doc) {
    doc.status = doc.status === 'Äang lÆ°u hÃ nh' ? 'Ngá»«ng sá»­ dá»¥ng' : 'Äang lÆ°u hÃ nh';
    showToast(`ÄÃ£ chuyá»ƒn tráº¡ng thÃ¡i tÃ i liá»‡u sang "${doc.status}"`);
  }

  function confirmDeleteDoc(doc) {
    docToDelete.value = doc;
  }

  function executeDeleteDoc() {
    if (docToDelete.value) {
      documents.value = documents.value.filter(d => d.id !== docToDelete.value.id);
      showToast('âœ“ ÄÃ£ xÃ³a vÄ©nh viá»…n tÃ i liá»‡u!');
      docToDelete.value = null;
    }
  }

  function showToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }
</script>
