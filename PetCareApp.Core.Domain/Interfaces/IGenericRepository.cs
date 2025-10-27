namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity);
        Task<Entity?> UpdateAsync(int id, Entity entity);
        Task DeleteAsync(int id);
        Task<Entity?> GetByIdAsync(int id);
        Task<List<Entity>> GetAllAsync();
        IQueryable<Entity> GetAllQueryWithInclude(List<string> properties);
        IQueryable<Entity> GetAllQuery();
    }
}
