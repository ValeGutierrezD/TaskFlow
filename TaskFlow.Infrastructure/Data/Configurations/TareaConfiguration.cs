using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Data.Configurations
{
    public class TareaConfiguration : IEntityTypeConfiguration<Tarea>
    {
        public void Configure(EntityTypeBuilder<Tarea> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("tareas");
            entity.Property(e => e.Titulo).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Estado).HasMaxLength(50).HasDefaultValue("Pendiente");
            entity.HasOne(e => e.Proyecto)
                .WithMany(p => p.Tareas)
                .HasForeignKey(e => e.ProyectoId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.UsuarioAsignado)
                .WithMany(u => u.TareasAsignadas)
                .HasForeignKey(e => e.UsuarioAsignadoId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
