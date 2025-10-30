namespace PetCareApp.Core.Domain.Entities;

public class Mascota
{
    public int Id { get; set; }

    public required string Nombre { get; set; }

    public int Edad { get; set; }

    public decimal Peso { get; set; }

    public bool EstaCastrado { get; set; }

    public int DueñoId { get; set; }

    public int TipoMascotaId { get; set; }

    public required Dueño Dueño { get; set; }

    public ICollection<MascotaPruebasMedica> MascotaPruebasMedicas { get; set; } = new List<MascotaPruebasMedica>();
    public ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public required TipoMascota TipoMascota { get; set; }
}
