using Microsoft.EntityFrameworkCore;
using Saul_Ap1_P2.Models;

namespace Saul_Ap1_P2.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
        
    }

    public DbSet<Productos> Productos { get; set; }

    public DbSet<Ventas> Ventas { get; set; } 

    public DbSet<VentasDetalles> VentasDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Productos>().HasData(
            
            new Productos
            {
                ProductoId = 1,
                Nombre = "Starbucks Frappuccino Vainilla",
                Precio = 150,
                Stock = 10
            },

            new Productos
            {
                ProductoId = 2,
                Nombre = "Starbucks Frappuccino Mocha",
                Precio = 175,
                Stock = 10
            },

            new Productos
            {
                ProductoId = 3,
                Nombre = "Starbucks Frappuccino Caramelo",
                Precio = 160,
                Stock = 10
            },

            new Productos
            {
                ProductoId = 4,
                Nombre = "Starbucks Doubleshot Espresso",
                Precio = 130,
                Stock = 10
            },

            new Productos
            {
                ProductoId = 5,
                Nombre = "Starbucks Frappuccino White Chocolate",
                Precio = 180,
                Stock = 10
            }
        );  
    }
}
