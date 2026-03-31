using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Data.Configurations
{
    public class ProyectoConfiguration : IEntityTypeConfiguration<Proyecto>
    {
        public void Configure(EntityTypeBuilder<Proyecto> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("proyectos");
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.HasOne(e => e.Creador)
                .WithMany(u => u.ProyectosCreados)
                .HasForeignKey(e => e.CreadorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
