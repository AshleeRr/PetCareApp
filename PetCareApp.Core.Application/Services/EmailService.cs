using Infraestructura.Servicios;
using Microsoft.Extensions.Options;
using PetCareApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly ConfiguracionServices2 _config;

        public EmailService(IOptions<ConfiguracionServices2> config)
        {
            _config = config.Value;
        }

        public async Task<bool> EnviarEmailResetPasswordAsync(string destinatario, string token)
        {
            try
            {
                // URL del frontend para resetear contraseña
                // En desarrollo, puedes usar localhost del frontend
                var resetUrl = $"http://localhost:3000/reset-password?token={token}";
                // En producción: var resetUrl = $"https://tudominio.com/reset-password?token={token}";

                var subject = "🔐 Restablecer Contraseña - PetCare";

                var body = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            body {{ 
                                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
                                line-height: 1.6; 
                                color: #333; 
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{ 
                                max-width: 600px; 
                                margin: 20px auto; 
                                background-color: white;
                                border-radius: 10px;
                                overflow: hidden;
                                box-shadow: 0 0 20px rgba(0,0,0,0.1);
                            }}
                            .header {{ 
                                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                                color: white; 
                                padding: 30px 20px; 
                                text-align: center; 
                            }}
                            .header h1 {{ 
                                margin: 0; 
                                font-size: 28px; 
                            }}
                            .content {{ 
                                padding: 40px 30px; 
                            }}
                            .content h2 {{
                                color: #667eea;
                                margin-top: 0;
                            }}
                            .button {{ 
                                display: inline-block; 
                                padding: 15px 40px; 
                                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                                color: white !important; 
                                text-decoration: none; 
                                border-radius: 50px;
                                margin: 25px 0;
                                font-weight: bold;
                                font-size: 16px;
                                box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
                                transition: all 0.3s ease;
                            }}
                            .button:hover {{
                                transform: translateY(-2px);
                                box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
                            }}
                            .warning {{
                                background-color: #fff3cd;
                                border-left: 4px solid #ffc107;
                                padding: 15px;
                                margin: 20px 0;
                                border-radius: 4px;
                            }}
                            .footer {{ 
                                text-align: center; 
                                padding: 20px; 
                                background-color: #f8f9fa;
                                font-size: 12px; 
                                color: #666; 
                            }}
                            .token-box {{
                                background-color: #f8f9fa;
                                border: 1px dashed #dee2e6;
                                padding: 15px;
                                border-radius: 5px;
                                word-break: break-all;
                                font-family: 'Courier New', monospace;
                                font-size: 14px;
                                margin: 15px 0;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>🐾 PetCare</h1>
                                <p style='margin: 10px 0 0 0; font-size: 16px;'>Sistema de Gestión Veterinaria</p>
                            </div>
                            <div class='content'>
                                <h2>Restablecer Contraseña</h2>
                                <p>Hola,</p>
                                <p>Hemos recibido una solicitud para restablecer la contraseña de tu cuenta en PetCare.</p>
                                <p>Para crear una nueva contraseña, haz clic en el siguiente botón:</p>
                                <center>
                                    <a href='{resetUrl}' class='button'>🔓 Restablecer Contraseña</a>
                                </center>
                                <div class='warning'>
                                    <strong>⏰ Este enlace expirará en 1 hora.</strong>
                                </div>
                                <p>Si no solicitaste este cambio, puedes ignorar este correo de forma segura. Tu contraseña no será modificada.</p>
                                <hr style='border: none; border-top: 1px solid #dee2e6; margin: 30px 0;'>
                                <p style='font-size: 12px; color: #666;'>
                                    Si el botón no funciona, copia y pega este enlace en tu navegador:
                                </p>
                                <div class='token-box'>
                                    {resetUrl}
                                </div>
                            </div>
                            <div class='footer'>
                                <p><strong>© 2025 PetCare</strong></p>
                                <p>Sistema de Gestión Veterinaria</p>
                                <p style='margin-top: 10px;'>
                                    Este es un correo automático, por favor no respondas a este mensaje.
                                </p>
                            </div>
                        </div>
                    </body>
                    </html>
                ";

                return await EnviarEmailAsync(destinatario, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al enviar email de reset: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EnviarEmailConfirmacionCitaAsync(string destinatario, string nombreCliente, DateTime fechaCita)
        {
            try
            {
                var subject = "✅ Confirmación de Cita - PetCare";

                var body = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                        <style>
                            body {{ 
                                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
                                line-height: 1.6; 
                                color: #333; 
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{ 
                                max-width: 600px; 
                                margin: 20px auto; 
                                background-color: white;
                                border-radius: 10px;
                                overflow: hidden;
                                box-shadow: 0 0 20px rgba(0,0,0,0.1);
                            }}
                            .header {{ 
                                background: linear-gradient(135deg, #2196F3 0%, #21CBF3 100%);
                                color: white; 
                                padding: 30px 20px; 
                                text-align: center; 
                            }}
                            .header h1 {{ 
                                margin: 0; 
                                font-size: 28px; 
                            }}
                            .content {{ 
                                padding: 40px 30px; 
                            }}
                            .content h2 {{
                                color: #2196F3;
                                margin-top: 0;
                            }}
                            .cita-info {{ 
                                background: linear-gradient(135deg, #E3F2FD 0%, #BBDEFB 100%);
                                padding: 25px; 
                                border-left: 4px solid #2196F3; 
                                margin: 25px 0; 
                                border-radius: 8px;
                            }}
                            .cita-info p {{
                                margin: 10px 0;
                                font-size: 16px;
                            }}
                            .cita-info strong {{
                                color: #1976D2;
                            }}
                            .icon {{
                                font-size: 24px;
                                margin-right: 10px;
                            }}
                            .footer {{ 
                                text-align: center; 
                                padding: 20px; 
                                background-color: #f8f9fa;
                                font-size: 12px; 
                                color: #666; 
                            }}
                            .contact-info {{
                                background-color: #f8f9fa;
                                padding: 20px;
                                border-radius: 8px;
                                margin: 20px 0;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>🐾 PetCare</h1>
                                <p style='margin: 10px 0 0 0; font-size: 16px;'>Sistema de Gestión Veterinaria</p>
                            </div>
                            <div class='content'>
                                <h2>¡Cita Confirmada!</h2>
                                <p>Hola <strong>{nombreCliente}</strong>,</p>
                                <p>Tu cita ha sido registrada exitosamente en nuestro sistema. A continuación, los detalles:</p>
                                <div class='cita-info'>
                                    <p><span class='icon'>📅</span><strong>Fecha:</strong><br>
                                    {fechaCita:dddd, dd 'de' MMMM 'de' yyyy}</p>
                                    <p><span class='icon'>🕐</span><strong>Hora:</strong><br>
                                    {fechaCita:hh:mm tt}</p>
                                </div>
                                <div class='contact-info'>
                                    <p><strong>📍 Recordatorios importantes:</strong></p>
                                    <ul>
                                        <li>Por favor, llega <strong>10 minutos antes</strong> de tu cita</li>
                                        <li>Trae la cartilla de vacunación de tu mascota</li>
                                        <li>Si necesitas cancelar o reprogramar, contacta con nosotros con al menos 24 horas de anticipación</li>
                                    </ul>
                                </div>
                                <p>Si tienes alguna pregunta, no dudes en contactarnos.</p>
                                <p>¡Esperamos verte pronto! 🐶🐱</p>
                            </div>
                            <div class='footer'>
                                <p><strong>© 2025 PetCare</strong></p>
                                <p>Sistema de Gestión Veterinaria</p>
                                <p style='margin-top: 15px;'>
                                    📞 <strong>Teléfono:</strong> (809) 123-4567<br>
                                    📧 <strong>Email:</strong> info@petcare.com<br>
                                    📍 <strong>Dirección:</strong> Calle Principal #123, Santo Domingo
                                </p>
                            </div>
                        </div>
                    </body>
                    </html>
                ";

                return await EnviarEmailAsync(destinatario, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al enviar email de confirmación: {ex.Message}");
                return false;
            }
        }

        // Método privado para enviar emails
        private async Task<bool> EnviarEmailAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            try
            {
                Console.WriteLine($"📧 Intentando enviar email a: {destinatario}");
                Console.WriteLine($"📧 SMTP Host: {_config.Host}:{_config.Port}");
                Console.WriteLine($"📧 Username: {_config.Username}");
                Console.WriteLine($"📧 EnableSsl: {_config.EnableSsl}");

                using var smtpClient = new SmtpClient(_config.Host, _config.Port)
                {
                    EnableSsl = _config.EnableSsl,
                    Credentials = new NetworkCredential(_config.Username, _config.Password),
                    Timeout = 30000 // 30 segundos
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config.FromEmail, _config.FromName),
                    Subject = asunto,
                    Body = cuerpoHtml,
                    IsBodyHtml = true,
                    Priority = MailPriority.High
                };

                mailMessage.To.Add(destinatario);

                await smtpClient.SendMailAsync(mailMessage);

                Console.WriteLine($"✅ Email enviado exitosamente a: {destinatario}");
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"❌ Error SMTP al enviar email:");
                Console.WriteLine($"   - Mensaje: {ex.Message}");
                Console.WriteLine($"   - StatusCode: {ex.StatusCode}");
                Console.WriteLine($"   - InnerException: {ex.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error general al enviar email:");
                Console.WriteLine($"   - Mensaje: {ex.Message}");
                Console.WriteLine($"   - StackTrace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
