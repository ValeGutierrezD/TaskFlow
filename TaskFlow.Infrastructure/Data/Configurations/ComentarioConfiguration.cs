using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Data.Configurations
{
    public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("comentarios");
            builder.Property(e => e.Contenido).HasColumnType("text").IsRequired();
            builder.Property(e => e.FechaCreacion).HasColumnType("datetime").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasOne(e => e.Tarea)
                .WithMany()
                .HasForeignKey(e => e.TareaId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
