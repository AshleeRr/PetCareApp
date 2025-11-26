using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IMotivoCitaRepository
    {
        Task<List<MotivoCita>> GetAllAsync();
        Task<MotivoCita?> GetByIdAsync(int id);
        Task<MotivoCita> AddAsync(MotivoCita motivo);
        Task<MotivoCita?> UpdateAsync(MotivoCita motivo);
        Task<bool> DeleteAsync(int id);
    }
}
