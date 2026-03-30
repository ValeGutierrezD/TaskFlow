using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly TaskFlowContext _context;

    public UsuarioRepository(TaskFlowContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByIdAsync(int id) =>
        await _context.Usuarios.FindAsync(id);

    public async Task AddAsync(Usuario usuario) =>
        await _context.AddAsync(usuario);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task<Usuario?> ObtenerPorEmail(string email) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AgregarMiembroAProyecto(int proyectoId, int usuarioId)
    {
        var proyecto = await _context.Proyectos.FindAsync(proyectoId);
        if (proyecto != null)
        {
            proyecto.UsuarioId = usuarioId; // Si es 1 a muchos
        }
    }

    public async Task<bool> EsMiembroDelProyecto(int proyectoId, int usuarioId) =>
        await _context.Proyectos.AnyAsync(p => p.Id == proyectoId && p.UsuarioId == usuarioId);
}