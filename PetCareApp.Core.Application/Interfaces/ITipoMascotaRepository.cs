using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Application.Interfaces
{
    public interface ITipoMascotaRepository
    {
        Task<List<TipoMascota>> ObtenerTodosAsync();
        Task<TipoMascota?> ObtenerPorIdAsync(int id);
    }
}
