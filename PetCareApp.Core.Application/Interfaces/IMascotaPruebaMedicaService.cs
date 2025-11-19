
using PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IMascotaPruebaMedicaService
    {
        Task<bool> CrearPruebaParaMascotaAsync(CreateMascotaPruebaMedicaDto dto);
        Task<List<MascotaPruebaMedicaDto>> GetPruebasMedicasOfMascotaById(int mascotaId);
    }
}
