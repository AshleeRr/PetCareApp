namespace PetCareApp.Core.Domain.Entities;

public class Despacho
{
    public int Id { get; set; }

    public int ProductoId { get; set; }
    public required Producto Producto { get; set; }

    public int PersonalId { get; set; }
    public required Personal Personal { get; set; }

    public DateTime Fecha { get; set; }
    
}
