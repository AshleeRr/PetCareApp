
using PetCareApp.Core.Application.Dtos.MascotasDtos;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IHistorialService
    {

        Task<MascotaHistorialDto> GetHistorialDeMascotaAsync(int mascotaId);
    }
}
