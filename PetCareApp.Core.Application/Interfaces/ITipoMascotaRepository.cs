using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Domain.Interfaces;

namespace PetCareApp.Application.Interfaces
{
    public interface ITipoMascotaRepository : IRepository<TipoMascota>
    {
        //Task<List<TipoMascotaDto>> ObtenerTodosAsync();
        //Task<TipoMascotaDto?> ObtenerPorIdAsync(int id);
    }
}

namespace PetCareApp.Application.Interfaces
{
    public interface ITipoMascotaRepository
    {
        Task<List<TipoMascota>> ObtenerTodosAsync();
        Task<TipoMascota?> ObtenerPorIdAsync(int id);
    }
}

