using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Helpers;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUsuarioAdminRepository _usuarioRepo;
        private readonly IPersonalRepository _personalRepo;
        private readonly ISistemaLogRepository _logRepo;
        private readonly ICitaRepository _citaRepo;
        private readonly IVentaRepository _ventaRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly Ilogger _logger;

        public AdminService(
            IUsuarioAdminRepository usuarioRepo,
            IPersonalRepository personalRepo,
            ISistemaLogRepository logRepo,
            ICitaRepository citaRepo,
            IVentaRepository ventaRepo,
            IProductoRepository productoRepo,
            Ilogger logger)
        {
            _usuarioRepo = usuarioRepo;
            _personalRepo = personalRepo;
            _logRepo = logRepo;
            _citaRepo = citaRepo;
            _ventaRepo = ventaRepo;
            _productoRepo = productoRepo;
            _logger = logger;
        }

        // ====================================
        // DASHBOARD
        // ====================================
        public async Task<DashboardStatsDto> ObtenerEstadisticasDashboardAsync()
        {
            try
            {
                // ✅ Usar repositorios en lugar de _context
                var totalUsuarios = await _usuarioRepo.ContarTotalAsync();
                var totalPersonal = await _personalRepo.ContarTotalAsync();
                var citasMes = await _citaRepo.ContarCitasMesActualAsync();
                var ingresosMes = await _ventaRepo.ObtenerIngresosMesActualAsync();

                // Obtener usuarios por rol
                var usuarios = await _usuarioRepo.GetAllAsync();

                return new DashboardStatsDto
                {
                    TotalUsuarios = totalUsuarios,
                    TotalPersonal = totalPersonal,
                    CitasMes = citasMes,
                    IngresosMes = ingresosMes,
                    UsuariosPorRol = new UsuariosPorRolDto
                    {
                        Administradores = usuarios.Count(u => u.Role?.Rol == "Admin"),
                        Veterinarios = usuarios.Count(u => u.Role?.Rol == "Veterinario"),
                        Recepcionistas = usuarios.Count(u => u.Role?.Rol == "Recepcionista"),
                        Clientes = usuarios.Count(u => u.Role?.Rol == "Cliente")
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener estadísticas: {ex.Message}");
                throw;
            }
        }

        // ====================================
        // GESTIÓN DE USUARIOS
        // ====================================
        public async Task<IEnumerable<UsuarioAdminDto>> ObtenerTodosUsuariosAsync()
        {
            try
            {
                var usuarios = await _usuarioRepo.GetAllAsync();
                return usuarios.Select(MapToUsuarioAdminDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener usuarios: {ex.Message}");
                throw;
            }
        }

        public async Task<UsuarioAdminDto?> ObtenerUsuarioPorIdAsync(int id)
        {
            try
            {
                var usuario = await _usuarioRepo.GetByIdAsync(id);
                return usuario != null ? MapToUsuarioAdminDto(usuario) : null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener usuario {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<UsuarioAdminDto> CrearUsuarioAsync(CrearUsuarioAdminDto dto)
        {
            try
            {
                // Validar que el email no exista
                if (await _usuarioRepo.ExisteEmailAsync(dto.Email))
                {
                    throw new Exception("El email ya está registrado");
                }

                var usuario = new Usuario
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    PasswordHashed = HashPassword(dto.Password),
                    RoleId = dto.RoleId,
                    PhotoUrl = dto.PhotoUrl,
                    Activo = true,
                    UltimaConexion = null
                };

                var resultado = await _usuarioRepo.AddAsync(usuario);

                await RegistrarLogAsync(new RegistrarLogDto
                {
                    Usuario = "Admin",
                    Accion = $"Creó usuario: {resultado.Email}",
                    Tipo = "INFO",
                    Detalles = $"Nuevo usuario con rol {resultado.Role?.Rol ?? "Sin rol"}"
                });

                _logger.Info($"Usuario creado: {resultado.Email}");

                return MapToUsuarioAdminDto(resultado);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al crear usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<UsuarioAdminDto?> ActualizarUsuarioAsync(int id, ActualizarUsuarioAdminDto dto)
        {
            try
            {
                var usuarioExistente = await _usuarioRepo.GetByIdAsync(id);
                if (usuarioExistente == null)
                    return null;

                // Validar email único
                if (await _usuarioRepo.ExisteEmailAsync(dto.Email, id))
                {
                    throw new Exception("El email ya está siendo usado por otro usuario");
                }

                var usuario = new Usuario
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    RoleId = dto.RoleId,
                    PhotoUrl = dto.PhotoUrl,
                    Activo = dto.Activo,
                    PasswordHashed = usuarioExistente.PasswordHashed,
                    UltimaConexion = usuarioExistente.UltimaConexion
                };

                var resultado = await _usuarioRepo.UpdateAsync(id, usuario);
                if (resultado == null)
                    return null;

                await RegistrarLogAsync(new RegistrarLogDto
                {
                    Usuario = "Admin",
                    Accion = $"Actualizó usuario: {resultado.Email}",
                    Tipo = "INFO",
                    Detalles = $"Usuario ID: {id}"
                });

                _logger.Info($"Usuario actualizado: {resultado.Email}");

                return MapToUsuarioAdminDto(resultado);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al actualizar usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            try
            {
                var usuario = await _usuarioRepo.GetByIdAsync(id);
                if (usuario == null)
                    return false;

                var resultado = await _usuarioRepo.DeleteAsync(id);

                if (resultado)
                {
                    await RegistrarLogAsync(new RegistrarLogDto
                    {
                        Usuario = "Admin",
                        Accion = $"Eliminó usuario: {usuario.Email}",
                        Tipo = "WARNING",
                        Detalles = $"Usuario ID: {id}"
                    });

                    _logger.Info($"Usuario eliminado: {usuario.Email}");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al eliminar usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> CambiarPasswordUsuarioAsync(int id, CambiarPasswordDto dto)
        {
            try
            {
                var usuario = await _usuarioRepo.GetByIdAsync(id);
                if (usuario == null)
                    return false;

                usuario.PasswordHashed = HashPassword(dto.NuevaPassword);

                var resultado = await _usuarioRepo.UpdateAsync(id, usuario);

                if (resultado != null)
                {
                    await RegistrarLogAsync(new RegistrarLogDto
                    {
                        Usuario = "Admin",
                        Accion = $"Cambió contraseña de usuario: {usuario.Email}",
                        Tipo = "INFO",
                        Detalles = $"Usuario ID: {id}"
                    });

                    _logger.Info($"Contraseña cambiada para usuario: {usuario.Email}");
                }

                return resultado != null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al cambiar contraseña: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ActivarDesactivarUsuarioAsync(int id, bool activo)
        {
            try
            {
                var usuario = await _usuarioRepo.GetByIdAsync(id);
                if (usuario == null)
                    return false;

                usuario.Activo = activo;

                var resultado = await _usuarioRepo.UpdateAsync(id, usuario);

                if (resultado != null)
                {
                    await RegistrarLogAsync(new RegistrarLogDto
                    {
                        Usuario = "Admin",
                        Accion = $"{(activo ? "Activó" : "Desactivó")} usuario: {usuario.Email}",
                        Tipo = "INFO",
                        Detalles = $"Usuario ID: {id}"
                    });

                    _logger.Info($"Usuario {(activo ? "activado" : "desactivado")}: {usuario.Email}");
                }

                return resultado != null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al cambiar estado de usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UsuarioAdminDto>> BuscarUsuariosAsync(string termino)
        {
            try
            {
                var usuarios = await _usuarioRepo.BuscarAsync(termino);
                return usuarios.Select(MapToUsuarioAdminDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al buscar usuarios: {ex.Message}");
                throw;
            }
        }

        // ====================================
        // GESTIÓN DE PERSONAL
        // ====================================
        public async Task<IEnumerable<PersonalDto>> ObtenerTodoPersonalAsync()
        {
            try
            {
                var personal = await _personalRepo.GetAllAsync();
                return personal.Select(MapToPersonalDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener personal: {ex.Message}");
                throw;
            }
        }

        public async Task<PersonalDto?> ObtenerPersonalPorIdAsync(int id)
        {
            try
            {
                var personal = await _personalRepo.GetByIdAsync(id);
                return personal != null ? MapToPersonalDto(personal) : null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener personal {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<PersonalDto> CrearPersonalAsync(CrearPersonalDto dto)
        {
            try
            {
                // Validar cédula única
                if (await _personalRepo.ExisteCedulaAsync(dto.Cedula))
                {
                    throw new Exception("La cédula ya está registrada");
                }

                var personal = new Personal
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Cedula = dto.Cedula,
                    Cargo = dto.Cargo,
                    Activo = true
                };

                var resultado = await _personalRepo.AddAsync(personal);

                await RegistrarLogAsync(new RegistrarLogDto
                {
                    Usuario = "Admin",
                    Accion = $"Creó personal: {resultado.Nombre} {resultado.Apellido}",
                    Tipo = "INFO",
                    Detalles = $"Cargo: {resultado.Cargo}"
                });

                _logger.Info($"Personal creado: {resultado.Nombre} {resultado.Apellido}");

                return MapToPersonalDto(resultado);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al crear personal: {ex.Message}");
                throw;
            }
        }

        public async Task<PersonalDto?> ActualizarPersonalAsync(int id, ActualizarPersonalDto dto)
        {
            try
            {
                // Validar cédula única
                if (await _personalRepo.ExisteCedulaAsync(dto.Cedula, id))
                {
                    throw new Exception("La cédula ya está siendo usada por otro registro");
                }

                var personal = new Personal
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Cedula = dto.Cedula,
                    Cargo = dto.Cargo,
                    Activo = true
                };

                var resultado = await _personalRepo.UpdateAsync(id, personal);
                if (resultado == null)
                    return null;

                await RegistrarLogAsync(new RegistrarLogDto
                {
                    Usuario = "Admin",
                    Accion = $"Actualizó personal: {resultado.Nombre} {resultado.Apellido}",
                    Tipo = "INFO",
                    Detalles = $"Personal ID: {id}"
                });

                _logger.Info($"Personal actualizado: {resultado.Nombre} {resultado.Apellido}");

                return MapToPersonalDto(resultado);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al actualizar personal: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EliminarPersonalAsync(int id)
        {
            try
            {
                var personal = await _personalRepo.GetByIdAsync(id);
                if (personal == null)
                    return false;

                var resultado = await _personalRepo.DeleteAsync(id);

                if (resultado)
                {
                    await RegistrarLogAsync(new RegistrarLogDto
                    {
                        Usuario = "Admin",
                        Accion = $"Eliminó personal: {personal.Nombre} {personal.Apellido}",
                        Tipo = "WARNING",
                        Detalles = $"Personal ID: {id}"
                    });

                    _logger.Info($"Personal eliminado: {personal.Nombre} {personal.Apellido}");
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al eliminar personal: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PersonalDto>> BuscarPersonalAsync(string termino)
        {
            try
            {
                var personal = await _personalRepo.BuscarAsync(termino);
                return personal.Select(MapToPersonalDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al buscar personal: {ex.Message}");
                throw;
            }
        }

 

        // ====================================
        // REPORTES
        // ====================================
        public async Task<ReporteDto> GenerarReporteAsync(string tipo, DateTime desde, DateTime hasta)
        {
            try
            {
                object datos = tipo.ToLower() switch
                {
                    "ventas" => await GenerarReporteVentasAsync(desde, hasta),
                    "citas" => await GenerarReporteCitasAsync(desde, hasta),
                    "inventario" => await GenerarReporteInventarioAsync(),
                    "clientes" => await GenerarReporteClientesAsync(),
                    _ => throw new Exception("Tipo de reporte no válido")
                };

                await RegistrarLogAsync(new RegistrarLogDto
                {
                    Usuario = "Admin",
                    Accion = $"Generó reporte de {tipo}",
                    Tipo = "INFO",
                    Detalles = $"Desde: {desde:yyyy-MM-dd}, Hasta: {hasta:yyyy-MM-dd}"
                });

                return new ReporteDto
                {
                    TipoReporte = tipo,
                    FechaInicio = desde,
                    FechaFin = hasta,
                    Datos = datos
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al generar reporte: {ex.Message}");
                throw;
            }
        }

        // ====================================
        // MÉTODOS PRIVADOS DE REPORTES
        // ====================================

        private async Task<object> GenerarReporteVentasAsync(DateTime desde, DateTime hasta)
        {
            var ventas = await _ventaRepo.GetByFechaRangoAsync(desde, hasta);

            return new
            {
                TotalVentas = ventas.Count(),
                MontoTotal = ventas.Sum(v => v.Total),
                VentasPorDia = ventas.GroupBy(v => v.Fecha.Date)
                    .Select(g => new
                    {
                        Fecha = g.Key,
                        Cantidad = g.Count(),
                        Monto = g.Sum(v => v.Total)
                    })
                    .OrderBy(x => x.Fecha),
                VentasPorMetodoPago = ventas.GroupBy(v => v.MetodoPago)
                    .Select(g => new
                    {
                        MetodoPago = g.Key,
                        Cantidad = g.Count(),
                        Monto = g.Sum(v => v.Total)
                    })
            };
        }

        private async Task<object> GenerarReporteCitasAsync(DateTime desde, DateTime hasta)
        {
            var citas = await _citaRepo.ObtenerCitasPorRangoFechaAsync(desde, hasta);

            return new
            {
                TotalCitas = citas.Count(),
                CitasPorEstado = citas.GroupBy(c => c.Estado)
                    .Select(g => new
                    {
                        Estado = g.Key,
                        Cantidad = g.Count()
                    })
            };
        }

        private async Task<object> GenerarReporteInventarioAsync()
        {
            var productos = await _productoRepo.GetAllAsync();

            return new
            {
                TotalProductos = productos.Count(),
                ProductosBajoStock = productos.Count(p => p.Stock < 10),
                ValorTotalInventario = productos.Sum(p => p.Stock * p.Precio),
                ProductosPorCategoria = productos
                    .GroupBy(p => p.TipoProducto?.Tipo ?? "Sin categoría")
                    .Select(g => new
                    {
                        Categoria = g.Key,
                        Cantidad = g.Count(),
                        Stock = g.Sum(p => p.Stock)
                    })
            };
        }

        private async Task<object> GenerarReporteClientesAsync()
        {
            // ✅ CORREGIDO - Sin parámetros 'desde' y 'hasta'
            await Task.CompletedTask; // Para evitar warning CS1998

            return new
            {
                TotalClientes = 0,
                ClientesActivos = 0,
                NuevosEsteMes = 0,
                Mensaje = "Reporte de clientes pendiente de implementar"
            };
        }




        // ====================================
        // LOGS
        // ====================================
        public async Task RegistrarLogAsync(RegistrarLogDto dto)
        {
            try
            {
                var log = new SistemaLog
                {
                    Timestamp = DateTime.Now,
                    Usuario = dto.Usuario,
                    Accion = dto.Accion,
                    Tipo = dto.Tipo,
                    Detalles = dto.Detalles
                };

                await _logRepo.AddAsync(log);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al registrar log: {ex.Message}");
                // No lanzar excepción para no interrumpir el flujo principal
            }
        }

        public async Task<IEnumerable<LogDto>> ObtenerLogsAsync(string? tipo, DateTime? fecha)
        {
            try
            {
                var logs = await _logRepo.GetByFiltrosAsync(tipo, fecha);
                return logs.Select(l => new LogDto
                {
                    Id = l.Id,
                    Timestamp = l.Timestamp,
                    Usuario = l.Usuario,
                    Accion = l.Accion,
                    Tipo = l.Tipo,
                    Detalles = l.Detalles
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al obtener logs: {ex.Message}");
                throw;
            }
        }

        // ====================================
        // MAPPERS
        // ====================================
        private static UsuarioAdminDto MapToUsuarioAdminDto(Usuario usuario)
        {
            return new UsuarioAdminDto
            {
                Id = usuario.Id,
                UserName = usuario.UserName,
                Email = usuario.Email,
                RoleId = usuario.RoleId,
                RolNombre = usuario.Role?.Rol ?? "Sin rol",
                PhotoUrl = usuario.PhotoUrl,
                UltimaConexion = usuario.UltimaConexion,
                Activo = usuario.Activo
            };
        }

        private static PersonalDto MapToPersonalDto(Personal personal)
        {
            return new PersonalDto
            {
                Id = personal.Id,
                Nombre = personal.Nombre,
                Apellido = personal.Apellido,
                Cedula = personal.Cedula,
                Cargo = personal.Cargo,
                Activo = personal.Activo
            };
        }

        // Hash de contraseña
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}