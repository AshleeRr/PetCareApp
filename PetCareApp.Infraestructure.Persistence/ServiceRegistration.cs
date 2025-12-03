using Infraestructura.Persistencia.Repositorios;
using Infraestructura.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCareApp.Application.Interfaces;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infraestructure.Persistence.Repositories;
using PetCareApp.Infrastructure.Repositorios;

namespace PetCareApp.Infraestructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistencelayerIoc(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context
            var connectionString = configuration.GetConnectionString("AppConnection");
            services.AddDbContext<PetCareContext>(opt => opt.UseSqlServer(connectionString,
                m => m.MigrationsAssembly(typeof(PetCareContext).Assembly.FullName)),
                ServiceLifetime.Scoped); // Usualmente Scoped es mejor para Web APIs
            #endregion

            #region Repositories
            // Genérico
            services.AddTransient(typeof(IGenericRepositorio<>), typeof(GeneRepositorio<>));

            // Usuarios y Auth
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IUsuarioAdminRepository, UsuarioAdminRepository>();
            services.AddScoped<IRoleRepositorio, RoleRepositorio>();
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

            // Admin y Logs
            services.AddScoped<IPersonalRepository, PersonalRepository>();
            services.AddScoped<ISistemaLogRepository, SistemaLogRepository>();

            // Negocio Principal (Citas, Mascotas, Clientes)
            services.AddScoped<ICitaRepository, CitaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IMascotaRepository, MascotaRepository>();
            services.AddScoped<IEstadoRepository, EstadoRepository>();
            services.AddScoped<IMotivoCitaRepository, MotivoCitaRepository>();
            services.AddScoped<IRazaMascotaRepository, RazaMascotRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITipoMascotaRepository, TipoMascotaRepository>();
            services.AddScoped<ITipoProductoRepository, TipoProductoRepository>();

            // Médicos y Tratamientos
            services.AddScoped<IMedicamentoRepository, MedicamentosRepository>();
            services.AddScoped<IRecetaRepository, RecetaRepository>();
            services.AddScoped<ITratamientoRepository, TratamientoRepository>();
            services.AddScoped<IPruebasMedicasRepository, PruebasMedicasRepository>();

            // Ventas e Inventario
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<ICarritoRepository, CarritoRepository>();
            services.AddScoped<IVentaRepository, VentaRepository>();
            #endregion

            #region Servicios de Infraestructura (Externos)
            // Estos son implementaciones técnicas, van aquí
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<Ilogger, Logger>();
            services.AddScoped<TokenService>();
            #endregion
        }
    }
}