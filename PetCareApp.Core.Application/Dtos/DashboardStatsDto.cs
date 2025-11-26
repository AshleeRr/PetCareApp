using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class DashboardStatsDto
    {
        public int TotalUsuarios { get; set; }
        public int TotalPersonal { get; set; }
        public int CitasMes { get; set; }
        public decimal IngresosMes { get; set; }
        public UsuariosPorRolDto UsuariosPorRol { get; set; } = new();
    }
}
