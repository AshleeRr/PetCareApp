using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.Repositorios
{
    public class GeneRepositorio<T> : IGeneRepositorio<T> where T : class
    {
        protected readonly DbContext _ctx;
        protected readonly DbSet<T> _set;

        public GeneRepositorio(DbContext ctx)
        {
            _ctx = ctx;
            _set = ctx.Set<T>();
        }

        public async Task AddAsync(T entity) => await _set.AddAsync(entity);
        public void Update(T entity) => _set.Update(entity);
        public void Remove(T entity) => _set.Remove(entity);

        public async Task<T?> GetByIdAsync(int id) => await _set.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
            => predicate == null ? await _set.ToListAsync() : await _set.Where(predicate).ToListAsync();

        public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
    }
}
