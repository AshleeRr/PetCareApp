function initProductos() {
    console.log("🟢 Productos cargados");

    cargarProductos();

    const btnBuscar = document.getElementById("btnBuscarProducto");
    if (btnBuscar) {
        btnBuscar.addEventListener("click", () => {
            const nombre = document.getElementById("filtroNombre").value || "";
            const tipo = document.getElementById("filtroTipo").value || "";
            filtrarProductos(nombre, tipo);
        });
    }
}

function cargarProductos() {
    fetch("/api/Productos")
        .then(r => r.json())
        .then(data => renderTablaProductos(data))
        .catch(err => console.log(err));
}

function filtrarProductos(nombre, tipo) {
    fetch(`/api/Productos?nombre=${nombre}&tipoId=${tipo}`)
        .then(r => r.json())
        .then(data => renderTablaProductos(data))
        .catch(err => console.log("Error filtrando:", err));
}

function renderTablaProductos(lista) {
    const tbody = document.getElementById("tablaProductosBody");
    if (!tbody) return;

    tbody.innerHTML = "";

    lista.forEach(p => {
        const tr = document.createElement("tr");

        tr.innerHTML = `
            <td>${p.id}</td>
            <td>${p.nombre}</td>
            <td>${p.stock}</td>
            <td>${p.precio}</td>
            <td>${p.tipoProducto}</td>
        `;

        tbody.appendChild(tr);
    });
}
