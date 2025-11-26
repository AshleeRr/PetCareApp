
using AutoMapper;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos;
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
        public async Task<MascotaHistorialDto> GetHistorialDeMascotaAsync(int mascotaId)
        {
            var citas = await _citaRepository.GetCitasOfMascotaById(mascotaId);
            var pruebasMedicas = await _mascotaPruebaMedicaRepository.GetPruebasOfMascotaById(mascotaId);

            var historialDto = new MascotaHistorialDto
            {
                MascotaId = mascotaId,
                HistorialCitas = _mapper.Map<List<CitaDto>>(citas),

                PruebasMedicas = _mapper.Map<List<MascotaPruebaMedicaHistorialDto>>(pruebasMedicas)
            };

            return historialDto;
        }
    }
}
