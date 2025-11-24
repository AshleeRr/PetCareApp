using System;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Core.Application.Dtos
{
    public class CrearCitaDto
    {
        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        public int EstadoId { get; set; }

        [Required]
        public int DueñoId { get; set; }

        [Required]
        public int MascotaId { get; set; } 

        [Required]
        public int VeterinarioId { get; set; }

        [Required]
        public int MotivoId { get; set; }

        public string Observaciones { get; set; }
    }
}
