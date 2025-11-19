
namespace PetCareApp.Core.Application.Dtos.MedicamentosDtos
{
    public class MedicamentoDto
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public required string Uso { get; set; }

        public required string Presentacion { get; set; }

        public required string EspecificadoPara { get; set; }
    }
}
