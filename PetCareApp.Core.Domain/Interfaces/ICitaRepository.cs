using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface ICitaRepository : IGenericRepository<Cita>
    {
        Task<List<Cita?>> GetCitasByDate(DateOnly date);
        //Task<List<Cita?>> GetCitasByDuenio()
    }
}
