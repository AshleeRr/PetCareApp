using System;
using System.Threading.Tasks;

namespace PetCareApp.Core.Domain.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Envía un email de bienvenida cuando un nuevo usuario se registra
        /// </summary>
        /// <param name="destinatario">Email del destinatario</param>
        /// <param name="nombreUsuario">Nombre del usuario registrado</param>
        /// <param name="email">Email del usuario</param>
        /// <returns>True si el email se envió correctamente, False en caso contrario</returns>
        Task<bool> EnviarEmailBienvenidaAsync(string destinatario, string nombreUsuario, string email);

        /// <summary>
        /// Envía un email de notificación cuando un usuario inicia sesión
        /// </summary>
        /// <param name="destinatario">Email del destinatario</param>
        /// <param name="nombreUsuario">Nombre del usuario que inició sesión</param>
        /// <param name="ipAddress">Dirección IP desde donde se inició sesión (opcional)</param>
        /// <returns>True si el email se envió correctamente, False en caso contrario</returns>
        Task<bool> EnviarEmailLoginAsync(string destinatario, string nombreUsuario, string ipAddress = "Desconocida");

        /// <summary>
        /// Envía un email de confirmación cuando se crea una cita
        /// </summary>
        /// <param name="destinatario">Email del destinatario</param>
        /// <param name="nombreCliente">Nombre del cliente</param>
        /// <param name="fechaCita">Fecha y hora de la cita</param>
        /// <param name="nombreMascota">Nombre de la mascota (opcional)</param>
        /// <param name="tipoMascota">Tipo de mascota (opcional)</param>
        /// <param name="nombreVeterinario">Nombre del veterinario (opcional)</param>
        /// <param name="motivo">Motivo de la cita (opcional)</param>
        /// <returns>True si el email se envió correctamente, False en caso contrario</returns>
        Task<bool> EnviarEmailConfirmacionCitaAsync(
            string destinatario,
            string nombreCliente,
            DateTime fechaCita,
            string nombreMascota = "Tu mascota",
            string tipoMascota = "",
            string nombreVeterinario = "Nuestro equipo",
            string motivo = "Consulta general");

        /// <summary>
        /// Envía un email con el enlace para restablecer la contraseña
        /// </summary>
        /// <param name="destinatario">Email del destinatario</param>
        /// <param name="token">Token de restablecimiento de contraseña</param>
        /// <returns>True si el email se envió correctamente, False en caso contrario</returns>
        Task<bool> EnviarEmailResetPasswordAsync(string destinatario, string token);
    }
}