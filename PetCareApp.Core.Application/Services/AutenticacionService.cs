using AutoMapper;
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
        private readonly IRoleRepositorio _roleRepo; // ✅ Usar IRoleRepositorio en lugar de DbContext
        private readonly Ilogger _logger;

        public AutenticacionService(
            IUsuarioRepositorio usuarioRepo,
            IRoleRepositorio roleRepo, // ✅ Inyectar IRoleRepositorio
            Ilogger logger)
        {
            _usuarioRepo = usuarioRepo;
            _roleRepo = roleRepo;
            _logger = logger;
        }

        public async Task<Usuario> RegistrarAsync(RegistroDto dto)
        {
            var existente = await _usuarioRepo.GetByEmailAsync(dto.Email);
            if (existente != null)
                throw new InvalidOperationException("Email ya registrado");

            var roleName = string.IsNullOrEmpty(dto.Role) ? "Cliente" : dto.Role;

            // ✅ BUSCAR el rol existente (NO crear uno nuevo)
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
                RoleId = role.Id // ✅ Asignar el ID del rol EXISTENTE
            };

            await _usuarioRepo.AddAsync(user);
          //  await _usuarioRepo.SaveChangesAsync();

            var usuarioConRole = await _usuarioRepo.GetByEmailAsync(user.Email);

            _logger.Info($"Usuario {user.Email} registrado con rol {role.Rol}");
            return usuarioConRole!;
        }
        

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
            return user;
        }
   
        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}