using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces
{
    public interface IProyectoRepository
    {
        Task<int> Crear(Proyecto proyecto);
        Task<Proyecto> ObtenerPorId(int id);
    }
}
