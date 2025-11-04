
namespace PetCareApp.Core.Application.ViewModels.CitasVms
{
    public class CitaDetalleViewModel
    {
        public int Id{ get; set; }
        public DateOnly FechaHora { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public string VeterinarioNombre { get; set; }
        public string MascotaNombre { get; set; }
        public string DueñoNombre { get; set; }
        public string RazasMascota { get; set; }
        public string TipoMascota { get; set; }
        public string EdadMascota { get; set; }
    }
}
