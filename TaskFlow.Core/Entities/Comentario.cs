using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.Core.Entities
{
    [Table("comentarios")]
    public class Comentario : BaseEntity
    {
        [Column("tarea_id")]
        public int TareaId { get; set; }
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public string Contenido { get; set; } = null!;
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public virtual Tarea? Tarea { get; set; }
        public virtual Usuario? Usuario { get; set; }
    }
}
