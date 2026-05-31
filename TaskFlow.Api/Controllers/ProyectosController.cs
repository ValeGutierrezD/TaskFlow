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

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        /// <summary>
        /// Crear nuevo proyecto
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ProyectoDto>), 200)]
        public async Task<IActionResult> CrearProyecto(CrearProyectoDto dto)
        {
            var validation = await _crearValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validacion", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var proyecto = await _proyectoService.CrearProyecto(dto, GetUserId());
                return Ok(new ApiResponse<ProyectoDto>(proyecto!, "Proyecto creado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        /// <summary>
        /// Obtener proyectos del usuario autenticado
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProyectoDto>>), 200)]
        public async Task<IActionResult> GetProyectos([FromQuery] PaginationQueryFilter pagination)
        {
            var proyectos = await _proyectoService.GetProyectosByUsuario(GetUserId(), pagination);
            var paginationMeta = new Pagination
            {
                TotalCount = proyectos.Count(),
                PageSize = pagination.PageSize,
                CurrentPage = pagination.PageNumber,
                TotalPages = (int)Math.Ceiling(proyectos.Count() / (double)pagination.PageSize),
                HasNextPage = pagination.PageNumber < (int)Math.Ceiling(proyectos.Count() / (double)pagination.PageSize),
                HasPreviousPage = pagination.PageNumber > 1
            };
            return Ok(new ApiResponse<IEnumerable<ProyectoDto>>(proyectos, "Proyectos recuperados", paginationMeta));
        }

        /// <summary>
        /// Obtener proyecto por Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProyecto(int id)
        {
            var proyecto = await _proyectoService.GetProyectoById(id);
            if (proyecto == null)
                return NotFound(new ApiResponse<object>("Proyecto no encontrado"));
            return Ok(new ApiResponse<ProyectoDto>(proyecto));
        }

        /// <summary>
        /// Actualizar proyecto
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProyecto(int id, ActualizarProyectoDto dto)
        {
            var validation = await _actualizarValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(new ApiResponse<object>("Error de validacion", validation.Errors.Select(e => e.ErrorMessage).ToList()));

            try
            {
                var proyecto = await _proyectoService.ActualizarProyecto(id, dto, GetUserId());
                return Ok(new ApiResponse<ProyectoDto>(proyecto!, "Proyecto actualizado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        /// <summary>
        /// Eliminar proyecto
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProyecto(int id)
        {
            try
            {
                await _proyectoService.EliminarProyecto(id, GetUserId());
                return Ok(new ApiResponse<bool>(true, "Proyecto eliminado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        /// <summary>
        /// Invitar miembro al proyecto
        /// </summary>
        [HttpPost("{id}/invitar")]
        public async Task<IActionResult> InvitarMiembro(int id, InvitarMiembroDto dto)
        {
            try
            {
                var result = await _proyectoService.InvitarMiembro(id, dto, GetUserId());
                return Ok(new ApiResponse<bool>(result, "Miembro invitado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }

        /// <summary>
        /// Cambiar rol de un miembro del proyecto
        /// </summary>
        [HttpPut("{id}/miembros/{miembroId}/rol")]
        public async Task<IActionResult> CambiarRolMiembro(int id, int miembroId, [FromBody] string nuevoRol)
        {
            try
            {
                var result = await _proyectoService.CambiarRolMiembro(id, miembroId, nuevoRol, GetUserId());
                return Ok(new ApiResponse<bool>(result, "Rol actualizado"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(ex.Message));
            }
        }
    }
}
