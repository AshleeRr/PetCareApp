
using PetCareApp.Core.Application.Dtos.PruebasMedicasDtos;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IPruebaMedicaService : IGenericService<PruebasMedica, PruebaMedicaDto>
    {
        Task<PruebaMedicaDto?> GetPruebaMedicaByNameAsync (string name);
    }
}
