
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;
using PetCareApp.Infraestructure.Persistence.Context;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class RecetaRepository : GeneRepositorio<Receta>, IRecetaRepository
    {
        private readonly PetCareContext _context;
        public RecetaRepository(PetCareContext context) : base(context)
        {
            _context = context;
        }
    }
}
