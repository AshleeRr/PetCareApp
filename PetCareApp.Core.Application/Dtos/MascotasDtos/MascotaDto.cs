using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos.MascotasDtos
{
    public class MascotaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public decimal Peso { get; set; }
        public bool EstaCastrado { get; set; }
        public int DueñoId { get; set; }
        public int TipoMascotaId { get; set; }
    }
}
