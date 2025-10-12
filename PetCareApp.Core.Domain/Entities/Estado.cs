namespace PetCareApp.Core.Domain.Entities;
public class Estado
{
    public int Id { get; set; }

    public required string Nombre { get; set; }
    public required  ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
