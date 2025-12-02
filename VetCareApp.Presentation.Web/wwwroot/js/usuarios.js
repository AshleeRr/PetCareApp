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
    { nombre: "Collar para Perro", categoria: "perros", tipo: "collares", img: "https://petmarket.do/wp-content/uploads/2023/12/DOGLINE_ROJO.jpg" },
    { nombre: "Vitaminas para Aves", categoria: "aves", tipo: "vitaminas", img: "https://images.squarespace-cdn.com/content/v1/5cd05048c46f6d1994f32d60/1595428550041-7H7J76GK0E7PRM5JQCEP/Ornivit+S.jpg" },
    { nombre: "Shampoo para Conejos", categoria: "conejos", tipo: "shampoo", img: "https://www.famavet.com/img/shampoConejos1g.jpg" },
    { nombre: "Cama para Perros", categoria: "perros", tipo: "camas", img: "https://m.media-amazon.com/images/I/71GlSpiJuyL._AC_SL1500_.jpg" },
    { nombre: "Plato para Gatos", categoria: "gatos", tipo: "platos", img: "https://veterinariaelcountry.com/wp-content/uploads/COMEDERO-CARA-DE-GATO1.jpg" },
    { nombre: "Transportador para Perros", categoria: "perros", tipo: "transportadores", img: "https://i5.walmartimages.com/asr/07a17b19-3910-49bb-83fc-222c3f547654.6c00067060a1ca0d56d4aa4bbf0a848f.jpeg" },
    { nombre: "Juguete para Gatos", categoria: "gatos", tipo: "juguetes", img: "https://wongfood.vtexassets.com/arquivos/ids/516950-800-auto?v=637789953707370000&width=800&height=auto&aspect=true" }
  ];

  const productosGrid = document.getElementById("productos-grid");

  // ============================
  // NUEVO: filtros separados
  // ============================
  const filtrosBtns = document.querySelectorAll(".filtro-btn"); // tipos (comida, collares...)
  const categoriaBtns = document.querySelectorAll(".categoria-item"); // gatos, perros...

  let filtroCategoria = "todo";
  let filtroTipo = "todo";

  // ============================
  // Render de productos (MEJORADO)
  // ============================
  function renderProductos() {
    productosGrid.innerHTML = "";

    let filtrados = productos;

    // 1️⃣ Filtrar primero por CATEGORÍA
    if (filtroCategoria !== "todo") {
      filtrados = filtrados.filter(p => p.categoria === filtroCategoria);
    }

    // 2️⃣ Luego por TIPO dentro de esa categoría
    if (filtroTipo !== "todo") {
      filtrados = filtrados.filter(p => p.tipo === filtroTipo);
    }

    // Render Final
    filtrados.forEach(p => {
      const card = document.createElement("div");
      card.className = "producto-card";
      card.innerHTML = `
        <img src="${p.img}" alt="${p.nombre}" style="
          width: 100%;
          height: 150px;
          object-fit: contain;
          object-position: center;
          background-color: #fff;
          border-radius: 8px;
        ">
        <h4>${p.nombre}</h4>
        <p>Categoria: ${p.categoria}</p>
        <p>Tipo: ${p.tipo}</p>
        <button onclick="alert('Agregado al carrito: ${p.nombre}')">Agregar al Carrito</button>
      `;
      productosGrid.appendChild(card);
    });
  }

  // ============================
  // Cargar al inicio
  // ============================
  renderProductos();

  // ============================
  // Filtro de TIPO (comida, camas...)
  // ============================
  filtrosBtns.forEach(btn => {
    btn.addEventListener("click", () => {
      filtrosBtns.forEach(b => b.classList.remove("active"));
      btn.classList.add("active");

      filtroTipo = btn.dataset.tipo;

      renderProductos();
    });
  });

  // ============================
  // Filtro de CATEGORÍA (gatos, perros...)
  // ============================
  categoriaBtns.forEach(btn => {
    btn.addEventListener("click", () => {
      categoriaBtns.forEach(c => c.classList.remove("active"));
      btn.classList.add("active");

      filtroCategoria = btn.dataset.categoria;

      // Resetear tipo al cambiar categoría
      filtroTipo = "todo";
      filtrosBtns.forEach(b => b.classList.remove("active"));

      renderProductos();
    });
  });

  // ======================================
  // ⭐ NUEVO: BOTÓN "LEER MÁS" FUNCIONAL ⭐
  // ======================================
  const btnLeerMas = document.querySelector(".btn-leer-mas");
  const contenido = document.querySelector("#sobre .sobre-contenido");

  if (btnLeerMas && contenido) {
    const textoExtra = document.createElement("p");
    textoExtra.classList.add("texto-extra");
    textoExtra.style.display = "none";
    textoExtra.innerHTML = `
      Además, en PetCare buscamos mantenernos a la vanguardia en tecnología veterinaria,
      invirtiendo constantemente en equipos modernos y capacitación para nuestro personal.
      Nuestro objetivo es convertirnos en un centro de referencia donde cada mascota reciba
      un trato digno, cálido y especializado.
    `;

    contenido.appendChild(textoExtra);

    btnLeerMas.addEventListener("click", () => {
      const visible = textoExtra.style.display === "block";

      if (visible) {
        textoExtra.style.display = "none";
        btnLeerMas.textContent = "Leer Más";
      } else {
        textoExtra.style.display = "block";
        btnLeerMas.textContent = "Leer Menos";
      }
    });
  }

});
