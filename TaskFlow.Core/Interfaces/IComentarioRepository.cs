using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces
{
    public interface IComentarioRepository : IBaseRepository<Comentario>
    {
        Task<IEnumerable<Comentario>> GetByTareaIdAsync(int tareaId);
        Task<IEnumerable<Comentario>> GetComentariosConUsuarioAsync(int tareaId);
    }
}
