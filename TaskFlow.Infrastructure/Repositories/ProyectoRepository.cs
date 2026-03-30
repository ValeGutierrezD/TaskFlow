using TaskFlow.Core.Entities;
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

    }
}
