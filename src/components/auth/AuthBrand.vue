<template>
  <div class="flex items-center justify-center">
    <img
      :src="logoSrc"
      alt="Logo Đại học Đà Nẵng - Trường Đại học Kinh tế"
      :class="sizeClass"
      class="w-auto object-contain"
    />
  </div>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  logoSrc: {
    type: String,
    default:
      'https://lh3.googleusercontent.com/aida-public/AB6AXuBY-V-ETBnj2vtnqINCtqrAlkJPXTnMj8jvTwDdeKgx19ulKNTwGOaoijasxY_lIysvwowlvCSWkbEOvtaXe3IxYuVftunlj8Td9c2fVetw3R8TJqtM9zSD0HNGk9FxaP-vl50Uz0A5QMoCH9NVg8FYqySnOI6ojbC3DeMuzc-vaOK2nloqPZF6au9Gd4xC8NvgGECStxGUiInrUwDZD7CEAvzc3A0bJofi0KNpr0LI-oJsY974iYQcRi9xkYqb5GZS0pao11fyUCxu',
  },
  size: {
    type: String,
    default: 'md',
  },
});

const sizeClass = computed(() => {
  if (props.size === 'lg') return 'h-24 sm:h-28';
  return 'h-16 sm:h-20';
});
</script>
