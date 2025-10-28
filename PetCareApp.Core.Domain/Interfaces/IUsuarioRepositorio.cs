using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IUsuarioRepositorio : IGenericRepositorio<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
       // Task AddAsync(Usuario usuario); // por que add async esta aqui si lo esta heredando
        //Task SaveChangesAsync(); //esto tmb se supone que se hereda
    }
}
