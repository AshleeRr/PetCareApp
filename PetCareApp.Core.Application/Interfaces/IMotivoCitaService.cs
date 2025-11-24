using PetCareApp.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IMotivoCitaService
    {
        Task<List<MotivoCitaDto>> ObtenerMotivosAsync();
        Task<MotivoCitaDto?> ObtenerPorIdAsync(int id);
        Task<MotivoCitaDto> CrearAsync(CrearMotivoCitaDto dto);
        Task<MotivoCitaDto?> ActualizarAsync(int id, ActualizarMotivoCitaDto dto);
        Task<bool> EliminarAsync(int id);
    }
}
