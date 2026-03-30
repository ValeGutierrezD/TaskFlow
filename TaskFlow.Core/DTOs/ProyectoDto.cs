using System;

namespace TaskFlow.Core.DTOs
{
    public class ProyectoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CreadorId { get; set; }
    }
}
