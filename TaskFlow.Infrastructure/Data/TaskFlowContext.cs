using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Data;

public partial class TaskFlowContext : DbContext
{
    public TaskFlowContext()
    {
    }

    public TaskFlowContext(DbContextOptions<TaskFlowContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    // => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Password123!;database=taskflowdb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.45-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
