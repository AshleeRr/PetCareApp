using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class RegistrarDTOS
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public required string Email { get; set; }
        
        [Required(ErrorMessage = "La contraseña es requerida")]
        public required string PasswordHashed { get; set; }
    }
}
