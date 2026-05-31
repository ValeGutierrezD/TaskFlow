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

        public UsuariosController(IUsuarioService usuarioService, CrearUsuarioDtoValidator crearValidator)
        {
            _usuarioService = usuarioService;
            _crearValidator = crearValidator;
        }

        /// <summary>
        /// Registrar un nuevo usuario
        /// </summary>
        [HttpPost("registro")]
        public async Task<IActionResult> Registrar(CrearUsuarioDto dto)
        {
            var validation = await _crearValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validacion", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var usuario = await _usuarioService.Registrar(dto);
                return Ok(new ApiResponse<UsuarioDto>(usuario!, "Usuario registrado con exito"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }
    }
}
