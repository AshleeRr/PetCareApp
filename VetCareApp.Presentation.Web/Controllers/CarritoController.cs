using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using System.Security.Claims;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        // Método helper para obtener el ID del usuario autenticado
        private int GetUsuarioId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            return int.Parse(userIdClaim.Value);
        }

        /// <summary>
        /// Obtener el carrito del usuario actual
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<CarritoDto>> GetMiCarrito()
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var carrito = await _carritoService.ObtenerCarritoUsuarioAsync(usuarioId);
                return Ok(carrito);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Agregar producto al carrito
        /// </summary>
        [HttpPost("items")]
        public async Task<ActionResult<CarritoDto>> AgregarProducto([FromBody] AgregarAlCarritoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usuarioId = GetUsuarioId();
                var carrito = await _carritoService.AgregarProductoAsync(usuarioId, dto);

                return Ok(new
                {
                    mensaje = "Producto agregado al carrito",
                    carrito
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Actualizar cantidad de un item del carrito
        /// </summary>
        [HttpPut("items/{itemId}")]
        public async Task<ActionResult<CarritoDto>> ActualizarCantidad(
            int itemId,
            [FromBody] ActualizarCarritoItemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usuarioId = GetUsuarioId();
                var carrito = await _carritoService.ActualizarCantidadAsync(usuarioId, itemId, dto);

                return Ok(new
                {
                    mensaje = "Cantidad actualizada",
                    carrito
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Eliminar un item del carrito
        /// </summary>
        [HttpDelete("items/{itemId}")]
        public async Task<ActionResult> EliminarItem(int itemId)
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var resultado = await _carritoService.EliminarItemAsync(usuarioId, itemId);

                if (!resultado)
                    return NotFound(new { mensaje = "Item no encontrado" });

                return Ok(new { mensaje = "Item eliminado del carrito" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Vaciar todo el carrito
        /// </summary>
        [HttpDelete]
        public async Task<ActionResult> VaciarCarrito()
        {
            try
            {
                var usuarioId = GetUsuarioId();
                var resultado = await _carritoService.VaciarCarritoAsync(usuarioId);

                if (!resultado)
                    return NotFound(new { mensaje = "Carrito no encontrado" });

                return Ok(new { mensaje = "Carrito vaciado exitosamente" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }
    }
}
