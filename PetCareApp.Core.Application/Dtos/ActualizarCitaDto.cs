using System;

namespace PetCareApp.Core.Application.Dtos
{
    public class ActualizarCitaDto
    {
        public DateTime FechaHora { get; set; }
        public int EstadoId { get; set; }
        public int VeterinarioId { get; set; }
        public int MascotaId { get; set; }  
        public int MotivoId { get; set; }
        public string Observaciones { get; set; }
    }
}