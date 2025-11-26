using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Application.DTOs;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Application.Services;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoMascotaController : ControllerBase
    {
        private readonly ITipoMascotaService _service;

        public TipoMascotaController(ITipoMascotaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipoMascotaDto>>> GetAll()
        {
            var tipos = await _service.ObtenerTodosAsync();
            return Ok(tipos);
        }
    }
}

// DTO
namespace PetCareApp.Application.DTOs
{
    public class TipoMascotaDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }
}