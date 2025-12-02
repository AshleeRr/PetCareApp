using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "El token es requerido")]
        public required string Token { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public required string NuevaPassword { get; set; }
    }
}
//b457925685e581f1f86e25f8fb5f45cfb313a82a
