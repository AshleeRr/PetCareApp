using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface ICitaRepository : IGenericRepositorio<Cita>
    {
        // Task<List<Cita>> GetCitasByDate(DateOnly date);

        Task<List<Cita>> GetAllAsync();
        Task<Cita?> GetByIdAsync(int id);
        Task<Cita> AddAsync(Cita cita);
        Task<Cita?> UpdateAsync(Cita cita);
        Task<bool> DeleteAsync(int id);
        Task<int> ContarCitasMesActualAsync();
        Task<IEnumerable<Cita>> ObtenerCitasPorRangoFechaAsync(DateTime desde, DateTime hasta);
        Task<List<Cita>> GetCitasOfMascotaById(int mascotaId);
        Task<List<Cita>> GetByFechaAsync(DateTime fecha);
        Task<List<Cita>> GetByClienteAsync(int clienteId);


        //  Task<Cita> AddAsync(Cita cita);
        //  Task UpdateAsync(Cita cita);
        //  Task DeleteAsync(int id);
    }
}
