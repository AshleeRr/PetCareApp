using PetCareApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCareApp.Domain.Interfaces;
using PetCareApp.Core.Application.Dtos;

namespace PetCareApp.Application.Services
{
    public class TipoProductoService : ITipoProductoService
    {
        private readonly ITipoProductoRepository _repo;

        public TipoProductoService(ITipoProductoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<TipoProductoDto>> ObtenerTiposAsync()
        {
            var tipos = await _repo.GetAllAsync();
            return tipos.Select(t => new TipoProductoDto
            {
                Id = t.Id,
                Tipo = t.Tipo
            }).ToList();
        }

        public async Task<TipoProductoDto?> ObtenerPorIdAsync(int id)
        {
            var tipo = await _repo.GetByIdAsync(id);
            if (tipo == null) return null;

            return new TipoProductoDto
            {
                Id = tipo.Id,
                Tipo = tipo.Tipo
            };
        }
    }
}