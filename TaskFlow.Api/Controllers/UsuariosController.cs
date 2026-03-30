using Microsoft.AspNetCore.Mvc;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // La ruta será: api/usuarios
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepository _repository;

    public UsuariosController(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    // GET: api/usuarios/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);

        if (usuario == null)
        {
            return NotFound(new { message = $"Usuario con ID {id} no encontrado." });
        }

        return Ok(usuario);
    }

    // POST: api/usuarios
    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {
        // Validación básica de negocio: ¿Ya existe el email?
        var existente = await _repository.ObtenerPorEmail(usuario.Email);
        if (existente != null)
        {
            return BadRequest(new { message = "El correo electrónico ya está registrado." });
        }

        await _repository.AddAsync(usuario);
        await _repository.SaveChangesAsync();

        // Retorna un 201 Created y la ubicación del nuevo recurso
        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
    }

    // GET: api/usuarios/buscar?email=test@test.com
    [HttpGet("buscar")]
    public async Task<ActionResult<Usuario>> GetByEmail([FromQuery] string email)
    {
        var usuario = await _repository.ObtenerPorEmail(email);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }
}
