using AutoMapper;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Services.Services
{
    public class TareaService : ITareaService
    {
        private readonly ITareaRepository _tareaRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IProyectoRepository _proyectoRepo;
        private readonly IMapper _mapper;

        public TareaService(ITareaRepository tareaRepo, IUsuarioRepository usuarioRepo, IProyectoRepository proyectoRepo, IMapper mapper)
        {
            _tareaRepo = tareaRepo;
            _usuarioRepo = usuarioRepo;
            _proyectoRepo = proyectoRepo;
            _mapper = mapper;
        }

        public async Task<TareaDto?> CrearTarea(CrearTareaDto dto)
        {
            var proyecto = await _proyectoRepo.GetById(dto.ProyectoId);
            if (proyecto == null) throw new Exception("Proyecto no existe");

            if (dto.UsuarioAsignadoId.HasValue)
            {
                bool esMiembro = await _usuarioRepo.EsMiembroDelProyecto(dto.UsuarioAsignadoId.Value, dto.ProyectoId);
                if (!esMiembro) throw new Exception("El usuario asignado no es miembro del proyecto");
            }

            var tarea = _mapper.Map<Tarea>(dto);
            tarea.Estado = "Pendiente";
            await _tareaRepo.Add(tarea);
            return _mapper.Map<TareaDto>(tarea);
        }

        public async Task<IEnumerable<TareaDto>> GetTareasByProyecto(int proyectoId)
        {
            var tareas = await _tareaRepo.GetByProyecto(proyectoId);
            return _mapper.Map<IEnumerable<TareaDto>>(tareas);
        }

        public async Task<TareaDto?> GetTareaById(int id)
        {
            var tarea = await _tareaRepo.GetById(id);
            return tarea == null ? null : _mapper.Map<TareaDto>(tarea);
        }

        public async Task<TareaDto?> ActualizarTarea(int id, ActualizarTareaDto dto, int usuarioId)
        {
            var tarea = await _tareaRepo.GetById(id);
            if (tarea == null) throw new Exception("Tarea no encontrada");

            // Verificar que el usuario sea miembro del proyecto (o admin)
            bool esMiembro = await _usuarioRepo.EsMiembroDelProyecto(usuarioId, tarea.ProyectoId);
            if (!esMiembro) throw new Exception("No tienes permiso para modificar esta tarea");

            _mapper.Map(dto, tarea);
            await _tareaRepo.Update(tarea);
            return _mapper.Map<TareaDto>(tarea);
        }

        public async Task<bool> EliminarTarea(int id, int usuarioId)
        {
            var tarea = await _tareaRepo.GetById(id);
            if (tarea == null) throw new Exception("Tarea no encontrada");

            bool esMiembro = await _usuarioRepo.EsMiembroDelProyecto(usuarioId, tarea.ProyectoId);
            if (!esMiembro) throw new Exception("No tienes permiso para eliminar esta tarea");

            await _tareaRepo.Delete(id);
            return true;
        }

        public async Task<bool> AsignarTarea(AsignarTareaDto dto)
        {
            var tarea = await _tareaRepo.GetById(dto.TareaId);
            if (tarea == null) throw new Exception("Tarea no existe");

            bool esMiembro = await _usuarioRepo.EsMiembroDelProyecto(dto.UsuarioId, tarea.ProyectoId);
            if (!esMiembro) throw new Exception("El usuario no pertenece al proyecto");

            tarea.UsuarioAsignadoId = dto.UsuarioId;
            await _tareaRepo.Update(tarea);
            return true;
        }

        public async Task<bool> CambiarEstado(int tareaId, string nuevoEstado, int usuarioId)
        {
            var tarea = await _tareaRepo.GetById(tareaId);
            if (tarea == null) throw new Exception("Tarea no existe");

            bool esMiembro = await _usuarioRepo.EsMiembroDelProyecto(usuarioId, tarea.ProyectoId);
            if (!esMiembro) throw new Exception("No eres miembro del proyecto");

            var estadosValidos = new[] { "Pendiente", "EnProgreso", "Completada" };
            if (!Array.Exists(estadosValidos, e => e == nuevoEstado))
                throw new Exception("Estado no válido");

            // Regla: solo puede avanzar, no retroceder a menos que sea admin
            if (nuevoEstado == "Pendiente" && tarea.Estado != "Pendiente")
                throw new Exception("No se puede retroceder a Pendiente");
            if (nuevoEstado == "EnProgreso" && tarea.Estado == "Completada")
                throw new Exception("No se puede retroceder desde Completada");

            tarea.Estado = nuevoEstado;
            await _tareaRepo.Update(tarea);
            return true;
        }
    }
}