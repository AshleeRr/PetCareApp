using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class CambiarPasswordDto
    {
        [Required]
        [MinLength(6)]
        public string NuevaPassword { get; set; } = string.Empty;
    }
}
