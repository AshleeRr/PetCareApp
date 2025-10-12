namespace PetCareApp.Core.Domain.Entities;

public class LogInventario
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public int PersonalId { get; set; }

    public int Cantidad { get; set; }

    public required string TipoMovimiento { get; set; } 

    public DateTime Fecha { get; set; }

    public string? Observaciones { get; set; }

    public required Personal Personal { get; set; }

    public required Producto Producto { get; set; }
}
