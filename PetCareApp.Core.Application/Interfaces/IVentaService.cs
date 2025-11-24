using PetCareApp.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IVentaService
    {
        // Consultas
        Task<IEnumerable<VentaDto>> ObtenerTodasAsync();
        Task<VentaDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<VentaDto>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<VentaDto>> ObtenerPorRangoFechaAsync(DateTime desde, DateTime hasta);

        // Crear ventas
        Task<VentaDto> CrearVentaDesdeCarritoAsync(int usuarioId, CrearVentaDesdeCarritoDto dto);
        Task<VentaDto> CrearVentaMostradorAsync(CrearVentaMostradorDto dto); // Admin/Recepcionista
        Task<bool> CancelarVentaAsync(int ventaId);
    }
}
