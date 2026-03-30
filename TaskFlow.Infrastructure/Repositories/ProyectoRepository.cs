using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class ProyectoRepository
    {
        private readonly TaskFlowContext _context;
        public ProyectoRepository(TaskFlowContext contex)
        {
            _context = contex;
        }

        public async Task<int> Crear(Proyecto proyecto)
        {
            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();
            return proyecto.Id;
        }

        public async Task<Proyecto> ObtenerPorId(int id)
        {
            return await _context.Proyectos.FindAsync(id);
        }

    }
}
