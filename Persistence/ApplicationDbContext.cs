using Microsoft.EntityFrameworkCore;

namespace webapi.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Organization> Organizations { get; set; } = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(builder);
        builder.Entity<Organization>().HasData(
            new Organization()
            {
                Id = Guid.Parse("d7b8b57c-5d7b-4e8a-9a9c-b2b7d2b21d9f"),
                Name = "KoyaOrg",
                Description = "Amazing Organization",
            },
            new Organization
            {
                Id = Guid.Parse("c2a9133d-3e52-4c90-bb9d-bbc7b5284b44"),
                Name = "Smartwatch Organization",
                Description = "Track your fitness and receive notifications.",
            }
        );
    }
}