using FluentValidation;
using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Validators
{
    public class AsignarTareaDtoValidator : AbstractValidator<AsignarTareaDto>
    {
        public AsignarTareaDtoValidator()
        {
            RuleFor(x => x.TareaId).GreaterThan(0);
            RuleFor(x => x.UsuarioId).GreaterThan(0);
        }
    }
}