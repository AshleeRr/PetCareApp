namespace PetCareApp.Core.Domain.Entities;

public class TipoProducto
{
    public int Id { get; set; }

    public required string Tipo { get; set; } 

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
