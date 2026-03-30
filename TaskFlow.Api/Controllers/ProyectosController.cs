using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Core.Entities;

namespace TaskFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectosController : ControllerBase
    {
        private readonly IProyectoRepository _proyectoRepo;

        public ProyectosController(IProyectoRepository proyectoRepo)
        {
            _proyectoRepo = proyectoRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Proyecto proyecto)
        {
            // Validaciones del Flujo Principal
            if (string.IsNullOrEmpty(proyecto.Nombre))
                return BadRequest("El nombre del proyecto es obligatorio.");

            // Validación Flujo Alternativo A
            var existe = await _proyectoRepo.ExisteNombreParaUsuarioAsync(proyecto.Nombre, proyecto.UsuarioId);
            if (existe)
                return BadRequest("Ya tienes un proyecto con ese nombre.");

            var nuevoProyecto = await _proyectoRepo.CrearAsync(proyecto);
            return CreatedAtAction(nameof(Crear), new { id = nuevoProyecto.Id }, nuevoProyecto);
        }
    }
}
