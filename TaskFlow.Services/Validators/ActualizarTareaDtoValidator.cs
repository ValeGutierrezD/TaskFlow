using FluentValidation;
using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Validators
{
    public class ActualizarTareaDtoValidator : AbstractValidator<ActualizarTareaDto>
    {
        public ActualizarTareaDtoValidator()
        {
            RuleFor(x => x.Titulo)
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Titulo))
                .MaximumLength(150);
            RuleFor(x => x.FechaVencimiento)
                .GreaterThan(DateTime.Now).When(x => x.FechaVencimiento.HasValue);
            RuleFor(x => x.Estado)
                .Must(estado => new[] { "Pendiente", "EnProgreso", "Completada" }.Contains(estado))
                .When(x => !string.IsNullOrEmpty(x.Estado));
        }
    }
}