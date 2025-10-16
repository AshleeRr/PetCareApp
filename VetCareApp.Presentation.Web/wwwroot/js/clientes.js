const API_URL = 'https://localhost:7071/api/Clientes';
const tbody = document.querySelector('#tablaClientes tbody');
const btnBuscar = document.getElementById('btnBuscar');
const inputBuscar = document.getElementById('buscarNombre');

async function cargarClientes(nombre = '') {
  try {
    let url = API_URL;
    if (nombre) url += `/filtrar?nombre=${encodeURIComponent(nombre)}`;

    const res = await fetch(url);
    const clientes = await res.json();

    tbody.innerHTML = '';
    clientes.forEach(c => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${c.id}</td>
        <td>${c.nombre}</td>
        <td>${c.cedula}</td>
        <td>${c.telefono || '—'}</td>
        <td>
          <button onclick="editarCliente(${c.id})">Editar</button>
          <button onclick="eliminarCliente(${c.id})">Eliminar</button>
        </td>
      `;
      tbody.appendChild(tr);
    });
  } catch (err) {
    console.error('Error cargando clientes:', err);
  }
}

btnBuscar.addEventListener('click', () => {
  cargarClientes(inputBuscar.value);
});

async function eliminarCliente(id) {
  if (!confirm('¿Seguro que quieres eliminar este cliente?')) return;
  try {
    await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
    cargarClientes();
  } catch (err) {
    console.error('Error al eliminar cliente:', err);
  }
}

// Inicializar
document.addEventListener('DOMContentLoaded', () => {
  cargarClientes();
});
