const API_CLIENTES = `${window.API_URL}/Clientes`;
const token = window.token;

let clienteEditando = null;

// ========== CARGAR CLIENTES ==========
async function cargarClientes(filtro = '') {
    try {
        let url = API_CLIENTES;
        if (filtro) url += `?nombre=${encodeURIComponent(filtro)}`;

        const response = await fetch(url, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Error al cargar clientes');

        const clientes = await response.json();
        const tbody = document.getElementById('tablaClientes');
        
        if (clientes.length === 0) {
            tbody.innerHTML = '<tr><td colspan="7" style="text-align: center;">No se encontraron clientes</td></tr>';
            return;
        }

        tbody.innerHTML = clientes.map(c => `
            <tr>
                <td>${c.id}</td>
                <td>${c.nombre}</td>
                <td>${c.apellido}</td>
                <td>${c.cedula}</td>
                <td>${c.direccion || '—'}</td>
                <td>${c.email || '—'}</td>
                <td>
                    <button class="btn-action btn-edit" onclick="editarCliente(${c.id})">Editar</button>
                    <button class="btn-action btn-delete" onclick="eliminarCliente(${c.id})">Eliminar</button>
                </td>
            </tr>
        `).join('');
    } catch (error) {
        console.error('Error:', error);
        document.getElementById('tablaClientes').innerHTML = `
            <tr><td colspan="7" style="text-align: center; color: #EF4444;">Error al cargar datos</td></tr>
        `;
    }
}

// ========== BUSCAR CLIENTE ==========
document.getElementById('btnBuscarCliente').addEventListener('click', () => {
    const filtro = document.getElementById('buscarCliente').value.trim();
    cargarClientes(filtro);
});

document.getElementById('buscarCliente').addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        e.preventDefault();
        document.getElementById('btnBuscarCliente').click();
    }
});

// ========== AGREGAR CLIENTE ==========
document.getElementById('btnAgregarCliente').addEventListener('click', () => {
    clienteEditando = null;
    mostrarModalCliente();
});

function mostrarModalCliente(cliente = null) {
    const modal = document.getElementById('modal');
    const modalTitle = document.getElementById('modalTitle');
    const modalFormContent = document.getElementById('modalFormContent');

    modalTitle.textContent = cliente ? 'Editar Cliente' : 'Nuevo Cliente';
    
    modalFormContent.innerHTML = `
        <div class="form-group">
            <label>Nombre</label>
            <input type="text" id="inputNombre" class="form-control" value="${cliente?.nombre || ''}" required>
        </div>
        <div class="form-group">
            <label>Apellido</label>
            <input type="text" id="inputApellido" class="form-control" value="${cliente?.apellido || ''}" required>
        </div>
        <div class="form-group">
            <label>Cédula</label>
            <input type="text" id="inputCedula" class="form-control" value="${cliente?.cedula || ''}" ${cliente ? 'disabled' : 'required'}>
        </div>
        <div class="form-group">
            <label>Dirección</label>
            <input type="text" id="inputDireccion" class="form-control" value="${cliente?.direccion || ''}">
        </div>
        <div class="form-group">
            <label>Email</label>
            <input type="email" id="inputEmail" class="form-control" value="${cliente?.email || ''}" required>
        </div>
    `;

    modal.classList.add('active');

    // Configurar submit del formulario
    const form = document.getElementById('modalForm');
    form.onsubmit = async (e) => {
        e.preventDefault();
        await guardarCliente();
    };
}

async function guardarCliente() {
    const data = {
        nombre: document.getElementById('inputNombre').value.trim(),
        apellido: document.getElementById('inputApellido').value.trim(),
        cedula: document.getElementById('inputCedula').value.trim(),
        direccion: document.getElementById('inputDireccion').value.trim(),
        email: document.getElementById('inputEmail').value.trim()
    };

    try {
        let response;
        
        if (clienteEditando) {
            // Actualizar
            response = await fetch(`${API_CLIENTES}/${clienteEditando.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(data)
            });
        } else {
            // Crear
            response = await fetch(API_CLIENTES, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(data)
            });
        }

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.error || 'Error al guardar cliente');
        }

        alert(clienteEditando ? 'Cliente actualizado exitosamente' : 'Cliente creado exitosamente');
        cerrarModal();
        cargarClientes();

    } catch (error) {
        alert('Error: ' + error.message);
        console.error('Error:', error);
    }
}

// ========== EDITAR CLIENTE ==========
async function editarCliente(id) {
    try {
        const response = await fetch(`${API_CLIENTES}/${id}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Cliente no encontrado');

        const cliente = await response.json();
        clienteEditando = cliente;
        mostrarModalCliente(cliente);

    } catch (error) {
        alert('Error al cargar cliente: ' + error.message);
    }
}

// ========== ELIMINAR CLIENTE ==========
async function eliminarCliente(id) {
    if (!confirm('¿Estás seguro de eliminar este cliente?')) return;

    try {
        const response = await fetch(`${API_CLIENTES}/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Error al eliminar cliente');

        alert('Cliente eliminado exitosamente');
        cargarClientes();

    } catch (error) {
        alert('Error: ' + error.message);
    }
}

// ========== CERRAR MODAL ==========
function cerrarModal() {
    document.getElementById('modal').classList.remove('active');
    clienteEditando = null;
}

document.getElementById('btnCloseModal').addEventListener('click', cerrarModal);
document.getElementById('modal').addEventListener('click', (e) => {
    if (e.target.id === 'modal') cerrarModal();
});

// ========== HACER FUNCIONES GLOBALES ==========
window.cargarClientes = cargarClientes;
window.editarCliente = editarCliente;
window.eliminarCliente = eliminarCliente;