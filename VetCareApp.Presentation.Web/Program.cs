using AutoMapper;
using Infraestructura.Persistencia.Repositorios;
using Infraestructura.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infraestructure.Persistence.Repositories;
using System.Text;

namespace VetCareApp.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ====================================
            // 1. DBCONTEXT
            // ====================================
            builder.Services.AddDbContext<PetCareContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection"))
            );

            // ====================================
            // 1.5 AUTOMAPPER
            // ====================================
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // ====================================
            // 2. REPOSITORIOS
            // ====================================

            // Autenticación y Usuarios
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IUsuarioAdminRepository, UsuarioAdminRepository>();
            builder.Services.AddScoped<IRoleRepositorio, RoleRepositorio>();
            builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

            // Admin
            builder.Services.AddScoped<IPersonalRepository, PersonalRepository>();
            builder.Services.AddScoped<ISistemaLogRepository, SistemaLogRepository>();

            // Citas y Clientes
            builder.Services.AddScoped<ICitaRepository, CitaRepository>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
            builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
            builder.Services.AddScoped<IMotivoCitaRepository, MotivoCitaRepository>();

            // Médicos
            builder.Services.AddScoped<IMedicamentoRepository, MedicamentosRepository>();
            builder.Services.AddScoped<IRecetaRepository, RecetaRepository>();
            builder.Services.AddScoped<ITratamientoRepository, TratamientoRepository>();
            builder.Services.AddScoped<IPruebasMedicasRepository, PruebasMedicasRepository>();

            // Productos y Ventas
            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
            builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
            builder.Services.AddScoped<IVentaRepository, VentaRepository>();

            // ====================================
            // 3. SERVICIOS DE APLICACIÓN
            // ====================================

            // Autenticación y Seguridad
            builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // Logging
            builder.Services.AddScoped<Ilogger, Logger>();

            // Módulos de Negocio
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ICitaService, CitaService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IMascotaService, MascotaService>();
            builder.Services.AddScoped<IEstadoService, EstadoService>();
            builder.Services.AddScoped<IMotivoCitaService, MotivoCitaService>();
            builder.Services.AddScoped<IProductoService, ProductoService>();
            builder.Services.AddScoped<ICarritoService, CarritoService>();
            builder.Services.AddScoped<IVentaService, VentaService>();

            // ====================================
            // 4. CONFIGURACIÓN DE OPCIONES
            // ====================================
            builder.Services.Configure<ConfiguracionServices>(
                builder.Configuration.GetSection("JwtSettings")
            );

            builder.Services.Configure<ConfiguracionServices2>(
                builder.Configuration.GetSection("SmtpSettings")
            );

            // ====================================
            // 5. ROUTING Y CONTROLADORES
            // ====================================

            // ✅ Configurar routing options
            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = false;
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // ====================================
            // 6. AUTENTICACIÓN JWT
            // ====================================
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

            // ====================================
            // 7. CORS
            // ====================================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            // ====================================
            // 8. SWAGGER
            // ====================================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PetCare API",
                    Version = "v1",
                    Description = "API para el sistema de gestión veterinaria PetCare"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT sin el prefijo 'Bearer'.\n\nEjemplo: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
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

            // ====================================
            // BUILD APP
            // ====================================
            var app = builder.Build();

            // ====================================
            // PIPELINE DE MIDDLEWARE
            // ====================================

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // ✅ Agregar para ver errores detallados
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetCare API V1");
                    c.RoutePrefix = string.Empty;
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