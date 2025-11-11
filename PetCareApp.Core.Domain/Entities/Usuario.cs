namespace PetCareApp.Core.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }

    public required string UserName { get; set; } 

    public required string PasswordHashed { get; set; } 

    public required string Email { get; set; } 

    public string? PhotoUrl { get; set; }

    public int RoleId { get; set; }

    public Role Role { get; set; } = null!;
    public ICollection<Receta>? Recetas { get; set; } 
}
