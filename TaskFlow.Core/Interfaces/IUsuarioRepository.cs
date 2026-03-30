using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObtenerPorEmail(string email);
        Task AgregarMiembroAProyecto(int proyectoId, int usuarioId);
        Task<bool> EsMiembroDelProyecto(int proyectoId, int usuarioId);
    }
}
