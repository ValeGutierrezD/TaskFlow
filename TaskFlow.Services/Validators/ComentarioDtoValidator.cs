using FluentValidation;
using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Validators
{
    public class ComentarioDtoValidator : AbstractValidator<ComentarioDto>
    {
        public ComentarioDtoValidator()
        {
            RuleFor(x => x.Contenido).NotEmpty().MinimumLength(3).MaximumLength(500);
            RuleFor(x => x.TareaId).GreaterThan(0);
            RuleFor(x => x.UsuarioId).GreaterThan(0);
        }
    }
}