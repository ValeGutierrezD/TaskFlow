using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Interfaces
{
    public interface IComentarioService
    {
        Task<ComentarioDto> AgregarComentario(int tareaId, int usuarioId, string contenido);
        Task<IEnumerable<ComentarioDto>> GetComentariosByTarea(int tareaId);
    }
}
