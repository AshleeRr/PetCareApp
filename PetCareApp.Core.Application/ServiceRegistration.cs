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
            #region Configurations
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            #region Services IOC
            services.AddTransient<IAutenticacionService, AutenticacionService>();
            services.AddTransient<IClienteService, ClienteService>();
            services.AddTransient<ICitaService, CitaService>();
            services.AddTransient<IEstadoService, EstadoService>();
            services.AddTransient<IMotivoCitaService, MotivoCitaService>();
            services.AddTransient<IMascotaService, MascotaService>();
            services.AddTransient<IEstadoService, EstadoService>();
            services.AddTransient<TokenService>();
            #endregion
        }
    }
}
