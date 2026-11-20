using Microsoft.EntityFrameworkCore;

namespace Tecnyfarma.Server.User.Infrastructure.DataBase;

public class DbContext(DbContextOptions<DbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Models.User> Users => Set<Models.User>();
    public  DbSet<Models.Purchase> Purchases => Set<Models.Purchase>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.User>(builder =>
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(100);
            builder.Property(u => u.CreatedAtUtc).IsRequired();
            builder.Property(u => u.Type).IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Models.Purchase>(builder =>
        {
            builder.ToTable("Purchases");
            
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Price).IsRequired();
            builder.Property(u => u.CreatedAtUtc).IsRequired();
            
            builder.HasIndex(u => u.Email).IsUnique();
        });
    }
}