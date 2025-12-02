const API_URL = "https://localhost:7164/api/ControllerAutenticacion";

document.addEventListener("DOMContentLoaded", () => {
  const loginForm = document.getElementById("loginForm");
  const errorMsg = document.getElementById("error-msg");
  const googleBtn = document.querySelector(".btn-google");

  // ========================================================
  // FUNCIÓN DE REDIRECCIÓN 
  // ========================================================
  function redirigirPorRol(role) {
    const r = (role || "").toLowerCase();
    console.log("Rol detectado:", r);

    switch (r) {
      case "cliente":
        window.location.href = "principal.html";
        break;
      case "recepcionista":
        window.location.href = "dashboard.html";
        break;
      case "admin":
        window.location.href = "admin.html";
        break;
      case "veterinario":
        window.location.href = "veterinario.html";
        break;
      default:
        console.warn("Rol desconocido:", r);
        window.location.href = "principal.html";
    }
  }

  // ========================================================
  // LOGIN NORMAL 
  // ========================================================
  loginForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    errorMsg.textContent = "";

    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value.trim();

    const usuario = { email, passwordHashed: password };

    try {
      const response = await fetch(`${API_URL}/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(usuario),
      });

      console.log("HTTP status:", response.status);
      const raw = await response.text();
      console.log("RAW response:", raw);

      let data = null;
      try { data = raw ? JSON.parse(raw) : null; } 
      catch { console.warn("No se pudo parsear JSON"); }

      if (!response.ok) {
        errorMsg.textContent = "Credenciales inválidas o error del servidor (" + response.status + ")";
        return;
      }

      if (!data || !data.token) {
        errorMsg.textContent = "Respuesta inválida del servidor.";
        return;
      }

      localStorage.setItem("token", data.token);
      localStorage.setItem("user", JSON.stringify(data));

      redirigirPorRol(data.role);

    } catch (err) {
      console.error("❌ Error en login:", err);
      errorMsg.textContent = "Error al iniciar sesión.";
    }
  });

  // ========================================================
  // LOGIN CON GOOGLE 
  // ========================================================
  googleBtn.addEventListener("click", () => {
    const popup = window.open(
      `${API_URL}/google-login`,
      "Google Login",
      "width=500,height=600"
    );

    const handler = (event) => {
      const data = event.data;
      console.log("Evento del popup:", data);

      if (!data?.token) return;

      localStorage.setItem("token", data.token);
      localStorage.setItem("user", JSON.stringify(data));

      window.removeEventListener("message", handler);

      redirigirPorRol(data.role);
    };

    window.addEventListener("message", handler);
  });
});