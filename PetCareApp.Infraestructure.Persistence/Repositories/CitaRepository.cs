using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class CitaRepository : GeneRepositorio<Cita>, ICitaRepository
    {
        private readonly PetCareContext _context;

        public CitaRepository(PetCareContext context) : base(context)
        {
            _context = context;
        }
        public Task<List<Cita?>> GetCitasByDate(DateOnly date)
        {
            throw new NotImplementedException();
        }
    }
}
