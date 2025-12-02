using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IVentaRepository
    {
        Task<IEnumerable<Venta>> GetAllAsync();
        Task<Venta?> GetByIdAsync(int id);
        Task<Venta> AddAsync(Venta venta);
        Task<Venta?> UpdateAsync(Venta venta);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Venta>> GetByUsuarioIdAsync(int usuarioId); // ✅ AGREGAR
        Task<decimal> ObtenerIngresosMesActualAsync(); // ✅ AGREGAR
        Task<IEnumerable<Venta>> GetByFechaRangoAsync(DateTime desde, DateTime hasta); // ✅ AGREGAR


    }
}
