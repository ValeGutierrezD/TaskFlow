using TaskFlow.Core.DTOs;
using TaskFlow.Core.QueryFilters;

namespace TaskFlow.Services.Interfaces
{
    public interface ITareaService
    {
        Task<TareaDto?> CrearTarea(CrearTareaDto dto, int usuarioId);
        Task<IEnumerable<TareaDto>> GetTareasByProyecto(int proyectoId, PaginationQueryFilter pagination);
        Task<TareaDto?> GetTareaById(int id);
        Task<TareaDto?> ActualizarTarea(int id, ActualizarTareaDto dto, int usuarioId);
        Task<bool> EliminarTarea(int id, int usuarioId);
        Task<bool> AsignarTarea(AsignarTareaDto dto, int usuarioId);
        Task<bool> CambiarEstado(int tareaId, string nuevoEstado, int usuarioId);
        Task<IEnumerable<TareaDto>> GetTareasFiltradas(TareaQueryFilter filtros);
    }
}
