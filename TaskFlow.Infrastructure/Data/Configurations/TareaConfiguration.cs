using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Data.Configurations
{
    public class TareaConfiguration :
        IEntityTypeConfiguration<Tarea>
    {
        public void Configure(EntityTypeBuilder<Tarea> entity )
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tareas");

            entity.HasIndex(e => e.ProyectoId, "proyecto_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pendiente'")
                .HasColumnName("estado");
            entity.Property(e => e.ProyectoId).HasColumnName("proyecto_id");
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .HasColumnName("titulo");

            entity.HasOne(d => d.Proyecto).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.ProyectoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tareas_ibfk_1");
        }
    }
}
