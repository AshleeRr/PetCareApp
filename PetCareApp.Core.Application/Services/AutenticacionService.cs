using AutoMapper;
using Infraestructura.Servicios;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Dtos.UsersDtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PetCareApp.Core.Application.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly IUsuarioRepositorio _usuarioRepo;
        private readonly IRoleRepositorio _roleRepo;
        private readonly TokenService _tokenService;
        private readonly Ilogger _logger;
        private readonly IEmailService _emailService; // ⭐ NUEVO

        public AutenticacionService(
            IUsuarioRepositorio usuarioRepo,
            IRoleRepositorio roleRepo,
            Ilogger logger,
            IEmailService emailService) // ⭐ INYECTAR EmailService
        {
            _usuarioRepo = usuarioRepo;
            _roleRepo = roleRepo;
            _logger = logger;
            _emailService = emailService;
        }

        // ========================================
        // REGISTRO DE USUARIO CON EMAIL
        // ========================================
        public async Task<Usuario> RegistrarAsync(RegistroDto dto)
        {
            var existente = await _usuarioRepo.GetByEmailAsync(dto.Email);
            if (existente != null)
                throw new InvalidOperationException("Email ya registrado");

            var roleName = string.IsNullOrEmpty(dto.Role) ? "Cliente" : dto.Role;

            // Buscar el rol existente
            var role = await _roleRepo.GetByNameAsync(roleName);

            if (role == null)
            {
                throw new InvalidOperationException($"El rol '{roleName}' no existe. Roles válidos: Cliente, Recepcionista, Admin");
            }

            var user = new Usuario
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHashed = Hash(dto.PasswordHashed),
                RoleId = role.Id
            };

            await _usuarioRepo.AddAsync(user);

            var usuarioConRole = await _usuarioRepo.GetByEmailAsync(user.Email);

            _logger.Info($"Usuario {user.Email} registrado con rol {role.Rol}");

            // ⭐ ENVIAR EMAIL DE BIENVENIDA
            try
            {
                var emailEnviado = await _emailService.EnviarEmailBienvenidaAsync(
                    user.Email,
                    user.UserName,
                    user.Email
                );

                if (emailEnviado)
                {
                    _logger.Info($"✅ Email de bienvenida enviado a {user.Email}");
                }
                else
                {
                    _logger.Warning($"⚠️ No se pudo enviar email de bienvenida a {user.Email}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"❌ Error al enviar email de bienvenida: {ex.Message}");
                // No lanzamos excepción para no interrumpir el registro
            }

            return usuarioConRole!;
        }

        // ========================================
        // LOGIN DE USUARIO CON EMAIL
        // ========================================
        public async Task<Usuario?> LoginAsync(RegistrarDTOS dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.PasswordHashed))
            {
                _logger.Error("Intento de login con campos vacíos");
                return null;
            }

            var user = await _usuarioRepo.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                _logger.Error($"Intento de login fallido - usuario no encontrado: {dto.Email}");
                return null;
            }

            var hashedPassword = Hash(dto.PasswordHashed);
            if (user.PasswordHashed != hashedPassword)
            {
                _logger.Error($"Intento de login fallido - contraseña incorrecta: {dto.Email}");
                return null;
            }

            if (user.Role == null)
            {
                _logger.Error($"Error crítico: El usuario {user.Email} no tiene un rol asignado");
                return null;
            }

            _logger.Info($"Usuario {user.Email} inició sesión exitosamente con rol {user.Role.Rol}");

            // ⭐ ENVIAR EMAIL DE NOTIFICACIÓN DE LOGIN
            try
            {
                // Ejecutar el envío de email de forma asíncrona sin esperar (fire and forget)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var emailEnviado = await _emailService.EnviarEmailLoginAsync(
                            user.Email,
                            user.UserName,
                            "Navegador Web" // Puedes mejorar esto con la IP real del cliente
                        );

                        if (emailEnviado)
                        {
                            _logger.Info($"✅ Email de login enviado a {user.Email}");
                        }
                        else
                        {
                            _logger.Warning($"⚠️ No se pudo enviar email de login a {user.Email}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"❌ Error al enviar email de login: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"❌ Error al iniciar tarea de email de login: {ex.Message}");
                // No lanzamos excepción para no interrumpir el login
            }

            return user;
        }

        // ========================================
        // LOGIN CON GOOGLE
        // ========================================
        public async Task<Usuario> LoginConGoogleAsync(string email, string name)
        {
            // Retornar un usuario "temporal" sin tocar la DB
            return new Usuario
            {
                Email = email,
                UserName = name,
                PasswordHashed = "",
                RoleId = 1,
                Role = new Role
                {
                    Id = 1,
                    Rol = "Cliente"
                }
            };
        }

        // ========================================
        // MÉTODO PRIVADO PARA HASHEAR CONTRASEÑAS
        // ========================================
        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}