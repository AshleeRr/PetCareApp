// js/admin.js
document.addEventListener("DOMContentLoaded", () => {
  const API_URL = "https://localhost:7245/api"; // asegúrate que use https si tu backend también
  const token = localStorage.getItem("token");
  const user = JSON.parse(localStorage.getItem("user"));
  const logoutBtn = document.getElementById("logoutBtn");
  const userNameLabel = document.getElementById("userName");

  // 🔒 Validar sesión y existencia del usuario
  if (!token || !user) {
    window.location.href = "index.html";
    return;
  }

  // ✅ Normalizar el rol (por si viene en minúsculas)
  const role = user.role?.toLowerCase();

  // 🔐 Verificar que sea admin
  if (role !== "admin" && role !== "administrador") {
    alert("Acceso denegado. Solo los administradores pueden acceder a esta página.");
    window.location.href = "principal.html";
    return;
  }

  console.log(`✅ Bienvenido ${user.userName} (${user.role})`);

  // 👤 Mostrar nombre del usuario en el header si existe el elemento
  if (userNameLabel) {
    userNameLabel.textContent = user.userName || user.email;
  }

  // 🚪 Logout
  if (logoutBtn) {
    logoutBtn.addEventListener("click", () => {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      window.location.href = "index.html";
    });
  }

  // 🧭 Navegación entre secciones (menú lateral o tabs)
  document.querySelectorAll(".menu-item").forEach(item => {
    item.addEventListener("click", () => {
      const sectionId = item.getAttribute("data-section");
      document.querySelectorAll(".section").forEach(sec => sec.classList.remove("active"));
      document.getElementById(sectionId)?.classList.add("active");
    });
  });

  console.log("✅ Admin panel inicializado correctamente");
});
