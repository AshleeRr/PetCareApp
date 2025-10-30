
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface ITratamientoRepository : IGenericRepositorio<MascotaPruebasMedica>
    {
        Task<List<MascotaPruebasMedica?>> GetPruebasOfMascotaById(int mascotaId);
    }
}
