using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class UsuariosPorRolDto
    {
        public int Administradores { get; set; }
        public int Veterinarios { get; set; }
        public int Recepcionistas { get; set; }
        public int Clientes { get; set; }
    }
}
