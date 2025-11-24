using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Dtos
{
    public class ActualizarStockDto
    {
        public int Cantidad { get; set; } // Puede ser positivo (agregar) o negativo (restar)
    }
}
