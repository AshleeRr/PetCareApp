using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infraestructure.Persistence.Repositories;

namespace PetCareApp.Infraestructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddersistencelayerIoc(this IServiceCollection services, IConfiguration configuration)
        {
            #region context
            var connectionString = configuration.GetConnectionString("BdConnection");
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
            #endregion
        }
    }
}
