namespace VetCareApp.Presentation.Web
{
    public class README
    {
    
    # VetCareApp

## Características principales

- Inicio de sesión con roles: 
  - Usuario normal: Página principal.
  - Recepcionista: Panel de gestión de usuarios.
- Gestión de clientes: crear, editar, eliminar y buscar.
- Conexión al backend ASP.NET Core mediante API REST.
- Diseño responsive y moderno con HTML, CSS y JavaScript.

## Tecnologías utilizadas

### Frontend
- HTML5
- CSS3
- JavaScript

### Backend
- ASP.NET Core (C#)
- Entity Framework Core
- SQL Server

## Estructura del proyecto
VetCareApp/
│
├── index.html                 Página de inicio de sesión
├── dashboard.html             Panel principal (recepcionista o usuario)
│
├── css/
│   └── styles.css             Estilos del proyecto
│
├── js/
│   ├── login.js               Lógica del login y roles
│   └── usuarios.js            Gestión CRUD de usuarios
│
└── assets/
    └── logo.png               Logo del sistema


## Instalación y uso

1. Clona o descarga este repositorio.
2. Asegúrate de tener el backend de ASP.NET Core en ejecución.
3. Abre `index.html` en tu navegador.
4. Inicia sesión con un usuario válido:
   - Usuario normal: será redirigido al panel básico.
   - Recepcionista: podrá acceder al dashboard de administración.

## Endpoints disponibles

- GET `/api/clientes` : Obtener todos los clientes.
- GET `/api/clientes/{id}` : Obtener cliente por ID.
- GET `/api/clientes/cedula/{cedula}` : Buscar cliente por cédula.
- GET `/api/clientes/filtrar?nombre=...` : Filtrar clientes por nombre.
- POST `/api/clientes` : Crear un nuevo cliente.
- PUT `/api/clientes/{id}` : Editar cliente existente.
- DELETE `/api/clientes/{id}` : Eliminar cliente.
    
   }
}
