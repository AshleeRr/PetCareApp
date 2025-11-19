namespace PetCareApp.Core.Application.Dtos.RecetasDtos
{
    public class RecetaMedicamentoDto
    {
        public int MedicamentoId { get; set; }
        public string? Nombre { get; set; }
        public string? DosisIndicada { get; set; }
        public string? DuracionTratamiento { get; set; }
    }
}
