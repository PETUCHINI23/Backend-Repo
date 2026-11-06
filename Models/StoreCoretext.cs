using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }

    public DbSet<Beer> Beers { get; set; }
    public DbSet<Brand> Brands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(brand =>
        {
            brand.Property(b => b.Name).HasMaxLength(20);
            brand.HasIndex(b => b.Name).IsUnique();
        });
    }
}
