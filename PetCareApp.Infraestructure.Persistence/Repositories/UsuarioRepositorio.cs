using Infraestructura.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class UsuarioRepositorio : GeneRepositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly PetCareContext _context;

        public UsuarioRepositorio(PetCareContext context) : base(context) // ✅ Pasar context al constructor base
        {
            _context = context;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .Include(u => u.Role) // ✅ IMPORTANTE: Cargar la relación Role
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        /*
        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }*/
    }
}