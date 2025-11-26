using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos.UsersDtos
{
    public class CrearUsuarioRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; } // En app real, enviar plain => hash en service
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
}
