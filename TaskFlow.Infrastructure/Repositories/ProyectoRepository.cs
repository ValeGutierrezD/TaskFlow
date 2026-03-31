using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;
namespace TaskFlow.Infrastructure.Repositories
{
    public class ProyectoRepository : BaseRepository<Proyecto>, IProyectoRepository
    {
        public ProyectoRepository(TaskFlowContext context) : base(context) { }

        public async Task<bool> ExisteNombreParaCreador(string nombre, int creadorId)
            => await _entities.AnyAsync(p => p.Nombre == nombre && p.CreadorId == creadorId);

        public async Task<Proyecto?> GetByIdWithMembers(int id)
            => await _entities.Include(p => p.Miembros).ThenInclude(pu => pu.Usuario).FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddMember(int proyectoId, int usuarioId, string rol)
        {
            var pu = new ProyectoUsuario { ProyectoId = proyectoId, UsuarioId = usuarioId, Rol = rol };
            await _context.ProyectoUsuarios.AddAsync(pu);
            await _context.SaveChangesAsync();
        }
    }
}
