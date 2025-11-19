
namespace PetCareApp.Core.Application.ViewModels.MascotasPruebasMedicasVms
{
    public class MascotaPruebaMedicaVm
    {
        public int MascotaId { get; set; }
        public int PruebaMedicaId { get; set; }
        public required string Resultado { get; set; }
    }
}
