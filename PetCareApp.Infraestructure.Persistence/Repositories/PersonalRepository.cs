using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class PersonalRepository : IPersonalRepository
    {
        private readonly PetCareContext _context;

        public PersonalRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Personal>> GetAllAsync()
        {
            return await _context.Personal.ToListAsync();
        }

        public async Task<Personal?> GetByIdAsync(int id)
        {
            return await _context.Personal.FindAsync(id);
        }

        public async Task<Personal> AddAsync(Personal personal)
        {
            await _context.Personal.AddAsync(personal);
            await _context.SaveChangesAsync();
            return personal;
        }

        public async Task<Personal?> UpdateAsync(int id, Personal personal)
        {
            var existente = await _context.Personal.FindAsync(id);
            if (existente == null) return null;

            existente.Nombre = personal.Nombre;
            existente.Apellido = personal.Apellido;
            existente.Cedula = personal.Cedula;
            existente.Cargo = personal.Cargo;
            existente.Activo = personal.Activo;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var personal = await _context.Personal.FindAsync(id);
            if (personal == null) return false;

            _context.Personal.Remove(personal);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Personal>> BuscarAsync(string termino)
        {
            return await _context.Personal
                .Where(p => p.Nombre.Contains(termino) ||
                           p.Apellido.Contains(termino) ||
                           p.Cedula.Contains(termino))
                .ToListAsync();
        }

        // ✅ MÉTODO NUEVO
        public async Task<int> ContarTotalAsync()
        {
            return await _context.Personal.CountAsync();
        }

        // ✅ MÉTODO NUEVO
        public async Task<bool> ExisteCedulaAsync(string cedula, int? excludeId = null)
        {
            var query = _context.Personal.Where(p => p.Cedula == cedula);

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}