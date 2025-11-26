using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos.MedicamentosDtos;
using PetCareApp.Core.Application.Interfaces;

namespace VetCareApp.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentoController : Controller
    {
        private readonly IMedicamentoService _medicamentoService;
        private readonly IMapper _mapper;
        public MedicamentoController(IMedicamentoService medicamentoService, IMapper mapper)
        {
            _medicamentoService = medicamentoService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<MedicamentoDto>>> GetAll([FromQuery] string? nombre,[FromQuery] string? presentacion)
        {
            if (!string.IsNullOrWhiteSpace(nombre))
                return Ok(await _medicamentoService.GetMedicamentoByNameAsync(nombre));

            if (!string.IsNullOrWhiteSpace(presentacion))
                return Ok(await _medicamentoService.GetMedicamentosByPresentacionAsync(presentacion));

            return Ok(await _medicamentoService.GetAllAsync());
        }
    }
}
