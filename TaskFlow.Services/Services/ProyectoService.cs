using AutoMapper;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Services.Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly IProyectoRepository _proyectoRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IMapper _mapper;

        public ProyectoService(IProyectoRepository proyectoRepo, IUsuarioRepository usuarioRepo, IMapper mapper)
        {
            _proyectoRepo = proyectoRepo;
            _usuarioRepo = usuarioRepo;
            _mapper = mapper;
        }

        public async Task<ProyectoDto?> CrearProyecto(CrearProyectoDto dto)
        {
            var existe = await _proyectoRepo.ExisteNombreParaCreador(dto.Nombre, dto.CreadorId);
            if (existe) throw new Exception("Ya tienes un proyecto con ese nombre");

            var proyecto = _mapper.Map<Proyecto>(dto);
            await _proyectoRepo.Add(proyecto);
            // Agregar al creador como miembro admin
            await _proyectoRepo.AddMember(proyecto.Id, dto.CreadorId, "Admin");
            return _mapper.Map<ProyectoDto>(proyecto);
        }

        public async Task<IEnumerable<ProyectoDto>> GetProyectosByUsuario(int usuarioId)
        {
            // Obtiene los proyectos donde el usuario es miembro o creador
            var proyectos = await _proyectoRepo.GetAll(); // Podría optimizarse con un método en el repositorio
            var proyectosUsuario = proyectos.Where(p => p.CreadorId == usuarioId ||
                p.Miembros.Any(m => m.UsuarioId == usuarioId)).ToList();
            return _mapper.Map<IEnumerable<ProyectoDto>>(proyectosUsuario);
        }

        public async Task<ProyectoDto?> GetProyectoById(int id)
        {
            var proyecto = await _proyectoRepo.GetById(id);
            return proyecto == null ? null : _mapper.Map<ProyectoDto>(proyecto);
        }

        public async Task<ProyectoDto?> ActualizarProyecto(int id, ActualizarProyectoDto dto, int usuarioId)
        {
            var proyecto = await _proyectoRepo.GetById(id);
            if (proyecto == null) throw new Exception("Proyecto no encontrado");
            if (proyecto.CreadorId != usuarioId) throw new Exception("No tienes permiso para modificar este proyecto");

            _mapper.Map(dto, proyecto);
            await _proyectoRepo.Update(proyecto);
            return _mapper.Map<ProyectoDto>(proyecto);
        }

        public async Task<bool> EliminarProyecto(int id, int usuarioId)
        {
            var proyecto = await _proyectoRepo.GetById(id);
            if (proyecto == null) throw new Exception("Proyecto no encontrado");
            if (proyecto.CreadorId != usuarioId) throw new Exception("No tienes permiso para eliminar este proyecto");

            await _proyectoRepo.Delete(id);
            return true;
        }

        public async Task<bool> InvitarMiembro(int proyectoId, InvitarMiembroDto dto, int adminId)
        {
            var proyecto = await _proyectoRepo.GetById(proyectoId);
            if (proyecto == null) throw new Exception("Proyecto no existe");
            if (proyecto.CreadorId != adminId) throw new Exception("Solo el creador puede invitar miembros");

            var usuario = await _usuarioRepo.GetById(dto.UsuarioId);
            if (usuario == null) throw new Exception("Usuario no existe");

            await _usuarioRepo.AgregarMiembro(proyectoId, dto.UsuarioId, dto.Rol ?? "Miembro");
            return true;
        }
    }
}