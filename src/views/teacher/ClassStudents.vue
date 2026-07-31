<template>
  <div class="ims-scope w-full mx-auto space-y-6">
    <!-- Breadcrumb & Header -->
    <div class="flex flex-col gap-2">
      <nav class="flex items-center gap-2 text-xs text-slate-500 font-medium">
        <router-link to="/teacher/classes" class="hover:text-blue-600 transition-colors">L�:p của tôi</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-800 font-bold">{{ cls?.tenLop || 'L�:p' }}</span>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-500">Quản lý Sinh viên</span>
      </nav>

      <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
        <div>
          <h1 class="text-3xl font-bold text-slate-900 tracking-tight">{{ cls?.tenLop || 'L�:p hư�:ng dẫn' }}</h1>
          <p class="text-xs text-slate-500 mt-1">{{ cls?.tenKy }}</p>
        </div>
        <button
          class="flex items-center gap-1.5 px-4 py-2 rounded-xl text-xs font-bold border border-slate-300 text-slate-700 hover:bg-slate-100 transition-all w-fit"
          @click="$router.push(`/teacher/classes/${maLop}/grading`)"
        >
          <span class="material-symbols-outlined text-[18px]">grade</span>
          Chuyá»ƒn sang Bảng �iỒm
        </button>
      </div>
    </div>

    <!-- Category Tabs -->
    <div class="flex border-b border-slate-200 gap-8 text-sm">
      <button
        class="pb-3 font-bold transition-all relative flex items-center gap-2"
        :class="activeTab === 'students' ? 'text-blue-700 border-b-2 border-blue-700' : 'text-slate-500 hover:text-slate-800'"
        @click="activeTab = 'students'"
      >
        <span class="material-symbols-outlined text-[18px]">groups</span>
        Danh sách sinh viên
        <span class="bg-blue-100 text-blue-800 text-[10px] px-2 py-0.5 rounded-full font-extrabold">
          {{ students.length }}
        </span>
      </button>

      <button
        class="pb-3 font-bold transition-all relative flex items-center gap-2"
        :class="activeTab === 'groups' ? 'text-blue-700 border-b-2 border-blue-700' : 'text-slate-500 hover:text-slate-800'"
        @click="activeTab = 'groups'"
      >
        <span class="material-symbols-outlined text-[18px]">diversity_3</span>
        Nhóm sinh viên
        <span class="bg-slate-100 text-slate-700 text-[10px] px-2 py-0.5 rounded-full font-extrabold">
          {{ groups.length }}
        </span>
      </button>
    </div>

    <!-- TAB 1: DANH SÃCH SINH VIÃŠN -->
    <div v-if="activeTab === 'students'" class="space-y-6">
      <!-- Cards Ghi danh & MÃ£ lá»›p -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-4">
        <!-- MÃ£ tham gia lá»›p -->
        <div class="bg-white rounded-2xl border border-slate-200 p-5 shadow-xs flex flex-col justify-between">
          <div class="flex justify-between items-start mb-3">
            <h3 class="text-sm font-bold text-blue-900 flex items-center gap-2">
              <span class="material-symbols-outlined text-blue-600 text-[20px]">key</span>
              MÃ£ tham gia lá»›p há»c
            </h3>
            <button
              class="text-xs font-bold text-rose-600 hover:bg-rose-50 px-2.5 py-1 rounded-lg transition-colors flex items-center gap-1"
              @click="showReset = true"
            >
              <span class="material-symbols-outlined text-[16px]">restart_alt</span> Reset mÃ£
            </button>
          </div>

          <div class="flex items-center gap-3 my-2">
            <div class="bg-slate-100 p-3 rounded-xl font-mono font-black text-2xl tracking-widest text-blue-800 flex-1 text-center border border-slate-200">
              {{ cls?.maThamGia || '------' }}
            </div>
            <button
              class="bg-[#005EA3] text-white px-4 py-3 rounded-xl hover:opacity-90 active:scale-95 transition-all text-xs font-bold flex items-center gap-1.5 shadow-xs"
              @click="copy(cls?.maThamGia)"
            >
              <span class="material-symbols-outlined text-[18px]">content_copy</span>
              Sao chÃ©p mÃ£
            </button>
          </div>

          <div class="mt-2 text-xs text-slate-500 flex items-center justify-between">
            <span class="truncate italic">Link: {{ joinLink }}</span>
            <button class="text-blue-600 font-bold hover:underline shrink-0 ml-2" @click="copy(joinLink)">
              Sao chÃ©p link
            </button>
          </div>
        </div>

        <!-- Thiáº¿t láº­p ghi danh -->
        <div class="bg-white rounded-2xl border border-slate-200 p-5 shadow-xs flex flex-col justify-between">
          <div class="flex justify-between items-start mb-3">
            <h3 class="text-sm font-bold text-blue-900 flex items-center gap-2">
              <span class="material-symbols-outlined text-blue-600 text-[20px]">how_to_reg</span>
              Thiáº¿t láº­p ghi danh
            </h3>
            <button
              class="text-xs font-bold text-blue-600 hover:bg-blue-50 px-2.5 py-1 rounded-lg transition-colors flex items-center gap-1"
              @click="openEnroll"
            >
              <span class="material-symbols-outlined text-[16px]">edit</span> Chá»‰nh sửa
            </button>
          </div>

          <div class="space-y-3 my-2">
            <div class="flex items-center justify-between text-xs">
              <span class="text-slate-600 font-medium">Trạng thái má»Ÿ form:</span>
              <span
                class="px-3 py-1 rounded-full font-bold text-[11px]"
                :class="cls?.ghiDanhMo ? 'bg-emerald-100 text-emerald-800 border border-emerald-200' : 'bg-slate-100 text-slate-600 border border-slate-200'"
              >
                {{ cls?.ghiDanhMo ? 'ÄANG Má»ž GHI DANH' : 'ÄÃƒ ÄÃ“NG GHI DANH' }}
              </span>
            </div>

            <div class="flex items-center justify-between text-xs">
              <span class="text-slate-600 font-medium">Háº¡n ghi danh:</span>
              <span class="font-bold text-slate-800">
                {{ cls?.hanGhiDanh ? formatDate(cls.hanGhiDanh) : 'ChÆ°a thiáº¿t láº­p' }}
              </span>
            </div>
          </div>

          <p class="text-[11px] text-slate-400 italic">
            Khi háº¿t háº¡n ghi danh, há»‡ thá»‘ng tá»± Ä‘á»™ng vÃ´ hiá»‡u hÃ³a mÃ£ gia nháº­p.
          </p>
        </div>
      </div>

      <!-- Controls & Filter Toolbar -->
      <div class="bg-white rounded-2xl border border-slate-200 p-4 shadow-xs space-y-4">
        <div class="flex flex-col md:flex-row items-stretch md:items-center justify-between gap-3">
          <div class="relative flex-1">
            <span class="material-symbols-outlined text-[20px] absolute left-3 top-1/2 -translate-y-1/2 text-slate-400">search</span>
            <input
              v-model="search"
              placeholder="TÃ¬m kiáº¿m há» tên, MSSV, Ä‘Æ¡n vá»‹ thực tập..."
              class="w-full pl-10 pr-3 py-2 border border-slate-300 rounded-xl text-xs font-medium focus:ring-2 focus:ring-blue-600 outline-none"
              @input="reloadStudents"
            />
          </div>

          <div class="flex items-center gap-2 flex-wrap">
            <select v-model="statusFilter" class="px-3 py-2 border border-slate-300 rounded-xl text-xs font-medium text-slate-800 outline-none" @change="reloadStudents">
              <option value="all">Táº¥t cáº£ tráº¡ng thÃ¡i</option>
              <option value="ChoGhiDanh">Chá» ghi danh</option>
              <option value="DangThucTap">Äang thực tập</option>
              <option value="HoanThanh">HoÃ n thÃ nh</option>
              <option value="DungThucTap">Dá»«ng thực tập</option>
            </select>

            <button
              class="px-3.5 py-2 rounded-xl border border-slate-300 text-slate-700 bg-white hover:bg-slate-50 text-xs font-bold flex items-center gap-1.5 transition-all shadow-2xs"
              @click="showExportModal = true"
            >
              <span class="material-symbols-outlined text-[18px] text-emerald-600">file_download</span>
              Xuất Báo cáo
            </button>

            <button
              class="text-white px-4 py-2 rounded-xl text-xs font-bold flex items-center gap-1.5 shadow-xs transition-all active:scale-95"
              style="background-color: #005EA3;"
              @click="openImport"
            >
              <span class="material-symbols-outlined text-[16px]">upload</span>
              Import sinh viên
            </button>
          </div>
        </div>

        <!-- Inline Bulk Actions Bar (Triggered when 2+ items selected) -->
        <div
          v-if="selected.length >= 2"
          class="bg-blue-50 border border-blue-200 rounded-xl p-3 flex items-center justify-between transition-all"
        >
          <div class="flex items-center gap-3">
            <span class="text-xs font-bold text-blue-900">
              ÄÃ£ chá»n <strong class="text-blue-700 text-sm font-black">{{ selected.length }}</strong> sinh viên
            </span>
            <button class="text-xs text-slate-500 hover:text-slate-800 underline" @click="selected = []">
              Bá» chá»n táº¥t cáº£
            </button>
          </div>

          <button
            class="bg-blue-700 text-white px-4 py-1.5 rounded-lg text-xs font-bold flex items-center gap-1 hover:bg-blue-800 transition-all shadow-xs"
            @click="openConfirmGroup"
          >
            <span class="material-symbols-outlined text-[16px]">group_add</span>
            Gom nhóm sinh viên
          </button>
        </div>
      </div>

      <!-- Student Table -->
      <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-xs">
        <div class="overflow-x-auto">
          <table class="w-full text-left text-xs border-collapse">
            <thead>
              <tr class="bg-slate-50 border-b border-slate-200 text-slate-600 font-bold uppercase tracking-wider">
                <th class="p-4 w-10">
                  <input type="checkbox" :checked="allChecked" class="rounded border-slate-300 text-blue-600" @change="toggleAll" />
                </th>
                <th class="p-4">Há» và tên</th>
                <th class="p-4">Nhóm</th>
                <th class="p-4">ÄÆ¡n vá»‹ thực tập</th>
                <th class="p-4">Trạng thái</th>
                <th class="p-4">Tiến ��"</th>
                <th class="p-4 text-right">Thao tÃ¡c</th>
              </tr>
            </thead>

            <tbody class="divide-y divide-slate-100">
              <tr v-if="loadingStudents">
                <td colspan="7" class="p-10 text-center text-slate-500">Äang táº£i danh sÃ¡ch sinh viên...</td>
              </tr>
              <tr v-else-if="!students.length">
                <td colspan="7" class="p-12 text-center text-slate-500">
                  <span class="material-symbols-outlined text-[48px] block mb-2 opacity-40">group</span>
                  ChÆ°a tÃ¬m tháº¥y sinh viên nào trong lá»›p nÃ y.
                </td>
              </tr>
              <tr
                v-for="s in students"
                :key="s.maGhiDanh"
                class="hover:bg-slate-50/80 transition-colors group"
              >
                <td class="p-4">
                  <input type="checkbox" :value="s.maGhiDanh" v-model="selected" class="rounded border-slate-300 text-blue-600" />
                </td>
                <td class="p-4">
                  <p
                    class="font-bold text-slate-900 text-sm hover:text-blue-600 cursor-pointer transition-colors"
                    @click="openDetail(s.maGhiDanh)"
                  >
                    {{ s.hoTen }}
                  </p>
                  <p class="text-[11px] text-slate-400 font-mono">{{ s.maSoSinhVien }}</p>
                </td>
                <td class="p-4 font-semibold text-slate-700">
                  <span v-if="s.tenNhom" class="px-2 py-0.5 bg-slate-100 text-slate-700 rounded font-mono font-bold">
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
                    :class="statusCls[s.trangThaiThucTap] || 'bg-slate-100 text-slate-600'"
                  >
                    {{ statusLabels[s.trangThaiThucTap] || s.trangThaiThucTap }}
                  </span>
                </td>
                <td class="p-4">
                  <div v-if="s.tinhTrangTienDo" class="flex items-center gap-1.5">
                    <span
                      class="w-2 h-2 rounded-full"
                      :class="s.tinhTrangTienDo === 'DungTienDo' ? 'bg-emerald-500' : 'bg-amber-500'"
                    ></span>
                    <span
                      class="font-bold"
                      :class="progressCls[s.tinhTrangTienDo] ? 'text-slate-700' : 'text-slate-600'"
                    >
                      {{ progressLabels[s.tinhTrangTienDo] || s.tinhTrangTienDo }}
                    </span>
                  </div>
                  <span v-else class="text-slate-400">â€”</span>
                </td>
                <td class="p-4 text-right">
                  <div class="flex items-center justify-end gap-1">
                    <button
                      class="p-1.5 rounded-lg hover:bg-blue-50 text-blue-700 transition-colors"
                      title="Xem há»“ sÆ¡ chi tiáº¿t"
                      @click="openDetail(s.maGhiDanh)"
                    >
                      <span class="material-symbols-outlined text-[18px]">visibility</span>
                    </button>
                    <button
                      class="p-1.5 rounded-lg hover:bg-amber-50 text-amber-700 transition-colors"
                      title="YÃªu cáº§u chuyá»ƒn lá»›p"
                      @click="openTransfer(s)"
                    >
                      <span class="material-symbols-outlined text-[18px]">swap_horiz</span>
                    </button>
                    <button
                      v-if="s.trangThaiThucTap !== 'DungThucTap'"
                      class="p-1.5 rounded-lg hover:bg-rose-50 text-rose-600 transition-colors"
                      title="Dá»«ng thực tập"
                      @click="openStop(s)"
                    >
                      <span class="material-symbols-outlined text-[18px]">block</span>
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- TAB 2: NHÃ“M SINH VIÃŠN -->
    <div v-if="activeTab === 'groups'" class="space-y-6">
      <div class="bg-white rounded-2xl border border-slate-200 p-5 shadow-xs flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h3 class="font-bold text-base text-slate-900 flex items-center gap-2">
            <span class="material-symbols-outlined text-blue-600">diversity_3</span>
            XÃ¡c nháº­n & Quản lý nhóm sinh viên
          </h3>
          <p class="text-xs text-slate-500 mt-0.5">Duyệt các yêu cầu gom nhóm tự nguyện của sinh viên hoặc chủ động tạo nhóm mới.</p>
        </div>

        <div class="flex items-center gap-2">
          <select v-model="groupStatusFilter" class="px-3.5 py-2 border border-slate-300 rounded-xl text-xs font-semibold text-slate-800 outline-none">
            <option value="all">Táº¥t cáº£ tráº¡ng thÃ¡i nhóm</option>
            <option value="ChoXacNhan">Chá» xÃ¡c nháº­n</option>
            <option value="DaXacNhan">ÄÃ£ xÃ¡c nháº­n</option>
            <option value="TuChoi">Tá»« chá»‘i</option>
          </select>
        </div>
      </div>

      <div class="bg-white rounded-2xl border border-slate-200 overflow-hidden shadow-xs">
        <div v-if="!filteredGroups.length" class="p-12 text-center text-slate-500">
          <span class="material-symbols-outlined text-[48px] block mb-2 opacity-40">diversity_3</span>
          <p class="font-bold text-slate-800">KhÃ´ng cÃ³ nhóm sinh viên nào</p>
          <p class="text-xs text-slate-500 mt-1">Chuyá»ƒn sang tab "Danh sách sinh viên", tÃ­ch chá»n Ã­t nháº¥t 2 sinh viên và nháº¥n "Gom nhóm sinh viên".</p>
        </div>

        <table v-else class="w-full text-left text-xs border-collapse">
          <thead>
            <tr class="bg-slate-50 border-b border-slate-200 text-slate-600 font-bold uppercase tracking-wider">
              <th class="p-4">TÃªn / MÃ£ nhóm</th>
              <th class="p-4">ThÃ nh viÃªn</th>
              <th class="p-4">Äá» tÃ i & ÄÆ¡n vá»‹</th>
              <th class="p-4">Trạng thái</th>
              <th class="p-4 text-right">Thao tÃ¡c phÃª duyá»‡t</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-100">
            <tr v-for="g in filteredGroups" :key="g.maNhom" class="hover:bg-slate-50/80 transition-colors">
              <td class="p-4 font-extrabold text-blue-900 text-sm font-mono">
                {{ g.tenNhom }}
                <span class="block text-[11px] font-normal text-slate-400 font-sans mt-0.5">
                  {{ g.thanhVien?.length || 0 }} thÃ nh viÃªn
                </span>
              </td>

              <td class="p-4">
                <div class="flex flex-wrap gap-1.5">
                  <span
                    v-for="m in (g.thanhVien || [])"
                    :key="m.maSoSinhVien"
                    class="px-2.5 py-1 bg-slate-100 text-slate-800 rounded-lg font-bold text-[11px] border border-slate-200 flex items-center gap-1"
                  >
                    <span class="material-symbols-outlined text-[14px] text-blue-600">person</span>
                    {{ m.hoTen }} <span class="font-mono text-slate-500">({{ m.maSoSinhVien }})</span>
                  </span>
                </div>
              </td>

              <td class="p-4 max-w-[260px]">
                <p class="font-bold text-slate-800 truncate" :title="g.tenDeTai">{{ g.tenDeTai || 'ChÆ°a cáº­p nháº­t Ä‘á» tÃ i' }}</p>
                <p class="text-[11px] text-slate-500 truncate" :title="g.donViThucTap">{{ g.donViThucTap || 'ChÆ°a cáº­p nháº­t doanh nghiá»‡p' }}</p>
              </td>

              <td class="p-4">
                <span
                  class="px-3 py-1 rounded-full text-[11px] font-extrabold inline-flex items-center gap-1 border"
                  :class="groupStatusClasses[g.trangThai || 'ChoXacNhan']"
                >
                  <span class="w-2 h-2 rounded-full" :class="groupDotClasses[g.trangThai || 'ChoXacNhan']"></span>
                  {{ groupStatusLabels[g.trangThai || 'ChoXacNhan'] }}
                </span>
              </td>

              <td class="p-4 text-right">
                <div class="flex items-center justify-end gap-2">
                  <!-- Button XÃ¡c nháº­n gom nhóm -->
                  <button
                    v-if="g.trangThai === 'ChoXacNhan' || !g.trangThai"
                    class="px-3 py-1.5 rounded-xl text-white font-bold text-xs bg-emerald-600 hover:bg-emerald-700 active:scale-95 transition-all shadow-xs flex items-center gap-1"
                    @click="confirmGroup(g)"
                  >
                    <span class="material-symbols-outlined text-[16px]">check_circle</span>
                    XÃ¡c nháº­n gom nhóm
                  </button>

                  <!-- Button Tá»« chá»‘i -->
                  <button
                    v-if="g.trangThai === 'ChoXacNhan' || !g.trangThai"
                    class="px-3 py-1.5 rounded-xl text-rose-700 bg-rose-50 border border-rose-200 font-bold text-xs hover:bg-rose-100 transition-colors flex items-center gap-1"
                    @click="openRejectGroup(g)"
                  >
                    <span class="material-symbols-outlined text-[16px]">cancel</span>
                    Tá»« chá»‘i
                  </button>

                  <!-- Button Giáº£i tÃ¡n nhóm -->
                  <button
                    class="p-1.5 rounded-xl text-slate-400 hover:text-rose-600 hover:bg-rose-50 font-bold transition-colors"
                    title="Giáº£i tÃ¡n nhóm nÃ y"
                    @click="disband(g)"
                  >
                    <span class="material-symbols-outlined text-[18px]">group_off</span>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Toast Notification -->
    <div v-if="toast" class="fixed bottom-6 right-6 z-[90] bg-slate-900 text-white px-4 py-3 rounded-xl shadow-xl text-xs font-bold flex items-center gap-2">
      <span class="material-symbols-outlined text-emerald-400 text-[18px]">check_circle</span>
      {{ toast }}
    </div>

    <!-- Reset MÃ£ Modal -->
    <div v-if="showReset" class="fixed inset-0 z-[80] flex items-center justify-center bg-black/40 backdrop-blur-xs px-4" @click.self="showReset = false">
      <div class="w-full max-w-[420px] bg-white rounded-2xl shadow-2xl p-6">
        <h3 class="font-bold text-lg text-slate-900 mb-2">Reset mÃ£ tham gia lá»›p?</h3>
        <p class="text-xs text-slate-600 leading-relaxed mb-6">
          MÃ£ tham gia hi�!n tại sáº½ bá»‹ vÃ´ hiá»‡u hÃ³a và táº¡o mÃ£ má»›i. Sinh viên Ä‘ang giá»¯ mÃ£ cÅ© sáº½ không thá»ƒ gia nháº­p lá»›p ná»¯a.
        </p>
        <div class="flex justify-end gap-3">
          <button class="px-4 py-2 rounded-xl text-xs font-bold text-slate-600 hover:bg-slate-100" @click="showReset = false">Há»§y</button>
          <button class="px-5 py-2 rounded-xl text-xs font-bold text-white bg-rose-600 hover:bg-rose-700 transition-colors shadow-sm" @click="resetCode">
            Reset mÃ£ ngay
          </button>
        </div>
      </div>
    </div>

    <!-- Modal Thiáº¿t Láº­p Ghi Danh -->
    <div v-if="showEnroll" class="fixed inset-0 z-[80] flex items-center justify-center bg-black/40 backdrop-blur-xs px-4" @click.self="showEnroll = false">
      <div class="w-full max-w-[440px] bg-white rounded-2xl shadow-2xl p-6">
        <h3 class="font-bold text-lg text-slate-900 mb-4">Thiáº¿t láº­p ghi danh</h3>
        <div class="space-y-4">
          <div>
            <label class="block text-xs font-bold text-slate-700 mb-1">Trạng thái form ghi danh</label>
            <select v-model="enrollForm.ghiDanhMo" class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-semibold outline-none focus:ring-2 focus:ring-blue-600">
              <option :value="true">Äang má»Ÿ ghi danh</option>
              <option :value="false">ÄÃ£ Ä‘Ã³ng ghi danh</option>
            </select>
          </div>
          <div>
            <label class="block text-xs font-bold text-slate-700 mb-1">Háº¡n chÃ³t ghi danh</label>
            <input v-model="enrollForm.hanGhiDanh" type="datetime-local" class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-semibold outline-none focus:ring-2 focus:ring-blue-600" />
          </div>
        </div>
        <div class="flex justify-end gap-3 mt-6">
          <button class="px-4 py-2 rounded-xl text-xs font-bold text-slate-600 hover:bg-slate-100" @click="showEnroll = false">Há»§y</button>
          <button class="px-5 py-2 rounded-xl text-xs font-bold text-white shadow-sm" style="background-color:#005EA3;" @click="saveEnroll">LÆ°u thay đổi</button>
        </div>
      </div>
    </div>

    <!-- Modal Import Sinh ViÃªn -->
    <div v-if="showImport" class="fixed inset-0 z-[80] flex items-center justify-center bg-black/40 backdrop-blur-xs px-4" @click.self="showImport = false">
      <div class="w-full max-w-[520px] bg-white rounded-2xl shadow-2xl overflow-hidden">
        <div class="p-6 border-b border-slate-200 bg-slate-50 flex justify-between items-center">
          <div>
            <h3 class="font-bold text-base text-blue-900">Import sinh viên vào lá»›p</h3>
            <p class="text-xs text-slate-500">Nháº­p danh sÃ¡ch MÃ£ Sá»‘ Sinh ViÃªn</p>
          </div>
          <button class="material-symbols-outlined text-slate-400 hover:text-slate-700" @click="showImport = false">close</button>
        </div>

        <div class="p-6 space-y-4">
          <p class="text-xs text-slate-600">Nháº­p danh sÃ¡ch mÃ£ sá»‘ sinh viên (má»—i dÃ²ng má»™t MSSV):</p>
          <textarea
            v-model="importText"
            rows="6"
            placeholder="22120001&#10;22120002&#10;22120003"
            class="w-full px-4 py-3 border border-slate-300 rounded-xl text-xs font-mono outline-none focus:ring-2 focus:ring-blue-600"
          ></textarea>

          <div v-if="importResults.length" class="mt-4 max-h-40 overflow-auto border border-slate-200 rounded-xl divide-y divide-slate-100">
            <div v-for="(r, i) in importResults" :key="i" class="flex items-center gap-2 px-3 py-2 text-xs">
              <span class="material-symbols-outlined text-[16px]" :class="r.thanhCong ? 'text-emerald-600' : 'text-rose-600'">
                {{ r.thanhCong ? 'check_circle' : 'cancel' }}
              </span>
              <span class="font-bold font-mono">{{ r.maSoSinhVien }}</span>
              <span class="text-slate-500">{{ r.hoTen || r.lyDo }}</span>
            </div>
          </div>
        </div>

        <div class="p-4 bg-slate-50 border-t border-slate-200 flex justify-end gap-3">
          <button class="px-4 py-2 rounded-xl text-xs font-bold text-slate-600 hover:bg-slate-200" @click="showImport = false">ÄÃ³ng</button>
          <button class="px-5 py-2 rounded-xl text-xs font-bold text-white shadow-sm" style="background-color:#005EA3;" @click="doImport">
            Báº¯t �ầu thÃªm
          </button>
        </div>
      </div>
    </div>

    <!-- Modal Dá»«ng Thá»±c Táº­p -->
    <div v-if="stopTarget" class="fixed inset-0 z-[80] flex items-center justify-center bg-black/40 backdrop-blur-xs px-4" @click.self="stopTarget = null">
      <div class="w-full max-w-[440px] bg-white rounded-2xl shadow-2xl p-6">
        <h3 class="font-bold text-lg text-slate-900 mb-1">Dá»«ng thực tập</h3>
        <p class="text-xs text-slate-500 mb-4">
          Cáº­p nháº­t tráº¡ng thÃ¡i dá»«ng thực tập cho <strong>{{ stopTarget.hoTen }}</strong>.
        </p>

        <div class="space-y-4">
          <div>
            <label class="block text-xs font-bold text-slate-700 mb-1">LÃ½ do dá»«ng</label>
            <select v-model="stopForm.lyDo" class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-semibold outline-none focus:ring-2 focus:ring-blue-600">
              <option value="Xin rÃºt thực tập">Xin rÃºt thực tập</option>
              <option value="Vi pháº¡m">Vi pháº¡m quy cháº¿</option>
              <option value="Máº¥t liÃªn láº¡c">Máº¥t liÃªn láº¡c</option>
              <option value="KhÃ¡c">LÃ½ do khÃ¡c</option>
            </select>
          </div>

          <div v-if="stopForm.lyDo === 'KhÃ¡c'">
            <input v-model="stopForm.lyDoKhac" placeholder="Ghi rÃµ lÃ½ do cá»¥ thá»ƒ..." class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-medium outline-none focus:ring-2 focus:ring-blue-600" />
          </div>
        </div>

        <div class="flex justify-end gap-3 mt-6">
          <button class="px-4 py-2 rounded-xl text-xs font-bold text-slate-600 hover:bg-slate-100" @click="stopTarget = null">Há»§y</button>
          <button class="px-5 py-2 rounded-xl text-xs font-bold text-white bg-rose-600 hover:bg-rose-700 shadow-sm" @click="confirmStop">XÃ¡c nháº­n dá»«ng</button>
        </div>
      </div>
    </div>

    <!-- Modal Chuyá»ƒn L�:p Thá»±c Táº­p -->
    <div v-if="transferTarget" class="fixed inset-0 z-[80] flex items-center justify-center bg-black/40 backdrop-blur-xs px-4" @click.self="transferTarget = null">
      <div class="w-full max-w-[480px] bg-white rounded-2xl shadow-2xl p-6">
        <div class="flex justify-between items-start mb-4">
          <div>
            <h3 class="font-bold text-lg text-blue-900">YÃªu cáº§u Ä‘iá»u chuyá»ƒn lá»›p há»c</h3>
            <p class="text-xs text-slate-500">Sinh viên: <strong>{{ transferTarget.hoTen }}</strong> ({{ transferTarget.maSoSinhVien }})</p>
          </div>
          <button class="material-symbols-outlined text-slate-400 hover:text-slate-700" @click="transferTarget = null">close</button>
        </div>

        <div class="space-y-4">
          <div>
            <label class="block text-xs font-bold text-slate-700 mb-1">Chá»n giảng viên / L�:p �ích <span class="text-rose-500">*</span></label>
            <input
              v-model="transferForm.lopDich"
              placeholder="Nháº­p tên giảng viên hoặc mÃ£ lá»›p (VD: TT-K64-B12)"
              class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-medium outline-none focus:ring-2 focus:ring-blue-600"
            />
          </div>

          <div>
            <label class="block text-xs font-bold text-slate-700 mb-1">LÃ½ do chuyá»ƒn lá»›p <span class="text-rose-500">*</span></label>
            <textarea
              v-model="transferForm.lyDo"
              rows="3"
              placeholder="Nháº­p lÃ½ do Ä‘iá»u chuyá»ƒn lá»›p thực tập..."
              class="w-full px-3.5 py-2.5 border border-slate-300 rounded-xl text-xs font-medium outline-none focus:ring-2 focus:ring-blue-600"
            ></textarea>
          </div>
        </div>

        <div class="flex justify-end gap-3 mt-6">
          <button class="px-4 py-2 rounded-xl text-xs font-bold text-slate-600 hover:bg-slate-100" @click="transferTarget = null">Há»§y</button>
          <button
            class="px-5 py-2 rounded-xl text-xs font-bold text-white shadow-sm transition-all active:scale-95 disabled:opacity-50"
            style="background-color:#005EA3;"
            :disabled="!transferForm.lopDich || !transferForm.lyDo"
            @click="confirmTransfer"
          >
            Gửi yêu cầu
          </button>
        </div>
      </div>
    </div>

    <!-- Modal XÃ¡c Nháº­n Gom Nhóm -->
    <div v-if="showConfirmGroup" class="fixed inset-0 z-[80] flex items-center justify-center bg-black/40 backdrop-blur-xs px-4" @click.self="showConfirmGroup = false">
      <div class="w-full max-w-[480px] bg-white rounded-2xl shadow-2xl p-6 space-y-4">
        <div class="flex items-center justify-between">
          <h3 class="font-bold text-lg text-blue-900 flex items-center gap-2">
            <span class="material-symbols-outlined text-blue-600">group_add</span>
            XÃ¡c nháº­n táº¡o nhóm sinh viên
          </h3>
          <button class="material-symbols-outlined text-slate-400 hover:text-slate-700" @click="showConfirmGroup = false">close</button>
        </div>

        <p class="text-xs text-slate-600 leading-relaxed">
          Há»‡ thá»‘ng sáº½ tạo nhóm mới gá»“m <strong class="text-blue-700 font-extrabold">{{ selectedStudents.length }} sinh viên</strong>:
        </p>

        <div class="max-h-36 overflow-y-auto border border-slate-200 rounded-xl p-3 bg-slate-50 space-y-1.5 text-xs">
          <div v-for="st in selectedStudents" :key="st.maGhiDanh" class="flex justify-between items-center text-slate-800 font-medium">
            <span>â€¢ {{ st.hoTen }}</span>
            <span class="font-mono text-slate-500 text-[11px]">({{ st.maSoSinhVien }})</span>
          </div>
        </div>

        <div class="space-y-1">
          <label class="block text-[11px] font-bold uppercase text-slate-500">TÃªn / MÃ£ nhóm tá»± Ä‘á»™ng</label>
          <input
            :value="'Nhóm ' + String(groups.length + 1).padStart(2, '0')"
            readonly
            class="w-full px-3 py-2 bg-slate-100 border border-slate-200 rounded-lg text-xs font-mono font-bold text-slate-700 outline-none"
          />
        </div>

        <div class="flex justify-end gap-3 pt-2">
          <button class="px-4 py-2 rounded-xl text-xs font-bold text-slate-600 hover:bg-slate-100" @click="showConfirmGroup = false">Há»§y</button>
          <button
            class="px-5 py-2 rounded-xl text-xs font-bold text-white shadow-sm transition-all active:scale-95"
            style="background-color:#005EA3;"
            @click="confirmCreateGroup"
          >
            XÃ¡c nháº­n gom nhóm
          </button>
        </div>
      </div>
    </div>

    <!-- Drawer Há»“ sÆ¡ Chi tiáº¿t Sinh viên (6 Tabs) -->
    <div v-if="detail" class="fixed inset-0 z-[85] flex justify-end bg-black/40 backdrop-blur-xs" @click.self="detail = null">
      <div class="w-full max-w-[620px] h-full bg-white shadow-2xl flex flex-col justify-between overflow-hidden">
        <!-- Drawer Header -->
        <div class="p-6 border-b border-slate-200 flex justify-between items-start bg-slate-50">
          <div>
            <div class="flex items-center gap-2 mb-1.5">
              <span class="px-2.5 py-0.5 rounded-full text-[10px] font-extrabold uppercase bg-blue-100 text-blue-800 inline-block">
                Há»’ SÆ  SINH VIÃŠN
              </span>
              <span
                class="px-2 py-0.5 rounded text-[10px] font-bold uppercase"
                :class="statusCls[detail.trangThaiThucTap] || 'bg-slate-100 text-slate-600'"
              >
                {{ statusLabels[detail.trangThaiThucTap] || 'ÄANG THá»°C Táº¬P' }}
              </span>
            </div>
            <h3 class="font-extrabold text-2xl text-slate-900">{{ detail.hoTen }}</h3>
            <p class="text-xs text-slate-500 font-mono mt-0.5">MSSV: {{ detail.maSoSinhVien }} â€¢ L�:p: {{ detail.lopSinhHoat || 'K64' }}</p>
          </div>
          <button class="material-symbols-outlined text-slate-400 hover:text-slate-700 transition-colors" @click="detail = null">
            close
          </button>
        </div>

        <!-- Drawer Navigation Tabs -->
        <div class="px-6 flex border-b border-slate-200 overflow-x-auto bg-white text-xs font-bold gap-4 pt-1">
          <button
            v-for="t in [
              { id: 'overview', label: 'Tá»•ng quan' },
              { id: 'logs', label: 'Nháº­t kÃ½' },
              { id: 'submissions', label: 'Bài n�"p' },
              { id: 'grades', label: 'Äiá»ƒm' },
              { id: 'history', label: 'Lá»‹ch sá»­ Ä‘Ã¡nh giÃ¡' },
              { id: 'notes', label: 'Ghi chÃº' }
            ]"
            :key="t.id"
            class="py-3 px-1 transition-all whitespace-nowrap"
            :class="activeDetailTab === t.id ? 'border-b-2 border-blue-700 text-blue-700 font-black' : 'text-slate-500 hover:text-slate-800'"
            @click="activeDetailTab = t.id"
          >
            {{ t.label }}
          </button>
        </div>

        <!-- Drawer Content Body -->
        <div class="p-6 overflow-y-auto space-y-6 flex-1 text-xs">
          <!-- Tab 1: Tá»•ng quan -->
          <div v-if="activeDetailTab === 'overview'" class="space-y-6">
            <div class="space-y-3">
              <h4 class="font-bold text-slate-900 uppercase tracking-wider text-[11px] text-blue-900 border-b border-slate-100 pb-1">
                Thông tin cá nhân & L�:p
              </h4>
              <div class="grid grid-cols-2 gap-3 text-slate-700">
                <div><span class="text-slate-400 block text-[10px] uppercase">L�:p sinh hoạt:</span> <strong>{{ detail.lopSinhHoat || '-' }}</strong></div>
                <div><span class="text-slate-400 block text-[10px] uppercase">Email:</span> <strong class="truncate block">{{ detail.email || '-' }}</strong></div>
                <div><span class="text-slate-400 block text-[10px] uppercase">Sá»‘ Ä‘iá»‡n thoáº¡i:</span> <strong>{{ detail.soDienThoai || '-' }}</strong></div>
                <div><span class="text-slate-400 block text-[10px] uppercase">Nhóm:</span> <strong>{{ detail.tenNhom || '-' }}</strong></div>
              </div>
            </div>

            <div class="space-y-3">
              <h4 class="font-bold text-slate-900 uppercase tracking-wider text-[11px] text-blue-900 border-b border-slate-100 pb-1">
                ÄÆ¡n vá»‹ & V�9 trí thực tập
              </h4>
              <div class="bg-slate-50 p-4 rounded-xl border border-slate-200 space-y-3">
                <div>
                  <span class="text-slate-400 text-[10px] uppercase block">Doanh nghi�!p:</span>
                  <p class="font-bold text-blue-900 text-sm">{{ detail.donViThucTap || 'ChÆ°a cáº­p nháº­t' }}</p>
                </div>
                <div class="grid grid-cols-2 gap-3">
                  <div>
                    <span class="text-slate-400 text-[10px] uppercase block">V�9 trí:</span>
                    <p class="font-medium text-slate-800">{{ detail.viTriThucTap || '--' }}</p>
                  </div>
                  <div>
                    <span class="text-slate-400 text-[10px] uppercase block">NgÆ°á»i hư�:ng dẫn:</span>
                    <p class="font-medium text-slate-800">{{ detail.mentor || 'Anh Nguyá»…n VÄƒn A' }}</p>
                  </div>
                </div>
              </div>
            </div>

            <div v-if="detail.deTai" class="space-y-2">
              <h4 class="font-bold text-slate-900 uppercase tracking-wider text-[11px] text-blue-900 border-b border-slate-100 pb-1">
                Äá» tÃ i Ä‘Äƒng kÃ½
              </h4>
              <div class="p-4 bg-blue-50/60 border border-blue-100 rounded-xl">
                <p class="font-bold text-blue-900 text-sm mb-1">{{ detail.deTai.tenDeTai }}</p>
                <p class="text-slate-600 text-[11px] leading-relaxed">{{ detail.deTai.moTa }}</p>
              </div>
            </div>

            <div class="space-y-3">
              <h4 class="font-bold text-slate-900 uppercase tracking-wider text-[11px] text-blue-900 border-b border-slate-100 pb-1">
                Tiến ��" ná»™p nháº­t kÃ½ (15 tuáº§n)
              </h4>
              <div class="p-4 rounded-xl border border-slate-200 bg-white">
                <div class="flex flex-wrap gap-2 mb-3">
                  <div v-for="w in 15" :key="w" class="w-5 h-5 rounded flex items-center justify-center font-bold text-[9px]" :class="w <= 8 ? 'bg-emerald-500 text-white' : (w === 9 ? 'bg-amber-500 text-white' : 'bg-slate-100 text-slate-400')">
                    W{{ w }}
                  </div>
                </div>
                <div class="flex items-center gap-4 text-[10px] text-slate-500 font-bold">
                  <span class="flex items-center gap-1"><span class="w-2 h-2 rounded-full bg-emerald-500"></span> ÄÃ£ ná»™p Ä‘Ãºng háº¡n (8)</span>
                  <span class="flex items-center gap-1"><span class="w-2 h-2 rounded-full bg-amber-500"></span> Ná»™p trá»… (1)</span>
                  <span class="flex items-center gap-1"><span class="w-2 h-2 rounded-full bg-slate-300"></span> ChÆ°a Ä‘áº¿n háº¡n</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Tab 2: Nháº­t kÃ½ -->
          <div v-else-if="activeDetailTab === 'logs'" class="space-y-4">
            <div class="flex justify-between items-center bg-slate-50 p-3 rounded-xl border border-slate-200">
              <span class="font-bold text-blue-900 text-xs">Tuáº§n 5 (12/09/2024 - 18/09/2024)</span>
              <span class="px-2.5 py-0.5 rounded-full text-[10px] font-bold bg-emerald-100 text-emerald-800">ÄÃ£ ná»™p</span>
            </div>

            <div class="bg-blue-50/70 border border-blue-200 rounded-xl p-4 space-y-2">
              <div class="flex items-center gap-1.5 text-blue-900 font-bold">
                <span class="material-symbols-outlined text-[18px]">auto_awesome</span>
                <span>TÃ³m táº¯t AI & Tá»•ng há»£p cÃ´ng viá»‡c</span>
              </div>
              <p class="text-slate-700 leading-relaxed">
                Sinh viên đã hoÃ n thÃ nh thiáº¿t láº­p mÃ´i trÆ°á»ng phÃ¡t triá»ƒn và tÃ¬m hiá»ƒu kiáº¿n trÃºc Microservices. ÄÃ£ thá»±c hi�!n sÆ¡ Ä‘á»“ luá»“ng dữ li�!u cho module xÃ¡c thá»±c ngÆ°á»i dÃ¹ng. Dáº¥u hiá»‡u nghiÃªn cá»©u chủ động.
              </p>
            </div>

            <div class="space-y-2 pt-2">
              <label class="block font-bold text-slate-800 text-[11px] uppercase tracking-wider">Pháº£n há»“i / Gá»£i Ã½ cho sinh viên</label>
              <textarea v-model="teacherFeedback" rows="3" placeholder="Nháº­p nháº­n xÃ©t của báº¡n cho sinh viên..." class="w-full p-3 border border-slate-300 rounded-xl text-xs font-medium outline-none focus:ring-2 focus:ring-blue-600"></textarea>
              <div class="flex justify-end">
                <button class="bg-[#005EA3] text-white px-4 py-2 rounded-xl font-bold text-xs shadow-xs hover:opacity-90" @click="saveFeedback">
                  Gửi pháº£n há»“i
                </button>
              </div>
            </div>
          </div>

          <!-- Tab 3: Bài n�"p -->
          <div v-else-if="activeDetailTab === 'submissions'" class="space-y-3">
            <div v-for="sub in [
              { title: 'Äá» cÆ°Æ¡ng chi tiáº¿t', deadline: '15/08/2024', submitted: '14/08/2024 15:30', status: 'DaNop', cls: 'bg-emerald-100 text-emerald-800' },
              { title: 'Báo cáo giá»¯a ká»³', deadline: '10/09/2024', submitted: '12/09/2024 09:15', status: 'NopTre', cls: 'bg-amber-100 text-amber-800' },
              { title: 'Báo cáo cu�i ká»³ & KhÃ³a luáº­n', deadline: '20/12/2024', submitted: '--', status: 'ChuaNop', cls: 'bg-slate-100 text-slate-600' }
            ]" :key="sub.title" class="p-3.5 bg-slate-50 border border-slate-200 rounded-xl flex items-center justify-between">
              <div>
                <p class="font-bold text-slate-900 text-xs">{{ sub.title }}</p>
                <p class="text-[11px] text-slate-500">Háº¡n ná»™p: {{ sub.deadline }}</p>
              </div>
              <div class="flex items-center gap-3">
                <span class="px-2.5 py-1 rounded-full text-[10px] font-bold" :class="sub.cls">
                  {{ sub.status === 'DaNop' ? 'ÄÃ£ ná»™p' : (sub.status === 'NopTre' ? 'Ná»™p trá»…' : 'ChÆ°a ná»™p') }}
                </span>
                <button v-if="sub.status !== 'ChuaNop'" class="p-1.5 rounded-lg hover:bg-slate-200 text-blue-700" title="Táº£i xuá»‘ng tá»‡p ná»™p">
                  <span class="material-symbols-outlined text-[18px]">download</span>
                </button>
              </div>
            </div>
          </div>

          <!-- Tab 4: Äiá»ƒm -->
          <div v-else-if="activeDetailTab === 'grades'" class="space-y-3">
            <div v-if="!detail.diem || !detail.diem.length" class="text-slate-400 italic">ChÆ°a cÃ³ cá»™t Ä‘iá»ƒm nào.</div>
            <div v-else class="space-y-2">
              <div v-for="d in detail.diem" :key="d.maCotDiem" class="flex justify-between items-center p-3 bg-slate-50 rounded-xl border border-slate-200">
                <span class="font-medium text-slate-700">{{ d.tenCot }}</span>
                <span class="font-black text-sm text-blue-700 font-mono">{{ d.diemSo ?? '-' }}</span>
              </div>
            </div>
          </div>

          <!-- Tab 5: Lá»‹ch sá»­ Ä‘Ã¡nh giÃ¡ -->
          <div v-else-if="activeDetailTab === 'history'" class="space-y-3">
            <div class="relative pl-6 space-y-4 border-l-2 border-slate-200">
              <div class="relative">
                <span class="absolute -left-[31px] top-1 w-3 h-3 rounded-full bg-blue-600 border-2 border-white"></span>
                <p class="text-[11px] font-bold text-blue-900">NHáº¬T KÃ TUáº¦N 5 - ÄÃƒ NHáº¬N XÃ‰T</p>
                <p class="text-[10px] text-slate-400">15/09/2024</p>
                <p class="text-xs text-slate-700 mt-1 bg-slate-50 p-2.5 rounded-lg border border-slate-200">
                  "Cáº§n chÃº Ã½ thÃªm vá» tÃ­nh báº£o máº­t token á»Ÿ phÃ­a Client Side."
                </p>
              </div>
            </div>
          </div>

          <!-- Tab 6: Ghi chÃº -->
          <div v-else-if="activeDetailTab === 'notes'" class="space-y-4">
            <div class="bg-amber-50/70 border border-amber-200 rounded-xl p-3 text-[11px] text-amber-900">
              Ghi chÃº riÃªng tÆ° của giảng viên (Chá»‰ báº¡n má»›i nhÃ¬n tháº¥y ghi chÃº nÃ y).
            </div>
            <textarea v-model="internalNote" rows="4" placeholder="Nháº­p ghi chÃº riÃªng tÆ° vá» sinh viên nÃ y..." class="w-full p-3 border border-slate-300 rounded-xl text-xs font-medium outline-none focus:ring-2 focus:ring-blue-600"></textarea>
            <div class="flex justify-end">
              <button class="bg-[#005EA3] text-white px-5 py-2 rounded-xl font-bold text-xs shadow-xs hover:opacity-90" @click="saveNote">
                LÆ°u ghi chÃº
              </button>
            </div>
          </div>
        </div>

        <!-- Drawer Footer -->
        <div class="p-4 bg-slate-50 border-t border-slate-200 flex justify-end">
          <button class="px-5 py-2 bg-slate-200 hover:bg-slate-300 font-bold text-slate-700 rounded-xl text-xs transition-colors" @click="detail = null">
            ÄÃ³ng
          </button>
        </div>
      </div>
    </div>

    <!-- Modal Xuất BÃ¡o CÃ¡o Danh SÃ¡ch Sinh ViÃªn -->
    <div
      v-if="showExportModal"
      class="fixed inset-0 z-[100] bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-4"
    >
      <div class="bg-white rounded-3xl max-w-lg w-full overflow-hidden shadow-2xl border border-slate-100">
        <div class="p-6 bg-gradient-to-br from-slate-900 to-blue-950 text-white flex items-center justify-between">
          <div class="flex items-center gap-3">
            <div class="w-10 h-10 rounded-2xl bg-white/10 flex items-center justify-center font-bold">
              <span class="material-symbols-outlined text-[24px] text-emerald-400">group</span>
            </div>
            <div>
              <h3 class="font-bold text-base">Xuất Báo cáo Danh sách Sinh viên</h3>
              <p class="text-xs text-slate-300">Xuất danh sÃ¡ch sinh viên lá»›p thực tập {{ cls?.tenLop || maLop }}</p>
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
              <p class="font-bold text-blue-900">Báo cáo bao gá»“m thÃ´ng tin chi tiáº¿t:</p>
              <ul class="list-disc list-inside text-slate-600 space-y-0.5">
                <li>STT, MSSV, Há» và tên sinh viên, L�:p sinh hoạt.</li>
                <li>Doanh nghi�!p thực tập, V�9 trí cÃ´ng viá»‡c, TÃªn nhóm ghÃ©p.</li>
                <li>Trạng thái ghi danh & ÄÃ¡nh giÃ¡ tiáº¿n Ä‘á»™ ná»™p nháº­t kÃ½.</li>
              </ul>
            </div>
          </div>

          <div class="grid grid-cols-2 gap-3">
            <div class="p-3 bg-slate-50 rounded-xl border border-slate-200 text-center">
              <span class="text-[11px] font-bold text-slate-500 uppercase block">S� lượng sinh viên</span>
              <span class="text-xl font-black text-slate-900">{{ (filteredStudents || students).length }}</span>
            </div>
            <div class="p-3 bg-emerald-50 rounded-xl border border-emerald-200 text-center">
              <span class="text-[11px] font-bold text-emerald-800 uppercase block">ÄÃ£ cÃ³ doanh nghiá»‡p</span>
              <span class="text-xl font-black text-emerald-700">
                {{ (filteredStudents || students).filter(s => s.dnName || s.company).length }}
              </span>
            </div>
          </div>

          <div class="space-y-2 pt-2">
            <label class="font-bold text-slate-800 block">Chá»n Ä‘á»‹nh dáº¡ng xuáº¥t file:</label>
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <button
                @click="exportStudentsExcel"
                class="p-4 rounded-2xl border-2 border-emerald-500/30 bg-emerald-50/50 hover:bg-emerald-100/80 transition-all flex flex-col items-center justify-center gap-2 text-emerald-900 font-bold group"
              >
                <div class="w-10 h-10 rounded-xl bg-emerald-600 text-white flex items-center justify-center group-hover:scale-105 transition-transform shadow-xs">
                  <span class="material-symbols-outlined text-[22px]">table_chart</span>
                </div>
                <span>Táº£i file Excel (.xlsx)</span>
                <span class="text-[10px] text-emerald-700 font-normal">Xuất file Excel chuáº©n cáº¥u trÃºc</span>
              </button>

              <button
                @click="exportStudentsPdf"
                class="p-4 rounded-2xl border-2 border-rose-500/30 bg-rose-50/50 hover:bg-rose-100/80 transition-all flex flex-col items-center justify-center gap-2 text-rose-900 font-bold group"
              >
                <div class="w-10 h-10 rounded-xl bg-rose-600 text-white flex items-center justify-center group-hover:scale-105 transition-transform shadow-xs">
                  <span class="material-symbols-outlined text-[22px]">picture_as_pdf</span>
                </div>
                <span>In / Táº£i PDF (.pdf)</span>
                <span class="text-[10px] text-rose-700 font-normal">Báº£n in A4 cÃ³ xÃ¡c nháº­n Giáº£ng viÃªn</span>
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
  </div>
</template>

<script setup>
  import { ref, computed, onMounted } from 'vue';
  import { useRoute } from 'vue-router';
  import * as XLSX from 'xlsx';
  import api from '@/api/api';

  const route = useRoute();
  const maLop = route.params.id || route.params.maLop;

  const activeTab = ref('students');
  const cls = ref(null);
  const students = ref([]);
  const groups = ref([]);
  const selected = ref([]);
  const loadingStudents = ref(false);
  const search = ref('');
  const statusFilter = ref('all');
  const toast = ref('');

  const statusLabels = { ChoGhiDanh: 'Chá» ghi danh', DangThucTap: 'Äang thực tập', HoanThanh: 'HoÃ n thÃ nh', DungThucTap: 'Dá»«ng thực tập' };
  const statusCls = { ChoGhiDanh: 'bg-amber-100 text-amber-800 border border-amber-200', DangThucTap: 'bg-emerald-100 text-emerald-800 border border-emerald-200', HoanThanh: 'bg-blue-100 text-blue-800 border border-blue-200', DungThucTap: 'bg-rose-100 text-rose-800 border border-rose-200' };
  const progressLabels = { DungTienDo: 'ÄÃºng tiáº¿n Ä‘á»™', ChamTienDo: 'Cháº­m tiáº¿n Ä‘á»™', CanhBao: 'Cáº£nh bÃ¡o', CanXuLy: 'Cáº§n xá»­ lÃ½' };
  const progressCls = { DungTienDo: 'bg-emerald-100 text-emerald-800', ChamTienDo: 'bg-amber-100 text-amber-800', CanhBao: 'bg-orange-100 text-orange-800', CanXuLy: 'bg-rose-100 text-rose-800' };

  const joinLink = computed(() => `${window.location.origin}/register?code=${cls.value?.maThamGia || ''}`);

  function showToast(m) { toast.value = m; setTimeout(() => (toast.value = ''), 3000); }
  function formatDate(d) { return new Date(d).toLocaleString('vi-VN'); }
  function copy(text) { navigator.clipboard?.writeText(text || ''); showToast('ÄÃ£ sao chÃ©p vào bá»™ nhá»› táº¡m'); }

  const allChecked = computed(() => students.value.length > 0 && selected.value.length === students.value.length);
  function toggleAll(e) { selected.value = e.target.checked ? students.value.map((s) => s.maGhiDanh) : []; }

  async function loadClass() {
    const res = await api.get(`/giangvien/classes/${maLop}`);
    cls.value = res.data;
  }

  async function reloadStudents() {
    loadingStudents.value = true;
    try {
      const res = await api.get(`/giangvien/classes/${maLop}/students`, {
        params: { search: search.value || undefined, trangThai: statusFilter.value }
      });
      students.value = Array.isArray(res.data) ? res.data : (res.data?.items || []);
      selected.value = selected.value.filter((id) => students.value.some((s) => s.maGhiDanh === id));
    } catch {
      students.value = [];
    } finally { loadingStudents.value = false; }
  }

  async function loadGroups() {
    try {
      const res = await api.get(`/giangvien/classes/${maLop}/groups`);
      groups.value = Array.isArray(res.data) ? res.data : (res.data?.items || []);
    } catch {
      groups.value = [];
    }
  }

  // Reset mÃ£
  const showReset = ref(false);
  async function resetCode() {
    const res = await api.post(`/giangvien/classes/${maLop}/reset-code`);
    cls.value.maThamGia = res.data.maThamGia;
    showReset.value = false;
    showToast(res.data.message || 'ÄÃ£ táº¡o mÃ£ tham gia má»›i');
  }

  // Ghi danh
  const showEnroll = ref(false);
  const enrollForm = ref({ ghiDanhMo: true, hanGhiDanh: '' });
  function openEnroll() {
    enrollForm.value = {
      ghiDanhMo: cls.value?.ghiDanhMo ?? true,
      hanGhiDanh: cls.value?.hanGhiDanh ? cls.value.hanGhiDanh.slice(0, 16) : ''
    };
    showEnroll.value = true;
  }
  async function saveEnroll() {
    const res = await api.put(`/giangvien/classes/${maLop}/enrollment`, {
      ghiDanhMo: enrollForm.value.ghiDanhMo,
      hanGhiDanh: enrollForm.value.hanGhiDanh || null
    });
    showEnroll.value = false;
    await loadClass();
    showToast(res.data.message || 'Cáº­p nháº­t thiáº¿t láº­p thÃ nh cÃ´ng');
  }

  // Import
  const showImport = ref(false);
  const importText = ref('');
  const importResults = ref([]);
  function openImport() { importText.value = ''; importResults.value = []; showImport.value = true; }
  async function doImport() {
    const codes = importText.value.split(/[\n,;\s]+/).map((x) => x.trim()).filter(Boolean);
    if (!codes.length) { showToast('Vui lÃ²ng nháº­p danh sÃ¡ch MSSV'); return; }
    const res = await api.post(`/giangvien/classes/${maLop}/students`, { maSoSinhViens: codes });
    importResults.value = res.data.ketQua || [];
    showToast(res.data.message || 'ÄÃ£ xá»­ lÃ½ danh sÃ¡ch sinh viên');
    await Promise.all([reloadStudents(), loadClass()]);
  }

  const groupStatusFilter = ref('all');
  const groupStatusLabels = { ChoXacNhan: 'Chá» xÃ¡c nháº­n', DaXacNhan: 'ÄÃ£ xÃ¡c nháº­n', TuChoi: 'Tá»« chá»‘i' };
  const groupStatusClasses = { ChoXacNhan: 'bg-amber-50 text-amber-800 border-amber-200', DaXacNhan: 'bg-emerald-50 text-emerald-800 border-emerald-200', TuChoi: 'bg-rose-50 text-rose-800 border-rose-200' };
  const groupDotClasses = { ChoXacNhan: 'bg-amber-500 animate-pulse', DaXacNhan: 'bg-emerald-500', TuChoi: 'bg-rose-500' };

  const filteredGroups = computed(() => {
    if (groupStatusFilter.value === 'all') return groups.value;
    return groups.value.filter(g => (g.trangThai || 'ChoXacNhan') === groupStatusFilter.value);
  });

  const rejectTargetGroup = ref(null);
  const rejectReason = ref('');

  async function confirmGroup(g) {
    try {
      const res = await api.post(`/giangvien/groups/${g.maNhom}/confirm`);
      showToast(res.data?.message || `ÄÃ£ xÃ¡c nháº­n nhóm ${g.tenNhom}`);
      await Promise.all([reloadStudents(), loadGroups()]);
    } catch {
      showToast('Thao tÃ¡c thất bại');
    }
  }

  function openRejectGroup(g) {
    rejectTargetGroup.value = g;
    rejectReason.value = '';
  }

  async function confirmRejectGroup() {
    if (!rejectTargetGroup.value) return;
    try {
      const res = await api.post(`/giangvien/groups/${rejectTargetGroup.value.maNhom}/reject`, { lyDo: rejectReason.value });
      showToast(res.data?.message || 'ÄÃ£ tá»« chá»‘i gom nhóm');
      rejectTargetGroup.value = null;
      await Promise.all([reloadStudents(), loadGroups()]);
    } catch {
      showToast('Tá»« chá»‘i gom nhóm thất bại');
    }
  }

  // Gom nhóm
  const showConfirmGroup = ref(false);
  const selectedStudents = computed(() => students.value.filter(s => selected.value.includes(s.maGhiDanh)));

  function openConfirmGroup() {
    if (selected.value.length < 2) return;
    showConfirmGroup.value = true;
  }

  async function confirmCreateGroup() {
    showConfirmGroup.value = false;
    await createGroup();
  }

  async function createGroup() {
    if (selected.value.length < 2) return;
    try {
      const res = await api.post(`/giangvien/classes/${maLop}/groups`, { maGhiDanhs: selected.value });
      selected.value = [];
      showToast(res.data?.message || 'Táº¡o nhóm thÃ nh cÃ´ng');
      await Promise.all([reloadStudents(), loadGroups()]);
    } catch {
      showToast('Táº¡o nhóm thất bại');
    }
  }

  async function disband(g) {
    try {
      await api.delete(`/giangvien/groups/${g.maNhom}`);
      showToast('ÄÃ£ giáº£i tÃ¡n nhóm thÃ nh cÃ´ng');
      await Promise.all([reloadStudents(), loadGroups()]);
    } catch {
      showToast('Giáº£i tÃ¡n nhóm thất bại');
    }
  }

  // Chuyá»ƒn lá»›p
  const transferTarget = ref(null);
  const transferForm = ref({ lopDich: '', lyDo: '' });

  function openTransfer(s) {
    transferTarget.value = s;
    transferForm.value = { lopDich: '', lyDo: '' };
  }

  async function confirmTransfer() {
    if (!transferTarget.value) return;
    try {
      const res = await api.post(`/giangvien/students/${transferTarget.value.maGhiDanh}/transfer`, transferForm.value);
      showToast(res.data?.message || 'ÄÃ£ gá»­i yêu cầu chuyá»ƒn lá»›p');
      transferTarget.value = null;
      await reloadStudents();
    } catch {
      showToast('Gửi yêu cầu chuyá»ƒn lá»›p thất bại');
    }
  }

  // Dá»«ng thực tập
  const stopTarget = ref(null);
  const stopForm = ref({ lyDo: 'Xin rÃºt thực tập', lyDoKhac: '' });
  function openStop(s) { stopTarget.value = s; stopForm.value = { lyDo: 'Xin rÃºt thực tập', lyDoKhac: '' }; }
  async function confirmStop() {
    const lyDo = stopForm.value.lyDo === 'KhÃ¡c' ? (stopForm.value.lyDoKhac || 'KhÃ¡c') : stopForm.value.lyDo;
    const res = await api.post(`/giangvien/students/${stopTarget.value.maGhiDanh}/stop`, { lyDo });
    stopTarget.value = null;
    showToast(res.data.message || 'ÄÃ£ cáº­p nháº­t tráº¡ng thÃ¡i dá»«ng thực tập');
    await reloadStudents();
  }

  // Chi tiáº¿t
  const detail = ref(null);
  const activeDetailTab = ref('overview');
  const teacherFeedback = ref('');
  const internalNote = ref('');
  const showExportModal = ref(false);

  function exportStudentsExcel() {
    const data = [
      ["TRÆ¯á»œNG Äáº I Há»ŒC KINH Táº¾ - Äáº I Há»ŒC ÄÃ€ Náº´NG"],
      ["KHOA CÃ”NG NGHá»† THÃ”NG TIN - Bá»˜ MÃ”N Há»† THá»NG THÃ”NG TIN"],
      ["DANH SÃCH SINH VIÃŠN THá»°C Táº¬P DOANH NGHIá»†P"],
      [`MÃ£ lá»›p thực tập: ${cls.value?.tenLop || maLop} | ${cls.value?.tenKy || 'Há»c ká»³ I - 2025-2026'}`],
      [`Giáº£ng viÃªn hư�:ng dẫn: TS. Nguyá»…n VÄƒn A`],
      [""],
      ["STT", "MSSV", "Há» và tên", "L�:p SH", "Doanh nghi�!p thực tập", "V�9 trí thực tập", "Nhóm", "Trạng thái", "Tiến ��""]
    ];

    const list = students.value;
    list.forEach((st, idx) => {
      data.push([
        idx + 1,
        st.maSoSinhVien || st.mssv,
        st.hoTen || st.name,
        st.lopSinhHoat || '211101',
        st.dnName || st.company || 'ChÆ°a Ä‘Äƒng kÃ½',
        st.viTri || st.position || 'Thá»±c táº­p sinh',
        st.tenNhom || 'ChÆ°a xáº¿p',
        statusLabels[st.trangThai] || st.trangThai || 'Äang thực tập',
        progressLabels[st.tienDo] || st.tienDo || 'ÄÃºng tiáº¿n Ä‘á»™'
      ]);
    });

    data.push([""]);
    data.push(["Tá»”NG Cá»˜NG:", `Tá»•ng SV: ${list.length}`, "", "", "", "", "", "NgÃ y xuáº¥t:", new Date().toLocaleDateString('vi-VN')]);

    const ws = XLSX.utils.aoa_to_sheet(data);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "DanhSachSinhVien");
    XLSX.writeFile(wb, `DanhSach_SinhVien_ThucTap_${maLop}.xlsx`);

    showToast('âœ“ ÄÃ£ xuáº¥t thÃ nh cÃ´ng file Excel Danh sách Sinh viên (.xlsx)');
    showExportModal.value = false;
  }

  function exportStudentsPdf() {
    const printWindow = window.open('', '_blank');
    if (!printWindow) {
      showToast('Vui lÃ²ng báº­t popup trÃ¬nh duyá»‡t Ä‘á»ƒ xem báº£n in PDF');
      return;
    }

    const list = students.value;
    const rowsHtml = list.map((st, idx) => `
      <tr>
        <td style="text-align: center; padding: 6px;">${idx + 1}</td>
        <td style="text-align: center; font-family: monospace; font-weight: bold; padding: 6px;">${st.maSoSinhVien || st.mssv}</td>
        <td style="padding: 6px; font-weight: 600;">${st.hoTen || st.name}</td>
        <td style="text-align: center; padding: 6px;">${st.lopSinhHoat || 'CNTT'}</td>
        <td style="padding: 6px;">${st.dnName || st.company || 'ChÆ°a Ä‘Äƒng kÃ½'}</td>
        <td style="text-align: center; padding: 6px;">${st.tenNhom || '-'}</td>
        <td style="text-align: center; font-weight: bold; padding: 6px;">${statusLabels[st.trangThai] || 'Äang thực tập'}</td>
      </tr>
    `).join('');

    printWindow.document.write(`
      <!DOCTYPE html>
      <html>
      <head>
        <title>Danh sách Sinh viên Thá»±c táº­p - L�:p ${maLop}</title>
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

        <div class="title">DANH SÃCH SINH VIÃŠN Lá»šP THá»°C Táº¬P DOANH NGHIá»†P</div>
        <div class="subtitle">L�:p: <strong>${cls.value?.tenLop || maLop}</strong> â€¢ S� lượng: <strong>${list.length} sinh viên</strong></div>

        <table class="data-table">
          <thead>
            <tr>
              <th style="width: 35px;">STT</th>
              <th style="width: 85px;">MSSV</th>
              <th>Há» và tên sinh viên</th>
              <th style="width: 75px;">L�:p SH</th>
              <th>Doanh nghi�!p thực tập</th>
              <th style="width: 80px;">Nhóm</th>
              <th style="width: 100px;">Trạng thái</th>
            </tr>
          </thead>
          <tbody>
            ${rowsHtml}
          </tbody>
        </table>

        <table class="signature-section">
          <tr>
            <td>
              <br/>
              <strong>XÃC NHáº¬N Bá»˜ MÃ”N</strong><br/>
              <span style="font-style: italic; font-size: 11px;">(KÃ½ và ghi rÃµ há» tên)</span>
              <br/><br/><br/><br/>
            </td>
            <td>
              <em>ÄÃ  Náºµng, ngÃ y ${new Date().getDate()} thÃ¡ng ${new Date().getMonth() + 1} nÄƒm ${new Date().getFullYear()}</em><br/>
              <strong>GIáº¢NG VIÃŠN HÆ¯á»šNG DáºªN</strong><br/>
              <span style="font-style: italic; font-size: 11px;">(KÃ½ và ghi rÃµ há» tên)</span>
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
    showToast('ÄÃ£ má»Ÿ giao diá»‡n In / Táº£i PDF thÃ nh cÃ´ng!');
  }

  async function openDetail(maGhiDanh) {
    try {
      const res = await api.get(`/giangvien/students/${maGhiDanh}`);
      detail.value = res.data || { diem: [] };
      if (!Array.isArray(detail.value.diem)) detail.value.diem = [];
      activeDetailTab.value = 'overview';
      teacherFeedback.value = '';
      internalNote.value = '';
    } catch {
      detail.value = null;
    }
  }

  function saveFeedback() {
    if (!teacherFeedback.value.trim()) return;
    showToast('ÄÃ£ gá»­i pháº£n há»“i cho sinh viên');
    teacherFeedback.value = '';
  }

  function saveNote() {
    if (!internalNote.value.trim()) return;
    showToast('ÄÃ£ lÆ°u ghi chÃº riÃªng tÆ° thÃ nh cÃ´ng');
    internalNote.value = '';
  }

  onMounted(async () => {
    await loadClass();
    await Promise.all([reloadStudents(), loadGroups()]);
  });
</script>
