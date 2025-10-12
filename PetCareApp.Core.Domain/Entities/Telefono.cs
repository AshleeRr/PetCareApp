namespace PetCareApp.Core.Domain.Entities;
public class Telefono
{
    public int Id { get; set; }

    public required string Contacto { get; set; } 

    public string? Tipo { get; set; }

    public int DueñoId { get; set; }

    public Dueño Dueño { get; set; } = null!;
}
