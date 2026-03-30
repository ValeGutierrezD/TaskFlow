namespace TaskFlow.Core.Entities;

public partial class Tarea
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Estado { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public int? UsuarioAsignadoId { get; set; }

    public int? ProyectoId { get; set; }

    public virtual Proyecto? Proyecto { get; set; }
}
