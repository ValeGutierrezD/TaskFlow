using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Api.Responses;
using TaskFlow.Core.CustomEntities;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.QueryFilters;
using TaskFlow.Services.Interfaces;
using TaskFlow.Services.Validators;

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
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

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TareaDto>), 200)]
        public async Task<IActionResult> CrearTarea(CrearTareaDto dto)
        {
            var validation = await _crearValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validacion", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var tarea = await _tareaService.CrearTarea(dto, GetUserId());
                return Ok(new ApiResponse<TareaDto>(tarea!, "Tarea creada"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        [HttpGet("proyecto/{proyectoId}")]
        public async Task<IActionResult> GetTareasByProyecto(int proyectoId, [FromQuery] PaginationQueryFilter pagination)
        {
            var tareas = await _tareaService.GetTareasByProyecto(proyectoId, pagination);
            var paginationMeta = new Pagination
            {
                TotalCount = tareas.Count(),
                PageSize = pagination.PageSize,
                CurrentPage = pagination.PageNumber,
                TotalPages = (int)Math.Ceiling(tareas.Count() / (double)pagination.PageSize),
                HasNextPage = pagination.PageNumber < (int)Math.Ceiling(tareas.Count() / (double)pagination.PageSize),
                HasPreviousPage = pagination.PageNumber > 1
            };
            return Ok(new ApiResponse<IEnumerable<TareaDto>>(tareas, "Tareas del proyecto", paginationMeta));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTarea(int id)
        {
            var tarea = await _tareaService.GetTareaById(id);
            if (tarea == null) return NotFound(new ApiResponse<object>("Tarea no encontrada"));
            return Ok(new ApiResponse<TareaDto>(tarea));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTarea(int id, ActualizarTareaDto dto)
        {
            var validation = await _actualizarValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validacion", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var tarea = await _tareaService.ActualizarTarea(id, dto, GetUserId());
                return Ok(new ApiResponse<TareaDto>(tarea!, "Tarea actualizada"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            try
            {
                await _tareaService.EliminarTarea(id, GetUserId());
                return Ok(new ApiResponse<bool>(true, "Tarea eliminada"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        [HttpPut("asignar")]
        public async Task<IActionResult> AsignarTarea(AsignarTareaDto dto)
        {
            var validation = await _asignarValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validacion", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var result = await _tareaService.AsignarTarea(dto, GetUserId());
                return Ok(new ApiResponse<bool>(result, "Tarea asignada"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromQuery] string nuevoEstado)
        {
            try
            {
                var result = await _tareaService.CambiarEstado(id, nuevoEstado, GetUserId());
                return Ok(new ApiResponse<bool>(result, "Estado actualizado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetTareasFiltradas([FromQuery] TareaQueryFilter filtros)
        {
            var tareas = await _tareaService.GetTareasFiltradas(filtros);
            return Ok(new ApiResponse<IEnumerable<TareaDto>>(tareas, "Tareas filtradas"));
        }
    }
}
