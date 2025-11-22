using AutoMapper;
using PetCareApp.Core.Application.Dtos.MedicamentosDtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Core.Application.Services
{
    public class MedicamentoService : GenericService<Medicamento, MedicamentoDto>, IMedicamentoService
    {
        private readonly IMedicamentoRepository _medicamentoRepository;
        private readonly IMapper _mapper;
        public MedicamentoService(IMedicamentoRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _mapper = mapper;
            _medicamentoRepository = repository;
        }

        public async Task<MedicamentoDto> GetMedicamentoByNameAsync(string nombre)
        {
            var medicamento = await _medicamentoRepository.GetMedicamentoByNameAsync(nombre);
            return _mapper.Map<MedicamentoDto>(medicamento);
        }

        public async Task<List<MedicamentoDto>> GetMedicamentosByPresentacionAsync(string presentacion)
        {
            var medicamentos = await _medicamentoRepository.GetMedicamentosByPresentacionAsync(presentacion);
            return _mapper.Map<List<MedicamentoDto>>(medicamentos);
        }
    }
}
