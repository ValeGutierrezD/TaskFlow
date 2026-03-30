using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly TaskFlowContext _context;

        public TareaRepository(TaskFlowContext context)
        {
            _context = context;
        }

        public async Task<int> Crear(Tarea tarea)
        {
            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();
            return tarea.Id;
        }

        public async Task<Tarea> ObtenerPorId(int id)
        {
            return await _context.Tareas.FindAsync(id);
        }

        public async Task Actualizar(Tarea tarea)
        {
            _context.Entry(tarea).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tarea>> ObtenerPorProyecto(int proyectoId)
        {
            return await _context.Tareas
                .Where(t => t.ProyectoId == proyectoId)
                .ToListAsync();
        }
    }
}
