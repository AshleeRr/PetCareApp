using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PetCareApp.Infraestructure.Persistence;
using PetCareApp.Core.Application;
using System.Text;

namespace VetCareApp.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Registrar servicios
            builder.Services.AddPersistencelayerIoc(builder.Configuration);
            builder.Services.AddApplicationlayerIoc();
            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages();

            var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
            var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
            var jwtAudience = builder.Configuration["JwtSettings:Audience"];

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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            // ✅ DbContext ya está registrado correctamente
           /* builder.Services.AddDbContext<PetCareContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection"))
            );*/

            // ✅ Registrar repositorios y servicios
            

            builder.Services.Configure<Infraestructura.Servicios.ConfiguracionServices2>(
                builder.Configuration.GetSection("SmtpSettings")
            );

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "PetCare API",
                    Version = "v1"
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

            // Inyección de dependencias de la capa de infraestructura
            // NOTA: Se asume que AddersistencelayerIoc es un método de extensión en otro proyecto.
            //builder.Services.AddersistencelayerIoc(builder.Configuration);

            // Contexto de Base de Datos
           // builder.Services.AddSqlServer<PetCareContext>(builder.Configuration.GetConnectionString("AppConnection"));

            // Repositorios y servicios (inyecci�n manual)
           // builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            /*

            builder.Services.AddScoped<ICitaRepository, CitaRepository>();
           

            builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
            builder.Services.AddScoped<IEstadoService, EstadoService>();

            builder.Services.AddScoped<IMotivoCitaRepository, MotivoCitaRepository>();
            builder.Services.AddScoped<IMotivoCitaService, MotivoCitaService>();

            builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
            builder.Services.AddScoped<IMascotaService, MascotaService>();
            */
            var app = builder.Build();

            // -----------------------------
            // CONFIGURACIÓN DEL PIPELINE
            // -----------------------------

            // HABILITAR SWAGGER
            if (app.Environment.IsDevelopment())
            // 2. Configurar el pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // 1. Usa UseSwagger() para generar el documento JSON
                app.UseSwagger();

                // 2. Usa UseSwaggerUI() para servir la interfaz web.
                // NOTA: La "I" en UI debe ser mayúscula.
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Veterinaria v1");
                    options.RoutePrefix = string.Empty; // Muestra Swagger en la raíz (http://localhost:5000/)
                });
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Habilita CORS
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetCare API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            // Mapeo de Endpoints
            app.MapControllers();

            // Ruta de prueba
            app.MapGet("/", () => "Gestion de clientes funciona ?");

            // Ejecutar la aplicaci�n
            app.Run();
        }
    }
}