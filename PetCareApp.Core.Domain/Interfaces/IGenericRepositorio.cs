using System.Linq.Expressions;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IGenericRepositorio<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
        IQueryable<T> GetAllQuery();
        IQueryable<T> GetAllQueryWithInclude(List<string> properties);
        Task<T> AddAsync(T entity);
        Task<T?> UpdateAsync(int id,T entity); 
        Task RemoveAsync(int id); 

        //Task<int> SaveChangesAsync();
    }
}
