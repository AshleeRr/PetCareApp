using AutoMapper;
using PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Mappings.Entities_Dtos
{
    public class MascotaPruebaMedicaMappingProfile : Profile
    {
        public MascotaPruebaMedicaMappingProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<MascotaPruebasMedica, MascotaPruebaMedicaDto>()
                .ForMember(dest => dest.MascotaId, opt => opt.MapFrom(src => src.MascotaId))
                .ForMember(dest => dest.PruebaMedicaId, opt => opt.MapFrom(src => src.PruebaMedicaId))
                .ForMember(dest => dest.Resultado, opt => opt.MapFrom(src => src.Resultado));
        }
    }
}
