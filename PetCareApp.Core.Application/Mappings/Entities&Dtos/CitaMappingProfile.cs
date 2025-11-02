using AutoMapper;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Mappings.Entities_Dtos
{
    public class CitaMappingProfile : Profile
    {
        public CitaMappingProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<Cita, CitaDto>()
                .ForMember(dest => dest.Mascota, opt => opt.MapFrom(src => src.Mascota.Nombre))
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Dueño.Nombre))
                .ForMember(dest => dest.Veterinario, opt => opt.MapFrom(src => src.Veterinario.Nombre))
                .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.Motivo.Motivo))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.Nombre))
                .ReverseMap()
                .ForMember(dest => dest.Mascota, opt => opt.Ignore())
                .ForMember(dest => dest.Dueño, opt => opt.Ignore())
                .ForMember(dest => dest.Veterinario, opt => opt.Ignore())
                .ForMember(dest => dest.Motivo, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore());
        }
    }
}
