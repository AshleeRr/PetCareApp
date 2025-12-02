// ============================================
// 1. config.js - Configuración global
// ============================================
const CONFIG = {
  API_URL: "https://localhost:7245/api",
  getToken: () => localStorage.getItem("token"),
  getUser: () => {
    const raw = localStorage.getItem("user");
    return raw ? JSON.parse(raw) : null;
  }
};

window.CONFIG = CONFIG;