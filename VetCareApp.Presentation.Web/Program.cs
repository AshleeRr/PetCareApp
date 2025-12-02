using Infraestructura.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PetCareApp.Application.Interfaces;
using PetCareApp.Application.Services;
using PetCareApp.Core.Application;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence;
using PetCareApp.Infraestructure.Persistence.Repositories;
using System.Text;

namespace VetCareApp.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ========================
            // IOC
            // ========================
            builder.Services.AddPersistencelayerIoc(builder.Configuration);
            builder.Services.AddApplicationlayerIoc();

            // ========================
            // SMTP
            // ========================
            builder.Services.Configure<ConfiguracionServices2>(
                builder.Configuration.GetSection("SmtpSettings")
            );

            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            builder.Services.AddRazorPages();

            // ========================
            // SERVICIOS EXTRAS
            // ========================
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();

            // ========================
            // PRODUCTOS
            // ========================
            builder.Services.AddScoped<IProductoService, ProductoService>();
            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
            
            builder.Services.AddScoped<IMascotaService, MascotaService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IUsuarioAdminRepository, UsuarioAdminRepository>();
            builder.Services.AddScoped<IPersonalRepository, PersonalRepository>();
            builder.Services.AddScoped<ISistemaLogRepository, SistemaLogRepository>();
            builder.Services.AddScoped<IVentaRepository, VentaRepository>();
            //



            builder.Services.AddScoped<ITipoProductoService, TipoProductoService>();
            builder.Services.AddScoped<ITipoProductoRepository, TipoProductoRepository>();

            // ========================
            // MASCOTAS
            // ========================
            builder.Services.AddScoped<IMascotaService, MascotaService>();
            builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();

            builder.Services.AddScoped<ITipoMascotaService, TipoMascotaService>();
            builder.Services.AddScoped<ITipoMascotaRepository, TipoMascotaRepository>();

            // ========================
            // JWT SETTINGS
            // ========================
            var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
            var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
            var jwtAudience = builder.Configuration["JwtSettings:Audience"];

            if (string.IsNullOrEmpty(jwtKey) ||
                string.IsNullOrEmpty(jwtIssuer) ||
                string.IsNullOrEmpty(jwtAudience))
                throw new Exception("Faltan configuraciones de JWT.");

            builder.Services.Configure<ConfiguracionServices>(
                builder.Configuration.GetSection("JwtSettings")
            );

            // ========================
            // AUTHENTICATION ✅ (SIN DUPLICADOS)
            // ========================
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
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
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    )
                };
            })
            .AddGoogle("Google", googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            builder.Services.AddAuthorization();

            // ========================
            // CORS
            // ========================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            // ========================
            // SWAGGER
            // ========================
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetCare API V1");
                });
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
