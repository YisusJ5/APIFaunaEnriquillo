using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Auth;
using FluentValidation;
namespace APIFaunaEnriquillo.Validations.Account
{
    public class AuthValidation: AbstractValidator<AuthRequest>
    {
        public AuthValidation()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("El email es requerido")
               .EmailAddress().WithMessage("El correo electrónico no es válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
