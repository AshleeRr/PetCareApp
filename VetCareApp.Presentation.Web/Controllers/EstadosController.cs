using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly IEstadoService _service;

        public EstadosController(IEstadoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<EstadoDto>>> GetAll()
        {
            var estados = await _service.ObtenerEstadosAsync();
            return Ok(estados);
        }
    }
}
