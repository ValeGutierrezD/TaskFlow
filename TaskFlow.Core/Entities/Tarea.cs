using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.Core.Entities
{
    [Table("tareas")]
    public class Tarea : BaseEntity
    {
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = "Pendiente";

        [Column("fecha_vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [Column("proyecto_id")]
        public int ProyectoId { get; set; }

        [Column("usuario_assigned_id")] 
        public int? UsuarioAsignadoId { get; set; }

        [ForeignKey("ProyectoId")]
        public virtual Proyecto? Proyecto { get; set; }

        [ForeignKey("UsuarioAsignadoId")]
        public virtual Usuario? UsuarioAsignado { get; set; }
    }
}