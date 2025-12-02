using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class TipoMascotaDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }
}
