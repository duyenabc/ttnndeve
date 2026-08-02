<template>
  <div class="space-y-4 font-sans relative">
    <div
      v-if="!isWriting"
      class="border border-slate-200 rounded-xl bg-white px-5 py-4 flex flex-col xl:flex-row xl:items-center justify-between gap-4"
    >
      <div class="flex flex-wrap items-center gap-3">
        <button
          type="button"
          @click="changeWeek(-1)"
          :disabled="currentWeek <= 1"
          class="flex items-center gap-1.5 border border-slate-300 text-slate-600 px-3 py-1.5 rounded-md hover:bg-slate-50 transition text-[13px] disabled:opacity-40"
        >
          <span>&larr;</span>
          Tuần trước
        </button>

        <div class="relative min-w-[220px]">
          <select
            v-model.number="currentWeek"
            class="w-full appearance-none pl-3 pr-8 py-1.5 bg-white border border-slate-300 rounded-md text-[13px] font-semibold text-slate-800 outline-none focus:ring-1 focus:ring-[#005EA3]"
          >
            <option v-for="w in maxWeek" :key="w" :value="w">
              Tuần {{ w }} ({{ getWeekDateRange(w) }})
            </option>
          </select>
          <span class="material-symbols-outlined absolute right-2 top-1/2 -translate-y-1/2 text-slate-400 text-[18px] pointer-events-none">
            expand_more
          </span>
        </div>

        <button
          type="button"
          @click="changeWeek(1)"
          :disabled="currentWeek >= maxWeek"
          class="flex items-center gap-1.5 border border-slate-300 text-slate-600 px-3 py-1.5 rounded-md hover:bg-slate-50 transition text-[13px] disabled:opacity-40"
        >
          Tuần sau
          <span>&rarr;</span>
        </button>
      </div>

      <div class="flex flex-col sm:flex-row sm:items-center gap-4 sm:gap-6">
        <div class="w-full sm:w-[260px]">
          <div class="flex justify-between items-center text-[12px] mb-1.5">
            <span class="text-slate-600 font-medium">Tiến độ tuần này</span>
            <span
              class="font-bold"
              :class="weekSubmittedCount >= config.minPerWeek ? 'text-emerald-700' : 'text-[#005EA3]'"
            >
              {{ weekSubmittedCount }}/{{ config.minPerWeek }} nhật ký đã nộp
            </span>
          </div>
          <div class="w-full bg-slate-200 rounded-full h-1.5 overflow-hidden">
            <div
              class="h-full transition-all duration-500 rounded-full"
              :class="weekSubmittedCount >= config.minPerWeek ? 'bg-emerald-500' : 'bg-[#005EA3]'"
              :style="{ width: weekProgressPercent + '%' }"
            ></div>
          </div>
        </div>

        <div class="flex flex-col items-stretch sm:items-end gap-1.5 shrink-0">
          <button
            type="button"
            @click="startWriting"
            :disabled="!canWrite"
            class="bg-[#005EA3] hover:bg-[#003362] text-white px-4 py-2.5 rounded-md transition text-[14px] font-medium flex items-center gap-1 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <span>+</span>
            Viết nhật ký
          </button>
          <p v-if="isCurrentWeekOverdue" class="text-[12px] text-rose-600 font-medium">
            Tuần này đã quá hạn — không thể ghi nhật ký
          </p>
          <p v-else-if="!config.isEnabled" class="text-[12px] text-slate-500 font-medium">
            Nhật ký đang bị tắt bởi giảng viên
          </p>
        </div>
      </div>
    </div>

    <div v-if="isLoading" class="text-center py-8">
      <span class="material-symbols-outlined animate-spin text-3xl text-[#005EA3]">refresh</span>
      <p class="text-sm text-slate-500 mt-2">Đang tải dữ liệu nhật ký...</p>
    </div>

    <template v-else>
      <div v-if="isWriting" class="bg-white rounded-xl p-6 sm:p-8 border border-slate-200 pb-24">
        <div class="mb-7">
          <h2 class="font-bold text-[22px] sm:text-[24px] text-slate-900 mb-1.5">
            Viết nhật ký tuần hiện tại
          </h2>
          <p class="text-[13px] text-slate-500">
            Tuần thực tập: Ngày {{ todayLabel }} — Tuần {{ currentWeek }} ({{ getWeekDateRange(currentWeek) }})
          </p>
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-2 gap-6 mb-8">
          <div v-if="activeFields.find((f) => f.id === 'completionLevel')">
            <label class="block text-[13px] font-bold text-slate-800 mb-2">Mức độ hoàn thành</label>
            <div class="flex items-center gap-0.5">
              <button
                v-for="i in 10"
                :key="'c' + i"
                type="button"
                class="material-symbols-outlined text-[26px] select-none transition leading-none"
                :class="form.completionLevel >= i ? 'text-amber-400' : 'text-slate-300'"
                :style="form.completionLevel >= i ? { fontVariationSettings: `'FILL' 1` } : {}"
                @click="form.completionLevel = i"
              >
                star
              </button>
            </div>
          </div>
          <div v-if="activeFields.find((f) => f.id === 'feeling')">
            <label class="block text-[13px] font-bold text-slate-800 mb-2">Cảm nhận chung</label>
            <div class="flex items-center gap-0.5">
              <button
                v-for="i in 5"
                :key="'f' + i"
                type="button"
                class="material-symbols-outlined text-[26px] select-none transition leading-none"
                :class="form.feeling >= i ? 'text-amber-400' : 'text-slate-300'"
                :style="form.feeling >= i ? { fontVariationSettings: `'FILL' 1` } : {}"
                @click="form.feeling = i"
              >
                star
              </button>
            </div>
          </div>
        </div>

        <div class="space-y-5">
          <template v-for="field in activeFields" :key="field.id">
            <div v-if="!['completionLevel', 'feeling'].includes(field.id)">
              <label class="block text-[13px] font-bold text-slate-800 mb-2">
                {{ field.label }}
                <span v-if="field.isRequired" class="text-slate-500">(*)</span>
              </label>

              <textarea
                v-if="textAreaFields.includes(field.id)"
                v-model="form[field.id]"
                rows="4"
                class="w-full rounded-lg px-4 py-3 text-[13px] outline-none bg-white placeholder:text-slate-400 text-slate-700 transition border"
                :class="
                  fieldErrors[field.id]
                    ? 'border-red-500 focus:ring-2 focus:ring-red-100'
                    : 'border-slate-300 focus:border-[#005EA3] focus:ring-2 focus:ring-blue-100'
                "
                :placeholder="placeholderFor(field.id, field.label)"
                @input="clearFieldError(field.id)"
              ></textarea>
              <div v-else-if="field.id === 'proofFile'" class="space-y-2">
                <label
                  class="flex flex-col sm:flex-row sm:items-center gap-3 w-full rounded-lg border border-dashed border-slate-300 bg-slate-50 px-4 py-4 cursor-pointer hover:border-[#005EA3] hover:bg-blue-50/40 transition"
                >
                  <span class="inline-flex h-10 w-10 items-center justify-center rounded-full bg-white border border-slate-200 text-[#005EA3] shrink-0">
                    <span class="material-symbols-outlined text-[22px]">upload_file</span>
                  </span>
                  <span class="min-w-0 flex-1">
                    <span class="block text-[13px] font-semibold text-slate-800">
                      {{ form.proofFileName || 'Chọn file minh chứng' }}
                    </span>
                    <span class="block text-[12px] text-slate-500 mt-0.5">
                      PDF, ảnh, Word… · tối đa 100MB
                    </span>
                  </span>
                  <input
                    type="file"
                    class="sr-only"
                    accept=".pdf,.png,.jpg,.jpeg,.doc,.docx,.xls,.xlsx,.zip"
                    @change="onProofFileChange"
                  />
                </label>
                <button
                  v-if="form.proofFileName"
                  type="button"
                  class="text-[12px] text-rose-600 font-medium hover:underline"
                  @click="clearProofFile"
                >
                  Xóa file đã chọn
                </button>
              </div>
              <input
                v-else
                v-model="form[field.id]"
                type="text"
                class="w-full rounded-lg px-4 py-3 text-[13px] outline-none bg-white text-slate-700 transition border"
                :class="
                  fieldErrors[field.id]
                    ? 'border-red-500 focus:ring-2 focus:ring-red-100'
                    : 'border-slate-300 focus:border-[#005EA3] focus:ring-2 focus:ring-blue-100'
                "
                @input="clearFieldError(field.id)"
              />
              <p
                v-if="fieldErrors[field.id]"
                class="mt-1.5 text-[12px] text-red-600 flex items-center gap-1 font-medium"
              >
                <span class="material-symbols-outlined text-[14px]">warning</span>
                {{ fieldErrors[field.id] }}
              </p>
            </div>
          </template>
        </div>

        <div
          class="fixed bottom-0 left-0 right-0 lg:left-64 bg-white border-t border-slate-200 p-4 px-6 flex justify-end gap-3 z-30 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)]"
        >
          <button
            type="button"
            class="px-5 py-2.5 text-slate-600 font-semibold text-[13px] hover:text-slate-900 transition"
            @click="cancelWriting"
          >
            Hủy bỏ
          </button>
          <button
            type="button"
            class="px-5 py-2.5 border border-slate-300 text-slate-700 font-semibold rounded-md text-[13px] hover:bg-slate-50 transition"
            @click="saveDraft"
          >
            Lưu nháp
          </button>
          <button
            type="button"
            class="px-5 py-2.5 bg-[#005EA3] text-white font-semibold rounded-md text-[13px] hover:bg-[#003362] transition flex items-center gap-2"
            @click="submitDiary"
          >
            <span class="material-symbols-outlined text-[18px]">send</span>
            Nộp nhật ký
          </button>
        </div>
      </div>

      <div v-else class="space-y-3">
        <template v-if="currentWeekDiaries.length > 0">
          <div
            v-for="diary in currentWeekDiaries"
            :key="diary.id"
            class="border border-slate-200 bg-white rounded-xl p-5 flex flex-col md:flex-row gap-5"
          >
            <div class="md:w-[180px] shrink-0 flex flex-col items-start md:border-r md:border-slate-100 md:pr-4 gap-2">
              <div class="font-bold text-[14px] text-slate-900">
                {{ formatCardDate(diary.ngayCapNhat || diary.ngayTao) }}
              </div>

              <div class="flex flex-wrap gap-1.5">
                <span
                  v-if="diary.status === 'Submitted'"
                  class="inline-block bg-[#c6f6d5] text-[#166534] px-2.5 py-1 rounded-full text-[11px] font-bold uppercase"
                >
                  Đã nộp
                </span>
                <span
                  v-else
                  class="inline-block bg-slate-200 text-slate-600 px-2.5 py-1 rounded-full text-[11px] font-bold uppercase"
                >
                  Nháp
                </span>
                <span
                  v-if="diary.feedbacks?.length && !diary.isReadByStudent"
                  class="inline-block bg-[#ffe4d6] text-[#a43d00] px-2.5 py-1 rounded-full text-[11px] font-bold uppercase"
                >
                  Có phản hồi mới
                </span>
              </div>

              <div class="text-[11px] text-slate-400">
                Cập nhật: {{ formatUpdateTime(diary.ngayCapNhat || diary.ngayTao) }}
              </div>
            </div>

            <div class="flex-1 flex flex-col sm:flex-row sm:items-center justify-between gap-4 min-w-0">
              <div class="flex-1 min-w-0">
                <p
                  class="text-[13px] text-slate-800 leading-relaxed mb-3"
                  :class="{ 'italic text-slate-400': diary.status === 'Draft' && !diary.taskDescription }"
                >
                  {{
                    diary.status === 'Draft' && !diary.taskDescription
                      ? 'Chưa có nội dung trích đoạn nhật ký cho ngày hôm nay. Hãy cập nhật tiến độ công việc của bạn...'
                      : getPreviewText(diary)
                  }}
                </p>

                <div class="flex flex-wrap items-center gap-x-3 gap-y-1 text-[12px] text-slate-500 mb-2">
                  <span>
                    Mức độ hoàn thành:
                    <span class="font-bold text-slate-800">{{ diary.completionLevel || '--' }}/10</span>
                  </span>
                  <span class="text-slate-300 hidden sm:inline">|</span>
                  <span>
                    Cảm nhận:
                    <span class="font-bold text-slate-800">{{ diary.feeling || '--' }}/5</span>
                  </span>
                </div>

                <div
                  v-if="diary.feedbacks?.length"
                  class="mt-2 bg-[#fff5f2] border border-[#ffdbcd] rounded-lg px-3 py-2.5 text-[#a43d00] text-[13px] flex items-start gap-2"
                >
                  <span class="material-symbols-outlined text-[16px] mt-0.5 shrink-0">chat_bubble</span>
                  <span class="italic leading-relaxed">
                    "{{ diary.feedbacks[diary.feedbacks.length - 1].content }}"
                  </span>
                </div>
              </div>

              <button
                type="button"
                class="text-[#005EA3] font-semibold text-[13px] flex items-center gap-1 hover:underline shrink-0 self-start sm:self-center"
                @click="openDrawer(diary)"
              >
                <template v-if="diary.status === 'Draft'">
                  <span class="material-symbols-outlined text-[18px]">edit_note</span>
                  Tiếp tục viết
                </template>
                <template v-else>
                  Xem chi tiết <span>&rarr;</span>
                </template>
              </button>
            </div>
          </div>
        </template>
        <template v-else>
          <div class="border border-slate-200 bg-white rounded-xl p-10 text-center text-slate-500">
            <span class="material-symbols-outlined text-4xl mb-2 text-slate-300">menu_book</span>
            <p class="text-sm">Chưa có nhật ký nào trong tuần này.</p>
          </div>
        </template>

        <div class="mt-8 border border-slate-200 rounded-xl bg-white overflow-hidden">
          <div class="px-5 py-3.5 border-b border-slate-100 flex items-center justify-between">
            <h3 class="text-[15px] font-bold text-slate-900 flex items-center gap-2">
              <span class="material-symbols-outlined text-[#005EA3] text-[20px]">history_edu</span>
              Lịch sử đánh giá
            </h3>
            <span class="text-[12px] text-slate-500">{{ evaluationHistory.length }} nhận xét</span>
          </div>
          <div v-if="evaluationHistory.length === 0" class="px-5 py-8 text-center text-[13px] text-slate-500">
            Chưa có nhận xét từ giảng viên.
          </div>
          <ul v-else class="divide-y divide-slate-100">
            <li
              v-for="item in evaluationHistory"
              :key="item.id"
              class="px-5 py-3.5 hover:bg-slate-50/80"
            >
              <div class="flex flex-wrap items-center justify-between gap-2 mb-1">
                <span class="text-[13px] font-bold text-slate-800">{{ item.teacherName || 'GVHD' }}</span>
                <span class="text-[11px] text-slate-500">
                  Tuần {{ item.week }} · {{ formatFeedbackDate(item.timestamp) }}
                </span>
              </div>
              <p class="text-[13px] text-slate-700 leading-relaxed whitespace-pre-wrap">{{ item.content }}</p>
            </li>
          </ul>
        </div>

        <div class="mt-6 pt-5 border-t border-slate-200 flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div class="text-[13px] font-semibold text-slate-800">
            Tổng cả kỳ:
            <span class="text-[#005EA3]">{{ totalSubmitted }}/{{ totalRequired }} nhật ký đã nộp</span>
          </div>
          <div class="w-full sm:w-[240px]">
            <div class="flex justify-between items-center text-[12px] mb-1">
              <span class="text-slate-500 font-medium">Tiến độ hoàn thành</span>
              <span class="text-[#005EA3] font-bold">
                {{ Math.round((totalSubmitted / Math.max(1, totalRequired)) * 100) }}%
              </span>
            </div>
            <div class="w-full bg-slate-200 rounded-full h-1.5 overflow-hidden">
              <div
                class="h-full rounded-full bg-[#005EA3] transition-all"
                :style="{
                  width: Math.min(100, Math.round((totalSubmitted / Math.max(1, totalRequired)) * 100)) + '%',
                }"
              ></div>
            </div>
          </div>
        </div>
      </div>
    </template>

    <div
      v-if="isDrawerOpen"
      class="fixed inset-0 z-[100] bg-slate-900/50 backdrop-blur-sm flex justify-end"
      @click.self="closeDrawer"
    >
      <div class="bg-white w-full max-w-[500px] h-full shadow-2xl flex flex-col border-l border-slate-200">
        <div class="p-6 border-b border-slate-200 flex items-start justify-between">
          <div class="space-y-1">
            <h3 class="text-xl font-bold text-slate-900">Chi tiết Nhật ký thực tập</h3>
            <span class="text-sm font-semibold text-[#005EA3]">
              Ngày {{ formatDrawerDate(viewingDiary) }} — Tuần {{ viewingDiary.week }}
            </span>
          </div>
          <button
            type="button"
            class="w-8 h-8 rounded-lg hover:bg-slate-100 flex items-center justify-center"
            @click="closeDrawer"
          >
            <span class="material-symbols-outlined text-[18px]">close</span>
          </button>
        </div>

        <div class="p-6 overflow-y-auto flex-1 space-y-5">
          <div
            v-if="viewingDiary.completionLevel || viewingDiary.feeling"
            class="bg-slate-50 rounded-lg py-4 flex items-center justify-between"
          >
            <div
              v-if="viewingDiary.completionLevel"
              class="flex-1 flex flex-col items-center"
              :class="{ 'border-r border-slate-200': viewingDiary.feeling }"
            >
              <span class="text-[11px] text-slate-500 font-bold uppercase mb-1">Mức độ hoàn thành</span>
              <span class="text-xl font-bold text-[#005EA3]">{{ viewingDiary.completionLevel }}/10</span>
            </div>
            <div v-if="viewingDiary.feeling" class="flex-1 flex flex-col items-center">
              <span class="text-[11px] text-slate-500 font-bold uppercase mb-1">Cảm nhận chung</span>
              <span class="text-xl font-bold text-[#005EA3]">{{ viewingDiary.feeling }}/5</span>
            </div>
          </div>

          <div
            v-for="field in activeFields"
            :key="field.id"
            class="space-y-1"
          >
            <template v-if="!['completionLevel', 'feeling'].includes(field.id)">
              <h4 class="font-bold text-[12px] text-slate-500">{{ field.label }}</h4>

              <div v-if="field.id === 'proofFile'" class="text-[14px] text-slate-800">
                <template v-if="viewingEvidence.fileName">
                  <a
                    v-if="viewingEvidence.dataUrl"
                    :href="viewingEvidence.dataUrl"
                    :download="viewingEvidence.fileName"
                    class="inline-flex items-center gap-2 text-[#005EA3] font-semibold hover:underline break-all"
                  >
                    <span class="material-symbols-outlined text-[18px] shrink-0">attach_file</span>
                    {{ viewingEvidence.fileName }}
                  </a>
                  <span v-else class="inline-flex items-center gap-2 font-medium break-all">
                    <span class="material-symbols-outlined text-[18px] text-slate-500 shrink-0">attach_file</span>
                    {{ viewingEvidence.fileName }}
                    <span
                      v-if="viewingEvidence.size"
                      class="text-[12px] text-slate-400 font-normal shrink-0"
                    >
                      ({{ formatFileSize(viewingEvidence.size) }})
                    </span>
                  </span>
                </template>
                <span v-else class="text-slate-400">---</span>
              </div>
              <p
                v-else-if="field.id === 'proofDescription'"
                class="text-[14px] text-slate-800 whitespace-pre-wrap"
              >
                {{ viewingEvidence.description || '---' }}
              </p>
              <p v-else class="text-[14px] text-slate-800 whitespace-pre-wrap">
                {{ viewingDiary[field.id] || '---' }}
              </p>
            </template>
          </div>

          <div v-if="viewingDiary.feedbacks?.length" class="space-y-3 pt-2">
            <div
              v-for="(fb, index) in viewingDiary.feedbacks"
              :key="index"
              class="bg-[#fff5f2] border border-[#ffdbcd] p-4 rounded-lg space-y-1.5"
            >
              <div class="flex items-center gap-2">
                <span class="material-symbols-outlined text-[14px] text-[#a43d00]">chat_bubble</span>
                <span class="text-[12px] font-bold text-[#a43d00]">
                  {{ fb.teacherName }} · {{ formatFeedbackDate(fb.timestamp) }}
                </span>
              </div>
              <p class="text-[13px] text-[#a43d00] italic leading-relaxed">"{{ fb.content }}"</p>
            </div>
          </div>
        </div>

        <div class="p-5 border-t border-slate-200 flex justify-end">
          <button
            type="button"
            class="px-5 py-2 border border-slate-300 text-slate-700 font-semibold rounded-md hover:bg-slate-50 text-[13px]"
            @click="closeDrawer"
          >
            Đóng
          </button>
        </div>
      </div>
    </div>

    <div
      v-if="showToast"
      class="fixed top-24 right-8 z-[200] flex items-center px-5 py-3 bg-white border border-slate-200 shadow-lg rounded-lg"
      role="alert"
    >
      <span class="material-symbols-outlined text-[24px] text-emerald-600">check_circle</span>
      <div class="ml-3 text-sm font-semibold text-slate-800">{{ toastMessage }}</div>
    </div>

    <div
      v-if="showConfirmSubmit"
      class="fixed inset-0 z-[200] bg-slate-900/40 backdrop-blur-sm flex items-center justify-center"
      @click.self="showConfirmSubmit = false"
    >
      <div class="bg-white rounded-xl shadow-2xl p-8 w-[400px] flex flex-col items-center text-center">
        <span class="material-symbols-outlined text-[48px] text-[#005EA3] mb-3">send</span>
        <h3 class="text-lg font-bold text-slate-900 mb-2">Xác nhận nộp nhật ký?</h3>
        <p class="text-sm text-slate-600 mb-6">Sau khi nộp, bạn sẽ không thể chỉnh sửa</p>
        <div class="flex gap-3 w-full">
          <button
            type="button"
            class="flex-1 py-2.5 border border-slate-300 rounded-md text-sm font-bold text-slate-800 hover:bg-slate-50"
            @click="showConfirmSubmit = false"
          >
            Hủy
          </button>
          <button
            type="button"
            class="flex-1 py-2.5 bg-[#005EA3] rounded-md text-sm font-bold text-white hover:bg-[#003362]"
            @click="confirmSubmit"
          >
            Xác nhận nộp
          </button>
        </div>
      </div>
    </div>

    <div
      v-if="showConfirmCancel"
      class="fixed inset-0 z-[200] bg-slate-900/40 backdrop-blur-sm flex items-center justify-center"
      @click.self="showConfirmCancel = false"
    >
      <div class="bg-white rounded-xl shadow-2xl p-8 w-[400px]">
        <h3 class="text-lg font-bold text-slate-900 mb-2">Hủy bỏ soạn thảo</h3>
        <p class="text-sm text-slate-700 mb-6 leading-relaxed">
          Mọi thông tin bạn đã nhập sẽ bị mất nếu không lưu nháp. Bạn có chắc chắn muốn hủy?
        </p>
        <div class="flex flex-col gap-2">
          <button
            type="button"
            class="w-full py-2.5 bg-slate-200 rounded-md text-sm font-bold text-slate-800 hover:bg-slate-300"
            @click="showConfirmCancel = false"
          >
            Tiếp tục viết
          </button>
          <button
            type="button"
            class="w-full py-2.5 bg-red-700 rounded-md text-sm font-bold text-white hover:bg-red-800"
            @click="confirmCancel"
          >
            Hủy bỏ
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';
import api from '@/api/api';
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();
const props = defineProps({
  classId: { type: String, required: true },
});
const emit = defineEmits(['writing-change']);

const textAreaFields = [
  'taskDescription',
  'newKnowledge',
  'issues',
  'solutions',
  'nextPlan',
  'supportNeeded',
  'feedback',
  'proofDescription',
];

const isLoading = ref(true);
const currentWeek = ref(2);
const maxWeek = ref(15);
/**
 * Monday of internship week 1.
 * Default: today falls in week 2 (week 1 = previous Mon–Sun).
 * Override via diary-config.internshipStart when available.
 */
const internshipStart = ref(addDays(alignToMonday(new Date()), -7));
const diaries = ref([]);
const evaluationHistory = ref([]);
const config = ref({
  isEnabled: true,
  minPerWeek: 2,
  deadlineDay: 0,
  deadlineDayName: 'Chủ nhật',
  deadlineTime: '23:59',
  fields: [],
});

const defaultFields = [
  { id: 'completionLevel', label: 'Mức độ hoàn thành', isEnabled: true, isRequired: true },
  { id: 'feeling', label: 'Cảm nhận chung', isEnabled: true, isRequired: true },
  { id: 'taskDescription', label: 'Mô tả nhiệm vụ đã thực hiện', isEnabled: true, isRequired: true },
  { id: 'newKnowledge', label: 'Kiến thức/kỹ năng mới học được', isEnabled: true, isRequired: true },
  { id: 'issues', label: 'Khó khăn/vướng mắc', isEnabled: true, isRequired: false },
  { id: 'solutions', label: 'Cách xử lý/hướng giải quyết', isEnabled: true, isRequired: false },
  { id: 'nextPlan', label: 'Kế hoạch làm việc tiếp theo', isEnabled: true, isRequired: true },
  { id: 'proofFile', label: 'Minh chứng công việc (file, tối đa 100MB)', isEnabled: true, isRequired: false },
  { id: 'proofDescription', label: 'Mô tả minh chứng', isEnabled: true, isRequired: false },
];

const MAX_PROOF_BYTES = 100 * 1024 * 1024;

const isWriting = ref(false);
const form = ref({});
const fieldErrors = ref({});
const isDrawerOpen = ref(false);
const viewingDiary = ref({});
const showConfirmSubmit = ref(false);
const showConfirmCancel = ref(false);
const showToast = ref(false);
const toastMessage = ref('');

watch(isWriting, (v) => emit('writing-change', v));

onMounted(async () => {
  await loadConfig();
  await loadDiaries();
  await loadEvaluationHistory();
  // Keep progress on the internship "current" week (default calendar → tuần 2)
  currentWeek.value = weekNumberForDate(new Date());
  isLoading.value = false;
});

const loadConfig = async () => {
  try {
    const res = await api.get(`/giangvien/classes/${props.classId}/diary-config`);
    if (res.data) {
      config.value = {
        isEnabled: res.data.isEnabled !== false,
        minPerWeek: res.data.minPerWeek || 2,
        deadlineDay: res.data.deadlineDay ?? 0,
        deadlineDayName: getDayName(res.data.deadlineDay),
        deadlineTime: res.data.deadlineTime || '23:59',
        fields: res.data.fields?.length ? res.data.fields : defaultFields,
      };
      if (res.data.internshipStart) {
        internshipStart.value = alignToMonday(new Date(res.data.internshipStart));
      }
    } else {
      config.value.fields = defaultFields;
    }
  } catch {
    config.value.fields = defaultFields;
  }
};

const loadDiaries = async () => {
  try {
    const uid = authStore.user?.maNguoiDung || authStore.user?.id;
    const res = await api.get('/diaries', {
      params: { classId: props.classId, userId: uid },
    });
    diaries.value = res.data || [];
  } catch (e) {
    console.error('Error loading diaries', e);
  }
};

const loadEvaluationHistory = async () => {
  try {
    const uid = authStore.user?.maNguoiDung || authStore.user?.id;
    if (!uid) return;
    const res = await api.get('/diaries/feedback-history', {
      params: { userId: uid, classId: props.classId },
    });
    evaluationHistory.value = Array.isArray(res.data) ? res.data : [];
  } catch (e) {
    console.error('Error loading evaluation history', e);
    evaluationHistory.value = [];
  }
};

const getDayName = (day) => {
  const map = { 1: 'Thứ 2', 2: 'Thứ 3', 3: 'Thứ 4', 4: 'Thứ 5', 5: 'Thứ 6', 6: 'Thứ 7', 0: 'Chủ nhật' };
  return map[day] || 'Chủ nhật';
};

const activeFields = computed(() => config.value.fields.filter((f) => f.isEnabled));

const currentWeekDiaries = computed(() =>
  diaries.value
    .filter((d) => Number(d.week) === currentWeek.value)
    .sort((a, b) => new Date(b.ngayCapNhat || b.ngayTao) - new Date(a.ngayCapNhat || a.ngayTao))
);

const weekSubmittedCount = computed(
  () => currentWeekDiaries.value.filter((d) => d.status === 'Submitted').length
);

const weekProgressPercent = computed(() => {
  if (!config.value.minPerWeek) return 0;
  return Math.min(100, Math.round((weekSubmittedCount.value / config.value.minPerWeek) * 100));
});

const totalSubmitted = computed(() => diaries.value.filter((d) => d.status === 'Submitted').length);
const totalRequired = computed(() => config.value.minPerWeek * maxWeek.value);

const isCurrentWeekOverdue = computed(() => isWeekOverdue(currentWeek.value));
const canWrite = computed(() => config.value.isEnabled && !isCurrentWeekOverdue.value);

const todayLabel = computed(() => {
  const d = new Date();
  return d.toLocaleDateString('vi-VN', { day: 'numeric', month: 'numeric', year: 'numeric' });
});

function startOfDay(date) {
  const d = new Date(date);
  d.setHours(0, 0, 0, 0);
  return d;
}

function addDays(date, days) {
  const d = new Date(date);
  d.setDate(d.getDate() + days);
  return d;
}

function alignToMonday(date) {
  const d = startOfDay(date);
  const day = d.getDay();
  const diff = day === 0 ? -6 : 1 - day;
  return addDays(d, diff);
}

function pad2(n) {
  return String(n).padStart(2, '0');
}

function formatDM(date) {
  return `${pad2(date.getDate())}/${pad2(date.getMonth() + 1)}`;
}

function formatDMY(date) {
  return `${formatDM(date)}/${date.getFullYear()}`;
}

function getWeekBounds(week) {
  const start = addDays(internshipStart.value, (Number(week) - 1) * 7);
  const end = addDays(start, 6);
  return { start, end };
}

function getWeekDeadline(week) {
  const { start } = getWeekBounds(week);
  const targetDow = Number(config.value.deadlineDay ?? 0);
  let deadlineDate = addDays(start, 6);
  for (let i = 0; i < 7; i++) {
    const d = addDays(start, i);
    if (d.getDay() === targetDow) {
      deadlineDate = d;
      break;
    }
  }
  const [hh, mm] = String(config.value.deadlineTime || '23:59').split(':');
  deadlineDate.setHours(Number(hh) || 23, Number(mm) || 59, 59, 999);
  return deadlineDate;
}

function isWeekOverdue(week) {
  return Date.now() > getWeekDeadline(week).getTime();
}

function weekNumberForDate(date) {
  const start = internshipStart.value;
  const diffDays = Math.floor((startOfDay(date) - start) / 86400000);
  const week = Math.floor(diffDays / 7) + 1;
  return Math.max(1, Math.min(maxWeek.value, week));
}

function placeholderFor(id, label) {
  const map = {
    taskDescription: 'Ghi rõ các đầu việc cụ thể bạn đã xử lý trong tuần này...',
    newKnowledge: 'Những công nghệ, quy trình hoặc kỹ năng mềm bạn đã tích lũy...',
    issues: 'Nêu rõ những khó khăn bạn đang gặp phải...',
    solutions: 'Bạn đã hoặc dự định giải quyết những khó khăn đó như thế nào...',
    nextPlan: 'Dự kiến công việc cho tuần kế tiếp...',
  };
  return map[id] || `Nhập ${label.toLowerCase()}...`;
}

function clearFieldError(id) {
  if (fieldErrors.value[id]) {
    const next = { ...fieldErrors.value };
    delete next[id];
    fieldErrors.value = next;
  }
}

function validateRequiredFields() {
  const errors = {};
  for (const field of activeFields.value) {
    if (!field.isRequired || ['completionLevel', 'feeling'].includes(field.id)) continue;
    const val = form.value[field.id];
    if (!val || !String(val).trim()) {
      errors[field.id] = `Vui lòng nhập ${field.label}`;
    }
  }
  fieldErrors.value = errors;
  return Object.keys(errors).length === 0;
}

const changeWeek = (delta) => {
  const newWeek = currentWeek.value + delta;
  if (newWeek > 0 && newWeek <= maxWeek.value) currentWeek.value = newWeek;
};

const startWriting = () => {
  if (!canWrite.value) {
    displayToast(
      isCurrentWeekOverdue.value
        ? 'Tuần này đã quá hạn — không thể ghi nhật ký'
        : 'Chức năng nhật ký đang tắt'
    );
    return;
  }
  isWriting.value = true;
  form.value = {
    completionLevel: 0,
    feeling: 0,
    proofFileName: '',
    evidence: '',
  };
  fieldErrors.value = {};
};

function clearProofFile() {
  form.value.proofFileName = '';
  form.value.evidence = '';
  clearFieldError('proofFile');
}

async function onProofFileChange(event) {
  const file = event.target.files?.[0];
  event.target.value = '';
  if (!file) return;

  if (file.size > MAX_PROOF_BYTES) {
    displayToast('File vượt quá 100MB. Vui lòng chọn file nhỏ hơn.');
    return;
  }

  form.value.proofFileName = file.name;
  clearFieldError('proofFile');

  // Store metadata (+ small files as data URL) in Evidence string for API
  const meta = { fileName: file.name, size: file.size, type: file.type || 'application/octet-stream' };
  if (file.size <= 2 * 1024 * 1024) {
    const dataUrl = await new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => resolve(reader.result);
      reader.onerror = reject;
      reader.readAsDataURL(file);
    });
    form.value.evidence = JSON.stringify({ ...meta, dataUrl });
  } else {
    form.value.evidence = JSON.stringify(meta);
  }
}

const cancelWriting = () => {
  showConfirmCancel.value = true;
};

const confirmCancel = () => {
  showConfirmCancel.value = false;
  isWriting.value = false;
  form.value = {};
  fieldErrors.value = {};
};

const submitDiary = () => {
  if (!validateRequiredFields()) return;
  showConfirmSubmit.value = true;
};

const confirmSubmit = async () => {
  showConfirmSubmit.value = false;
  await saveDiary('Submitted');
};

const saveDraft = async () => {
  fieldErrors.value = {};
  await saveDiary('Draft');
};

const saveDiary = async (status) => {
  if (isWeekOverdue(currentWeek.value)) {
    displayToast('Tuần này đã quá hạn — không thể ghi nhật ký');
    return;
  }
  try {
    const {
      proofFileName,
      proofFile,
      feedbacks,
      feedback,
      proofDescription,
      ...fields
    } = form.value;

    const payload = {
      id: fields.id || undefined,
      classId: props.classId,
      userId: authStore.user?.maNguoiDung || authStore.user?.id,
      week: String(currentWeek.value),
      status,
      completionLevel: fields.completionLevel ?? null,
      feeling: fields.feeling ?? null,
      taskDescription: fields.taskDescription || null,
      newKnowledge: fields.newKnowledge || null,
      issues: fields.issues || null,
      solutions: fields.solutions || null,
      nextPlan: fields.nextPlan || null,
      supportNeeded: fields.supportNeeded || feedback || null,
      evidence: fields.evidence || null,
    };

    // Attach proof description into evidence JSON when present
    if (proofDescription) {
      try {
        const ev = payload.evidence ? JSON.parse(payload.evidence) : {};
        payload.evidence = JSON.stringify({ ...ev, description: proofDescription });
      } catch {
        payload.evidence = JSON.stringify({
          fileName: proofFileName || null,
          description: proofDescription,
        });
      }
    }

    if (payload.id) {
      await api.put(`/diaries/${payload.id}`, payload);
    } else {
      await api.post('/diaries', payload);
    }

    displayToast(status === 'Submitted' ? 'Đã nộp nhật ký thành công' : 'Đã lưu nháp nhật ký thành công');
    isWriting.value = false;
    await loadDiaries();
  } catch (e) {
    console.error('Save diary error:', e);
    alert('Lưu nhật ký thất bại, vui lòng thử lại sau!');
  }
};

const getPreviewText = (diary) => {
  if (diary.taskDescription) return diary.taskDescription;
  const anyKey = Object.keys(diary).find(
    (k) =>
      !['id', 'week', 'status', 'ngayTao', 'ngayCapNhat', 'userId', 'classId', 'feedbacks', 'isReadByStudent'].includes(k)
  );
  return diary[anyKey] || 'Nội dung nhật ký...';
};

const getWeekDateRange = (week) => {
  const { start, end } = getWeekBounds(week);
  if (start.getFullYear() === end.getFullYear()) {
    return `${formatDM(start)} - ${formatDMY(end)}`;
  }
  return `${formatDMY(start)} - ${formatDMY(end)}`;
};

const formatCardDate = (isoString) => {
  if (!isoString) return '';
  const d = new Date(isoString);
  const dayNames = ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
  const dateStr = d.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
  return `${dateStr} — ${dayNames[d.getDay()]}`;
};

const formatUpdateTime = (isoString) => {
  if (!isoString) return '';
  const d = new Date(isoString);
  const dateStr = d.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
  const timeStr = d.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
  return `${dateStr} - ${timeStr}`;
};

const formatFeedbackDate = (isoString) => {
  if (!isoString) return '';
  const d = new Date(isoString);
  return (
    d.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' }) +
    ' ' +
    d.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' })
  );
};

const formatDrawerDate = (diary) => {
  const dateString = diary.ngayCapNhat || diary.ngayTao;
  if (!dateString) return 'Chưa xác định';
  const d = new Date(dateString);
  const dayNames = ['Chủ nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
  const dateStr = d.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' });
  return `${dateStr} (${dayNames[d.getDay()]})`;
};

function parsedEvidence(diary) {
  const raw = diary?.evidence ?? diary?.Evidence ?? null;
  if (!raw) {
    return {
      fileName: diary?.proofFileName || diary?.proofFile || '',
      description: diary?.proofDescription || '',
    };
  }
  if (typeof raw === 'object') {
    return {
      fileName: raw.fileName || raw.FileName || '',
      description: raw.description || raw.Description || '',
      size: raw.size || raw.Size,
      type: raw.type || raw.Type,
      dataUrl: raw.dataUrl || raw.DataUrl,
    };
  }
  try {
    const parsed = JSON.parse(raw) || {};
    return {
      fileName: parsed.fileName || parsed.FileName || '',
      description: parsed.description || parsed.Description || '',
      size: parsed.size || parsed.Size,
      type: parsed.type || parsed.Type,
      dataUrl: parsed.dataUrl || parsed.DataUrl,
    };
  } catch {
    return { fileName: String(raw), description: diary?.proofDescription || '' };
  }
}

const viewingEvidence = computed(() => parsedEvidence(viewingDiary.value));

function formatFileSize(bytes) {
  const n = Number(bytes) || 0;
  if (n < 1024) return `${n} B`;
  if (n < 1024 * 1024) return `${(n / 1024).toFixed(1)} KB`;
  return `${(n / (1024 * 1024)).toFixed(1)} MB`;
}

const openDrawer = async (diary) => {
  const ev = parsedEvidence(diary);

  if (diary.status === 'Draft') {
    if (isWeekOverdue(diary.week ?? currentWeek.value)) {
      displayToast('Tuần này đã quá hạn — không thể ghi nhật ký');
      return;
    }
    isWriting.value = true;
    form.value = {
      ...diary,
      proofFileName: ev.fileName || '',
      proofDescription: ev.description || '',
      evidence: typeof diary.evidence === 'string' ? diary.evidence : JSON.stringify(ev || {}),
    };
    fieldErrors.value = {};
  } else {
    // Denormalize evidence so detail fields always have values to show
    viewingDiary.value = {
      ...diary,
      proofFile: ev.fileName || '',
      proofFileName: ev.fileName || '',
      proofDescription: ev.description || '',
    };
    isDrawerOpen.value = true;

    if (!diary.isReadByStudent && diary.feedbacks?.length) {
      try {
        await api.put(`/diaries/${diary.id}/read`, null, { params: { by: 'student' } });
        diary.isReadByStudent = true;
      } catch {
        diary.isReadByStudent = true;
      }
    }
  }
};

const closeDrawer = () => {
  isDrawerOpen.value = false;
  viewingDiary.value = {};
};

const displayToast = (msg) => {
  toastMessage.value = msg;
  showToast.value = true;
  setTimeout(() => (showToast.value = false), 3000);
};
</script>
