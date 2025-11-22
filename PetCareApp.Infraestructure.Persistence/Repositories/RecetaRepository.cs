
using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class RecetaRepository : GeneRepositorio<Receta>, IRecetaRepository
    {
        private readonly PetCareContext _context;
        public RecetaRepository(PetCareContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddMedicamentoToRecetaAsync(RecetaMedicamento relacion)
        {
            await _context.RecetaMedicamentos.AddAsync(relacion);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Receta>> GetByCitaIdAsync(int citaId)
        {
            return await _context.Recetas
            .Include(r => r.RecetaMedicamentos)
                .ThenInclude(rm => rm.Medicamento)
            .Where(r => r.CitaId == citaId)
            .ToListAsync();
        }
    }
}
