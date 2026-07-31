<template>
  <footer class="bg-[#005EA3] text-white w-full mt-auto shadow-inner font-sans">
    <div class="w-full mx-auto px-6 lg:px-8 py-12">
      <!-- 4 Column Grid -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-10">
        <!-- Column 1: Logo & Introduction -->
        <div class="flex flex-col space-y-4">
          <div class="flex items-center space-x-3 bg-white/10 p-3 rounded-2xl w-fit backdrop-blur-xs">
            <img
              alt="TrÆ°á»ng Äáº¡i há»c Kinh táº¿ - ÄH ÄÃ  Náºµng"
              class="h-14 w-auto object-contain drop-shadow"
              src="https://lh3.googleusercontent.com/aida-public/AB6AXuBAI8djrAd-nZQ5MYlWMJpXo45oC7yEgroVX6knYldfG57Tf2Rbt03V-3ZVOMAhcbLGBMcS4HzqK9ag1JgEyj2btMyxiioLkRfKGqkJni6tddzWXuuoisK1fx4pOCH5dkuF58C-y4Xjdlk88uvBZn13VKmgi1t_gP9uuDa8arV9bL4ClNpQNzK7KGAItjaF66dD56SttjZJl2Kw58MhWgooivjmf_Bb1rKSJGcDmIeqn2-Wmxib7S3YlyN9R3xLtDH4Kc0mz6KHwRLZ"
            />
          </div>
          <p class="text-[13px] leading-relaxed text-white/80 font-normal">
            ChÃºng tÃ´i tin tÆ°á»Ÿng ráº±ng giÃ¡o dá»¥c Ä‘áº¡i há»c Ä‘Ã³ng vai trÃ² then chá»‘t trong "kiáº¿n táº¡o xÃ£ há»™i tÆ°Æ¡ng lai". Há»‡ thá»‘ng quáº£n lÃ½ thá»±c táº­p (IMS) Ä‘Æ°á»£c thiáº¿t káº¿ nháº±m tá»‘i Æ°u hÃ³a quy trÃ¬nh hÆ°á»›ng dáº«n vÃ  theo sÃ¡t lá»™ trÃ¬nh phÃ¡t triá»ƒn cá»§a sinh viÃªn.
          </p>
          <router-link to="/documents" class="text-[13px] font-semibold text-white hover:text-amber-300 transition-colors duration-300 flex items-center gap-1 w-fit">
            Xem thÃªm kho tÃ i liá»‡u Â»
          </router-link>
        </div>

        <!-- Column 2: LiÃªn Há»‡ -->
        <div class="flex flex-col">
          <h3 class="font-bold text-[15px] tracking-wider uppercase mb-5 relative pb-2 text-white border-b-2 border-amber-400/80 w-fit">
            LIÃŠN Há»†
          </h3>
          <div class="space-y-3.5">
            <div class="flex items-start space-x-3">
              <span class="material-symbols-outlined text-[#f37021] text-[20px] mt-0.5 shrink-0">location_on</span>
              <span class="text-[13px] text-white/80 leading-relaxed">
                Trung tÃ¢m Sá»‘ vÃ  Há»c Liá»‡u, TrÆ°á»ng Äáº¡i há»c Kinh táº¿, ÄH ÄÃ  Náºµng.
              </span>
            </div>
            <a href="tel:02363525656" class="flex items-center space-x-3 group cursor-pointer">
              <span class="material-symbols-outlined text-[#f37021] text-[20px] shrink-0 group-hover:scale-110 transition-transform">phone_in_talk</span>
              <span class="text-[13px] text-white/80 group-hover:text-white transition-colors">(84.236) 3525656</span>
            </a>
            <a href="mailto:i.t.c@due.edu.vn" class="flex items-center space-x-3 group cursor-pointer">
              <span class="material-symbols-outlined text-[#f37021] text-[20px] shrink-0 group-hover:scale-110 transition-transform">mail</span>
              <span class="text-[13px] text-white/80 group-hover:text-white transition-colors">i.t.c@due.edu.vn</span>
            </a>
          </div>
        </div>

        <!-- Column 3: Quáº£n LÃ½ Thá»±c Táº­p (Dynamic by Role) -->
        <div class="flex flex-col">
          <h3 class="font-bold text-[15px] tracking-wider uppercase mb-5 relative pb-2 text-white border-b-2 border-amber-400/80 w-fit">
            QUáº¢N LÃ THá»°C Táº¬P
          </h3>
          <ul class="space-y-2.5 text-[13px]">
            <template v-if="userRole === 'SinhVien'">
              <li>
                <router-link to="/dashboard" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Dashboard tá»•ng quan
                </router-link>
              </li>
              <li>
                <router-link to="/student/progress" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Tiáº¿n trÃ¬nh cá»§a tÃ´i
                </router-link>
              </li>
              <li>
                <router-link to="/student/classes" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Lá»›p thá»±c táº­p cá»§a tÃ´i
                </router-link>
              </li>
              <li>
                <router-link to="/diaries" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Nháº­t kÃ½ thá»±c táº­p
                </router-link>
              </li>
              <li>
                <router-link to="/scores" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Báº£ng Ä‘iá»ƒm & ÄÃ¡nh giÃ¡
                </router-link>
              </li>
              <li>
                <router-link to="/documents" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Kho tÃ i liá»‡u & biá»ƒu máº«u
                </router-link>
              </li>
            </template>

            <template v-else-if="userRole === 'GiangVien'">
              <li>
                <router-link to="/dashboard" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Dashboard tá»•ng quan
                </router-link>
              </li>
              <li>
                <router-link to="/teacher/classes" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Lá»›p cá»§a tÃ´i
                </router-link>
              </li>
              <li>
                <router-link to="/teacher/schedule" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Lá»‹ch hÆ°á»›ng dáº«n
                </router-link>
              </li>
              <li>
                <router-link to="/documents" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Kho tÃ i liá»‡u & biá»ƒu máº«u
                </router-link>
              </li>
            </template>

            <template v-else-if="userRole === 'GiaoVu'">
              <li>
                <router-link to="/dashboard" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Dashboard tá»•ng quan
                </router-link>
              </li>
              <li>
                <router-link to="/giaovu-requests" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> YÃªu cáº§u cá»§a tÃ´i
                </router-link>
              </li>
              <li>
                <router-link to="/documents" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Kho tÃ i liá»‡u & biá»ƒu máº«u
                </router-link>
              </li>
            </template>

            <template v-else-if="userRole === 'Admin'">
              <li>
                <router-link to="/dashboard" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Dashboard tá»•ng quan
                </router-link>
              </li>
              <li>
                <router-link to="/admin/accounts" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Quáº£n lÃ½ tÃ i khoáº£n
                </router-link>
              </li>
              <li>
                <router-link to="/admin/account-management" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> PhÃ¢n quyá»n & Vai trÃ²
                </router-link>
              </li>
              <li>
                <router-link to="/pending-requests" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> YÃªu cáº§u chá» xá»­ lÃ½
                </router-link>
              </li>
              <li>
                <router-link to="/documents" class="text-white/80 hover:text-amber-300 transition-colors flex items-center gap-1.5">
                  <span class="material-symbols-outlined text-[16px]">chevron_right</span> Kho tÃ i liá»‡u
                </router-link>
              </li>
            </template>
          </ul>
        </div>

        <!-- Column 4: Káº¿t Ná»‘i -->
        <div class="flex flex-col relative justify-between">
          <div>
            <h3 class="font-bold text-[15px] tracking-wider uppercase mb-5 relative pb-2 text-white border-b-2 border-amber-400/80 w-fit">
              Káº¾T Ná»I
            </h3>
            <div class="flex flex-wrap gap-3">
              <a href="https://facebook.com" target="_blank" rel="noopener" class="w-10 h-10 border border-white/20 rounded-xl flex items-center justify-center hover:bg-white/20 hover:border-white transition-all text-white shadow-xs" title="Facebook">
                <svg class="w-5 h-5 fill-current" viewBox="0 0 24 24"><path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z"></path></svg>
              </a>
              <a href="https://youtube.com" target="_blank" rel="noopener" class="w-10 h-10 border border-white/20 rounded-xl flex items-center justify-center hover:bg-white/20 hover:border-white transition-all text-white shadow-xs" title="YouTube">
                <svg class="w-5 h-5 fill-current" viewBox="0 0 24 24"><path d="M23.498 6.186a3.016 3.016 0 0 0-2.122-2.136C19.505 3.545 12 3.545 12 3.545s-7.505 0-9.377.505A3.017 3.017 0 0 0 .502 6.186C0 8.07 0 12 0 12s0 3.93.502 5.814a3.016 3.016 0 0 0 2.122 2.136c1.871.505 9.376.505 9.376.505s7.505 0 9.377-.505a3.015 3.015 0 0 0 2.122-2.136C24 15.93 24 12 24 12s0-3.93-.502-5.814zM9.545 15.568V8.432L15.818 12l-6.273 3.568z"></path></svg>
              </a>
              <a href="https://tiktok.com" target="_blank" rel="noopener" class="w-10 h-10 border border-white/20 rounded-xl flex items-center justify-center hover:bg-white/20 hover:border-white transition-all text-white shadow-xs" title="TikTok">
                <svg class="w-5 h-5 fill-current" viewBox="0 0 24 24"><path d="M12.525.02c1.31-.02 2.61-.01 3.91-.01.08 1.53.63 3.09 1.75 4.17 1.12 1.11 2.7 1.62 4.24 1.79v4.03c-1.44-.11-2.85-.56-4.06-1.35zM22.425 9.99c-.06 0-.12 0-.18-.01-.28 0-.56-.01-.84-.03-.06 0-.12 0-.18-.01a7.29 7.29 0 0 1-1.3-.23l-.18-.05c-.15-.05-.3-.1-.45-.16l-.18-.07c-.14-.06-.28-.13-.42-.2l-.17-.09a6.711 6.711 0 0 1-.82-.54l-.15-.12a6.974 6.974 0 0 1-.68-.65l-.12-.13c-.11-.12-.22-.24-.32-.37l-.1-.13a7.351 7.351 0 0 1-.5-.73l-.08-.14c-.08-.16-.16-.32-.23-.48l-.07-.15c-.06-.17-.12-.34-.17-.52l-.05-.18c-.04-.19-.08-.38-.11-.57l-.03-.18a7.864 7.864 0 0 1-.06-.71c0-.1 0-.2 0-.3V0h-4.22v14.74c.007 1.56-.933 2.94-2.35 3.45a3.504 3.504 0 0 1-4.49-2.01c-.28-.78-.23-1.64.13-2.38.35-.74.98-1.3 1.74-1.55a4.462 4.462 0 0 1 1.27-.18v-4.24c-2.45-.04-4.83 1.12-6.19 3.16-1.36 2.04-1.5 4.67-.37 6.84 1.13 2.17 3.52 3.4 5.96 3.09a6.836 6.836 0 0 0 5.48-4.57c.34-.97.52-2.13.52-3.31V3.95c1.07.76 2.3 1.28 3.61 1.51V.01l.01-.01z"></path></svg>
              </a>
              <a href="https://linkedin.com" target="_blank" rel="noopener" class="w-10 h-10 border border-white/20 rounded-xl flex items-center justify-center hover:bg-white/20 hover:border-white transition-all text-white shadow-xs" title="LinkedIn">
                <svg class="w-5 h-5 fill-current" viewBox="0 0 24 24"><path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433a2.062 2.062 0 0 1-2.063-2.065 2.064 2.064 0 1 1 2.063 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z"></path></svg>
              </a>
            </div>
          </div>

          <!-- Scroll to Top Button -->
          <button
            @click="scrollToTop"
            class="self-end mt-6 w-10 h-10 bg-[#1d3573] border border-white/20 rounded-xl flex items-center justify-center hover:bg-[#f37021] transition-colors shadow-lg active:scale-95"
            title="LÃªn Ä‘áº§u trang"
          >
            <span class="material-symbols-outlined text-white">keyboard_control_key</span>
          </button>
        </div>
      </div>
    </div>

    <!-- Copyright Strip -->
    <div class="bg-[#1d3573] py-5 border-t border-white/10">
      <div class="w-full mx-auto px-6 text-center">
        <p class="text-[13px] font-medium text-white/80 tracking-wide">
          Â© Báº£n quyá»n TrÆ°á»ng Äáº¡i há»c Kinh táº¿, Äáº¡i há»c ÄÃ  Náºµng | PhÃ¡t triá»ƒn bá»Ÿi <span class="text-white font-bold">Trung tÃ¢m Sá»‘ vÃ  Há»c Liá»‡u</span>
        </p>
      </div>
    </div>
  </footer>
</template>

<script setup>
import { computed } from 'vue';
import { useAuthStore } from '@/stores/auth';

const authStore = useAuthStore();
const userRole = computed(() => authStore.user?.vaiTro || 'SinhVien');

function scrollToTop() {
  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  });
}
</script>

