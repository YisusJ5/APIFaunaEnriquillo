


using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Register;
using FluentValidation;

namespace APIFaunaEnriquillo.Validations.Account
{
    public class RegisterValidation: AbstractValidator<RegisterRequest>
    {

        public RegisterValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido")
                .EmailAddress().WithMessage("El correo electrónico no es válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El primer nombre es requerido")
                .MinimumLength(3).WithMessage("El primer nombre no puede ser menor a 3 caracteres");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido")
                .MinimumLength(3).WithMessage("El apellido no puede ser menor a 3 caracteres");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("El telefono es requerido")
                .MinimumLength(11).WithMessage("El telefono no puede ser menor a 11 caracteres");
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es requerido")
                .MinimumLength(3).WithMessage("El nombre de usuario no puede ser menor a 3 caracteres");
        }

    }
}
