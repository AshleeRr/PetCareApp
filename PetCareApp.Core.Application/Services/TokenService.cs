using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Services
{
    public class TokenService
    {
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _expirationMinutes;

        public TokenService(IConfiguration configuration)
        {
            _jwtKey = configuration["JwtSettings:SecretKey"]
                      ?? throw new ArgumentNullException("JwtSettings:SecretKey no configurado");
            _jwtIssuer = configuration["JwtSettings:Issuer"];
            _jwtAudience = configuration["JwtSettings:Audience"];
            _expirationMinutes = int.Parse(configuration["JwtSettings:ExpirationInMinutes"] ?? "60");
        }

        public string GenerateToken(Usuario usuario)
        {
            if (usuario.Role == null)
            {
                throw new InvalidOperationException("El usuario no tiene un rol asignado");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                 new Claim(ClaimTypes.Role, usuario.Role.Rol)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
