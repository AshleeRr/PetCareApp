
using AutoMapper;
using PetCareApp.Core.Application.Dtos.MedicamentosDtos;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Mappings.Entities_Dtos
{
    public class MedicamentoMappingProfile : Profile
    {
        public MedicamentoMappingProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<Medicamento, MedicamentoDto>()
                .ReverseMap()
                .ForMember(dest => dest.RecetaMedicamentos, opt => opt.Ignore());
        }
    }
}
