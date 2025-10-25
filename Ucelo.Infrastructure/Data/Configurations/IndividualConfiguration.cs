using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ucelo.Domain.Entities;

namespace Ucelo.Infrastructure.Data.Configurations;

public class IndividualConfiguration : IEntityTypeConfiguration<Individual>
{
    public void Configure(EntityTypeBuilder<Individual> builder)
    {
        builder.ToTable("pessoas_fisicas");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(i => i.UserId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.Property(i => i.FullName)
            .HasColumnName("nome_completo")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(i => i.TaxId)
            .HasColumnName("cpf")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(i => i.IdentityCard)
            .HasColumnName("rg")
            .HasMaxLength(20);

        builder.Property(i => i.BirthDate)
            .HasColumnName("data_nascimento");

        builder.Property(i => i.Phone)
            .HasColumnName("telefone")
            .HasMaxLength(20);

        builder.Property(i => i.MobilePhone)
            .HasColumnName("celular")
            .HasMaxLength(20);

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(i => i.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(i => i.TaxId)
            .IsUnique()
            .HasDatabaseName("pessoas_fisicas_cpf_key");

        builder.HasIndex(i => i.UserId)
            .IsUnique()
            .HasDatabaseName("pessoas_fisicas_usuario_id_key");
    }
}