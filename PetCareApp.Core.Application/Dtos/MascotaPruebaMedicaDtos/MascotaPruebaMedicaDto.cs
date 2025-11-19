
namespace PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos
{
    public class MascotaPruebaMedicaDto
    {
        public int MascotaId { get; set; }
        public int PruebaMedicaId { get; set; }
        public required string Resultado { get; set; }
    }
}
