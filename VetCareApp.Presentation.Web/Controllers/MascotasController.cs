using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Dtos.MascotasDtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotasController : ControllerBase
    {
        private readonly IMascotaService _service;
        public MascotasController(IMascotaService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<List<MascotaDto>>> GetAll([FromQuery] string? nombre, [FromQuery] int? tipoId, [FromQuery] bool? estaCastrado)
        {
            if (!string.IsNullOrWhiteSpace(nombre) || tipoId.HasValue || estaCastrado.HasValue)
                return Ok(await _service.FiltrarAsync(nombre ?? string.Empty, tipoId, estaCastrado));
            return Ok(await _service.ObtenerTodosAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MascotaDto>> GetById(int id)
        {
            var m = await _service.ObtenerPorIdAsync(id);
            if (m == null) return NotFound();
            return Ok(m);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CrearMascotaDto dto)
        {
            var created = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CrearMascotaDto dto)
        {
            var ok = await _service.EditarAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await _service.EliminarAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
