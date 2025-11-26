const API_MASCOTAS = `${window.API_URL}/Mascotas`;
const API_TIPOS = `${window.API_URL}/TipoMascota`;
const API_DUENOS = `${window.API_URL}/Clientes`;
const token = window.token;

let mascotaEditando = null;
let tiposMascota = [];
let duenos = [];

// ========== CARGAR MASCOTAS ==========
async function cargarMascotas(filtro = '') {
    try {
        let url = API_MASCOTAS;
        if (filtro) url += `?nombre=${encodeURIComponent(filtro)}`;

        const response = await fetch(url, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Error al cargar mascotas');

        const mascotas = await response.json();
        const tbody = document.getElementById('tablaMascotas');
        
        if (mascotas.length === 0) {
            tbody.innerHTML = '<tr><td colspan="8" style="text-align: center;">No se encontraron mascotas</td></tr>';
            return;
        }

        tbody.innerHTML = mascotas.map(m => `
            <tr>
                <td>${m.id}</td>
                <td>${m.nombre}</td>
                <td>${m.tipoMascota}</td>
                <td>${m.edad} años</td>
                <td>${m.peso} kg</td>
                <td>${m.dueñoNombre}</td>
                <td>${m.estaCastrado ? '✅' : '❌'}</td>
                <td>
                    <button class="btn-action btn-edit" onclick="editarMascota(${m.id})">Editar</button>
                    <button class="btn-action btn-delete" onclick="eliminarMascota(${m.id})">Eliminar</button>
                </td>
            </tr>
        `).join('');
    } catch (error) {
        console.error('Error:', error);
        document.getElementById('tablaMascotas').innerHTML = `
            <tr><td colspan="8" style="text-align: center; color: #EF4444;">Error al cargar datos</td></tr>
        `;
    }
}

// ========== BUSCAR MASCOTA ==========
document.getElementById('btnBuscarMascota').addEventListener('click', () => {
    const filtro = document.getElementById('buscarMascota').value.trim();
    cargarMascotas(filtro);
});

document.getElementById('buscarMascota').addEventListener('keypress', (e) => {
    if (e.key === 'Enter') {
        e.preventDefault();
        document.getElementById('btnBuscarMascota').click();
    }
});

// ========== AGREGAR MASCOTA ==========
document.getElementById('btnAgregarMascota').addEventListener('click', async () => {
    mascotaEditando = null;
    await cargarDatosFormulario();
    mostrarModalMascota();
});

async function cargarDatosFormulario() {
    try {
        // Cargar tipos de mascota
        const resTipos = await fetch(API_TIPOS, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        tiposMascota = await resTipos.json();

        // Cargar dueños
        const resDuenos = await fetch(API_DUENOS, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        duenos = await resDuenos.json();
    } catch (error) {
        console.error('Error cargando datos:', error);
        alert('Error al cargar datos necesarios para el formulario');
    }
}

function mostrarModalMascota(mascota = null) {
    const modal = document.getElementById('modal');
    const modalTitle = document.getElementById('modalTitle');
    const modalFormContent = document.getElementById('modalFormContent');

    modalTitle.textContent = mascota ? 'Editar Mascota' : 'Nueva Mascota';
    
    modalFormContent.innerHTML = `
        <div class="form-group">
            <label>Nombre</label>
            <input type="text" id="inputNombreMascota" class="form-control" value="${mascota?.nombre || ''}" required>
        </div>
        <div class="form-group">
            <label>Tipo de Mascota</label>
            <select id="inputTipoMascota" class="form-control" required>
                <option value="">Seleccionar...</option>
                ${tiposMascota.map(t => `
                    <option value="${t.id}" ${mascota?.tipoMascotaId === t.id ? 'selected' : ''}>
                        ${t.tipo}
                    </option>
                `).join('')}
            </select>
        </div>
        <div class="form-group">
            <label>Dueño</label>
            <select id="inputDueno" class="form-control" required>
                <option value="">Seleccionar...</option>
                ${duenos.map(d => `
                    <option value="${d.id}" ${mascota?.dueñoId === d.id ? 'selected' : ''}>
                        ${d.nombre} ${d.apellido} (${d.cedula})
                    </option>
                `).join('')}
            </select>
        </div>
        <div class="form-group">
            <label>Edad (años)</label>
            <input type="number" id="inputEdad" class="form-control" value="${mascota?.edad || ''}" min="0" required>
        </div>
        <div class="form-group">
            <label>Peso (kg)</label>
            <input type="number" id="inputPeso" class="form-control" value="${mascota?.peso || ''}" min="0" step="0.01" required>
        </div>
        <div class="form-group">
            <label>
                <input type="checkbox" id="inputCastrado" ${mascota?.estaCastrado ? 'checked' : ''}>
                ¿Está castrado?
            </label>
        </div>
    `;

    modal.classList.add('active');

    // Configurar submit del formulario
    const form = document.getElementById('modalForm');
    form.onsubmit = async (e) => {
        e.preventDefault();
        await guardarMascota();
    };
}

async function guardarMascota() {
    const data = {
        nombre: document.getElementById('inputNombreMascota').value.trim(),
        tipoMascotaId: parseInt(document.getElementById('inputTipoMascota').value),
        dueñoId: parseInt(document.getElementById('inputDueno').value),
        edad: parseInt(document.getElementById('inputEdad').value),
        peso: parseFloat(document.getElementById('inputPeso').value),
        estaCastrado: document.getElementById('inputCastrado').checked
    };

    try {
        let response;
        
        if (mascotaEditando) {
            // Actualizar
            response = await fetch(`${API_MASCOTAS}/${mascotaEditando.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(data)
            });
        } else {
            // Crear
            response = await fetch(API_MASCOTAS, {
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
            throw new Error(error.error || 'Error al guardar mascota');
        }

        alert(mascotaEditando ? 'Mascota actualizada exitosamente' : 'Mascota creada exitosamente');
        cerrarModal();
        cargarMascotas();

    } catch (error) {
        alert('Error: ' + error.message);
        console.error('Error:', error);
    }
}

// ========== EDITAR MASCOTA ==========
async function editarMascota(id) {
    try {
        await cargarDatosFormulario();
        
        const response = await fetch(`${API_MASCOTAS}/${id}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Mascota no encontrada');

        const mascota = await response.json();
        mascotaEditando = mascota;
        mostrarModalMascota(mascota);

    } catch (error) {
        alert('Error al cargar mascota: ' + error.message);
    }
}

// ========== ELIMINAR MASCOTA ==========
async function eliminarMascota(id) {
    if (!confirm('¿Estás seguro de eliminar esta mascota?')) return;

    try {
        const response = await fetch(`${API_MASCOTAS}/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error('Error al eliminar mascota');

        alert('Mascota eliminada exitosamente');
        cargarMascotas();

    } catch (error) {
        alert('Error: ' + error.message);
    }
}

// ========== CERRAR MODAL ==========
function cerrarModal() {
    document.getElementById('modal').classList.remove('active');
    mascotaEditando = null;
}

// ========== HACER FUNCIONES GLOBALES ==========
window.cargarMascotas = cargarMascotas;
window.editarMascota = editarMascota;
window.eliminarMascota = eliminarMascota;