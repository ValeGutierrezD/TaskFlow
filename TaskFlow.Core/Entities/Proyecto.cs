using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.Core.Entities
{
    [Table("proyectos")]
    public class Proyecto : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("creador_id")] 
        public int CreadorId { get; set; }

        [ForeignKey("CreadorId")]
        public virtual Usuario? Creador { get; set; }

        public virtual ICollection<ProyectoUsuario> Miembros { get; set; } = new List<ProyectoUsuario>();
        public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
    }
}