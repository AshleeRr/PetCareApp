
using AutoMapper;
using PetCareApp.Core.Application.Dtos;
using PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos;
using PetCareApp.Core.Application.Dtos.MascotasDtos;
using PetCareApp.Core.Application.ViewModels.CitasVms;
using PetCareApp.Core.Application.ViewModels.HistorialVms;
using PetCareApp.Core.Application.ViewModels.PruebasVms;

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
            CreateMap<MascotaPruebaMedicaDto, PruebaViewModel>();
        }
    }
}
