using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;

namespace PetCareApp.Infraestructure.Persistence.Context;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions<AppContext> options): base(options){}

    #region DbSets
    public  DbSet<Cita> Citas { get; set; }
    public  DbSet<Despacho> Despachos { get; set; }
    public  DbSet<Dueño> Dueños { get; set; }
    public  DbSet<Estado> Estados { get; set; }
    public  DbSet<LogInventario> LogInventarios { get; set; }
    public  DbSet<MascotaPruebasMedica> MascotaPruebasMedicas { get; set; }
    public  DbSet<Mascota> Mascota { get; set; }
    public  DbSet<Medicamento> Medicamentos { get; set; }
    public  DbSet<MotivoCita> MotivoCita { get; set; }
    public  DbSet<Personal> Personals { get; set; }
    public  DbSet<Producto> Productos { get; set; }
    public  DbSet<Proveedor> Proveedors { get; set; }
    public  DbSet<PruebasMedica> PruebasMedicas { get; set; }
    public  DbSet<Receta> Recetas { get; set; }
    public  DbSet<RecetaMedicamento> RecetaMedicamentos { get; set; }
    public  DbSet<Role> Roles { get; set; }
    public  DbSet<Telefono> Telefonos { get; set; }
    public  DbSet<TipoMascota> TipoMascota { get; set; }
    public  DbSet<TipoProducto> TipoProductos { get; set; }
    public  DbSet<Usuario> Usuarios { get; set; }
    #endregion

    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=PETCARE;Trusted_Connection=True;TrustServerCertificate=True;");
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
       

    }
}
