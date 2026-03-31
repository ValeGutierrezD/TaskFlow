using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> Registrar(CrearUsuarioDto usuarioDto);
        Task<UsuarioDto?> Login(LoginDto loginDto);
        Task<bool> InvitarMiembro(int proyectoId, int usuarioId, int adminId);
    }
}
