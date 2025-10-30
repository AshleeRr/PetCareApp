using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection; // Aseg�rate de tener este 'using' si usas m�todos de extensi�n
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Application.Services;
using PetCareApp.Infraestructure.Persistence;
using PetCareApp.Infraestructure.Persistence.Repositories;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Infraestructure.Persistence.Context;

namespace VetCareApp.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -----------------------------
            // CONFIGURACI�N DE SERVICIOS
            // -----------------------------

            // Controladores tipo API
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Inyecci�n de dependencias de la capa de infraestructura
            // NOTA: Se asume que AddersistencelayerIoc es un m�todo de extensi�n en otro proyecto.
            builder.Services.AddersistencelayerIoc(builder.Configuration);

            // Contexto de Base de Datos
            builder.Services.AddSqlServer<PetCareContext>(builder.Configuration.GetConnectionString("AppConnection"));

            // Repositorios y servicios (inyecci�n manual)
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<ClienteService>();

            builder.Services.AddScoped<ICitaRepository, CitaRepository>();
            builder.Services.AddScoped<ICitaService, CitaService>();

            builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
            builder.Services.AddScoped<IEstadoService, EstadoService>();

            builder.Services.AddScoped<IMotivoCitaRepository, MotivoCitaRepository>();
            builder.Services.AddScoped<IMotivoCitaService, MotivoCitaService>();

            builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
            builder.Services.AddScoped<IMascotaService, MascotaService>();

            var app = builder.Build();

            // -----------------------------
            // CONFIGURACI�N DEL PIPELINE
            // -----------------------------

            // HABILITAR SWAGGER
            if (app.Environment.IsDevelopment())
            {
                // 1. Usa UseSwagger() para generar el documento JSON
                app.UseSwagger();

                // 2. Usa UseSwaggerUI() para servir la interfaz web.
                // NOTA: La "I" en UI debe ser may�scula.
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Veterinaria v1");
                    options.RoutePrefix = string.Empty; // Muestra Swagger en la ra�z (http://localhost:5000/)
                });
            }

            app.UseHttpsRedirection();

            // Habilita CORS
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthorization();

            // Mapeo de Endpoints
            app.MapControllers();

            // Ruta de prueba
            app.MapGet("/", () => "Gestion de clientes funciona ?");

            // Ejecutar la aplicaci�n
            app.Run();
        }
    }

}

