using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IEmailService
    {
        Task<bool> EnviarEmailResetPasswordAsync(string destinatario, string token);
        Task<bool> EnviarEmailConfirmacionCitaAsync(string destinatario, string nombreCliente, DateTime fechaCita);
    }
}
