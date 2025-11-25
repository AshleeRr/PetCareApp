using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public int TipoProductoId { get; set; }
        public string TipoProducto { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; } 
        public bool DisponibleParaVenta => Stock > 0;
    }
}
