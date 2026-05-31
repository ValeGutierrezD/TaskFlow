namespace TaskFlow.Core.QueryFilters
{
    public class ProyectoQueryFilter : PaginationQueryFilter
    {
        public int? UsuarioId { get; set; }
        public string? Nombre { get; set; }
    }
}
