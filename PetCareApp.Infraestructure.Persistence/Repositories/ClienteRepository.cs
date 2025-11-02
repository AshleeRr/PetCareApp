using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly PetCareContext _context;

        public ClienteRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<List<Dueño>> GetAllAsync()
        {
            return await _context.Dueños.AsNoTracking().ToListAsync();
        }

        public async Task<Dueño?> GetByIdAsync(int id)
        {
            return await _context.Dueños.FindAsync(id);
        }

        public async Task<Dueño?> GetByCedulaAsync(string cedula)
        {
            return await _context.Dueños.FirstOrDefaultAsync(c => c.Cedula == cedula);
        }

        public async Task<List<Dueño>> FilterAsync(string nombre, string cedula)
        {
            var query = _context.Dueños.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var lower = nombre.ToLower();
                query = query.Where(c => c.Nombre.ToLower().Contains(lower) || c.Apellido.ToLower().Contains(lower));
            }

            if (!string.IsNullOrWhiteSpace(cedula))
            {
                query = query.Where(c => c.Cedula.Contains(cedula));
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Dueño> AddAsync(Dueño cliente)
        {
            var entry = await _context.Dueños.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task UpdateAsync(Dueño cliente)
        {
            _context.Dueños.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _context.Dueños.FindAsync(id);
            if (cliente != null)
            {
                _context.Dueños.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
