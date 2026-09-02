using Microsoft.EntityFrameworkCore;

namespace Tecnyfarma.Server.User.Infrastructure.DataBase;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

    public DbSet<Models.User> Users => Set<Models.User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.User>(builder =>
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);

            builder.Property(u => u.Password).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.CreatedAtUtc).IsRequired();
            builder.Property(u => u.Type).IsRequired();
        });
    }
}