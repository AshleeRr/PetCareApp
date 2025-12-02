using PetCareApp.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Application.Interfaces
{
    public interface ITipoProductoService
    {
        Task<List<TipoProductoDto>> ObtenerTiposAsync();
        Task<TipoProductoDto?> ObtenerPorIdAsync(int id);
    }
}
