
using AutoMapper;
using PetCareApp.Core.Application.Dtos.MedicamentosDtos;
using PetCareApp.Core.Application.ViewModels.MedicamentosVms;

namespace PetCareApp.Core.Application.Mappings.Dtos_Vm
{
    public class MedicamentoDtoMappingProfile : Profile
    {
        public MedicamentoDtoMappingProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<MedicamentoDto, MedicamentoVm>()
                .ReverseMap();
        }
    }
}
