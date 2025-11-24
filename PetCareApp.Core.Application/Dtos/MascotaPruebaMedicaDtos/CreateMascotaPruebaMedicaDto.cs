
namespace PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos
{
    public class CreateMascotaPruebaMedicaDto
    {
        public int MascotaId { get; set; }
        public int PruebaMedicaId { get; set; }
        public required string Resultado { get; set; }
        public int CitaId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
