// js/mascotas.js
const API_MASCOTAS = `${window.API_URL}/Mascotas`;
const API_BASE = window.API_URL;
const token = window.token;
const q = window.q;
const on = window.on;
const abrirModal = window.abrirModal;
const cerrarModal = window.cerrarModal;
const attachTableButtons = window.attachTableButtons;
const safeJSON = window.safeJSON;

/* ===========================
    MASCOTAS (listar, crear, editar, eliminar)
   =========================== */
async function cargarMascotas(filtro = '') {
  try {
    let url = API_MASCOTAS;
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
  let tipos = [];
  let duenos = [];
  try {
    const rtip = await fetch(`${API_BASE}/TipoMascota`, { headers: { Authorization: `Bearer ${token}` }});
    tipos = rtip.ok ? await rtip.json() : [];
    const rdu = await fetch(`${API_BASE}/Clientes`, { headers: { Authorization: `Bearer ${token}` }});
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
        res = await fetch(`${API_MASCOTAS}/${mascota.id}`, { method: 'PUT', headers: {'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
      } else {
        res = await fetch(API_MASCOTAS, { method: 'POST', headers: {'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
      }
      if (!res.ok) {
        const err = await safeJSON(res);
        throw new Error(err?.error || 'Error guardando mascota');
      }
      cerrarModal();
      cargarMascotas();
    } catch (e) {
      alert('Error: ' + e.message);
    }
  });
}

// Handlers de botones/buscadores de Mascotas
on(q('btnBuscarMascota'), 'click', () => { const f = q('buscarMascota').value.trim(); cargarMascotas(f); });
on(q('buscarMascota'), 'keypress', (e) => { if (e.key === 'Enter') { e.preventDefault(); q('btnBuscarMascota').click(); } });
on(q('btnAgregarMascota'), 'click', async () => { await mostrarModalMascota(null); });

// Exportar funciones globales
window.cargarMascotas = cargarMascotas;
window.mostrarModalMascota = mostrarModalMascota;


