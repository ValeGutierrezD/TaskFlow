using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Data.Configurations
{
    public class ProyectoUsuarioConfiguration : IEntityTypeConfiguration<ProyectoUsuario>
    {
        public void Configure(EntityTypeBuilder<ProyectoUsuario> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("proyecto_usuarios");
            entity.HasIndex(e => new { e.ProyectoId, e.UsuarioId }).IsUnique();
            entity.Property(e => e.Rol).HasMaxLength(20).HasDefaultValue("Miembro");
            entity.HasOne(e => e.Proyecto)
                .WithMany(p => p.Miembros)
                .HasForeignKey(e => e.ProyectoId);
            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.ProyectosMiembro)
                .HasForeignKey(e => e.UsuarioId);
        }
    }
}
