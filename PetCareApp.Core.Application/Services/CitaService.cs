using AutoMapper;
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

        public CitaService(ICitaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        private static CitaDto MapToDto(Cita c) => new()
        {
            Id = c.Id,
            FechaHora = c.FechaHora,
            Estado = c.Estado?.Nombre ?? "",
            Cliente = $"{c.Dueño?.Nombre} {c.Dueño?.Apellido}",
            Veterinario = $"{c.Veterinario?.Nombre} {c.Veterinario?.Apellido}",
            Motivo = c.Motivo?.Motivo ?? ""
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

        public async Task<CitaDto> CrearCitaAsync(CrearCitaDto dto)
        {
            var cita = new Cita
            {
                FechaHora = dto.FechaHora,
                EstadoId = dto.EstadoId,
                DueñoId = dto.DueñoId,
                VeterinarioId = dto.VeterinarioId,
                MotivoId = dto.MotivoId
            };

            var created = await _repo.AddAsync(cita);
            return MapToDto(created);
        }

        public async Task<bool> EditarCitaAsync(int id, ActualizarCitaDto dto)
        {
            var cita = await _repo.GetByIdAsync(id);
            if (cita == null) return false;

            cita.FechaHora = dto.FechaHora;
            cita.EstadoId = dto.EstadoId;
            cita.VeterinarioId = dto.VeterinarioId;
            cita.MotivoId = dto.MotivoId;

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
    }
}
