using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IRecetaRepository : IGenericRepositorio<Receta>
    {
        Task AddMedicamentoToRecetaAsync(RecetaMedicamento relacion);
        Task<List<Receta>> GetByCitaIdAsync(int citaId);

    }
}
