using Infraestructura.Persistencia.Repositorios;
using Infraestructura.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infraestructure.Persistence.Repositories;
using System.Text;


using PetCareApp.Core.Application;
using PetCareApp.Infraestructure.Persistence;
namespace VetCareApp.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddPersistencelayerIoc(builder.Configuration);
            builder.Services.AddApplicationlayerIoc();
            // -----------------------------
            // CONFIGURACIÓN DE SERVICIOS
            // -----------------------------




            /*
            // 1. DbContext
            builder.Services.AddDbContext<PetCareContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection"))
            );
            
            // 2. ✅ TODOS LOS REPOSITORIOS
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IUsuarioAdminRepository, UsuarioAdminRepository>(); // ✅ Nuevo
            builder.Services.AddScoped<IPersonalRepository, PersonalRepository>(); // ✅ Nuevo
            builder.Services.AddScoped<ISistemaLogRepository, SistemaLogRepository>(); // ✅ Nuevo
            builder.Services.AddScoped<IRoleRepositorio, RoleRepositorio>();
            builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>(); 
            builder.Services.AddScoped<ICitaRepository, CitaRepository>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
            builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
            builder.Services.AddScoped<IMedicamentoRepository, MedicamentosRepository>();
            builder.Services.AddScoped<IMotivoCitaRepository, MotivoCitaRepository>();
            builder.Services.AddScoped<IPruebasMedicasRepository, PruebasMedicasRepository>();
            builder.Services.AddScoped<IRecetaRepository, RecetaRepository>();
            builder.Services.AddScoped<ITratamientoRepository, TratamientoRepository>();
            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
            builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
            builder.Services.AddScoped<IVentaRepository, VentaRepository>();

            // 3. ✅ TODOS LOS SERVICIOS DE APLICACIÓN
            builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<Ilogger, Logger>();
            builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IProductoService, ProductoService>(); // ✅ Nuevo
            builder.Services.AddScoped<ICarritoService, CarritoService>(); // ✅ Nuevo
            builder.Services.AddScoped<IVentaService, VentaService>();
            builder.Services.AddScoped<IAdminService, AdminService>(); // ✅ Nuevo
            builder.Services.AddScoped<ICitaService, CitaService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IEstadoService, EstadoService>();
            builder.Services.AddScoped<IMascotaService, MascotaService>();
            builder.Services.AddScoped<IMotivoCitaService, MotivoCitaService>();
            */




            // 4. Configuración SMTP (si la usas)
            builder.Services.Configure<ConfiguracionServices>(
            builder.Configuration.GetSection("JwtSettings"));

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

            builder.Services.Configure<Infraestructura.Servicios.ConfiguracionServices2>(
                builder.Configuration.GetSection("SmtpSettings")
            );

            // 8. Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PetCare API",
                    Version = "v1",
                    Description = "API para el sistema de gestión veterinaria PetCare"
                });

                // ✅ CONFIGURACIÓN DE SEGURIDAD JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese 'Bearer' seguido de un espacio y el token JWT.\n\nEjemplo: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
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