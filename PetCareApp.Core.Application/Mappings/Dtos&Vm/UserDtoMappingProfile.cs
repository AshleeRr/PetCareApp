using AutoMapper;
using PetCareApp.Core.Application.Dtos.UsersDtos;
using PetCareApp.Core.Application.ViewModels.UsersVms;

namespace PetCareApp.Core.Application.Mappings.Dtos_Vm
{
    public class UserDtoMappingProfile : Profile
    {
        public UserDtoMappingProfile() 
        {
            CreateMap<UserDto, UserVm>();
        }
    }
}
