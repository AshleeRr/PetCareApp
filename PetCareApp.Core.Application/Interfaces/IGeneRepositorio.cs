using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interfaces
{
    public interface IGeneRepositorio<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync();
    }
}
