using PetCareApp.Application.Interfaces;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Infraestructure.Persistence.Context;
using PetCareApp.Infrastructure.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Infraestructure.Persistence.Repositories
{
    public class TipoProductoRepository : Repository<TipoProducto>, ITipoProductoRepository
    {
        public TipoProductoRepository(PetCareContext context) : base(context)
        {
        }
    }
}
