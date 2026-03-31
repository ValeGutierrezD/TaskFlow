using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Interfaces
{
    public interface IProyectoService
    {
        Task<ProyectoDto?> CrearProyecto(CrearProyectoDto dto);
    }
}
