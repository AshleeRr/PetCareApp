using System;

namespace PetCareApp.Core.Application.Dtos
{
    public class CitaDto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Veterinario { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
    }
}
