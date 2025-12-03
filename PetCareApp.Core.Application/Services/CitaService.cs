using AutoMapper;
using Infraestructura.Servicios;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Core.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _repo;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService; // ⭐ NUEVO
        private readonly Ilogger _logger; // ⭐ NUEVO

        public CitaService(
            ICitaRepository repo,
            IMapper mapper,
            IEmailService emailService, // ⭐ INYECTAR EmailService
            Ilogger logger) // ⭐ INYECTAR Logger
        {
            _repo = repo;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        private static CitaDto MapToDto(Cita c) => new()
        {
            Id = c.Id,
            FechaHora = c.FechaHora,
            Estado = c.Estado?.Nombre ?? "",
            Cliente = $"{c.Dueño?.Nombre} {c.Dueño?.Apellido}",
            Veterinario = $"{c.Veterinario?.Nombre} {c.Veterinario?.Apellido}",
            Motivo = c.Motivo?.Motivo ?? "",
            Mascota = c.Mascota?.Nombre ?? "",
            Observaciones = c.Observaciones ?? ""
        };

        public async Task<List<CitaDto>> ObtenerCitasAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(MapToDto).ToList();
        }

        public async Task<CitaDto?> ObtenerPorIdAsync(int id)
        {
            var cita = await _repo.GetByIdAsync(id);
            return cita == null ? null : MapToDto(cita);
        }

        public async Task<List<CitaDto>> ObtenerPorFechaAsync(DateTime fecha)
        {
            var list = await _repo.GetByFechaAsync(fecha);
            return list.Select(MapToDto).ToList();
        }

        public async Task<List<CitaDto>> ObtenerPorClienteAsync(int clienteId)
        {
            var list = await _repo.GetByClienteAsync(clienteId);
            return list.Select(MapToDto).ToList();
        }

        // ========================================
        // CREAR CITA CON EMAIL
        // ========================================
        public async Task<CitaDto> CrearCitaAsync(CrearCitaDto dto)
        {
            var cita = new Cita
            {
                FechaHora = dto.FechaHora,
                EstadoId = dto.EstadoId,
                DueñoId = dto.DueñoId,
                MascotaId = dto.MascotaId,
                VeterinarioId = dto.VeterinarioId,
                MotivoId = dto.MotivoId,
                Observaciones = dto.Observaciones
            };

            var created = await _repo.AddAsync(cita);

            // ⭐ OBTENER LA CITA CON TODAS LAS RELACIONES CARGADAS
            var citaCompleta = await _repo.GetByIdAsync(created.Id);

            if (citaCompleta != null)
            {
                // ⭐ ENVIAR EMAIL DE CONFIRMACIÓN DE CITA
                try
                {
                    var emailCliente = citaCompleta.Dueño?.Email;
                    var nombreCliente = $"{citaCompleta.Dueño?.Nombre} {citaCompleta.Dueño?.Apellido}";
                    var nombreMascota = citaCompleta.Mascota?.Nombre ?? "Tu mascota";
                    var tipoMascota = citaCompleta.Mascota?.TipoMascota?.Tipo ?? "";
                    var nombreVeterinario = $"{citaCompleta.Veterinario?.Nombre} {citaCompleta.Veterinario?.Apellido}";
                    var motivo = citaCompleta.Motivo?.Motivo ?? "Consulta general";

                    if (!string.IsNullOrEmpty(emailCliente))
                    {
                        // Ejecutar el envío de email de forma asíncrona sin esperar (fire and forget)
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                var emailEnviado = await _emailService.EnviarEmailConfirmacionCitaAsync(
                                    emailCliente,
                                    nombreCliente,
                                    citaCompleta.FechaHora,
                                    nombreMascota,
                                    tipoMascota,
                                    nombreVeterinario,
                                    motivo
                                );

                                if (emailEnviado)
                                {
                                    _logger.Info($"✅ Email de confirmación de cita enviado a {emailCliente}");
                                }
                                else
                                {
                                    _logger.Warning($"⚠️ No se pudo enviar email de confirmación a {emailCliente}");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Error($"❌ Error al enviar email de confirmación de cita: {ex.Message}");
                            }
                        });
                    }
                    else
                    {
                        _logger.Warning("⚠️ El cliente no tiene email registrado, no se envió confirmación");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"❌ Error al iniciar tarea de email de cita: {ex.Message}");
                    // No lanzamos excepción para no interrumpir la creación de la cita
                }
            }

            return MapToDto(created);
        }

        public async Task<bool> EditarCitaAsync(int id, ActualizarCitaDto dto)
        {
            var cita = await _repo.GetByIdAsync(id);
            if (cita == null) return false;

            cita.FechaHora = dto.FechaHora;
            cita.EstadoId = dto.EstadoId;
            cita.MascotaId = dto.MascotaId;
            cita.VeterinarioId = dto.VeterinarioId;
            cita.MotivoId = dto.MotivoId;
            cita.Observaciones = dto.Observaciones;

            await _repo.UpdateAsync(id, cita);
            return true;
        }

        public async Task<bool> EliminarCitaAsync(int id)
        {
            var cita = await _repo.GetByIdAsync(id);
            if (cita == null) return false;

            await _repo.RemoveAsync(id);
            return true;
        }

        public async Task<List<CitaDto>> GetCitasAsiganasAVeterinarioAsync(int userId)
        {
            var citas = await _repo.GetCitasAsiganasAVeterinarioAsync(userId);
            return _mapper.Map<List<CitaDto>>(citas);
        }

        public async Task<List<CitaDto>> GetCitasOfMascotaById(int mascota)
        {
            var citas = await _repo.GetCitasOfMascotaById(mascota);
            return _mapper.Map<List<CitaDto>>(citas);
        }
    }
}