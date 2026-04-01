using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Interfaces
{
    public interface ITareaService
    {
        Task<TareaDto?> CrearTarea(CrearTareaDto dto);
        Task<IEnumerable<TareaDto>> GetTareasByProyecto(int proyectoId);
        Task<TareaDto?> GetTareaById(int id);
        Task<TareaDto?> ActualizarTarea(int id, ActualizarTareaDto dto, int usuarioId);
        Task<bool> EliminarTarea(int id, int usuarioId);
        Task<bool> AsignarTarea(AsignarTareaDto dto);
        Task<bool> CambiarEstado(int tareaId, string nuevoEstado, int usuarioId);
    }
}