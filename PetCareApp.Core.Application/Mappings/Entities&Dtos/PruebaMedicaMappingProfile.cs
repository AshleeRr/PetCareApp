
using AutoMapper;
using PetCareApp.Core.Application.Dtos.PruebasMedicasDtos;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Mappings.Entities_Dtos
{
    public class PruebaMedicaMappingProfile : Profile
    {
        public PruebaMedicaMappingProfile() 
        {
            CreateMap<PruebasMedica, PruebaMedicaDto>()
                .ReverseMap()
                .ForMember(dest => dest.MascotaPruebasMedicas, opt => opt.Ignore());
        }
    }
}
