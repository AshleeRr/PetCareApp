using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class CrearVentaMostradorDto
    {
        public int? ClienteId { get; set; }
        public int PersonalId { get; set; }
        public string MetodoPago { get; set; } = "Efectivo";
        public List<VentaItemDto> Items { get; set; } = new();
    }
}
