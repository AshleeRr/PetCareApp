using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IRoleRepositorio
    {
        Task<Role?> GetByNameAsync(string roleName);
        Task<Role?> GetByIdAsync(int id);
       // Task<List<Role>> GetAllAsync();
    }
}
