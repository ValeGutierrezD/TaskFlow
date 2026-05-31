using AutoMapper;
using System.Net;
using TaskFlow.Core.CustomEntities;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Exceptions;
using TaskFlow.Core.Interfaces;
using TaskFlow.Core.QueryFilters;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Services.Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProyectoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProyectoDto?> CrearProyecto(CrearProyectoDto dto, int usuarioId)
        {
            var existe = await _unitOfWork.ProyectoRepository.ExisteNombreParaCreador(dto.Nombre, usuarioId);
            if (existe) throw new BusinessException("Ya tienes un proyecto con ese nombre");

            var proyecto = _mapper.Map<Proyecto>(dto);
            proyecto.CreadorId = usuarioId;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.ProyectoRepository.Add(proyecto);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.ProyectoRepository.AddMember(proyecto.Id, usuarioId, "Admin");
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ProyectoDto>(proyecto);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<ProyectoDto>> GetProyectosByUsuario(int usuarioId, PaginationQueryFilter pagination)
        {
            var proyectos = await _unitOfWork.ProyectoRepository.GetAll();
            var proyectosUsuario = proyectos.Where(p => p.CreadorId == usuarioId || p.Miembros.Any(m => m.UsuarioId == usuarioId));
            var paged = PagedList<Proyecto>.Create(proyectosUsuario, pagination.PageNumber, pagination.PageSize);
            return _mapper.Map<IEnumerable<ProyectoDto>>(paged);
        }

        public async Task<ProyectoDto?> GetProyectoById(int id)
        {
            var proyecto = await _unitOfWork.ProyectoRepository.GetById(id);
            return proyecto == null ? null : _mapper.Map<ProyectoDto>(proyecto);
        }

        public async Task<ProyectoDto?> ActualizarProyecto(int id, ActualizarProyectoDto dto, int usuarioId)
        {
            var proyecto = await _unitOfWork.ProyectoRepository.GetById(id);
            if (proyecto == null) throw new BusinessException("Proyecto no encontrado", HttpStatusCode.NotFound);
            if (proyecto.CreadorId != usuarioId) throw new BusinessException("No tienes permiso para modificar", HttpStatusCode.Forbidden);

            _mapper.Map(dto, proyecto);
            await _unitOfWork.ProyectoRepository.Update(proyecto);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProyectoDto>(proyecto);
        }

        public async Task<bool> EliminarProyecto(int id, int usuarioId)
        {
            var proyecto = await _unitOfWork.ProyectoRepository.GetById(id);
            if (proyecto == null) throw new BusinessException("Proyecto no encontrado", HttpStatusCode.NotFound);
            if (proyecto.CreadorId != usuarioId) throw new BusinessException("No tienes permiso para eliminar", HttpStatusCode.Forbidden);

            await _unitOfWork.ProyectoRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InvitarMiembro(int proyectoId, InvitarMiembroDto dto, int adminId)
        {
            var proyecto = await _unitOfWork.ProyectoRepository.GetById(proyectoId);
            if (proyecto == null) throw new BusinessException("Proyecto no existe", HttpStatusCode.NotFound);
            if (proyecto.CreadorId != adminId) throw new BusinessException("Solo el creador puede invitar", HttpStatusCode.Forbidden);

            var usuario = await _unitOfWork.UsuarioRepository.GetById(dto.UsuarioId);
            if (usuario == null) throw new BusinessException("Usuario no existe", HttpStatusCode.NotFound);

            await _unitOfWork.UsuarioRepository.AgregarMiembro(proyectoId, dto.UsuarioId, dto.Rol ?? "Miembro");
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarRolMiembro(int proyectoId, int miembroId, string nuevoRol, int adminId)
        {
            var proyecto = await _unitOfWork.ProyectoRepository.GetById(proyectoId);
            if (proyecto == null) throw new BusinessException("Proyecto no existe", HttpStatusCode.NotFound);
            if (proyecto.CreadorId != adminId) throw new BusinessException("Solo el creador puede cambiar roles", HttpStatusCode.Forbidden);

            var miembro = proyecto.Miembros.FirstOrDefault(m => m.UsuarioId == miembroId);
            if (miembro == null) throw new BusinessException("El usuario no es miembro del proyecto", HttpStatusCode.BadRequest);

            miembro.Rol = nuevoRol;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
