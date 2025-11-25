using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class CarritoDto
    {
        public int Id { get; set; }
        public List<CarritoItemDto> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Subtotal);
        public int CantidadTotal => Items.Sum(i => i.Cantidad);
    }
}
