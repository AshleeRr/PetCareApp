const API_URL = "https://localhost:7245/api/ControllerAutenticacion";

document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("registerForm");

  if (!form) {
    console.error("⚠️ No se encontró el formulario con id 'registerForm'");
    return;
  }

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const usuario = {
      userName: document.getElementById("nombre").value.trim(),
      email: document.getElementById("email").value.trim(),
      passwordHashed: document.getElementById("password").value.trim(),
      roleId: 1 // cliente por defecto
    };

    try {
      const response = await fetch(`${API_URL}/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(usuario)
      });

      if (!response.ok) {
        const errorText = await response.text();
        console.error("❌ Error HTTP:", response.status, errorText);
        throw new Error("No se pudo registrar el usuario.");
      }

      const data = await response.json();
      console.log("✅ Registro exitoso:", data);
      alert("Usuario registrado correctamente ✅");

      // Redirigir al login
      window.location.href = "index.html";
    } catch (error) {
      console.error("❌ Error de registro:", error);
      alert("Error al registrar usuario. Verifica la conexión o los datos.");
    }
  });
});
