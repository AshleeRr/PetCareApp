namespace PetCareApp.Core.Domain.Entities;
public class Medicamento
{
    public int Id { get; set; }

    public string Uso { get; set; } = null!;

    public required string Presentacion { get; set; } 

    public string? EspecificadoPara { get; set; }

    public ICollection<RecetaMedicamento> RecetaMedicamentos { get; set; } = new List<RecetaMedicamento>();
}
