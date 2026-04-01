using FluentValidation;
using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Validators
{
    public class ActualizarProyectoDtoValidator : AbstractValidator<ActualizarProyectoDto>
    {
        public ActualizarProyectoDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Nombre))
                .MaximumLength(100);
        }
    }
}