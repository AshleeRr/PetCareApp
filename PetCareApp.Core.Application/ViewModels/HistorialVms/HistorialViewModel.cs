using PetCareApp.Core.Application.ViewModels.CitasVms;
using PetCareApp.Core.Application.ViewModels.PruebasVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.ViewModels.HistorialVms
{
    public class HistorialViewModel
    {
        public string NombreMascota { get; set; }
        public List<CitaViewModel> Citas { get; set; } = new();
        public List<PruebaViewModel> Pruebas { get; set; } = new();
    }
}
