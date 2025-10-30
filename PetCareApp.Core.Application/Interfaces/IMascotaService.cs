using PetCareApp.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IMascotaService
    {
        Task<List<MascotaDto>> ObtenerTodosAsync();
        Task<MascotaDto?> ObtenerPorIdAsync(int id);
        Task<List<MascotaDto>> FiltrarAsync(string nombre, int? tipoMascotaId, bool? estaCastrado);
        Task<MascotaDto> CrearAsync(CrearMascotaDto dto);
        Task<bool> EditarAsync(int id, CrearMascotaDto dto);
        Task<bool> EliminarAsync(int id);
    }
}
