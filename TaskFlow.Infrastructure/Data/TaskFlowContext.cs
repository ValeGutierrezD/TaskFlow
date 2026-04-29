using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Data
{
    public class TaskFlowContext : DbContext
    {
        public TaskFlowContext(DbContextOptions<TaskFlowContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<ProyectoUsuario> ProyectoUsuarios { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
