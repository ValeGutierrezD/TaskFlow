using FluentValidation;
using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Validators
{
    public class CrearTareaDtoValidator : AbstractValidator<CrearTareaDto>
    {
        public CrearTareaDtoValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().MinimumLength(3).MaximumLength(150);
            RuleFor(x => x.FechaVencimiento).GreaterThan(DateTime.Now);
            RuleFor(x => x.ProyectoId).GreaterThan(0);
        }
    }
}
