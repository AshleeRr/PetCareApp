using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Repositories;
using Infraestructura.Persistencia.Repositorios;
using PetCareApp.Core.Application.Services;
using PetCareApp.Core.Application.Interfaces;
using Infraestructura.Servicios;
using System.Text;

namespace VetCareApp.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -----------------------------
            // CONFIGURACIÓN DE SERVICIOS
            // -----------------------------

            // 1. DbContext
            builder.Services.AddDbContext<PetCareContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection"))
            );

            // 2. ✅ TODOS LOS REPOSITORIOS
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IRoleRepositorio, RoleRepositorio>();
            builder.Services.AddScoped<ICitaRepository, CitaRepository>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
            builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
            builder.Services.AddScoped<IMedicamentoRepository, MedicamentosRepository>();
            builder.Services.AddScoped<IMotivoCitaRepository, MotivoCitaRepository>();
            builder.Services.AddScoped<IPruebasMedicasRepository, PruebasMedicasRepository>();
            builder.Services.AddScoped<IRecetaRepository, RecetaRepository>();
            builder.Services.AddScoped<ITratamientoRepository, TratamientoRepository>();

            // 3. ✅ TODOS LOS SERVICIOS DE APLICACIÓN
            builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<Ilogger, Logger>();
            builder.Services.AddScoped<ICitaService, CitaService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IEstadoService, EstadoService>();
            builder.Services.AddScoped<IMascotaService, MascotaService>();
            builder.Services.AddScoped<IMotivoCitaService, MotivoCitaService>();

            // 4. Configuración SMTP (si la usas)
            builder.Services.Configure<ConfiguracionServices2>(
                builder.Configuration.GetSection("SmtpSettings")
            );

            // 5. Controladores y vistas
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });
            builder.Services.AddRazorPages();

            // 6. Configuración JWT
            var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
            var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
            var jwtAudience = builder.Configuration["JwtSettings:Audience"];

            if (string.IsNullOrEmpty(jwtKey))
                throw new ArgumentNullException("JwtSettings:SecretKey", "La clave JWT no está configurada");

            if (string.IsNullOrEmpty(jwtIssuer))
                throw new ArgumentNullException("JwtSettings:Issuer", "El Issuer JWT no está configurado");

            if (string.IsNullOrEmpty(jwtAudience))
                throw new ArgumentNullException("JwtSettings:Audience", "El Audience JWT no está configurado");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            builder.Services.AddAuthorization();

            // 7. CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            // 8. Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "PetCare API",
                    Version = "v1",
                    Description = "API para el sistema de gestión veterinaria PetCare"
                });
            });

            var app = builder.Build();

            // -----------------------------
            // CONFIGURACIÓN DEL PIPELINE
            // -----------------------------

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetCare API V1");
                    c.RoutePrefix = string.Empty; // Swagger en la raíz
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}