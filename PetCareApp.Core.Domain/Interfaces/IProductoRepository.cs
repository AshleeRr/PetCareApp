using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(int id);
        Task<IEnumerable<Producto>> GetByTipoAsync(int tipoProductoId);
        Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre);
        Task<Producto> AddAsync(Producto producto);
        Task<Producto?> UpdateAsync(int id, Producto producto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Producto>> GetDisponiblesAsync(); 
    }
}
