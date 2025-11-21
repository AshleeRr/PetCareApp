using AutoMapper;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Dtos.MascotasDtos;
using PetCareApp.Core.Application.ViewModels.CitasVms;
using PetCareApp.Core.Application.ViewModels.HistorialVms;
using PetCareApp.Core.Application.ViewModels.MascotasPruebasMedicasVms;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Mappings.Dtos_Vm
{
    public class HistorialDtoMappingProfile : Profile
    {
        public HistorialDtoMappingProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<MascotaHistorialDto, HistorialViewModel>();

            // Submapeos de los elementos internos
            CreateMap<CitaDto, CitaViewModel>();
            //CreateMap<CreateMascotaPruebaMedicaDto, PruebaMedicaViewModel>();
            CreateMap<MascotaPruebasMedica, MascotaPruebaMedicaHistorialDto>()
                .ForMember(dest => dest.NombrePrueba, opt => opt.MapFrom(src => src.PruebaMedica.NombrePrueba))
                .ForMember(dest => dest.Resultado, opt => opt.MapFrom(src => src.Resultado))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha));

            CreateMap<MascotaPruebaMedicaHistorialDto, MascotaPruebaMedicaHistorialVm>();

        }
    }
}
