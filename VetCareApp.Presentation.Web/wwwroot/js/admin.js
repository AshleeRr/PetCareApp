// js/admin.js - VERSIÓN CORREGIDA Y FUNCIONAL
document.addEventListener("DOMContentLoaded", () => {
  // Configuración de la API
  const API_URL = "https://localhost:7164/api";
  const API_BASE = `${API_URL}/Admin`;

  const token = localStorage.getItem("token");
  const user = JSON.parse(localStorage.getItem("user") || "{}");

  // Elementos del DOM
  const logoutBtn = document.getElementById("logoutBtn");
  const userNameLabel = document.getElementById("userName");
  const modal = document.getElementById("modal");
  const modalFormContent = document.getElementById("modalFormContent");
  const modalTitle = document.getElementById("modalTitle");
  const btnCloseModal = document.getElementById("btnCloseModal");

  // ===== VALIDACIÓN DE SESIÓN =====
  if (!token || !user) {
    console.error("❌ No hay token o usuario en localStorage");
    window.location.href = "index.html";
    return;
  }

  console.log("🔑 Token presente:", token.substring(0, 20) + "...");
  console.log("👤 Usuario:", user);
  console.log("🎭 Rol del usuario:", user.role || user.rolNombre || "No definido");

  if (userNameLabel) userNameLabel.textContent = user.userName || user.email;

  if (logoutBtn) {
    logoutBtn.addEventListener("click", () => {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      window.location.href = "index.html";
    });
  }

  if (btnCloseModal) {
    btnCloseModal.addEventListener("click", () => cerrarModal());
  }

  // ===== NAVEGACIÓN LATERAL =====
  document.querySelectorAll(".menu-item").forEach(item => {
    item.addEventListener("click", () => {
      const sectionId = item.getAttribute("data-section");

      // Remover active de todos los items
      document.querySelectorAll(".menu-item").forEach(m => m.classList.remove("active"));
      item.classList.add("active");

      // Remover active de todas las secciones
      document.querySelectorAll(".section").forEach(sec => sec.classList.remove("active"));

      // Activar la sección seleccionada
      const section = document.getElementById(sectionId);
      if (section) section.classList.add("active");
    });
  });

  // ===== FUNCIÓN PARA LLAMADAS A LA API =====
  async function apiRequest(url, method = "GET", body = null) {
    try {
      const opts = {
        method,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`
        }
      };

      if (body) {
        opts.body = JSON.stringify(body);
        console.log("📤 Enviando:", method, url);
        console.log("📦 Payload:", JSON.stringify(body, null, 2));
      }

      const res = await fetch(url, opts);

      console.log("📡 Status:", res.status, res.statusText);

      if (!res.ok) {
        let errorMsg = `Error ${res.status}`;
        let errorDetails = "";

        try {
          const contentType = res.headers.get("content-type");
          console.log("📋 Content-Type:", contentType);

          if (contentType && contentType.includes("application/json")) {
            const errorData = await res.json();
            console.error("❌ Error JSON del servidor:", errorData);

            // Manejar errores de validación de ASP.NET Core
            if (errorData.errors) {
              errorDetails = "\n\nDetalles de validación:\n";
              for (const [field, messages] of Object.entries(errorData.errors)) {
                errorDetails += `• ${field}: ${messages.join(", ")}\n`;
              }
            }

            errorMsg = errorData.mensaje || errorData.title || errorMsg;
          } else {
            const text = await res.text();
            console.error("❌ Error texto:", text);
            errorMsg = text || errorMsg;
          }
        } catch (parseErr) {
          console.error("❌ Error al parsear respuesta:", parseErr);
        }

        alert(errorMsg + errorDetails);
        return null;
      }

      // Para DELETE que no devuelve contenido
      if (method === "DELETE") {
        return { success: true };
      }

      const result = await res.json();
      console.log("✅ Respuesta exitosa:", result);
      return result;
    } catch (err) {
      console.error("❌ Error de red:", err);
      alert("Error de conexión. Verifica que el servidor esté corriendo.");
      return null;
    }
  }

  // =========================
  // DASHBOARD
  // =========================
  async function cargarDashboard() {
    const data = await apiRequest(`${API_BASE}/dashboard/stats`);
    if (!data) return;

    // Actualizar estadísticas principales
    const elTotalUsuarios = document.getElementById("totalUsuarios");
    const elTotalPersonal = document.getElementById("totalPersonal");
    const elCitasMes = document.getElementById("citasMes");
    const elIngresosMes = document.getElementById("ingresosMes");

    if (elTotalUsuarios) elTotalUsuarios.textContent = data.totalUsuarios || 0;
    if (elTotalPersonal) elTotalPersonal.textContent = data.totalPersonal || 0;
    if (elCitasMes) elCitasMes.textContent = data.citasMes || 0;
    if (elIngresosMes) {
      const ingresos = data.ingresosMes || 0;
      elIngresosMes.textContent = `RD$ ${ingresos.toLocaleString('es-DO')}`;
    }

    // Actualizar contadores por rol
    const elCountAdmin = document.getElementById("countAdmin");
    const elCountVet = document.getElementById("countVet");
    const elCountRecep = document.getElementById("countRecep");

    if (data.usuariosPorRol) {
      if (elCountAdmin) elCountAdmin.textContent = data.usuariosPorRol.administradores || 0;
      if (elCountVet) elCountVet.textContent = data.usuariosPorRol.veterinarios || 0;
      if (elCountRecep) elCountRecep.textContent = data.usuariosPorRol.recepcionistas || 0;
    }
  }

  // =========================
  // GESTIÓN DE USUARIOS
  // =========================
  async function cargarUsuarios(termino = "") {
    const tbody = document.getElementById("tablaUsuarios");
    if (!tbody) {
      console.warn("⚠️ Elemento #tablaUsuarios no encontrado");
      return;
    }

    tbody.innerHTML = `<tr><td colspan="7" style="text-align:center;color:#6B7280">Cargando usuarios...</td></tr>`;

    let url = `${API_BASE}/usuarios`;
    if (termino && termino.trim().length > 0) {
      url = `${API_BASE}/usuarios/buscar?termino=${encodeURIComponent(termino)}`;
    }

    console.log("🔍 Cargando usuarios desde:", url);
    const usuarios = await apiRequest(url);

    tbody.innerHTML = "";

    if (!usuarios) {
      tbody.innerHTML = `<tr><td colspan="7" style="text-align:center;color:#EF4444">Error al cargar usuarios. Verifica la conexión con el servidor.</td></tr>`;
      return;
    }

    if (usuarios.length === 0) {
      tbody.innerHTML = `<tr><td colspan="7" style="text-align:center;color:#6B7280">No hay usuarios registrados</td></tr>`;
      return;
    }

    usuarios.forEach(u => {
      const id = u.id;
      const username = u.userName || "";
      const email = u.email || "";
      const rol = u.rolNombre || "—";
      const activo = u.activo;
      const estadoTexto = activo ? "Activo" : "Inactivo";
      const ultimaConexion = u.ultimaConexion ? new Date(u.ultimaConexion).toLocaleString('es-DO') : "—";

      tbody.innerHTML += `
        <tr>
          <td>${id}</td>
          <td>${username}</td>
          <td>${email}</td>
          <td><span class="role-badge badge-${rol.toLowerCase()}">${rol}</span></td>
          <td><span style="color: ${activo ? '#10B981' : '#EF4444'}">${estadoTexto}</span></td>
          <td>${ultimaConexion}</td>
          <td>
            <button class="quick-btn" onclick="verUsuario(${id})" title="Ver detalles">👁️</button>
            <button class="quick-btn" onclick="abrirEditarUsuario(${id})" title="Editar">✏️</button>
            <button class="quick-btn" onclick="toggleActivoUsuario(${id}, ${!activo})" title="${activo ? 'Desactivar' : 'Activar'}">${activo ? '🔴' : '🟢'}</button>
            <button class="quick-btn" onclick="abrirCambiarPassword(${id})" title="Cambiar contraseña">🔑</button>
            <button class="quick-btn" onclick="eliminarUsuario(${id})" title="Eliminar">🗑️</button>
          </td>
        </tr>
      `;
    });
  }

  // Buscar usuario
  const btnBuscarUsuario = document.getElementById("btnBuscarUsuario");
  if (btnBuscarUsuario) {
    btnBuscarUsuario.addEventListener("click", () => {
      const termino = document.getElementById("buscarUsuario")?.value || "";
      cargarUsuarios(termino);
    });
  }

  // Agregar nuevo usuario
  const btnAgregarUsuario = document.getElementById("btnAgregarUsuario");
  if (btnAgregarUsuario) {
    btnAgregarUsuario.addEventListener("click", () => {
      modalTitle.textContent = "Nuevo Usuario";
      modalFormContent.innerHTML = `
        <div class="config-item">
          <label>Username *</label>
          <input id="newUser_userName" class="form-control" required />
        </div>
        <div class="config-item">
          <label>Email *</label>
          <input id="newUser_email" type="email" class="form-control" required />
        </div>
        <div class="config-item">
          <label>Password *</label>
          <input id="newUser_password" type="password" class="form-control" required />
        </div>
        <div class="config-item">
          <label>Rol *</label>
          <select id="newUser_roleId" class="form-control">
            <option value="1">Cliente</option>
            <option value="2">Recepcionista</option>
            <option value="3">Admin</option>
            <option value="4">Veterinario</option>
          </select>
        </div>
      `;
      modal.classList.add("active");

      // Remover listener anterior si existe
      const oldBtn = document.getElementById("saveNewUserBtn");
      if (oldBtn) oldBtn.remove();

      const saveBtn = document.createElement("button");
      saveBtn.id = "saveNewUserBtn";
      saveBtn.className = "btn-submit";
      saveBtn.textContent = "Crear Usuario";
      modalFormContent.appendChild(saveBtn);

      saveBtn.addEventListener("click", async (e) => {
        e.preventDefault();

        const userName = document.getElementById("newUser_userName").value.trim();
        const email = document.getElementById("newUser_email").value.trim();
        const password = document.getElementById("newUser_password").value;
        const roleId = parseInt(document.getElementById("newUser_roleId").value);

        if (!userName || !email || !password) {
          alert("Por favor completa todos los campos obligatorios");
          return;
        }

        if (password.length < 6) {
          alert("La contraseña debe tener al menos 6 caracteres");
          return;
        }

        // Validar email
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
          alert("Por favor ingresa un email válido");
          return;
        }

        const payload = {
          userName,
          email,
          password,
          roleId,
          photoUrl: null // Opcional, puede ser null
        };

        console.log("📝 Creando usuario con payload:", payload);
        const res = await apiRequest(`${API_BASE}/usuarios`, "POST", payload);

        if (res) {
          alert("Usuario creado exitosamente");
          cerrarModal();
          cargarUsuarios();
        }
      });
    });
  }

  // Ver usuario
  window.verUsuario = async function (id) {
    const data = await apiRequest(`${API_BASE}/usuarios/${id}`);
    if (!data) return;

    modalTitle.textContent = `Información del Usuario #${id}`;
    modalFormContent.innerHTML = `
      <div style="padding: 1rem;">
        <p><strong>Username:</strong> ${data.userName || ""}</p>
        <p><strong>Email:</strong> ${data.email || ""}</p>
        <p><strong>Rol:</strong> ${data.rolNombre || "—"}</p>
        <p><strong>Estado:</strong> ${data.activo ? "Activo" : "Inactivo"}</p>
        <p><strong>Última conexión:</strong> ${data.ultimaConexion ? new Date(data.ultimaConexion).toLocaleString('es-DO') : "—"}</p>
      </div>
      <button id="closeInfoBtn" class="btn-submit">Cerrar</button>
    `;
    modal.classList.add("active");
    document.getElementById("closeInfoBtn").addEventListener("click", cerrarModal);
  };

  // Editar usuario
  window.abrirEditarUsuario = async function (id) {
    const data = await apiRequest(`${API_BASE}/usuarios/${id}`);
    if (!data) return;

    modalTitle.textContent = `Editar Usuario #${id}`;
    modalFormContent.innerHTML = `
      <input type="hidden" id="editUser_id" value="${id}" />
      <div class="config-item">
        <label>Username *</label>
        <input id="editUser_userName" class="form-control" value="${data.userName || ""}" />
      </div>
      <div class="config-item">
        <label>Email *</label>
        <input id="editUser_email" type="email" class="form-control" value="${data.email || ""}" />
      </div>
      <div class="config-item">
        <label>Rol *</label>
        <select id="editUser_roleId" class="form-control">
          <option value="1" ${data.roleId === 1 ? 'selected' : ''}>Cliente</option>
          <option value="2" ${data.roleId === 2 ? 'selected' : ''}>Recepcionista</option>
          <option value="3" ${data.roleId === 3 ? 'selected' : ''}>Admin</option>
          <option value="4" ${data.roleId === 4 ? 'selected' : ''}>Veterinario</option>
        </select>
      </div>
      <div class="config-item">
        <label>
          <input type="checkbox" id="editUser_activo" ${data.activo ? 'checked' : ''} />
          Usuario activo
        </label>
      </div>
      <button id="saveEditUserBtn" class="btn-submit">Guardar cambios</button>
    `;
    modal.classList.add("active");

    document.getElementById("saveEditUserBtn").addEventListener("click", async (e) => {
      e.preventDefault();

      const userName = document.getElementById("editUser_userName").value.trim();
      const email = document.getElementById("editUser_email").value.trim();
      const roleId = parseInt(document.getElementById("editUser_roleId").value);
      const activo = document.getElementById("editUser_activo").checked;

      if (!userName || !email) {
        alert("Por favor completa todos los campos obligatorios");
        return;
      }

      const payload = { userName, email, roleId, activo };
      const res = await apiRequest(`${API_BASE}/usuarios/${id}`, "PUT", payload);

      if (res) {
        alert("Usuario actualizado exitosamente");
        cerrarModal();
        cargarUsuarios();
      }
    });
  };

  // Cambiar password
  window.abrirCambiarPassword = function (id) {
    modalTitle.textContent = `Cambiar Contraseña - Usuario #${id}`;
    modalFormContent.innerHTML = `
      <input type="hidden" id="chgPass_id" value="${id}" />
      <div class="config-item">
        <label>Nueva contraseña *</label>
        <input id="chgPass_new" type="password" class="form-control" minlength="6" />
      </div>
      <div class="config-item">
        <label>Confirmar contraseña *</label>
        <input id="chgPass_confirm" type="password" class="form-control" minlength="6" />
      </div>
      <button id="btnChangePass" class="btn-submit">Cambiar Contraseña</button>
    `;
    modal.classList.add("active");

    document.getElementById("btnChangePass").addEventListener("click", async (e) => {
      e.preventDefault();

      const newPass = document.getElementById("chgPass_new").value;
      const confirmPass = document.getElementById("chgPass_confirm").value;

      if (!newPass || newPass.length < 6) {
        alert("La contraseña debe tener al menos 6 caracteres");
        return;
      }

      if (newPass !== confirmPass) {
        alert("Las contraseñas no coinciden");
        return;
      }

      const payload = { nuevaPassword: newPass };
      const res = await apiRequest(`${API_BASE}/usuarios/${id}/cambiar-password`, "PATCH", payload);

      if (res) {
        alert("Contraseña cambiada exitosamente");
        cerrarModal();
      }
    });
  };

  // Toggle activo/inactivo
  window.toggleActivoUsuario = async function (id, activo) {
    const payload = { activo: Boolean(activo) };
    const res = await apiRequest(`${API_BASE}/usuarios/${id}/toggle-activo`, "PATCH", payload);

    if (res) {
      alert(`Usuario ${activo ? "activado" : "desactivado"} exitosamente`);
      cargarUsuarios();
    }
  };

  // Eliminar usuario
  window.eliminarUsuario = async function (id) {
    if (!confirm("¿Estás seguro de eliminar este usuario? Esta acción no se puede deshacer.")) return;

    const res = await apiRequest(`${API_BASE}/usuarios/${id}`, "DELETE");

    if (res) {
      alert("Usuario eliminado exitosamente");
      cargarUsuarios();
    }
  };

  // =========================
  // GESTIÓN DE PERSONAL
  // =========================
  async function cargarPersonal(termino = "") {
    let url = `${API_BASE}/personal`;
    if (termino && termino.trim().length > 0) {
      url = `${API_BASE}/personal/buscar?termino=${encodeURIComponent(termino)}`;
    }

    const lista = await apiRequest(url);
    const tbody = document.getElementById("tablaPersonal");
    if (!tbody) return;

    tbody.innerHTML = "";

    if (!lista || lista.length === 0) {
      tbody.innerHTML = `<tr><td colspan="6" style="text-align:center;color:#6B7280">No hay personal registrado</td></tr>`;
      return;
    }

    lista.forEach(p => {
      const id = p.id;
      const nombre = `${p.nombre || ""} ${p.apellido || ""}`.trim();
      const cedula = p.cedula || "—";
      const cargo = p.cargo || "—";
      const activo = p.activo;
      const estadoTexto = activo ? "Activo" : "Inactivo";

      tbody.innerHTML += `
        <tr>
          <td>${id}</td>
          <td>${nombre}</td>
          <td>${cedula}</td>
          <td>${cargo}</td>
          <td><span style="color: ${activo ? '#10B981' : '#EF4444'}">${estadoTexto}</span></td>
          <td>
            <button class="quick-btn" onclick="abrirEditarPersonal(${id})" title="Editar">✏️</button>
            <button class="quick-btn" onclick="eliminarPersonal(${id})" title="Eliminar">🗑️</button>
          </td>
        </tr>
      `;
    });
  }

  // Buscar personal
  const btnBuscarPersonal = document.getElementById("btnBuscarPersonal");
  if (btnBuscarPersonal) {
    btnBuscarPersonal.addEventListener("click", () => {
      const termino = document.getElementById("buscarPersonal")?.value || "";
      cargarPersonal(termino);
    });
  }

  // Agregar personal
  const btnAgregarPersonal = document.getElementById("btnAgregarPersonal");
  if (btnAgregarPersonal) {
    btnAgregarPersonal.addEventListener("click", () => {
      modalTitle.textContent = "Nuevo Personal";
      modalFormContent.innerHTML = `
        <div class="config-item">
          <label>Nombre *</label>
          <input id="newPers_nombre" class="form-control" />
        </div>
        <div class="config-item">
          <label>Apellido *</label>
          <input id="newPers_apellido" class="form-control" />
        </div>
        <div class="config-item">
          <label>Cédula *</label>
          <input id="newPers_cedula" class="form-control" />
        </div>
        <div class="config-item">
          <label>Cargo *</label>
          <input id="newPers_cargo" class="form-control" placeholder="Ej: Veterinario, Asistente, etc." />
        </div>
        <button id="saveNewPersBtn" class="btn-submit">Agregar Personal</button>
      `;
      modal.classList.add("active");

      document.getElementById("saveNewPersBtn").addEventListener("click", async (e) => {
        e.preventDefault();

        const nombre = document.getElementById("newPers_nombre").value.trim();
        const apellido = document.getElementById("newPers_apellido").value.trim();
        const cedula = document.getElementById("newPers_cedula").value.trim();
        const cargo = document.getElementById("newPers_cargo").value.trim();

        if (!nombre || !apellido || !cedula || !cargo) {
          alert("Por favor completa todos los campos obligatorios");
          return;
        }

        const payload = { nombre, apellido, cedula, cargo };
        const res = await apiRequest(`${API_BASE}/personal`, "POST", payload);

        if (res) {
          alert("Personal creado exitosamente");
          cerrarModal();
          cargarPersonal();
        }
      });
    });
  }

  // Editar personal
  window.abrirEditarPersonal = async function (id) {
    const data = await apiRequest(`${API_BASE}/personal/${id}`);
    if (!data) return;

    modalTitle.textContent = `Editar Personal #${id}`;
    modalFormContent.innerHTML = `
      <input type="hidden" id="editPers_id" value="${id}" />
      <div class="config-item">
        <label>Nombre *</label>
        <input id="editPers_nombre" class="form-control" value="${data.nombre || ""}" />
      </div>
      <div class="config-item">
        <label>Apellido *</label>
        <input id="editPers_apellido" class="form-control" value="${data.apellido || ""}" />
      </div>
      <div class="config-item">
        <label>Cédula *</label>
        <input id="editPers_cedula" class="form-control" value="${data.cedula || ""}" />
      </div>
      <div class="config-item">
        <label>Cargo *</label>
        <input id="editPers_cargo" class="form-control" value="${data.cargo || ""}" />
      </div>
      <button id="saveEditPersBtn" class="btn-submit">Guardar Cambios</button>
    `;
    modal.classList.add("active");

    document.getElementById("saveEditPersBtn").addEventListener("click", async (e) => {
      e.preventDefault();

      const nombre = document.getElementById("editPers_nombre").value.trim();
      const apellido = document.getElementById("editPers_apellido").value.trim();
      const cedula = document.getElementById("editPers_cedula").value.trim();
      const cargo = document.getElementById("editPers_cargo").value.trim();

      if (!nombre || !apellido || !cedula || !cargo) {
        alert("Por favor completa todos los campos obligatorios");
        return;
      }

      const payload = { nombre, apellido, cedula, cargo };
      const res = await apiRequest(`${API_BASE}/personal/${id}`, "PUT", payload);

      if (res) {
        alert("Personal actualizado exitosamente");
        cerrarModal();
        cargarPersonal();
      }
    });
  };

  // Eliminar personal
  window.eliminarPersonal = async function (id) {
    if (!confirm("¿Estás seguro de eliminar este registro? Esta acción no se puede deshacer.")) return;

    const res = await apiRequest(`${API_BASE}/personal/${id}`, "DELETE");

    if (res) {
      alert("Personal eliminado exitosamente");
      cargarPersonal();
    }
  };

  // =========================
  // REPORTES
  // =========================
  /*const btnGenerarReporte = document.getElementById("btnGenerarReporte");
  if (btnGenerarReporte) {
    btnGenerarReporte.addEventListener("click", async () => {
      const tipo = document.getElementById("tipoReporte")?.value || "";
      const desde = document.getElementById("fechaInicio")?.value || "";
      const hasta = document.getElementById("fechaFin")?.value || "";

      if (!tipo) {
        alert("Selecciona un tipo de reporte");
        return;
      }

      if (!desde || !hasta) {
        alert("Selecciona el rango de fechas");
        return;
      }

      if (new Date(desde) > new Date(hasta)) {
        alert("La fecha inicial no puede ser mayor que la fecha final");
        return;
      }

      const url = `${API_BASE}/reportes?tipo=${encodeURIComponent(tipo)}&desde=${encodeURIComponent(desde)}&hasta=${encodeURIComponent(hasta)}`;
      const reporte = await apiRequest(url);

      const container = document.getElementById("reportResults");
      if (!container) return;

      if (!reporte) {
        container.innerHTML = `<p style="text-align:center;color:#EF4444;padding:2rem">No se pudo generar el reporte.</p>`;
        return;
      }

      // Mostrar reporte formateado
      container.innerHTML = `
        <div style="background: white; padding: 2rem; border-radius: 12px;">
          <h3 style="color: var(--dark-blue); margin-bottom: 1rem;">📊 Reporte de ${tipo.charAt(0).toUpperCase() + tipo.slice(1)}</h3>
          <p><strong>Período:</strong> ${new Date(desde).toLocaleDateString('es-DO')} - ${new Date(hasta).toLocaleDateString('es-DO')}</p>
          <hr style="margin: 1rem 0; border: none; border-top: 1px solid #E5E7EB;">
          <pre style="white-space: pre-wrap; background: #F3F4F6; padding: 1rem; border-radius: 8px; overflow-x: auto;">${JSON.stringify(reporte.datos, null, 2)}</pre>
        </div>
      `;
    });
  }*/

  // =========================
  // LOGS DEL SISTEMA
  // =========================
  const btnBuscarLogs = document.getElementById("btnBuscarLogs");
  if (btnBuscarLogs) {
    btnBuscarLogs.addEventListener("click", async () => {
      const tipo = document.getElementById("filtroTipoLog")?.value || "";
      const fecha = document.getElementById("filtroFechaLog")?.value || "";

      let url = `${API_BASE}/logs`;
      const params = [];

      if (tipo) params.push(`tipo=${encodeURIComponent(tipo)}`);
      if (fecha) params.push(`fecha=${encodeURIComponent(fecha)}`);

      if (params.length) url += `?${params.join("&")}`;

      const logs = await apiRequest(url);
      const tbody = document.getElementById("tablaLogs");
      if (!tbody) return;

      tbody.innerHTML = "";

      if (!logs || logs.length === 0) {
        tbody.innerHTML = `<tr><td colspan="5" style="text-align:center;color:#6B7280">No hay logs disponibles</td></tr>`;
        return;
      }

      logs.forEach(l => {
        const timestamp = l.timestamp ? new Date(l.timestamp).toLocaleString('es-DO') : "—";
        const usuario = l.usuario || "—";
        const accion = l.accion || "—";
        const tipo = l.tipo || "—";
        const detalles = l.detalles || "—";

        let colorTipo = "#6B7280";
        if (tipo === "INFO") colorTipo = "#10B981";
        if (tipo === "WARNING") colorTipo = "#F59E0B";
        if (tipo === "ERROR") colorTipo = "#EF4444";

        tbody.innerHTML += `
          <tr>
            <td>${timestamp}</td>
            <td>${usuario}</td>
            <td>${accion}</td>
            <td><span style="color: ${colorTipo}; font-weight: 600;">${tipo}</span></td>
            <td style="max-width: 300px; overflow: hidden; text-overflow: ellipsis;">${detalles}</td>
          </tr>
        `;
      });
    });
  }

  // =========================
  // ACCESOS RÁPIDOS
  // =========================
  window.irSeccion = function (seccionId) {
    document.querySelectorAll(".menu-item").forEach(item => {
      if (item.getAttribute("data-section") === seccionId) {
        item.click();
      }
    });
  };

  window.generarBackup = function () {
    alert("Funcionalidad de backup en desarrollo.\n\nEsta función generará una copia de seguridad de la base de datos.");
  };

  // =========================
  // CERRAR MODAL
  // =========================
  function cerrarModal() {
    modal?.classList.remove("active");
    modalFormContent.innerHTML = "";
    modalTitle.textContent = "Nuevo Registro";
  }
  window.cerrarModal = cerrarModal;

  // Cerrar modal al hacer clic fuera de él
  modal?.addEventListener("click", (e) => {
    if (e.target === modal) {
      cerrarModal();
    }
  });

  // =========================
  // CARGA INICIAL
  // =========================
  (async () => {
    console.log("🚀 Iniciando panel de administración...");
    console.log("👤 Usuario actual:", user.userName || user.email);
    console.log("🔗 API Base:", API_BASE);

    try {
      // Cargar dashboard
      console.log("📊 Cargando dashboard...");
      await cargarDashboard();

      // Cargar usuarios
      console.log("👥 Cargando usuarios...");
      await cargarUsuarios();

      // Cargar personal
      console.log("👨‍⚕️ Cargando personal...");
      await cargarPersonal();

      console.log("✅ Panel de administración cargado correctamente");
    } catch (error) {
      console.error("❌ Error durante la carga inicial:", error);
      alert("Hubo un error al cargar el panel. Verifica la consola para más detalles.");
    }
  })();

  // ============================
  // GRÁFICO: Actividad del Sistema
  // ============================
  const actividadCanvas = document.getElementById("actividadChart");

  if (actividadCanvas) {
    const actividadChart = new Chart(actividadCanvas, {
      type: "line",
      data: {
        labels: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio"],
        datasets: [{
          label: "Eventos del Sistema",
          data: [12, 19, 8, 15, 22, 30], // datos inventados
          borderWidth: 2,
          borderColor: "#3b82f6",
          backgroundColor: "rgba(59, 130, 246, 0.2)",
          tension: 0.3
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: true }
        },
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
  } else {
    console.warn("No se encontró el canvas #actividadChart");
  }

});