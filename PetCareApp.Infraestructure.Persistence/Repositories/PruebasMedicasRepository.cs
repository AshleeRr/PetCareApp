using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class PruebasMedicasRepository : GeneRepositorio<PruebasMedica>, IPruebasMedicasRepository
    {
        private readonly PetCareContext _context;
        public PruebasMedicasRepository(PetCareContext context) : base(context)
        {
            _context = context;
        }
    }
}
