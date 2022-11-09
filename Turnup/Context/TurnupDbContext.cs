using Microsoft.EntityFrameworkCore;
using Turnup.Entities;

namespace Turnup.Context;

public class TurnupDbContext : DbContext
{
    public TurnupDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    
}