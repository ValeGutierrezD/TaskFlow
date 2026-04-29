using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Responses;
using TaskFlow.Core.DTOs;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentariosController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;

        public ComentariosController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        [HttpPost]
        public async Task<IActionResult> AgregarComentario([FromQuery] int tareaId, [FromQuery] int usuarioId, [FromBody] string contenido)
        {
            var comentario = await _comentarioService.AgregarComentario(tareaId, usuarioId, contenido);
            return Ok(new ApiResponse<ComentarioDto>(comentario, "Comentario agregado"));
        }

        [HttpGet("tarea/{tareaId}")]
        public async Task<IActionResult> GetComentariosByTarea(int tareaId)
        {
            var comentarios = await _comentarioService.GetComentariosByTarea(tareaId);
            return Ok(new ApiResponse<IEnumerable<ComentarioDto>>(comentarios));
        }
    }
}
