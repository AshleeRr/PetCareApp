using System;

namespace PetCareApp.Core.Application.Dtos
{
    public class CrearCitaDto
    {
        public DateTime FechaHora { get; set; }
        public int EstadoId { get; set; }
        public int DueñoId { get; set; }
        public int VeterinarioId { get; set; }
        public int MotivoId { get; set; }
    }
}
