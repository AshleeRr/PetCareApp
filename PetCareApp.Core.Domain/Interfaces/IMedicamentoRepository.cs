
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IMedicamentoRepository : IGenericRepositorio<Medicamento>
    {
        Task<Medicamento> GetMedicamentoByNameAsync(string nombre);
        Task<List<Medicamento>> GetMedicamentosByPresentacionAsync(string presentacion);
    }
}
