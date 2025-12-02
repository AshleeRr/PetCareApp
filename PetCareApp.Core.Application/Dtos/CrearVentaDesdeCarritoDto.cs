using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class CrearVentaDesdeCarritoDto
    {
        [Required]
        public string MetodoPago { get; set; } = "Efectivo"; // Efectivo, Tarjeta, Transferencia
    }
}
