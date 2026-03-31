using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Interfaces
{
    public interface ITareaService
    {
        Task<TareaDto?> CrearTarea(CrearTareaDto dto);
        Task<bool> AsignarTarea(AsignarTareaDto dto);
        Task<bool> CambiarEstado(int tareaId, string nuevoEstado, int usuarioId);
    }
}
