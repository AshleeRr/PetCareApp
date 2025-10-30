using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Core.Application.Services
{
    public class EstadoService : IEstadoService
    {
        private readonly IEstadoRepository _repo;
        public EstadoService(IEstadoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<EstadoDto>> ObtenerEstadosAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(e => new EstadoDto
            {
                Id = e.Id,
                Nombre = e.Nombre
            }).ToList();
        }
    }
}
