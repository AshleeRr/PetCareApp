namespace PetCareApp.Core.Domain.Entities;
public class Estado
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public required  ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
