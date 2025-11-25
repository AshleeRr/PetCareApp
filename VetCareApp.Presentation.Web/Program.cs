using Microsoft.IdentityModel.Tokens;
using Infraestructura.Servicios;
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

            builder.Services.Configure<Infraestructura.Servicios.ConfiguracionServices2>(
                builder.Configuration.GetSection("SmtpSettings")
            );

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

            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                });
            // -----------------------------
            // CONFIGURACIÓN DE SERVICIOS
            // -----------------------------

            // Controladores tipo API
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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