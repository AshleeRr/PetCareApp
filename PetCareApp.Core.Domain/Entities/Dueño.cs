namespace PetCareApp.Core.Domain.Entities;

public class Dueño
{
    public int Id { get; set; }

    public required string Nombre { get; set; } 

    public required string Apellido { get; set; }

    public required string Direccion { get; set; }

    public required string Cedula { get; set; }
    
    //public string? Email { get; set; }

    public ICollection<Cita>? Cita { get; set; } 

    public ICollection<Mascota>? Mascota { get; set; } 

    public ICollection<Telefono>? Telefonos { get; set; }
}
