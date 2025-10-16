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
        public IActionResult GetAll()
        {
            return Ok(_service.ObtenerClientes());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cliente = _service.ObtenerPorId(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpGet("cedula/{cedula}")]
        public IActionResult GetByCedula(string cedula)
        {
            var cliente = _service.ObtenerPorCedula(cedula);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpGet("filtrar")]
        public IActionResult FiltrarPorNombre([FromQuery] string nombre)
        {
            return Ok(_service.FiltrarPorNombre(nombre));
        }

        [HttpPost]
        public IActionResult Crear([FromBody] CrearClienteDto dto)
        {
            _service.CrearCliente(dto);
            return Ok("Cliente creado exitosamente.");
        }

        [HttpPut("{id}")]
        public IActionResult Editar(int id, [FromBody] CrearClienteDto dto)
        {
            _service.EditarCliente(id, dto);
            return Ok("Cliente actualizado exitosamente.");
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            _service.EliminarCliente(id);
            return Ok("Cliente eliminado exitosamente.");
        }
    }
}