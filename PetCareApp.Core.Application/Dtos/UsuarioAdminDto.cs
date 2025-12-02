using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class UsuarioAdminDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public DateTime? UltimaConexion { get; set; }
        public bool Activo { get; set; }
    }
}
