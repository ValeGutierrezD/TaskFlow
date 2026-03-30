using System;
using System.Collections.Generic;

namespace TaskFlow.Core.Entities
{
    public class Proyecto : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public int CreadorId { get; set; }
        public virtual Usuario Creador { get; set; } = null!;
        public virtual ICollection<ProyectoUsuario> Miembros { get; set; } = new List<ProyectoUsuario>();
        public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
    }
}