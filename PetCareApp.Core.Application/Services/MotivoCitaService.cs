using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Interfaces;
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
    }
}
