

namespace PetCareApp.Core.Application.ViewModels.PruebasVms
{
    public class PruebaViewModel
    {
        public int Id { get; set; }
        public string NombrePrueba { get; set; }
        public string Resultado { get; set; }
        public DateOnly FechaRealizacion { get; set; }
        public string Observaciones { get; set; }
    }
}
