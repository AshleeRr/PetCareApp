using PetCareApp.Core.Application.Dtos.MedicamentosDtos;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IMedicamentoService : IGenericService<MedicamentoDto>
    {
        Task<MedicamentoDto> GetMedicamentoByNameAsync(string nombre);
        Task<List<MedicamentoDto>> GetMedicamentosByPresentacionAsync(string presentacion);
    }
}
