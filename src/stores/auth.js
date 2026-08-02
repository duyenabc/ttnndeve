import { defineStore } from 'pinia';
import api from '@/api/api';

function getStoredUser() {
  try {
    const raw = localStorage.getItem('user');
    if (!raw || raw === 'undefined' || raw === 'null') return null;
    return JSON.parse(raw);
  } catch {
    localStorage.removeItem('user');
    return null;
  }
}

function getStoredToken(key) {
  const val = localStorage.getItem(key);
  if (!val || val === 'undefined' || val === 'null') return null;
  return val;
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: getStoredUser(),
    accessToken: getStoredToken('accessToken'),
    refreshToken: getStoredToken('refreshToken')
  }),
  getters: {
    isAuthenticated: (state) => !!state.accessToken && !!state.user,
    userRole: (state) => state.user?.vaiTro || null
  },
  actions: {
    setAuth(data) {
      if (!data) return;
      this.user = data.user || null;
      this.accessToken = data.accessToken || null;
      this.refreshToken = data.refreshToken || null;

      if (this.user) {
        localStorage.setItem('user', JSON.stringify(this.user));
      } else {
        localStorage.removeItem('user');
      }

      if (this.accessToken) {
        localStorage.setItem('accessToken', this.accessToken);
      } else {
        localStorage.removeItem('accessToken');
      }

      if (this.refreshToken) {
        localStorage.setItem('refreshToken', this.refreshToken);
      } else {
        localStorage.removeItem('refreshToken');
      }
    },
    async login(credentials) {
      const payload = {
        maDinhDanh: String(credentials?.maDinhDanh || '').trim(),
        matKhau: String(credentials?.matKhau || '').trim(),
      };
      const attempt = () => api.post('/auth/login', payload);
      let res;
      try {
        res = await attempt();
      } catch (err) {
        const retryable =
          err?.code === 'ERR_NETWORK' ||
          err?.code === 'ECONNABORTED' ||
          err?.response?.status === 503;
        if (!retryable) throw err;
        res = await attempt();
      }
      this.setAuth(res.data);
      return res.data;
    },
    async fetchProfile() {
      if (!this.accessToken) return;
      try {
        const res = await api.get('/auth/me');
        this.user = { ...this.user, ...res.data };
        localStorage.setItem('user', JSON.stringify(this.user));
      } catch (err) {
        console.error('Fetch profile error:', err);
      }
    },
    logout() {
      this.user = null;
      this.accessToken = null;
      this.refreshToken = null;
      localStorage.removeItem('user');
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
    }
  }
});

