import path from 'path';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [plugin()],
  resolve: {
    alias: {
      '@': path.resolve(process.cwd(), './src')
    }
  },
  server: {
    host: '0.0.0.0',
    port: 3000,
    cors: true,
    // Avoid EBUSY when .NET locks apphost.exe under Backend/
    watch: {
      ignored: ['**/Backend/**', '**/Backend\\**']
    }
  }
});
