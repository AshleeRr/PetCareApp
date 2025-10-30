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
            return await _context.MotivoCita.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
