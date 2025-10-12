namespace PetCareApp.Core.Domain.Entities;

public class MascotaPruebasMedica
{
    public int MascotaId { get; set; }

    public int PruebaMedicaId { get; set; }

    public  string Resultado { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public Mascota Mascota { get; set; } = null!;

    public PruebasMedica PruebaMedica { get; set; } = null!;
}
