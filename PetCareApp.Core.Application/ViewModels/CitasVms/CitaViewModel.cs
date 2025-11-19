
namespace PetCareApp.Core.Application.ViewModels.CitasVms
{
    public class CitaViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public required string Motivo { get; set; }
        public required string Estado { get; set; }

    }
}
