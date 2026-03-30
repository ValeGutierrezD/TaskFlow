using TaskFlow.Core.Entities;

public interface IProyectoRepository
{
    Task<Proyecto> CrearAsync(Proyecto proyecto);
    Task<bool> ExisteNombreParaUsuarioAsync(string nombre, int usuarioId);
}
