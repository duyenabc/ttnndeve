import axios from 'axios';

const api = axios.create({
  // Production: set VITE_API_BASE_URL=https://ims-api-ftzr.onrender.com/api (must end with /api)
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5071/api',
  // Render free tier cold-start often exceeds 15s
  timeout: 45000,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

api.interceptors.response.use(
  (response) => response,
  (error) => {
    const url = String(error.config?.url || '');
    const isAuthAttempt =
      url.includes('/auth/login') ||
      url.includes('/auth/register') ||
      url.includes('/auth/forgot') ||
      url.includes('/auth/reset');

    // Failed login returns 401 — do not treat as "session expired"
    if (error.response?.status === 401 && !isAuthAttempt) {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
      if (!window.location.pathname.startsWith('/login')) {
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  }
);

export default api;
