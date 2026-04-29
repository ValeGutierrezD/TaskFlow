using System.Data;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUsuarioRepository UsuarioRepository { get; }   
        IProyectoRepository ProyectoRepository { get; }
        ITareaRepository TareaRepository { get; }
        IComentarioRepository ComentarioRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();
    }
}
