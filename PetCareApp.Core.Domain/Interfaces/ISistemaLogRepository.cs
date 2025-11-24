using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface ISistemaLogRepository
    {
        // Task AddAsync(SistemaLog log);
        Task<IEnumerable<SistemaLog>> GetAllAsync();
        Task<SistemaLog?> GetByIdAsync(int id);
        Task<SistemaLog> AddAsync(SistemaLog log);
        Task<IEnumerable<SistemaLog>> GetByFiltrosAsync(string? tipo, DateTime? fecha); // ✅ AGREGAR

    }
}
