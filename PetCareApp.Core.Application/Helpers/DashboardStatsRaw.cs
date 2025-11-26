using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Helpers
{
    public class DashboardStatsRaw
    {
        public int TotalUsuarios { get; set; }
        public int TotalPersonal { get; set; }
        public int CitasMes { get; set; }
        public decimal IngresosMes { get; set; }
        public int Administradores { get; set; }
        public int Veterinarios { get; set; }
        public int Recepcionistas { get; set; }
        public int Clientes { get; set; }
    }
}
