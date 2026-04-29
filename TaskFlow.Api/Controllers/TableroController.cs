using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Responses;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableroController : ControllerBase
    {
        private readonly IDapperContext _dapper;

        public TableroController(IDapperContext dapper)
        {
            _dapper = dapper;
        }

        [HttpGet("proyecto/{proyectoId}")]
        public async Task<IActionResult> GetTablero(int proyectoId, [FromQuery] int? usuarioId = null)
        {
            var sql = @"
                SELECT t.Id, t.Titulo, t.Descripcion, t.Estado, t.FechaVencimiento, 
                       p.Nombre as ProyectoNombre, u.Nombre as UsuarioAsignadoNombre
                FROM tareas t
                INNER JOIN proyectos p ON t.proyecto_id = p.Id
                LEFT JOIN usuarios u ON t.usuario_assigned_id = u.Id
                WHERE p.Id = @ProyectoId
            ";
            if (usuarioId.HasValue)
                sql += " AND (t.usuario_assigned_id = @UsuarioId OR p.creador_id = @UsuarioId)";
            sql += " ORDER BY t.FechaVencimiento";

            var result = await _dapper.QueryAsync<dynamic>(sql, new { ProyectoId = proyectoId, UsuarioId = usuarioId });
            return Ok(new ApiResponse<object>(result, "Tablero del proyecto"));
        }
    }
}
