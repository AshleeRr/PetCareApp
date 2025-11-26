using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface ICarritoRepository
    {
        Task<Carrito?> GetCarritoActivoByUsuarioIdAsync(int usuarioId);
        Task<Carrito> CrearCarritoAsync(int usuarioId);
        Task<CarritoItem?> GetCarritoItemByIdAsync(int itemId);
        Task<CarritoItem> AgregarItemAsync(CarritoItem item);
        Task<CarritoItem?> ActualizarItemAsync(CarritoItem item);
        Task<bool> EliminarItemAsync(int itemId);
        Task<bool> VaciarCarritoAsync(int carritoId);
        Task<bool> DesactivarCarritoAsync(int carritoId);
    }
}
