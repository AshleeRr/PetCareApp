// const API_URL = "https://localhost:7164/api";
// let veterinarioId = null;
// let todasLasCitas = [];
// let todasLasMascotas = [];
// let medicamentosDisponibles = [];
// let pruebasMedicas = [];
// let clientesCache = new Map();
// let mascotasCache = new Map();

// // ========================================================
// // INICIALIZACIÓN
// // ========================================================
// document.addEventListener("DOMContentLoaded", async () => {
//   verificarAutenticacion();
//   await cargarDatosIniciales();
// });

// function verificarAutenticacion() {
//   const user = JSON.parse(localStorage.getItem("user") || "{}");
//   const token = localStorage.getItem("token");
  
//   if (!token || user.role?.toLowerCase() !== "veterinario") {
//     window.location.href = "login.html";
//     return;
//   }
  
//   veterinarioId = user.id;
//   document.getElementById("vet-nombre").textContent = user.name || "Veterinario";
//   document.getElementById("vet-email").textContent = user.email || "";
// }

// async function cargarDatosIniciales() {
//   await Promise.all([
//     cargarCitas(),
//     cargarMedicamentos(),
//     cargarPruebasMedicas()
//   ]);
//   await cargarMascotas();
//   actualizarEstadisticas();
// }

// // ========================================================
// // CITAS
// // ========================================================
// async function cargarCitas() {
//   try {
//     const response = await fetchAPI(`${API_URL}/Citas/veterinario/${veterinarioId}`);
    
//     if (!response || response.length === 0) {
//       todasLasCitas = [];
//       mostrarCitas([]);
//       return;
//     }
    
//     // Filtrar solo las del día de hoy
//     const hoy = new Date().toDateString();
//     todasLasCitas = response.filter(c => {
//       const citaFecha = new Date(c.fechaHora).toDateString();
//       return citaFecha === hoy;
//     });
    
//     // Enriquecer los datos de las citas
//     todasLasCitas = await Promise.all(todasLasCitas.map(async (cita) => {
//       // El backend devuelve strings, necesitamos extraer información
//       // Asumimos que "mascota" contiene el nombre y "cliente" el nombre del dueño
      
//       return {
//         ...cita,
//         nombreMascota: cita.mascota || 'Desconocida',
//         nombreCliente: cita.cliente || 'Desconocido',
//         motivoCita: cita.motivo || '-',
//         // Intentamos extraer IDs si están en formato "ID:Nombre" o similar
//         // Si no, usaremos el hash del nombre como ID temporal
//         mascotaId: extraerIdDeCadena(cita.mascota) || hashString(cita.mascota),
//         clienteId: extraerIdDeCadena(cita.cliente) || hashString(cita.cliente)
//       };
//     }));
    
//     mostrarCitas(todasLasCitas);
//   } catch (error) {
//     console.error("Error al cargar citas:", error);
//     document.getElementById("loading-citas").textContent = "Error al cargar citas";
//   }
// }

// // Función auxiliar para intentar extraer ID de una cadena como "5:Juan Perez"
// function extraerIdDeCadena(str) {
//   if (!str) return null;
//   const match = str.match(/^(\d+):/);
//   return match ? parseInt(match[1]) : null;
// }

// // Función para generar un hash simple de un string
// function hashString(str) {
//   if (!str) return 0;
//   let hash = 0;
//   for (let i = 0; i < str.length; i++) {
//     const char = str.charCodeAt(i);
//     hash = ((hash << 5) - hash) + char;
//     hash = hash & hash;
//   }
//   return Math.abs(hash);
// }

// function mostrarCitas(citas) {
//   const loading = document.getElementById("loading-citas");
//   const tabla = document.getElementById("tabla-citas");
//   const tbody = document.getElementById("tbody-citas");
//   const empty = document.getElementById("empty-citas");
  
//   loading.style.display = "none";
  
//   if (!citas || citas.length === 0) {
//     tabla.style.display = "none";
//     empty.style.display = "block";
//     return;
//   }
  
//   tabla.style.display = "table";
//   empty.style.display = "none";
  
//   tbody.innerHTML = citas.map(cita => {
//     const fecha = new Date(cita.fechaHora);
//     const hora = fecha.toLocaleTimeString('es-ES', { hour: '2-digit', minute: '2-digit' });
//     const estadoClass = cita.estado === 'Completada' ? 'badge-completed' : 
//                        cita.estado === 'Cancelada' ? 'badge-cancelled' : 'badge-pending';
    
//     return `
//       <tr>
//         <td><strong>${hora}</strong></td>
//         <td>${cita.nombreMascota}</td>
//         <td>${cita.nombreCliente}</td>
//         <td>${cita.motivoCita}</td>
//         <td><span class="badge ${estadoClass}">${cita.estado}</span></td>
//         <td>
//           ${cita.estado === 'Pendiente' ? 
//             `<button class="btn btn-success" onclick="atenderCita(${cita.id}, '${cita.nombreMascota}', '${cita.mascota}')">Atender</button>` : 
//             `<button class="btn btn-info" onclick="verHistorial('${cita.mascota}', '${cita.nombreMascota}')">Ver Historial</button>`
//           }
//         </td>
//       </tr>
//     `;
//   }).join('');
// }

// function filtrarCitas(estado) {
//   if (estado === 'all') {
//     mostrarCitas(todasLasCitas);
//   } else {
//     const filtradas = todasLasCitas.filter(c => c.estado === estado);
//     mostrarCitas(filtradas);
//   }
// }

// // ========================================================
// // MASCOTAS
// // ========================================================
// async function cargarMascotas() {
//   try {
//     // Obtener IDs únicos de mascotas de las citas
//     const mascotasNombres = [...new Set(todasLasCitas.map(c => c.mascota))];
    
//     if (mascotasNombres.length === 0) {
//       document.getElementById("loading-mascotas").textContent = "No hay mascotas asignadas";
//       return;
//     }
    
//     // Obtener todas las mascotas del sistema
//     const todasMascotasAPI = await fetchAPI(`${API_URL}/Mascotas`);
    
//     // Filtrar solo las mascotas que están en las citas
//     todasLasMascotas = todasMascotasAPI.filter(m => 
//       mascotasNombres.includes(m.nombre)
//     );
    
//     // Si no encontramos por nombre, crear datos temporales
//     if (todasLasMascotas.length === 0) {
//       todasLasMascotas = mascotasNombres.map((nombre, index) => ({
//         id: hashString(nombre),
//         nombre: nombre,
//         tipoMascota: '-',
//         raza: '-',
//         edad: '-',
//         nombreCliente: todasLasCitas.find(c => c.mascota === nombre)?.nombreCliente || '-'
//       }));
//     }
    
//     mostrarMascotas(todasLasMascotas);
//   } catch (error) {
//     console.error("Error al cargar mascotas:", error);
//     document.getElementById("loading-mascotas").textContent = "Error al cargar mascotas";
//   }
// }

// function mostrarMascotas(mascotas) {
//   const loading = document.getElementById("loading-mascotas");
//   const tabla = document.getElementById("tabla-mascotas");
//   const tbody = document.getElementById("tbody-mascotas");
  
//   loading.style.display = "none";
//   tabla.style.display = "table";
  
//   tbody.innerHTML = mascotas.map(m => `
//     <tr>
//       <td><strong>${m.nombre}</strong></td>
//       <td>${m.tipoMascota || '-'}</td>
//       <td>${m.raza || '-'}</td>
//       <td>${m.edad || '-'} años</td>
//       <td>${m.nombreCliente || '-'}</td>
//       <td>
//         <button class="btn btn-info" onclick="verHistorial('${m.nombre}', '${m.nombre}')">Historial</button>
//       </td>
//     </tr>
//   `).join('');
// }

// function buscarMascota() {
//   const texto = document.getElementById("search-mascota").value.toLowerCase();
//   const filtradas = todasLasMascotas.filter(m => 
//     m.nombre.toLowerCase().includes(texto)
//   );
//   mostrarMascotas(filtradas);
// }

// // ========================================================
// // ATENDER CITA
// // ========================================================
// function atenderCita(citaId, nombreMascota, mascotaNombreBackend) {
//   document.getElementById("cita-id").value = citaId;
//   document.getElementById("mascota-id").value = mascotaNombreBackend; // Guardamos el string del backend
//   document.getElementById("info-cita").innerHTML = `
//     <div style="background: #e7f3ff; padding: 15px; border-radius: 8px; margin-bottom: 20px;">
//       <h3>Mascota: ${nombreMascota}</h3>
//       <p>Cita ID: ${citaId}</p>
//     </div>
//   `;
//   abrirModal("modal-atender");
// }

// document.getElementById("crear-receta").addEventListener("change", (e) => {
//   document.getElementById("receta-section").style.display = e.target.checked ? "block" : "none";
// });

// let contadorMedicamentos = 0;

// function agregarMedicamento() {
//   contadorMedicamentos++;
//   const div = document.createElement("div");
//   div.className = "form-group";
//   div.id = `medicamento-${contadorMedicamentos}`;
//   div.innerHTML = `
//     <select class="med-select" required>
//       <option value="">-- Seleccionar medicamento --</option>
//       ${medicamentosDisponibles.map(m => `<option value="${m.id}">${m.nombre}</option>`).join('')}
//     </select>
//     <input type="text" class="med-dosis" placeholder="Dosis (ej: 1 tableta cada 8 horas)" required>
//     <button type="button" class="btn btn-warning" onclick="eliminarMedicamento(${contadorMedicamentos})">Eliminar</button>
//   `;
//   document.getElementById("medicamentos-list").appendChild(div);
// }

// function eliminarMedicamento(id) {
//   document.getElementById(`medicamento-${id}`).remove();
// }

// async function cargarMedicamentos() {
//   try {
//     medicamentosDisponibles = await fetchAPI(`${API_URL}/Medicamento`);
//   } catch (error) {
//     console.error("Error al cargar medicamentos:", error);
//     medicamentosDisponibles = [];
//   }
// }

// async function cargarPruebasMedicas() {
//   try {
//     const select = document.getElementById("prueba-medica");
//     pruebasMedicas = ["Hemograma", "Radiografía", "Ecografía", "Análisis de orina"];
//     pruebasMedicas.forEach(p => {
//       const option = document.createElement("option");
//       option.value = p;
//       option.textContent = p;
//       select.appendChild(option);
//     });
//   } catch (error) {
//     console.error("Error al cargar pruebas:", error);
//   }
// }

// document.getElementById("form-consulta").addEventListener("submit", async (e) => {
//   e.preventDefault();
  
//   const citaId = parseInt(document.getElementById("cita-id").value);
//   const mascotaNombre = document.getElementById("mascota-id").value;
//   const diagnostico = document.getElementById("diagnostico").value;
//   const tratamiento = document.getElementById("tratamiento").value;
//   const pruebaMedica = document.getElementById("prueba-medica").value;
//   const crearReceta = document.getElementById("crear-receta").checked;
  
//   try {
//     // Obtener la cita actual para mantener los datos
//     const citaActual = await fetchAPI(`${API_URL}/Citas/${citaId}`);
    
//     // Actualizar estado de la cita - usando observaciones para guardar diagnóstico/tratamiento
//     await fetchAPI(`${API_URL}/Citas/${citaId}`, {
//       method: "PUT",
//       body: JSON.stringify({
//         fechaHora: citaActual.fechaHora,
//         estado: "Completada",
//         dueñoId: citaActual.dueñoId || 0,
//         mascotaId: citaActual.mascotaId || 0,
//         motivoId: citaActual.motivoId || 0,
//         observaciones: `Diagnóstico: ${diagnostico}\nTratamiento: ${tratamiento}`
//       })
//     });

//     // Crear receta si es necesario
//     if (crearReceta) {
//       try {
//         const receta = await fetchAPI(`${API_URL}/Receta`, {
//           method: "POST",
//           body: JSON.stringify({
//             citaId: citaId,
//             fechaEmision: new Date().toISOString(),
//             instrucciones: tratamiento
//           })
//         });

//         // Agregar medicamentos
//         const medicamentos = document.querySelectorAll("#medicamentos-list > div");
//         for (const div of medicamentos) {
//           const medId = parseInt(div.querySelector(".med-select").value);
//           const dosis = div.querySelector(".med-dosis").value;
          
//           if (medId && dosis) {
//             await fetchAPI(`${API_URL}/Receta/agregar-medicamento`, {
//               method: "POST",
//               body: JSON.stringify({
//                 recetaId: receta.id,
//                 medicamentoId: medId,
//                 dosis: dosis
//               })
//             });
//           }
//         }
//       } catch (recetaError) {
//         console.error("Error al crear receta:", recetaError);
//         // Continuar aunque falle la receta
//       }
//     }

//     // Crear prueba médica si fue seleccionada
//     if (pruebaMedica) {
//       try {
//         // Buscar el ID real de la mascota por nombre
//         const todasMascotasAPI = await fetchAPI(`${API_URL}/Mascotas`);
//         const mascotaEncontrada = todasMascotasAPI.find(m => m.nombre === mascotaNombre);
        
//         if (mascotaEncontrada) {
//           await fetchAPI(`${API_URL}/MascotaPruebaMedica`, {
//             method: "POST",
//             body: JSON.stringify({
//               mascotaId: mascotaEncontrada.id,
//               nombrePrueba: pruebaMedica,
//               fechaRealizacion: new Date().toISOString(),
//               resultado: "Pendiente"
//             })
//           });
//         }
//       } catch (pruebaError) {
//         console.error("Error al crear prueba médica:", pruebaError);
//         // Continuar aunque falle la prueba
//       }
//     }

//     alert("✅ Consulta registrada exitosamente");
//     cerrarModal("modal-atender");
//     await cargarCitas();
//     actualizarEstadisticas();
    
//     // Limpiar formulario
//     document.getElementById("form-consulta").reset();
//     document.getElementById("medicamentos-list").innerHTML = "";
//     document.getElementById("receta-section").style.display = "none";
//     contadorMedicamentos = 0;
    
//   } catch (error) {
//     console.error("Error al guardar consulta:", error);
//     alert("❌ Error al guardar la consulta: " + error.message);
//   }
// });

// // ========================================================
// // HISTORIAL
// // ========================================================
// async function verHistorial(mascotaNombre, nombreMascotaDisplay) {
//   document.getElementById("info-mascota-historial").innerHTML = `
//     <div style="background: #e7f3ff; padding: 15px; border-radius: 8px; margin-bottom: 20px;">
//       <h3>${nombreMascotaDisplay}</h3>
//     </div>
//   `;
  
//   abrirModal("modal-historial");
  
//   try {
//     // Buscar todas las citas de esta mascota por nombre
//     const todasCitas = await fetchAPI(`${API_URL}/Citas`);
//     const citasMascota = todasCitas.filter(c => c.mascota === mascotaNombre);
    
//     const tbody = document.getElementById("tbody-historial");
//     const loading = document.getElementById("loading-historial");
//     const tabla = document.getElementById("tabla-historial");
    
//     loading.style.display = "none";
//     tabla.style.display = "table";
    
//     const citasCompletadas = citasMascota.filter(c => c.estado === "Completada");
    
//     if (citasCompletadas.length === 0) {
//       tbody.innerHTML = '<tr><td colspan="4" style="text-align: center;">No hay historial disponible</td></tr>';
//       return;
//     }
    
//     tbody.innerHTML = await Promise.all(citasCompletadas.map(async (cita) => {
//       const fecha = new Date(cita.fechaHora).toLocaleDateString('es-ES');
      
//       // Extraer diagnóstico y tratamiento de observaciones
//       let diagnostico = '-';
//       let tratamiento = '-';
      
//       if (cita.observaciones) {
//         const diagMatch = cita.observaciones.match(/Diagnóstico:\s*(.+?)(?:\n|$)/);
//         const tratMatch = cita.observaciones.match(/Tratamiento:\s*(.+?)$/);
        
//         if (diagMatch) diagnostico = diagMatch[1].trim();
//         if (tratMatch) tratamiento = tratMatch[1].trim();
//       }
      
//       // Intentar obtener recetas
//       let recetaInfo = "No";
//       try {
//         const recetas = await fetchAPI(`${API_URL}/Receta/cita/${cita.id}`);
//         if (recetas && recetas.length > 0) {
//           recetaInfo = "Sí";
//         }
//       } catch (e) {
//         // No hay recetas
//       }
      
//       return `
//         <tr>
//           <td>${fecha}</td>
//           <td>${diagnostico}</td>
//           <td>${tratamiento}</td>
//           <td>${recetaInfo}</td>
//         </tr>
//       `;
//     })).then(rows => rows.join(''));
    
//   } catch (error) {
//     console.error("Error al cargar historial:", error);
//     document.getElementById("loading-historial").textContent = "Error al cargar historial";
//   }
// }

// // ========================================================
// // ESTADÍSTICAS
// // ========================================================
// function actualizarEstadisticas() {
//   document.getElementById("stat-citas-hoy").textContent = todasLasCitas.length;
//   document.getElementById("stat-mascotas").textContent = todasLasMascotas.length;
//   document.getElementById("stat-pendientes").textContent = 
//     todasLasCitas.filter(c => c.estado === "Pendiente").length;
// }

// // ========================================================
// // UTILIDADES
// // ========================================================
// async function fetchAPI(url, options = {}) {
//   const token = localStorage.getItem("token");
//   const defaultOptions = {
//     headers: {
//       "Content-Type": "application/json",
//       "Authorization": `Bearer ${token}`
//     }
//   };
  
//   const response = await fetch(url, { ...defaultOptions, ...options });
  
//   if (!response.ok) {
//     throw new Error(`HTTP error! status: ${response.status}`);
//   }
  
//   return await response.json();
// }

// function abrirModal(id) {
//   document.getElementById(id).classList.add("active");
// }

// function cerrarModal(id) {
//   document.getElementById(id).classList.remove("active");
// }

// function cerrarSesion() {
//   localStorage.removeItem("token");
//   localStorage.removeItem("user");
//   window.location.href = "login.html";
// }

// // Cerrar modales al hacer clic fuera
// window.onclick = (e) => {
//   if (e.target.classList.contains("modal")) {
//     e.target.classList.remove("active");
//   }
// };


// js/veterinario.js - CÓDIGO FINAL CORREGIDO Y FUNCIONAL

const API_URL = "https://localhost:7164/api";
let veterinarioId = null;
let todasLasCitas = [];
let todasLasMascotas = [];
let medicamentosDisponibles = [];
// Asumimos que las pruebas médicas son fijas o no requieren un fetch inicial
let pruebasMedicas = ["Hemograma", "Radiografía", "Ecografía", "Análisis de orina"]; 
let contadorMedicamentos = 0; // Contador global para IDs únicos de recetas

// ========================================================
// INICIALIZACIÓN Y AUTENTICACIÓN
// ========================================================
document.addEventListener("DOMContentLoaded", async () => {
    verificarAutenticacion();
    if (veterinarioId) {
        await cargarDatosIniciales();
    }
});

function verificarAutenticacion() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    const token = localStorage.getItem("token");

    if (!token || user.role?.toLowerCase() !== "veterinario") {
        window.location.href = "login.html";
        return;
    }

    veterinarioId = user.id;
    document.getElementById("vet-nombre").textContent = user.name || "Veterinario";
    document.getElementById("vet-email").textContent = user.email || "";
}

async function cargarDatosIniciales() {
    await Promise.all([
        cargarCitas(),
        cargarMedicamentos()
    ]);
    await cargarMascotas();
    actualizarEstadisticas();
    cargarPruebasMedicasSelect(); // Llenar el select con pruebas fijas
}

// ========================================================
// CITAS (CORREGIDO PARA USAR IDs Y filtrar por hoy)
// ========================================================
async function cargarCitas() {
    try {
        // Usamos el ID del veterinario autenticado
        const urlCitas = `${API_URL}/Citas/veterinario/${veterinarioId}`;
        const response = await fetchAPI(urlCitas);
        
        if (!response || response.length === 0) {
            todasLasCitas = [];
            mostrarCitas([]);
            return;
        }

        const hoy = new Date().toDateString();
        
        // Mapeamos y filtramos para enriquecer los datos y quedarnos solo con las de hoy
        todasLasCitas = response
            .filter(c => new Date(c.fechaHora).toDateString() === hoy)
            .map(cita => ({
                ...cita,
                // Usamos campos del backend:
                nombreMascota: cita.mascota || cita.mascotaNombre || 'Desconocida',
                nombreCliente: cita.cliente || cita.dueñoNombre || 'Desconocido',
                motivoCita: cita.motivo || cita.motivoNombre || '-',
                mascotaId: cita.mascotaId || null, 
                clienteId: cita.dueñoId || cita.clienteId || null 
            }));

        mostrarCitas(todasLasCitas);
    } catch (error) {
        console.error("Error al cargar citas:", error);
        document.getElementById("loading-citas").textContent = "Error al cargar citas";
    }
}

function mostrarCitas(citas) {
    const loading = document.getElementById("loading-citas");
    const tabla = document.getElementById("tabla-citas");
    const tbody = document.getElementById("tbody-citas");
    const empty = document.getElementById("empty-citas");

    loading.style.display = "none";

    if (!citas || citas.length === 0) {
        tabla.style.display = "none";
        empty.style.display = "block";
        return;
    }

    tabla.style.display = "table";
    empty.style.display = "none";

    tbody.innerHTML = citas.map(cita => {
        const fecha = new Date(cita.fechaHora);
        const hora = fecha.toLocaleTimeString('es-ES', { hour: '2-digit', minute: '2-digit' });
        const estadoClass = cita.estado === 'Completada' ? 'badge-completed' : 
                           cita.estado === 'Cancelada' ? 'badge-cancelled' : 'badge-pending';
        
        // Usamos el ID de la mascota si existe, si no el nombre (como fallback)
        const idMascota = cita.mascotaId || cita.nombreMascota; 
        
        return `
            <tr>
                <td><strong>${hora}</strong></td>
                <td>${cita.nombreMascota}</td>
                <td>${cita.nombreCliente}</td>
                <td>${cita.motivoCita}</td>
                <td><span class="badge ${estadoClass}">${cita.estado}</span></td>
                <td>
                    ${cita.estado === 'Pendiente' ? 
                        `<button class="btn btn-success" onclick="atenderCita(${cita.id}, '${cita.nombreMascota}', '${idMascota}')">Atender</button>` : 
                        `<button class="btn btn-info" onclick="verHistorial('${idMascota}', '${cita.nombreMascota}')">Ver Historial</button>`
                    }
                </td>
            </tr>
        `;
    }).join('');
}

function filtrarCitas(estado) {
    if (estado === 'all') {
        mostrarCitas(todasLasCitas);
    } else {
        const filtradas = todasLasCitas.filter(c => c.estado === estado);
        mostrarCitas(filtradas);
    }
}

// ========================================================
// MASCOTAS (CORREGIDO PARA USAR ID de las citas)
// ========================================================
async function cargarMascotas() {
    try {
        const mascotasIdsUnicos = [...new Set(todasLasCitas.map(c => c.mascotaId).filter(id => id !== null))];
        
        if (mascotasIdsUnicos.length === 0) {
            document.getElementById("loading-mascotas").textContent = "No hay mascotas asignadas hoy";
            todasLasMascotas = [];
            return;
        }

        const todasMascotasAPI = await fetchAPI(`${API_URL}/Mascotas`);
        
        todasLasMascotas = todasMascotasAPI.filter(m => 
            mascotasIdsUnicos.includes(m.id)
        );
        
        // Enriquecer datos de mascotas con nombre de cliente (si se encuentra en las citas de hoy)
        todasLasMascotas = todasLasMascotas.map(mascota => {
            const citaAsignada = todasLasCitas.find(c => c.mascotaId === mascota.id);
            return {
                ...mascota,
                nombreCliente: citaAsignada?.nombreCliente || '-'
            };
        });
        
        mostrarMascotas(todasLasMascotas);
    } catch (error) {
        console.error("Error al cargar mascotas:", error);
        document.getElementById("loading-mascotas").textContent = "Error al cargar mascotas";
    }
}

function mostrarMascotas(mascotas) {
    const loading = document.getElementById("loading-mascotas");
    const tabla = document.getElementById("tabla-mascotas");
    const tbody = document.getElementById("tbody-mascotas");

    loading.style.display = "none";
    tabla.style.display = "table";

    tbody.innerHTML = mascotas.map(m => `
        <tr>
            <td><strong>${m.nombre}</strong></td>
            <td>${m.tipoMascota || m.tipo || '-'}</td>
            <td>${m.raza || '-'}</td>
            <td>${m.edad || '-'} años</td>
            <td>${m.nombreCliente || '-'}</td>
            <td>
                <button class="btn btn-info" onclick="verHistorial('${m.id || m.nombre}', '${m.nombre}')">Historial</button>
            </td>
        </tr>
    `).join('');
}

function buscarMascota() {
    const texto = document.getElementById("search-mascota").value.toLowerCase();
    const filtradas = todasLasMascotas.filter(m => 
        m.nombre.toLowerCase().includes(texto)
    );
    mostrarMascotas(filtradas);
}

// ========================================================
// ATENDER CITA / RECETA
// ========================================================
async function cargarMedicamentos() {
    try {
        medicamentosDisponibles = await fetchAPI(`${API_URL}/Medicamento`);
    } catch (error) {
        console.error("Error al cargar medicamentos:", error);
        medicamentosDisponibles = [];
    }
}

function cargarPruebasMedicasSelect() {
    const select = document.getElementById("prueba-medica");
    select.innerHTML = '<option value="">-- Seleccionar prueba --</option>';
    pruebasMedicas.forEach(p => {
        const option = document.createElement("option");
        option.value = p;
        option.textContent = p;
        select.appendChild(option);
    });
}

function atenderCita(citaId, nombreMascota, mascotaIdOString) {
    document.getElementById("cita-id").value = citaId;
    document.getElementById("mascota-id").value = mascotaIdOString; 
    document.getElementById("info-cita").innerHTML = `
        <div style="background: #e7f3ff; padding: 15px; border-radius: 8px; margin-bottom: 20px;">
            <h3>Mascota: ${nombreMascota}</h3>
            <p>Cita ID: ${citaId}</p>
        </div>
    `;
    abrirModal("modal-atender");
}

document.getElementById("crear-receta").addEventListener("change", (e) => {
    document.getElementById("receta-section").style.display = e.target.checked ? "block" : "none";
});

// Función exportada (CORREGIDA LA EXPORTACIÓN)
function agregarMedicamento() {
    contadorMedicamentos++;
    const div = document.createElement("div");
    div.className = "form-group medicamento-item";
    div.id = `medicamento-${contadorMedicamentos}`;
    div.innerHTML = `
        <select class="med-select" required>
            <option value="">-- Seleccionar medicamento --</option>
            ${medicamentosDisponibles.map(m => `<option value="${m.id}">${m.nombre}</option>`).join('')}
        </select>
        <input type="text" class="med-dosis" placeholder="Dosis (ej: 1 tableta cada 8 horas)" required>
        <button type="button" class="btn btn-warning" onclick="eliminarMedicamento(${contadorMedicamentos})">Eliminar</button>
    `;
    document.getElementById("medicamentos-list").appendChild(div);
}

// Función exportada (CORREGIDA LA EXPORTACIÓN)
function eliminarMedicamento(id) {
    document.getElementById(`medicamento-${id}`).remove();
}

document.getElementById("form-consulta").addEventListener("submit", async (e) => {
    e.preventDefault();
    
    const citaId = parseInt(document.getElementById("cita-id").value);
    const mascotaIdOString = document.getElementById("mascota-id").value;
    const diagnostico = document.getElementById("diagnostico").value;
    const tratamiento = document.getElementById("tratamiento").value;
    const pruebaMedica = document.getElementById("prueba-medica").value;
    const crearReceta = document.getElementById("crear-receta").checked;

    try {
        const citaActual = await fetchAPI(`${API_URL}/Citas/${citaId}`);
        
        let mascotaIdNum = !isNaN(parseInt(mascotaIdOString)) && isFinite(mascotaIdOString) ? parseInt(mascotaIdOString) : citaActual.mascotaId || 0;

        // Actualizar estado de la cita
        await fetchAPI(`${API_URL}/Citas/${citaId}`, {
            method: "PUT",
            body: JSON.stringify({
                fechaHora: citaActual.fechaHora,
                estado: "Completada",
                dueñoId: citaActual.dueñoId || 0,
                mascotaId: citaActual.mascotaId || 0,
                motivoId: citaActual.motivoId || 0,
                veterinarioId: citaActual.veterinarioId || 0,
                observaciones: `Diagnóstico: ${diagnostico}\nTratamiento: ${tratamiento}`
            })
        });

        // Crear receta
        if (crearReceta) {
             const receta = await fetchAPI(`${API_URL}/Receta`, {
                method: "POST",
                body: JSON.stringify({
                    citaId: citaId,
                    fechaEmision: new Date().toISOString(),
                    instrucciones: tratamiento
                })
            });

            const medicamentos = document.querySelectorAll("#medicamentos-list > div");
            for (const div of medicamentos) {
                const medId = parseInt(div.querySelector(".med-select").value);
                const dosis = div.querySelector(".med-dosis").value;
                if (medId && dosis) {
                    await fetchAPI(`${API_URL}/Receta/agregar-medicamento`, {
                        method: "POST",
                        body: JSON.stringify({
                            recetaId: receta.id,
                            medicamentoId: medId,
                            dosis: dosis
                        })
                    });
                }
            }
        }

        // Crear prueba médica
        if (pruebaMedica && mascotaIdNum) {
            await fetchAPI(`${API_URL}/MascotaPruebaMedica`, {
                method: "POST",
                body: JSON.stringify({
                    mascotaId: mascotaIdNum,
                    nombrePrueba: pruebaMedica,
                    fechaRealizacion: new Date().toISOString(),
                    resultado: "Pendiente"
                })
            });
        }

        alert("✅ Consulta registrada exitosamente");
        cerrarModal("modal-atender");
        await cargarCitas();
        actualizarEstadisticas();
        
        // Limpiar formulario y contador
        document.getElementById("form-consulta").reset();
        document.getElementById("medicamentos-list").innerHTML = "";
        document.getElementById("receta-section").style.display = "none";
        contadorMedicamentos = 0;
        
    } catch (error) {
        console.error("Error al guardar consulta:", error);
        alert("❌ Error al guardar la consulta: " + error.message);
    }
});


// ========================================================
// HISTORIAL
// ========================================================
async function verHistorial(mascotaIdOString, nombreMascotaDisplay) {
    document.getElementById("info-mascota-historial").innerHTML = `
        <div style="background: #e7f3ff; padding: 15px; border-radius: 8px; margin-bottom: 20px;">
            <h3>${nombreMascotaDisplay}</h3>
        </div>
    `;

    abrirModal("modal-historial");
    
    const tbody = document.getElementById("tbody-historial");
    const loading = document.getElementById("loading-historial");
    const tabla = document.getElementById("tabla-historial");
    
    loading.style.display = "block";
    tabla.style.display = "none";
    tbody.innerHTML = '';

    try {
        const todasCitas = await fetchAPI(`${API_URL}/Citas`);

        // Filtramos por ID numérico (si es un número) o por nombre (si es un string)
        const isNumericId = !isNaN(parseInt(mascotaIdOString)) && isFinite(mascotaIdOString);
        
        const citasMascota = todasCitas.filter(c => {
            if (isNumericId) {
                return c.mascotaId === parseInt(mascotaIdOString);
            }
            return c.mascota === mascotaIdOString;
        });

        loading.style.display = "none";
        tabla.style.display = "table";

        const citasCompletadas = citasMascota.filter(c => c.estado === "Completada");

        if (citasCompletadas.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4" style="text-align: center;">No hay historial disponible</td></tr>';
            return;
        }

        tbody.innerHTML = await Promise.all(citasCompletadas.map(async (cita) => {
            const fecha = new Date(cita.fechaHora).toLocaleDateString('es-ES');
            
            let diagnostico = '-';
            let tratamiento = '-';
            
            if (cita.observaciones) {
                const diagMatch = cita.observaciones.match(/Diagnóstico:\s*(.+?)(?:\n|$)/);
                const tratMatch = cita.observaciones.match(/Tratamiento:\s*(.+?)$/);
                
                if (diagMatch) diagnostico = diagMatch[1].trim();
                if (tratMatch) tratamiento = tratMatch[1].trim();
            }
            
            let recetaInfo = "No";
            try {
                const recetas = await fetchAPI(`${API_URL}/Receta/cita/${cita.id}`);
                if (recetas && recetas.length > 0) {
                    recetaInfo = "Sí";
                }
            } catch (e) { /* Error al obtener recetas */ }
            
            return `
                <tr>
                    <td>${fecha}</td>
                    <td>${diagnostico}</td>
                    <td>${tratamiento}</td>
                    <td>${recetaInfo}</td>
                </tr>
            `;
        })).then(rows => rows.join(''));
        
    } catch (error) {
        console.error("Error al cargar historial:", error);
        document.getElementById("loading-historial").textContent = "Error al cargar historial";
    }
}

// ========================================================
// ESTADÍSTICAS
// ========================================================
function actualizarEstadisticas() {
    document.getElementById("stat-citas-hoy").textContent = todasLasCitas.length;
    document.getElementById("stat-mascotas").textContent = todasLasMascotas.length;
    document.getElementById("stat-pendientes").textContent = 
        todasLasCitas.filter(c => c.estado === "Pendiente").length;
}

// ========================================================
// UTILIDADES Y EXPORTACIONES GLOBALES
// ========================================================
async function fetchAPI(url, options = {}) {
    const token = localStorage.getItem("token");
    const defaultOptions = {
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    };
    
    const response = await fetch(url, { ...defaultOptions, ...options });
    
    if (!response.ok) {
        const errorText = await response.text();
        // Lanzamos un error más detallado para debug
        throw new Error(`HTTP error! status: ${response.status}. Detalle: ${errorText}`);
    }
    
    // Intenta devolver el JSON, si falla (ej. respuesta vacía), devuelve null
    try {
        return await response.json();
    } catch (e) {
        return null;
    }
}

function abrirModal(id) {
    document.getElementById(id).classList.add("active");
}

function cerrarModal(id) {
    document.getElementById(id).classList.remove("active");
}

function cerrarSesion() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    window.location.href = "login.html";
}

// Cerrar modales al hacer clic fuera
window.onclick = (e) => {
    if (e.target.classList.contains("modal")) {
        e.target.classList.remove("active");
    }
};

// EXPORTACIONES GLOBALES (¡CORRECCIÓN CLAVE!)
// Hacemos que todas las funciones llamadas desde onclick="..." sean globales (parte de window)

window.atenderCita = atenderCita;
window.verHistorial = verHistorial;
window.filtrarCitas = filtrarCitas;
window.buscarMascota = buscarMascota;
window.cerrarSesion = cerrarSesion; 
window.agregarMedicamento = agregarMedicamento; // Exportado para onclick
window.eliminarMedicamento = eliminarMedicamento; // Exportado para onclick