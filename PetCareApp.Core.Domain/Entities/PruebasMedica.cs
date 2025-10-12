namespace PetCareApp.Core.Domain.Entities;
public class PruebasMedica
{
    public int Id { get; set; }

    public required string NombrePrueba { get; set; }
    public ICollection<MascotaPruebasMedica> MascotaPruebasMedicas { get; set; } = new List<MascotaPruebasMedica>();
}
