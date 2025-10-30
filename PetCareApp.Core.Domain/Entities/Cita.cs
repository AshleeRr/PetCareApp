namespace PetCareApp.Core.Domain.Entities;
public class Cita
{
    public int Id { get; set; }

    public DateTime FechaHora { get; set; }

    public int EstadoId { get; set; }
    public required Estado Estado { get; set; } = null!;

    public int DueñoId { get; set; }
    public required Dueño Dueño { get; set; }

    public int MascotaId { get; set; }
    public required Mascota Mascota { get; set; } = null!;

    public required Personal Veterinario { get; set; } = null!;

    public int VeterinarioId { get; set; }

    public int MotivoId { get; set; }
    public required MotivoCita Motivo { get; set; } = null!;

}
