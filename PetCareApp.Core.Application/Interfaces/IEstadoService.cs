using PetCareApp.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IEstadoService
    {
        Task<List<EstadoDto>> ObtenerEstadosAsync();
    }
}
