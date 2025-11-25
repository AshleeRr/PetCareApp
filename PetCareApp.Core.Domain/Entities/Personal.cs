namespace PetCareApp.Core.Domain.Entities;
public class Personal
{
    public int Id { get; set; }

    public required string Nombre { get; set; }

    public required string Apellido { get; set; }

    public required string Cedula { get; set; } 

    public required string Cargo { get; set; }

    public bool Activo { get; set; } = true; // ✅ Nuevo


    public ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public ICollection<Despacho> Despachos { get; set; } = new List<Despacho>();

    public ICollection<LogInventario> LogInventarios { get; set; } = new List<LogInventario>();
}
