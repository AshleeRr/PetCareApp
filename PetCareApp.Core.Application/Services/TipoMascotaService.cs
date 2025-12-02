using PetCareApp.Application.Interfaces;
using PetCareApp.Domain.Interfaces;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Application.Services
{
    public class TipoMascotaService : ITipoMascotaService
    {
        private readonly ITipoMascotaRepository _repository;

        public TipoMascotaService(ITipoMascotaRepository repository);
        private readonly IRepository<TipoMascota> _repository;

        public TipoMascotaService(IRepository<TipoMascota> repository)
        {
            _repository = repository;
        }

        public async Task<List<TipoMascotaDto>> ObtenerTodosAsync()
        {
            var tipos = await _repository.GetAllAsync(); // ENTIDADES, no DTOs
            var tipos = await _repository.GetAllAsync(); // del repositorio genérico

            return tipos.Select(t => new TipoMascotaDto
            {
                Id = t.Id,
                Tipo = t.Tipo
            }).ToList();
        }
    }
}
