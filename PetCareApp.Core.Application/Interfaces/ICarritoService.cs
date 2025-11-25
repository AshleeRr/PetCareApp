using PetCareApp.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface ICarritoService
    {
        Task<CarritoDto> ObtenerCarritoUsuarioAsync(int usuarioId);
        Task<CarritoDto> AgregarProductoAsync(int usuarioId, AgregarAlCarritoDto dto);
        Task<CarritoDto> ActualizarCantidadAsync(int usuarioId, int itemId, ActualizarCarritoItemDto dto);
        Task<bool> EliminarItemAsync(int usuarioId, int itemId);
        Task<bool> VaciarCarritoAsync(int usuarioId);
    }
}
