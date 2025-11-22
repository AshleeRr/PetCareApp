namespace PetCareApp.Core.Domain.Entities;
public class Medicamento
{
    public int Id { get; set; }

    public required string Nombre { get; set; }
    public required string Uso { get; set; } 

    public required string Presentacion { get; set; } 

    public required string EspecificadoPara { get; set; }

    public ICollection<RecetaMedicamento> RecetaMedicamentos { get; set; }
        = new List<RecetaMedicamento>();
}
