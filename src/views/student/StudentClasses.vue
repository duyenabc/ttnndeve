<template>
  <div class="ims-scope w-full mx-auto font-sans space-y-6 pb-20">
    <!-- VIEW 1: CLASS LIST -->
    <div v-if="currentView === 'list'" class="space-y-8">
      <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs">
        <div>
          <h1 class="text-2xl font-extrabold text-slate-900 tracking-tight flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3] text-[28px]">school</span>
            L�:p thực tập của tôi
          </h1>
          <p class="text-xs text-slate-500 mt-1">Danh sách cÃ¡c lá»›p há»c pháº§n thực tập đã ghi danh hoặc chá» tham gia.</p>
        </div>

        <button
          @click="showEnrollModal = true"
          class="px-5 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl text-xs shadow-md transition-all active:scale-95 flex items-center gap-2 w-fit"
        >
          <span class="material-symbols-outlined text-[18px]">add</span>
          Nháº­p mÃ£ lá»›p
        </button>
      </div>

      <!-- SECTION 1: Há»ŒC Ká»² HÃˆ -->
      <div class="space-y-4">
        <h3 class="uppercase font-bold text-xs text-slate-500 tracking-wider flex items-center gap-2">
          <span class="w-2 h-2 rounded-full bg-emerald-500"></span>
          Há»ŒC Ká»² HÃˆ â€” NÄ‚M Há»ŒC 2025-2026
        </h3>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <!-- Active Class Card -->
          <div
            @click="openClassDetail(activeClass)"
            class="bg-white border border-slate-200 rounded-2xl overflow-hidden shadow-2xs hover:shadow-md hover:-translate-y-0.5 transition-all cursor-pointer group flex flex-col justify-between"
          >
            <div>
              <div class="h-32 bg-blue-900 relative flex items-center justify-center overflow-hidden">
                <div class="absolute inset-0 bg-cover bg-center opacity-30 group-hover:scale-105 transition-transform duration-500" style="background-image: url('https://lh3.googleusercontent.com/aida-public/AB6AXuBoNj0OnfLqaRQIPoQKP_hTlVAVcryzicOk7ZBfBgERjRmj4FYpE2WaMPkCpNNlZZQ4DvU2HO2RC0VNJ2tPhE_b5jENCsfcpWd-GIV62So7pnVsql4pHph2Ebj6wblysT0sd89xuj5P5i9eBt0m1J_YpzQ8-Np6A1s2MCcdZ_9MqJDKBEkxLZTVPL82GcCXedYtNv7EPsRGIUt9SGOqIiBvSkIug03ccDmbN_dbG1WPnKkum6lrzUwTKjjlZ2GuKs4owPAQuDRfo3Xr')"></div>
                <span class="material-symbols-outlined text-white text-[48px] relative z-10 drop-shadow">laptop_mac</span>
                <div class="absolute top-3 right-3 bg-emerald-500 text-white text-[10px] font-extrabold px-3 py-1 rounded-full uppercase tracking-wider shadow-xs">
                  Äang thực tập
                </div>
              </div>

              <div class="p-5 space-y-4">
                <div>
                  <h4 class="font-bold text-base text-slate-900 line-clamp-1 group-hover:text-[#005EA3] transition">Thá»±c táº­p tá»‘t nghiá»‡p K64.A1</h4>
                  <p class="text-xs text-slate-500 mt-0.5 font-medium">GVHD: TS. Nguyá»…n VÄƒn A</p>
                </div>

                <div class="space-y-2 text-xs text-slate-600">
                  <div class="flex items-center gap-2">
                    <span class="material-symbols-outlined text-slate-400 text-[18px]">event_available</span>
                    <span>Káº¿t thÃºc: 30/08/2026</span>
                  </div>
                  <div class="flex items-center gap-2">
                    <span class="material-symbols-outlined text-slate-400 text-[18px]">task_alt</span>
                    <span>Tiến ��": 4/12 Báo cáo</span>
                  </div>
                </div>
              </div>
            </div>

            <div class="px-5 py-3 border-t border-slate-100 flex justify-end bg-slate-50/50">
              <span class="text-[#005EA3] font-bold text-xs flex items-center gap-1 group-hover:translate-x-1 transition-transform">
                Chi tiáº¿t lá»›p <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- SECTION 2: Há»ŒC Ká»² 2 -->
      <div class="space-y-4 pt-2">
        <h3 class="uppercase font-bold text-xs text-slate-500 tracking-wider flex items-center gap-2">
          <span class="w-2 h-2 rounded-full bg-amber-500"></span>
          Há»ŒC Ká»² 2 â€” NÄ‚M Há»ŒC 2025-2026
        </h3>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <!-- Wait Enrollment Card -->
          <div
            @click="openClassDetail(pendingClass)"
            class="bg-white border border-slate-200 border-l-4 border-l-amber-500 rounded-2xl overflow-hidden shadow-2xs hover:shadow-md hover:-translate-y-0.5 transition-all cursor-pointer group flex flex-col justify-between"
          >
            <div>
              <div class="h-32 bg-amber-100/80 relative flex items-center justify-center overflow-hidden">
                <div class="absolute inset-0 bg-cover bg-center opacity-25 group-hover:scale-105 transition-transform duration-500" style="background-image: url('https://lh3.googleusercontent.com/aida-public/AB6AXuC22TgsN57j17y5g5HKdtUzXIP13tXgRB4PZDUWzI2pZYcSSUnQ7iEfbG5hE_ZHxATL87uhE2jncmQSQcFX7M-dZcTIZUjC7cCQbuXVitIjgP8NmAtNp0uOsU0yEHjsh1emGQXY9ym0B03QkOHXqcimpDLVpbz4uDb2MBS8AjPaOSEYHQVgRss5jUTa6BB8oHyzUChst35MYfMQAW7he0vljA0y6NMfArFJzPBx_oHg6lOlcCqutNG5iwblwsG2I2fWmVI_we5ipqVg')"></div>
                <span class="material-symbols-outlined text-amber-800 text-[48px] relative z-10">school</span>
                <div class="absolute top-3 right-3 bg-amber-500 text-white text-[10px] font-extrabold px-3 py-1 rounded-full uppercase tracking-wider shadow-xs">
                  Chá» ghi danh
                </div>
              </div>

              <div class="p-5 space-y-4">
                <div>
                  <h4 class="font-bold text-base text-slate-900 line-clamp-1 group-hover:text-[#005EA3] transition">Ká»¹ thuáº­t pháº§n má»m - Nhóm 02</h4>
                  <p class="text-xs text-slate-500 mt-0.5 font-medium">GVHD: ThS. Tráº§n Thá»‹ B</p>
                </div>

                <div class="space-y-2 text-xs text-slate-600">
                  <div class="flex items-center gap-2">
                    <span class="material-symbols-outlined text-slate-400 text-[18px]">calendar_today</span>
                    <span>HK2 - 2025-2026</span>
                  </div>
                  <div class="flex items-center gap-2">
                    <span class="material-symbols-outlined text-slate-400 text-[18px]">group</span>
                    <span>45 Sinh viên</span>
                  </div>
                </div>
              </div>
            </div>

            <div class="px-5 py-3 border-t border-slate-100 flex justify-end bg-slate-50/50">
              <span class="text-[#005EA3] font-bold text-xs flex items-center gap-1 group-hover:translate-x-1 transition-transform">
                Xem ghi danh <span class="material-symbols-outlined text-[16px]">arrow_forward</span>
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- SECTION 3: Há»ŒC Ká»² 1 -->
      <div class="space-y-4 pt-2">
        <h3 class="uppercase font-bold text-xs text-slate-500 tracking-wider flex items-center gap-2">
          <span class="w-2 h-2 rounded-full bg-slate-400"></span>
          Há»ŒC Ká»² 1 â€” NÄ‚M Há»ŒC 2025-2026
        </h3>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <!-- Finished Class Card -->
          <div
            class="bg-white border border-slate-200 rounded-2xl overflow-hidden shadow-2xs opacity-80 group flex flex-col justify-between"
          >
            <div>
              <div class="h-32 bg-slate-100 relative flex items-center justify-center">
                <span class="material-symbols-outlined text-slate-400 text-[48px] relative z-10">history_edu</span>
                <div class="absolute top-3 right-3 bg-slate-500 text-white text-[10px] font-extrabold px-3 py-1 rounded-full uppercase tracking-wider">
                  ÄÃ£ káº¿t thÃºc
                </div>
              </div>

              <div class="p-5 space-y-4">
                <div>
                  <h4 class="font-bold text-base text-slate-900 line-clamp-1">PhÃ¡t triá»ƒn á»©ng dá»¥ng Web</h4>
                  <p class="text-xs text-slate-500 mt-0.5 font-medium">GVHD: TS. LÃª VÄƒn C</p>
                </div>

                <div class="space-y-2 text-xs text-slate-600">
                  <div class="flex items-center gap-2">
                    <span class="material-symbols-outlined text-emerald-600 text-[18px]">grade</span>
                    <span class="font-bold text-slate-800">Äiá»ƒm tá»•ng káº¿t: 9.0 (ThÄƒng A)</span>
                  </div>
                </div>
              </div>
            </div>

            <div class="px-5 py-3 border-t border-slate-100 flex justify-end bg-slate-50/50">
              <span class="text-slate-400 font-bold text-xs flex items-center gap-1">
                HoÃ n thÃ nh
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- VIEW 2: CLASS DETAIL PAGE -->
    <div v-else-if="currentView === 'detail'" class="space-y-6">
      <button
        @click="currentView = 'list'"
        class="inline-flex items-center gap-2 text-slate-600 hover:text-[#005EA3] font-bold text-xs transition"
      >
        <span class="material-symbols-outlined text-[18px]">arrow_back</span>
        Quay láº¡i danh sÃ¡ch lá»›p
      </button>

      <div class="bg-white p-8 rounded-2xl border border-slate-200 shadow-2xs space-y-6">
        <div class="flex flex-col lg:flex-row lg:items-center justify-between gap-6 border-b border-slate-100 pb-6">
          <div class="space-y-3">
            <div class="inline-flex items-center gap-2 px-3 py-1 bg-amber-50 text-amber-800 rounded-full text-xs font-bold border border-amber-200">
              <span class="w-2 h-2 rounded-full bg-amber-500 animate-pulse"></span>
              L�:p há»c chá» ghi danh
            </div>
            <h2 class="text-2xl lg:text-3xl font-extrabold text-slate-900">{{ selectedClassDetail?.tenLop }}</h2>
            <p class="text-xs text-slate-500">MÃ£ lá»›p: <strong class="text-slate-800 font-mono">KTPM2026_02</strong></p>
          </div>

          <div class="flex flex-wrap items-center gap-3">
            <button
              @click="handleEnrollSubmit"
              :disabled="isEnrolling"
              class="px-8 py-3 bg-[#005EA3] hover:bg-blue-800 text-white font-bold text-xs rounded-xl shadow-md transition-all active:scale-95 flex items-center gap-2"
            >
              <span v-if="isEnrolling" class="material-symbols-outlined animate-spin text-[18px]">sync</span>
              <span>{{ isEnrolling ? 'Äang xÃ¡c nháº­n...' : 'Ghi danh ngay' }}</span>
            </button>
            <button
              @click="currentView = 'list'"
              class="px-6 py-3 border border-slate-300 text-slate-700 font-bold text-xs rounded-xl hover:bg-slate-50 transition"
            >
              Äá»ƒ sau
            </button>
          </div>
        </div>

        <!-- Class Quick Specs -->
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
          <div class="p-4 bg-slate-50 rounded-xl border border-slate-200 flex items-center gap-3">
            <span class="material-symbols-outlined text-[#005EA3] text-[28px] p-2 bg-blue-100 rounded-xl">person</span>
            <div>
              <p class="text-[11px] font-bold text-slate-400 uppercase">Giáº£ng viÃªn hư�:ng dẫn</p>
              <p class="text-xs font-bold text-slate-900 mt-0.5">ThS. Tráº§n Thá»‹ B</p>
            </div>
          </div>

          <div class="p-4 bg-slate-50 rounded-xl border border-slate-200 flex items-center gap-3">
            <span class="material-symbols-outlined text-[#005EA3] text-[28px] p-2 bg-blue-100 rounded-xl">event</span>
            <div>
              <p class="text-[11px] font-bold text-slate-400 uppercase">Há»c ká»³ - NÄƒm há»c</p>
              <p class="text-xs font-bold text-slate-900 mt-0.5">Há»c ká»³ 2, 2025-2026</p>
            </div>
          </div>

          <div class="p-4 bg-slate-50 rounded-xl border border-slate-200 flex items-center gap-3">
            <span class="material-symbols-outlined text-[#005EA3] text-[28px] p-2 bg-blue-100 rounded-xl">schedule</span>
            <div>
              <p class="text-[11px] font-bold text-slate-400 uppercase">Thá»i gian sinh hoạt</p>
              <p class="text-xs font-bold text-slate-900 mt-0.5">Thá»© 3 (Tiáº¿t 1 - 4)</p>
            </div>
          </div>

          <div class="p-4 bg-slate-50 rounded-xl border border-slate-200 flex items-center gap-3">
            <span class="material-symbols-outlined text-[#005EA3] text-[28px] p-2 bg-blue-100 rounded-xl">location_on</span>
            <div>
              <p class="text-[11px] font-bold text-slate-400 uppercase">Äá»‹a Ä‘iá»ƒm</p>
              <p class="text-xs font-bold text-slate-900 mt-0.5">PhÃ²ng Lab 202, Khu A</p>
            </div>
          </div>
        </div>

        <!-- Course Roadmap -->
        <div class="space-y-4 pt-4">
          <h3 class="font-bold text-sm text-slate-900 flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3]">timeline</span>
            Lá»™ trÃ¬nh mÃ´n há»c & Các cá»™t má»‘c thực tập
          </h3>

          <div class="relative pl-8 border-l-2 border-blue-200 ml-3 space-y-6 py-2">
            <div class="relative">
              <span class="absolute -left-[41px] top-0.5 w-4 h-4 rounded-full bg-[#005EA3] ring-4 ring-blue-100"></span>
              <h4 class="font-bold text-xs text-slate-900">Giai Ä‘oáº¡n 1: Khá»Ÿi Ä‘á»™ng và Äá» xuáº¥t Ä‘á» tÃ i</h4>
              <p class="text-xs text-slate-500 mt-1">Sinh viên thÃ nh láº­p nhóm và ná»™p Ä‘á» cÆ°Æ¡ng chi tiáº¿t cho giảng viên phÃª duyá»‡t.</p>
            </div>

            <div class="relative">
              <span class="absolute -left-[41px] top-0.5 w-4 h-4 rounded-full bg-slate-300"></span>
              <h4 class="font-bold text-xs text-slate-800">Giai Ä‘oáº¡n 2: PhÃ¢n tÃ­ch và Thiáº¿t káº¿ há»‡ thá»‘ng</h4>
              <p class="text-xs text-slate-500 mt-1">Xác định yêu cầu, thiết kế Database và Mockup giao diá»‡n ngÆ°á»i dÃ¹ng.</p>
            </div>

            <div class="relative">
              <span class="absolute -left-[41px] top-0.5 w-4 h-4 rounded-full bg-slate-300"></span>
              <h4 class="font-bold text-xs text-slate-800">Giai Ä‘oáº¡n 3: PhÃ¡t triá»ƒn và Kiá»ƒm thá»­</h4>
              <p class="text-xs text-slate-500 mt-1">CÃ i Ä‘áº·t mÃ£ nguá»“n, triá»ƒn khai cÃ¡c tÃ­nh nÄƒng cá»‘t lÃµi và kiá»ƒm thá»­ Ä‘Æ¡n vá»‹.</p>
            </div>

            <div class="relative">
              <span class="absolute -left-[41px] top-0.5 w-4 h-4 rounded-full bg-slate-300"></span>
              <h4 class="font-bold text-xs text-slate-800">Giai Ä‘oáº¡n 4: Báo cáo cu�i ká»³</h4>
              <p class="text-xs text-slate-500 mt-1">HoÃ n thi�!n sáº£n pháº©m, Ä‘Ã³ng gÃ³i mÃ£ nguá»“n và thuyáº¿t trÃ¬nh trÆ°á»›c há»™i Ä‘á»“ng.</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- VIEW 3: ONBOARDING SCREEN AFTER ENROLLMENT -->
    <div v-else-if="currentView === 'onboarding'" class="max-w-2xl mx-auto py-10 space-y-8 text-center font-sans">
      <div class="inline-flex items-center justify-center w-20 h-20 bg-emerald-100 text-emerald-700 rounded-full mb-2 shadow-sm">
        <span class="material-symbols-outlined text-[42px]">celebration</span>
      </div>

      <div>
        <h2 class="text-2xl font-black text-slate-900">ChÃ o má»«ng Ä‘áº¿n vá»›i ká»³ thực tập!</h2>
        <p class="text-xs text-slate-600 mt-2 max-w-md mx-auto">
          ChÃºc má»«ng báº¡n đã ghi danh thÃ nh cÃ´ng. HÃ£y hoÃ n táº¥t cÃ¡c bÆ°á»›c chuẩn bị dÆ°á»›i Ä‘Ã¢y Ä‘á»ƒ báº¯t �ầu quÃ¡ trÃ¬nh thực tập của mÃ¬nh.
        </p>
      </div>

      <div class="space-y-4 text-left">
        <!-- Step 1 -->
        <div class="flex items-center justify-between p-5 bg-white border border-slate-200 rounded-2xl shadow-2xs hover:border-[#005EA3] transition group">
          <div class="flex items-center gap-4">
            <div class="w-10 h-10 rounded-full bg-blue-50 text-[#005EA3] font-bold flex items-center justify-center text-sm shrink-0">1</div>
            <div>
              <h4 class="font-bold text-xs text-slate-900">Khai bÃ¡o Ä‘Æ¡n vá»‹ thực tập</h4>
              <p class="text-[11px] text-slate-500 mt-0.5">Thông tin vá» doanh nghiá»‡p báº¡n sáº½ lÃ m viá»‡c</p>
            </div>
          </div>
          <router-link to="/student/progress" class="px-4 py-2 bg-blue-50 hover:bg-[#005EA3] hover:text-white text-[#005EA3] font-bold rounded-xl text-xs transition">
            Khai bÃ¡o ngay
          </router-link>
        </div>

        <!-- Step 2 -->
        <div class="flex items-center justify-between p-5 bg-white border border-slate-200 rounded-2xl shadow-2xs hover:border-[#005EA3] transition group">
          <div class="flex items-center gap-4">
            <div class="w-10 h-10 rounded-full bg-blue-50 text-[#005EA3] font-bold flex items-center justify-center text-sm shrink-0">2</div>
            <div>
              <h4 class="font-bold text-xs text-slate-900">ÄÄƒng kÃ½ Ä‘á» tÃ i</h4>
              <p class="text-[11px] text-slate-500 mt-0.5">Ná»™p tên Ä‘á» tÃ i thực tập đã Ä‘Æ°á»£c duyá»‡t</p>
            </div>
          </div>
          <router-link to="/student/progress" class="px-4 py-2 bg-blue-50 hover:bg-[#005EA3] hover:text-white text-[#005EA3] font-bold rounded-xl text-xs transition">
            ÄÄƒng kÃ½
          </router-link>
        </div>

        <!-- Step 3 -->
        <div class="flex items-center justify-between p-5 bg-white border border-slate-200 rounded-2xl shadow-2xs hover:border-[#005EA3] transition group">
          <div class="flex items-center gap-4">
            <div class="w-10 h-10 rounded-full bg-blue-50 text-[#005EA3] font-bold flex items-center justify-center text-sm shrink-0">3</div>
            <div>
              <h4 class="font-bold text-xs text-slate-900">Äá»c tÃ i liá»‡u hư�:ng dẫn</h4>
              <p class="text-[11px] text-slate-500 mt-0.5">Quy Ä‘á»‹nh và cÃ¡c biá»ƒu máº«u báo cáo</p>
            </div>
          </div>
          <router-link to="/documents" class="px-4 py-2 bg-blue-50 hover:bg-[#005EA3] hover:text-white text-[#005EA3] font-bold rounded-xl text-xs transition">
            Xem tÃ i liá»‡u
          </router-link>
        </div>
      </div>

      <div class="pt-4 flex justify-center">
        <router-link
          to="/dashboard"
          class="px-8 py-3 bg-[#005EA3] hover:bg-blue-800 text-white font-bold text-xs rounded-xl shadow-md transition-all active:scale-95 flex items-center gap-2"
        >
          <span>VÃ o Dashboard</span>
          <span class="material-symbols-outlined text-[18px]">arrow_forward</span>
        </router-link>
      </div>
    </div>

    <!-- ENROLLMENT MODAL -->
    <div
      v-if="showEnrollModal"
      class="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-slate-900/50 backdrop-blur-xs font-sans"
      @click.self="showEnrollModal = false"
    >
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-md overflow-hidden p-6 space-y-6">
        <div class="flex justify-between items-center border-b pb-3">
          <h3 class="font-bold text-slate-900 text-base flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3]">key</span>
            Ghi danh báº±ng mÃ£ lá»›p
          </h3>
          <button @click="showEnrollModal = false" class="text-slate-400 hover:text-slate-600">âœ•</button>
        </div>

        <p class="text-xs text-slate-600 leading-relaxed">
          MÃ£ lá»›p thÆ°á»ng cÃ³ 8 kÃ½ tá»± do giảng viên cung cáº¥p Ä‘á»ƒ tham gia vào lá»›p há»c nhanh chÃ³ng. (Gá»£i Ã½ thá»­: <strong class="text-[#005EA3]">VALID</strong>)
        </p>

        <div class="space-y-4">
          <input
            v-model="inputClassCode"
            type="text"
            maxlength="10"
            placeholder="Nháº­p mÃ£ (VÃ­ dá»¥: KTPM2026)"
            class="w-full px-4 py-3 bg-slate-50 border-2 border-slate-200 focus:border-[#005EA3] focus:bg-white rounded-xl outline-none font-mono text-center font-bold uppercase tracking-widest text-sm transition"
          />

          <button
            @click="searchClassCode"
            :disabled="isSearching"
            class="w-full py-3 bg-[#005EA3] hover:bg-blue-800 text-white font-bold text-xs rounded-xl shadow-md transition active:scale-95 flex items-center justify-center gap-2"
          >
            <span v-if="isSearching" class="material-symbols-outlined animate-spin text-[18px]">sync</span>
            <span>{{ isSearching ? 'Äang tÃ¬m kiáº¿m...' : 'TÃ¬m lá»›p' }}</span>
          </button>
        </div>

        <!-- Search Result Area -->
        <div v-if="searchResult" class="space-y-3 pt-2 border-t border-slate-100">
          <div v-if="searchResult.success" class="p-4 bg-blue-50 border border-blue-200 rounded-xl space-y-2">
            <div class="flex items-center gap-2 font-bold text-xs text-blue-950">
              <span class="material-symbols-outlined text-[#005EA3] text-[18px]">school</span>
              {{ searchResult.data.tenLop }}
            </div>
            <p class="text-[11px] text-blue-800">{{ searchResult.data.mota }}</p>

            <button
              @click="confirmEnrollFromModal"
              class="w-full mt-2 py-2.5 bg-[#005EA3] text-white rounded-xl font-bold text-xs hover:bg-blue-800 transition"
            >
              Ghi danh vào lá»›p nÃ y
            </button>
          </div>

          <div v-else class="p-4 bg-rose-50 text-rose-800 border border-rose-200 rounded-xl flex items-center gap-2 text-xs font-medium">
            <span class="material-symbols-outlined text-rose-600">error</span>
            {{ searchResult.error }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue';

  const currentView = ref('list'); // 'list' | 'detail' | 'onboarding'
  const showEnrollModal = ref(false);
  const inputClassCode = ref('');
  const isSearching = ref(false);
  const searchResult = ref(null);
  const isEnrolling = ref(false);

  const activeClass = {
    id: 1,
    tenLop: 'Thá»±c táº­p tá»‘t nghiá»‡p K64.A1',
    gv: 'TS. Nguyá»…n VÄƒn A',
    status: 'active'
  };

  const pendingClass = {
    id: 2,
    tenLop: 'Ká»¹ thuáº­t pháº§n má»m - Nhóm 02',
    gv: 'ThS. Tráº§n Thá»‹ B',
    status: 'pending'
  };

  const selectedClassDetail = ref(pendingClass);

  function openClassDetail(cls) {
    selectedClassDetail.value = cls;
    currentView.value = 'detail';
  }

  function handleEnrollSubmit() {
    isEnrolling.value = true;
    setTimeout(() => {
      isEnrolling.value = false;
      currentView.value = 'onboarding';
    }, 1200);
  }

  function searchClassCode() {
    if (!inputClassCode.value.trim()) return;
    isSearching.value = true;
    searchResult.value = null;

    setTimeout(() => {
      isSearching.value = false;
      const code = inputClassCode.value.toUpperCase().trim();
      if (code === 'VALID' || code === 'KTPM2026' || code === 'K64A1') {
        searchResult.value = {
          success: true,
          data: {
            tenLop: 'Láº­p trÃ¬nh á»©ng dá»¥ng di Ä‘á»™ng - N01',
            mota: 'GV. Nguyá»…n Thá»‹ C â€¢ HK2 2025-2026'
          }
        };
      } else {
        searchResult.value = {
          success: false,
          error: 'MÃ£ lá»›p không tá»“n tại hoặc đã háº¿t háº¡n ghi danh. Thá»­ nháº­p "VALID"'
        };
      }
    }, 800);
  }

  function confirmEnrollFromModal() {
    showEnrollModal.value = false;
    currentView.value = 'onboarding';
  }
</script>
