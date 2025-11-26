// js/dashboard.js - VERSION UNIFICADA PARA TODO EL DASHBOARD
// Reemplaza tus otros scripts por este (o usa solo este).
(function () {
  const API = "https://localhost:7245/api";
  const token = localStorage.getItem("token");
  const rawUser = localStorage.getItem("user");
  const user = rawUser ? JSON.parse(rawUser) : null;

  // Seguridad básica
  function requireAuth() {
    if (!token || !user) {
      window.location.href = "index.html";
      return false;
    }
    // role puede ser role o roleId o propiedad similar
    const roleStr = (user.role || user.roleName || user.roleId || "").toString().toLowerCase();
    if (!roleStr.includes("recepcionista") && roleStr !== "2" && roleStr !== "recepcionist") {
      // Si no quieres obligar a recepcionista, comenta lo siguiente
      // alert("Acceso denegado. Solo recepcionistas pueden acceder al panel.");
      // window.location.href = "principal.html";
      // return false;
    }
    return true;
  }

  if (!requireAuth()) return;

  // Export globals que otros módulos pueden usar
  window.API_URL = API;
  window.token = token;

  // ------------- UTILIDADES -------------
  function q(id) { return document.getElementById(id); }
  function on(el, evt, cb) { if (el) el.addEventListener(evt, cb); }

  function safeJSON(res) {
    return res.text().then(text => {
      try { return text ? JSON.parse(text) : null; }
      catch { return null; }
    });
  }

  // Estado badge para citas
  function getEstadoBadge(estado) {
    const estados = {
      'Pendiente': '<span style="background: #FEF3C7; color: #92400E; padding: 0.25rem 0.6rem; border-radius: 12px;">Pendiente</span>',
      'Confirmada': '<span style="background: #D1FAE5; color: #065F46; padding: 0.25rem 0.6rem; border-radius: 12px;">Confirmada</span>',
      'Completada': '<span style="background: #DBEAFE; color: #1E40AF; padding: 0.25rem 0.6rem; border-radius: 12px;">Completada</span>',
      'Cancelada': '<span style="background: #FEE2E2; color: #991B1B; padding: 0.25rem 0.6rem; border-radius: 12px;">Cancelada</span>'
    };
    return estados[estado] || `<span>${estado}</span>`;
  }
  window.getEstadoBadge = getEstadoBadge;

  // Modal helpers
  function abrirModal(title, innerHtml, onSubmit) {
    const modal = q('modal');
    const modalTitle = q('modalTitle');
    const modalFormContent = q('modalFormContent');
    modalTitle.textContent = title;
    modalFormContent.innerHTML = innerHtml;
    modal.classList.add('active');

    const form = q('modalForm');
    // quitar handler anterior
    form.onsubmit = async (e) => {
      e.preventDefault();
      await onSubmit();
    };
  }
  function cerrarModal() {
    const modal = q('modal');
    if (modal) modal.classList.remove('active');
  }
  on(q('btnCloseModal'), 'click', cerrarModal);
  on(q('modal'), 'click', (e) => { if (e.target.id === 'modal') cerrarModal(); });

  /* ===========================
      NAVEGACIÓN Y AUTENTICACIÓN
     =========================== */
  function configurarNavegacion() {
    const menuItems = document.querySelectorAll('.menu-item');
    const sections = document.querySelectorAll('.section');

    menuItems.forEach(item => {
      item.addEventListener('click', () => {
        const sec = item.dataset.section;
        menuItems.forEach(m => m.classList.remove('active'));
        sections.forEach(s => s.classList.remove('active'));
        item.classList.add('active');
        const el = q(sec);
        if (el) el.classList.add('active');

        // Llamar carga de sección cuando se active
        switch (sec) {
          case 'dashboard': cargarDashboard(); break;
          case 'clientes': cargarClientes(); break;
          case 'mascotas': cargarMascotas(); break;
          case 'citas': cargarCitas(); break;
          case 'productos': cargarProductos(); break;
          case 'inventario': cargarInventario(); break;
        }
      });
    });
  }

  on(q('logoutBtn'), 'click', () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    window.location.href = 'index.html';
  });

  // Inicialización
  configurarNavegacion();

  /* ===========================
      DASHBOARD: ESTADÍSTICAS
     =========================== */
  async function cargarDashboard() {
    try {
      // clientes
      const resClientes = await fetch(`${API}/Clientes`, { headers: { Authorization: `Bearer ${token}` } });
      const clientes = resClientes.ok ? await resClientes.json() : [];
      if (q('totalClientes')) q('totalClientes').textContent = (clientes || []).length;

      // mascotas
      const resMascotas = await fetch(`${API}/Mascotas`, { headers: { Authorization: `Bearer ${token}` } });
      const mascotas = resMascotas.ok ? await resMascotas.json() : [];
      if (q('totalMascotas')) q('totalMascotas').textContent = (mascotas || []).length;

      // citas
      const resCitas = await fetch(`${API}/Citas`, { headers: { Authorization: `Bearer ${token}` } });
      const citas = resCitas.ok ? await resCitas.json() : [];
      // citas hoy
      const hoy = new Date().toISOString().split('T')[0];
      const citasHoy = (citas || []).filter(c => (c.fechaHora || '').startsWith(hoy));
      if (q('citasHoy')) q('citasHoy').textContent = citasHoy.length;

      // productos stock total
      const resProductos = await fetch(`${API}/Productos`, { headers: { Authorization: `Bearer ${token}` } });
      const productos = resProductos.ok ? await resProductos.json() : [];
      const totalStock = (productos || []).reduce((acc, p) => acc + (Number(p.stock) || 0), 0);
      if (q('productosStock')) q('productosStock').textContent = totalStock;

      // proximas citas (primeras 5)
      if (q('tablaCitasRecientes')) {
        const tbody = q('tablaCitasRecientes');
        const proximas = (citas || []).slice(0, 5);
        if (proximas.length === 0) {
          tbody.innerHTML = `<tr><td colspan="6" style="text-align:center">No hay citas próximas</td></tr>`;
        } else {
          tbody.innerHTML = proximas.map(c => {
            const dt = new Date(c.fechaHora);
            const estado = getEstadoBadge(c.estado || c.estadoNombre || 'Pendiente');
            return `
              <tr>
                <td>${dt.toLocaleDateString('es-DO')}</td>
                <td>${dt.toLocaleTimeString('es-DO', {hour:'2-digit', minute:'2-digit'})}</td>
                <td>${c.dueñoNombre || c.clienteNombre || '-'}</td>
                <td>${c.mascotaNombre || '-'}</td>
                <td>${c.motivo || '-'}</td>
                <td>${estado}</td>
              </tr>
            `;
          }).join('');
        }
      }
    } catch (err) {
      console.error('Error cargando dashboard', err);
      if (q('totalClientes')) q('totalClientes').textContent = '0';
      if (q('totalMascotas')) q('totalMascotas').textContent = '0';
    }
  }

  /* ===========================
      CLIENTES (listar, crear, editar, eliminar)
     =========================== */
  async function cargarClientes(filtro = '') {
    try {
      let url = `${API}/Clientes`;
      if (filtro) url += `?nombre=${encodeURIComponent(filtro)}`;
      const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
      if (!res.ok) throw new Error('Error al cargar clientes');
      const clientes = await res.json();
      const tbody = q('tablaClientes');
      if (!tbody) return;
      if (!clientes || clientes.length === 0) {
        tbody.innerHTML = `<tr><td colspan="7" style="text-align:center">No se encontraron clientes</td></tr>`;
        return;
      }
      tbody.innerHTML = clientes.map(c => `
        <tr>
          <td>${c.id}</td>
          <td>${c.nombre}</td>
          <td>${c.apellido}</td>
          <td>${c.cedula || '-'}</td>
          <td>${c.direccion || '-'}</td>
          <td>${c.telefono || c.email || '-'}</td>
          <td>
            <button class="btn-action btn-edit" data-id="${c.id}" data-type="cliente">Editar</button>
            <button class="btn-action btn-delete" data-id="${c.id}" data-type="cliente">Eliminar</button>
          </td>
        </tr>
      `).join('');
      attachTableButtons(); // delegación botones
    } catch (err) {
      console.error(err);
      if (q('tablaClientes')) q('tablaClientes').innerHTML = `<tr><td colspan="7" style="text-align:center;color:#EF4444">Error al cargar datos</td></tr>`;
    }
  }

  async function mostrarModalCliente(cliente = null) {
    const html = `
      <div class="form-group"><label>Nombre</label><input id="inputNombre" required value="${cliente?.nombre || ''}" class="form-control"></div>
      <div class="form-group"><label>Apellido</label><input id="inputApellido" required value="${cliente?.apellido || ''}" class="form-control"></div>
      <div class="form-group"><label>Cédula</label><input id="inputCedula" ${cliente ? 'disabled' : 'required'} value="${cliente?.cedula || ''}" class="form-control"></div>
      <div class="form-group"><label>Dirección</label><input id="inputDireccion" value="${cliente?.direccion || ''}" class="form-control"></div>
      <div class="form-group"><label>Email</label><input id="inputEmail" type="email" required value="${cliente?.email || ''}" class="form-control"></div>
    `;
    abrirModal(cliente ? 'Editar Cliente' : 'Nuevo Cliente', html, async () => {
      const data = {
        nombre: q('inputNombre').value.trim(),
        apellido: q('inputApellido').value.trim(),
        cedula: q('inputCedula') ? q('inputCedula').value.trim() : (cliente?.cedula || ''),
        direccion: q('inputDireccion').value.trim(),
        email: q('inputEmail').value.trim()
      };
      try {
        let res;
        if (cliente) {
          res = await fetch(`${API}/Clientes/${cliente.id}`, {
            method: 'PUT', headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${token}` },
            body: JSON.stringify(data)
          });
        } else {
          res = await fetch(`${API}/Clientes`, {
            method: 'POST', headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${token}` },
            body: JSON.stringify(data)
          });
        }
        if (!res.ok) {
          const err = await safeJSON(res);
          throw new Error(err?.error || 'Error guardando cliente');
        }
        cerrarModal();
        cargarClientes();
      } catch (e) {
        alert('Error: ' + e.message);
      }
    });
  }

  /* ===========================
      MASCOTAS
     =========================== */
  async function cargarMascotas(filtro = '') {
    try {
      let url = `${API}/Mascotas`;
      if (filtro) url += `?nombre=${encodeURIComponent(filtro)}`;
      const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
      if (!res.ok) throw new Error('Error al cargar mascotas');
      const items = await res.json();
      const tbody = q('tablaMascotas');
      if (!tbody) return;
      if (!items || items.length === 0) {
        tbody.innerHTML = `<tr><td colspan="8" style="text-align:center">No se encontraron mascotas</td></tr>`;
        return;
      }
      tbody.innerHTML = items.map(m => `
        <tr>
          <td>${m.id}</td>
          <td>${m.nombre}</td>
          <td>${m.tipoMascota || m.tipo || '-'}</td>
          <td>${m.edad ?? '-'}</td>
          <td>${m.peso ?? '-'}</td>
          <td>${m.dueñoNombre || m.duenoNombre || '-'}</td>
          <td>${m.estaCastrado ? '✅' : '❌'}</td>
          <td>
            <button class="btn-action btn-edit" data-id="${m.id}" data-type="mascota">Editar</button>
            <button class="btn-action btn-delete" data-id="${m.id}" data-type="mascota">Eliminar</button>
          </td>
        </tr>
      `).join('');
      attachTableButtons();
    } catch (err) {
      console.error(err);
      if (q('tablaMascotas')) q('tablaMascotas').innerHTML = `<tr><td colspan="8" style="text-align:center;color:#EF4444">Error al cargar datos</td></tr>`;
    }
  }

  async function mostrarModalMascota(mascota = null) {
    // necesitamos tipos y dueños
    let tipos = [];
    let duenos = [];
    try {
      const rtip = await fetch(`${API}/TipoMascota`, { headers: { Authorization: `Bearer ${token}` }});
      tipos = rtip.ok ? await rtip.json() : [];
      const rdu = await fetch(`${API}/Clientes`, { headers: { Authorization: `Bearer ${token}` }});
      duenos = rdu.ok ? await rdu.json() : [];
    } catch(e) { console.warn('No se pudieron cargar tipos/duenos', e) }

    const tipoOptions = tipos.map(t => `<option value="${t.id}" ${mascota?.tipoMascotaId === t.id ? 'selected' : ''}>${t.tipo || t.nombre || t.tipoMascota}</option>`).join('');
    const duenOptions = duenos.map(d => `<option value="${d.id}" ${mascota?.dueñoId === d.id ? 'selected' : ''}>${d.nombre} ${d.apellido}</option>`).join('');

    const html = `
      <div class="form-group"><label>Nombre</label><input id="inputNombreMascota" required value="${mascota?.nombre || ''}" class="form-control"></div>
      <div class="form-group"><label>Tipo</label><select id="inputTipoMascota" class="form-control" required><option value="">Seleccionar...</option>${tipoOptions}</select></div>
      <div class="form-group"><label>Dueño</label><select id="inputDueno" class="form-control" required><option value="">Seleccionar...</option>${duenOptions}</select></div>
      <div class="form-group"><label>Edad</label><input id="inputEdad" type="number" min="0" value="${mascota?.edad ?? ''}" class="form-control"></div>
      <div class="form-group"><label>Peso (kg)</label><input id="inputPeso" type="number" step="0.01" min="0" value="${mascota?.peso ?? ''}" class="form-control"></div>
      <div class="form-group"><label><input id="inputCastrado" type="checkbox" ${mascota?.estaCastrado ? 'checked' : ''}>¿Castrado?</label></div>
    `;
    abrirModal(mascota ? 'Editar Mascota' : 'Nueva Mascota', html, async () => {
      const payload = {
        nombre: q('inputNombreMascota').value.trim(),
        tipoMascotaId: Number(q('inputTipoMascota').value),
        dueñoId: Number(q('inputDueno').value),
        edad: Number(q('inputEdad').value),
        peso: Number(q('inputPeso').value),
        estaCastrado: !!q('inputCastrado').checked
      };
      try {
        let res;
        if (mascota) {
          res = await fetch(`${API}/Mascotas/${mascota.id}`, { method: 'PUT', headers: {'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
        } else {
          res = await fetch(`${API}/Mascotas`, { method: 'POST', headers: {'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
        }
        if (!res.ok) throw new Error('Error guardando mascota');
        cerrarModal();
        cargarMascotas();
      } catch (e) {
        alert('Error: ' + e.message);
      }
    });
  }

  /* ===========================
      CITAS
     =========================== */
  async function cargarCitas(fecha = null) {
    try {
      let url = `${API}/Citas`;
      if (fecha) url += `?fecha=${encodeURIComponent(fecha)}`;
      const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` }});
      if (!res.ok) throw new Error('Error al cargar citas');
      const items = await res.json();
      const tbody = q('tablaCitas');
      if (!tbody) return;
      if (!items || items.length === 0) {
        tbody.innerHTML = `<tr><td colspan="7" style="text-align:center">No se encontraron citas</td></tr>`;
        return;
      }
      tbody.innerHTML = items.map(c => {
        const dt = new Date(c.fechaHora);
        const estado = getEstadoBadge(c.estado || c.estadoNombre || 'Pendiente');
        return `
          <tr>
            <td>${c.id}</td>
            <td>${dt.toLocaleString('es-DO')}</td>
            <td>${c.dueñoNombre || '-'}</td>
            <td>${c.veterinarioNombre || '-'}</td>
            <td>${c.motivo || '-'}</td>
            <td>${estado}</td>
            <td>
              <button class="btn-action btn-edit" data-id="${c.id}" data-type="cita">Editar</button>
              <button class="btn-action btn-delete" data-id="${c.id}" data-type="cita">Cancelar</button>
            </td>
          </tr>
        `;
      }).join('');
      attachTableButtons();
    } catch (err) {
      console.error(err);
      if (q('tablaCitas')) q('tablaCitas').innerHTML = `<tr><td colspan="7" style="text-align:center;color:#EF4444">Error al cargar datos</td></tr>`;
    }
  }

  async function mostrarModalCita(cita = null) {
    // cargar estados, motivos, dueños
    let estados = [], motivos = [], duenos = [], veterinarios = [];
    try {
      const r1 = await fetch(`${API}/Estados`, { headers: { Authorization: `Bearer ${token}` }});
      estados = r1.ok ? await r1.json() : [];
      const r2 = await fetch(`${API}/MotivosCita`, { headers: { Authorization: `Bearer ${token}` }});
      motivos = r2.ok ? await r2.json() : [];
      const r3 = await fetch(`${API}/Clientes`, { headers: { Authorization: `Bearer ${token}` }});
      duenos = r3.ok ? await r3.json() : [];
      // veterinarios: si no hay endpoint, usar valores dummy
      // TODO: si tienes un endpoint de personal filtrado por veterinario, úsalo
      veterinarios = [{ id: 1, nombre: 'Dr. Juan Pérez' }, { id: 2, nombre: 'Dra. Ana Torres' }];
    } catch (e) { console.warn('error cargando datos cita', e); }

    const html = `
      <div class="form-group"><label>Fecha y Hora</label><input id="inputFechaHora" type="datetime-local" class="form-control" value="${cita ? new Date(cita.fechaHora).toISOString().slice(0,16) : ''}" required></div>
      <div class="form-group"><label>Cliente</label><select id="inputDuenoCita" class="form-control" required><option value="">Seleccionar...</option>${duenos.map(d => `<option value="${d.id}" ${cita?.dueñoId === d.id ? 'selected' : ''}>${d.nombre} ${d.apellido}</option>`).join('')}</select></div>
      <div class="form-group"><label>Veterinario</label><select id="inputVeterinario" class="form-control" required><option value="">Seleccionar...</option>${veterinarios.map(v => `<option value="${v.id}" ${cita?.veterinarioId === v.id ? 'selected' : ''}>${v.nombre}</option>`).join('')}</select></div>
      <div class="form-group"><label>Motivo</label><select id="inputMotivo" class="form-control" required><option value="">Seleccionar...</option>${motivos.map(m => `<option value="${m.id}" ${cita?.motivoId === m.id ? 'selected' : ''}>${m.motivo || m.motivoCita || 'Motivo'}</option>`).join('')}</select></div>
      <div class="form-group"><label>Estado</label><select id="inputEstado" class="form-control" required>${estados.map(e => `<option value="${e.id}" ${cita?.estadoId === e.id ? 'selected' : ''}>${e.nombre}</option>`).join('')}</select></div>
    `;
    abrirModal(cita ? 'Editar Cita' : 'Nueva Cita', html, async () => {
      const payload = {
        fechaHora: new Date(q('inputFechaHora').value).toISOString(),
        dueñoId: Number(q('inputDuenoCita').value),
        veterinarioId: Number(q('inputVeterinario').value),
        motivoId: Number(q('inputMotivo').value),
        estadoId: Number(q('inputEstado').value)
      };
      try {
        let res;
        if (cita) {
          res = await fetch(`${API}/Citas/${cita.id}`, { method: 'PUT', headers: {'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
        } else {
          res = await fetch(`${API}/Citas`, { method: 'POST', headers: {'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
        }
        if (!res.ok) throw new Error('Error guardando cita');
        cerrarModal();
        cargarCitas();
      } catch (e) { alert('Error: '+e.message) }
    });
  }

  /* ===========================
      PRODUCTOS
     =========================== */
  async function cargarTiposProducto() {
    try {
      const res = await fetch(`${API}/TipoProducto`, { headers: { Authorization: `Bearer ${token}` }});
      return res.ok ? await res.json() : [];
    } catch (e) { return []; }
  }

  async function cargarProductos(filtro = '') {
    try {
      let url = `${API}/Productos`;
      if (filtro) url += `?nombre=${encodeURIComponent(filtro)}`;
      const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` }});
      if (!res.ok) throw new Error('Error al cargar productos');
      const items = await res.json();
      const tbody = q('tablaProductos');
      if (!tbody) return;
      if (!items || items.length === 0) {
        tbody.innerHTML = `<tr><td colspan="6" style="text-align:center">No se encontraron productos</td></tr>`;
        return;
      }
      tbody.innerHTML = items.map(p => `
        <tr>
          <td>${p.id}</td>
          <td>${p.nombre}</td>
          <td>${p.tipoProducto || p.tipo || '-'}</td>
          <td>${p.stock}</td>
          <td>RD$ ${Number(p.precio || 0)}</td>
          <td>
            <button class="btn-action btn-edit" data-id="${p.id}" data-type="producto">Editar</button>
            <button class="btn-action btn-delete" data-id="${p.id}" data-type="producto">Eliminar</button>
          </td>
        </tr>
      `).join('');
      attachTableButtons();
    } catch (err) {
      console.error(err);
      if (q('tablaProductos')) q('tablaProductos').innerHTML = `<tr><td colspan="6" style="text-align:center;color:#EF4444">Error al cargar productos</td></tr>`;
    }
  }

  async function mostrarModalProducto(producto = null) {
    const tipos = await cargarTiposProducto();
    const tipoOptions = tipos.map(t => `<option value="${t.id}" ${producto?.tipoProductoId === t.id ? 'selected' : ''}>${t.tipo || t.nombre}</option>`).join('');
    const html = `
      <div class="form-group"><label>Nombre</label><input id="nombreProducto" required value="${producto?.nombre || ''}" class="form-control"></div>
      <div class="form-group"><label>Tipo</label><select id="tipoProducto" class="form-control" required><option value="">Seleccionar...</option>${tipoOptions}</select></div>
      <div class="form-group"><label>Stock</label><input id="stockProducto" type="number" min="0" value="${producto?.stock ?? 0}" class="form-control"></div>
      <div class="form-group"><label>Precio</label><input id="precioProducto" type="number" min="0" step="0.01" value="${producto?.precio ?? 0}" class="form-control"></div>
    `;
    abrirModal(producto ? 'Editar Producto' : 'Nuevo Producto', html, async () => {
      const payload = {
        nombre: q('nombreProducto').value.trim(),
        tipoProductoId: Number(q('tipoProducto').value),
        stock: Number(q('stockProducto').value),
        precio: Number(q('precioProducto').value)
      };
      try {
        let res;
        if (producto) {
          res = await fetch(`${API}/Productos/${producto.id}`, { method: 'PUT', headers:{'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
        } else {
          res = await fetch(`${API}/Productos`, { method: 'POST', headers:{'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
        }
        if (!res.ok) {
          const err = await safeJSON(res);
          throw new Error(err?.error || 'Error guardando producto');
        }
        cerrarModal();
        cargarProductos();
      } catch (e) {
        alert('Error: ' + e.message);
      }
    });
  }

  /* ===========================
      INVENTARIO (solo listado básico)
     =========================== */
  async function cargarInventario(fecha = null) {
    try {
      let url = `${API}/Inventario`;
      if (fecha) url += `?fecha=${encodeURIComponent(fecha)}`;
      const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` }});
      if (!res.ok) throw new Error('Error al cargar inventario');
      const items = await res.json();
      const tbody = q('tablaInventario');
      if (!tbody) return;
      if (!items || items.length === 0) {
        tbody.innerHTML = `<tr><td colspan="7" style="text-align:center">No se encontraron movimientos</td></tr>`;
        return;
      }
      tbody.innerHTML = items.map(l => `
        <tr>
          <td>${l.id}</td>
          <td>${l.productoNombre || l.producto?.nombre || '-'}</td>
          <td>${l.tipoMovimiento || '-'}</td>
          <td>${l.cantidad}</td>
          <td>${new Date(l.fecha).toLocaleString('es-DO')}</td>
          <td>${l.personalNombre || '-'}</td>
          <td>${l.observaciones || '-'}</td>
        </tr>
      `).join('');
    } catch (err) {
      console.error(err);
      if (q('tablaInventario')) q('tablaInventario').innerHTML = `<tr><td colspan="7" style="text-align:center;color:#EF4444">Error al cargar inventario</td></tr>`;
    }
  }

  /* ===========================
      BOTONES DE TABLA (delegación)
     =========================== */
  function attachTableButtons() {
    // Edit
    document.querySelectorAll('.btn-action.btn-edit').forEach(btn => {
      btn.onclick = async () => {
        const id = btn.dataset.id;
        const type = btn.dataset.type;
        try {
          const res = await fetch(`${API}/${capitalizePlural(type)}/${id}`, { headers: { Authorization: `Bearer ${token}` }});
          const obj = await (res.ok ? res.json() : Promise.reject('no encontrado'));
          if (type === 'cliente') mostrarModalCliente(obj);
          else if (type === 'mascota') mostrarModalMascota(obj);
          else if (type === 'cita') mostrarModalCita(obj);
          else if (type === 'producto') mostrarModalProducto(obj);
        } catch (e) {
          // fallback: try singular endpoints
          try {
            const res2 = await fetch(`${API}/${type === 'producto' ? 'Productos' : type === 'mascota' ? 'Mascotas' : type === 'cliente' ? 'Clientes' : type === 'cita' ? 'Citas' : ''}/${id}`, { headers: { Authorization: `Bearer ${token}` }});
            if (!res2.ok) throw new Error('no encontrado');
            const obj2 = await res2.json();
            if (type === 'cliente') mostrarModalCliente(obj2);
            else if (type === 'mascota') mostrarModalMascota(obj2);
            else if (type === 'cita') mostrarModalCita(obj2);
            else if (type === 'producto') mostrarModalProducto(obj2);
          } catch (err) {
            alert('No se pudo cargar el registro para editar');
          }
        }
      };
    });

    // Delete / Cancel
    document.querySelectorAll('.btn-action.btn-delete').forEach(btn => {
      btn.onclick = async () => {
        const id = btn.dataset.id;
        const type = btn.dataset.type;
        if (!confirm('¿Estás seguro?')) return;
        try {
          const res = await fetch(`${API}/${capitalizePlural(type)}/${id}`, { method: 'DELETE', headers: { Authorization: `Bearer ${token}` }});
          if (!res.ok) {
            const err = await safeJSON(res);
            throw new Error(err?.error || 'Error en operación');
          }
          // recargar la sección correspondiente
          if (type === 'cliente') cargarClientes();
          else if (type === 'mascota') cargarMascotas();
          else if (type === 'cita') cargarCitas();
          else if (type === 'producto') cargarProductos();
        } catch (e) {
          alert('Error: ' + (e.message || e));
        }
      };
    });
  }

  function capitalizePlural(type) {
    // mapea 'cliente' -> 'Clientes', 'mascota'->'Mascotas' etc.
    if (type === 'cliente') return 'Clientes';
    if (type === 'mascota') return 'Mascotas';
    if (type === 'cita') return 'Citas';
    if (type === 'producto') return 'Productos';
    return type;
  }

  /* ===========================
      BUSCADORES Y BOTONES
     =========================== */
  on(q('btnBuscarCliente'), 'click', () => { const f = q('buscarCliente').value.trim(); cargarClientes(f); });
  on(q('buscarCliente'), 'keypress', (e) => { if (e.key === 'Enter') { e.preventDefault(); q('btnBuscarCliente').click(); } });

  on(q('btnBuscarMascota'), 'click', () => { const f = q('buscarMascota').value.trim(); cargarMascotas(f); });
  on(q('buscarMascota'), 'keypress', (e) => { if (e.key === 'Enter') { e.preventDefault(); q('btnBuscarMascota').click(); } });

  on(q('btnBuscarCita'), 'click', () => { const f = q('buscarFechaCita').value; cargarCitas(f || null); });

  on(q('btnBuscarProducto'), 'click', () => { const f = q('buscarProducto').value.trim(); cargarProductos(f); });
  on(q('buscarProducto'), 'keypress', (e) => { if (e.key === 'Enter') { e.preventDefault(); q('btnBuscarProducto').click(); } });

  // Nuevo cliente
  on(q('btnAgregarCliente'), 'click', () => mostrarModalCliente(null));
  // Nueva mascota
  on(q('btnAgregarMascota'), 'click', async () => { await mostrarModalMascota(null); });
  // Nueva cita
  on(q('btnAgregarCita'), 'click', async () => { await mostrarModalCita(null); });
  // Nuevo producto
  on(q('btnAgregarProducto'), 'click', async () => { await mostrarModalProducto(null); });

  /* ===========================
      EXPORTAR FUNCIONES GLOBALES
     =========================== */
  window.cargarDashboard = cargarDashboard;
  window.cargarClientes = cargarClientes;
  window.cargarMascotas = cargarMascotas;
  window.cargarCitas = cargarCitas;
  window.cargarProductos = cargarProductos;
  window.cargarInventario = cargarInventario;

  // Inicializar vista por defecto
  (function init() {
    cargarDashboard();
    // Cargar clientes/mascotas/productos en background (no bloquear)
    cargarClientes();
    cargarMascotas();
    cargarProductos();
  })();

})();
