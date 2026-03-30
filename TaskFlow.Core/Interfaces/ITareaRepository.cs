using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces
{
    public interface ITareaRepository
    {
        Task<int> Crear(Tarea tarea);
        Task<Tarea> ObtenerPorId(int id);
        Task Actualizar(Tarea tarea); 
        Task<IEnumerable<Tarea>> ObtenerPorProyecto(int proyectoId);
    }
}
