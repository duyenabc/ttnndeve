<template>
  <div class="ims-scope max-w-6xl mx-auto font-sans space-y-5 pb-24">
    <div class="space-y-2">
      <nav class="flex items-center text-[13px] text-slate-500 gap-1.5 flex-wrap">
        <router-link to="/student/classes" class="hover:text-[#005EA3] transition">Lớp của tôi</router-link>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span>Lớp Thực tập K64</span>
        <span class="material-symbols-outlined text-[14px]">chevron_right</span>
        <span class="text-[#005EA3] font-medium">{{ breadcrumbTail }}</span>
      </nav>

      <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-3">
        <h1 class="text-[28px] font-bold text-slate-900 tracking-tight leading-tight">Tiến trình của tôi</h1>
        <button
          v-if="activeTab === 'info'"
          @click="showEditModal = true"
          class="px-4 py-2 bg-blue-50 hover:bg-blue-100 text-[#005EA3] rounded-lg font-bold text-xs transition flex items-center gap-1.5 w-fit border border-blue-200"
        >
          <span class="material-symbols-outlined text-[18px]">edit</span>
          Cập nhật thông tin
        </button>
      </div>
    </div>

    <div class="flex items-center gap-8 border-b border-slate-200">
      <button
        type="button"
        @click="activeTab = 'info'"
        class="relative -mb-px pb-3 text-[15px] transition"
        :class="activeTab === 'info' ? 'text-[#005EA3] border-b-[3px] border-[#005EA3] font-bold' : 'text-slate-500 hover:text-[#005EA3] font-semibold border-b-[3px] border-transparent'"
      >
        Thông tin thực tập
      </button>
      <button
        type="button"
        @click="activeTab = 'topic'"
        class="relative -mb-px pb-3 text-[15px] transition"
        :class="activeTab === 'topic' ? 'text-[#005EA3] border-b-[3px] border-[#005EA3] font-bold' : 'text-slate-500 hover:text-[#005EA3] font-semibold border-b-[3px] border-transparent'"
      >
        Đề tài
      </button>
      <button
        type="button"
        @click="activeTab = 'diaries'"
        class="relative -mb-px pb-3 text-[15px] transition"
        :class="activeTab === 'diaries' ? 'text-[#005EA3] border-b-[3px] border-[#005EA3] font-bold' : 'text-slate-500 hover:text-[#005EA3] font-semibold border-b-[3px] border-transparent'"
      >
        Nhật ký
      </button>
    </div>

    <div v-if="activeTab === 'info'" class="space-y-6">
      <div class="bg-white border border-slate-200 rounded-2xl p-8 shadow-2xs relative overflow-hidden space-y-8">
        <div class="absolute top-0 right-0 w-32 h-32 bg-blue-50 rounded-bl-[100px] pointer-events-none opacity-60"></div>

        <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4 relative z-10">
          <div class="flex items-center gap-4">
            <div class="w-16 h-16 bg-blue-100/80 text-[#005EA3] flex items-center justify-center rounded-2xl shrink-0">
              <span class="material-symbols-outlined text-3xl">apartment</span>
            </div>
            <div>
              <div class="flex flex-wrap items-center gap-3">
                <h3 class="font-extrabold text-lg text-slate-900">{{ enterpriseInfo.tenCongTy }}</h3>
                <span v-if="isEnterpriseDeclared" class="inline-flex items-center gap-1.5 py-1 px-3 rounded-full bg-emerald-100 text-emerald-800 font-bold text-[11px]">
                  <span class="material-symbols-outlined text-[14px]">check_circle</span>
                  Đã khai báo
                </span>
                <span v-else class="inline-flex items-center gap-1.5 py-1 px-3 rounded-full bg-amber-100 text-amber-800 font-bold text-[11px]">
                  <span class="material-symbols-outlined text-[14px]">warning</span>
                  Chưa khai báo
                </span>
              </div>
              <p class="text-xs text-slate-500 mt-0.5 font-medium">{{ enterpriseInfo.chiNhanh }}</p>
            </div>
          </div>

          <button
            @click="showEditModal = true"
            class="p-2 text-slate-400 hover:text-[#005EA3] hover:bg-slate-100 rounded-full transition self-start sm:self-center"
            title="Chỉnh sửa"
          >
            <span class="material-symbols-outlined text-[20px]">edit</span>
          </button>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-y-6 gap-x-12 text-xs relative z-10">
          <div class="space-y-1">
            <label class="font-bold text-[11px] text-slate-400 uppercase tracking-wider block">Địa chỉ công ty</label>
            <p class="font-medium text-slate-800 leading-relaxed">{{ enterpriseInfo.diaChi }}</p>
          </div>

          <div class="space-y-1">
            <label class="font-bold text-[11px] text-slate-400 uppercase tracking-wider block">Vị trí thực tập</label>
            <p class="font-semibold text-slate-900">{{ enterpriseInfo.viTri }}</p>
          </div>

          <div class="space-y-1">
            <label class="font-bold text-[11px] text-slate-400 uppercase tracking-wider block">Mentor hướng dẫn</label>
            <p class="font-bold text-slate-900">{{ enterpriseInfo.mentor }}</p>
          </div>

          <div class="space-y-1">
            <label class="font-bold text-[11px] text-slate-400 uppercase tracking-wider block">Giấy xác nhận thực tập</label>
            <div v-if="isEnterpriseDeclared" class="flex items-center justify-between mt-1 p-3 border border-slate-200 rounded-xl bg-slate-50/80">
              <div class="flex items-center gap-3">
                <span class="material-symbols-outlined text-rose-500 text-[22px]">description</span>
                <span class="font-semibold text-slate-800 truncate max-w-[180px]">Giay_xac_nhan.pdf</span>
              </div>
              <button
                @click="downloadDocument"
                class="p-1.5 hover:bg-slate-200 rounded-lg transition text-[#005EA3] font-bold flex items-center gap-1"
                title="Tải xuống"
              >
                <span class="material-symbols-outlined text-[18px]" :class="{ 'animate-spin': isDownloading }">
                  {{ isDownloading ? 'sync' : 'download' }}
                </span>
              </button>
            </div>
            <div v-else class="flex items-center justify-between mt-1 p-3 border border-dashed border-slate-200 rounded-xl bg-slate-50/50 text-slate-400">
              <div class="flex items-center gap-3">
                <span class="material-symbols-outlined text-slate-300 text-[22px]">description</span>
                <span class="font-semibold text-xs italic">Chưa có (Cập nhật sau khi khai báo thông tin)</span>
              </div>
            </div>
          </div>
        </div>

        <div class="pt-4 border-t border-dashed border-slate-200 flex items-center gap-2 text-slate-500 text-[11px] italic">
          <template v-if="isEnterpriseDeclared">
            <span class="material-symbols-outlined text-blue-600 text-[16px]">verified</span>
            <span>Thông tin đã được xác thực bởi văn phòng Khoa.</span>
          </template>
          <template v-else>
            <span class="material-symbols-outlined text-amber-600 text-[16px]">info</span>
            <span>Vui lòng nhấn nút "Cập nhật thông tin" để khai báo đơn vị tiếp nhận thực tập của em.</span>
          </template>
        </div>
      </div>
    </div>

    <div v-else-if="activeTab === 'topic'" class="space-y-6">
      <div class="bg-white p-4 rounded-2xl border border-slate-200 shadow-2xs flex flex-wrap items-center justify-between gap-3 text-xs">
        <div class="flex items-center gap-3">
          <span class="material-symbols-outlined text-[#005EA3] text-[24px]">assignment</span>
          <div>
            <div class="flex items-center gap-2">
              <span class="font-bold text-slate-800 text-sm">Đề tài thực tập của sinh viên</span>
              <span v-if="hasGroup" class="px-2.5 py-0.5 bg-[#005EA3]/10 text-[#005EA3] border border-[#005EA3]/20 rounded-lg text-[11px] font-extrabold flex items-center gap-1">
                <span class="material-symbols-outlined text-[14px]">groups</span>
                {{ userGroup }}
              </span>
              <span v-else class="px-2.5 py-0.5 bg-slate-100 text-slate-600 border border-slate-200 rounded-lg text-[11px] font-extrabold flex items-center gap-1">
                <span class="material-symbols-outlined text-[14px]">person</span>
                Cá nhân (Chưa có nhóm)
              </span>
            </div>
            <span class="text-[11px] text-slate-500">Mỗi nhóm thực hiện 01 đề tài duy nhất - Tất cả thành viên trong nhóm dùng chung đề tài</span>
          </div>
        </div>

        <div class="flex items-center gap-2">
          <span
            v-if="topicStatus === 'approved'"
            class="px-3.5 py-1.5 rounded-xl font-bold bg-emerald-100 text-emerald-800 border border-emerald-200 flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[16px]">check_circle</span>
            Trạng thái: Đã phê duyệt
          </span>

          <span
            v-else-if="topicStatus === 'pending'"
            class="px-3.5 py-1.5 rounded-xl font-bold bg-amber-100 text-amber-800 border border-amber-200 flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[16px]">pending</span>
            Trạng thái: Đang chờ GVHD duyệt
          </span>

          <span
            v-else-if="topicStatus === 'rejected'"
            class="px-3.5 py-1.5 rounded-xl font-bold bg-amber-100 text-amber-800 border border-amber-200 flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[16px]">rate_review</span>
            Trạng thái: Yêu cầu chỉnh sửa
          </span>

          <span
            v-else-if="topicStatus === 'denied'"
            class="px-3.5 py-1.5 rounded-xl font-bold bg-rose-100 text-rose-800 border border-rose-200 flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[16px]">cancel</span>
            Trạng thái: Bị từ chối
          </span>

          <span
            v-else-if="topicStatus === 'draft'"
            class="px-3.5 py-1.5 rounded-xl font-bold bg-amber-50 text-amber-800 border border-amber-200 flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[16px]">edit_square</span>
            Trạng thái: Nháp
          </span>

          <span
            v-else
            class="px-3.5 py-1.5 rounded-xl font-bold bg-slate-100 text-slate-700 border border-slate-200 flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[16px]">help_outline</span>
            Trạng thái: Chưa nộp đề tài
          </span>

          <button
            v-if="topicStatus === 'approved' || topicStatus === 'pending'"
            @click="openRequestEditModal"
            class="px-3 py-1.5 border border-amber-300 bg-amber-50 hover:bg-amber-100 text-amber-900 rounded-xl font-bold transition flex items-center gap-1 cursor-pointer select-none shadow-2xs"
            title="Gửi yêu cầu điều chỉnh đề tài"
          >
            <span class="material-symbols-outlined text-[16px] text-amber-700">edit_note</span>
            Xin sửa đề tài
          </button>
        </div>
      </div>

      <div v-if="topicStatus === 'rejected' || topicStatus === 'new' || topicStatus === 'draft'" class="grid grid-cols-1 lg:grid-cols-10 gap-8 items-start">
        <div class="lg:col-span-7 space-y-6">
          <div v-if="topicStatus === 'rejected'" class="rounded-2xl p-6 space-y-3 border border-amber-200 bg-amber-50/80 shadow-2xs">
            <div class="flex items-center gap-2 text-amber-700 font-extrabold text-base">
              <span class="material-symbols-outlined text-[24px]">rate_review</span>
              <span>Yêu cầu chỉnh sửa đề tài</span>
            </div>
            <p class="text-xs text-slate-800 leading-relaxed font-medium">
              Giảng viên hướng dẫn đã xem xét đề tài của bạn và yêu cầu điều chỉnh. Vui lòng xem phản hồi chi tiết bên dưới, hiệu chỉnh lại thông tin đề tài và nhấn <strong class="text-amber-900 font-bold">Gửi duyệt</strong> để nộp lại.
            </p>
            <div class="text-xs text-slate-800 leading-relaxed font-medium bg-white p-3 rounded-xl border border-amber-100">
              <strong class="text-amber-900 font-bold">Ý kiến phản hồi từ GVHD:</strong> "{{ topicFeedback || 'Cần làm rõ thêm phạm vi đề tài, bổ sung quy trình thực tế.' }}"
            </div>
          </div>

          <div v-else-if="topicStatus === 'denied'" class="rounded-2xl p-6 space-y-3 border border-rose-200 bg-rose-50/80 shadow-2xs">
            <div class="flex items-center gap-2 text-rose-700 font-extrabold text-base">
              <span class="material-symbols-outlined text-[24px]">cancel</span>
              <span>Đề tài đã bị từ chối</span>
            </div>
            <p class="text-xs text-slate-800 leading-relaxed font-medium">
              Giảng viên hướng dẫn đã từ chối đề tài này của bạn. Bạn cần đăng ký một đề tài hoàn toàn mới để tiếp tục thực tập.
            </p>
            <div class="text-xs text-slate-800 leading-relaxed font-medium bg-white p-3 rounded-xl border border-rose-100">
              <strong class="text-rose-900 font-bold">Lý do từ chối từ GVHD:</strong> "{{ topicFeedback || 'Đề tài chưa phù hợp với chuyên ngành thực tập.' }}"
            </div>
          </div>

          <div v-if="topicStatus === 'denied'" class="bg-slate-50 border border-slate-200 rounded-2xl p-5 space-y-3">
            <div class="flex items-center gap-2 text-slate-500 font-extrabold text-xs uppercase tracking-wide">
              <span class="material-symbols-outlined text-[18px]">lock</span>
              <span>Thông tin đề tài đã bị từ chối (Chỉ xem)</span>
            </div>
            <div class="space-y-3 text-xs">
              <div>
                <p class="text-slate-900 font-bold mb-1">Tên đề tài cũ:</p>
                <div class="font-medium text-slate-700 p-2.5 bg-white rounded-lg border border-slate-200">
                  {{ rejectedTopicName || '— Chưa cập nhật —' }}
                </div>
              </div>
              <div>
                <p class="text-slate-900 font-bold mb-1">Mô tả chi tiết cũ:</p>
                <div class="font-medium text-slate-600 p-3 bg-white rounded-lg border border-slate-200 leading-relaxed whitespace-pre-wrap">
                  {{ rejectedTopicDesc || '— Chưa cập nhật —' }}
                </div>
              </div>
            </div>
          </div>

          <div v-if="topicStatus === 'draft'" class="rounded-2xl p-6 space-y-3 border border-amber-200 bg-amber-50/50 shadow-2xs">
            <div class="flex items-center gap-2 text-amber-700 font-extrabold text-base">
              <span class="material-symbols-outlined text-[24px]">drafts</span>
              <span>Bản nháp đề tài đang chỉnh sửa</span>
            </div>
            <p class="text-xs text-slate-800 leading-relaxed font-medium">
              Bạn đang làm việc trên bản nháp đề tài. Bạn có thể tiếp tục chỉnh sửa hoàn thiện và nhấn <strong class="text-amber-900 font-bold">Gửi duyệt</strong> để nộp lên Giảng viên hướng dẫn.
            </p>
          </div>

          <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs space-y-6">
            <h2 class="text-base font-extrabold text-[#005EA3] uppercase tracking-wide flex items-center gap-2">
              <span class="material-symbols-outlined text-[20px]">edit_document</span>
              <span v-if="topicStatus === 'rejected'">HIỆU CHỈNH ĐỀ TÀI THỰC TẬP</span>
              <span v-else-if="topicStatus === 'denied'">ĐĂNG KÝ ĐỀ TÀI MỚI</span>
              <span v-else>FORM ĐĂNG KÝ ĐỀ TÀI THỰC TẬP</span>
            </h2>

            <div class="space-y-4 text-xs">
              <div class="space-y-1.5">
                <label class="block font-bold text-slate-700 uppercase tracking-wider text-[11px]">
                  <span v-if="topicStatus === 'rejected'">Tên đề tài</span>
                  <span v-else-if="topicStatus === 'denied'">Tên đề tài mới</span>
                  <span v-else>Tên đề tài đăng ký</span>
                  <span class="text-rose-600 font-black"> *</span>
                </label>
                <div class="relative">
                  <input
                    v-model="newTopicName"
                    @input="topicNameError = ''"
                    type="text"
                    maxlength="500"
                    :placeholder="topicStatus === 'rejected' ? 'Nhập tên đề tài thực tập hiệu chỉnh...' : (topicStatus === 'denied' ? 'Nhập tên đề tài thực tập mới...' : 'Nhập tên đề tài thực tập đăng ký...')"
                    class="w-full p-3.5 border rounded-xl outline-none focus:ring-2 focus:ring-blue-600 font-medium text-slate-900"
                    :class="topicNameError ? 'border-rose-500 bg-rose-50/20' : 'border-slate-300'"
                  />
                  <div class="absolute bottom-3 right-3 text-[10px] text-slate-400 font-mono">
                    {{ (newTopicName || '').length }}/500
                  </div>
                </div>
                <p v-if="topicNameError" class="text-rose-600 font-bold text-[11px] mt-1">{{ topicNameError }}</p>
              </div>

              <div class="space-y-1.5">
                <div class="flex justify-between items-end">
                  <label class="block font-bold text-slate-700 uppercase tracking-wider text-[11px]">
                    Mô tả sơ lược <span class="text-rose-600 font-black">*</span>
                  </label>
                  <span class="text-[11px] text-slate-400 italic">Mô tả ngắn gọn nội dung, phạm vi và mục tiêu đề tài</span>
                </div>
                <div class="relative">
                  <textarea
                    v-model="newTopicDesc"
                    @input="topicDescError = ''"
                    maxlength="5000"
                    rows="6"
                    :placeholder="topicStatus === 'rejected' ? 'Hiệu chỉnh nội dung mô tả đề tài...' : (topicStatus === 'denied' ? 'Nội dung mô tả đề tài mới...' : 'Nội dung mô tả đề tài đăng ký...')"
                    class="w-full p-3.5 border rounded-xl outline-none focus:ring-2 focus:ring-blue-600 font-medium text-slate-900 resize-none"
                    :class="topicDescError ? 'border-rose-500 bg-rose-50/20' : 'border-slate-300'"
                  ></textarea>
                  <div class="absolute bottom-3 right-3 text-[10px] text-slate-400 font-mono">
                    {{ (newTopicDesc || '').length }}/5000
                  </div>
                </div>
                <p v-if="topicDescError" class="text-rose-600 font-bold text-[11px] mt-1">{{ topicDescError }}</p>
              </div>

              <div class="space-y-1.5">
                <label class="block font-bold text-slate-700 uppercase tracking-wider text-[11px]">
                  <span v-if="topicStatus === 'rejected'">File đề cương chi tiết</span>
                  <span v-else-if="topicStatus === 'denied'">File đề cương chi tiết mới</span>
                  <span v-else>File đề cương chi tiết</span>
                </label>
                <div
                  @click="triggerOutlineUpload"
                  @dragover.prevent
                  @drop.prevent="onOutlineDrop"
                  class="border-2 border-dashed border-slate-300 hover:border-[#005EA3] bg-slate-50/50 hover:bg-blue-50/30 rounded-2xl p-8 text-center cursor-pointer transition flex flex-col items-center justify-center space-y-2"
                >
                  <span class="material-symbols-outlined text-4xl text-slate-400">cloud_upload</span>
                  <p class="font-bold text-slate-800 text-xs">Kéo thả file vào đây hoặc nhấn để chọn</p>
                  <p class="text-[11px] text-slate-400">Hỗ trợ .doc, .docx, .pdf (Dung lượng tối đa 10MB)</p>
                  <input ref="outlineInputRef" type="file" accept=".doc,.docx,.pdf" class="hidden" @change="onOutlineSelected" />
                </div>

                <div v-if="outlineFileName" class="p-3 bg-blue-50 border border-blue-200 rounded-xl flex items-center justify-between">
                  <div class="flex items-center gap-2">
                    <span class="material-symbols-outlined text-rose-600">
                      {{ (outlineFileName || '').toLowerCase().endsWith('.pdf') ? 'picture_as_pdf' : 'description' }}
                    </span>
                    <span class="font-bold text-slate-900 text-xs">{{ outlineFileName }}</span>
                  </div>
                  <button @click="clearSelectedOutline" class="text-slate-400 hover:text-rose-600 font-bold">✕</button>
                </div>
              </div>
            </div>

            <div class="pt-4 border-t border-slate-100 flex items-center justify-end gap-3 text-xs">
              <button
                @click="saveDraftTopic"
                class="px-6 py-2.5 border border-slate-300 text-slate-700 font-bold rounded-xl hover:bg-slate-100 transition"
              >
                Lưu nháp
              </button>
              <button
                @click="submitTopicProposal"
                class="px-8 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-md transition active:scale-95"
              >
                Gửi duyệt
              </button>
            </div>
          </div>
        </div>

        <div class="lg:col-span-3 space-y-6">
          <div class="bg-blue-50/90 p-4 rounded-2xl border border-blue-200 flex items-start gap-3 text-xs">
            <span class="material-symbols-outlined text-[#005EA3] shrink-0 text-[20px]">info</span>
            <p v-if="hasGroup" class="text-blue-900 leading-snug font-medium">
              Bạn đang trong <strong class="font-extrabold text-[#005EA3]">{{ userGroup }}</strong>. Khi bạn đăng ký đề tài, đề tài sẽ tự động áp dụng cho toàn bộ thành viên trong nhóm.
            </p>
            <p v-else class="text-blue-900 leading-snug font-medium">
              Bạn đang đăng ký đề tài với tư cách <strong class="font-extrabold text-[#005EA3]">Cá nhân</strong>. Nếu bạn thực hiện theo nhóm, vui lòng liên hệ Giảng viên hướng dẫn để được ghép nhóm chính thức trên hệ thống.
            </p>
          </div>

          <div v-if="topicStatus !== 'new'" class="bg-white p-5 rounded-2xl border border-slate-200 shadow-2xs space-y-4">
            <h3 class="font-extrabold text-xs text-slate-500 uppercase tracking-wider flex items-center gap-1.5">
              <span class="material-symbols-outlined text-[16px] text-[#005EA3]">history</span>
              LỊCH SỬ PHIÊN BẢN
            </h3>

            <div class="relative pl-6 border-l-2 space-y-4 text-xs" :class="topicStatus === 'denied' ? 'border-rose-200' : 'border-amber-200'">
              <div class="relative">
                <div class="absolute -left-[31px] top-0 w-4 h-4 rounded-full bg-white border-2 flex items-center justify-center" :class="topicStatus === 'denied' ? 'border-rose-600' : 'border-amber-600'">
                  <div class="w-1.5 h-1.5 rounded-full" :class="topicStatus === 'denied' ? 'bg-rose-600' : 'bg-amber-600'"></div>
                </div>

                <div class="space-y-1">
                  <div class="flex items-center justify-between">
                    <span class="font-extrabold text-slate-900">Phiên bản 1</span>
                    <span 
                      class="px-2 py-0.5 font-bold rounded text-[10px]"
                      :class="topicStatus === 'denied' ? 'bg-rose-100 text-rose-800' : 'bg-amber-100 text-amber-800'"
                    >
                      {{ topicStatus === 'denied' ? 'Từ chối' : 'Yêu cầu sửa' }}
                    </span>
                  </div>
                  <p class="text-[10px] text-slate-400 font-medium">12/07/2026 14:30</p>
                  <p class="text-slate-600 text-[11px] leading-relaxed pt-1 border-t border-slate-100">
                    <strong>Ý kiến phản hồi:</strong> {{ topicFeedback || 'Cần làm rõ thêm phạm vi đề tài, bổ sung quy trình thực tế.' }}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div v-else-if="topicStatus === 'pending'" class="bg-white border border-slate-200 rounded-2xl p-8 shadow-2xs space-y-6">
        <div class="flex flex-wrap items-start justify-between border-b pb-4 gap-4">
          <div>
            <span class="px-3 py-1 bg-amber-100 text-amber-800 rounded-md text-[11px] font-bold uppercase tracking-wider">
              Chờ duyệt từ GVHD
            </span>
            <h3 class="font-extrabold text-lg text-slate-900 mt-2">
              {{ newTopicName || '— Chưa cập nhật tên đề tài —' }}
            </h3>
          </div>
          <div v-if="hasGroup" class="px-3.5 py-1.5 bg-[#005EA3]/10 text-[#005EA3] border border-[#005EA3]/20 rounded-xl font-extrabold text-xs flex items-center gap-1.5 shrink-0">
            <span class="material-symbols-outlined text-[18px]">groups</span>
            {{ userGroup }} (Dùng chung đề tài)
          </div>
          <div v-else class="px-3.5 py-1.5 bg-slate-100 text-slate-600 border border-slate-200 rounded-xl font-extrabold text-xs flex items-center gap-1.5 shrink-0">
            <span class="material-symbols-outlined text-[18px]">person</span>
            Cá nhân
          </div>
        </div>

        <div class="space-y-5 text-xs text-slate-700 leading-relaxed">
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-3 bg-slate-50 p-4 rounded-xl border border-slate-100">
            <p><strong>Ngày gửi:</strong> {{ submissionDate || '26/08/2025 09:15' }}</p>
            <p><strong>Giảng viên nhận:</strong> TS. Nguyễn Văn Thành</p>
          </div>

          <div>
            <h4 class="font-bold text-slate-900 mb-2 flex items-center justify-between">
              <span class="flex items-center gap-1.5">
                <span class="material-symbols-outlined text-[#005EA3] text-[18px]">{{ hasGroup ? 'groups' : 'person' }}</span>
                {{ hasGroup ? `Thành viên ${userGroup} (Đang áp dụng cùng đề tài này):` : 'Thông tin người đăng ký đề tài:' }}
              </span>
            </h4>
            <div class="grid grid-cols-1 sm:grid-cols-3 gap-3">
              <div class="p-3 bg-blue-50/60 rounded-xl border border-blue-100 flex items-center justify-between" :class="{ 'sm:col-span-3': !hasGroup }">
                <div>
                  <p class="font-bold text-slate-900 text-xs">{{ currentStudentName }}</p>
                  <p class="text-[11px] text-slate-500 font-mono">MSSV: {{ currentStudentMssv }}</p>
                </div>
                <span class="px-2 py-0.5 bg-[#005EA3] text-white text-[10px] font-bold rounded-md">
                  {{ hasGroup ? 'Trưởng nhóm' : 'Sinh viên đăng ký' }}
                </span>
              </div>
              <template v-if="hasGroup">
                <div class="p-3 bg-white rounded-xl border border-slate-200 flex items-center justify-between">
                  <div>
                    <p class="font-bold text-slate-800 text-xs">Trần Thị Bình</p>
                    <p class="text-[11px] text-slate-500 font-mono">MSSV: 20241002</p>
                  </div>
                  <span class="px-2 py-0.5 bg-slate-100 text-slate-600 text-[10px] font-bold rounded-md">Thành viên</span>
                </div>
                <div class="p-3 bg-white rounded-xl border border-slate-200 flex items-center justify-between">
                  <div>
                    <p class="font-bold text-slate-800 text-xs">Lê Hoàng Cường</p>
                    <p class="text-[11px] text-slate-500 font-mono">MSSV: 20241003</p>
                  </div>
                  <span class="px-2 py-0.5 bg-slate-100 text-slate-600 text-[10px] font-bold rounded-md">Thành viên</span>
                </div>
              </template>
            </div>
          </div>

          <div>
            <h4 class="font-bold text-slate-900 mb-1">Mô tả sơ lược đề tài:</h4>
            <div class="bg-slate-50 p-4 rounded-xl border border-slate-100 whitespace-pre-wrap leading-relaxed text-slate-700">
              {{ newTopicDesc || '— Chưa cập nhật mô tả sơ lược —' }}
            </div>
          </div>

          <div v-if="outlineFileName" class="p-3 bg-blue-50/50 border border-blue-200 rounded-xl flex items-center justify-between">
            <div class="flex items-center gap-2">
              <span class="material-symbols-outlined text-rose-600">
                {{ (outlineFileName || '').toLowerCase().endsWith('.pdf') ? 'picture_as_pdf' : 'description' }}
              </span>
              <div>
                <p class="font-bold text-slate-900 text-xs">File đề cương chi tiết đính kèm:</p>
                <p class="text-[11px] text-slate-600 font-mono">{{ outlineFileName }}</p>
              </div>
            </div>
            <button @click="downloadOutlineFile" class="text-blue-700 hover:underline font-bold text-xs flex items-center gap-1 cursor-pointer">
              <span class="material-symbols-outlined text-[16px]">download</span>
              Tải file
            </button>
          </div>

          <p class="bg-amber-50 p-4 rounded-xl border border-amber-200 text-amber-900 font-medium">
            Đề tài đã được gửi tới Giảng viên hướng dẫn TS. Nguyễn Văn Thành. Vui lòng chờ phản hồi trong vòng 24 - 48 giờ.
          </p>
        </div>
      </div>

      <div v-else-if="topicStatus === 'approved' || topicStatus === 'denied'" class="bg-white border border-slate-200 rounded-2xl p-8 shadow-2xs space-y-6">
        <div class="flex flex-wrap items-start justify-between border-b pb-4 gap-4">
          <div>
            <span
              v-if="topicStatus === 'approved'"
              class="px-2.5 py-1 bg-emerald-100 text-emerald-800 rounded-md text-[11px] font-bold uppercase tracking-wider"
            >
              Đã được GVHD duyệt
            </span>
            <span
              v-else
              class="px-2.5 py-1 bg-rose-100 text-rose-800 rounded-md text-[11px] font-bold uppercase tracking-wider"
            >
              Đã bị GVHD từ chối
            </span>
            <h3 class="font-extrabold text-lg text-slate-900 mt-2">
              {{ newTopicName || '— Chưa cập nhật tên đề tài —' }}
            </h3>
          </div>
          <div v-if="hasGroup" class="px-3.5 py-1.5 bg-[#005EA3]/10 text-[#005EA3] border border-[#005EA3]/20 rounded-xl font-extrabold text-xs flex items-center gap-1.5 shrink-0">
            <span class="material-symbols-outlined text-[18px]">groups</span>
            {{ userGroup }} (Dùng chung đề tài)
          </div>
          <div v-else class="px-3.5 py-1.5 bg-slate-100 text-slate-600 border border-slate-200 rounded-xl font-extrabold text-xs flex items-center gap-1.5 shrink-0">
            <span class="material-symbols-outlined text-[18px]">person</span>
            Cá nhân
          </div>
        </div>

        <div v-if="topicFeedback" :class="topicStatus === 'approved' ? 'bg-emerald-50 border-emerald-200 text-emerald-900' : 'bg-rose-50 border-rose-200 text-rose-900'" class="rounded-2xl p-5 border shadow-2xs space-y-2">
          <div class="flex items-center gap-2 font-bold text-xs">
            <span class="material-symbols-outlined text-[18px]">{{ topicStatus === 'approved' ? 'check_circle' : 'cancel' }}</span>
            <span>{{ topicStatus === 'approved' ? 'Ý kiến phản hồi / Nhận xét của Giáo viên hướng dẫn:' : 'Lý do Giáo viên từ chối đề tài:' }}</span>
          </div>
          <p class="text-xs text-slate-800 leading-relaxed font-medium bg-white p-3 rounded-xl border" :class="topicStatus === 'approved' ? 'border-emerald-100' : 'border-rose-100'">
            "{{ topicFeedback }}"
          </p>
        </div>

        <div class="space-y-5 text-xs text-slate-700 leading-relaxed">
          <div class="bg-slate-50/80 p-4 rounded-2xl border border-slate-200 space-y-3">
            <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-2">
              <h4 class="font-bold text-slate-900 flex items-center gap-1.5">
                <span class="material-symbols-outlined text-[#005EA3] text-[18px]">{{ hasGroup ? 'groups' : 'person' }}</span>
                {{ hasGroup ? `Thành viên trong ${userGroup} (Thực hiện chung đề tài này):` : 'Thông tin người đăng ký đề tài:' }}
              </h4>
              <span v-if="hasGroup" class="text-[11px] text-blue-800 bg-blue-50 px-2.5 py-0.5 rounded-full font-bold border border-blue-200">
                01 Đề tài cho 03 Thành viên
              </span>
              <span v-else class="text-[11px] text-slate-600 bg-slate-100 px-2.5 py-0.5 rounded-full font-bold border border-slate-200">
                Đăng ký cá nhân
              </span>
            </div>
            <div class="grid grid-cols-1 sm:grid-cols-3 gap-3">
              <div class="p-3 bg-white rounded-xl border border-slate-200 flex items-center justify-between shadow-2xs" :class="{ 'sm:col-span-3': !hasGroup }">
                <div>
                  <p class="font-bold text-slate-900 text-xs">{{ currentStudentName }}</p>
                  <p class="text-[11px] text-slate-500 font-mono">MSSV: {{ currentStudentMssv }}</p>
                </div>
                <span class="px-2 py-0.5 bg-[#005EA3] text-white text-[10px] font-bold rounded-md">
                  {{ hasGroup ? 'Trưởng nhóm' : 'Sinh viên đăng ký' }}
                </span>
              </div>
              <template v-if="hasGroup">
                <div class="p-3 bg-white rounded-xl border border-slate-200 flex items-center justify-between shadow-2xs">
                  <div>
                    <p class="font-bold text-slate-800 text-xs">Trần Thị Bình</p>
                    <p class="text-[11px] text-slate-500 font-mono">MSSV: 20241002</p>
                  </div>
                  <span class="px-2 py-0.5 bg-slate-100 text-slate-600 text-[10px] font-bold rounded-md">Thành viên</span>
                </div>
                <div class="p-3 bg-white rounded-xl border border-slate-200 flex items-center justify-between shadow-2xs">
                  <div>
                    <p class="font-bold text-slate-800 text-xs">Lê Hoàng Cường</p>
                    <p class="text-[11px] text-slate-500 font-mono">MSSV: 20241003</p>
                  </div>
                  <span class="px-2 py-0.5 bg-slate-100 text-slate-600 text-[10px] font-bold rounded-md">Thành viên</span>
                </div>
              </template>
            </div>
          </div>

          <div>
            <h4 class="font-bold text-slate-900 mb-1">Mô tả sơ lược đề tài:</h4>
            <p class="bg-slate-50 p-4 rounded-xl border border-slate-100 whitespace-pre-wrap leading-relaxed text-slate-700">
              {{ newTopicDesc || '— Chưa cập nhật mô tả sơ lược đề tài —' }}
            </p>
          </div>

          <div v-if="outlineFileName" class="p-3 bg-blue-50/50 border border-blue-200 rounded-xl flex items-center justify-between">
            <div class="flex items-center gap-2">
              <span class="material-symbols-outlined text-rose-600">
                {{ (outlineFileName || '').toLowerCase().endsWith('.pdf') ? 'picture_as_pdf' : 'description' }}
              </span>
              <div>
                <p class="font-bold text-slate-900 text-xs">File đề cương chi tiết đính kèm:</p>
                <p class="text-[11px] text-slate-600 font-mono">{{ outlineFileName }}</p>
              </div>
            </div>
            <button @click="downloadOutlineFile" class="text-blue-700 hover:underline font-bold text-xs flex items-center gap-1 cursor-pointer">
              <span class="material-symbols-outlined text-[16px]">download</span>
              Tải file
            </button>
          </div>

          <div>
            <h4 class="font-bold text-slate-900 mb-1">Công nghệ ứng dụng đề xuất:</h4>
            <div class="flex flex-wrap gap-2">
              <span class="px-3 py-1 bg-blue-50 text-[#005EA3] font-bold rounded-lg border border-blue-100">Vue.js 3</span>
              <span class="px-3 py-1 bg-emerald-50 text-emerald-800 font-bold rounded-lg border border-emerald-100">Tailwind CSS</span>
              <span class="px-3 py-1 bg-indigo-50 text-indigo-800 font-bold rounded-lg border border-indigo-100">Node.js Express</span>
              <span class="px-3 py-1 bg-amber-50 text-amber-800 font-bold rounded-lg border border-amber-100">PostgreSQL</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-else-if="activeTab === 'diaries'">
      <StudentDiaryTab classId="L001" @writing-change="onDiaryWritingChange" />
    </div>

    <div v-else-if="activeTab === 'schedule'" class="space-y-6">
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-2xs space-y-3">
          <span class="px-2.5 py-1 bg-rose-50 text-rose-700 border border-rose-200 rounded-lg text-[10px] font-black uppercase tracking-wider flex items-center gap-1 w-fit">
            <span class="material-symbols-outlined text-[14px]">assignment</span>
            Hạn nộp Đề tài
          </span>
          <h3 class="font-bold text-sm text-slate-900">15/07/2026 — 23:59</h3>
          <p class="text-xs text-slate-500">Đăng ký & Nộp đề cương đề tài thực tập chính thức.</p>
          <span v-if="topicStatus === 'approved'" class="inline-flex items-center gap-1 text-emerald-700 font-bold text-xs bg-emerald-50 px-2.5 py-1 rounded-md border border-emerald-200">
            <span class="material-symbols-outlined text-[14px]">check_circle</span> Đã phê duyệt đề tài
          </span>
          <span v-else-if="topicStatus === 'pending'" class="inline-flex items-center gap-1 text-amber-700 font-bold text-xs bg-amber-50 px-2.5 py-1 rounded-md border border-amber-200 animate-pulse">
            <span class="material-symbols-outlined text-[14px]">pending</span> Chờ duyệt đề tài
          </span>
          <span v-else-if="topicStatus === 'rejected'" class="inline-flex items-center gap-1 text-rose-700 font-bold text-xs bg-rose-50 px-2.5 py-1 rounded-md border border-rose-200">
            <span class="material-symbols-outlined text-[14px]">cancel</span> Cần sửa đổi
          </span>
          <span v-else class="inline-flex items-center gap-1 text-rose-600 font-bold text-xs bg-rose-50 px-2.5 py-1 rounded-md border border-rose-200">
            <span class="material-symbols-outlined text-[14px]">error</span> Chưa đăng ký đề tài
          </span>
        </div>

        <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-2xs space-y-3">
          <span class="px-2.5 py-1 bg-amber-50 text-amber-800 border border-amber-200 rounded-lg text-[10px] font-black uppercase tracking-wider flex items-center gap-1 w-fit">
            <span class="material-symbols-outlined text-[14px]">description</span>
            Hạn nộp Báo cáo Giữa kỳ
          </span>
          <h3 class="font-bold text-sm text-slate-900">15/08/2026 — 17:00</h3>
          <p class="text-xs text-slate-500">Nộp file PDF báo cáo tiến độ giữa kỳ có con dấu xác nhận.</p>
          <span class="inline-flex items-center gap-1 text-amber-800 font-bold text-xs bg-amber-50 px-2.5 py-1 rounded-md">
            <span class="material-symbols-outlined text-[14px]">schedule</span> Còn 18 ngày
          </span>
        </div>

        <div class="bg-white p-5 rounded-2xl border border-slate-200 shadow-2xs space-y-3">
          <span class="px-2.5 py-1 bg-emerald-50 text-emerald-800 border border-emerald-200 rounded-lg text-[10px] font-black uppercase tracking-wider flex items-center gap-1 w-fit">
            <span class="material-symbols-outlined text-[14px]">groups</span>
            Lịch họp với GVHD
          </span>
          <h3 class="font-bold text-sm text-slate-900">25/07/2026 — 14:00</h3>
          <p class="text-xs text-slate-500">TS. Nguyễn Văn A — Review sơ đồ kiến trúc ứng dụng.</p>
          <span class="inline-flex items-center gap-1 text-emerald-800 font-bold text-xs bg-emerald-50 px-2.5 py-1 rounded-md">
            <span class="material-symbols-outlined text-[14px]">videocam</span> Google Meet
          </span>
        </div>
      </div>

      <div class="bg-gradient-to-r from-blue-900 to-[#005EA3] p-6 rounded-2xl text-white shadow-md flex flex-col sm:flex-row items-center justify-between gap-4">
        <div>
          <h3 class="font-extrabold text-base flex items-center gap-2">
            <span class="material-symbols-outlined text-amber-300">calendar_month</span>
            Quản lý Lịch trình Thực tập Chi tiết
          </h3>
          <p class="text-xs text-blue-100 mt-1">
            Xem lịch tuần, lịch tháng, đặt nhắc nhở và thêm các lịch họp/ghi chú cá nhân với giảng viên.
          </p>
        </div>
        <router-link
          to="/student/calendar"
          class="px-5 py-2.5 bg-white text-[#005EA3] font-extrabold rounded-xl text-xs hover:bg-blue-50 transition shadow-xs shrink-0 flex items-center gap-1.5"
        >
          <span>Mở Lịch Thực Tập</span>
          <span class="material-symbols-outlined text-[18px]">arrow_forward</span>
        </router-link>
      </div>
    </div>

    <div
      v-if="showRequestEditModal"
      class="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-slate-900/60 backdrop-blur-xs font-sans"
      @click.self="showRequestEditModal = false"
    >
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-md p-6 space-y-5 border border-slate-200">
        <div class="flex justify-between items-center border-b pb-3">
          <h3 class="font-bold text-slate-900 text-base flex items-center gap-2">
            <span class="material-symbols-outlined text-amber-600">edit_note</span>
            Xin chỉnh sửa lại đề tài
          </h3>
          <button @click="showRequestEditModal = false" class="text-slate-400 hover:text-slate-600 font-bold">✕</button>
        </div>

        <div class="space-y-3 text-xs">
          <p class="text-slate-600 leading-relaxed">
            Bạn đang yêu cầu mở lại quyền chỉnh sửa đề tài. Vui lòng nhập lý do cần điều chỉnh (ví dụ: thay đổi phạm vi, cập nhật công nghệ, bổ sung tính năng...):
          </p>

          <div>
            <label class="block font-bold text-slate-700 mb-1">Lý do điều chỉnh đề tài (*)</label>
            <textarea
              v-model="requestEditReason"
              rows="3"
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-amber-500 font-medium text-xs"
              placeholder="Nhập lý do chi tiết để gửi đến Giảng viên hướng dẫn..."
            ></textarea>
          </div>
        </div>

        <div class="pt-3 border-t flex justify-end gap-2.5 text-xs">
          <button @click="showRequestEditModal = false" class="px-4 py-2 border border-slate-300 rounded-xl font-bold text-slate-700 hover:bg-slate-100">
            Hủy bỏ
          </button>
          <button @click="confirmRequestEditTopic" class="px-5 py-2 bg-amber-600 hover:bg-amber-700 text-white font-bold rounded-xl shadow-xs">
            Xác nhận xin sửa
          </button>
        </div>
      </div>
    </div>

    <div
      v-if="showEditModal"
      class="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-slate-900/50 backdrop-blur-xs font-sans"
      @click.self="showEditModal = false"
    >
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-lg p-6 space-y-5">
        <div class="flex justify-between items-center border-b pb-3">
          <h3 class="font-bold text-slate-900 text-base flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3]">edit_square</span>
            Cập nhật đơn vị thực tập
          </h3>
          <button @click="showEditModal = false" class="text-slate-400 hover:text-slate-600">✕</button>
        </div>

        <div class="space-y-4 text-xs">
          <div>
            <label class="block font-bold text-slate-700 mb-1">Tên Doanh nghiệp / Công ty (*)</label>
            <input
              v-model="enterpriseInfo.tenCongTy"
              type="text"
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600 font-semibold"
            />
          </div>

          <div>
            <label class="block font-bold text-slate-700 mb-1">Chi nhánh / Tòa nhà (*)</label>
            <input
              v-model="enterpriseInfo.chiNhanh"
              type="text"
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
            />
          </div>

          <div>
            <label class="block font-bold text-slate-700 mb-1">Địa chỉ chi tiết (*)</label>
            <input
              v-model="enterpriseInfo.diaChi"
              type="text"
              class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
            />
          </div>

          <div class="grid grid-cols-2 gap-3">
            <div>
              <label class="block font-bold text-slate-700 mb-1">Vị trí thực tập (*)</label>
              <input
                v-model="enterpriseInfo.viTri"
                type="text"
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
              />
            </div>
            <div>
              <label class="block font-bold text-slate-700 mb-1">Mentor hướng dẫn (*)</label>
              <input
                v-model="enterpriseInfo.mentor"
                type="text"
                class="w-full px-3.5 py-2.5 rounded-xl border border-slate-300 outline-none focus:ring-2 focus:ring-blue-600"
              />
            </div>
          </div>
        </div>

        <div class="pt-4 border-t flex justify-end gap-3 text-xs">
          <button @click="showEditModal = false" class="px-4 py-2 border border-slate-300 rounded-xl font-bold text-slate-700 hover:bg-slate-100">
            Hủy
          </button>
          <button @click="saveEnterpriseInfo" class="px-5 py-2 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-xs">
            Lưu thông tin
          </button>
        </div>
      </div>
    </div>

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
  import { ref, computed, onMounted, onUnmounted, watch } from 'vue';
  import { useAuthStore } from '@/stores/auth';
  import { sendRealtimeNotification } from '@/services/notificationService';
  import { listenStudentTopic, saveTopicProposalToDb } from '@/services/topicService';
  import { getStudentDiariesFromDb } from '@/services/diaryService';
  import StudentDiaryTab from './StudentDiaryTab.vue';

  const authStore = useAuthStore();
  const currentStudentName = computed(() => authStore.user?.hoTen || 'Sinh viên');
  const currentStudentMssv = computed(() => authStore.user?.maDinhDanh || authStore.user?.username || '---');

  function normalizeTopicStatus(status) {
    if (!status) return 'new';
    const s = String(status).trim();
    const lower = s.toLowerCase();

    if (s === 'Pending' || lower === 'pending' || lower === 'chờ duyệt') {
      return 'pending';
    }
    if (
      s === 'Requested Changes' ||
      s === 'RequestedChanges' ||
      lower === 'requested_changes' ||
      lower === 'requested changes' ||
      lower === 'yêu cầu chỉnh sửa' ||
      lower === 'yêu cầu sửa' ||
      (lower === 'rejected' && s !== 'Rejected' && s !== 'rejected_final')
    ) {
      return 'rejected'; // 'rejected' key represents Requested Changes in the UI
    }
    if (
      s === 'Rejected' ||
      lower === 'rejected_final' ||
      lower === 'rejected final' ||
      lower === 'denied' ||
      lower === 'từ chối'
    ) {
      return 'denied'; // 'denied' key represents Rejected (final rejection) in the UI
    }
    if (s === 'Draft' || lower === 'draft' || lower === 'nháp') {
      return 'draft';
    }
    if (s === 'Approved' || lower === 'approved' || lower === 'đã duyệt') {
      return 'approved';
    }
    if (s === 'New' || lower === 'new' || lower === 'chưa nộp') {
      return 'new';
    }

    return lower;
  }

  function clearFormPersistence(studentId) {
    if (!studentId) return;
    localStorage.removeItem(`ims_student_topic_name_${studentId}`);
    localStorage.removeItem(`ims_student_topic_desc_${studentId}`);
    localStorage.removeItem(`ims_student_topic_outline_${studentId}`);
    localStorage.removeItem(`ims_student_topic_outline_data_${studentId}`);
  }

  function clearSelectedOutline() {
    outlineFileName.value = '';
    outlineFileData.value = '';
    if (outlineInputRef.value) {
      outlineInputRef.value.value = '';
    }
    const user = authStore.user || {};
    const studentId = user.maDinhDanh || '20241001';
    localStorage.removeItem(`ims_student_topic_outline_${studentId}`);
    localStorage.removeItem(`ims_student_topic_outline_data_${studentId}`);
  }

  const hasGroup = computed(() => {
    const user = authStore.user || {};
    const studentId = user.maDinhDanh || '';
    if (!studentId || studentId === 'default' || studentId === '---') return false;
    if (studentId === '20241001') return true;
    
    const savedGroup = localStorage.getItem(`ims_student_group_${studentId}`);
    return !!savedGroup;
  });

  const userGroup = computed(() => {
    const user = authStore.user || {};
    const studentId = user.maDinhDanh || '';
    if (studentId === '20241001') return 'Nhóm 06';
    return localStorage.getItem(`ims_student_group_${studentId}`) || 'Chưa có nhóm';
  });

  const activeTab = ref('diaries');
  const isDiaryWriting = ref(false);
  const topicStatus = ref('new');
  const showEditModal = ref(false);

  const breadcrumbTail = computed(() => {
    if (activeTab.value === 'diaries' && isDiaryWriting.value) return 'Viết nhật ký mới';
    if (activeTab.value === 'diaries') return 'Nhật ký';
    if (activeTab.value === 'topic') return 'Đề tài';
    return 'Thông tin thực tập';
  });

  function onDiaryWritingChange(writing) {
    isDiaryWriting.value = writing;
  }
  const showRequestEditModal = ref(false);
  const requestEditReason = ref('');
  const toastMsg = ref('');
  const isDownloading = ref(false);

  const newTopicName = ref('');
  const newTopicDesc = ref('');
  const rejectedTopicName = ref('');
  const rejectedTopicDesc = ref('');
  const topicNameError = ref('');
  const topicDescError = ref('');
  const outlineFileName = ref('');
  const outlineFileData = ref('');
  const topicFeedback = ref('');
  const submissionDate = ref('');
  const outlineInputRef = ref(null);

  // Diaries & Progress state
  const allDiaries = ref([]);
  const currentWeek = ref(5);
  const weekRequiredCount = ref(3); // Example: 3 diaries required per week
  const totalRequiredCount = ref(12); // Example: 12 total diaries required

  const currentWeekDiaries = computed(() => {
    return allDiaries.value.filter(d => d.weekNumber === currentWeek.value).sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
  });

  const weekSubmittedCount = computed(() => {
    return currentWeekDiaries.value.filter(d => d.status === 'Submitted' || d.status === 'Graded').length;
  });

  const weekProgressPercent = computed(() => {
    if (weekRequiredCount.value === 0) return 0;
    return Math.min(100, Math.round((weekSubmittedCount.value / weekRequiredCount.value) * 100));
  });

  const totalSubmittedCount = computed(() => {
    return allDiaries.value.filter(d => d.status === 'Submitted' || d.status === 'Graded').length;
  });

  const totalProgressPercent = computed(() => {
    if (totalRequiredCount.value === 0) return 0;
    return Math.min(100, Math.round((totalSubmittedCount.value / totalRequiredCount.value) * 100));
  });

  const weekDateRange = computed(() => {
    if (currentWeek.value === 5) return '15/05 - 21/05/2024';
    if (currentWeek.value === 6) return '22/05 - 28/05/2024';
    if (currentWeek.value === 4) return '08/05 - 14/05/2024';
    return '... / ... / 2024';
  });

  function changeWeek(delta) {
    if (currentWeek.value + delta > 0 && currentWeek.value + delta <= 15) {
      currentWeek.value += delta;
    }
  }

  function formatShortDate(dateStr) {
    if (!dateStr) return '';
    const d = new Date(dateStr);
    if (isNaN(d.getTime())) return dateStr;
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();
    return `${day}/${month}/${year}`;
  }

  function formatDateTime(dateStr) {
    if (!dateStr) return '';
    const d = new Date(dateStr);
    if (isNaN(d.getTime())) return dateStr;
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();
    const hr = String(d.getHours()).padStart(2, '0');
    const min = String(d.getMinutes()).padStart(2, '0');
    return `${day}/${month}/${year} - ${hr}:${min}`;
  }

  function getDayOfWeek(dateStr) {
    if (!dateStr) return '';
    const d = new Date(dateStr);
    if (isNaN(d.getTime())) return '';
    const days = ['Chủ nhật', '2', '3', '4', '5', '6', '7'];
    return days[d.getDay()];
  }

  function triggerToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      if (toastMsg.value === msg) toastMsg.value = '';
    }, 3500);
  }

  const defaultEnterpriseFPT = {
    tenCongTy: 'Công ty Cổ phần Công nghệ FPT (FPT Software Đà Nẵng)',
    chiNhanh: 'Chi nhánh FPT Complex - Ngũ Hành Sơn',
    diaChi: 'Tòa nhà FPT Complex, Đường Võ Chí Công, Quận Ngũ Hành Sơn, TP. Đà Nẵng',
    viTri: 'Intern Web Developer',
    mentor: 'Anh Nguyễn Hữu Tuấn (Senior Architect - FPT Software)'
  };

  const defaultEnterpriseClean = {
    tenCongTy: 'Chưa khai báo đơn vị thực tập',
    chiNhanh: 'Vui lòng cập nhật đơn vị tiếp nhận',
    diaChi: 'Chưa cập nhật địa chỉ công ty',
    viTri: 'Thực tập sinh',
    mentor: 'Chưa cập nhật Mentor'
  };

  const enterpriseInfo = ref({ ...defaultEnterpriseClean });

  const isEnterpriseDeclared = computed(() => {
    const user = authStore.user || {};
    const studentId = user.maDinhDanh || '';
    if (studentId === '20241001') return true;
    return enterpriseInfo.value &&
           enterpriseInfo.value.tenCongTy &&
           enterpriseInfo.value.tenCongTy !== 'Chưa khai báo đơn vị thực tập' &&
           enterpriseInfo.value.tenCongTy !== '';
  });

  watch([newTopicName, newTopicDesc, outlineFileName, outlineFileData], () => {
    const user = authStore.user || {};
    const studentId = user.maDinhDanh || '20241001';
    const norm = normalizeTopicStatus(topicStatus.value);
    if (norm === 'draft' || norm === 'rejected') {
      localStorage.setItem(`ims_student_topic_name_${studentId}`, newTopicName.value || '');
      localStorage.setItem(`ims_student_topic_desc_${studentId}`, newTopicDesc.value || '');
      localStorage.setItem(`ims_student_topic_outline_${studentId}`, outlineFileName.value || '');
      localStorage.setItem(`ims_student_topic_outline_data_${studentId}`, outlineFileData.value || '');
    } else {
      clearFormPersistence(studentId);
    }
  });

  let unsubscribeTopic = null;

  onUnmounted(() => {
    if (unsubscribeTopic) {
      unsubscribeTopic();
    }
  });

  onMounted(async () => {
    try {
      const user = authStore.user || JSON.parse(localStorage.getItem('user') || '{}');
      const studentId = user?.maDinhDanh || '20241001';

      if (studentId && studentId !== 'default' && studentId !== '---') {
        const dbDiaries = await getStudentDiariesFromDb(studentId);
        allDiaries.value = dbDiaries;

        unsubscribeTopic = listenStudentTopic(studentId, (topicData) => {
          const savedName = localStorage.getItem(`ims_student_topic_name_${studentId}`);
          const savedDesc = localStorage.getItem(`ims_student_topic_desc_${studentId}`);
          const savedOutline = localStorage.getItem(`ims_student_topic_outline_${studentId}`);
          const savedOutlineData = localStorage.getItem(`ims_student_topic_outline_data_${studentId}`);
          const savedStatus = localStorage.getItem(`ims_student_topic_status_${studentId}`);
          const savedFeedback = localStorage.getItem(`ims_student_topic_feedback_${studentId}`);
          const savedDate = localStorage.getItem(`ims_student_topic_date_${studentId}`);

          if (topicData) {
            const normStatus = normalizeTopicStatus(topicData.status || savedStatus || 'new');
            topicStatus.value = normStatus;

            if (normStatus === 'rejected') {
              rejectedTopicName.value = '';
              rejectedTopicDesc.value = '';
              newTopicName.value = topicData.title || savedName || '';
              newTopicDesc.value = topicData.description || savedDesc || '';
              outlineFileName.value = topicData.file || topicData.fileName || savedOutline || '';
              outlineFileData.value = topicData.fileUrl || topicData.fileData || savedOutlineData || '';
            } else if (normStatus === 'denied') {
              rejectedTopicName.value = topicData.title || savedName || '';
              rejectedTopicDesc.value = topicData.description || savedDesc || '';
              newTopicName.value = topicData.title || savedName || '';
              newTopicDesc.value = topicData.description || savedDesc || '';
              outlineFileName.value = topicData.file || topicData.fileName || savedOutline || '';
              outlineFileData.value = topicData.fileUrl || topicData.fileData || savedOutlineData || '';
              clearFormPersistence(studentId);
            } else if (normStatus === 'pending') {
              rejectedTopicName.value = '';
              rejectedTopicDesc.value = '';
              newTopicName.value = topicData.title || savedName || '';
              newTopicDesc.value = topicData.description || savedDesc || '';
              outlineFileName.value = topicData.file || topicData.fileName || savedOutline || '';
              outlineFileData.value = topicData.fileUrl || topicData.fileData || savedOutlineData || '';
              clearFormPersistence(studentId);
            } else if (normStatus === 'draft') {
              rejectedTopicName.value = '';
              rejectedTopicDesc.value = '';
              newTopicName.value = topicData.title || savedName || '';
              newTopicDesc.value = topicData.description || savedDesc || '';
              outlineFileName.value = topicData.file || topicData.fileName || savedOutline || '';
              outlineFileData.value = topicData.fileUrl || topicData.fileData || savedOutlineData || '';
            } else {
              rejectedTopicName.value = '';
              rejectedTopicDesc.value = '';
              newTopicName.value = topicData.title || savedName || '';
              newTopicDesc.value = topicData.description || savedDesc || '';
              outlineFileName.value = topicData.file || topicData.fileName || savedOutline || '';
              outlineFileData.value = topicData.fileUrl || topicData.fileData || savedOutlineData || '';
              if (normStatus === 'approved') {
                clearFormPersistence(studentId);
              }
            }
            topicFeedback.value = topicData.feedback || savedFeedback || '';
            submissionDate.value = topicData.date || savedDate || '';
          } else {
            const normStatus = normalizeTopicStatus(savedStatus || ((studentId === '20241001') ? 'approved' : 'new'));
            topicStatus.value = normStatus;

            if (normStatus === 'rejected') {
              rejectedTopicName.value = '';
              rejectedTopicDesc.value = '';
              newTopicName.value = savedName || '';
              newTopicDesc.value = savedDesc || '';
              outlineFileName.value = savedOutline || '';
              outlineFileData.value = savedOutlineData || '';
            } else if (normStatus === 'denied') {
              rejectedTopicName.value = savedName || 'Xây dựng Hệ thống Quản lý Thực tập Doanh nghiệp (IMS) dựa trên Kiến trúc Microservices';
              rejectedTopicDesc.value = savedDesc || 'Nghiên cứu quy trình quản lý thực tập tại Đại học Kinh tế - Đại học Đà Nẵng, thiết kế giải pháp phần mềm hỗ trợ chấm điểm nhật ký, quản lý danh sách sinh viên, đăng ký đề tài và phân quyền nhiều vai trò.';
              newTopicName.value = savedName || 'Xây dựng Hệ thống Quản lý Thực tập Doanh nghiệp (IMS) dựa trên Kiến trúc Microservices';
              newTopicDesc.value = savedDesc || 'Nghiên cứu quy trình quản lý thực tập tại Đại học Kinh tế - Đại học Đà Nẵng, thiết kế giải pháp phần mềm hỗ trợ chấm điểm nhật ký, quản lý danh sách sinh viên, đăng ký đề tài và phân quyền nhiều vai trò.';
              outlineFileName.value = savedOutline || '';
              outlineFileData.value = savedOutlineData || '';
              clearFormPersistence(studentId);
            } else if (normStatus === 'pending') {
              rejectedTopicName.value = '';
              rejectedTopicDesc.value = '';
              newTopicName.value = savedName || '';
              newTopicDesc.value = savedDesc || '';
              outlineFileName.value = savedOutline || '';
              outlineFileData.value = savedOutlineData || '';
              clearFormPersistence(studentId);
            } else {
              if (savedName) {
                newTopicName.value = savedName;
              } else if (studentId === '20241001') {
                newTopicName.value = 'Xây dựng Hệ thống Quản lý Thực tập Doanh nghiệp (IMS) dựa trên Kiến trúc Microservices';
              }

              if (savedDesc) {
                newTopicDesc.value = savedDesc;
              } else if (studentId === '20241001') {
                newTopicDesc.value = 'Nghiên cứu quy trình quản lý thực tập tại Đại học Kinh tế - Đại học Đà Nẵng, thiết kế giải pháp phần mềm hỗ trợ chấm điểm nhật ký, quản lý danh sách sinh viên, đăng ký đề tài và phân quyền nhiều vai trò.';
              }

              if (savedOutline) {
                outlineFileName.value = savedOutline;
                outlineFileData.value = savedOutlineData || '';
              }
            }

            if (savedFeedback) {
              topicFeedback.value = savedFeedback;
            }

            if (savedDate) {
              submissionDate.value = savedDate;
            }
          }
        });
      } else {
        if (studentId === '20241001') {
          topicStatus.value = 'approved';
          newTopicName.value = 'Xây dựng Hệ thống Quản lý Thực tập Doanh nghiệp (IMS) dựa trên Kiến trúc Microservices';
          newTopicDesc.value = 'Nghiên cứu quy trình quản lý thực tập tại Đại học Kinh tế - Đại học Đà Nẵng, thiết kế giải pháp phần mềm hỗ trợ chấm điểm nhật ký, quản lý danh sách sinh viên, đăng ký đề tài và phân quyền nhiều vai trò.';
        } else {
          topicStatus.value = 'new';
        }
      }

      if (studentId === '20241001') {
        enterpriseInfo.value = { ...defaultEnterpriseFPT };
      } else {
        const savedEnterprise = localStorage.getItem(`ims_student_enterprise_${studentId}`);
        if (savedEnterprise) {
          enterpriseInfo.value = JSON.parse(savedEnterprise);
        } else {
          enterpriseInfo.value = { ...defaultEnterpriseClean };
        }
      }
    } catch (e) {
      console.error(e);
    }
  });

  function openRequestEditModal() {
    requestEditReason.value = '';
    showRequestEditModal.value = true;
  }

  function confirmRequestEditTopic() {
    showRequestEditModal.value = false;
    topicStatus.value = 'rejected';
    const user = authStore.user || {};
    const studentId = user.maDinhDanh || '20241001';
    localStorage.setItem(`ims_student_topic_status_${studentId}`, 'rejected');
    localStorage.setItem('ims_student_topic_status', 'rejected');

    if (!newTopicName.value) {
      newTopicName.value = 'Xây dựng Hệ thống Quản lý Thực tập Doanh nghiệp (IMS) dựa trên Kiến trúc Microservices';
    }
    if (!newTopicDesc.value) {
      newTopicDesc.value = 'Nghiên cứu quy trình quản lý thực tập tại Đại học Kinh tế - Đại học Đà Nẵng, thiết kế giải pháp phần mềm hỗ trợ chấm điểm nhật ký, quản lý danh sách sinh viên, đăng ký đề tài và phân quyền nhiều vai trò.';
    }

    localStorage.setItem(`ims_student_topic_name_${studentId}`, newTopicName.value);
    localStorage.setItem(`ims_student_topic_desc_${studentId}`, newTopicDesc.value);
    localStorage.setItem(`ims_student_topic_outline_${studentId}`, outlineFileName.value);

    saveTopicProposalToDb(studentId, {
      title: newTopicName.value,
      description: newTopicDesc.value,
      file: outlineFileName.value,
      status: 'rejected',
      studentName: user.hoTen || 'Sinh viên',
      class: 'L01',
      group: userGroup.value,
      date: submissionDate.value || new Date().toLocaleDateString('vi-VN') + ' ' + new Date().toTimeString().slice(0, 5)
    });

    const studentName = user.hoTen || 'Nguyễn Văn An';

    sendRealtimeNotification({
      role: 'GiangVien',
      userId: 'GV001',
      title: 'Yêu cầu xin sửa đề tài thực tập',
      content: `Sinh viên ${studentName} (MSSV: ${studentId}) vừa gửi đề xuất xin chỉnh sửa đề tài. Lý do: ${requestEditReason.value || 'Cần bổ sung phạm vi.'}`,
      icon: 'edit_note',
      bgClass: 'bg-amber-100 text-amber-800',
      link: '/teacher/classes/L01/topics'
    });

    triggerToast('✓ Đã gửi yêu cầu xin chỉnh sửa đề tài! Bạn có thể cập nhật thông tin ở biểu mẫu bên dưới.');
  }

  function triggerOutlineUpload() {
    outlineInputRef.value?.click();
  }

  function validateAndSetFile(file) {
    if (!file) return;
    if (file.size > 10 * 1024 * 1024) {
      triggerToast('Lỗi: Dung lượng file vượt quá giới hạn 10MB!');
      return;
    }
    const ext = file.name.split('.').pop().toLowerCase();
    if (ext !== 'pdf' && ext !== 'doc' && ext !== 'docx') {
      triggerToast('Lỗi: Vui lòng tải file định dạng hợp lệ (.doc, .docx, .pdf)!');
      return;
    }
    outlineFileName.value = file.name;

    const reader = new FileReader();
    reader.onload = (evt) => {
      outlineFileData.value = evt.target?.result || '';
      const user = authStore.user || {};
      const studentId = user.maDinhDanh || '20241001';
      localStorage.setItem(`ims_student_topic_outline_data_${studentId}`, outlineFileData.value);
    };
    reader.onerror = () => {
      console.error('Error reading selected file');
    };
    reader.readAsDataURL(file);

    triggerToast('✓ Đã tải lên file đề cương chi tiết: ' + file.name);
  }

  function downloadOutlineFile() {
    if (outlineFileData.value) {
      const link = document.createElement('a');
      link.href = outlineFileData.value;
      link.download = outlineFileName.value || 'De_cuong_chi_tiet.pdf';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      triggerToast('✓ Đã tải xuống file đề cương thành công!');
    } else {
      triggerToast('✓ Đang tải xuống file đề cương: ' + (outlineFileName.value || 'De_cuong_chi_tiet.pdf'));
    }
  }

  function onOutlineSelected(e) {
    const file = e.target.files?.[0];
    if (file) {
      validateAndSetFile(file);
    }
  }

  function onOutlineDrop(e) {
    const file = e.dataTransfer?.files?.[0];
    if (file) {
      validateAndSetFile(file);
    }
  }

  function saveDraftTopic() {
    const user = authStore.user || {};
    const studentId = user.maDinhDanh || '20241001';

    topicNameError.value = '';
    topicDescError.value = '';
    let hasError = false;

    const nameVal = (newTopicName.value || '').trim();
    const descVal = (newTopicDesc.value || '').trim();

    if (nameVal.length > 500) {
      topicNameError.value = 'Tên đề tài không được vượt quá 500 ký tự';
      hasError = true;
    }
    if (descVal.length > 5000) {
      topicDescError.value = 'Mô tả không được vượt quá 5000 ký tự';
      hasError = true;
    }

    if (hasError) {
      triggerToast('Vui lòng đảm bảo thông tin không vượt quá giới hạn ký tự!');
      return;
    }

    topicStatus.value = 'draft';
    localStorage.setItem(`ims_student_topic_status_${studentId}`, 'draft');
    localStorage.setItem('ims_student_topic_status', 'draft');
    localStorage.setItem(`ims_student_topic_name_${studentId}`, newTopicName.value || '');
    localStorage.setItem(`ims_student_topic_desc_${studentId}`, newTopicDesc.value || '');
    localStorage.setItem(`ims_student_topic_outline_${studentId}`, outlineFileName.value || '');
    localStorage.setItem(`ims_student_topic_outline_data_${studentId}`, outlineFileData.value || '');

    saveTopicProposalToDb(studentId, {
      title: newTopicName.value || '',
      description: newTopicDesc.value || '',
      file: outlineFileName.value || '',
      fileName: outlineFileName.value || '',
      fileUrl: outlineFileData.value || '',
      fileData: outlineFileData.value || '',
      status: 'draft',
      studentName: user.hoTen || 'Sinh viên',
      class: 'L01',
      group: userGroup.value,
      date: submissionDate.value || new Date().toLocaleDateString('vi-VN') + ' ' + new Date().toTimeString().slice(0, 5)
    });

    triggerToast('Đã lưu nháp đề tài thành công!');
  }

  function submitTopicProposal() {
    topicNameError.value = '';
    topicDescError.value = '';

    let hasError = false;
    const nameVal = (newTopicName.value || '').trim();
    const descVal = (newTopicDesc.value || '').trim();

    if (!nameVal) {
      topicNameError.value = 'Vui lòng nhập tên đề tài';
      hasError = true;
    } else if (nameVal.length > 500) {
      topicNameError.value = 'Tên đề tài không được vượt quá 500 ký tự';
      hasError = true;
    }

    if (!descVal) {
      topicDescError.value = 'Vui lòng nhập mô tả';
      hasError = true;
    } else if (descVal.length > 5000) {
      topicDescError.value = 'Mô tả không được vượt quá 5000 ký tự';
      hasError = true;
    }

    if (hasError) {
      triggerToast('Vui lòng điền đầy đủ thông tin bắt buộc và đúng quy định!');
      return;
    }

    const user = authStore.user || {};
    const studentId = user.maDinhDanh || '20241001';
    const studentName = user.hoTen || 'Sinh viên';

    topicStatus.value = 'pending';
    const nowStr = new Date().toLocaleDateString('vi-VN') + ' ' + new Date().toTimeString().slice(0, 5);
    submissionDate.value = nowStr;

    localStorage.setItem(`ims_student_topic_status_${studentId}`, 'pending');
    localStorage.setItem('ims_student_topic_status', 'pending');
    clearFormPersistence(studentId);
    localStorage.setItem(`ims_student_topic_date_${studentId}`, nowStr);

    saveTopicProposalToDb(studentId, {
      title: newTopicName.value,
      description: newTopicDesc.value,
      file: outlineFileName.value || '',
      fileName: outlineFileName.value || '',
      fileUrl: outlineFileData.value || '',
      fileData: outlineFileData.value || '',
      status: 'pending',
      studentName,
      class: 'L01',
      group: userGroup.value,
      date: nowStr
    });

    sendRealtimeNotification({
      role: 'GiangVien',
      userId: 'GV001',
      title: 'Đề tài thực tập mới gửi duyệt',
      content: `Sinh viên ${studentName} (MSSV: ${studentId}) vừa đăng ký đề tài: "${newTopicName.value}".`,
      icon: 'add_task',
      bgClass: 'bg-blue-100 text-blue-700',
      link: '/teacher/classes/L01/topics'
    });

    triggerToast('Đã gửi đăng ký đề tài thành công tới Giảng viên hướng dẫn!');
  }

  function downloadDocument() {
    isDownloading.value = true;
    setTimeout(() => {
      isDownloading.value = false;
      triggerToast('Tải tập tin Giay_xac_nhan.pdf thành công!');
    }, 1200);
  }

  function saveEnterpriseInfo() {
    const user = authStore.user || {};
    const studentId = user.maDinhDanh || 'default';
    localStorage.setItem(`ims_student_enterprise_${studentId}`, JSON.stringify(enterpriseInfo.value));
    showEditModal.value = false;
    triggerToast('Đã cập nhật thông tin doanh nghiệp!');
  }
</script>
