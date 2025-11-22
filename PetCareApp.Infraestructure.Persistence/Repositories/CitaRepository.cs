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

        // ====================================
        // MÉTODOS BÁSICOS
        // ====================================

        public async Task<List<Cita>> GetAllAsync()
        {
            return await _context.Citas
                .Include(c => c.Estado)
                .Include(c => c.Dueño)
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .Include(c => c.Motivo)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Cita?> GetByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Estado)
                .Include(c => c.Dueño)
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .Include(c => c.Motivo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cita> AddAsync(Cita cita)
        {
            var entry = await _context.Citas.AddAsync(cita);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Cita?> UpdateAsync(Cita cita)
        {
            var existente = await _context.Citas.FindAsync(cita.Id);
            if (existente == null) return null;

            _context.Entry(existente).CurrentValues.SetValues(cita);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(cita.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return false;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return true;
        }

        // ====================================
        // CONSULTAS ESPECÍFICAS
        // ====================================

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

        public async Task<List<Cita>> GetByFechaAsync(DateTime fecha)
        {
            return await _context.Citas
                .Include(c => c.Estado)
                .Include(c => c.Dueño)
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .Include(c => c.Motivo)
                .Where(c => c.FechaHora.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<List<Cita>> GetByClienteAsync(int clienteId)
        {
            return await _context.Citas
                .Include(c => c.Estado)
                .Include(c => c.Dueño)
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .Include(c => c.Motivo)
                .Where(c => c.DueñoId == clienteId)
                .ToListAsync();
        }

        public async Task<List<Cita>> GetCitasAsiganasAVeterinarioAsync(int userId)
        {
            return await _context.Citas
                .Include(c => c.Mascota)
                .Include(c => c.Dueño)
                .Include(c => c.Estado)
                .Include(c => c.Motivo)
                .Where(c => c.VeterinarioId == userId)
                .ToListAsync();
        }
        /*
public async Task<Cita> AddAsync(Cita cita)
{
   var entry = await _context.Citas.AddAsync(cita);
   await _context.SaveChangesAsync();
   return entry.Entity;
}

        // ====================================
        // MÉTODOS PARA ADMIN
        // ====================================

        public async Task<int> ContarCitasMesActualAsync()
        {
            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddDays(-1);

            return await _context.Citas
                .Where(c => c.FechaHora >= inicioMes && c.FechaHora <= finMes) // ✅ Usar FechaHora
                .CountAsync();
        }
public async Task UpdateAsync(Cita cita)
{
   _context.Citas.Update(cita);
   await _context.SaveChangesAsync();
}

        public async Task<IEnumerable<Cita>> ObtenerCitasPorRangoFechaAsync(DateTime desde, DateTime hasta)
        {
            return await _context.Citas
                .Include(c => c.Estado)
                .Include(c => c.Dueño)
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .Include(c => c.Motivo)
                .Where(c => c.FechaHora >= desde && c.FechaHora <= hasta) // ✅ Usar FechaHora
                .ToListAsync();
        }
public async Task DeleteAsync(int id)
{
   var cita = await _context.Citas.FindAsync(id);
   if (cita != null)
{
       _context.Citas.Remove(cita);
       await _context.SaveChangesAsync();
   }
}
*/
    }
}