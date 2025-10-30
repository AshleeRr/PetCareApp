using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class CitaRepository : ICitaRepository
    {
        private readonly PetCareContext _context;

        public CitaRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<List<Cita>> GetAllAsync()
        {
            return await _context.Citas
                .Include(c => c.Estado)
                .Include(c => c.Dueño)
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
                .Include(c => c.Veterinario)
                .Include(c => c.Motivo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Cita>> GetByFechaAsync(DateTime fecha)
        {
            return await _context.Citas
                .Include(c => c.Estado)
                .Include(c => c.Dueño)
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
                .Include(c => c.Veterinario)
                .Include(c => c.Motivo)
                .Where(c => c.DueñoId == clienteId)
                .ToListAsync();
        }

        public async Task<Cita> AddAsync(Cita cita)
        {
            var entry = await _context.Citas.AddAsync(cita);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task UpdateAsync(Cita cita)
        {
            _context.Citas.Update(cita);
            await _context.SaveChangesAsync();
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
    }
}
