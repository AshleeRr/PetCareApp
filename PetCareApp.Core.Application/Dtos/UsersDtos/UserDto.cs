namespace PetCareApp.Core.Application.Dtos.UsersDtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }

        public string? PhotoUrl { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
