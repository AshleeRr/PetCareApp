using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface ICitaService
    {
        Task<List<CitaDto>> ObtenerCitasAsync();
        Task<CitaDto?> ObtenerPorIdAsync(int id);
        Task<List<CitaDto>> ObtenerPorFechaAsync(DateTime fecha);
        Task<List<CitaDto>> ObtenerPorClienteAsync(int clienteId);
        Task<CitaDto> CrearCitaAsync(CrearCitaDto dto);
        Task<bool> EditarCitaAsync(int id, ActualizarCitaDto dto);
        Task<bool> EliminarCitaAsync(int id);
        Task<List<CitaDto>> GetCitasAsiganasAVeterinarioAsync(int userId);

    }
}
