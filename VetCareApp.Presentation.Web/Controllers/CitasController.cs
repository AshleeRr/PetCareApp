using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _service;
        private readonly IMapper _mapper;

        public CitasController(ICitaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CitaDto>>> GetAll([FromQuery] DateTime? fecha, [FromQuery] int? clienteId)
        {
            if (fecha.HasValue)
                return Ok(await _service.ObtenerPorFechaAsync(fecha.Value));

            if (clienteId.HasValue)
                return Ok(await _service.ObtenerPorClienteAsync(clienteId.Value));

            return Ok(await _service.ObtenerCitasAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CitaDto>> GetById(int id)
        {
            var cita = await _service.ObtenerPorIdAsync(id);
            if (cita == null) return NotFound();
            return Ok(cita);
        }

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<List<CitaDto>>> GetCitasByVeterinarioId(int userId)
        {
            var citas = await _service.GetCitasAsiganasAVeterinarioAsync(userId);
            if (citas == null) return NotFound();
            //if(!citas.Any()) return BadRequest("No se ha podido encontrar citas asignadas a este veterinario");
           // var vm = _mapper.Map<List<CitaViewModel>>(citas);

            return Ok(citas);
        }

        [HttpGet("{mascotaId:int}")]
        public async Task<ActionResult<List<CitaDto>>> GetCitasOfMascotaId(int mascotaId)
        {
            var citas = await _service.GetCitasOfMascotaById(mascotaId);
            if (citas == null) return NotFound();

            return Ok(citas);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CrearCitaDto dto)
        {
            var creada = await _service.CrearCitaAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] ActualizarCitaDto dto)
        {
            var ok = await _service.EditarCitaAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await _service.EliminarCitaAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

    }
}
