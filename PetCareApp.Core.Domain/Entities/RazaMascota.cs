namespace PetCareApp.Core.Domain.Entities;

public class RazaMascota
{
    public int Id { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
