using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class TratamientoRepository : GeneRepositorio<MascotaPruebasMedica>, ITratamientoRepository
    {
        private readonly PetCareContext _context;
        public TratamientoRepository(PetCareContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<MascotaPruebasMedica>> GetPruebasOfMascotaById(int mascotaId)
        {
            return await _context.MascotaPruebasMedicas
                    .Include(pm => pm.PruebaMedica)
                    .Where(pm => pm.MascotaId == mascotaId)
                    .OrderByDescending(pm => pm.Fecha) 
                    .ToListAsync();
        }
    }
}
