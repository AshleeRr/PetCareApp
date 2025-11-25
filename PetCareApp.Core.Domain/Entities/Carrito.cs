using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Entities
{
    public class Carrito
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public ICollection<CarritoItem> Items { get; set; } = new List<CarritoItem>();
    }
}
