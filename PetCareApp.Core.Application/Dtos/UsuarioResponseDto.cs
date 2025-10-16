using PetCareApp.Core.Domain.Entities;

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }

    public static UsuarioResponseDto FromUsuario(Usuario usuario, string token)
    {
        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Email = usuario.Email,
            UserName = usuario.UserName,
            Role = usuario.Role.Rol,
            Token = token
        };
    }
}