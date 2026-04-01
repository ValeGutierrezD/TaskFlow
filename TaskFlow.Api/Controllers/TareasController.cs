using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Responses;
using TaskFlow.Core.DTOs;
using TaskFlow.Services.Interfaces;
using TaskFlow.Services.Validators;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly ITareaService _tareaService;
        private readonly CrearTareaDtoValidator _crearValidator;
        private readonly AsignarTareaDtoValidator _asignarValidator;
        private readonly ActualizarTareaDtoValidator _actualizarValidator;

        public TareasController(
            ITareaService tareaService,
            CrearTareaDtoValidator crearValidator,
            AsignarTareaDtoValidator asignarValidator,
            ActualizarTareaDtoValidator actualizarValidator)
        {
            _tareaService = tareaService;
            _crearValidator = crearValidator;
            _asignarValidator = asignarValidator;
            _actualizarValidator = actualizarValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CrearTarea(CrearTareaDto dto)
        {
            var validation = await _crearValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validación", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var tarea = await _tareaService.CrearTarea(dto);
                return Ok(new ApiResponse<TareaDto>(tarea!, "Tarea creada con éxito"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }

        [HttpGet("proyecto/{proyectoId}")]
        public async Task<IActionResult> GetTareasByProyecto(int proyectoId)
        {
            var tareas = await _tareaService.GetTareasByProyecto(proyectoId);
            return Ok(new ApiResponse<IEnumerable<TareaDto>>(tareas));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTarea(int id)
        {
            var tarea = await _tareaService.GetTareaById(id);
            if (tarea == null)
                return NotFound(new ApiResponse<object>("Tarea no encontrada", new List<string>()));
            return Ok(new ApiResponse<TareaDto>(tarea));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTarea(int id, ActualizarTareaDto dto, [FromQuery] int usuarioId)
        {
            var validation = await _actualizarValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validación", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var tarea = await _tareaService.ActualizarTarea(id, dto, usuarioId);
                return Ok(new ApiResponse<TareaDto>(tarea!, "Tarea actualizada"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTarea(int id, [FromQuery] int usuarioId)
        {
            try
            {
                await _tareaService.EliminarTarea(id, usuarioId);
                return Ok(new ApiResponse<bool>(true, "Tarea eliminada"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }

        [HttpPut("asignar")]
        public async Task<IActionResult> AsignarTarea(AsignarTareaDto dto)
        {
            var validation = await _asignarValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validación", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var result = await _tareaService.AsignarTarea(dto);
                return Ok(new ApiResponse<bool>(result, "Tarea asignada correctamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromQuery] string nuevoEstado, [FromQuery] int usuarioId)
        {
            try
            {
                var result = await _tareaService.CambiarEstado(id, nuevoEstado, usuarioId);
                return Ok(new ApiResponse<bool>(result, "Estado actualizado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message, new List<string>()));
            }
        }
    }
}