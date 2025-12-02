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

