namespace PetCareApp.Core.Application.Interfaces
{
    public interface IGenericService<Entity,Dto> where Dto : class where Entity : class
    {
        Task<Dto?> CreateAsync(Dto dto);
        Task<Dto?> UpdateAsync(Dto dto, int id);
        Task<bool> DeleteAsync(int id); //si es true, se elimino correctamente
        Task<List<Dto?>> GetAllAsync();
        Task<Dto?> GetByIdAsync(int id);
    }
}
