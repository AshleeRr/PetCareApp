
using AutoMapper;
using PetCareApp.Core.Application.Dtos.PruebasMedicasDtos;
using PetCareApp.Core.Application.ViewModels.PruebasVms;

namespace PetCareApp.Core.Application.Mappings.Dtos_Vm
{
    public class PruebaMedicaDtoMappingProfile : Profile
    {
        public PruebaMedicaDtoMappingProfile()
        {
            CreateMap<PruebaMedicaDto, PruebaMedicaViewModel>()
                .ReverseMap();
        }
    }
}
