using PetCareApp.Application.Interfaces;
using PetCareApp.Core.Application.Dtos;
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
    public class TipoMascotaRepository : Repository<TipoMascota>, ITipoMascotaRepository
    {
        public TipoMascotaRepository(PetCareContext context) : base(context)
        {
        }
    }
}
