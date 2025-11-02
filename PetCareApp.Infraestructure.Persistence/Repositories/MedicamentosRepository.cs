using Microsoft.EntityFrameworkCore;
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

        public async Task<Medicamento?> GetMedicamentoByName(string nombre)
        {
            var medicamento = await _context.Medicamentos
                .FirstOrDefaultAsync(m => m.Presentacion.ToLower() == nombre.ToLower().Trim());

            if (medicamento == null)
            {
                return null;
            }
            return medicamento;
        }
    }
}
