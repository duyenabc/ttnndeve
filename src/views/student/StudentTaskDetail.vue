<template>
  <div class="ims-scope max-w-5xl mx-auto font-sans space-y-6 pb-20">
    <!-- Breadcrumb -->
    <nav class="flex items-center text-xs text-slate-500 gap-1.5">
      <router-link to="/student/classes" class="hover:text-[#005EA3] transition font-medium">Lớp của tôi</router-link>
      <span class="material-symbols-outlined text-[14px]">chevron_right</span>
      <span class="hover:text-[#005EA3] transition font-medium">Lớp Thực tập K64</span>
      <span class="material-symbols-outlined text-[14px]">chevron_right</span>
      <router-link to="/student/tasks" class="hover:text-[#005EA3] transition font-medium">Nhiệm vụ</router-link>
      <span class="material-symbols-outlined text-[14px]">chevron_right</span>
      <span class="text-slate-900 font-bold">Chi tiết nhiệm vụ</span>
    </nav>

    <!-- Task Header & Demo State Selector Switcher -->
    <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs space-y-4">
      <div class="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-slate-100 pb-4">
        <div>
          <div class="flex items-center gap-2 mb-1">
            <span class="px-2.5 py-0.5 rounded-md bg-blue-100 text-[#005EA3] font-bold text-[10px] uppercase">
              Môn học: Thực tập tốt nghiệp
            </span>
            <span class="text-slate-400 text-xs">•</span>
            <span class="text-xs text-slate-500 font-medium">GVHD: TS. Nguyễn Văn A</span>
          </div>
          <h1 class="text-2xl font-extrabold text-slate-900 tracking-tight">{{ taskData.title }}</h1>
        </div>

        <!-- Interactive Quick Switcher for Demo -->
        <div class="flex items-center gap-1.5 bg-slate-100 p-1 rounded-xl text-xs font-bold shrink-0">
          <button
            @click="setSubmissionState('unsubmitted')"
            class="px-3 py-1.5 rounded-lg transition"
            :class="submissionState === 'unsubmitted' ? 'bg-white text-slate-900 shadow-2xs' : 'text-slate-500 hover:text-slate-900'"
          >
            Chưa nộp
          </button>
          <button
            @click="setSubmissionState('submitted')"
            class="px-3 py-1.5 rounded-lg transition"
            :class="submissionState === 'submitted' ? 'bg-white text-slate-900 shadow-2xs' : 'text-slate-500 hover:text-slate-900'"
          >
            Đã nộp
          </button>
          <button
            @click="setSubmissionState('graded')"
            class="px-3 py-1.5 rounded-lg transition"
            :class="submissionState === 'graded' ? 'bg-white text-slate-900 shadow-2xs' : 'text-slate-500 hover:text-slate-900'"
          >
            Đã chấm điểm
          </button>
        </div>
      </div>

      <!-- Task Instructions & Description -->
      <div class="space-y-3 text-xs text-slate-700 leading-relaxed">
        <h3 class="font-bold text-slate-900 text-sm flex items-center gap-1.5">
          <span class="material-symbols-outlined text-[#005EA3] text-[18px]">description</span>
          Yêu cầu nhiệm vụ:
        </h3>
        <p>Trong tuần này, sinh viên cần thực hiện báo cáo chi tiết về tiến độ công việc tại đơn vị thực tập. Nội dung báo cáo bao gồm:</p>
        <ul class="list-disc pl-5 space-y-1.5 text-slate-600">
          <li>Mô tả các đầu việc đã hoàn thành trong tuần (ít nhất 3 đầu việc).</li>
          <li>Các vấn đề phát sinh và giải pháp xử lý kỹ thuật (nếu có).</li>
          <li>Kế hoạch dự kiến chi tiết cho tuần tiếp theo.</li>
          <li>Hình ảnh minh họa, sơ đồ kiến trúc hoặc link kho mã nguồn sản phẩm (nếu có).</li>
        </ul>
        <div class="p-3 bg-blue-50/80 border border-blue-100 rounded-xl text-blue-900 font-medium text-[11px] flex items-center gap-2">
          <span class="material-symbols-outlined text-[#005EA3] text-[18px]">info</span>
          Báo cáo phải được nộp dưới định dạng PDF, DOCX hoặc ZIP. Dung lượng tối đa không quá 50MB.
        </div>
      </div>

      <!-- Cấu hình điểm Sub Items & Tiêu chí đánh giá -->
      <div class="border-t border-slate-100 pt-4 space-y-3">
        <div class="flex items-center justify-between">
          <h3 class="font-bold text-slate-900 text-xs uppercase tracking-wider flex items-center gap-1.5">
            <span class="material-symbols-outlined text-[#005EA3] text-[16px]">checklist</span>
            Cấu hình điểm theo Sub Items / Tiêu chí thành phần:
          </h3>
          <span class="text-[11px] font-bold text-blue-700 bg-blue-50 px-2 py-0.5 rounded-md border border-blue-200">
            Tổng điểm tối đa: 10.0 pts
          </span>
        </div>

        <div class="border border-slate-200 rounded-xl overflow-x-auto bg-slate-50/50">
          <table class="w-full text-left text-xs">
            <thead class="bg-slate-100 font-bold text-slate-600 text-[11px] uppercase border-b border-slate-200">
              <tr>
                <th class="px-4 py-2.5">Hạng mục con (Sub Item)</th>
                <th class="px-3 py-2.5 text-center">Thang điểm</th>
                <th class="px-3 py-2.5 text-center">Tỷ lệ %</th>
                <th class="px-3 py-2.5 text-center">Yêu cầu tối thiểu</th>
                <th class="px-3 py-2.5 text-right">Điểm đạt được</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-slate-200">
              <tr v-for="sub in taskSubItems" :key="sub.id" class="hover:bg-white transition">
                <td class="px-4 py-3">
                  <span class="font-bold text-slate-900 block">{{ sub.name }}</span>
                  <span class="text-[11px] text-slate-500">{{ sub.description }}</span>
                </td>
                <td class="px-3 py-3 text-center font-bold text-slate-800">
                  {{ sub.maxPoints }} pts
                </td>
                <td class="px-3 py-3 text-center font-bold text-blue-700">
                  {{ sub.weightRatio }}%
                </td>
                <td class="px-3 py-3 text-center">
                  <span class="px-2 py-0.5 rounded text-[10px] font-extrabold bg-amber-100 text-amber-900">
                    ≥ {{ sub.minPass }}đ
                  </span>
                </td>
                <td class="px-3 py-3 text-right font-black text-sm">
                  <span v-if="submissionState === 'graded'" class="text-[#005EA3]">
                    {{ sub.earnedPoints }} / {{ sub.maxPoints }}
                  </span>
                  <span v-else class="text-slate-400 font-normal">Chưa chấm</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Submission Status Table (Trạng thái nộp bài) -->
    <div class="bg-white rounded-2xl border border-slate-200 shadow-2xs overflow-hidden">
      <div class="px-6 py-4 bg-slate-50/80 border-b border-slate-200 flex items-center justify-between">
        <h3 class="font-bold text-sm text-slate-900 flex items-center gap-2">
          <span class="material-symbols-outlined text-[#005EA3]">fact_check</span>
          Trạng thái nộp bài
        </h3>
        <span
          v-if="submissionState === 'submitted'"
          class="px-3 py-1 bg-emerald-100 text-emerald-800 text-xs font-extrabold rounded-full flex items-center gap-1"
        >
          <span class="material-symbols-outlined text-[16px]">check_circle</span> Đã nộp bài
        </span>
        <span
          v-else-if="submissionState === 'graded'"
          class="px-3 py-1 bg-blue-100 text-[#005EA3] text-xs font-extrabold rounded-full flex items-center gap-1"
        >
          <span class="material-symbols-outlined text-[16px]">verified</span> Đã có điểm
        </span>
        <span
          v-else
          class="px-3 py-1 bg-slate-200 text-slate-700 text-xs font-bold rounded-full flex items-center gap-1"
        >
          <span class="material-symbols-outlined text-[16px]">pending</span> Chưa nộp
        </span>
      </div>

      <!-- Detail Rows Table -->
      <div class="divide-y divide-slate-100 text-xs font-medium">
        <!-- Row 1: Submission status -->
        <div class="grid grid-cols-1 sm:grid-cols-3 p-4 bg-slate-50/50">
          <span class="text-slate-500 font-semibold">Trạng thái nộp bài</span>
          <div class="sm:col-span-2">
            <span
              v-if="submissionState === 'submitted' || submissionState === 'graded'"
              class="inline-block px-3 py-1 bg-emerald-100 text-emerald-800 font-extrabold rounded-md text-xs"
            >
              Đã nộp
            </span>
            <span v-else class="text-slate-700 font-bold">Chưa nộp</span>
          </div>
        </div>

        <!-- Row 2: Grading status -->
        <div class="grid grid-cols-1 sm:grid-cols-3 p-4 bg-white">
          <span class="text-slate-500 font-semibold">Trạng thái chấm điểm</span>
          <div class="sm:col-span-2 flex items-center justify-between">
            <span v-if="submissionState === 'graded'" class="font-black text-emerald-700 text-sm">
              8.5 / 10.0 (Đạt loại Xuất sắc)
            </span>
            <span v-else class="text-slate-700">Chưa chấm điểm</span>

            <button
              v-if="submissionState === 'graded'"
              @click="showAssessmentModal = true"
              class="px-3 py-1 bg-blue-50 text-[#005EA3] border border-blue-200 rounded-lg hover:bg-blue-100 transition font-bold text-[11px] flex items-center gap-1"
            >
              <span class="material-symbols-outlined text-[14px]">visibility</span>
              Xem kết quả đánh giá
            </button>
          </div>
        </div>

        <!-- Row 3: Deadline -->
        <div class="grid grid-cols-1 sm:grid-cols-3 p-4 bg-slate-50/50">
          <span class="text-slate-500 font-semibold">Hạn chót</span>
          <span class="sm:col-span-2 font-bold text-slate-900 font-mono">Thứ ba, 26 tháng 8 2025, 9:00 AM</span>
        </div>

        <!-- Row 4: Time remaining -->
        <div class="grid grid-cols-1 sm:grid-cols-3 p-4 bg-white">
          <span class="text-slate-500 font-semibold">Thời gian còn lại</span>
          <div class="sm:col-span-2">
            <span
              v-if="submissionState === 'submitted' || submissionState === 'graded'"
              class="inline-block px-3 py-1 bg-emerald-50 text-emerald-700 border border-emerald-200 rounded-md font-bold text-xs"
            >
              Bài nộp được nộp sớm 14 phút 26 giây
            </span>
            <span v-else class="text-emerald-600 font-bold flex items-center gap-1">
              <span class="material-symbols-outlined text-[16px]">schedule</span>
              Còn 2 ngày 4 giờ
            </span>
          </div>
        </div>

        <!-- Row 5: Last modified -->
        <div class="grid grid-cols-1 sm:grid-cols-3 p-4 bg-slate-50/50">
          <span class="text-slate-500 font-semibold">Chỉnh sửa lần cuối</span>
          <span class="sm:col-span-2 font-mono text-slate-800">
            {{ submissionState !== 'unsubmitted' ? 'Thứ ba, 26 tháng 8 2025, 8:45 AM' : '—' }}
          </span>
        </div>

        <!-- Row 6: Submitted Files -->
        <div v-if="submissionState !== 'unsubmitted'" class="grid grid-cols-1 sm:grid-cols-3 p-4 bg-white items-center">
          <span class="text-slate-500 font-semibold">File bài nộp</span>
          <div class="sm:col-span-2 space-y-2">
            <div
              v-for="(file, idx) in uploadedFiles"
              :key="idx"
              class="inline-flex items-center gap-2 px-3 py-1.5 bg-slate-100 border border-slate-200 rounded-lg text-xs font-medium text-slate-800 hover:bg-slate-200 transition"
            >
              <span class="material-symbols-outlined text-[#005EA3] text-[18px]">description</span>
              <span class="font-mono font-bold text-slate-900">{{ file.name }}</span>
              <span class="text-slate-400 text-[10px] ml-2">({{ file.size }} • {{ file.uploadedAt }})</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Submission Action Controls & Inline File Upload Area -->
    <div class="bg-white p-6 rounded-2xl border border-slate-200 shadow-2xs space-y-6">
      <div v-if="!showInlineUpload" class="flex flex-col items-center justify-center py-4 space-y-3 text-center">
        <div class="flex flex-wrap items-center justify-center gap-3">
          <button
            v-if="submissionState === 'unsubmitted'"
            @click="showInlineUpload = true"
            class="px-8 py-3 bg-[#005EA3] hover:bg-blue-800 text-white font-bold text-xs rounded-xl shadow-md transition-all active:scale-95 flex items-center gap-2"
          >
            <span class="material-symbols-outlined text-[18px]">add</span>
            Thêm bài nộp
          </button>

          <template v-else>
            <button
              @click="showInlineUpload = true"
              class="px-6 py-2.5 bg-slate-100 hover:bg-slate-200 text-slate-800 font-bold text-xs rounded-xl border border-slate-300 transition flex items-center gap-2"
            >
              <span class="material-symbols-outlined text-[18px]">edit</span>
              Sửa bài nộp
            </button>
            <button
              @click="openFilePicker"
              class="px-6 py-2.5 bg-blue-50 hover:bg-blue-100 text-[#005EA3] font-bold text-xs rounded-xl border border-blue-200 transition flex items-center gap-2"
            >
              <span class="material-symbols-outlined text-[18px]">folder_open</span>
              Mở Bộ chọn tệp (Modal)
            </button>
            <button
              @click="removeSubmission"
              class="px-6 py-2.5 bg-rose-50 hover:bg-rose-100 text-rose-700 font-bold text-xs rounded-xl border border-rose-200 transition flex items-center gap-2"
            >
              <span class="material-symbols-outlined text-[18px]">delete</span>
              Gỡ bài nộp
            </button>
          </template>
        </div>

        <p class="text-xs text-slate-500 font-medium">
          {{ submissionState === 'unsubmitted' ? 'Bạn chưa nộp bài. Bấm "Thêm bài nộp" để tải tệp lên.' : 'Bạn vẫn có thể sửa hoặc thay đổi bài nộp trước hạn chót.' }}
        </p>
      </div>

      <!-- Inline Drag & Drop File Upload Form -->
      <div v-else class="space-y-6 border-t border-slate-100 pt-4">
        <div class="flex items-center justify-between">
          <h4 class="font-bold text-sm text-slate-900 flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3]">upload_file</span>
            Khu vực tải bài nộp
          </h4>
          <button @click="showInlineUpload = false" class="text-xs font-bold text-slate-500 hover:text-slate-800">
            Đóng ✕
          </button>
        </div>

        <!-- Drag & Drop Container -->
        <div
          @dragover.prevent="isDragging = true"
          @dragleave.prevent="isDragging = false"
          @drop.prevent="handleFileDrop"
          @click="triggerFileInput"
          class="border-2 border-dashed p-8 rounded-2xl text-center transition cursor-pointer space-y-2"
          :class="isDragging ? 'border-[#005EA3] bg-blue-50/80' : 'border-slate-300 hover:border-[#005EA3] hover:bg-slate-50/80'"
        >
          <input ref="fileInputRef" type="file" multiple class="hidden" @change="handleFileSelect" />
          <span class="material-symbols-outlined text-slate-400 text-[42px]">cloud_upload</span>
          <p class="font-bold text-xs text-slate-800">Kéo thả file vào đây hoặc click để chọn file từ máy tính</p>
          <p class="text-[11px] text-slate-400">Định dạng chấp nhận: .pdf, .docx, .zip | Dung lượng tối đa: 50MB | Tối đa 2 file</p>
        </div>

        <!-- Pending Selected Files List -->
        <div v-if="pendingFiles.length > 0" class="space-y-2">
          <p class="font-bold text-xs text-slate-700">Các tệp đã chọn để tải lên:</p>
          <div
            v-for="(pf, i) in pendingFiles"
            :key="i"
            class="flex items-center justify-between p-3 bg-slate-50 rounded-xl border border-slate-200 text-xs"
          >
            <div class="flex items-center gap-2.5">
              <span class="material-symbols-outlined text-[#005EA3]">description</span>
              <div>
                <p class="font-bold text-slate-900 font-mono">{{ pf.name }}</p>
                <p class="text-[10px] text-slate-400">{{ (pf.size / 1024 / 1024).toFixed(2) }} MB</p>
              </div>
            </div>

            <button @click="removePendingFile(i)" class="text-slate-400 hover:text-rose-600 p-1">
              <span class="material-symbols-outlined text-[18px]">cancel</span>
            </button>
          </div>
        </div>

        <!-- Save / Cancel Action Buttons -->
        <div class="flex items-center gap-3 justify-end pt-2 border-t border-slate-100">
          <button
            @click="openFilePicker"
            class="px-4 py-2.5 bg-blue-50 text-[#005EA3] font-bold text-xs rounded-xl hover:bg-blue-100 transition mr-auto flex items-center gap-1.5"
          >
            <span class="material-symbols-outlined text-[18px]">folder</span>
            Chọn từ Bộ chọn tệp...
          </button>

          <button
            @click="showInlineUpload = false"
            class="px-5 py-2.5 border border-slate-300 text-slate-700 font-bold text-xs rounded-xl hover:bg-slate-50 transition"
          >
            Hủy
          </button>
          <button
            @click="saveSubmission"
            :disabled="pendingFiles.length === 0"
            class="px-6 py-2.5 bg-[#005EA3] hover:bg-blue-800 disabled:opacity-50 text-white font-bold text-xs rounded-xl shadow-xs transition active:scale-95 flex items-center gap-2"
          >
            <span class="material-symbols-outlined text-[18px]">save</span>
            Lưu những thay đổi
          </button>
        </div>
      </div>
    </div>

    <!-- MODAL 1: BỘ CHỌN TỆP (FILE PICKER MODAL) -->
    <div
      v-if="showFilePickerModal"
      class="fixed inset-0 z-[120] bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-4"
      @click.self="showFilePickerModal = false"
    >
      <div class="bg-white rounded-2xl shadow-2xl w-full max-w-4xl overflow-hidden font-sans border border-slate-200">
        <!-- Header -->
        <div class="flex justify-between items-center px-6 py-4 border-b border-slate-200 bg-slate-50">
          <h3 class="font-extrabold text-slate-900 text-base flex items-center gap-2">
            <span class="material-symbols-outlined text-[#005EA3]">folder_managed</span>
            Bộ chọn tệp (File Picker)
          </h3>
          <button @click="showFilePickerModal = false" class="text-slate-400 hover:text-slate-600 font-bold text-lg">✕</button>
        </div>

        <div class="flex h-[480px]">
          <!-- Sidebar -->
          <aside class="w-56 border-r border-slate-200 bg-slate-50 p-3 space-y-2 shrink-0">
            <button
              @click="pickerTab = 'upload'"
              class="w-full flex items-center gap-2.5 px-3 py-2 rounded-xl text-xs font-bold transition"
              :class="pickerTab === 'upload' ? 'bg-[#005EA3] text-white shadow-2xs' : 'text-slate-700 hover:bg-slate-200/60'"
            >
              <span class="material-symbols-outlined text-[18px]">upload_file</span>
              Tải lên file
            </button>
            <button
              @click="pickerTab = 'recent'"
              class="w-full flex items-center gap-2.5 px-3 py-2 rounded-xl text-xs font-bold transition"
              :class="pickerTab === 'recent' ? 'bg-[#005EA3] text-white shadow-2xs' : 'text-slate-700 hover:bg-slate-200/60'"
            >
              <span class="material-symbols-outlined text-[18px]">history</span>
              Tập tin gần đây
            </button>
            <button
              @click="pickerTab = 'private'"
              class="w-full flex items-center gap-2.5 px-3 py-2 rounded-xl text-xs font-bold transition"
              :class="pickerTab === 'private' ? 'bg-[#005EA3] text-white shadow-2xs' : 'text-slate-700 hover:bg-slate-200/60'"
            >
              <span class="material-symbols-outlined text-[18px]">folder_shared</span>
              Tập tin riêng tư
            </button>
          </aside>

          <!-- Main Picker Area -->
          <div class="flex-1 p-6 overflow-y-auto space-y-5 text-xs">
            <!-- Upload Tab -->
            <div v-if="pickerTab === 'upload'" class="space-y-4">
              <div>
                <label class="block font-bold text-slate-700 mb-1">Đính kèm tệp (*)</label>

                <div v-if="!pickerFile" class="flex items-center gap-3">
                  <button
                    type="button"
                    @click="triggerPickerFileInput"
                    class="px-4 py-2 bg-slate-100 border border-slate-300 hover:bg-slate-200 text-slate-800 font-bold rounded-xl transition"
                  >
                    Chọn file
                  </button>
                  <span class="text-slate-400">Chưa có file nào được chọn</span>
                  <input ref="pickerFileInputRef" type="file" class="hidden" @change="onPickerFileSelected" />
                </div>

                <!-- Selected File Info Card -->
                <div v-else class="flex items-center justify-between p-3.5 bg-blue-50/70 border border-blue-200 rounded-xl">
                  <div class="flex items-center gap-3">
                    <span class="material-symbols-outlined text-[#005EA3]">description</span>
                    <div>
                      <p class="font-bold text-slate-900 font-mono">{{ pickerFile.name }}</p>
                      <p class="text-[10px] text-slate-500">{{ (pickerFile.size / (1024*1024)).toFixed(2) }} MB</p>
                    </div>
                  </div>
                  <button @click="pickerFile = null" class="text-slate-400 hover:text-rose-600 font-bold text-base p-1">✕</button>
                </div>
              </div>

              <div>
                <label class="block font-bold text-slate-700 mb-1">Lưu thành</label>
                <input
                  v-model="pickerSaveAs"
                  type="text"
                  placeholder="BT_3.3.2026_Whitebox Testing_Phan Nhật Minh Anh.docx"
                  class="w-full px-3 py-2 border border-slate-300 rounded-xl outline-none focus:ring-2 focus:ring-blue-600"
                />
              </div>

              <div>
                <label class="block font-bold text-slate-700 mb-1">Tác giả</label>
                <input
                  v-model="pickerAuthor"
                  type="text"
                  class="w-full px-3 py-2 bg-slate-100 border border-slate-300 rounded-xl text-slate-600 outline-none"
                  readonly
                />
              </div>

              <div class="pt-4 flex justify-center">
                <button
                  @click="submitPickerFile"
                  class="px-8 py-2.5 bg-[#005EA3] hover:bg-blue-800 text-white font-bold rounded-xl shadow-md transition active:scale-95"
                >
                  Đăng tải tệp này
                </button>
              </div>
            </div>

            <!-- Recent Files Tab -->
            <div v-else-if="pickerTab === 'recent'" class="space-y-3">
              <p class="font-bold text-slate-800 mb-2">Tập tin đã sử dụng gần đây:</p>
              <div
                v-for="(rf, index) in recentFilesList"
                :key="index"
                @click="selectRecentFile(rf)"
                class="p-3 bg-slate-50 border border-slate-200 hover:border-[#005EA3] rounded-xl flex items-center justify-between cursor-pointer transition"
              >
                <div class="flex items-center gap-3">
                  <span class="material-symbols-outlined text-[#005EA3]">picture_as_pdf</span>
                  <div>
                    <p class="font-bold text-slate-900">{{ rf.name }}</p>
                    <p class="text-[10px] text-slate-400">{{ rf.size }} • {{ rf.date }}</p>
                  </div>
                </div>
                <span class="text-[#005EA3] font-bold">Chọn →</span>
              </div>
            </div>

            <!-- Private Files Tab -->
            <div v-else class="p-8 text-center text-slate-400 space-y-2">
              <span class="material-symbols-outlined text-[42px]">folder_off</span>
              <p class="font-bold text-slate-600">Thư mục riêng tư trống</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- MODAL 2: KẾT QUẢ ĐÁNH GIÁ BÀI NỘP - SPLIT VIEW (E12.2) -->
    <div
      v-if="showAssessmentModal"
      class="fixed inset-0 z-[140] bg-slate-900/80 backdrop-blur-xs flex items-center justify-center p-4 font-sans"
    >
      <div class="bg-white w-full max-w-6xl h-[90vh] rounded-2xl shadow-2xl overflow-hidden flex flex-col border border-slate-300">
        <!-- Top Modal Nav Bar -->
        <div class="h-14 bg-[#005EA3] text-white px-6 flex items-center justify-between shrink-0">
          <div class="flex items-center gap-3">
            <span class="material-symbols-outlined text-[24px]">verified</span>
            <div>
              <h3 class="font-bold text-sm">Kết quả đánh giá chi tiết: {{ taskData.title }}</h3>
              <p class="text-[10px] text-blue-100">Sinh viên: {{ user.hoTen || 'Sinh viên' }} • MSSV: {{ user.maDinhDanh || user.username || '---' }}</p>
            </div>
          </div>

          <button @click="showAssessmentModal = false" class="px-3 py-1 bg-white/20 hover:bg-white/30 rounded-lg text-xs font-bold transition">
            Đóng ✕
          </button>
        </div>

        <!-- Split Content Area -->
        <div class="flex-1 flex overflow-hidden">
          <!-- Left Column: PDF Report Viewer (60%) -->
          <section class="w-[60%] flex flex-col border-r border-slate-200 bg-slate-100">
            <!-- Toolbar -->
            <div class="h-10 bg-white border-b border-slate-200 flex items-center justify-between px-4 text-xs shrink-0">
              <div class="flex items-center gap-2 text-slate-600 font-bold">
                <button class="p-1 hover:bg-slate-100 rounded"><span class="material-symbols-outlined text-[18px]">zoom_out</span></button>
                <span>100%</span>
                <button class="p-1 hover:bg-slate-100 rounded"><span class="material-symbols-outlined text-[18px]">zoom_in</span></button>
                <span class="text-slate-300 mx-1">|</span>
                <span>Trang 1 / 12</span>
              </div>

              <button class="px-3 py-1 bg-slate-100 hover:bg-slate-200 border rounded-lg font-bold text-slate-700 text-[11px] flex items-center gap-1">
                <span class="material-symbols-outlined text-[14px]">download</span> Tải xuống PDF
              </button>
            </div>

            <!-- Scrollable Report Canvas -->
            <div class="flex-1 overflow-y-auto p-6 space-y-4">
              <div class="bg-white p-8 rounded-xl shadow-xs border border-slate-200 max-w-xl mx-auto space-y-6 text-xs text-slate-800 relative min-h-[600px]">
                <header class="border-b-2 border-slate-900 pb-4 flex justify-between items-start">
                  <div>
                    <h4 class="font-extrabold text-sm uppercase text-slate-900">Báo Cáo Thực Tập Tốt Nghiệp</h4>
                    <p class="text-[11px] text-slate-500 font-medium">Sinh viên: {{ user.hoTen || 'Sinh viên' }} - MSSV: {{ user.maDinhDanh || user.username || '---' }}</p>
                  </div>
                  <div class="text-right">
                    <span class="px-2 py-0.5 bg-blue-100 text-[#005EA3] font-black text-[10px] rounded">TUẦN 4</span>
                  </div>
                </header>

                <div class="space-y-4">
                  <section>
                    <h5 class="font-bold text-slate-900 uppercase text-[11px] mb-1">1. Nội dung công việc trong tuần</h5>
                    <p class="text-slate-600 leading-relaxed">
                      Trong tuần làm việc thứ 4 tại doanh nghiệp, em đã hoàn thành thiết kế RESTful API cho module Quản lý kho, tích hợp Docker container và tiến hành kiểm thử giao diện mobile...
                    </p>
                  </section>

                  <section>
                    <h5 class="font-bold text-slate-900 uppercase text-[11px] mb-1">2. Kết quả đạt được</h5>
                    <p class="text-slate-600 leading-relaxed bg-amber-50 p-2 rounded-lg border border-amber-200">
                      "Đã viết xong tài liệu mô tả API cho phân hệ Quản lý người dùng và thực hiện thành công các bài kiểm thử đơn vị (Unit Test) đạt tỷ lệ coverage trên 85%."
                    </p>
                  </section>
                </div>

                <!-- Watermark -->
                <div class="absolute bottom-6 right-6 opacity-20 pointer-events-none text-slate-400 font-black text-xl">
                  IMS INTERNAL ONLY
                </div>
              </div>
            </div>
          </section>

          <!-- Right Column: Feedback & Assessment (40%) -->
          <section class="w-[40%] flex flex-col bg-slate-50 overflow-y-auto p-6 space-y-6 text-xs">
            <!-- Submission Info -->
            <div class="bg-white p-4 rounded-xl border border-slate-200 space-y-2">
              <div class="flex justify-between items-center border-b pb-2">
                <span class="font-bold text-slate-900">Thông tin nộp bài</span>
                <span class="px-2.5 py-0.5 bg-emerald-100 text-emerald-800 font-bold rounded-full text-[10px]">Đã nộp</span>
              </div>
              <div class="grid grid-cols-2 gap-2 text-[11px] text-slate-600">
                <p>Hạn chót: <strong class="text-slate-900">26/08/2025</strong></p>
                <p>Nộp lúc: <strong class="text-slate-900">26/08/2025 08:45</strong></p>
              </div>
            </div>

            <!-- Overall Lecturer Feedback -->
            <div class="bg-white p-4 rounded-xl border border-slate-200 space-y-3">
              <h4 class="font-bold text-slate-900 text-xs flex items-center gap-1.5">
                <span class="material-symbols-outlined text-[#005EA3] text-[18px]">record_voice_over</span>
                Nhận xét tổng quát từ Giảng viên
              </h4>

              <div class="flex items-center gap-2.5 border-b pb-2">
                <div class="w-8 h-8 rounded-full bg-[#005EA3] text-white flex items-center justify-center font-bold text-xs">
                  NA
                </div>
                <div>
                  <p class="font-bold text-slate-900">TS. Nguyễn Văn A</p>
                  <p class="text-[10px] text-slate-400">Giảng viên Hướng dẫn</p>
                </div>
              </div>

              <p class="italic text-slate-700 bg-blue-50/60 p-3 rounded-xl border border-blue-100 leading-relaxed">
                "Bài báo cáo trình bày rất mạch lạc, giải pháp khắc phục lỗi responsive trên trang Dashboard có tính thực tiễn cao. Tiếp tục phát huy trong đợt giữa kỳ!"
              </p>
            </div>

            <!-- Score Rubric -->
            <div class="bg-white p-4 rounded-xl border border-slate-200 space-y-3">
              <div class="flex items-center justify-between">
                <h4 class="font-bold text-slate-900 text-xs">Điểm số & Tiêu chí đánh giá</h4>
                <div class="text-right">
                  <span class="text-2xl font-black text-[#005EA3]">8.5</span>
                  <span class="text-slate-400 font-bold"> / 10.0</span>
                </div>
              </div>

              <div class="space-y-2 text-[11px]">
                <div class="flex justify-between items-center p-2 bg-slate-50 rounded-lg">
                  <span>Tiêu chí 1: Tiến độ & Nội dung</span>
                  <strong class="text-slate-900">4.5 / 5.0</strong>
                </div>
                <div class="flex justify-between items-center p-2 bg-slate-50 rounded-lg">
                  <span>Tiêu chí 2: Trình bày & Minh chứng</span>
                  <strong class="text-slate-900">4.0 / 5.0</strong>
                </div>
              </div>
            </div>
          </section>
        </div>
      </div>
    </div>

    <!-- Toast -->
    <div
      v-if="toastMsg"
      class="fixed bottom-6 right-6 z-[150] bg-slate-900 text-white px-4 py-3 rounded-xl shadow-xl text-xs font-bold flex items-center gap-2"
    >
      <span class="material-symbols-outlined text-emerald-400 text-[18px]">check_circle</span>
      {{ toastMsg }}
    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue';

  const taskData = ref({
    title: 'Báo cáo thực tập tuần 4',
    deadline: 'Thứ ba, 26 tháng 8 2025, 9:00 AM'
  });

  const taskSubItems = ref([
    {
      id: 'sub_1',
      name: '1. Nội dung công việc & Tiến độ thực tập',
      description: 'Mô tả chi tiết các đầu việc đã triển khai trong tuần',
      maxPoints: 4.0,
      weightRatio: 40,
      minPass: 2.0,
      earnedPoints: 3.5
    },
    {
      id: 'sub_2',
      name: '2. Sản phẩm / Minh chứng kỹ thuật đính kèm',
      description: 'Sơ đồ kiến trúc, link source code hoặc hình ảnh minh chứng',
      maxPoints: 3.0,
      weightRatio: 30,
      minPass: 1.5,
      earnedPoints: 2.5
    },
    {
      id: 'sub_3',
      name: '3. Khó khăn, vướng mắc & Kế hoạch tuần kế tiếp',
      description: 'Phân tích vấn đề kỹ thuật phát sinh và đề xuất giải pháp tuần tới',
      maxPoints: 3.0,
      weightRatio: 30,
      minPass: 1.5,
      earnedPoints: 2.5
    }
  ]);

  const submissionState = ref('graded'); // 'unsubmitted' | 'submitted' | 'graded'
  const showInlineUpload = ref(false);
  const showFilePickerModal = ref(false);
  const showAssessmentModal = ref(false);
  const isDragging = ref(false);
  const fileInputRef = ref(null);
  const pickerFileInputRef = ref(null);
  const toastMsg = ref('');

  // File picker state
  const pickerTab = ref('upload');
  const pickerFile = ref(null);
  const pickerSaveAs = ref('BT_3.3.2026_Whitebox Testing_Phan Nhật Minh Anh.docx');
  const pickerAuthor = ref('Phan Nhật Minh Anh');

  const recentFilesList = ref([
    { name: 'Huong_dan_thuc_tap_K64.pdf', size: '1.8 MB', date: '20/08/2025' },
    { name: 'Nhat_ky_ky_thuat_T10.docx', size: '520 KB', date: '22/08/2025' }
  ]);

  const uploadedFiles = ref([
    { name: '03.PhanNhatMinhAnh.cs', size: '2.4 MB', uploadedAt: '26/08/2025 08:45:34' }
  ]);

  const pendingFiles = ref([]);

  function setSubmissionState(state) {
    submissionState.value = state;
    if (state === 'unsubmitted') {
      showInlineUpload.value = false;
    }
  }

  function triggerFileInput() {
    fileInputRef.value?.click();
  }

  function handleFileSelect(e) {
    const files = Array.from(e.target.files || []);
    files.forEach(f => {
      pendingFiles.value.push({ name: f.name, size: f.size });
    });
  }

  function handleFileDrop(e) {
    isDragging.value = false;
    const files = Array.from(e.dataTransfer.files || []);
    files.forEach(f => {
      pendingFiles.value.push({ name: f.name, size: f.size });
    });
  }

  function removePendingFile(index) {
    pendingFiles.value.splice(index, 1);
  }

  function openFilePicker() {
    showFilePickerModal.value = true;
  }

  function triggerPickerFileInput() {
    pickerFileInputRef.value?.click();
  }

  function onPickerFileSelected(e) {
    const file = e.target.files[0];
    if (file) {
      pickerFile.value = file;
      pickerSaveAs.value = file.name;
    }
  }

  function selectRecentFile(file) {
    uploadedFiles.value = [{
      name: file.name,
      size: file.size,
      uploadedAt: new Date().toLocaleString('vi-VN')
    }];
    submissionState.value = 'submitted';
    showFilePickerModal.value = false;
    triggerToast(`Đã chọn file: ${file.name}`);
  }

  function submitPickerFile() {
    const fileName = pickerSaveAs.value.trim() || pickerFile.value?.name || 'File_Bai_Nop.docx';
    uploadedFiles.value = [{
      name: fileName,
      size: pickerFile.value ? `${(pickerFile.value.size / 1024 / 1024).toFixed(1)} MB` : '1.6 MB',
      uploadedAt: new Date().toLocaleString('vi-VN')
    }];

    submissionState.value = 'submitted';
    showFilePickerModal.value = false;
    showInlineUpload.value = false;
    triggerToast('Đã đăng tải tệp thành công từ Bộ chọn tệp!');
  }

  function saveSubmission() {
    if (pendingFiles.value.length === 0) return;

    uploadedFiles.value = pendingFiles.value.map(pf => ({
      name: pf.name,
      size: `${(pf.size / 1024 / 1024).toFixed(1)} MB`,
      uploadedAt: new Date().toLocaleString('vi-VN')
    }));

    submissionState.value = 'submitted';
    showInlineUpload.value = false;
    pendingFiles.value = [];
    triggerToast('Đã lưu bài nộp thành công!');
  }

  function removeSubmission() {
    if (confirm('Bạn có chắc chắn muốn gỡ bài nộp này?')) {
      submissionState.value = 'unsubmitted';
      uploadedFiles.value = [];
      triggerToast('Đã gỡ bài nộp thành công!');
    }
  }

  function triggerToast(msg) {
    toastMsg.value = msg;
    setTimeout(() => {
      toastMsg.value = '';
    }, 3000);
  }
</script>

