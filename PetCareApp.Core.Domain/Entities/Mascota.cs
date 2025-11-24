namespace PetCareApp.Core.Domain.Entities;

public class Mascota
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public decimal Peso { get; set; }
    public bool EstaCastrado { get; set; }

    // FK Dueño
    public int DueñoId { get; set; }
    public Dueño? Dueño { get; set; }

    // FK TipoMascota
    public int TipoMascotaId { get; set; }
    public TipoMascota? TipoMascota { get; set; }

    // ✅ NUEVO: RazaId
    public int? RazaId { get; set; }
    public RazaMascota? Raza { get; set; }

    // ✅ NUEVO: PhotoUrl
    public string? PhotoUrl { get; set; }

    // Navegación
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public ICollection<MascotaPruebasMedica> MascotaPruebasMedicas  { get; set; } = new List<MascotaPruebasMedica>();
}
