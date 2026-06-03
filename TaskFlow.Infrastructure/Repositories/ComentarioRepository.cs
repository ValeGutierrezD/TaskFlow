using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class ComentarioRepository : BaseRepository<Comentario>, IComentarioRepository
    {
        private readonly IDapperContext _dapper;
        public ComentarioRepository(TaskFlowContext context, IDapperContext dapper) : base(context)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<Comentario>> GetByTareaIdAsync(int tareaId)
        {
            return await _entities
                .Include(c => c.Usuario)
                .Where(c => c.TareaId == tareaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comentario>> GetComentariosConUsuarioAsync(int tareaId)
        {
            var sql = @"
                SELECT c.*, u.Nombre as UsuarioNombre
                FROM comentarios c
                INNER JOIN usuarios u ON c.usuario_id = u.Id
                WHERE c.tarea_id = @tareaId
                ORDER BY c.fecha_creacion DESC";
            return await _dapper.QueryAsync<Comentario>(sql, new { tareaId });
        }

    }
}
