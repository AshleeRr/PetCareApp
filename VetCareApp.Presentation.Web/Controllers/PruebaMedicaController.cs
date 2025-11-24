using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.ViewModels.PruebasMedicasVms;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PruebaMedicaController : Controller
    {
        private readonly IPruebaMedicaService _pruebaMedicaService;
        private readonly IMapper _mapper;
        public PruebaMedicaController(IPruebaMedicaService pruebaMedicaService, IMapper mapper)
        {
            _pruebaMedicaService = pruebaMedicaService;
            _mapper = mapper;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<PruebaMedicaViewModel>> GetPruebaByNameAsync(string name)
        {
            
            var pruebaMedica = await _pruebaMedicaService.GetPruebaMedicaByNameAsync(name);
            if (pruebaMedica == null)
            {
               return BadRequest("No se ha podido encontrar una prueba medica con el nombre proporcionado");
            }
            //var vm = _mapper.Map<PruebaMedicaViewModel>(pruebaMedica);
             return Ok(pruebaMedica);
        }
        
    }
}
