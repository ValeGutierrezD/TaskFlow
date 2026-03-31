using System.Collections.Generic;
using System.Threading.Tasks;
using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces
{
    public interface ITareaRepository : IBaseRepository<Tarea>
    {
        Task<IEnumerable<Tarea>> GetByProyecto(int proyectoId);
        Task ActualizarEstado(int tareaId, string nuevoEstado);
    }
}
