public class AddMedicamentoToRecetaDto
{
    public int RecetaId { get; set; }
    public int MedicamentoId { get; set; }
    public string? Dosis { get; set; }
    public string? Duracion { get; set; }
    public string? Observaciones { get; set; }
}
