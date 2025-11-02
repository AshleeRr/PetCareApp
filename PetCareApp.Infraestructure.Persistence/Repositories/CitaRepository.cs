using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class CitaRepository : GeneRepositorio<Cita>, ICitaRepository
    {
        private readonly PetCareContext _context;

        public CitaRepository(PetCareContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Cita>> GetCitasByDate(DateOnly date)
        {
            return await _context.Citas.Where(c => DateOnly.FromDateTime(c.FechaHora) == date).ToListAsync();
        }

        public async Task<List<Cita>> GetCitasOfMascotaById(int mascotaId)
        {
            return await _context.Citas
                .Include(c => c.Veterinario)
                .Include(c => c.Estado)
                .Include(c => c.Motivo)
                .Include(c => c.Dueño)
                .Where(c => c.MascotaId == mascotaId)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();
        }
    }
}
