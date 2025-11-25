using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class UsuarioAdminRepository : IUsuarioAdminRepository
    {
        private readonly PetCareContext _context;

        public UsuarioAdminRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            // Recargar con el Role incluido
            return await GetByIdAsync(usuario.Id) ?? usuario;
        }

        public async Task<Usuario?> UpdateAsync(int id, Usuario usuario)
        {
            var existente = await _context.Usuarios.FindAsync(id);
            if (existente == null) return null;

            existente.UserName = usuario.UserName;
            existente.Email = usuario.Email;
            existente.RoleId = usuario.RoleId;
            existente.PhotoUrl = usuario.PhotoUrl;
            existente.Activo = usuario.Activo;
            existente.PasswordHashed = usuario.PasswordHashed;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Usuario>> BuscarAsync(string termino)
        {
            return await _context.Usuarios
                .Include(u => u.Role)
                .Where(u => u.UserName.Contains(termino) || u.Email.Contains(termino))
                .ToListAsync();
        }

        // ✅ MÉTODO NUEVO
        public async Task<int> ContarTotalAsync()
        {
            return await _context.Usuarios.CountAsync();
        }

        // ✅ MÉTODO NUEVO
        public async Task<bool> ExisteEmailAsync(string email, int? excludeId = null)
        {
            var query = _context.Usuarios.Where(u => u.Email == email);

            if (excludeId.HasValue)
                query = query.Where(u => u.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}