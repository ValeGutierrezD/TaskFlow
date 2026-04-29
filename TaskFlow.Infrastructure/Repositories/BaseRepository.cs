using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly TaskFlowContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(TaskFlowContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll() => await _entities.ToListAsync();

        public async Task<T?> GetById(int id) => await _entities.FindAsync(id);

        public async Task Add(T entity) => await _entities.AddAsync(entity);

        public Task Update(T entity)
        {
            _entities.Update(entity);
            return Task.CompletedTask;
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            if (entity != null) _entities.Remove(entity);
        }
    }
}
