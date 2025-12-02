using AutoMapper;
using PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos;
using PetCareApp.Core.Domain.Entities;

public class MascotaPruebaMedicaMappingProfile : Profile
{
    public MascotaPruebaMedicaMappingProfile()
    {
        CreateMap<MascotaPruebasMedica, MascotaPruebaMedicaDto>()
            .ForMember(dest => dest.MascotaId, opt => opt.MapFrom(src => src.MascotaId))
            .ForMember(dest => dest.PruebaMedicaId, opt => opt.MapFrom(src => src.PruebaMedicaId))
            .ForMember(dest => dest.Resultado, opt => opt.MapFrom(src => src.Resultado));

        CreateMap<CreateMascotaPruebaMedicaDto, MascotaPruebasMedica>()
            .ForMember(dest => dest.MascotaId, opt => opt.MapFrom(src => src.MascotaId))
            .ForMember(dest => dest.PruebaMedicaId, opt => opt.MapFrom(src => src.PruebaMedicaId))
            .ForMember(dest => dest.Resultado, opt => opt.MapFrom(src => src.Resultado))
            .ForMember(dest => dest.CitaId, opt => opt.MapFrom(src => src.CitaId))
            //.ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
            .ForMember(dest => dest.Mascota, opt => opt.Ignore())
            .ForMember(dest => dest.PruebaMedica, opt => opt.Ignore());
    }
}
