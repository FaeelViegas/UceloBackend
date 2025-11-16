using Microsoft.EntityFrameworkCore;
using Ucelo.Domain.Entities;
using Ucelo.Infrastructure.Data.Configurations;

namespace Ucelo.Infrastructure.Data;

public class UceloDbContext : DbContext
{
    public UceloDbContext(DbContextOptions<UceloDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<LoginAttempt> LoginAttempts { get; set; }

    public DbSet<Calculation> Calculations { get; set; }
    public DbSet<Bucket> Buckets { get; set; }

    public DbSet<Material> Materials { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UceloDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new BucketConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialConfiguration());
    }
}