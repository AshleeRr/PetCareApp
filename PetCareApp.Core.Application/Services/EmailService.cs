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

        // ========================================
        // 1. EMAIL DE BIENVENIDA (REGISTRO)
        // ========================================
        public async Task<bool> EnviarEmailBienvenidaAsync(string destinatario, string nombreUsuario, string email)
        {
            try
            {
                var subject = "🎉 ¡Bienvenido a PetCare! - Tu cuenta ha sido creada";

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
                                padding: 40px 20px; 
                                text-align: center; 
                            }}
                            .header h1 {{ 
                                margin: 0; 
                                font-size: 32px; 
                            }}
                            .welcome-icon {{
                                font-size: 60px;
                                margin: 20px 0;
                            }}
                            .content {{ 
                                padding: 40px 30px; 
                            }}
                            .content h2 {{
                                color: #667eea;
                                margin-top: 0;
                            }}
                            .info-box {{ 
                                background: linear-gradient(135deg, #E8EAF6 0%, #C5CAE9 100%);
                                padding: 20px; 
                                border-left: 4px solid #667eea; 
                                margin: 25px 0; 
                                border-radius: 8px;
                            }}
                            .info-box p {{
                                margin: 8px 0;
                                font-size: 15px;
                            }}
                            .info-box strong {{
                                color: #5E35B1;
                            }}
                            .features {{
                                background-color: #f8f9fa;
                                padding: 25px;
                                border-radius: 8px;
                                margin: 25px 0;
                            }}
                            .features h3 {{
                                color: #667eea;
                                margin-top: 0;
                            }}
                            .features ul {{
                                list-style: none;
                                padding: 0;
                            }}
                            .features li {{
                                padding: 10px 0;
                                border-bottom: 1px solid #dee2e6;
                            }}
                            .features li:last-child {{
                                border-bottom: none;
                            }}
                            .features li::before {{
                                content: '✓ ';
                                color: #4CAF50;
                                font-weight: bold;
                                font-size: 18px;
                                margin-right: 10px;
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
                            }}
                            .footer {{ 
                                text-align: center; 
                                padding: 20px; 
                                background-color: #f8f9fa;
                                font-size: 12px; 
                                color: #666; 
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <div class='welcome-icon'></div>
                                <h1>¡Bienvenido a PetCare!</h1>
                                <p style='margin: 10px 0 0 0; font-size: 16px;'>Tu cuenta ha sido creada exitosamente</p>
                            </div>
                            <div class='content'>
                                <h2>¡Hola {nombreUsuario}! </h2>
                                <p>Nos complace darte la bienvenida a <strong>PetCare</strong>, el sistema de gestión veterinaria que cuidará de tus mejores amigos.</p>
                                
                                <div class='info-box'>
                                    <p><strong> Email:</strong> {email}</p>
                                    <p><strong>Usuario:</strong> {nombreUsuario}</p>
                                    <p><strong>Fecha de registro:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                                </div>

                                <center>
                                    <a href='https://localhost:5001' class='button'> Ir a PetCare</a>
                                </center>

                                <p style='margin-top: 30px;'>Si tienes alguna pregunta o necesitas ayuda, no dudes en contactarnos. ¡Estamos aquí para ayudarte!</p>
                                
                                <p style='margin-top: 20px;'>Gracias por confiar en PetCare para el cuidado de tus mascotas. </p>
                            </div>
                            <div class='footer'>
                                <p><strong>© 2025 PetCare</strong></p>
                                <p>Sistema de Gestión Veterinaria</p>
                                <p style='margin-top: 15px;'>
                                     <strong>Teléfono:</strong> (809) 123-4567<br>
                                     <strong>Email:</strong> info@petcare.com<br>
                                     <strong>Dirección:</strong> Calle Principal #123, Santo Domingo
                                </p>
                                <p style='margin-top: 10px; font-size: 11px; color: #999;'>
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
                Console.WriteLine($"❌ Error al enviar email de bienvenida: {ex.Message}");
                return false;
            }
        }

        // ========================================
        // 2. EMAIL DE INICIO DE SESIÓN
        // ========================================
        public async Task<bool> EnviarEmailLoginAsync(string destinatario, string nombreUsuario, string ipAddress = "Desconocida")
        {
            try
            {
                var subject = "🔐 Inicio de sesión detectado en PetCare";

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
                                background: linear-gradient(135deg, #4CAF50 0%, #45a049 100%);
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
                                color: #4CAF50;
                                margin-top: 0;
                            }}
                            .login-info {{ 
                                background: linear-gradient(135deg, #E8F5E9 0%, #C8E6C9 100%);
                                padding: 20px; 
                                border-left: 4px solid #4CAF50; 
                                margin: 25px 0; 
                                border-radius: 8px;
                            }}
                            .login-info p {{
                                margin: 10px 0;
                                font-size: 15px;
                            }}
                            .login-info strong {{
                                color: #2E7D32;
                            }}
                            .warning {{
                                background-color: #fff3cd;
                                border-left: 4px solid #ffc107;
                                padding: 15px;
                                margin: 20px 0;
                                border-radius: 4px;
                            }}
                            .warning strong {{
                                color: #856404;
                            }}
                            .footer {{ 
                                text-align: center; 
                                padding: 20px; 
                                background-color: #f8f9fa;
                                font-size: 12px; 
                                color: #666; 
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>🐾 PetCare</h1>
                                <p style='margin: 10px 0 0 0; font-size: 16px;'>Notificación de Seguridad</p>
                            </div>
                            <div class='content'>
                                <h2>Inicio de sesión detectado</h2>
                                <p>Hola <strong>{nombreUsuario}</strong>,</p>
                                <p>Se ha detectado un nuevo inicio de sesión en tu cuenta de PetCare.</p>
                                
                                <div class='login-info'>
                                    <p><strong> Fecha y hora:</strong> {DateTime.Now:dddd, dd 'de' MMMM 'de' yyyy}</p>
                                    <p><strong> Hora:</strong> {DateTime.Now:hh:mm:ss tt}</p>
                                    <p><strong> Dirección IP:</strong> {ipAddress}</p>
                                    <p><strong> Dispositivo:</strong> Navegador web</p>
                                </div>

                                <div class='warning'>
                                    <strong> ¿No fuiste tú?</strong><br>
                                    Si no reconoces esta actividad, te recomendamos cambiar tu contraseña inmediatamente y contactar con nuestro equipo de soporte.
                                </div>

                                <p>Este es un email de notificación de seguridad para mantener tu cuenta protegida.</p>
                                <p>Si fuiste tú quien inició sesión, puedes ignorar este mensaje. 😊</p>
                            </div>
                            <div class='footer'>
                                <p><strong>© 2025 PetCare</strong></p>
                                <p>Sistema de Gestión Veterinaria</p>
                                <p style='margin-top: 15px;'>
                                     <strong>Teléfono:</strong> (809) 123-4567<br>
                                     <strong>Email:</strong> info@petcare.com
                                </p>
                                <p style='margin-top: 10px; font-size: 11px; color: #999;'>
                                    Este es un correo automático de seguridad, por favor no respondas a este mensaje.
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
                Console.WriteLine($" Error al enviar email de login: {ex.Message}");
                return false;
            }
        }

        // ========================================
        // 3. EMAIL DE CONFIRMACIÓN DE CITA (MEJORADO)
        // ========================================
        public async Task<bool> EnviarEmailConfirmacionCitaAsync(
            string destinatario,
            string nombreCliente,
            DateTime fechaCita,
            string nombreMascota = "Tu mascota",
            string tipoMascota = "",
            string nombreVeterinario = "Nuestro equipo",
            string motivo = "Consulta general")
        {
            try
            {
                var subject = " Confirmación de Cita - PetCare";

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
                            .success-icon {{
                                font-size: 50px;
                                margin: 15px 0;
                            }}
                            .content {{ 
                                padding: 40px 30px; 
                            }}
                            .content h2 {{
                                color: #2196F3;
                                margin-top: 0;
                            }}
                            .cita-card {{ 
                                background: linear-gradient(135deg, #E3F2FD 0%, #BBDEFB 100%);
                                padding: 25px; 
                                border-left: 4px solid #2196F3; 
                                margin: 25px 0; 
                                border-radius: 8px;
                                box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                            }}
                            .cita-card h3 {{
                                color: #1976D2;
                                margin-top: 0;
                                margin-bottom: 15px;
                                font-size: 18px;
                            }}
                            .cita-detail {{
                                background-color: white;
                                padding: 12px;
                                margin: 10px 0;
                                border-radius: 5px;
                                display: flex;
                                align-items: center;
                            }}
                            .icon {{
                                font-size: 20px;
                                margin-right: 12px;
                                min-width: 25px;
                            }}
                            .cita-detail strong {{
                                color: #1565C0;
                                display: block;
                                font-size: 13px;
                            }}
                            .reminder-box {{
                                background-color: #FFF9C4;
                                border-left: 4px solid #FBC02D;
                                padding: 20px;
                                border-radius: 8px;
                                margin: 25px 0;
                            }}
                            .reminder-box h3 {{
                                color: #F57F17;
                                margin-top: 0;
                            }}
                            .reminder-box ul {{
                                margin: 10px 0;
                                padding-left: 20px;
                            }}
                            .reminder-box li {{
                                margin: 8px 0;
                            }}
                            .footer {{ 
                                text-align: center; 
                                padding: 20px; 
                                background-color: #f8f9fa;
                                font-size: 12px; 
                                color: #666; 
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <div class='success-icon'></div>
                                <h1> PetCare</h1>
                                <p style='margin: 10px 0 0 0; font-size: 16px;'>Cita Confirmada Exitosamente</p>
                            </div>
                            <div class='content'>
                                <h2>¡Tu cita ha sido agendada!</h2>
                                <p>Hola <strong>{nombreCliente}</strong>,</p>
                                <p>Tu cita en PetCare ha sido registrada exitosamente. A continuación, encontrarás todos los detalles:</p>
                                
                                <div class='cita-card'>
                                    <h3> Detalles de la Cita</h3>
                                    
                                    <div class='cita-detail'>
                                        <span class='icon'></span>
                                        <div>
                                            <strong>Mascota</strong>
                                            {nombreMascota}{(string.IsNullOrEmpty(tipoMascota) ? "" : $" ({tipoMascota})")}
                                        </div>
                                    </div>

                                    <div class='cita-detail'>
                                        <span class='icon'></span>
                                        <div>
                                            <strong>Fecha</strong>
                                            {fechaCita:dddd, dd 'de' MMMM 'de' yyyy}
                                        </div>
                                    </div>

                                    <div class='cita-detail'>
                                        <span class='icon'></span>
                                        <div>
                                            <strong>Hora</strong>
                                            {fechaCita:hh:mm tt}
                                        </div>
                                    </div>

                                    <div class='cita-detail'>
                                        <span class='icon'></span>
                                        <div>
                                            <strong>Veterinario</strong>
                                            {nombreVeterinario}
                                        </div>
                                    </div>

                                    <div class='cita-detail'>
                                        <span class='icon'></span>
                                        <div>
                                            <strong>Motivo</strong>
                                            {motivo}
                                        </div>
                                    </div>
                                </div>

                                <div class='reminder-box'>
                                    <h3>📌 Recordatorios Importantes</h3>
                                    <ul>
                                        <li> <strong>Llega 10 minutos antes</strong> de tu cita para completar el registro</li>
                                        <li> Trae la <strong>cartilla de vacunación</strong> de tu mascota</li>
                                        <li> Si tu mascota toma medicamentos, trae la lista de medicamentos actuales</li>
                                        <li> Para análisis de sangre, tu mascota debe estar en <strong>ayunas de 8-12 horas</strong></li>
                                        <li> Para cancelar o reprogramar, contacta con <strong>24 horas de anticipación</strong></li>
                                    </ul>
                                </div>

                                <p style='margin-top: 25px;'>Si tienes alguna pregunta o necesitas realizar cambios, no dudes en contactarnos.</p>
                                <p><strong>¡Esperamos verte pronto junto a {nombreMascota}!</strong> </p>
                            </div>
                            <div class='footer'>
                                <p><strong>© 2025 PetCare</strong></p>
                                <p>Sistema de Gestión Veterinaria</p>
                                <p style='margin-top: 15px;'>
                                    <strong>Teléfono:</strong> (809) 123-4567<br>
                                    <strong>Email:</strong> info@petcare.com<br>
                                    <strong>Dirección:</strong> Calle Principal #123, Santo Domingo<br>
                                    <strong>Horario:</strong> Lunes a Sábado, 8:00 AM - 6:00 PM
                                </p>
                                <p style='margin-top: 10px; font-size: 11px; color: #999;'>
                                    Este es un correo automático, por favor no respondas a este mensaje.<br>
                                    Para consultas, comunícate a través de nuestros canales oficiales.
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
                Console.WriteLine($"❌ Error al enviar email de confirmación de cita: {ex.Message}");
                return false;
            }
        }

        // ========================================
        // EMAIL DE RESET PASSWORD (YA EXISTENTE)
        // ========================================
        public async Task<bool> EnviarEmailResetPasswordAsync(string destinatario, string token)
        {
            try
            {
                var resetUrl = $"http://localhost:3000/reset-password?token={token}";
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
                                    <a href='{resetUrl}' class='button'> Restablecer Contraseña</a>
                                </center>
                                <div class='warning'>
                                    <strong> Este enlace expirará en 1 hora.</strong>
                                </div>
                                <p>Si no solicitaste este cambio, puedes ignorar este correo de forma segura.</p>
                            </div>
                            <div class='footer'>
                                <p><strong>© 2025 PetCare</strong></p>
                                <p>Sistema de Gestión Veterinaria</p>
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

        // ========================================
        // MÉTODO PRIVADO PARA ENVIAR EMAILS
        // ========================================
        private async Task<bool> EnviarEmailAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            try
            {
                Console.WriteLine($"📧 Intentando enviar email a: {destinatario}");
                Console.WriteLine($"📧 Asunto: {asunto}");

                using var smtpClient = new SmtpClient(_config.Host, _config.Port)
                {
                    EnableSsl = _config.EnableSsl,
                    Credentials = new NetworkCredential(_config.Username, _config.Password),
                    Timeout = 30000
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
                Console.WriteLine($"❌ Error SMTP: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error general: {ex.Message}");
                return false;
            }
        }
    }
}