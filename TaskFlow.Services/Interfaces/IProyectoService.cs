using TaskFlow.Core.DTOs;
using TaskFlow.Core.QueryFilters;

namespace TaskFlow.Services.Interfaces
{
    public interface IProyectoService
    {
        Task<ProyectoDto?> CrearProyecto(CrearProyectoDto dto, int usuarioId);
        Task<IEnumerable<ProyectoDto>> GetProyectosByUsuario(int usuarioId, PaginationQueryFilter pagination);
        Task<ProyectoDto?> GetProyectoById(int id);
        Task<ProyectoDto?> ActualizarProyecto(int id, ActualizarProyectoDto dto, int usuarioId);
        Task<bool> EliminarProyecto(int id, int usuarioId);
        Task<bool> InvitarMiembro(int proyectoId, InvitarMiembroDto dto, int adminId);
        Task<bool> CambiarRolMiembro(int proyectoId, int miembroId, string nuevoRol, int adminId);
    }
}
