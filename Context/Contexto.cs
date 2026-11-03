using Microsoft.EntityFrameworkCore;
using Saul_Ap1_P2.Models;

namespace Saul_Ap1_P2.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
        
    }

    public DbSet<Modelo> Modelo { get; set; }
}
