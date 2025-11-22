
using AutoMapper;
using PetCareApp.Core.Application.Dtos.RecetasDtos;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Mappings.Entities_Dtos
{
    public class RecetaMappingProfile : Profile
    {
        public RecetaMappingProfile()
        {
            // Mapping configurations will go here in the future
            CreateMap<Receta, RecetaDto>()
            .ForMember(dest => dest.RecetaMedicamentos, opt =>
                opt.MapFrom(src => src.RecetaMedicamentos));
            
            CreateMap<RecetaMedicamento, RecetaMedicamentoDto>()
            .ForMember(dest => dest.Nombre,
                opt => opt.MapFrom(src => src.Medicamento.Nombre));

            CreateMap<CreateRecetaDto, Receta>();

        }
    }
}
