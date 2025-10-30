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
    public class EstadoRepository : IEstadoRepository
    {
        private readonly PetCareContext _context;
        public EstadoRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<List<Estado>> GetAllAsync()
        {
            return await _context.Estados.AsNoTracking().ToListAsync();
        }

        public async Task<Estado?> GetByIdAsync(int id)
        {
            return await _context.Estados.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
