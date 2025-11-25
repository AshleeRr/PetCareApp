using PetCareApp.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IProductoService
    {
        // Consultas (todos los roles)
        Task<IEnumerable<ProductoDto>> ObtenerTodosAsync();
        Task<ProductoDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<ProductoDto>> ObtenerPorTipoAsync(int tipoProductoId);
        Task<IEnumerable<ProductoDto>> BuscarPorNombreAsync(string nombre);
        Task<IEnumerable<ProductoCatalogoDto>> ObtenerCatalogoAsync(); // Solo disponibles

        // Gestión (Admin y Recepcionista)
        Task<ProductoDto> CrearProductoAsync(CrearProductoDto dto);
        Task<ProductoDto?> ActualizarProductoAsync(int id, ActualizarProductoDto dto);
        Task<bool> EliminarProductoAsync(int id);
        Task<bool> ActualizarStockAsync(int id, int cantidad); // Para ventas
    }
}
