using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCareApp.Core.Application.Dtos;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IClienteService
    {
        Task<List<ClienteDto>> ObtenerClientesAsync();
        Task<ClienteDto?> ObtenerPorIdAsync(int id);
        Task<ClienteDto?> ObtenerPorCedulaAsync(string cedula);
        Task<ClienteDto> CrearClienteAsync(CrearClienteDto dto);
        Task<bool> EditarClienteAsync(int id, ActualizarClienteDto dto);
        Task<bool> EliminarClienteAsync(int id);
        Task<List<ClienteDto>> FiltrarPorNombreAsync(string nombre, string cedula);
    }
}
