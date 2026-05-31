using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Api.Responses;
using TaskFlow.Core.DTOs;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ComentariosController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;

        public ComentariosController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpPost]
        public async Task<IActionResult> AgregarComentario([FromQuery] int tareaId, [FromBody] string contenido)
        {
            var comentario = await _comentarioService.AgregarComentario(tareaId, GetUserId(), contenido);
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
