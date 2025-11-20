namespace PetCareApp.Core.Domain.Entities;
public class Cita
{
    public int Id { get; set; }

    public DateTime FechaHora { get; set; }

    public int EstadoId { get; set; }
    public Estado Estado { get; set; } = null!;

    public int DueñoId { get; set; }
    public Dueño Dueño { get; set; } = null!;

    public int MascotaId { get; set; }
    public Mascota Mascota { get; set; } = null!;

    public Personal? Veterinario { get; set; }
    public int VeterinarioId { get; set; }

    public int MotivoId { get; set; }
    public MotivoCita Motivo { get; set; } = null!;


    public Dueño? Dueños { get; set; }
    public Mascota? Mascotas { get; set; }
    public Personal? Veterinarios { get; set; }
}

