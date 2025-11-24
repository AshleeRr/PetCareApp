using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class VerificarTokenDto
    {
        public bool TokenValido { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
