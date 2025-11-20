using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class RegistrarLogDto
    {
        public string Usuario { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public string Tipo { get; set; } = "INFO";
        public string Detalles { get; set; } = string.Empty;
    }
}
