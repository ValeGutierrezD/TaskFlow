namespace TaskFlow.Core.DTOs
{
    public class InvitarMiembroDto
    {
        public int UsuarioId { get; set; }
        public string? Rol { get; set; } = "Miembro";
    }
}