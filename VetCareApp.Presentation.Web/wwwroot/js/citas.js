// js/citas.js
const API_CITAS = `${window.API_URL}/Citas`;
const API_BASE = window.API_URL;
const token = window.token;
const q = window.q;
const on = window.on;
const abrirModal = window.abrirModal;
const cerrarModal = window.cerrarModal;
const attachTableButtons = window.attachTableButtons;
const getEstadoBadge = window.getEstadoBadge;
const safeJSON = window.safeJSON;

/* ===========================
    CITAS (listar, crear, editar, cancelar)
   =========================== */
async function cargarCitas(fecha = null) {
  try {
    let url = API_CITAS;
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
  let estados = [], motivos = [], duenos = [], veterinarios = [];
  try {
    // Endpoints
    const [r1, r2, r3] = await Promise.all([
      fetch(`${API_BASE}/Estados`, { headers: { Authorization: `Bearer ${token}` }}),
      fetch(`${API_BASE}/MotivosCita`, { headers: { Authorization: `Bearer ${token}` }}),
      fetch(`${API_BASE}/Clientes`, { headers: { Authorization: `Bearer ${token}` }})
      // Agregar fetch para Veterinarios si tienes el endpoint
    ]);
    estados = r1.ok ? await r1.json() : [];
    motivos = r2.ok ? await r2.json() : [];
    duenos = r3.ok ? await r3.json() : [];
    // Valores dummy para veterinarios si no hay endpoint (MANTENER SI NO TIENES)
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
        res = await fetch(`${API_CITAS}/${cita.id}`, { method: 'PUT', headers: {'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
      } else {
        res = await fetch(API_CITAS, { method: 'POST', headers: {'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
      }
      if (!res.ok) {
        const err = await safeJSON(res);
        throw new Error(err?.error || 'Error guardando cita');
      }
      cerrarModal();
      cargarCitas();
    } catch (e) { alert('Error: '+e.message) }
  });
}

// Handlers de botones/buscadores de Citas
on(q('btnBuscarCita'), 'click', () => { const f = q('buscarFechaCita').value; cargarCitas(f || null); });
on(q('btnAgregarCita'), 'click', async () => { await mostrarModalCita(null); });

// Exportar funciones globales
window.cargarCitas = cargarCitas;
window.mostrarModalCita = mostrarModalCita;



// ============================================
// 4. citas.js - Módulo de Citas
// ============================================
// const Citas = {
//   async cargar(fecha = null) {
//     try {
//       let url = '/Citas';
//       if (fecha) url += `?fecha=${encodeURIComponent(fecha)}`;
      
//       const res = await Utils.fetchAPI(url);
//       if (!res.ok) throw new Error('Error al cargar citas');
      
//       const items = await res.json();
//       const tbody = Utils.q('tablaCitas');
      
//       if (!tbody) return;
      
//       if (!items || items.length === 0) {
//         tbody.innerHTML = `<tr><td colspan="7" style="text-align:center">No se encontraron citas</td></tr>`;
//         return;
//       }

//       tbody.innerHTML = items.map(c => {
//         const dt = new Date(c.fechaHora);
//         const estado = Utils.getEstadoBadge(c.estado || 'Pendiente');
//         return `
//           <tr>
//             <td>${c.id}</td>
//             <td>${dt.toLocaleString('es-DO')}</td>
//             <td>${c.dueñoNombre || '-'}</td>
//             <td>${c.veterinarioNombre || '-'}</td>
//             <td>${c.motivo || '-'}</td>
//             <td>${estado}</td>
//             <td>
//               <button class="btn-action btn-edit" onclick="Citas.editar(${c.id})">Editar</button>
//               <button class="btn-action btn-delete" onclick="Citas.eliminar(${c.id})">Cancelar</button>
//             </td>
//           </tr>
//         `;
//       }).join('');
//     } catch (err) {
//       console.error(err);
//       if (Utils.q('tablaCitas')) {
//         Utils.q('tablaCitas').innerHTML = `<tr><td colspan="7" style="text-align:center;color:#EF4444">Error al cargar datos</td></tr>`;
//       }
//     }
//   },

//   async mostrarModal(cita = null) {
//     let estados = [], motivos = [], duenos = [], veterinarios = [];
    
//     try {
//       const [r1, r2, r3] = await Promise.all([
//         Utils.fetchAPI('/Estados'),
//         Utils.fetchAPI('/MotivosCita'),
//         Utils.fetchAPI('/Clientes')
//       ]);
      
//       estados = r1.ok ? await r1.json() : [];
//       motivos = r2.ok ? await r2.json() : [];
//       duenos = r3.ok ? await r3.json() : [];
      
//       // TODO: Endpoint de veterinarios
//       veterinarios = [
//         { id: 1, nombre: 'Dr. Juan Pérez' },
//         { id: 2, nombre: 'Dra. Ana Torres' }
//       ];
//     } catch (e) {
//       console.warn('Error cargando datos de cita', e);
//     }

//     const html = `
//       <div class="form-group">
//         <label>Fecha y Hora</label>
//         <input id="inputFechaHora" type="datetime-local" class="form-control" 
//                value="${cita ? new Date(cita.fechaHora).toISOString().slice(0,16) : ''}" required>
//       </div>
//       <div class="form-group">
//         <label>Cliente</label>
//         <select id="inputDuenoCita" class="form-control" required>
//           <option value="">Seleccionar...</option>
//           ${duenos.map(d => `<option value="${d.id}" ${cita?.dueñoId === d.id ? 'selected' : ''}>${d.nombre} ${d.apellido}</option>`).join('')}
//         </select>
//       </div>
//       <div class="form-group">
//         <label>Veterinario</label>
//         <select id="inputVeterinario" class="form-control" required>
//           <option value="">Seleccionar...</option>
//           ${veterinarios.map(v => `<option value="${v.id}" ${cita?.veterinarioId === v.id ? 'selected' : ''}>${v.nombre}</option>`).join('')}
//         </select>
//       </div>
//       <div class="form-group">
//         <label>Motivo</label>
//         <select id="inputMotivo" class="form-control" required>
//           <option value="">Seleccionar...</option>
//           ${motivos.map(m => `<option value="${m.id}" ${cita?.motivoId === m.id ? 'selected' : ''}>${m.nombreMotivo || m.motivo || 'Motivo'}</option>`).join('')}
//         </select>
//       </div>
//       <div class="form-group">
//         <label>Estado</label>
//         <select id="inputEstado" class="form-control" required>
//           ${estados.map(e => `<option value="${e.id}" ${cita?.estadoId === e.id ? 'selected' : ''}>${e.nombreEstado || e.nombre || e.estado}</option>`).join('')}
//         </select>
//       </div>
//     `;

//     Utils.abrirModal(
//       cita ? 'Editar Cita' : 'Nueva Cita',
//       html,
//       () => Citas.guardar(cita?.id)
//     );
//   },

//   async guardar(id = null) {
//     const payload = {
//       fechaHora: new Date(Utils.q('inputFechaHora').value).toISOString(),
//       dueñoId: Number(Utils.q('inputDuenoCita').value),
//       veterinarioId: Number(Utils.q('inputVeterinario').value),
//       motivoId: Number(Utils.q('inputMotivo').value),
//       estadoId: Number(Utils.q('inputEstado').value)
//     };

//     try {
//       const res = await Utils.fetchAPI(
//         id ? `/Citas/${id}` : '/Citas',
//         {
//           method: id ? 'PUT' : 'POST',
//           body: JSON.stringify(payload)
//         }
//       );

//       if (!res.ok) throw new Error('Error guardando cita');
      
//       Utils.cerrarModal();
//       Citas.cargar();
//     } catch (e) {
//       alert('Error: ' + e.message);
//     }
//   },

//   async editar(id) {
//     try {
//       const res = await Utils.fetchAPI(`/Citas/${id}`);
//       if (!res.ok) throw new Error('No encontrado');
      
//       const cita = await res.json();
//       Citas.mostrarModal(cita);
//     } catch (e) {
//       alert('No se pudo cargar la cita');
//     }
//   },

//   async eliminar(id) {
//     if (!confirm('¿Estás seguro?')) return;
    
//     try {
//       const res = await Utils.fetchAPI(`/Citas/${id}`, { method: 'DELETE' });
//       if (!res.ok) throw new Error('Error al eliminar');
      
//       Citas.cargar();
//     } catch (e) {
//       alert('Error: ' + e.message);
//     }
//   }
// };

// window.Citas = Citas;