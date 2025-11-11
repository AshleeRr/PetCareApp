
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class RazaMascotRepository : GeneRepositorio<RazaMascota>, IRazaMascotaRepository
    {
        private readonly PetCareContext _context;

        public RazaMascotRepository(PetCareContext context) : base(context)
        {
            _context = context;
        }
    }
}
