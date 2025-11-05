using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface ICitaRepository : IGenericRepositorio<Cita>
    {
       // Task<List<Cita>> GetCitasByDate(DateOnly date);

        Task<List<Cita>> GetCitasOfMascotaById(int mascotaId);
        Task<List<Cita>> GetAllAsync();
        //Task<Cita?> GetByIdAsync(int id);
        Task<List<Cita>> GetByFechaAsync(DateTime fecha);
        Task<List<Cita>> GetByClienteAsync(int clienteId);
        Task<List<Cita?>> GetCitasAsiganasAVeterinarioAsync(int userId);
      //  Task<Cita> AddAsync(Cita cita);
      //  Task UpdateAsync(Cita cita);
      //  Task DeleteAsync(int id);
    }
}
