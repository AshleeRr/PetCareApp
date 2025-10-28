using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class GeneRepositorio<T> : IGenericRepositorio<T> where T : class
    {
        private readonly PetCareContext _context;
       // private readonly DbSet<T> _set;

        public GeneRepositorio(PetCareContext ctx)
        {
            _context = ctx;
           // _set = ctx.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<T?> UpdateAsync(int id, T entity) 
        {
            var entry = await _context.Set<T>().FindAsync(id);
            if(entry != null)
            {
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entity;
               
            }
            return null;
        }
        public virtual async Task RemoveAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }   
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
           return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
            => predicate == null ? await _context.Set<T>().ToListAsync() : await _context.Set<T>().Where(predicate).ToListAsync();

       // public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();

        public virtual IQueryable<T> GetAllQuery()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual IQueryable<T> GetAllQueryWithInclude(List<string> properties)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach(var property in properties)
            {
                query = query.Include(property);
            }
            return query;
        }
    }
}
