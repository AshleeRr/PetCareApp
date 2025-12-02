using Microsoft.Extensions.DependencyInjection;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;
using System.Reflection;

namespace PetCareApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationlayerIoc(this IServiceCollection services)
        {
            #region AutoMapper
            // Registra automáticamente todos los perfiles de mapeo en esta capa
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // Si tienes perfiles en la capa Web, mantén el AppDomain en Program.cs o agrégalos aquí
            #endregion

            #region Services (Lógica de Negocio)
            // Autenticación
            services.AddScoped<IAutenticacionService, AutenticacionService>();
            services.AddScoped<IPasswordResetService, PasswordResetService>();

            // Administración
            services.AddScoped<IAdminService, AdminService>();

            // Flujos Principales
            services.AddScoped<ICitaService, CitaService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IMascotaService, MascotaService>();

            // Veterinario
            services.AddScoped<IMedicamentoService, MedicamentoService>();
            services.AddScoped<IMascotaPruebaMedicaService, MascotaPruebaMedicaService>();
            services.AddScoped<IPruebaMedicaService, PruebaMedicaService>();
            services.AddScoped<IHistorialService, HistorialService>();

            // Catálogos y Auxiliares
            services.AddScoped<IEstadoService, EstadoService>();
            services.AddScoped<IMotivoCitaService, MotivoCitaService>();

            // Ventas
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<ICarritoService, CarritoService>();
            services.AddScoped<IVentaService, VentaService>();
            #endregion
        }
    }
}