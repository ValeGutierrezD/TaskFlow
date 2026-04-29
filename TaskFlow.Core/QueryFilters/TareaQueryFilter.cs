namespace TaskFlow.Core.QueryFilters
{
    public class TareaQueryFilter
    {
        public int? ProyectoId { get; set; }
        public int? UsuarioAsignadoId { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaVencimientoDesde { get; set; }
        public DateTime? FechaVencimientoHasta { get; set; }
    }
}
