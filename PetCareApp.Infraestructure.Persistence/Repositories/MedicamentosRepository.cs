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

        public async Task<List<Medicamento?>> GetMedicamentosByPresentacion(string presentacion)
        {
            return await _context.Medicamentos
                .Where(m => m.Presentacion.Contains(presentacion.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();
        }
    }
}
