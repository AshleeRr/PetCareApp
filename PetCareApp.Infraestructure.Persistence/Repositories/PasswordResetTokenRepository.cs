using Microsoft.EntityFrameworkCore;
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
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly PetCareContext _context;

        public PasswordResetTokenRepository(PetCareContext context)
        {
            _context = context;
        }

        public async Task<PasswordResetToken> AddAsync(PasswordResetToken token)
        {
            var entry = await _context.PasswordResetTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<PasswordResetToken?> GetByTokenAsync(string token)
        {
            return await _context.PasswordResetTokens
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task UpdateAsync(PasswordResetToken token)
        {
            _context.PasswordResetTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
