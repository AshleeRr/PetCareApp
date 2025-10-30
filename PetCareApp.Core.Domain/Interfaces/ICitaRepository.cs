using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface ICitaRepository : IGenericRepositorio<Cita>
    {
        Task<List<Cita?>> GetCitasByDate(DateOnly date);

        Task<List<Cita?>> GetCitasOfMascotaById(int mascotaId);
    }
}
