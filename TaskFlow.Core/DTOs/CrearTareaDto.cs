namespace TaskFlow.Core.DTOs
{
    public class CrearTareaDto
    {
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int ProyectoId { get; set; }
        public int? UsuarioAsignadoId { get; set; } // opcional
    }
}
