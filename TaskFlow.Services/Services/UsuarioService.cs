using AutoMapper;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IProyectoRepository _proyectoRepo;
        private readonly IMapper _mapper;
        public UsuarioService(IUsuarioRepository usuarioRepo, IProyectoRepository proyectoRepo, IMapper mapper)
        {
            _usuarioRepo = usuarioRepo;
            _proyectoRepo = proyectoRepo;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> Registrar(CrearUsuarioDto dto)
        {
            var existente = await _usuarioRepo.GetByEmail(dto.Email);
            if (existente != null) throw new Exception("El email ya está registrado");

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = dto.Password // En producción usar hash
            };
            await _usuarioRepo.Add(usuario);
            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> Login(LoginDto dto)
        {
            var usuario = await _usuarioRepo.GetByEmail(dto.Email);
            if (usuario == null || usuario.PasswordHash != dto.Password) // hash comparación
                throw new Exception("Credenciales inválidas");
            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<bool> InvitarMiembro(int proyectoId, int usuarioId, int adminId)
        {
            var proyecto = await _proyectoRepo.GetById(proyectoId);
            if (proyecto == null) throw new Exception("Proyecto no existe");
            if (proyecto.CreadorId != adminId) throw new Exception("Solo el creador puede invitar miembros");

            var usuario = await _usuarioRepo.GetById(usuarioId);
            if (usuario == null) throw new Exception("Usuario no existe");

            await _usuarioRepo.AgregarMiembro(proyectoId, usuarioId);
            return true;
        }
    }
}