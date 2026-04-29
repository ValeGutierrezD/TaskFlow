using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TareaRepository : BaseRepository<Tarea>, ITareaRepository
    {
        public TareaRepository(TaskFlowContext context) : base(context) { }

        public async Task<IEnumerable<Tarea>> GetByProyecto(int proyectoId)
            => await _entities.Where(t => t.ProyectoId == proyectoId).ToListAsync();

        public async Task ActualizarEstado(int tareaId, string nuevoEstado)
        {
            var tarea = await GetById(tareaId);
            if (tarea != null)
            {
                tarea.Estado = nuevoEstado;
                //Update(tarea);
            }
        }
    }
}