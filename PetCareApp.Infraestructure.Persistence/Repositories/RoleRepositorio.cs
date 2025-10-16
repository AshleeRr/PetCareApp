using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Application.Interfaceson;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace Infraestructura.Persistencia.Repositorios
{
    public class RoleRepositorio : GeneRepositorio<Role>, IRoleRepositorio // Si hereda de GeneRepositorio
    {
        private readonly PetCareContext _context;

        public RoleRepositorio(PetCareContext context) : base(context) // ✅ Pasar al constructor base
        {
            _context = context;
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Rol == roleName);
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}