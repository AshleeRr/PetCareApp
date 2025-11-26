const API_CITAS = `${window.API_URL}/Citas`;
const API_ESTADOS = `${window.API_URL}/Estados`;
const API_MOTIVOS = `${window.API_URL}/MotivosCita`;
const API_DUENOS_CITAS = `${window.API_URL}/Clientes`;
const token = window.token;

let citaEditando = null;
let estados = [];
let motivos = [];
let duenos = [];
let veterinarios = []; // Necesitarías un endpoint de Personal filtrado por veterinarios

// ========== CARGAR CITAS ==========
async function cargarCitas(fecha = null) {
    try {
        let url = API_CITAS;
        if (fecha) url += `?fecha=${fecha}`;

        const response = await fetch(url, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Error al cargar citas');

        const citas = await response.json();
        const tbody = document.getElementById('tablaCitas');
        
        if (citas.length === 0) {
            tbody.innerHTML = '<tr><td colspan="7" style="text-align: center;">No se encontraron citas</td></tr>';
            return;
        }

        tbody.innerHTML = citas.map(c => {
            const fecha = new Date(c.fechaHora);
            const estadoBadge = window.getEstadoBadge(c.estado);
            
            return `
                <tr>
                    <td>${c.id}</td>
                    <td>${fecha.toLocaleString('es-DO')}</td>
                    <td>${c.dueñoNombre}</td>
                    <td>${c.veterinarioNombre}</td>
                    <td>${c.motivo}</td>
                    <td>${estadoBadge}</td>
                    <td>
                        <button class="btn-action btn-edit" onclick="editarCita(${c.id})">Editar</button>
                        <button class="btn-action btn-delete" onclick="cancelarCita(${c.id})">Cancelar</button>
                    </td>
                </tr>
            `;
        }).join('');
    } catch (error) {
        console.error('Error:', error);
        document.getElementById('tablaCitas').innerHTML = `
            <tr><td colspan="7" style="text-align: center; color: #EF4444;">Error al cargar datos</td></tr>
        `;
    }
}

// ========== BUSCAR CITA POR FECHA ==========
document.getElementById('btnBuscarCita').addEventListener('click', () => {
    const fecha = document.getElementById('buscarFechaCita').value;
    if (fecha) {
        cargarCitas(fecha);
    } else {
        cargarCitas();
    }
});

// ========== AGREGAR CITA ==========
document.getElementById('btnAgregarCita').addEventListener('click', async () => {
    citaEditando = null;
    await cargarDatosFormularioCita();
    mostrarModalCita();
});

async function cargarDatosFormularioCita() {
    try {
        // Cargar estados
        const resEstados = await fetch(API_ESTADOS, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        estados = await resEstados.json();

        // Cargar motivos
        const resMotivos = await fetch(API_MOTIVOS, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        motivos = await resMotivos.json();

        // Cargar dueños
        const resDuenos = await fetch(API_DUENOS_CITAS, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        duenos = await resDuenos.json();

        // Cargar veterinarios (simulado, deberías tener un endpoint)
        veterinarios = [
            { id: 1, nombre: 'Dr. Juan Pérez' },
            { id: 2, nombre: 'Dra. Ana Torres' }
        ];
    } catch (error) {
        console.error('Error cargando datos:', error);
        alert('Error al cargar datos necesarios para el formulario');
    }
}

function mostrarModalCita(cita = null) {
    const modal = document.getElementById('modal');
    const modalTitle = document.getElementById('modalTitle');
    const modalFormContent = document.getElementById('modalFormContent');

    modalTitle.textContent = cita ? 'Editar Cita' : 'Nueva Cita';
    
    // Formatear fecha para el input datetime-local
    let fechaDefault = '';
    if (cita) {
        const fecha = new Date(cita.fechaHora);
        fechaDefault = fecha.toISOString().slice(0, 16);
    }
    
    modalFormContent.innerHTML = `
        <div class="form-group">
            <label>Fecha y Hora</label>
            <input type="datetime-local" id="inputFechaHora" class="form-control" value="${fechaDefault}" required>
        </div>
        <div class="form-group">
            <label>Cliente</label>
            <select id="inputDuenoCita" class="form-control" required>
                <option value="">Seleccionar...</option>
                ${duenos.map(d => `
                    <option value="${d.id}" ${cita?.dueñoId === d.id ? 'selected' : ''}>
                        ${d.nombre} ${d.apellido}
                    </option>
                `).join('')}
            </select>
        </div>
        <div class="form-group">
            <label>Veterinario</label>
            <select id="inputVeterinario" class="form-control" required>
                <option value="">Seleccionar...</option>
                ${veterinarios.map(v => `
                    <option value="${v.id}" ${cita?.veterinarioId === v.id ? 'selected' : ''}>
                        ${v.nombre}
                    </option>
                `).join('')}
            </select>
        </div>
        <div class="form-group">
            <label>Motivo</label>
            <select id="inputMotivo" class="form-control" required>
                <option value="">Seleccionar...</option>
                ${motivos.map(m => `
                    <option value="${m.id}" ${cita?.motivoId === m.id ? 'selected' : ''}>
                        ${m.motivoCita || m.motivo || 'Motivo'}
                    </option>
                `).join('')}
            </select>
        </div>
        <div class="form-group">
            <label>Estado</label>
            <select id="inputEstado" class="form-control" required>
                ${estados.map(e => `
                    <option value="${e.id}" ${cita?.estadoId === e.id ? 'selected' : ''}>
                        ${e.estado}
                    </option>
                `).join('')}
            </select>
        </div>
    `;

    modal.classList.add('active');

    // Configurar submit del formulario
    const form = document.getElementById('modalForm');
    form.onsubmit = async (e) => {
        e.preventDefault();
        await guardarCita();
    };
}

async function guardarCita() {
    const data = {
        fechaHora: new Date(document.getElementById('inputFechaHora').value).toISOString(),
        dueñoId: parseInt(document.getElementById('inputDuenoCita').value),
        veterinarioId: parseInt(document.getElementById('inputVeterinario').value),
        motivoId: parseInt(document.getElementById('inputMotivo').value),
        estadoId: parseInt(document.getElementById('inputEstado').value)
    };

    try {
        let response;
        
        if (citaEditando) {
            // Actualizar
            response = await fetch(`${API_CITAS}/${citaEditando.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(data)
            });
        } else {
            // Crear
            response = await fetch(API_CITAS, {
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
            throw new Error(error.error || 'Error al guardar cita');
        }

        alert(citaEditando ? 'Cita actualizada exitosamente' : 'Cita creada exitosamente');
        cerrarModal();
        cargarCitas();

    } catch (error) {
        alert('Error: ' + error.message);
        console.error('Error:', error);
    }
}

// ========== EDITAR CITA ==========
async function editarCita(id) {
    try {
        await cargarDatosFormularioCita();
        
        const response = await fetch(`${API_CITAS}/${id}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Cita no encontrada');

        const cita = await response.json();
        citaEditando = cita;
        mostrarModalCita(cita);

    } catch (error) {
        alert('Error al cargar cita: ' + error.message);
    }
}

// ========== CANCELAR CITA ==========
async function cancelarCita(id) {
    if (!confirm('¿Estás seguro de cancelar esta cita?')) return;

    try {
        const response = await fetch(`${API_CITAS}/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Error al cancelar cita');

        alert('Cita cancelada exitosamente');
        cargarCitas();

    } catch (error) {
        alert('Error: ' + error.message);
    }
}

// ========== CERRAR MODAL ==========
function cerrarModal() {
    document.getElementById('modal').classList.remove('active');
    citaEditando = null;
}

// ========== HACER FUNCIONES GLOBALES ==========
window.cargarCitas = cargarCitas;
window.editarCita = editarCita;
window.cancelarCita = cancelarCita;