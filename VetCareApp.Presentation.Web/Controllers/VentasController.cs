using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using System.Security.Claims;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentasController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        private int GetUsuarioId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            return int.Parse(userIdClaim.Value);
        }

        private string GetUsuarioRole()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            return roleClaim?.Value ?? "Cliente";
        }

        // ========================================
        // ENDPOINTS PARA CLIENTES
        // ========================================

        /// <summary>
        /// Cliente: Obtener mis compras
        /// Admin/Recepcionista: Obtener todas las ventas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentas(
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta)
        {
            try
            {
                var role = GetUsuarioRole();

                if (role == "Cliente")
                {
                    // Cliente solo ve sus propias compras
                    var usuarioId = GetUsuarioId();
                    var ventas = await _ventaService.ObtenerPorUsuarioAsync(usuarioId);
                    return Ok(ventas);
                }
                else
                {
                    // Admin y Recepcionista ven todas
                    if (desde.HasValue && hasta.HasValue)
                    {
                        var ventas = await _ventaService.ObtenerPorRangoFechaAsync(desde.Value, hasta.Value);
                        return Ok(ventas);
                    }

                    var todasVentas = await _ventaService.ObtenerTodasAsync();
                    return Ok(todasVentas);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener venta por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>> GetById(int id)
        {
            try
            {
                var venta = await _ventaService.ObtenerPorIdAsync(id);

                if (venta == null)
                    return NotFound(new { mensaje = "Venta no encontrada" });

                // Cliente solo puede ver sus propias ventas
                var role = GetUsuarioRole();
                if (role == "Cliente")
                {
                    var usuarioId = GetUsuarioId();
                    var misVentas = await _ventaService.ObtenerPorUsuarioAsync(usuarioId);

                    if (!misVentas.Any(v => v.Id == id))
                        return Forbid(); // 403 Forbidden
                }

                return Ok(venta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Cliente: Confirmar compra desde el carrito
        /// </summary>
        [HttpPost("confirmar-compra")]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult<VentaDto>> ConfirmarCompra([FromBody] CrearVentaDesdeCarritoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usuarioId = GetUsuarioId();
                var venta = await _ventaService.CrearVentaDesdeCarritoAsync(usuarioId, dto);

                return CreatedAtAction(nameof(GetById), new { id = venta.Id }, new
                {
                    mensaje = "Compra realizada exitosamente",
                    venta
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // ========================================
        // ENDPOINTS PARA ADMIN Y RECEPCIONISTA
        // ========================================

        /// <summary>
        /// Crear venta de mostrador (productos, citas, recetas)
        /// </summary>
        [HttpPost("mostrador")]
        [Authorize(Roles = "Admin,Recepcionista")]
        public async Task<ActionResult<VentaDto>> CrearVentaMostrador([FromBody] CrearVentaMostradorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var venta = await _ventaService.CrearVentaMostradorAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = venta.Id }, new
                {
                    mensaje = "Venta registrada exitosamente",
                    venta
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtener ventas por rango de fecha (reportes)
        /// </summary>
        [HttpGet("reportes/por-fecha")]
        [Authorize(Roles = "Admin,Recepcionista")]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetVentasPorFecha(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            try
            {
                if (desde > hasta)
                    return BadRequest(new { mensaje = "La fecha 'desde' no puede ser mayor que 'hasta'" });

                var ventas = await _ventaService.ObtenerPorRangoFechaAsync(desde, hasta);

                var totalVentas = ventas.Sum(v => v.Total);

                return Ok(new
                {
                    ventas,
                    resumen = new
                    {
                        cantidadVentas = ventas.Count(),
                        totalVendido = totalVentas,
                        desde,
                        hasta
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Cancelar venta (solo Admin)
        /// </summary>
        [HttpPatch("{id}/cancelar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CancelarVenta(int id)
        {
            try
            {
                var resultado = await _ventaService.CancelarVentaAsync(id);

                if (!resultado)
                    return NotFound(new { mensaje = "Venta no encontrada o ya cancelada" });

                return Ok(new { mensaje = "Venta cancelada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }
    }
}
