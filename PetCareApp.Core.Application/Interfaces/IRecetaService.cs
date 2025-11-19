using PetCareApp.Core.Application.Dtos.RecetasDtos;

public interface IRecetaService
{
    Task<RecetaDto> CreateRecetaAsync(CreateRecetaDto dto);
    Task<bool> AddMedicamentoToRecetaAsync(AddMedicamentoToRecetaDto dto);
    Task<List<RecetaDto>> GetRecetasByCitaAsync(int citaId);
}
