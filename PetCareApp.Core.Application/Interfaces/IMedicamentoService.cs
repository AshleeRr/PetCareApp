using PetCareApp.Core.Application.Dtos.MedicamentosDtos;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IMedicamentoService : IGenericService<Medicamento, MedicamentoDto>
    {
        Task<MedicamentoDto> GetMedicamentoByNameAsync(string nombre);
        Task<List<MedicamentoDto>> GetMedicamentosByPresentacionAsync(string presentacion);
    }
}
