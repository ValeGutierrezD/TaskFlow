namespace TaskFlow.Core.Entities
{
    public class ProyectoUsuario : BaseEntity
    {
        public int ProyectoId { get; set; }
        public int UsuarioId { get; set; }
        public string Rol { get; set; } = "Miembro"; // Admin, Miembro
        public virtual Proyecto Proyecto { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
