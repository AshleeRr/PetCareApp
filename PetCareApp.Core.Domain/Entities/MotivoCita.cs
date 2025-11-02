namespace PetCareApp.Core.Domain.Entities;

public class MotivoCita
{
    public int Id { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();
}
