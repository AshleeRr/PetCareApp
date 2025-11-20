
using AutoMapper;
using PetCareApp.Core.Application.Dtos.MascotaPruebaMedicaDtos;
using PetCareApp.Core.Application.ViewModels.MascotasPruebasMedicasVms;

namespace PetCareApp.Core.Application.Mappings.Dtos_Vm
{
    public class MascotaPruebaMedicaDtoMappingProfile : Profile
    {
        public MascotaPruebaMedicaDtoMappingProfile()
        {
            CreateMap<CreateMascotaPruebaMedicaVm, CreateMascotaPruebaMedicaDto>()
                .ReverseMap();
        }
    }
}
