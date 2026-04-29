using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.Core.Entities
{
    [Table("proyecto_usuarios")]
    public class ProyectoUsuario : BaseEntity
    {
        [Column("proyecto_id")]
        public int ProyectoId { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public string Rol { get; set; } = "Miembro";

        public virtual Proyecto Proyecto { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
