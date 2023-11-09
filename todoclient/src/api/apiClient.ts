// apiClient.js
import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'https://localhost:7189/api/',
  // Other configuration options
});

export const apiClientPrivate = axios.create({
  baseURL: 'https://localhost:7189/api/',
  headers: {
    'Content-Type': 'application/json',
    
  },
  // withCredentials: true,
});

export default apiClient;
