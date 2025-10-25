using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ucelo.Domain.Entities;

namespace Ucelo.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(u => u.SaltHash)
            .HasColumnName("salt_hash")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(u => u.UserType)
            .HasColumnName("tipo_usuario")
            .IsRequired();

        builder.Property(u => u.IsActive)
            .HasColumnName("ativo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("usuarios_email_key");

        builder.HasOne(u => u.Individual)
            .WithOne(i => i.User)
            .HasForeignKey<Individual>(i => i.UserId);

        builder.HasOne(u => u.Company)
            .WithOne(c => c.User)
            .HasForeignKey<Company>(c => c.UserId);
    }
}