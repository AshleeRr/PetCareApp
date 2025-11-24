using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Domain.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
