using AutoMapper;
using PetCareApp.Core.Application.Dtos.PruebasMedicasDtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Core.Application.Services
{
    public class PruebaMedicaService : GenericService<PruebasMedica, PruebaMedicaDto>, IPruebaMedicaService
    {
        private readonly IMapper _mapper;
        private readonly IPruebasMedicasRepository _pruebasMedicasRepository;

        public PruebaMedicaService(IPruebasMedicasRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _mapper = mapper;
            _pruebasMedicasRepository = repository;
        }

        public async Task<PruebaMedicaDto?> GetPruebaMedicaByNameAsync(string name)
        {
            var PruebaMedica = await _pruebasMedicasRepository.GetByNameAsync(name);
            return _mapper.Map<PruebaMedicaDto>(PruebaMedica);
        }
    }
}
