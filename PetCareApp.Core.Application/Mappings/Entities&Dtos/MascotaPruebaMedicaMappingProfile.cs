
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
                .ForMember(dest => dest.NombreMascota, opt => opt.MapFrom(src => src.Mascota.Nombre))
                .ForMember(dest => dest.NombrePruebaMedica, opt => opt.MapFrom(src => src.PruebaMedica.NombrePrueba))
                .ForMember(dest => dest.Resultado, opt => opt.MapFrom(src => src.Resultado))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Fecha)));
        }
    }
}
