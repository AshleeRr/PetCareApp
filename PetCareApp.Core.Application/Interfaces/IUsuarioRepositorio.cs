using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCareApp.Core.Domain.Entities;

namespace Dominio.Interfaces
{
    public interface IUsuarioRepositorio : IGeneRepositorio<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task AddAsync(Usuario usuario); // por que add async esta aqui si lo esta heredando
        Task SaveChangesAsync(); //esto tmb se supone que se hereda
    }
}
