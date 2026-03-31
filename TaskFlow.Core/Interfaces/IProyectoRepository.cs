using System.Threading.Tasks;
using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces
{
    public interface IProyectoRepository : IBaseRepository<Proyecto>
    {
        Task<bool> ExisteNombreParaCreador(string nombre, int creadorId);
        Task<Proyecto?> GetByIdWithMembers(int id);
    }
}
