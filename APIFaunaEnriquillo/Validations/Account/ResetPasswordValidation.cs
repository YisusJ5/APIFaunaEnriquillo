
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Reset;
using FluentValidation;
namespace APIFaunaEnriquillo.Validations.Account
{
    public class ResetPasswordValidation: AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidation()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido")
                .EmailAddress().WithMessage("El correo electrónico no es válido.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

        }
    }
}
