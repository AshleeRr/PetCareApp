using PetCareApp.Core.Application.ViewModels.CitasVms;
using PetCareApp.Core.Application.ViewModels.PruebasVms;
namespace PetCareApp.Core.Application.ViewModels.HistorialVms
{
    public class HistorialViewModel //mascotapruebameedica-entidad
    {
        public required string NombreMascota { get; set; }
        public List<CitaViewModel> Citas { get; set; } = new();
        public List<PruebaMedicaViewModel> Pruebas { get; set; } = new();
    }
}
