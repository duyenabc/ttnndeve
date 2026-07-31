<template>
  <div class="ims-scope max-w-6xl mx-auto font-sans space-y-6 pb-20">
    <!-- Breadcrumb & Top Header -->
    <div class="space-y-3">
      <nav class="flex items-center text-xs text-slate-500 gap-1.5">
        <router-link to="/student/classes" class="hover:text-[#005EA3] transition font-medium">Lớp của tôi</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-slate-900 font-bold">Lịch trình thực tập</span>
      </nav>

      <div class="flex flex-col lg:flex-row lg:items-center justify-between gap-4 bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs">
        <div>
          <h1 class="text-2xl font-extrabold text-slate-900 tracking-tight flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3] text-[28px]">calendar_month</span>
            Quản lý Lịch trình Thực tập
          </h1>
          <p class="text-xs text-slate-500 mt-1">Theo dõi hạn nộp đề tài, hạn nộp báo cáo, lịch họp với Giảng viên hướng dẫn và các mốc quan trọng.</p>
        </div>

        <div class="flex flex-wrap items-center gap-2">
          <!-- Button to Add Custom Meeting / Schedule Note -->
          <button
            @click="openAddModal"
            class="px-4 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white rounded-xl font-bold text-xs transition flex items-center gap-1.5 shadow-xs cursor-pointer"
          >
            <span class="material-symbols-outlined text-[18px]">add_circle</span>
            Thêm lịch họp / Ghi chú
          </button>

          <!-- View Mode Selector -->
          <div class="flex items-center gap-1 bg-slate-100 p-1 rounded-xl text-xs font-bold border border-slate-200">
            <button
              @click="viewMode = 'timeline'"
              class="px-3.5 py-1.5 rounded-lg transition flex items-center gap-1"
              :class="viewMode === 'timeline' ? 'bg-[#005EA3] text-white shadow-2xs' : 'text-slate-600 hover:text-slate-900'"
            >
              <span class="material-symbols-outlined text-[16px]">timeline</span>
              Lộ trình
            </button>
            <button
              @click="viewMode = 'week'"
              class="px-3.5 py-1.5 rounded-lg transition flex items-center gap-1"
              :class="viewMode === 'week' ? 'bg-[#005EA3] text-white shadow-2xs' : 'text-slate-600 hover:text-slate-900'"
            >
              <span class="material-symbols-outlined text-[16px]">calendar_view_week</span>
              Lịch Tuần
            </button>
            <button
              @click="viewMode = 'month'"
              class="px-3.5 py-1.5 rounded-lg transition flex items-center gap-1"
              :class="viewMode === 'month' ? 'bg-[#005EA3] text-white shadow-2xs' : 'text-slate-600 hover:text-slate-900'"
            >
              <span class="material-symbols-outlined text-[16px]">calendar_view_month</span>
              Lịch Tháng
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- 3 KEY MILESTONE HIGHLIGHT CARDS -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <!-- 1. Hạn nộp đề tài -->
      <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-2xs space-y-3 relative overflow-hidden group hover:border-rose-300 transition">
        <div class="absolute top-0 right-0 w-2 h-full bg-rose-500"></div>
        <div class="flex items-center justify-between">
          <span class="px-2.5 py-1 bg-rose-50 text-rose-700 border border-rose-200 rounded-lg text-[10px] font-black uppercase tracking-wider flex items-center gap-1">
            <span class="material-symbols-outlined text-[14px]">assignment</span>
            Hạn nộp Đề tài
          </span>
          <span class="text-[11px] font-bold text-slate-400">{{ getHighlightDate('1') }}</span>
        </div>
        <div>
          <h3 class="font-bold text-sm text-slate-900 group-hover:text-rose-700 transition">Đăng ký & Nộp Đề cương Đề tài</h3>
          <p class="text-xs text-slate-500 mt-1 line-clamp-2">Nộp tên đề tài chính thức, bản mô tả chi tiết và danh sách thành viên nhóm thực tập.</p>
        </div>
        <div class="pt-2 border-t border-slate-100 flex items-center justify-between text-xs">
          <span class="px-2.5 py-1 bg-emerald-100 text-emerald-800 rounded-md font-bold text-[11px] flex items-center gap-1">
            <span class="material-symbols-outlined text-[14px]">check_circle</span>
            Đã hoàn thành
          </span>
          <router-link to="/student/progress" class="font-bold text-[#005EA3] hover:underline flex items-center gap-0.5">
            Xem đề tài <span class="material-symbols-outlined text-[14px]">arrow_forward</span>
          </router-link>
        </div>
      </div>

      <!-- 2. Hạn nộp báo cáo -->
      <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-2xs space-y-3 relative overflow-hidden group hover:border-amber-300 transition">
        <div class="absolute top-0 right-0 w-2 h-full bg-amber-500"></div>
        <div class="flex items-center justify-between">
          <span class="px-2.5 py-1 bg-amber-50 text-amber-800 border border-amber-200 rounded-lg text-[10px] font-black uppercase tracking-wider flex items-center gap-1">
            <span class="material-symbols-outlined text-[14px]">description</span>
            Hạn nộp Báo cáo
          </span>
          <span class="text-[11px] font-bold text-amber-700 bg-amber-50 px-2 py-0.5 rounded">{{ getHighlightRemaining('4') }}</span>
        </div>
        <div>
          <h3 class="font-bold text-sm text-slate-900 group-hover:text-amber-700 transition">Báo cáo Tiến độ Giữa kỳ</h3>
          <p class="text-xs text-slate-500 mt-1 line-clamp-2">Nộp bản thảo báo cáo giữa kỳ kèm xác nhận từ Doanh nghiệp (File PDF & Minh chứng).</p>
        </div>
        <div class="pt-2 border-t border-slate-100 flex items-center justify-between text-xs">
          <span class="text-slate-500 font-medium text-[11px]">Hạn chót: {{ getHighlightDate('4') }}</span>
          <router-link to="/diaries" class="font-bold text-[#005EA3] hover:underline flex items-center gap-0.5">
            Nộp báo cáo <span class="material-symbols-outlined text-[14px]">arrow_forward</span>
          </router-link>
        </div>
      </div>

      <!-- 3. Lịch họp với GVHD -->
      <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-2xs space-y-3 relative overflow-hidden group hover:border-emerald-300 transition">
        <div class="absolute top-0 right-0 w-2 h-full bg-emerald-500"></div>
        <div class="flex items-center justify-between">
          <span class="px-2.5 py-1 bg-emerald-50 text-emerald-800 border border-emerald-200 rounded-lg text-[10px] font-black uppercase tracking-wider flex items-center gap-1">
            <span class="material-symbols-outlined text-[14px]">groups</span>
            Họp với GVHD
          </span>
          <span class="text-[11px] font-bold text-emerald-700 bg-emerald-50 px-2 py-0.5 rounded">{{ getHighlightRemaining('2') }}</span>
        </div>
        <div>
          <h3 class="font-bold text-sm text-slate-900 group-hover:text-emerald-700 transition">Họp Duyệt Tiến độ & Giải đáp Vướng mắc</h3>
          <p class="text-xs text-slate-500 mt-1 line-clamp-2">TS. Nguyễn Văn A — Review sơ đồ kiến trúc ứng dụng & chuẩn bị báo cáo giữa kỳ.</p>
        </div>
        <div class="pt-2 border-t border-slate-100 flex items-center justify-between text-xs">
          <span class="font-bold text-slate-700 text-[11px] flex items-center gap-1">
            <span class="material-symbols-outlined text-[14px] text-emerald-600">videocam</span>
            {{ getHighlightTime('2') }} Google Meet
          </span>
          <button @click="joinMeeting(getHighlightLink('2'))" class="font-bold text-[#005EA3] hover:underline flex items-center gap-0.5 cursor-pointer">
            Vào họp <span class="material-symbols-outlined text-[14px]">open_in_new</span>
          </button>
        </div>
      </div>
    </div>

    <!-- FILTER TABS & SEARCH BAR -->
    <div class="bg-white p-4 rounded-2xl border border-slate-200 shadow-2xs flex flex-col md:flex-row md:items-center justify-between gap-4">
      <!-- Category Filter Pills -->
      <div class="flex flex-wrap items-center gap-2 text-xs">
        <button
          v-for="cat in categoryTabs"
          :key="cat.id"
          @click="selectedCategory = cat.id"
          class="px-3.5 py-1.5 rounded-full font-bold transition flex items-center gap-1.5 border cursor-pointer"
          :class="selectedCategory === cat.id ? cat.activeClass : 'bg-slate-50 border-slate-200 text-slate-600 hover:bg-slate-100'"
        >
          <span class="material-symbols-outlined text-[16px]">{{ cat.icon }}</span>
          <span>{{ cat.label }}</span>
          <span class="ml-0.5 px-1.5 py-0.2 rounded-full text-[10px] bg-white/40">{{ getCategoryCount(cat.id) }}</span>
        </button>
      </div>

      <!-- Search Input -->
      <div class="relative w-full md:w-64 text-xs">
        <span class="material-symbols-outlined absolute left-3 top-2.5 text-slate-400 text-[18px]">search</span>
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Tìm mốc thời gian, lịch họp..."
          class="w-full pl-9 pr-3 py-2 bg-slate-50 border border-slate-200 rounded-xl outline-none focus:ring-2 focus:ring-blue-600 font-medium text-slate-800"
        />
      </div>
    </div>

    <!-- VIEW 1: TIMELINE LIST VIEW (Mặc định) -->
    <div v-if="viewMode === 'timeline'" class="space-y-4">
      <div v-if="filteredEvents.length === 0" class="bg-white rounded-2xl border border-slate-200 p-12 text-center text-slate-500 space-y-3">
        <span class="material-symbols-outlined text-4xl text-slate-300">event_busy</span>
        <p class="font-bold text-sm text-slate-700">Không tìm thấy mốc thời gian phù hợp</p>
        <p class="text-xs text-slate-400">Thử thay đổi từ khóa tìm kiếm hoặc chuyển bộ lọc danh mục.</p>
      </div>

      <div v-else class="space-y-4">
        <div
          v-for="item in filteredEvents"
          :key="item.id"
          class="bg-white rounded-2xl border border-slate-200 p-5 shadow-2xs hover:shadow-md transition-all space-y-4 relative"
          :class="{ 'opacity-75 bg-slate-50/50': item.completed }"
        >
          <div class="flex flex-col sm:flex-row sm:items-start justify-between gap-3 border-b border-slate-100 pb-3">
            <div class="flex items-start gap-3">
              <!-- Checkbox to toggle completion -->
              <button
                @click="toggleComplete(item)"
                class="mt-1 w-5 h-5 rounded-md border flex items-center justify-center transition cursor-pointer shrink-0"
                :class="item.completed ? 'bg-emerald-600 border-emerald-600 text-white' : 'border-slate-300 hover:border-emerald-600 bg-white'"
                :title="item.completed ? 'Đánh dấu chưa hoàn thành' : 'Đánh dấu đã hoàn thành'"
              >
                <span v-if="item.completed" class="material-symbols-outlined text-[14px]">check</span>
              </button>

              <div>
                <div class="flex flex-wrap items-center gap-2">
                  <span
                    class="px-2.5 py-0.5 rounded-md text-[10px] font-black uppercase tracking-wider border"
                    :class="getCategoryTagStyle(item.category)"
                  >
                    {{ getCategoryLabel(item.category) }}
                  </span>

                  <!-- Status Badge -->
                  <span
                    v-if="item.completed"
                    class="px-2.5 py-0.5 rounded-md text-[10px] font-bold bg-emerald-100 text-emerald-800 border border-emerald-200 flex items-center gap-1"
                  >
                    <span class="material-symbols-outlined text-[12px]">check_circle</span>
                    Đã hoàn thành
                  </span>
                  <span
                    v-else-if="item.isUrgent"
                    class="px-2.5 py-0.5 rounded-md text-[10px] font-bold bg-rose-100 text-rose-800 border border-rose-200 animate-pulse flex items-center gap-1"
                  >
                    <span class="material-symbols-outlined text-[12px]">alarm</span>
                    Sắp đến hạn
                  </span>
                  <span
                    v-else
                    class="px-2.5 py-0.5 rounded-md text-[10px] font-bold bg-blue-50 text-blue-800 border border-blue-200"
                  >
                    Đang theo dõi
                  </span>
                </div>

                <h3
                  class="font-extrabold text-slate-900 text-base mt-1.5"
                  :class="{ 'line-through text-slate-500': item.completed }"
                >
                  {{ item.title }}
                </h3>
              </div>
            </div>

            <!-- Date & Time Tag -->
            <div class="flex items-center gap-2 text-xs font-bold text-slate-700 bg-slate-50 p-2 rounded-xl border border-slate-100 shrink-0 self-start">
              <span class="material-symbols-outlined text-[#005EA3] text-[18px]">event</span>
              <span>{{ item.dateStr }}</span>
              <span class="text-slate-300">•</span>
              <span class="material-symbols-outlined text-[#005EA3] text-[18px]">schedule</span>
              <span>{{ item.time }}</span>
            </div>
          </div>

          <!-- Description & Details -->
          <p class="text-xs text-slate-600 leading-relaxed font-medium">
            {{ item.description }}
          </p>

          <!-- Action Footer -->
          <div class="flex flex-wrap items-center justify-between gap-3 pt-2 text-xs">
            <div class="flex items-center gap-3 text-slate-500 font-medium">
              <span v-if="item.location" class="flex items-center gap-1 text-slate-700">
                <span class="material-symbols-outlined text-[16px] text-rose-600">location_on</span>
                {{ item.location }}
              </span>
              <span v-if="item.lecturer" class="flex items-center gap-1 text-slate-700">
                <span class="material-symbols-outlined text-[16px] text-blue-600">person</span>
                GVHD: {{ item.lecturer }}
              </span>
            </div>

            <div class="flex items-center gap-2 ml-auto">
              <button
                v-if="item.meetingLink"
                @click="joinMeeting(item.meetingLink)"
                class="px-3.5 py-1.5 bg-emerald-600 hover:bg-emerald-700 text-white font-bold rounded-xl transition flex items-center gap-1 shadow-2xs cursor-pointer"
              >
                <span class="material-symbols-outlined text-[16px]">videocam</span>
                Vào Google Meet
              </button>

              <router-link
                v-if="item.actionLink"
                :to="item.actionLink"
                class="px-3.5 py-1.5 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl transition flex items-center gap-1 shadow-2xs"
              >
                <span class="material-symbols-outlined text-[16px]">upload_file</span>
                {{ item.actionText || 'Nộp bài ngay' }}
              </router-link>

              <button
                @click="openEventDetails(item)"
                class="px-3 py-1.5 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold rounded-xl transition flex items-center gap-1 cursor-pointer"
              >
                <span class="material-symbols-outlined text-[16px]">info</span>
                Chi tiết
              </button>

              <button
                v-if="item.isCustom"
                @click="deleteCustomItem(item.id)"
                class="p-1.5 text-rose-500 hover:bg-rose-50 rounded-lg transition"
                title="Xóa mốc cá nhân"
              >
                <span class="material-symbols-outlined text-[18px]">delete</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- VIEW 2: WEEKLY GRID VIEW -->
    <div v-else-if="viewMode === 'week'" class="bg-white rounded-2xl border border-slate-200 shadow-2xs overflow-hidden">
      <!-- Week Navigation -->
      <div class="p-4 bg-slate-50 border-b border-slate-200 flex items-center justify-between">
        <div class="flex items-center gap-3">
          <button @click="currentWeekOffset--" class="p-1.5 hover:bg-slate-200 rounded-lg transition text-slate-700">
            <span class="material-symbols-outlined text-[20px]">chevron_left</span>
          </button>
          <span class="font-extrabold text-sm text-slate-900">{{ weekTitle }}</span>
          <button @click="currentWeekOffset++" class="p-1.5 hover:bg-slate-200 rounded-lg transition text-slate-700">
            <span class="material-symbols-outlined text-[20px]">chevron_right</span>
          </button>
        </div>
        <button @click="currentWeekOffset = 0" class="px-3 py-1 bg-white border border-slate-300 text-slate-700 rounded-lg text-xs font-bold hover:bg-slate-100">
          Tuần này
        </button>
      </div>

      <!-- Grid Header -->
      <div class="grid grid-cols-8 border-b border-slate-200 text-center text-xs font-bold bg-slate-100/70">
        <div class="p-3 border-r border-slate-200 text-slate-400 flex items-center justify-center">Giờ</div>
        <div v-for="(day, idx) in weekDays" :key="idx" class="p-3 border-r last:border-r-0 border-slate-200" :class="{ 'bg-blue-50 text-[#005EA3]': day.isToday }">
          <p class="text-[10px] uppercase text-slate-500 font-bold">{{ day.dayName }}</p>
          <p class="text-sm font-black mt-0.5">{{ day.dateNum }}</p>
        </div>
      </div>

      <!-- Grid Body -->
      <div class="max-h-[500px] overflow-y-auto divide-y divide-slate-100 text-xs">
        <div v-for="hour in [8, 9, 10, 11, 13, 14, 15, 16, 17]" :key="hour" class="grid grid-cols-8 min-h-[56px]">
          <div class="p-2 border-r border-slate-200 text-slate-400 font-mono text-[11px] text-center bg-slate-50/50">
            {{ hour }}:00
          </div>
          <div v-for="(day, dayIdx) in weekDays" :key="dayIdx" class="border-r last:border-r-0 border-slate-100 p-1 relative hover:bg-slate-50/50 transition">
            <template v-for="event in getWeekSlotEvents(dayIdx, hour)" :key="event.id">
              <div
                @click="openEventDetails(event)"
                class="p-2 rounded-xl border text-[11px] font-bold cursor-pointer transition hover:scale-102 mb-1 shadow-2xs"
                :class="getCategoryTagStyle(event.category)"
              >
                <div class="truncate font-extrabold">{{ event.title }}</div>
                <div class="text-[10px] opacity-80 font-normal">{{ event.time }}</div>
              </div>
            </template>
          </div>
        </div>
      </div>
    </div>

    <!-- VIEW 3: MONTHLY GRID VIEW -->
    <div v-else class="bg-white rounded-2xl border border-slate-200 p-6 shadow-2xs space-y-4">
      <div class="flex items-center justify-between pb-3 border-b border-slate-100">
        <h3 class="font-extrabold text-sm text-slate-900">Tháng {{ currentMonthStr }} - Năm học {{ schoolYearStr }}</h3>
        <span class="text-xs text-slate-500 font-medium">Tổng số {{ scheduleItems.length }} mốc thời gian quan trọng</span>
      </div>

      <div class="grid grid-cols-7 gap-2 text-center text-xs font-bold text-slate-500 mb-2">
        <div>T2</div><div>T3</div><div>T4</div><div>T5</div><div>T6</div><div>T7</div><div>CN</div>
      </div>

      <div class="grid grid-cols-7 gap-2">
        <div
          v-for="d in daysInCurrentMonth"
          :key="d"
          class="min-h-[72px] p-2 bg-slate-50 border border-slate-200 rounded-xl relative hover:border-blue-400 transition"
          :class="{ 'bg-blue-50/80 border-[#005EA3]': d === 25 }"
        >
          <span class="font-bold text-xs" :class="d === 25 ? 'text-[#005EA3] font-black' : 'text-slate-700'">{{ d }}</span>
          <div class="mt-1 space-y-1">
            <div v-for="ev in getEventsForDateNum(d)" :key="ev.id" @click="openEventDetails(ev)" class="p-1 rounded text-[9px] font-bold truncate cursor-pointer" :class="getCategoryTagStyle(ev.category)">
              {{ ev.title }}
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- MODAL 1: ADD CUSTOM SCHEDULE ITEM / MEETING NOTE -->
    <div
      v-if="showAddModal"
      class="fixed inset-0 z-[120] flex items-center justify-center p-4 bg-slate-900/60 backdrop-blur-xs font-sans"
      @click.self="showAddModal = false"
    >
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-lg p-6 space-y-5 border border-slate-200">
        <div class="flex justify-between items-center border-b pb-3">
          <h3 class="font-bold text-slate-900 text-base flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3]">event_available</span>
            Thêm Lịch họp / Ghi chú mốc thời gian
          </h3>
          <button @click="showAddModal = false" class="text-slate-400 hover:text-slate-600 font-bold">✕</button>
        </div>

        <div class="space-y-4 text-xs">
          <div>
            <label class="block font-bold text-slate-700 mb-1">Tiêu đề mốc / Buổi họp (*)</label>
            <input
              v-model="newItem.title"
              type="text"
              placeholder="VD: Họp báo cáo tiến độ module với ThS. Lê Hoàng Nam"
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium text-slate-900"
            />
          </div>

          <div class="grid grid-cols-2 gap-3">
            <div>
              <label class="block font-bold text-slate-700 mb-1">Loại mốc (*)</label>
              <select v-model="newItem.category" class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-semibold text-slate-800">
                <option value="topic_deadline">Hạn nộp Đề tài</option>
                <option value="report_deadline">Hạn nộp Báo cáo</option>
                <option value="advisor_meeting">Lịch họp với GVHD</option>
                <option value="faculty_milestone">Sự kiện Khoa / Trường</option>
              </select>
            </div>

            <div>
              <label class="block font-bold text-slate-700 mb-1">Giảng viên hướng dẫn</label>
              <input
                v-model="newItem.lecturer"
                type="text"
                placeholder="TS. Nguyễn Văn A"
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium"
              />
            </div>
          </div>

          <div class="grid grid-cols-2 gap-3">
            <div>
              <label class="block font-bold text-slate-700 mb-1">Ngày (*)</label>
              <input
                v-model="newItem.dateStr"
                type="text"
                :placeholder="getTodayDateStr()"
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium"
              />
            </div>

            <div>
              <label class="block font-bold text-slate-700 mb-1">Khung giờ (*)</label>
              <input
                v-model="newItem.time"
                type="text"
                placeholder="14:00 - 15:30"
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium"
              />
            </div>
          </div>

          <div>
            <label class="block font-bold text-slate-700 mb-1">Địa điểm / Link Google Meet</label>
            <input
              v-model="newItem.location"
              type="text"
              placeholder="Google Meet hoặc Phòng 302 - Khoa CNTT"
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium"
            />
          </div>

          <div>
            <label class="block font-bold text-slate-700 mb-1">Ghi chú chi tiết / Nội dung chuẩn bị</label>
            <textarea
              v-model="newItem.description"
              rows="3"
              placeholder="Nội dung cần chuẩn bị trước khi tham gia họp hoặc các mục cần báo cáo..."
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-medium resize-none"
            ></textarea>
          </div>
        </div>

        <div class="pt-3 border-t flex justify-end gap-2.5 text-xs">
          <button @click="showAddModal = false" class="px-4 py-2 border border-slate-300 rounded-xl font-bold text-slate-700 hover:bg-slate-100">
            Hủy
          </button>
          <button @click="saveCustomScheduleItem" class="px-5 py-2 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-xs">
            Lưu vào lịch
          </button>
        </div>
      </div>
    </div>

    <!-- MODAL 2: EVENT DETAILS MODAL -->
    <div
      v-if="selectedEvent"
      class="fixed inset-0 z-[120] flex items-center justify-center p-4 bg-slate-900/60 backdrop-blur-xs font-sans"
      @click.self="selectedEvent = null"
    >
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-md p-6 space-y-4 border border-slate-200">
        <div class="flex justify-between items-start border-b pb-3">
          <span class="px-2.5 py-1 rounded-md text-[10px] font-black uppercase tracking-wider border" :class="getCategoryTagStyle(selectedEvent.category)">
            {{ getCategoryLabel(selectedEvent.category) }}
          </span>
          <button @click="selectedEvent = null" class="text-slate-400 hover:text-slate-600 font-bold">✕</button>
        </div>

        <div>
          <h3 class="font-extrabold text-slate-900 text-base">{{ selectedEvent.title }}</h3>
          <p class="text-xs text-slate-600 mt-1 font-medium leading-relaxed">{{ selectedEvent.description }}</p>
        </div>

        <div class="space-y-2 text-xs bg-slate-50 p-3.5 rounded-xl border border-slate-100">
          <div class="flex items-center gap-2 text-slate-800">
            <span class="material-symbols-outlined text-[16px] text-[#005EA3]">event</span>
            <span class="font-bold">Thời gian: {{ selectedEvent.time }} — {{ selectedEvent.dateStr }}</span>
          </div>
          <div v-if="selectedEvent.location" class="flex items-center gap-2 text-slate-800">
            <span class="material-symbols-outlined text-[16px] text-rose-600">location_on</span>
            <span class="font-medium">Địa điểm: {{ selectedEvent.location }}</span>
          </div>
          <div v-if="selectedEvent.lecturer" class="flex items-center gap-2 text-slate-800">
            <span class="material-symbols-outlined text-[16px] text-blue-600">person</span>
            <span class="font-medium">Giảng viên: {{ selectedEvent.lecturer }}</span>
          </div>
        </div>

        <div class="pt-2 flex justify-end gap-2 text-xs">
          <button @click="selectedEvent = null" class="px-4 py-2 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold rounded-xl">
            Đóng
          </button>
          <button
            v-if="selectedEvent.meetingLink"
            @click="joinMeeting(selectedEvent.meetingLink)"
            class="px-5 py-2 bg-emerald-600 hover:bg-emerald-700 text-white font-bold rounded-xl shadow-xs"
          >
            Vào phòng họp Online
          </button>
        </div>
      </div>
    </div>

    <!-- TOAST NOTIFICATION -->
    <div
      v-if="toastMsg"
      class="fixed bottom-6 right-6 z-[200] bg-slate-900 text-white px-5 py-3 rounded-xl shadow-2xl border border-slate-700 flex items-center gap-3 text-xs animate-bounce"
    >
      <span class="material-symbols-outlined text-emerald-400 text-[20px]">check_circle</span>
      <span class="font-bold">{{ toastMsg }}</span>
    </div>
  </div>
</template>

<script setup>
  import { ref, computed, onMounted } from 'vue';

  const viewMode = ref('timeline'); // 'timeline' | 'week' | 'month'
  const selectedCategory = ref('all');
  const searchQuery = ref('');
  const showAddModal = ref(false);
  const selectedEvent = ref(null);
  const toastMsg = ref('');
  const currentWeekOffset = ref(0);

  function triggerToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      if (toastMsg.value === msg) toastMsg.value = '';
    }, 3500);
  }

  const categoryTabs = [
    { id: 'all', label: 'Tất cả mốc', icon: 'apps', activeClass: 'bg-slate-900 text-white border-slate-900' },
    { id: 'topic_deadline', label: 'Hạn nộp đề tài', icon: 'assignment', activeClass: 'bg-rose-600 text-white border-rose-600' },
    { id: 'report_deadline', label: 'Hạn nộp báo cáo', icon: 'description', activeClass: 'bg-amber-600 text-white border-amber-600' },
    { id: 'advisor_meeting', label: 'Lịch họp với GVHD', icon: 'groups', activeClass: 'bg-emerald-600 text-white border-emerald-600' },
    { id: 'faculty_milestone', label: 'Sự kiện Khoa', icon: 'flag', activeClass: 'bg-indigo-600 text-white border-indigo-600' }
  ];

  // Dynamic Date Calculations
  const getDynamicDateStr = (daysFromToday) => {
    const d = new Date();
    d.setDate(d.getDate() + daysFromToday);
    const dd = String(d.getDate()).padStart(2, '0');
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const yyyy = d.getFullYear();
    return `${dd}/${mm}/${yyyy}`;
  };

  const getDynamicDateNum = (daysFromToday) => {
    const d = new Date();
    d.setDate(d.getDate() + daysFromToday);
    return d.getDate();
  };

  const getDynamicDayIdx = (daysFromToday) => {
    const d = new Date();
    d.setDate(d.getDate() + daysFromToday);
    const day = d.getDay();
    return day === 0 ? 6 : day - 1;
  };

  const getTodayDateStr = () => {
    return getDynamicDateStr(0);
  };

  // Base list of schedule items calculated dynamically relative to the current date
  const baseScheduleItems = [
    {
      id: '1',
      title: 'Hạn nộp & Đăng ký Đề tài Thực tập',
      category: 'topic_deadline',
      dateStr: getDynamicDateStr(2),
      dateNum: getDynamicDateNum(2),
      dayIdx: getDynamicDayIdx(2),
      hour: 11,
      time: '11:00 AM',
      description: 'Khóa cổng đăng ký và chốt tên đề tài thực tập doanh nghiệp chính thức của Nhóm 06.',
      location: 'Hệ thống IMS Online',
      completed: true,
      actionLink: '/student/progress',
      actionText: 'Xem Đề tài'
    },
    {
      id: '2',
      title: 'Lịch họp định kỳ với GVHD TS. Nguyễn Văn A',
      category: 'advisor_meeting',
      dateStr: getDynamicDateStr(5),
      dateNum: getDynamicDateNum(5),
      dayIdx: getDynamicDayIdx(5),
      hour: 14,
      time: '14:00 - 15:30',
      description: 'Duyệt sơ đồ kiến trúc Microservices và định hướng viết Báo cáo tiến độ Giữa kỳ.',
      location: 'Google Meet (Online)',
      lecturer: 'TS. Nguyễn Văn A',
      meetingLink: 'https://meet.google.com/abc-defg-hij',
      completed: false,
      isUrgent: true
    },
    {
      id: '3',
      title: 'Nộp Nhật ký Thực tập Tuần 5',
      category: 'report_deadline',
      dateStr: getDynamicDateStr(7),
      dateNum: getDynamicDateNum(7),
      dayIdx: getDynamicDayIdx(7),
      hour: 23,
      time: '23:59 PM',
      description: 'Nộp báo cáo công việc thực tập tại FPT Software kèm xác nhận nhật ký tuần từ Mentor.',
      location: 'Hệ thống IMS Online',
      completed: false,
      isUrgent: true,
      actionLink: '/diaries',
      actionText: 'Viết nhật ký'
    },
    {
      id: '4',
      title: 'Hạn nộp Báo cáo Tiến độ Giữa kỳ',
      category: 'report_deadline',
      dateStr: getDynamicDateStr(15),
      dateNum: getDynamicDateNum(15),
      dayIdx: getDynamicDayIdx(15),
      hour: 17,
      time: '17:00 PM',
      description: 'Nộp bản PDF Báo cáo giữa kỳ có xác nhận con dấu của doanh nghiệp tiếp nhận thực tập.',
      location: 'Khoa CNTT - Đại học Kinh tế (DUE)',
      completed: false,
      actionLink: '/documents',
      actionText: 'Nộp file báo cáo'
    },
    {
      id: '5',
      title: 'Họp sinh hoạt lớp Thực tập K64',
      category: 'faculty_milestone',
      dateStr: getDynamicDateStr(22),
      dateNum: getDynamicDateNum(22),
      dayIdx: getDynamicDayIdx(22),
      hour: 9,
      time: '09:00 - 11:00',
      description: 'ThS. Lê Hoàng Nam phổ biến quy định chấm khóa luận và thủ tục bảo vệ thực tập.',
      location: 'Hội trường A, Trường ĐH Kinh tế',
      completed: false
    },
    {
      id: '6',
      title: 'Hạn nộp Báo cáo Tổng kết & Khóa luận Thực tập',
      category: 'report_deadline',
      dateStr: getDynamicDateStr(40),
      dateNum: getDynamicDateNum(40),
      dayIdx: getDynamicDayIdx(40),
      hour: 16,
      time: '16:00 PM',
      description: 'Nộp 03 bản in Báo cáo Tổng kết thực tập kèm đĩa CD/File source code dự án.',
      location: 'Văn phòng Khoa CNTT (Phòng 201)',
      completed: false
    }
  ];

  const scheduleItems = ref([]);

  onMounted(() => {
    try {
      const saved = localStorage.getItem('ims_student_custom_schedule');
      if (saved) {
        const customItems = JSON.parse(saved);
        scheduleItems.value = [...baseScheduleItems, ...customItems];
      } else {
        scheduleItems.value = [...baseScheduleItems];
      }
    } catch (e) {
      scheduleItems.value = [...baseScheduleItems];
    }
  });

  const filteredEvents = computed(() => {
    return scheduleItems.value.filter((st) => {
      const matchCat = selectedCategory.value === 'all' || st.category === selectedCategory.value;
      const matchSearch =
        searchQuery.value === '' ||
        st.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        st.description.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        (st.lecturer && st.lecturer.toLowerCase().includes(searchQuery.value.toLowerCase()));

      return matchCat && matchSearch;
    });
  });

  function getCategoryCount(catId) {
    if (catId === 'all') return scheduleItems.value.length;
    return scheduleItems.value.filter(item => item.category === catId).length;
  }

  function getCategoryTagStyle(category) {
    if (category === 'topic_deadline') return 'bg-rose-50 text-rose-800 border-rose-200';
    if (category === 'report_deadline') return 'bg-amber-50 text-amber-800 border-amber-200';
    if (category === 'advisor_meeting') return 'bg-emerald-50 text-emerald-800 border-emerald-200';
    return 'bg-indigo-50 text-indigo-800 border-indigo-200';
  }

  function getCategoryLabel(category) {
    if (category === 'topic_deadline') return 'Hạn nộp Đề tài';
    if (category === 'report_deadline') return 'Hạn nộp Báo cáo';
    if (category === 'advisor_meeting') return 'Lịch họp với GVHD';
    return 'Mốc thời gian Khoa';
  }

  function toggleComplete(item) {
    item.completed = !item.completed;
    if (item.completed) {
      triggerToast(`✓ Đã đánh dấu hoàn thành: "${item.title}"`);
    } else {
      triggerToast(`Đã mở lại trạng thái mốc: "${item.title}"`);
    }
  }

  function joinMeeting(url) {
    triggerToast('Đang mở phòng họp Google Meet...');
    window.open(url, '_blank');
  }

  function openEventDetails(event) {
    selectedEvent.value = event;
  }

  // Highlight Card Accessors
  function getHighlightDate(id) {
    const item = scheduleItems.value.find(i => i.id === id);
    return item ? item.dateStr : '';
  }

  function getHighlightTime(id) {
    const item = scheduleItems.value.find(i => i.id === id);
    return item ? item.time : '';
  }

  function getHighlightLocation(id) {
    const item = scheduleItems.value.find(i => i.id === id);
    return item ? item.location : '';
  }

  function getHighlightLink(id) {
    const item = scheduleItems.value.find(i => i.id === id);
    return item ? (item.meetingLink || 'https://meet.google.com/abc-defg-hij') : 'https://meet.google.com/abc-defg-hij';
  }

  function getHighlightRemaining(id) {
    const item = scheduleItems.value.find(i => i.id === id);
    if (!item) return '';
    const parts = item.dateStr.split('/');
    if (parts.length === 3) {
      const d = new Date(parseInt(parts[2]), parseInt(parts[1]) - 1, parseInt(parts[0]));
      const today = new Date();
      today.setHours(0,0,0,0);
      const diffTime = d.getTime() - today.getTime();
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
      if (diffDays < 0) return 'Đã qua hạn';
      if (diffDays === 0) return 'Hôm nay';
      if (diffDays === 1) return 'Ngày mai';
      return `Còn ${diffDays} ngày`;
    }
    return '';
  }

  // Monthly View Dynamics
  const currentMonthStr = computed(() => {
    const d = new Date();
    return String(d.getMonth() + 1).padStart(2, '0');
  });

  const schoolYearStr = computed(() => {
    const d = new Date();
    const year = d.getFullYear();
    if (d.getMonth() >= 8) {
      return `${year}-${year + 1}`;
    } else {
      return `${year - 1}-${year}`;
    }
  });

  const daysInCurrentMonth = computed(() => {
    const d = new Date();
    return new Date(d.getFullYear(), d.getMonth() + 1, 0).getDate();
  });

  // Custom Item Modal State
  const newItem = ref({
    title: '',
    category: 'advisor_meeting',
    lecturer: 'TS. Nguyễn Văn A',
    dateStr: '',
    time: '14:00 - 15:00',
    location: 'Google Meet',
    description: ''
  });

  function openAddModal() {
    newItem.value = {
      title: '',
      category: 'advisor_meeting',
      lecturer: 'TS. Nguyễn Văn A',
      dateStr: getTodayDateStr(),
      time: '14:00 - 15:00',
      location: 'Google Meet',
      description: ''
    };
    showAddModal.value = true;
  }

  function saveCustomScheduleItem() {
    if (!newItem.value.title.trim()) {
      triggerToast('Vui lòng nhập tiêu đề mốc lịch!');
      return;
    }

    const dateStrVal = newItem.value.dateStr || getTodayDateStr();
    let computedDateNum = getDynamicDateNum(0);
    let computedDayIdx = getDynamicDayIdx(0);

    const parts = dateStrVal.split('/');
    if (parts.length === 3) {
      computedDateNum = parseInt(parts[0]);
      const parsedDate = new Date(parseInt(parts[2]), parseInt(parts[1]) - 1, parseInt(parts[0]));
      const day = parsedDate.getDay();
      computedDayIdx = day === 0 ? 6 : day - 1;
    }

    const createdItem = {
      id: 'custom_' + Date.now(),
      title: newItem.value.title,
      category: newItem.value.category,
      lecturer: newItem.value.lecturer,
      dateStr: dateStrVal,
      dateNum: computedDateNum,
      dayIdx: computedDayIdx,
      hour: 14,
      time: newItem.value.time || '14:00',
      location: newItem.value.location || 'Google Meet',
      description: newItem.value.description || 'Ghi chú lịch hẹn sinh viên tự thiết lập.',
      completed: false,
      isCustom: true
    };

    scheduleItems.value.unshift(createdItem);

    // Save custom items to local storage
    try {
      const customOnly = scheduleItems.value.filter(i => i.isCustom);
      localStorage.setItem('ims_student_custom_schedule', JSON.stringify(customOnly));
    } catch (e) {
      console.error(e);
    }

    showAddModal.value = false;
    triggerToast('✓ Đã thêm lịch họp / mốc thời gian mới thành công!');
  }

  function deleteCustomItem(itemId) {
    scheduleItems.value = scheduleItems.value.filter(i => i.id !== itemId);
    try {
      const customOnly = scheduleItems.value.filter(i => i.isCustom);
      localStorage.setItem('ims_student_custom_schedule', JSON.stringify(customOnly));
    } catch (e) {
      console.error(e);
    }
    triggerToast('Đã xóa mốc thời gian khỏi lịch cá nhân.');
  }

  // Week Grid Helpers
  const weekDays = computed(() => {
    const today = new Date();
    const currentDay = today.getDay();
    const diff = today.getDate() - currentDay + (currentDay === 0 ? -6 : 1);
    const monday = new Date(today.getFullYear(), today.getMonth(), diff);

    const names = ['Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7', 'Chủ Nhật'];
    return names.map((name, idx) => {
      const d = new Date(monday);
      d.setDate(monday.getDate() + idx);
      const isToday = d.toDateString() === new Date().toDateString();
      return {
        dayName: name,
        dateNum: String(d.getDate()),
        isToday,
        fullDate: d
      };
    });
  });

  const weekTitle = computed(() => {
    if (weekDays.value.length === 0) return '';
    const first = weekDays.value[0].fullDate;
    const last = weekDays.value[6].fullDate;
    const formatDate = (d) => {
      const dd = String(d.getDate()).padStart(2, '0');
      const mm = String(d.getMonth() + 1).padStart(2, '0');
      const yyyy = d.getFullYear();
      return `${dd}/${mm}/${yyyy}`;
    };
    return `Tuần thực tập (${formatDate(first)} - ${formatDate(last)})`;
  });

  function getWeekSlotEvents(dayIdx, hour) {
    return scheduleItems.value.filter(st => st.dayIdx === dayIdx && st.hour === hour);
  }

  function getEventsForDateNum(dNum) {
    return scheduleItems.value.filter(st => st.dateNum === dNum);
  }
</script>
