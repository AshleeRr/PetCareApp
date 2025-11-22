using AutoMapper;
using PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Core.Application.Services
{
    public class MascotaPruebaMedicaService : IMascotaPruebaMedicaService
    {
        private readonly IMapper _mapper;
        private readonly ITratamientoRepository _tratamientoRepository;
        public MascotaPruebaMedicaService(IMapper mapper, ITratamientoRepository tratamientoRepository)
        {
            _mapper = mapper;
            _tratamientoRepository = tratamientoRepository;
        }

        public async Task<bool> CrearPruebaParaMascotaAsync(CreateMascotaPruebaMedicaDto dto)
        {
            var entity = _mapper.Map<MascotaPruebasMedica>(dto);
            entity.Fecha = DateTime.UtcNow;
            await _tratamientoRepository.AddAsync(entity);

            return true;
        }
        
        public async Task<List<MascotaPruebaMedicaDto>> GetPruebasMedicasOfMascotaById(int mascotaId)
        {
            var pruebas = await _tratamientoRepository.GetPruebasOfMascotaById(mascotaId);
            return _mapper.Map<List<MascotaPruebaMedicaDto>>(pruebas);
        }
    }
}
