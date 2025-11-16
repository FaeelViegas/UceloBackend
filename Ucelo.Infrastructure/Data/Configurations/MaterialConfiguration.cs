using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ucelo.Domain.Entities;

namespace Ucelo.Infrastructure.Data.Configurations
{
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable("materiais");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("id");

            builder.Property(m => m.Nome)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.ValorImpacto)
                .HasColumnName("valor_impacto");

            builder.Property(m => m.TensaoEscoamento)
                .HasColumnName("tensao_escoamento");

            builder.Property(m => m.ModuloElasticidade)
                .HasColumnName("modulo_elasticidade");

            builder.Property(m => m.Abrasao)
                .HasColumnName("abrasao");

            builder.Property(m => m.TemperaturaMaxima)
                .HasColumnName("temperatura_maxima");

            builder.Property(m => m.Ativo)
                .HasColumnName("ativo")
                .HasDefaultValue(true);
        }
    }
}