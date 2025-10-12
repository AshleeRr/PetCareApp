namespace PetCareApp.Core.Domain.Entities;
public class Proveedor
{
    public int Id { get; set; }

    public required string Nombre { get; set; } 

    public string? Direccion { get; set; }

    public string? Contacto { get; set; }

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
