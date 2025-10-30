using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IMascotaRepository
    {
        Task<List<Mascota>> GetAllAsync();
        Task<Mascota?> GetByIdAsync(int id);
        Task<List<Mascota>> FilterAsync(string nombre, int? tipoMascotaId, bool? estaCastrado);
        Task<Mascota> AddAsync(Mascota mascota);
        Task UpdateAsync(Mascota mascota);
        Task DeleteAsync(int id);
    }
}
