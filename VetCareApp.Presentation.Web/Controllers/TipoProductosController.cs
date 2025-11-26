using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoProductoController : ControllerBase
    {
        private readonly ITipoProductoService _service;

        public TipoProductoController(ITipoProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoProductoDto>>> GetAll()
        {
            var tipos = await _service.ObtenerTiposAsync();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoProductoDto>> GetById(int id)
        {
            var tipo = await _service.ObtenerPorIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }
    }
}
