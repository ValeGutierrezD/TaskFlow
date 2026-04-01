using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Responses;
using TaskFlow.Core.DTOs;
using TaskFlow.Services.Interfaces;
using TaskFlow.Services.Validators;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectosController : ControllerBase
    {
        private readonly IProyectoService _proyectoService;
        private readonly CrearProyectoDtoValidator _crearValidator;
        private readonly ActualizarProyectoDtoValidator _actualizarValidator;

        public ProyectosController(
            IProyectoService proyectoService,
            CrearProyectoDtoValidator crearValidator,
            ActualizarProyectoDtoValidator actualizarValidator)
        {
            _proyectoService = proyectoService;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CrearProyecto(CrearProyectoDto dto)
        {
            var validation = await _crearValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validación", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var proyecto = await _proyectoService.CrearProyecto(dto);
                return Ok(new ApiResponse<ProyectoDto>(proyecto!, "Proyecto creado con éxito"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProyectos([FromQuery] int usuarioId)
        {
            var proyectos = await _proyectoService.GetProyectosByUsuario(usuarioId);
            return Ok(new ApiResponse<IEnumerable<ProyectoDto>>(proyectos, "Lista de proyectos"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProyecto(int id)
        {
            var proyecto = await _proyectoService.GetProyectoById(id);
            if (proyecto == null)
                return NotFound(new ApiResponse<object>("Proyecto no encontrado", new List<string>()));
            return Ok(new ApiResponse<ProyectoDto>(proyecto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProyecto(int id, ActualizarProyectoDto dto, [FromQuery] int usuarioId)
        {
            var validation = await _actualizarValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validación", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var proyecto = await _proyectoService.ActualizarProyecto(id, dto, usuarioId);
                return Ok(new ApiResponse<ProyectoDto>(proyecto!, "Proyecto actualizado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProyecto(int id, [FromQuery] int usuarioId)
        {
            try
            {
                await _proyectoService.EliminarProyecto(id, usuarioId);
                return Ok(new ApiResponse<bool>(true, "Proyecto eliminado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }

        [HttpPost("{id}/invitar")]
        public async Task<IActionResult> InvitarMiembro(int id, InvitarMiembroDto dto, [FromQuery] int adminId)
        {
            try
            {
                var result = await _proyectoService.InvitarMiembro(id, dto, adminId);
                return Ok(new ApiResponse<bool>(result, "Miembro invitado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }
    }
}