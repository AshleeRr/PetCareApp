using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class CrearClienteDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        public required string Apellido { get; set; }

        public string? Direccion { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        public required string Cedula { get; set; }

        // ✅ Agregar Email con validación
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string? Email { get; set; }


    }
}
