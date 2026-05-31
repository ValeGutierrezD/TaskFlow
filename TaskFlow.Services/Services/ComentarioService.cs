using AutoMapper;
using System.Net;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Exceptions;
using TaskFlow.Core.Interfaces;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Services.Services
{
    public class ComentarioService : IComentarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ComentarioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ComentarioDto> AgregarComentario(int tareaId, int usuarioId, string contenido)
        {
            var tarea = await _unitOfWork.TareaRepository.GetById(tareaId);
            if (tarea == null)
                throw new BusinessException("Tarea no existe", HttpStatusCode.NotFound);

            var esMiembro = await _unitOfWork.UsuarioRepository.EsMiembroDelProyecto(usuarioId, tarea.ProyectoId);
            if (!esMiembro)
                throw new BusinessException("No eres miembro del proyecto", HttpStatusCode.Forbidden);

            var comentario = new Comentario
            {
                TareaId = tareaId,
                UsuarioId = usuarioId,
                Contenido = contenido,
                FechaCreacion = DateTime.Now
            };

            await _unitOfWork.ComentarioRepository.Add(comentario);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ComentarioDto>(comentario);
        }

        public async Task<IEnumerable<ComentarioDto>> GetComentariosByTarea(int tareaId)
        {
            var comentarios = await _unitOfWork.ComentarioRepository.GetByTareaIdAsync(tareaId);
            return _mapper.Map<IEnumerable<ComentarioDto>>(comentarios);
        }
    }
}
