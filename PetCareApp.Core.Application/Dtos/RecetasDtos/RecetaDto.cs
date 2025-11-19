
namespace PetCareApp.Core.Application.Dtos.RecetasDtos
{
    public class RecetaDto
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string? Observaciones { get; set; }

        public List<RecetaMedicamentoDto> RecetaMedicamentos { get; set; } = new List<RecetaMedicamentoDto>();

        public int VeterinarioId { get; set; }
    }
}
