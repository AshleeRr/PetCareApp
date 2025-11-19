
namespace PetCareApp.Core.Application.ViewModels.UsersVms
{
    public class UserVm
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? PhotoUrl { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
