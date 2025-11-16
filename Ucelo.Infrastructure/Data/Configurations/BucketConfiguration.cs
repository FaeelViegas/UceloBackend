using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ucelo.Domain.Entities;

namespace Ucelo.Infrastructure.Data.Configurations
{
    public class BucketConfiguration : IEntityTypeConfiguration<Bucket>
    {
        public void Configure(EntityTypeBuilder<Bucket> builder)
        {
            builder.ToTable("canecas");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasColumnName("id");

            builder.Property(b => b.Codigo)
                .HasColumnName("codigo")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(b => b.Dimensions)
                .HasColumnName("dimensoes")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(b => b.Volume)
                .HasColumnName("volume")
                .IsRequired();

            builder.Property(b => b.VolumeBorda)
                .HasColumnName("volume_borda")
                .IsRequired();

            builder.Property(b => b.MaterialId)
                .HasColumnName("material_id")
                .IsRequired();

            builder.Property(b => b.Furacao)
                .HasColumnName("furacao")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(b => b.Deslocamento)
                .HasColumnName("deslocamento")
                .IsRequired();

            builder.Property(b => b.ResistenciaTracao)
                .HasColumnName("resistencia_tracao")
                .IsRequired();

            builder.Property(b => b.PassoRecomendado)
                .HasColumnName("passo_recomendado")
                .IsRequired();

            builder.Property(b => b.UnitPrice)
                .HasColumnName("preco_unitario");

            builder.Property(b => b.Ativo)
                .HasColumnName("ativo")
                .HasDefaultValue(true);

            builder.HasOne(b => b.Material)
                .WithMany()
                .HasForeignKey(b => b.MaterialId);
        }
    }
}