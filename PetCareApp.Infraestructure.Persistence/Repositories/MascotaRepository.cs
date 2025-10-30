using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class MascotaRepository : IMascotaRepository
    {
        private readonly PetCareContext _context;
        public MascotaRepository(PetCareContext context) => _context = context;

        public async Task<List<Mascota>> GetAllAsync()
        {
            return await _context.Mascota.Include(m => m.TipoMascota).AsNoTracking().ToListAsync();
        }

        public async Task<Mascota?> GetByIdAsync(int id)
        {
            return await _context.Mascota.Include(m => m.TipoMascota).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Mascota>> FilterAsync(string nombre, int? tipoMascotaId, bool? estaCastrado)
        {
            var q = _context.Mascota.AsQueryable();
            if (!string.IsNullOrWhiteSpace(nombre))
                q = q.Where(m => m.Nombre.Contains(nombre));
            if (tipoMascotaId.HasValue)
                q = q.Where(m => m.TipoMascotaId == tipoMascotaId.Value);
            if (estaCastrado.HasValue)
                q = q.Where(m => m.EstaCastrado == estaCastrado.Value);

            return await q.Include(m => m.TipoMascota).AsNoTracking().ToListAsync();
        }

        public async Task<Mascota> AddAsync(Mascota mascota)
        {
            var e = await _context.Mascota.AddAsync(mascota);
            await _context.SaveChangesAsync();
            return e.Entity;
        }

        public async Task UpdateAsync(Mascota mascota)
        {
            _context.Mascota.Update(mascota);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var m = await _context.Mascota.FindAsync(id);
            if (m != null)
            {
                _context.Mascota.Remove(m);
                await _context.SaveChangesAsync();
            }
        }
    }
}
