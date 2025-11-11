using Infraestructura.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infraestructure.Persistence.Repositories;

namespace PetCareApp.Infraestructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistencelayerIoc(this IServiceCollection services, IConfiguration configuration)
        {
            #region context
            var connectionString = configuration.GetConnectionString("AshBdConnection");
            services.AddDbContext<PetCareContext>(opt => opt.UseSqlServer(connectionString, 
                m => m.MigrationsAssembly(typeof(PetCareContext).Assembly.FullName)),
                ServiceLifetime.Transient);
            #endregion
            
            #region Repositories IOC
            services.AddTransient(typeof(IGenericRepositorio<>), typeof(GeneRepositorio<>));
            services.AddTransient<IMedicamentoRepository, MedicamentosRepository>();
            services.AddTransient<ICitaRepository, CitaRepository>();
            services.AddTransient<IPruebasMedicasRepository, PruebasMedicasRepository>();
            services.AddTransient<ITratamientoRepository, TratamientoRepository>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IEstadoRepository, EstadoRepository>();
            services.AddScoped<IRoleRepositorio, RoleRepositorio>();
            services.AddScoped<IMotivoCitaRepository, MotivoCitaRepository>();
            services.AddScoped<IMascotaRepository, MascotaRepository>();
            services.AddScoped<IRecetaRepository, RecetaRepository>();
            services.AddScoped<IRazaMascotaRepository, RazaMascotRepository>();

            services.AddScoped<Ilogger, Logger>();
            #endregion
        }
    }
}
