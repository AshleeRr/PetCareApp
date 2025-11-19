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
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            #region Services IOC
            services.AddTransient(typeof(IGenericService<>), typeof(GenericService<,>));
            services.AddTransient<IAutenticacionService, AutenticacionService>();
            services.AddTransient<IClienteService, ClienteService>();
            services.AddTransient<ICitaService, CitaService>();
            services.AddTransient<IEstadoService, EstadoService>();
            services.AddTransient<IMotivoCitaService, MotivoCitaService>();
            services.AddTransient<IMascotaService, MascotaService>();
            services.AddTransient<IEstadoService, EstadoService>();
            services.AddTransient<IMascotaPruebaMedicaService, MascotaPruebaMedicaService>();
            services.AddTransient<IPruebaMedicaService, PruebaMedicaService>();
            services.AddTransient<IMedicamentoService, MedicamentoService>();
            services.AddTransient<IHistorialService, HistorialService>();
            services.AddTransient<IRecetaService, RecetaService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ConfiguracionServices2>();
            services.AddTransient<TokenService>();
            #endregion
        }
    }
}
