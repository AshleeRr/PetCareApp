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
    }
}
