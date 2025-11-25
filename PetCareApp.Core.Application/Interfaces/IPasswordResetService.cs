using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IPasswordResetService
    {
        Task<bool> SolicitarResetPasswordAsync(string email);
        Task<bool> VerificarTokenAsync(string token);
        Task<bool> ResetPasswordAsync(string token, string nuevaPassword);
    }
}
