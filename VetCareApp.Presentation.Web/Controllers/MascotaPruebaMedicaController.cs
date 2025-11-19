using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaPruebaMedicaController : ControllerBase
    {
        private readonly IMascotaPruebaMedicaService _service;
        private readonly IMapper _mapper;

        public MascotaPruebaMedicaController(IMascotaPruebaMedicaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("mascota/{mascotaId:int}")]
        public async Task<ActionResult<List<MascotaPruebaMedicaDto>>> GetPruebasOfMascota(int mascotaId)
        {
            ValidateId(mascotaId);
            var pruebas = await _service.GetPruebasMedicasOfMascotaById(mascotaId);

            if (pruebas == null || !pruebas.Any())
                return NotFound("Esta mascota no tiene pruebas médicas registradas.");

            return Ok(pruebas);
        }

        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] CreateMascotaPruebaMedicaDto dto)
        { 

            var mpm = await _service.CrearPruebaParaMascotaAsync(dto);
            return Ok(mpm);
        }
        private IActionResult ValidateId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El id debe ser mayor que 0");
            }
            return Ok();
        }
    }
}
