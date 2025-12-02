
// ============================================
// 6. inventario.js - Módulo de Inventario
// ============================================
const Inventario = {
  async cargar(fecha = null) {
    try {
      let url = '/Inventario';
      if (fecha) url += `?fecha=${encodeURIComponent(fecha)}`;
      
      const res = await Utils.fetchAPI(url);
      if (!res.ok) throw new Error('Error al cargar inventario');
      
      const items = await res.json();
      const tbody = Utils.q('tablaInventario');
      
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
      if (Utils.q('tablaInventario')) {
        Utils.q('tablaInventario').innerHTML = `<tr><td colspan="7" style="text-align:center;color:#EF4444">Error al cargar inventario</td></tr>`;
      }
    }
  }
};

window.Inventario = Inventario;