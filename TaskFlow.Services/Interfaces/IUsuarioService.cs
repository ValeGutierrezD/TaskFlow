using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> Registrar(CrearUsuarioDto dto);
        Task<UsuarioDto?> GetByEmail(string email);
    }
}
