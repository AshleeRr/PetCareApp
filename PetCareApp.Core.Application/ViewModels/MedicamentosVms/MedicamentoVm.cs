namespace PetCareApp.Core.Application.ViewModels.MedicamentosVms
{
    public class MedicamentoVm
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public required string Uso { get; set; }

        public required string Presentacion { get; set; }

        public required string EspecificadoPara { get; set; }
    }
}
