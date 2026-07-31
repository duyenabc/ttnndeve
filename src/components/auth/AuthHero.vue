<template>
  <aside class="hidden lg:block w-[30%] relative overflow-hidden bg-slate-900 shrink-0 h-full min-h-screen">
    <img
      :src="imageSrc"
      alt="Trường Đại học Kinh tế - ĐHĐN"
      class="absolute inset-0 w-full h-full object-cover"
    />
    <div class="absolute inset-0 bg-gradient-to-t from-[#001D38]/40 via-transparent to-transparent"></div>
    <div class="absolute bottom-0 left-0 right-0 bg-[#003362]/92 p-7 xl:p-8 text-white backdrop-blur-sm border-t border-white/10 space-y-2">
      <h2 class="font-extrabold text-xl xl:text-2xl tracking-tight">Chào mừng đến với IMS</h2>
      <p class="text-xs xl:text-sm text-blue-100/95 leading-relaxed">
        Hệ thống Quản lý Thực tập số, nơi kết nối, theo dõi tiến trình và tối ưu hóa chu kỳ thực tập của bạn.
      </p>
    </div>
  </aside>
</template>

<script setup>
defineProps({
  imageSrc: {
    type: String,
    default:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuCZRkOs9Q3Dw3ZZBTmD5yzPfsGX_mB5RFGXg6CYzXTQaGFmHrEevcp5Ej2b0wTIM4RPFJI5HHYGFA4wvrU9ZZycMHAIv-Vr3PZHSVEM9aZ694V_B1Ztevu_KlxPeQCq4ycD-vXigkKjNseXDknNuMISayvDjVf13d5dU2SpkUYab9kdcWyy8AMMVMnb9-ldcSq0DBRWhJkxanTB8TLyNsqH0u4f3GHltk02LFsWTjd1U-DkraSv0wTYtFq_G26c4puhx7yKcSAaBRfr3p0',
  },
});
</script>
