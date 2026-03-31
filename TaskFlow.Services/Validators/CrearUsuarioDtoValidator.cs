using FluentValidation;
using TaskFlow.Core.DTOs;

namespace TaskFlow.Services.Validators
{
    public class CrearUsuarioDtoValidator : AbstractValidator<CrearUsuarioDto>
    {
        public CrearUsuarioDtoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
