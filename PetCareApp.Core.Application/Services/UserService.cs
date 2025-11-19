
using AutoMapper;
using PetCareApp.Core.Application.Dtos.UsersDtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUsuarioRepositorio _usuarioRepo;
        private readonly Ilogger _logger;
        private readonly IMapper _mapper;

        public UserService (IUsuarioRepositorio usuarioRepo, Ilogger logger, IMapper mapper )
        {
            _usuarioRepo = usuarioRepo;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.Error("El email proporcionado es nulo o vacío");
                throw new ArgumentException("El email no puede ser nulo o vacío", nameof(email));
            }
            var user = await _usuarioRepo.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.Error($"Usuario no encontrado: {email}");
                throw new InvalidOperationException("Usuario no encontrado");
            }
            if (user.Role == null)
            {
                _logger.Error($"Error crítico: El usuario {user.Email} no tiene un rol asignado");
                throw new InvalidOperationException("El usuario no tiene un rol asignado");
            }
            return _mapper.Map<UserDto>(user);
        }
    }
}
