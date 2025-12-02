using Microsoft.EntityFrameworkCore;
using PetCareApp.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using PetCareApp.Core.Domain.Entities;
using System.Threading.Tasks;

namespace PetCareApp.Infrastructure.Repositorios
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly PetCareContext _context;
        protected readonly DbSet<T> _set;

        public Repository(PetCareContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _set.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _set.Where(predicate).AsNoTracking().ToListAsync();

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _set.AsNoTracking().ToListAsync();

        public virtual async Task<T> GetByIdAsync(int id) => await _set.FindAsync(id);

        public virtual async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

