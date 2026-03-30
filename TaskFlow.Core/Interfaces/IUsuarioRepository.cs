using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces;

public interface IUsuarioRepository
{
    // Métodos CRUD básicos
    Task<Usuario?> GetByIdAsync(int id);
    Task AddAsync(Usuario usuario);
    Task SaveChangesAsync();

    // Métodos de lógica de negocio 
    Task<Usuario?> ObtenerPorEmail(string email);
    Task AgregarMiembroAProyecto(int proyectoId, int usuarioId);
    Task<bool> EsMiembroDelProyecto(int proyectoId, int usuarioId);
}
