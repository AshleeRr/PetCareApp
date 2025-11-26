using PetCareApp.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IAdminService
    {
        // Dashboard
        Task<DashboardStatsDto> ObtenerEstadisticasDashboardAsync();

        // Usuarios
        Task<IEnumerable<UsuarioAdminDto>> ObtenerTodosUsuariosAsync();
        Task<UsuarioAdminDto?> ObtenerUsuarioPorIdAsync(int id);
        Task<UsuarioAdminDto> CrearUsuarioAsync(CrearUsuarioAdminDto dto);
        Task<UsuarioAdminDto?> ActualizarUsuarioAsync(int id, ActualizarUsuarioAdminDto dto);
        Task<bool> EliminarUsuarioAsync(int id);
        Task<bool> CambiarPasswordUsuarioAsync(int id, CambiarPasswordDto dto);
        Task<bool> ActivarDesactivarUsuarioAsync(int id, bool activo);
        Task<IEnumerable<UsuarioAdminDto>> BuscarUsuariosAsync(string termino);

        // Personal
        Task<IEnumerable<PersonalDto>> ObtenerTodoPersonalAsync();
        Task<PersonalDto?> ObtenerPersonalPorIdAsync(int id);
        Task<PersonalDto> CrearPersonalAsync(CrearPersonalDto dto);
        Task<PersonalDto?> ActualizarPersonalAsync(int id, ActualizarPersonalDto dto);
        Task<bool> EliminarPersonalAsync(int id);
        Task<IEnumerable<PersonalDto>> BuscarPersonalAsync(string termino);


        // Reportes
        Task<ReporteDto> GenerarReporteAsync(string tipo, DateTime desde, DateTime hasta);

        // Logs
        Task RegistrarLogAsync(RegistrarLogDto dto);
        Task<IEnumerable<LogDto>> ObtenerLogsAsync(string? tipo, DateTime? fecha);
    }
}
