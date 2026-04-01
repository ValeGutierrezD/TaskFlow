using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Interfaces
{
    public interface IProyectoService
    {
        Task<ProyectoDto?> CrearProyecto(CrearProyectoDto dto);
        Task<IEnumerable<ProyectoDto>> GetProyectosByUsuario(int usuarioId);
        Task<ProyectoDto?> GetProyectoById(int id);
        Task<ProyectoDto?> ActualizarProyecto(int id, ActualizarProyectoDto dto, int usuarioId);
        Task<bool> EliminarProyecto(int id, int usuarioId);
        Task<bool> InvitarMiembro(int proyectoId, InvitarMiembroDto dto, int adminId);
    }
}