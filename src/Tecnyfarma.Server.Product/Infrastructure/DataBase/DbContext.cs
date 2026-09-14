using Microsoft.EntityFrameworkCore;

namespace Tecnyfarma.Server.Product.Infrastructure.DataBase;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options) : base(options) { }

    public DbSet<Models.User> Users => Set<Models.User>();
    public DbSet<Models.Product> Products => Set<Models.Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.User>(builder =>
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

         builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.Type).IsRequired();
        });
        
        modelBuilder.Entity<Models.Product>(builder =>
        {
            builder.ToTable("Products");

            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Description).HasMaxLength(1024);
            builder.Property(p => p.Price).IsRequired();
        });
    }
}