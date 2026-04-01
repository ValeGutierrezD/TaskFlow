using System;

namespace TaskFlow.Core.DTOs
{
    public class ActualizarTareaDto
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string? Estado { get; set; }
        public int? UsuarioAsignadoId { get; set; }
    }
}