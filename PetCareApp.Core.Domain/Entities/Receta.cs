namespace PetCareApp.Core.Domain.Entities;

public class Receta
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string? Observaciones { get; set; }

    public ICollection<RecetaMedicamento> RecetaMedicamentos { get; set; } = new List<RecetaMedicamento>();
    public Usuario? Veterinario { get; set; }
    public int VeterinarioId { get; set; }
    public int CitaId { get; set; }
    public Cita? Cita { get; set; }
    public ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();

}
