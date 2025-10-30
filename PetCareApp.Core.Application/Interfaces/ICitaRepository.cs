using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface ICitaRepository
    {
        Task<List<Cita>> GetAllAsync();
        Task<Cita?> GetByIdAsync(int id);
        Task<List<Cita>> GetByFechaAsync(DateTime fecha);
        Task<List<Cita>> GetByClienteAsync(int clienteId);
        Task<Cita> AddAsync(Cita cita);
        Task UpdateAsync(Cita cita);
        Task DeleteAsync(int id);
    }
}
