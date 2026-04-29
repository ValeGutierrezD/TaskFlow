namespace TaskFlow.Infrastructure.Queries
{
    public static class QueriesTablero
    {
        public static string TareasConProyectoYUsuarios = @"
            SELECT t.Id, t.Titulo, t.Estado, t.FechaVencimiento, 
                   p.Nombre as ProyectoNombre, u.Nombre as UsuarioAsignadoNombre
            FROM tareas t
            INNER JOIN proyectos p ON t.proyecto_id = p.Id
            LEFT JOIN usuarios u ON t.usuario_assigned_id = u.Id
            WHERE p.Id = @ProyectoId
            ORDER BY t.FechaVencimiento";
    }
}
