using Microsoft.EntityFrameworkCore;
using Ucelo.Domain.Entities;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UceloDbContext).Assembly);
    }
}