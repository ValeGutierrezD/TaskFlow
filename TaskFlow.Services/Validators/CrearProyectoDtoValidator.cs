using FluentValidation;
using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Validators
{
    public class CrearProyectoDtoValidator : AbstractValidator<CrearProyectoDto>
    {
        public CrearProyectoDtoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MinimumLength(3).MaximumLength(100);
            // RuleFor(x => x.CreadorId).GreaterThan(0);
        }
    }
}
