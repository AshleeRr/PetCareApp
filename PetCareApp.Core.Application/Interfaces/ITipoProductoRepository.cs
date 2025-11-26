using PetCareApp.Core.Domain.Entities;
using PetCareApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Application.Interfaces
{
    public interface ITipoProductoRepository : IRepository<TipoProducto>
    {
        //Task<List<TipoProducto>> ObtenerTodosAsync();
        //Task<TipoProducto?> ObtenerPorIdAsync(int id);
    }
}
