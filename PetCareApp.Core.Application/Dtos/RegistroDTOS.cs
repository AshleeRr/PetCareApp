using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public required string PasswordHashed { get; set; }

        // Opcional: si no se especifica, será "Cliente" por defecto
        public string? Role { get; set; }
    }
}
