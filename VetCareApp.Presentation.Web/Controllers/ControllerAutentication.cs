using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;

namespace VetCareApp.Presentation.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControllerAutenticacion : ControllerBase
    {
        private readonly IAutenticacionService _auth;
        private readonly TokenService _tokenService;

        public ControllerAutenticacion(IAutenticacionService auth, TokenService tokenService)
        {
            _auth = auth;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsuarioResponseDto>> Register([FromBody] RegistroDto dto)
        {
            var usuario = await _auth.RegistrarAsync(dto);

            if (usuario == null)
                return BadRequest("No se pudo registrar el usuario.");

            var token = _tokenService.GenerateToken(usuario);
            return Ok(UsuarioResponseDto.FromUsuario(usuario, token));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioResponseDto>> Login([FromBody] RegistrarDTOS dto)
        {
            var usuario = await _auth.LoginAsync(dto);
            if (usuario == null)
                return Unauthorized("Credenciales inválidas");

            var token = _tokenService.GenerateToken(usuario);
            return Ok(UsuarioResponseDto.FromUsuario(usuario, token));
        }
    }
}
