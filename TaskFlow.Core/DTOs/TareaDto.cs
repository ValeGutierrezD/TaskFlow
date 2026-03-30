using System;

namespace TaskFlow.Core.DTOs
{
    public class TareaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = null!;
        public DateTime FechaVencimiento { get; set; }
        public int ProyectoId { get; set; }
        public int? UsuarioAsignadoId { get; set; }
    }
}
