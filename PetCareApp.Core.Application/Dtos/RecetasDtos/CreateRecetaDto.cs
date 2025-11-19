
namespace PetCareApp.Core.Application.Dtos.RecetasDtos
{
    public class CreateRecetaDto
    {
        public string? Observaciones { get; set; }
        public int VeterinarioId { get; set; }
        public int CitaId { get; set; }
    }
}
