using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Responses;
using TaskFlow.Core.DTOs;
using TaskFlow.Services.Interfaces;
using TaskFlow.Services.Validators;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly CrearUsuarioDtoValidator _crearValidator;
        private readonly LoginDtoValidator _loginValidator;

        public UsuariosController(IUsuarioService usuarioService, CrearUsuarioDtoValidator crearValidator, LoginDtoValidator loginValidator)
        {
            _usuarioService = usuarioService;
            _crearValidator = crearValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar(CrearUsuarioDto dto)
        {
            var validation = await _crearValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validación", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var usuario = await _usuarioService.Registrar(dto);
                return Ok(new ApiResponse<UsuarioDto>(usuario!, "Usuario registrado con éxito"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var validation = await _loginValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validación", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var usuario = await _usuarioService.Login(dto);
                return Ok(new ApiResponse<UsuarioDto>(usuario!, "Login exitoso"));
            }
            catch (Exception ex)
            {
                return Unauthorized(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }
    }
}