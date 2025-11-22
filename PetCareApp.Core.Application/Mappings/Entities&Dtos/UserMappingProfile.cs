using AutoMapper;
using PetCareApp.Core.Application.Dtos.UsersDtos;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Core.Application.Mappings.Entities_Dtos
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<Usuario, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role!.Rol));
        }
    }
}
