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
}