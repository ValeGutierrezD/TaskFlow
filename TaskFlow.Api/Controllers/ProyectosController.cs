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

        public ProyectosController(IProyectoService proyectoService, CrearProyectoDtoValidator crearValidator)
        {
            _proyectoService = proyectoService;
            _crearValidator = crearValidator;
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
    }
}
