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
    public class MotivoCitaRepository :IMotivoCitaRepository
    {
        private readonly PetCareContext _context;
        public MotivoCitaRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<List<MotivoCita>> GetAllAsync()
        {
            return await _context.MotivoCita.AsNoTracking().ToListAsync();
        }

        public async Task<MotivoCita?> GetByIdAsync(int id)
        {
            return await _context.MotivoCita.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MotivoCita> AddAsync(MotivoCita motivo)
        {
            await _context.MotivoCita.AddAsync(motivo);
            await _context.SaveChangesAsync();
            return motivo;
        }

        public async Task<MotivoCita?> UpdateAsync(MotivoCita motivo)
        {
            var existe = await _context.MotivoCita.AnyAsync(m => m.Id == motivo.Id);
            if (!existe) return null;

            _context.MotivoCita.Update(motivo);
            await _context.SaveChangesAsync();
            return motivo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var motivo = await _context.MotivoCita.FindAsync(id);
            if (motivo == null) return false;

            _context.MotivoCita.Remove(motivo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
