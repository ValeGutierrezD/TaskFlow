using AutoMapper;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Exceptions;
using TaskFlow.Core.Interfaces;
using TaskFlow.Services.Interfaces;
using System.Net;

namespace TaskFlow.Services.Services
{
    public class TareaService : ITareaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TareaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TareaDto?> CrearTarea(CrearTareaDto dto)
        {
            var proyecto = await _unitOfWork.ProyectoRepository.GetById(dto.ProyectoId);
            if (proyecto == null) throw new BusinessException("Proyecto no existe", HttpStatusCode.NotFound);

            if (dto.UsuarioAsignadoId.HasValue)
            {
                bool esMiembro = await _unitOfWork.UsuarioRepository.EsMiembroDelProyecto(dto.UsuarioAsignadoId.Value, dto.ProyectoId);
                if (!esMiembro) throw new BusinessException("El usuario asignado no es miembro del proyecto", HttpStatusCode.BadRequest);
            }

            var tarea = _mapper.Map<Tarea>(dto);
            tarea.Estado = "Pendiente";
            await _unitOfWork.TareaRepository.Add(tarea);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TareaDto>(tarea);
        }

        public async Task<IEnumerable<TareaDto>> GetTareasByProyecto(int proyectoId)
        {
            var tareas = await _unitOfWork.TareaRepository.GetByProyecto(proyectoId);
            return _mapper.Map<IEnumerable<TareaDto>>(tareas);
        }

        public async Task<TareaDto?> GetTareaById(int id)
        {
            var tarea = await _unitOfWork.TareaRepository.GetById(id);
            return tarea == null ? null : _mapper.Map<TareaDto>(tarea);
        }

        public async Task<TareaDto?> ActualizarTarea(int id, ActualizarTareaDto dto, int usuarioId)
        {
            var tarea = await _unitOfWork.TareaRepository.GetById(id);
            if (tarea == null) throw new BusinessException("Tarea no encontrada", HttpStatusCode.NotFound);
            bool esMiembro = await _unitOfWork.UsuarioRepository.EsMiembroDelProyecto(usuarioId, tarea.ProyectoId);
            if (!esMiembro) throw new BusinessException("No tienes permiso para modificar esta tarea", HttpStatusCode.Forbidden);

            _mapper.Map(dto, tarea);
            await _unitOfWork.TareaRepository.Update(tarea);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TareaDto>(tarea);
        }

        public async Task<bool> EliminarTarea(int id, int usuarioId)
        {
            var tarea = await _unitOfWork.TareaRepository.GetById(id);
            if (tarea == null) throw new BusinessException("Tarea no encontrada", HttpStatusCode.NotFound);
            bool esMiembro = await _unitOfWork.UsuarioRepository.EsMiembroDelProyecto(usuarioId, tarea.ProyectoId);
            if (!esMiembro) throw new BusinessException("No tienes permiso para eliminar esta tarea", HttpStatusCode.Forbidden);

            await _unitOfWork.TareaRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AsignarTarea(AsignarTareaDto dto)
        {
            var tarea = await _unitOfWork.TareaRepository.GetById(dto.TareaId);
            if (tarea == null) throw new BusinessException("Tarea no existe", HttpStatusCode.NotFound);
            bool esMiembro = await _unitOfWork.UsuarioRepository.EsMiembroDelProyecto(dto.UsuarioId, tarea.ProyectoId);
            if (!esMiembro) throw new BusinessException("El usuario no pertenece al proyecto", HttpStatusCode.BadRequest);

            tarea.UsuarioAsignadoId = dto.UsuarioId;
            await _unitOfWork.TareaRepository.Update(tarea);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarEstado(int tareaId, string nuevoEstado, int usuarioId)
        {
            var tarea = await _unitOfWork.TareaRepository.GetById(tareaId);
            if (tarea == null) throw new BusinessException("Tarea no existe", HttpStatusCode.NotFound);
            bool esMiembro = await _unitOfWork.UsuarioRepository.EsMiembroDelProyecto(usuarioId, tarea.ProyectoId);
            if (!esMiembro) throw new BusinessException("No eres miembro del proyecto", HttpStatusCode.Forbidden);

            var estadosValidos = new[] { "Pendiente", "EnProgreso", "Completada" };
            if (!Array.Exists(estadosValidos, e => e == nuevoEstado))
                throw new BusinessException("Estado no válido", HttpStatusCode.BadRequest);

            tarea.Estado = nuevoEstado;
            await _unitOfWork.TareaRepository.Update(tarea);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}