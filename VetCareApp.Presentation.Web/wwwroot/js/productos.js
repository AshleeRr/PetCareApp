// js/productos.js
const API_PRODUCTOS = `${window.API_URL}/Productos`;
const API_BASE = window.API_URL;
const token = window.token;
const q = window.q;
const on = window.on;
const abrirModal = window.abrirModal;
const cerrarModal = window.cerrarModal;
const attachTableButtons = window.attachTableButtons;
const safeJSON = window.safeJSON;

async function cargarTiposProducto() {
  try {
    const res = await fetch(`${API_BASE}/TipoProducto`, { headers: { Authorization: `Bearer ${token}` }});
    return res.ok ? await res.json() : [];
  } catch (e) { return []; }
}

/* ===========================
    PRODUCTOS (listar, crear, editar, eliminar)
   =========================== */
async function cargarProductos(filtro = '') {
  try {
    let url = API_PRODUCTOS;
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
        res = await fetch(`${API_PRODUCTOS}/${producto.id}`, { method: 'PUT', headers:{'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
      } else {
        res = await fetch(API_PRODUCTOS, { method: 'POST', headers:{'Content-Type':'application/json', Authorization:`Bearer ${token}`}, body: JSON.stringify(payload) });
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

// Handlers de botones/buscadores de Productos
on(q('btnBuscarProducto'), 'click', () => { const f = q('buscarProducto').value.trim(); cargarProductos(f); });
on(q('buscarProducto'), 'keypress', (e) => { if (e.key === 'Enter') { e.preventDefault(); q('btnBuscarProducto').click(); } });
on(q('btnAgregarProducto'), 'click', async () => { await mostrarModalProducto(null); });

// Exportar funciones globales
window.cargarProductos = cargarProductos;
window.mostrarModalProducto = mostrarModalProducto;


// // ============================================
// // 5. productos.js - Módulo de Productos
// // ============================================
// const Productos = {
//   async cargar(filtro = '') {
//     try {
//       let url = '/Productos';

//       if (filtro) {
//         url = `/Productos/buscar?nombre=${encodeURIComponent(filtro)}`;
//       }

//       const res = await Utils.fetchAPI(url);
//       if (!res.ok) throw new Error('Error al cargar productos');

//       const items = await res.json();
//       const tbody = Utils.q('tablaProductos');

//       if (!tbody) return;

//       if (!items || items.length === 0) {
//         tbody.innerHTML = `<tr><td colspan="6" style="text-align:center">No se encontraron productos</td></tr>`;
//         return;
//       }

//       tbody.innerHTML = items.map(p => `
//         <tr>
//           <td>${p.id}</td>
//           <td>${p.nombre}</td>
//           <td>${p.tipoProducto?.tipo || '-'}</td>   <!-- ✅ OBJETO -->
//           <td>${p.stock}</td>
//           <td>RD$ ${Number(p.precio || 0).toFixed(2)}</td>
//           <td>
//             <button class="btn-action btn-edit" onclick="Productos.editar(${p.id})">Editar</button>
//             <button class="btn-action btn-delete" onclick="Productos.eliminar(${p.id})">Eliminar</button>
//           </td>
//         </tr>
//       `).join('');
//     } catch (err) {
//       console.error(err);
//       if (Utils.q('tablaProductos')) {
//         Utils.q('tablaProductos').innerHTML =
//           `<tr><td colspan="6" style="text-align:center;color:#EF4444">Error al cargar productos</td></tr>`;
//       }
//     }
//   },

//   async mostrarModal(producto = null) {
//     let tipos = [];

//     try {
//       const res = await Utils.fetchAPI('/api/TipoProducto');  // ✅ API real
//       tipos = res.ok ? await res.json() : [];
//     } catch (e) {
//       console.warn('No se pudieron cargar tipos de producto', e);
//     }

//     const tipoOptions = tipos.map(t =>
//       `<option value="${t.id}" ${producto?.tipoProductoId === t.id ? 'selected' : ''}>${t.tipo}</option>`
//     ).join('');

//     const html = `
//       <div class="form-group">
//         <label>Nombre</label>
//         <input id="nombreProducto" required value="${producto?.nombre || ''}" class="form-control">
//       </div>
//       <div class="form-group">
//         <label>Tipo</label>
//         <select id="tipoProducto" class="form-control" required>
//           <option value="">Seleccionar...</option>
//           ${tipoOptions}
//         </select>
//       </div>
//       <div class="form-group">
//         <label>Stock</label>
//         <input id="stockProducto" type="number" min="0" value="${producto?.stock ?? 0}" class="form-control">
//       </div>
//       <div class="form-group">
//         <label>Precio</label>
//         <input id="precioProducto" type="number" min="0" step="0.01" value="${producto?.precio ?? 0}" class="form-control">
//       </div>
//     `;

//     Utils.abrirModal(
//       producto ? 'Editar Producto' : 'Nuevo Producto',
//       html,
//       () => Productos.guardar(producto?.id)
//     );
//   },

//   async guardar(id = null) {
//     const payload = {
//       nombre: Utils.q('nombreProducto').value.trim(),
//       tipoProductoId: Number(Utils.q('tipoProducto').value),
//       stock: Number(Utils.q('stockProducto').value),
//       precio: Number(Utils.q('precioProducto').value)
//     };

//     try {
//       const res = await Utils.fetchAPI(
//         id ? `/api/Productos/${id}` : '/api/Productos',
//         {
//           method: id ? 'PUT' : 'POST',
//           body: JSON.stringify(payload)
//         }
//       );

//       if (!res.ok) {
//         const err = await Utils.safeJSON(res);
//         throw new Error(err?.mensaje || 'Error guardando producto');
//       }

//       Utils.cerrarModal();
//       Productos.cargar();
//     } catch (e) {
//       alert('Error: ' + e.message);
//     }
//   },

//   async editar(id) {
//     try {
//       const res = await Utils.fetchAPI(`/api/Productos/${id}`);
//       if (!res.ok) throw new Error('No encontrado');

//       const producto = await res.json();
//       Productos.mostrarModal(producto);
//     } catch (e) {
//       alert('No se pudo cargar el producto');
//     }
//   },

//   async eliminar(id) {
//     if (!confirm('¿Estás seguro?')) return;

//     try {
//       const res = await Utils.fetchAPI(`/api/Productos/${id}`, { method: 'DELETE' });
//       if (!res.ok) throw new Error('Error al eliminar');

//       Productos.cargar();
//     } catch (e) {
//       alert('Error: ' + e.message);
//     }
//   }
// };

// window.Productos = Productos;
