using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Services
{
    public class MotivoCitaService : IMotivoCitaService
    {
        private readonly IMotivoCitaRepository _repo;
        public MotivoCitaService(IMotivoCitaRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<MotivoCitaDto>> ObtenerMotivosAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(m => new MotivoCitaDto
            {
                Id = m.Id,
                Motivo = m.Motivo
            }).ToList();
        }

        public async Task<MotivoCitaDto?> ObtenerPorIdAsync(int id)
        {
            var motivo = await _repo.GetByIdAsync(id);
            if (motivo == null) return null;

            return new MotivoCitaDto
            {
                Id = motivo.Id,
                Motivo = motivo.Motivo
            };
        }

        public async Task<MotivoCitaDto> CrearAsync(CrearMotivoCitaDto dto)
        {
            var motivo = new MotivoCita
            {
                Motivo = dto.Motivo
            };

            var creado = await _repo.AddAsync(motivo);

            return new MotivoCitaDto
            {
                Id = creado.Id,
                Motivo = creado.Motivo
            };
        }

        public async Task<MotivoCitaDto?> ActualizarAsync(int id, ActualizarMotivoCitaDto dto)
        {
            var motivo = await _repo.GetByIdAsync(id);
            if (motivo == null) return null;

            motivo.Motivo = dto.Motivo;

            var actualizado = await _repo.UpdateAsync(motivo);
            if (actualizado == null) return null;

            return new MotivoCitaDto
            {
                Id = actualizado.Id,
                Motivo = actualizado.Motivo
            };
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
