using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IPersonalRepository
    {
        Task<IEnumerable<Personal>> GetAllAsync();
        Task<Personal?> GetByIdAsync(int id);
        Task<Personal> AddAsync(Personal personal);
        Task<Personal?> UpdateAsync(int id, Personal personal);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Personal>> BuscarAsync(string termino);
        Task<int> ContarTotalAsync(); // ✅ AGREGAR
        Task<bool> ExisteCedulaAsync(string cedula, int? excludeId = null); // ✅ AGREGAR

    }
}
