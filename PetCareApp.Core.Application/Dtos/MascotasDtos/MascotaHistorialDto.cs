
using PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos;

namespace PetCareApp.Core.Application.Dtos.MascotasDtos
{
    public class MascotaHistorialDto
    {
        public int MascotaId { get; set; }
        public string NombreMascota { get; set; } = string.Empty;
        public List<CitaDto> HistorialCitas { get; set; } = new List<CitaDto>();
        public List<MascotaPruebaMedicaHistorialDto> PruebasMedicas { get; set; } = new List<MascotaPruebaMedicaHistorialDto>();
    }
}
