using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Dtos.MascotasDtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialController : ControllerBase
    {
        private readonly IHistorialService _historialService;

        public HistorialController(IHistorialService historialService)
        {
            _historialService = historialService;
        }

        // ==========================================================
        // GET: api/historial/mascota/5 → obtener historial completo
        // ==========================================================
        [HttpGet("mascota/{mascotaId:int}")]
        public async Task<ActionResult<MascotaHistorialDto>> GetHistorial(int mascotaId)
        {
            if (mascotaId <= 0)
                return BadRequest("El id de la mascota debe ser mayor a 0.");

            var historial = await _historialService.GetHistorialDeMascotaAsync(mascotaId);

            if (historial == null ||
                (historial.HistorialCitas.Count == 0 && historial.PruebasMedicas.Count == 0))
            {
                return NotFound("Esta mascota no tiene historial registrado.");
            }

            return Ok(historial);
        }
    }
}
