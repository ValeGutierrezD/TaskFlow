namespace TaskFlow.Core.DTOs
{
    public class ComentarioDto
    {
        public int Id { get; set; }
        public int TareaId { get; set; }
        public int UsuarioId { get; set; }
        public string Contenido { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioNombre { get; set; }
    }
}
