using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Infrastructure.Data;

public class ProyectoRepository : IProyectoRepository
{
    private readonly TaskFlowContext _context;

    public ProyectoRepository(TaskFlowContext context)
    {
        _context = context;
    }

    public async Task<Proyecto> CrearAsync(Proyecto proyecto)
    {
        _context.Proyectos.Add(proyecto);
        await _context.SaveChangesAsync();
        return proyecto;
    }

    public async Task<bool> ExisteNombreParaUsuarioAsync(string nombre, int usuarioId)
    {
        return await _context.Proyectos
            .AnyAsync(p => p.Nombre == nombre && p.UsuarioId == usuarioId);
    }
}
