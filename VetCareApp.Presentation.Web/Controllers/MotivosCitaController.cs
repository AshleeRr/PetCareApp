using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotivosCitaController : ControllerBase
    {
        private readonly IMotivoCitaService _service;

        public MotivosCitaController(IMotivoCitaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MotivoCitaDto>>> GetAll()
        {
            var motivos = await _service.ObtenerMotivosAsync();
            return Ok(motivos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MotivoCitaDto>> GetById(int id)
        {
            var motivo = await _service.ObtenerPorIdAsync(id);
            if (motivo == null)
                return NotFound(new { mensaje = "Motivo de cita no encontrado" });

            return Ok(motivo);
        }

        // POST: api/MotivosCita
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MotivoCitaDto>> Create([FromBody] CrearMotivoCitaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Motivo))
                return BadRequest(new { mensaje = "El motivo es requerido" });

            var motivo = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = motivo.Id }, motivo);
        }

        // PUT: api/MotivosCita/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MotivoCitaDto>> Update(int id, [FromBody] ActualizarMotivoCitaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Motivo))
                return BadRequest(new { mensaje = "El motivo es requerido" });

            var motivo = await _service.ActualizarAsync(id, dto);
            if (motivo == null)
                return NotFound(new { mensaje = "Motivo de cita no encontrado" });

            return Ok(motivo);
        }

        // DELETE: api/MotivosCita/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var eliminado = await _service.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Motivo de cita no encontrado" });

            return Ok(new { mensaje = "Motivo de cita eliminado exitosamente" });
        }
    }
}
