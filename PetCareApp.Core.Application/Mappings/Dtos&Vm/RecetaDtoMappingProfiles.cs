
using AutoMapper;
using PetCareApp.Core.Application.Dtos.RecetasDtos;
using PetCareApp.Core.Application.ViewModels.RecetaVms;

namespace PetCareApp.Core.Application.Mappings.Dtos_Vm
{
    public class RecetaDtoMappingProfiles : Profile
    {
        public RecetaDtoMappingProfiles()
        {
            // Future mapping configurations will be added here
            CreateMap<CreateRecetaVm, CreateRecetaDto>()
                .ReverseMap();

            CreateMap<RecetaDto, RecetaVm>()
                .ReverseMap();
        }
    }
}
