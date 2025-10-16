using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IAutenticacionService
    {
        Task<Usuario> RegistrarAsync(RegistroDto dto);
        Task<Usuario?> LoginAsync(RegistrarDTOS dto);
    }
}
