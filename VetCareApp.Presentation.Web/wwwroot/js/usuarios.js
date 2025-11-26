document.addEventListener("DOMContentLoaded", () => {
  // Logout
  const logoutBtn = document.getElementById("logoutBtn");
  if (logoutBtn) {
    logoutBtn.addEventListener("click", () => {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      window.location.href = "index.html";
    });
  }

  // Protección de páginas
  const token = localStorage.getItem("token");
  const currentPage = window.location.pathname.split("/").pop();
  if (!token && currentPage !== "index.html" && currentPage !== "registro.html") {
    window.location.href = "index.html";
  }

  // =======================
  // Productos Front-End
  // =======================
  const productos = [
    { nombre: "Croquetas para Gatos", categoria: "gatos", tipo: "comida", img: "https://www.whiskas.com.mx/cdn-cgi/image/format=auto,q=90/sites/g/files/fnmzdf4861/files/2023-04/70646002928.H.png" },
    { nombre: "Collar para Perro", categoria: "perros", tipo: "collares", img: "https://images.unsplash.com/photo-1592194996308-7b43878e84a6?w=200" },
    { nombre: "Vitaminas para Aves", categoria: "aves", tipo: "vitaminas", img: "https://images.unsplash.com/photo-1592194996308-7b43878e84a6?w=200" },
    { nombre: "Shampoo para Conejos", categoria: "conejos", tipo: "shampoo", img: "https://images.unsplash.com/photo-1592194996308-7b43878e84a6?w=200" },
    { nombre: "Cama para Perros", categoria: "perros", tipo: "camas", img: "https://images.unsplash.com/photo-1592194996308-7b43878e84a6?w=200" },
    { nombre: "Plato para Gatos", categoria: "gatos", tipo: "platos", img: "https://images.unsplash.com/photo-1592194996308-7b43878e84a6?w=200" },
    { nombre: "Transportador para Perros", categoria: "perros", tipo: "transportadores", img: "https://images.unsplash.com/photo-1592194996308-7b43878e84a6?w=200" },
    { nombre: "Juguete para Gatos", categoria: "gatos", tipo: "juguetes", img: "https://images.unsplash.com/photo-1592194996308-7b43878e84a6?w=200" }
  ];

  const productosGrid = document.getElementById("productos-grid");
  const filtrosBtns = document.querySelectorAll(".filtro-btn");

  function renderProductos(filtro = "todo") {
    productosGrid.innerHTML = "";
    const filtrados = filtro === "todo" ? productos : productos.filter(p => p.tipo === filtro);
    filtrados.forEach(p => {
      const card = document.createElement("div");
      card.className = "producto-card";
      card.innerHTML = `
        <img src="${p.img}" alt="${p.nombre}">
        <h4>${p.nombre}</h4>
        <p>Categoria: ${p.categoria}</p>
        <p>Tipo: ${p.tipo}</p>
        <button onclick="alert('Agregado al carrito: ${p.nombre}')">Agregar al Carrito</button>
      `;
      productosGrid.appendChild(card);
    });
  }

  renderProductos(); // cargar todos al inicio

  filtrosBtns.forEach(btn => {
    btn.addEventListener("click", () => {
      filtrosBtns.forEach(b => b.classList.remove("active"));
      btn.classList.add("active");
      renderProductos(btn.dataset.tipo);
    });
  });
});
