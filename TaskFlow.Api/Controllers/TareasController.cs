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

        public TareasController(ITareaService tareaService, CrearTareaDtoValidator crearValidator, AsignarTareaDtoValidator asignarValidator)
        {
            _tareaService = tareaService;
            _crearValidator = crearValidator;
            _asignarValidator = asignarValidator;
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
