using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Turnup.Entities;
using Turnup.Entities.OrderEntities;

namespace Turnup.Context;

public class TurnupDbContext : IdentityDbContext<AuthUser>
{
    public TurnupDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(false);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
    }

    public AuthUser AuthUser { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public DbSet<Establishment> Establishments { get; set; }
    
    public DbSet<Order> Orders { get; set; }

}