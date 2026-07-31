import axios from 'axios';

// Base API configuration to point to ASP.NET Core local server
const apiClient = axios.create({
  baseURL: 'http://localhost:5000/api', // Default port for ASP.NET Core without HTTPS, adjust if using HTTPS (e.g., 5001)
  headers: {
    'Content-Type': 'application/json',
  },
});

export default apiClient;
