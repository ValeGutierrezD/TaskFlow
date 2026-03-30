namespace TaskFlow.Core.DTOs
{
    public class CrearProyectoDto
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int CreadorId { get; set; }
    }
}
