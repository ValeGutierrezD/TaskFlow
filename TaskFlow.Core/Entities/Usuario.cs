
namespace TaskFlow.Core.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; // En texto plano por ahora (luego se hashea)
        public virtual ICollection<Proyecto> ProyectosCreados { get; set; } = new List<Proyecto>();
        public virtual ICollection<ProyectoUsuario> ProyectosMiembro { get; set; } = new List<ProyectoUsuario>();
        public virtual ICollection<Tarea> TareasAsignadas { get; set; } = new List<Tarea>();
    }
}