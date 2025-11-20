using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Entities
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int? PersonalId { get; set; }
        public Personal? Personal { get; set; }
        public int? ClienteId { get; set; }
        public Dueño? Cliente { get; set; }
        public int? UsuarioId { get; set; } // ✅ Usuario que compra online
        public Usuario? Usuario { get; set; }
        public int? CitaId { get; set; }
        public Cita? Cita { get; set; }
        public string MetodoPago { get; set; } = "Efectivo";
        public string Estado { get; set; } = "Pendiente"; // ✅ Pendiente, Completada, Cancelada
        public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();
    }
}

