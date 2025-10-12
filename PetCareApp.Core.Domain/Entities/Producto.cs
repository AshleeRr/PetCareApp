namespace PetCareApp.Core.Domain.Entities;

public class Producto
{
    public int Id { get; set; }

    public required string Nombre { get; set; } 

    public int Stock { get; set; }

    public decimal Precio { get; set; }

    public int TipoProductoId { get; set; }

    public ICollection<Despacho> Despachos { get; set; } = new List<Despacho>();

    public ICollection<LogInventario> LogInventarios { get; set; } = new List<LogInventario>();

    public TipoProducto TipoProducto { get; set; } = null!;

    public ICollection<Proveedor> Proveedores { get; set; } = new List<Proveedor>();
}
