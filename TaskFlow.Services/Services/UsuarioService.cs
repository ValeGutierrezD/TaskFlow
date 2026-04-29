using AutoMapper;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Exceptions;
using TaskFlow.Core.Interfaces;
using TaskFlow.Services.Interfaces;
using System.Net;

namespace TaskFlow.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> Registrar(CrearUsuarioDto dto)
        {
            var existente = await _unitOfWork.UsuarioRepository.GetByEmail(dto.Email);
            if (existente != null) throw new BusinessException("El email ya está registrado", HttpStatusCode.BadRequest);

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = dto.Password // En producción usar hash
            };
            await _unitOfWork.UsuarioRepository.Add(usuario);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> Login(LoginDto dto)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetByEmail(dto.Email);
            if (usuario == null || usuario.PasswordHash != dto.Password)
                throw new BusinessException("Credenciales inválidas", HttpStatusCode.Unauthorized);
            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<bool> InvitarMiembro(int proyectoId, int usuarioId, int adminId)
        {
            var proyecto = await _unitOfWork.ProyectoRepository.GetById(proyectoId);
            if (proyecto == null) throw new BusinessException("Proyecto no existe", HttpStatusCode.NotFound);
            if (proyecto.CreadorId != adminId) throw new BusinessException("Solo el creador puede invitar miembros", HttpStatusCode.Forbidden);

            var usuario = await _unitOfWork.UsuarioRepository.GetById(usuarioId);
            if (usuario == null) throw new BusinessException("Usuario no existe", HttpStatusCode.NotFound);

            await _unitOfWork.UsuarioRepository.AgregarMiembro(proyectoId, usuarioId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}