
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IPruebasMedicasRepository : IGenericRepositorio<PruebasMedica>
    {
        Task<PruebasMedica> GetByNameAsync(string nombrePruebaMedica);
    }
}
