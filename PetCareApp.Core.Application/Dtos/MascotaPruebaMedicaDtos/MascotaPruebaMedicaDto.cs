
namespace PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos
{
    public class MascotaPruebaMedicaDto
    {
        public required string NombreMascota { get; set; }
        public required string NombrePruebaMedica { get; set; }
        public required string Resultado { get; set; }
        public DateOnly Fecha { get; set; }
    }
}
