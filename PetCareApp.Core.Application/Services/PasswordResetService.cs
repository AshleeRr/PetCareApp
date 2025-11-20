using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PetCareApp.Core.Application.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IPasswordResetTokenRepository _tokenRepo;
        private readonly IUsuarioRepositorio _usuarioRepo;
        private readonly Ilogger _logger;
        private readonly IEmailService _emailService;

        public PasswordResetService(
            IPasswordResetTokenRepository tokenRepo,
            IUsuarioRepositorio usuarioRepo,
            Ilogger logger,
            IEmailService emailService)
        {
            _tokenRepo = tokenRepo;
            _usuarioRepo = usuarioRepo;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<bool> SolicitarResetPasswordAsync(string email)
        {
            try
            {
                var usuario = await _usuarioRepo.GetByEmailAsync(email);
                if (usuario == null)
                {
                    _logger.Error($"Intento de reset para email inexistente: {email}");
                    return true;
                }

                var token = GenerarTokenSeguro(); // ✅ Método incluido abajo

                var resetToken = new PasswordResetToken
                {
                    UsuarioId = usuario.Id,
                    Token = token,
                    FechaCreacion = DateTime.Now,
                    FechaExpiracion = DateTime.Now.AddHours(1),
                    Usado = false
                };

                await _tokenRepo.AddAsync(resetToken);

                Console.WriteLine($"📧 Enviando email a: {email}");
                var emailEnviado = await _emailService.EnviarEmailResetPasswordAsync(email, token);

                if (emailEnviado)
                {
                    _logger.Info($"✅ Email enviado a: {email}");
                }

                Console.WriteLine($"🔑 TOKEN: {token}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> VerificarTokenAsync(string token)
        {
            try
            {
                var resetToken = await _tokenRepo.GetByTokenAsync(token);

                if (resetToken == null || resetToken.Usado || resetToken.FechaExpiracion < DateTime.Now)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al verificar token: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string token, string nuevaPassword)
        {
            try
            {
                var resetToken = await _tokenRepo.GetByTokenAsync(token);

                if (resetToken == null || resetToken.Usado || resetToken.FechaExpiracion < DateTime.Now)
                    return false;

                var usuario = await _usuarioRepo.GetByEmailAsync(resetToken.Usuario.Email);
                if (usuario == null)
                    return false;

                usuario.PasswordHashed = HashPassword(nuevaPassword);
                await _usuarioRepo.UpdateAsync(usuario.Id, usuario);

                resetToken.Usado = true;
                await _tokenRepo.UpdateAsync(resetToken);

                _logger.Info($"Contraseña actualizada para: {usuario.Email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al resetear password: {ex.Message}");
                return false;
            }
        }

        // ✅ Método para generar token seguro
        private static string GenerarTokenSeguro()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        // ✅ Método para hashear password
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}