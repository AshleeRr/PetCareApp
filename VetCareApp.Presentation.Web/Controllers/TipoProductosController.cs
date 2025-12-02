using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Domain.Interfaces;

using PetCareApp.Core.Application.Dtos;
using PetCareApp.Application.Interfaces;



namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")] // Cambios realizados
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

    public class TipoProductosController : ControllerBase
    {
        private readonly IGenericRepositorio<PetCareApp.Core.Domain.Entities.TipoProducto> _tipoProductoRepo;

        public TipoProductosController(IGenericRepositorio<PetCareApp.Core.Domain.Entities.TipoProducto> tipoProductoRepo)
        {
            _tipoProductoRepo = tipoProductoRepo;
        }

            /// <summary>
            /// Obtener todos los tipos de productos (categorías)
            /// </summary>
            [HttpGet]
            [AllowAnonymous]
            public async Task<ActionResult> GetAll()
            {
                try
                {
                    var tipos = await _tipoProductoRepo.GetAllAsync();
                    return Ok(tipos);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { mensaje = $"Error interno: {ex.Message}" });
                }
            }
        }
    }
}
