
using PetCareApp.Core.Application.Dtos.PruebasMedicasDtos;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IPruebaMedicaService : IGenericService<PruebaMedicaDto>
    {
        Task<PruebaMedicaDto?> GetPruebaMedicaByNameAsync (string name);
    }
}
