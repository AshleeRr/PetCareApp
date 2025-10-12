namespace PetCareApp.Core.Domain.Entities;

public class TipoMascota
{
    public int Id { get; set; }

    public required string Tipo { get; set; } 

    public ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
