
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IMedicamentoRepository : IGenericRepositorio<Medicamento>
    {
        Task<Medicamento?> GetMedicamentoByName(string nombre);
    }
}
