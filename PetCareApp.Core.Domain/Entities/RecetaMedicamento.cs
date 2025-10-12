namespace PetCareApp.Core.Domain.Entities;

public class RecetaMedicamento
{
    public int RecetaId { get; set; }

    public int MedicamentoId { get; set; }

    public required string DosisIndicada { get; set; }

    public required string DuracionTratamiento { get; set; }

    public string? Observaciones { get; set; }

    public Medicamento Medicamento { get; set; } = null!;

    public Receta Receta { get; set; } = null!;
}
