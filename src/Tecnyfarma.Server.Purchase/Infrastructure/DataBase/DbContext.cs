using Microsoft.EntityFrameworkCore;

namespace Tecnyfarma.Server.Purchase.Infrastructure.DataBase;

public class DbContext  : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options) : base(options) { }

    public DbSet<Models.User> Users => Set<Models.User>();
    public DbSet<Models.Product> Products => Set<Models.Product>();
    public DbSet<Models.Purchase> Purchases => Set<Models.Purchase>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.User>(builder =>
        {
            builder.ToTable("Users");

            builder.HasKey(p => p.Id);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Type).IsRequired();
        });
        
        modelBuilder.Entity<Models.Product>(builder =>
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Price).IsRequired();
        });

        modelBuilder.Entity<Models.Purchase>(builder =>
        {
            builder.ToTable("Purchases");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(256);
            builder.Property(p => p.ProductId).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.CreatedAtUtc).IsRequired();
        });
    }
}