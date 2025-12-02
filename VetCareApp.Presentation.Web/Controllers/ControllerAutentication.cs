using Infraestructura.Servicios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;
using System.Security.Claims;

namespace VetCareApp.Presentation.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControllerAutenticacion : ControllerBase
    {
        private readonly IAutenticacionService _auth;
        private readonly TokenService _tokenService; // ✅ Ya está correcto
        private readonly IPasswordResetService _passwordResetService; // ✅ Agregar

        public ControllerAutenticacion(
            IAutenticacionService auth,
            TokenService tokenService,
            IPasswordResetService passwordResetService) // ✅ Inyectar
        {
            _auth = auth;
            _tokenService = tokenService;
            _passwordResetService = passwordResetService;
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

        // ✅ NUEVOS ENDPOINTS PARA RESET PASSWORD

        [HttpPost("solicitar-reset-password")]
        public async Task<ActionResult> SolicitarResetPassword([FromBody] SolicitarResetPasswordDto dto)
        {
            var success = await _passwordResetService.SolicitarResetPasswordAsync(dto.Email);

            // Por seguridad, siempre retornamos éxito (no revelamos si el email existe)
            return Ok(new
            {
                mensaje = "Si el email existe, recibirás instrucciones para restablecer tu contraseña"
            });
        }

        [HttpGet("verificar-token/{token}")]
        public async Task<ActionResult> VerificarToken(string token)
        {
            var valido = await _passwordResetService.VerificarTokenAsync(token);

            if (!valido)
                return BadRequest(new { mensaje = "Token inválido o expirado" });

            return Ok(new { mensaje = "Token válido" });
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var success = await _passwordResetService.ResetPasswordAsync(dto.Token, dto.NuevaPassword);

            if (!success)
                return BadRequest(new { mensaje = "Token inválido, expirado o ya usado" });

            return Ok(new { mensaje = "Contraseña actualizada exitosamente" });
        }



        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "ControllerAutenticacion");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("Cookies");
            if (!result.Succeeded)
                return BadRequest("Error al autenticarse con Google");

            var email = result.Principal.FindFirst(c => c.Type == ClaimTypes.Email)?.Value ?? "";
            var name = result.Principal.FindFirst(c => c.Type == ClaimTypes.Name)?.Value ?? "";

            var user = await _auth.LoginConGoogleAsync(email, name);
            var token = _tokenService.GenerateToken(user);

            // Escapar comillas simples y caracteres problemáticos
            string safeName = name.Replace("'", "\\'");
            string safeEmail = email.Replace("'", "\\'");

            string html = $@"
<html>
    <body>
        <script>
            const data = {{
                token: '{token}',
                roleId: {user.RoleId},
                userName: '{safeName}',
                email: '{safeEmail}'
            }};
            window.opener.postMessage(data, '*');
            window.close();
        </script>
        <p>Autenticando... si la ventana no se cierra automáticamente, ciérrala manualmente.</p>
    </body>
</html>";
            return Content(html, "text/html");
        }
    }
}
