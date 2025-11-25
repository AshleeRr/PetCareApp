using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class SistemaLogRepository : ISistemaLogRepository
    {
        private readonly PetCareContext _context;

        public SistemaLogRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SistemaLog>> GetAllAsync()
        {
            return await _context.SistemaLogs
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<SistemaLog?> GetByIdAsync(int id)
        {
            return await _context.SistemaLogs.FindAsync(id);
        }

        public async Task<SistemaLog> AddAsync(SistemaLog log)
        {
            await _context.SistemaLogs.AddAsync(log);
            await _context.SaveChangesAsync();
            return log;
        }

        // ✅ MÉTODO NUEVO
        public async Task<IEnumerable<SistemaLog>> GetByFiltrosAsync(string? tipo, DateTime? fecha)
        {
            var query = _context.SistemaLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo))
                query = query.Where(l => l.Tipo == tipo);

            if (fecha.HasValue)
            {
                var fechaInicio = fecha.Value.Date;
                var fechaFin = fechaInicio.AddDays(1);
                query = query.Where(l => l.Timestamp >= fechaInicio && l.Timestamp < fechaFin);
            }

            return await query
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }
    }
}