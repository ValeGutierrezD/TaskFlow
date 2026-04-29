using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Infrastructure.Data;

namespace TaskFlow.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskFlowContext _context;
        private readonly IDapperContext _dapper;
        private IDbContextTransaction? _efTransaction;

        private IProyectoRepository? _proyectoRepo;
        private ITareaRepository? _tareaRepo;
        private IUsuarioRepository? _usuarioRepo;
        private IComentarioRepository? _comentarioRepo;

        public UnitOfWork(TaskFlowContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public IProyectoRepository ProyectoRepository => _proyectoRepo ??= new ProyectoRepository(_context);
        public ITareaRepository TareaRepository => _tareaRepo ??= new TareaRepository(_context);
        public IUsuarioRepository UsuarioRepository => _usuarioRepo ??= new UsuarioRepository(_context);
        public IComentarioRepository ComentarioRepository => _comentarioRepo ??= new ComentarioRepository(_context, _dapper);

        public void SaveChanges() => _context.SaveChanges();
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    await _efTransaction.DisposeAsync();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                await _efTransaction.DisposeAsync();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection();
        }

        public void Dispose()
        {
            _efTransaction?.Dispose();
            _context.Dispose();
        }

        public IDbConnection? GetDbConnection() => _context.Database.GetDbConnection();
        public IDbTransaction? GetDbTransaction() => _efTransaction?.GetDbTransaction();
    }
}
