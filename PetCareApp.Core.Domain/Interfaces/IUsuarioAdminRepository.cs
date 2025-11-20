using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IUsuarioAdminRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario> AddAsync(Usuario usuario);
        Task<Usuario?> UpdateAsync(int id, Usuario usuario);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Usuario>> BuscarAsync(string termino);
        Task<int> ContarTotalAsync(); // ✅ AGREGAR
        Task<bool> ExisteEmailAsync(string email, int? excludeId = null); // ✅ AGREGAR
    }
}
