using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task AddAsync(Usuario u);
        Task<Usuario> GetByEmailAsync(string email);
        Task<Usuario?> GetByIdWithRolAsync(int id);
    }
}
