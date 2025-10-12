namespace PetCareApp.Core.Domain.Entities;

public class MotivoCita
{
    public int Id { get; set; }

    public required string Motivo { get; set; } 

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
