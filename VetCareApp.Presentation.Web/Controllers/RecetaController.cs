using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos.RecetasDtos;
using PetCareApp.Core.Application.ViewModels.RecetaVms;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetaController : ControllerBase
    {
        private readonly IRecetaService _service;
        private readonly IMapper _mapper;

        public RecetaController(IRecetaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // ==========================================================
        // GET: api/receta/cita/5   → obtener recetas de una cita
        // ==========================================================
        [HttpGet("cita/{citaId:int}")]
        public async Task<ActionResult<List<RecetaDto>>> GetRecetasByCita(int citaId)
        {
            if (citaId <= 0)
                return BadRequest("El id debe ser mayor que 0");


            var recetas = await _service.GetRecetasByCitaAsync(citaId);

            if (recetas == null || recetas.Count == 0)
                return NotFound("Esta cita no tiene recetas registradas.");

            return Ok(recetas);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CreateRecetaVm vm)
        {

            var dto = _mapper.Map<CreateRecetaDto>(vm);

            await _service.CreateRecetaAsync(dto);

            return RedirectToAction("Details", "Cita", new { id = vm.CitaId });
        }
        [HttpPost("agregar-medicamento")]
        public async Task<ActionResult> AddMedicamento([FromBody] AddMedicamentoToRecetaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddMedicamentoToRecetaAsync(dto);

            if (!result)
                return BadRequest("No se pudo agregar el medicamento a la receta.");

            return Ok("Medicamento agregado correctamente.");
        }

    }
}
