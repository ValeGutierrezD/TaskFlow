using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario?> GetByEmail(string email);
        Task<bool> EsMiembroDelProyecto(int usuarioId, int proyectoId);
        Task AgregarMiembro(int proyectoId, int usuarioId, string rol = "Miembro");
    }
}
