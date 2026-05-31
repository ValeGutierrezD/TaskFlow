using System.Net;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Exceptions;
using TaskFlow.Core.Interfaces;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Services.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public UsuarioService(IUnitOfWork unitOfWork, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public async Task<UsuarioDto?> Registrar(CrearUsuarioDto dto)
        {
            var existente = await _unitOfWork.UsuarioRepository.GetByEmail(dto.Email);
            if (existente != null)
                throw new BusinessException("El email ya esta registrado", HttpStatusCode.BadRequest);

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = _passwordService.Hash(dto.Password)
            };

            await _unitOfWork.UsuarioRepository.Add(usuario);
            await _unitOfWork.SaveChangesAsync();
            return new UsuarioDto { Id = usuario.Id, Nombre = usuario.Nombre, Email = usuario.Email, PasswordHash = usuario.PasswordHash };
        }

        public async Task<UsuarioDto?> GetByEmail(string email)
        {
            var user = await _unitOfWork.UsuarioRepository.GetByEmail(email);
            if (user == null) return null;
            return new UsuarioDto { Id = user.Id, Nombre = user.Nombre, Email = user.Email, PasswordHash = user.PasswordHash };
        }
    }
}
