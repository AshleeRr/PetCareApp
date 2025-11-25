using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class ActualizarCarritoItemDto
    {
        [Required]
        [Range(1, 100)]
        public int Cantidad { get; set; }
    }
}
