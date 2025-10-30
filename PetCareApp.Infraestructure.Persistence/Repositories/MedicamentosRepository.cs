using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class MedicamentosRepository : GeneRepositorio<Medicamento>, IMedicamentoRepository
    {
        private readonly PetCareContext _context;
        public MedicamentosRepository(PetCareContext context) : base(context)
        {
            _context = context;
        }

        public Task<Medicamento> GetMedicamentoByName(string nombre)
        {
            throw new NotImplementedException();
        }
    }
}
