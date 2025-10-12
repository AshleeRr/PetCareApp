namespace PetCareApp.Core.Domain.Entities;

public class Role
{
    public int Id { get; set; }

    public required string Rol { get; set; } 

    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
