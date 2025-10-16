using Microsoft.EntityFrameworkCore;
using PetCareApp.Core.Domain.Entities;
using System.Reflection;

namespace PetCareApp.Infraestructure.Persistence.Context;

public class PetCareContext : DbContext
{
    public PetCareContext(DbContextOptions<PetCareContext> options): base(options){}

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dueño>().ToTable("Dueños");
        modelBuilder.Entity<Telefono>().ToTable("Telefonos");
    } 
}
