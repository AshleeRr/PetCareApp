using Infraestructura.Servicios;
using Microsoft.Extensions.DependencyInjection;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;

namespace PetCareApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationlayerIoc(this IServiceCollection services)
        {
            #region Configurations
           // services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion

            #region Services IOC
            services.AddScoped<IAutenticacionService, AutenticacionService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<ICitaService, CitaService>();
            services.AddScoped<IEstadoService, EstadoService>();
            services.AddScoped<IMotivoCitaService, MotivoCitaService>();
            services.AddScoped<IMascotaService, MascotaService>();
            services.AddScoped<IEstadoService, EstadoService>();
            services.AddScoped<TokenService>();
            #endregion
        }
    }
}
