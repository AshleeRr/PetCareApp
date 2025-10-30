using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClientesController(IClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDto>>> GetAll([FromQuery] string? nombre, [FromQuery] string? cedula)
        {
            if (!string.IsNullOrWhiteSpace(nombre) || !string.IsNullOrWhiteSpace(cedula))
            {
                var filtered = await _service.FiltrarPorNombreAsync(nombre ?? string.Empty, cedula ?? string.Empty);
                return Ok(filtered);
            }
            var list = await _service.ObtenerClientesAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClienteDto>> GetById(int id)
        {
            var c = await _service.ObtenerPorIdAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpGet("cedula/{cedula}")]
        public async Task<ActionResult<ClienteDto>> GetByCedula(string cedula)
        {
            var c = await _service.ObtenerPorCedulaAsync(cedula);
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CrearClienteDto dto)
        {
            try
            {
                var created = await _service.CrearClienteAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (System.InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] ActualizarClienteDto dto)
        {
            var ok = await _service.EditarClienteAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ok = await _service.EliminarClienteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

    }
}