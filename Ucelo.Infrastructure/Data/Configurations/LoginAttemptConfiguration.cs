using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ucelo.Domain.Entities;

namespace Ucelo.Infrastructure.Data.Configurations;

public class LoginAttemptConfiguration : IEntityTypeConfiguration<LoginAttempt>
{
    public void Configure(EntityTypeBuilder<LoginAttempt> builder)
    {
        builder.ToTable("tentativas_login");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(l => l.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(l => l.Attempts)
            .HasColumnName("tentativas")
            .IsRequired();

        builder.Property(l => l.LockedUntil)
            .HasColumnName("bloqueado_ate");

        builder.Property(l => l.FirstAttempt)
            .HasColumnName("primeira_tentativa")
            .IsRequired();

        builder.Property(l => l.LastAttempt)
            .HasColumnName("ultima_tentativa")
            .IsRequired();

        builder.HasIndex(l => l.Email)
            .IsUnique()
            .HasDatabaseName("tentativas_login_email_key");

        builder.HasIndex(l => l.LockedUntil)
            .HasDatabaseName("idx_tentativas_login_bloqueado_ate");
    }
}