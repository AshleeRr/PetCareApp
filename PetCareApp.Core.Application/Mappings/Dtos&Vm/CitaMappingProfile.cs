
using AutoMapper;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.ViewModels.CitasVms;

namespace PetCareApp.Core.Application.Mappings.Dtos_Vm
{
    public class CitaMappingProfile
    {
        public class CitaDtoMappingProfile : Profile
        {
            public CitaDtoMappingProfile()
            {
                //Mapeo general para la lista de citas
                CreateMap<CitaDto, CitaViewModel>()
                    .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.FechaHora))
                    .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.Motivo))
                    .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

                //Mapeo para los detalles
                CreateMap<CitaDto, CitaDetalleViewModel>()
                    .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.FechaHora)))
                    .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.Motivo))
                    .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                    .ForMember(dest => dest.VeterinarioNombre, opt => opt.MapFrom(src => src.Veterinario))
                    .ForMember(dest => dest.MascotaNombre, opt => opt.MapFrom(src => src.Mascota))
                    .ForMember(dest => dest.DueñoNombre, opt => opt.MapFrom(src => src.Cliente));
            }
        }
    }
}
