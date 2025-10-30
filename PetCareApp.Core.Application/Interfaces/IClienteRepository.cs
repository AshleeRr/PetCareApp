using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Dueño>> GetAllAsync();
        Task<Dueño?> GetByIdAsync(int id);
        Task<Dueño?> GetByCedulaAsync(string cedula);
        Task<List<Dueño>> FilterAsync(string nombre, string cedula);
        Task<Dueño> AddAsync(Dueño cliente);
        Task UpdateAsync(Dueño cliente);
        Task DeleteAsync(int id);
    }
}
