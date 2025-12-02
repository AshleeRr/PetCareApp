using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        // ========================================
        // ENDPOINTS PÚBLICOS (Todos los roles)
        // ========================================

        /// <summary>
        /// Obtener todos los productos (Admin y Recepcionista ven todo, Cliente solo disponibles)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
        {
            try
            {
                var productos = await _productoService.ObtenerTodosAsync();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener catálogo de productos disponibles (solo con stock > 0)
        /// </summary>
        [HttpGet("catalogo")]
        [AllowAnonymous] // Permitir acceso sin autenticación para el catálogo público
        public async Task<ActionResult<IEnumerable<ProductoCatalogoDto>>> GetCatalogo()
        {
            try
            {
                var catalogo = await _productoService.ObtenerCatalogoAsync();
                return Ok(catalogo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener producto por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            try
            {
                var producto = await _productoService.ObtenerPorIdAsync(id);

                if (producto == null)
                    return NotFound(new { mensaje = "Producto no encontrado" });

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Filtrar productos por tipo
        /// </summary>
        [HttpGet("tipo/{tipoId}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetByTipo(int tipoId)
        {
            try
            {
                var productos = await _productoService.ObtenerPorTipoAsync(tipoId);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Buscar productos por nombre
        /// </summary>
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> BuscarPorNombre([FromQuery] string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return BadRequest(new { mensaje = "Debe proporcionar un nombre para buscar" });

                var productos = await _productoService.BuscarPorNombreAsync(nombre);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        // ========================================
        // ENDPOINTS ADMINISTRATIVOS (Admin y Recepcionista)
        // ========================================

        /// <summary>
        /// Crear nuevo producto
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Recepcionista")]
        public async Task<ActionResult<ProductoDto>> Create([FromBody] CrearProductoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var producto = await _productoService.CrearProductoAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al crear producto: {ex.Message}" });
            }
        }

        /// <summary>
        /// Actualizar producto existente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Recepcionista")]
        public async Task<ActionResult<ProductoDto>> Update(int id, [FromBody] ActualizarProductoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var producto = await _productoService.ActualizarProductoAsync(id, dto);

                if (producto == null)
                    return NotFound(new { mensaje = "Producto no encontrado" });

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al actualizar producto: {ex.Message}" });
            }
        }

        /// <summary>
        /// Eliminar producto (solo Admin)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _productoService.EliminarProductoAsync(id);

                if (!resultado)
                    return NotFound(new { mensaje = "Producto no encontrado" });

                return Ok(new { mensaje = "Producto eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al eliminar producto: {ex.Message}" });
            }
        }

        /// <summary>
        /// Actualizar stock de un producto
        /// </summary>
        [HttpPatch("{id}/stock")]
        [Authorize(Roles = "Admin,Recepcionista")]
        public async Task<ActionResult> ActualizarStock(int id, [FromBody] ActualizarStockDto dto)
        {
            try
            {
                var resultado = await _productoService.ActualizarStockAsync(id, dto.Cantidad);

                if (!resultado)
                    return BadRequest(new { mensaje = "No se pudo actualizar el stock" });

                return Ok(new { mensaje = "Stock actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al actualizar stock: {ex.Message}" });
            }
        }
    }
}
