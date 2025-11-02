namespace PetCareApp.Core.Domain.Entities;

public class TipoMascota
{
    public int Id { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public ICollection<Mascota> Mascota { get; set; } = new List<Mascota>();
}
