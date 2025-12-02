using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class ActualizarClienteDto
    {
        [Required]
        public required string Nombre { get; set; }

        [Required]
        public required string Apellido { get; set; }

        public string? Direccion { get; set; }

        [Required]
        public required string Cedula { get; set; }

     
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string? Email { get; set; }
    }
}
