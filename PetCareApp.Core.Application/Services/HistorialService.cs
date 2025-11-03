
using AutoMapper;
using PetCareApp.Core.Application.Dtos.MascotasDtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Core.Application.Services
{
    public class HistorialService : IHistorialService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly ITratamientoRepository _mascotaPruebaMedicaRepository;
        private readonly IMapper _mapper;

        public HistorialService(ICitaRepository citaRepository, ITratamientoRepository tratamientoRepository, IMapper mapper) 
        {
            _citaRepository = citaRepository;
            _mascotaPruebaMedicaRepository = tratamientoRepository;
            _mapper = mapper;
        }
        public Task<MascotaHistorialDto> GetHistorialDeMascotaAsync(int mascotaId)
        {
            throw new NotImplementedException();
        }
    }
}
