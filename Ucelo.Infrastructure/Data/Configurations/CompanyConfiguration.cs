using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ucelo.Domain.Entities;

namespace Ucelo.Infrastructure.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("pessoas_juridicas");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UserId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.Property(c => c.LegalName)
            .HasColumnName("razao_social")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.TradeName)
            .HasColumnName("nome_fantasia")
            .HasMaxLength(500);

        builder.Property(c => c.TaxId)
            .HasColumnName("cnpj")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.StateRegistration)
            .HasColumnName("inscricao_estadual")
            .HasMaxLength(20);

        builder.Property(c => c.Phone)
            .HasColumnName("telefone")
            .HasMaxLength(20);

        builder.Property(c => c.CorporateEmail)
            .HasColumnName("email_corporativo")
            .HasMaxLength(100);

        builder.Property(c => c.IsActive)
            .HasColumnName("ativa")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(c => c.TaxId)
            .IsUnique()
            .HasDatabaseName("pessoas_juridicas_cnpj_key");

        builder.HasIndex(c => c.UserId)
            .IsUnique()
            .HasDatabaseName("pessoas_juridicas_usuario_id_key");
    }
}