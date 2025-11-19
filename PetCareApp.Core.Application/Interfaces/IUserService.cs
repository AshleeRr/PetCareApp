
using PetCareApp.Core.Application.Dtos.UsersDtos;

namespace PetCareApp.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByEmailAsync(string email);
    }
}
