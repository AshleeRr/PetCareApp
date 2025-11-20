using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // ====================================
        // DASHBOARD
        // ====================================

        /// <summary>
        /// Obtener estadísticas del dashboard
        /// </summary>
        [HttpGet("dashboard/stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
        {
            try
            {
                var stats = await _adminService.ObtenerEstadisticasDashboardAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        // ====================================
        // GESTIÓN DE USUARIOS
        // ====================================

        [HttpGet("usuarios")]
        public async Task<ActionResult<IEnumerable<UsuarioAdminDto>>> GetAllUsuarios()
        {
            try
            {
                var usuarios = await _adminService.ObtenerTodosUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        [HttpGet("usuarios/{id}")]
        public async Task<ActionResult<UsuarioAdminDto>> GetUsuarioById(int id)
        {
            try
            {
                var usuario = await _adminService.ObtenerUsuarioPorIdAsync(id);

                if (usuario == null)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPost("usuarios")]
        public async Task<ActionResult<UsuarioAdminDto>> CreateUsuario([FromBody] CrearUsuarioAdminDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usuario = await _adminService.CrearUsuarioAsync(dto);
                return CreatedAtAction(nameof(GetUsuarioById), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("usuarios/{id}")]
        public async Task<ActionResult<UsuarioAdminDto>> UpdateUsuario(int id, [FromBody] ActualizarUsuarioAdminDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usuario = await _adminService.ActualizarUsuarioAsync(id, dto);

                if (usuario == null)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("usuarios/{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            try
            {
                var resultado = await _adminService.EliminarUsuarioAsync(id);

                if (!resultado)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                return Ok(new { mensaje = "Usuario eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPatch("usuarios/{id}/cambiar-password")]
        public async Task<ActionResult> CambiarPassword(int id, [FromBody] CambiarPasswordDto dto)
        {
            try
            {
                var resultado = await _adminService.CambiarPasswordUsuarioAsync(id, dto);

                if (!resultado)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                return Ok(new { mensaje = "Contraseña actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPatch("usuarios/{id}/toggle-activo")]
        public async Task<ActionResult> ToggleActivo(int id, [FromBody] ToggleActivoDto dto)
        {
            try
            {
                var resultado = await _adminService.ActivarDesactivarUsuarioAsync(id, dto.Activo);

                if (!resultado)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                return Ok(new { mensaje = $"Usuario {(dto.Activo ? "activado" : "desactivado")} exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("usuarios/buscar")]
        public async Task<ActionResult<IEnumerable<UsuarioAdminDto>>> BuscarUsuarios([FromQuery] string termino)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(termino))
                    return BadRequest(new { mensaje = "Debe proporcionar un término de búsqueda" });

                var usuarios = await _adminService.BuscarUsuariosAsync(termino);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        // ====================================
        // GESTIÓN DE PERSONAL
        // ====================================

        [HttpGet("personal")]
        public async Task<ActionResult<IEnumerable<PersonalDto>>> GetAllPersonal()
        {
            try
            {
                var personal = await _adminService.ObtenerTodoPersonalAsync();
                return Ok(personal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        [HttpGet("personal/{id}")]
        public async Task<ActionResult<PersonalDto>> GetPersonalById(int id)
        {
            try
            {
                var personal = await _adminService.ObtenerPersonalPorIdAsync(id);

                if (personal == null)
                    return NotFound(new { mensaje = "Personal no encontrado" });

                return Ok(personal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPost("personal")]
        public async Task<ActionResult<PersonalDto>> CreatePersonal([FromBody] CrearPersonalDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var personal = await _adminService.CrearPersonalAsync(dto);
                return CreatedAtAction(nameof(GetPersonalById), new { id = personal.Id }, personal);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("personal/{id}")]
        public async Task<ActionResult<PersonalDto>> UpdatePersonal(int id, [FromBody] ActualizarPersonalDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var personal = await _adminService.ActualizarPersonalAsync(id, dto);

                if (personal == null)
                    return NotFound(new { mensaje = "Personal no encontrado" });

                return Ok(personal);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("personal/{id}")]
        public async Task<ActionResult> DeletePersonal(int id)
        {
            try
            {
                var resultado = await _adminService.EliminarPersonalAsync(id);

                if (!resultado)
                    return NotFound(new { mensaje = "Personal no encontrado" });

                return Ok(new { mensaje = "Personal eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        [HttpGet("personal/buscar")]
        public async Task<ActionResult<IEnumerable<PersonalDto>>> BuscarPersonal([FromQuery] string termino)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(termino))
                    return BadRequest(new { mensaje = "Debe proporcionar un término de búsqueda" });

                var personal = await _adminService.BuscarPersonalAsync(termino);
                return Ok(personal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        // ====================================
        // REPORTES
        // ====================================

        [HttpGet("reportes")]
        public async Task<ActionResult<ReporteDto>> GenerarReporte(
            [FromQuery] string tipo,
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tipo))
                    return BadRequest(new { mensaje = "Debe especificar el tipo de reporte" });

                if (desde > hasta)
                    return BadRequest(new { mensaje = "La fecha 'desde' no puede ser mayor que 'hasta'" });

                var reporte = await _adminService.GenerarReporteAsync(tipo, desde, hasta);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // ====================================
        // LOGS DEL SISTEMA
        // ====================================

        [HttpGet("logs")]
        public async Task<ActionResult<IEnumerable<LogDto>>> GetLogs(
            [FromQuery] string? tipo,
            [FromQuery] DateTime? fecha)
        {
            try
            {
                var logs = await _adminService.ObtenerLogsAsync(tipo, fecha);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }

        [HttpPost("logs")]
        public async Task<ActionResult> RegistrarLog([FromBody] RegistrarLogDto dto)
        {
            try
            {
                await _adminService.RegistrarLogAsync(dto);
                return Ok(new { mensaje = "Log registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
            }
        }
    }
}