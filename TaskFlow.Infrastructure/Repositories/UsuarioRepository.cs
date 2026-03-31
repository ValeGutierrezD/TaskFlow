using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(TaskFlowContext context) : base(context) { }

        public async Task<Usuario?> GetByEmail(string email)
            => await _entities.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> EsMiembroDelProyecto(int usuarioId, int proyectoId)
            => await _context.ProyectoUsuarios.AnyAsync(pu => pu.UsuarioId == usuarioId && pu.ProyectoId == proyectoId);

        public async Task AgregarMiembro(int proyectoId, int usuarioId, string rol = "Miembro")
        {
            var proyectoUsuario = new ProyectoUsuario { ProyectoId = proyectoId, UsuarioId = usuarioId, Rol = rol };
            await _context.ProyectoUsuarios.AddAsync(proyectoUsuario);
            await _context.SaveChangesAsync();
        }
    }
}