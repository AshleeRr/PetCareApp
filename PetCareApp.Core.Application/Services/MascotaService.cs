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
    public class MascotaService : IMascotaService
    {
        private readonly IMascotaRepository _repo;
        public MascotaService(IMascotaRepository repo) => _repo = repo;

        private static MascotaDto Map(Mascota m) => new()
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Edad = m.Edad,
            Peso = m.Peso,
            EstaCastrado = m.EstaCastrado,
            DueñoId = m.DueñoId,
            TipoMascotaId = m.TipoMascotaId,
        };

        public async Task<List<MascotaDto>> ObtenerTodosAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(Map).ToList();
        }

        public async Task<MascotaDto?> ObtenerPorIdAsync(int id)
        {
            var m = await _repo.GetByIdAsync(id);
            return m == null ? null : Map(m);
        }

        public async Task<List<MascotaDto>> FiltrarAsync(string nombre, int? tipoMascotaId, bool? estaCastrado)
        {
            var list = await _repo.FilterAsync(nombre, tipoMascotaId, estaCastrado);
            return list.Select(Map).ToList();
        }

        public async Task<MascotaDto> CrearAsync(CrearMascotaDto dto)
        {
            var m = new Mascota
            {

                Nombre = dto.Nombre,
                Edad = dto.Edad,
                Peso = dto.Peso,
                EstaCastrado = dto.EstaCastrado,
                DueñoId = dto.DueñoId,
                TipoMascotaId = dto.TipoMascotaId
            };
            var created = await _repo.AddAsync(m);
            return Map(created);
        }

        public async Task<bool> EditarAsync(int id, CrearMascotaDto dto)
        {
            var m = await _repo.GetByIdAsync(id);
            if (m == null) return false;
            m.Nombre = dto.Nombre;
            m.Edad = dto.Edad;
            m.Peso = dto.Peso;
            m.EstaCastrado = dto.EstaCastrado;
            m.TipoMascotaId = dto.TipoMascotaId;
            await _repo.UpdateAsync(m);
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var m = await _repo.GetByIdAsync(id);
            if (m == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
