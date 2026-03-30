using System;
namespace TaskFlow.Core.Entities
{
    public class Tarea : BaseEntity
    {
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, EnProgreso, Completada
        public DateTime FechaVencimiento { get; set; }
        public int ProyectoId { get; set; }
        public int? UsuarioAsignadoId { get; set; }
        public virtual Proyecto Proyecto { get; set; } = null!;
        public virtual Usuario? UsuarioAsignado { get; set; }
    }
}
