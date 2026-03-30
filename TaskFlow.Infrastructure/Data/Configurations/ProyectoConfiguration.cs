using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Data.Configurations
{
    public class ProyectoConfiguration :
        IEntityTypeConfiguration<Proyecto>
    {
        public void Configure(EntityTypeBuilder<Proyecto> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("proyectos");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Proyectos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("proyectos_ibfk_1");
        }
    }
}
