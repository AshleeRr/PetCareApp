using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetCareApp.Core.Application.Services;
using PetCareApp.Core.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infraestructura.Servicios
{
    public class TokenService
    {
        private readonly ConfiguracionServices _config;

        public TokenService(IOptions<ConfiguracionServices> config)
        {
            _config = config.Value;
        }

        public string GenerateToken(Usuario usuario)
        {                
            if (usuario.Role == null)
            {
                throw new InvalidOperationException("El usuario no tiene un rol asignado");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()), // ✅ ID del usuario
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role.Rol), // ✅ Rol del usuario
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_config.ExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}