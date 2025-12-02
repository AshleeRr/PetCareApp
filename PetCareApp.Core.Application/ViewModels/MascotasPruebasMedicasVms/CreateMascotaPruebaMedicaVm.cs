
namespace PetCareApp.Core.Application.ViewModels.MascotasPruebasMedicasVms
{
    public class CreateMascotaPruebaMedicaVm
    {
        public int MascotaId { get; set; }
        public int PruebaMedicaId { get; set; }
        public required string Resultado { get; set; }
        public int CitaId { get; set; }
       // public DateTime Fecha { get; set; }
    }
}
