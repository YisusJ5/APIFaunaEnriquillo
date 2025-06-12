


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
                .EmailAddress().WithMessage("El correo electrónico no es válido.")
                .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
                .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.");


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula.")
                .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una minúscula.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El primer nombre es requerido")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
                .MinimumLength(3).WithMessage("El primer nombre no puede ser menor a 3 caracteres")
                .MaximumLength(50).WithMessage("El Maximo de caracteres es 50")
                .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
                .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
                .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido")
                .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
                .MinimumLength(3).WithMessage("El apellido no puede ser menor a 3 caracteres")
                .MaximumLength(50).WithMessage("El Maximo de caracteres es 50")
                .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
                .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
                .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("El telefono es requerido")
                .Matches(@"^\+?[0-9]+$").WithMessage("El teléfono solo puede contener números y, opcionalmente, el símbolo + al inicio.")
                .MinimumLength(11).WithMessage("El telefono no puede ser menor a 11 caracteres")
                .MaximumLength(11).WithMessage("El Maximo de caracteres es 11")
                .Must(phone => !phone.Contains(" ")).WithMessage("El teléfono no puede contener espacios");



            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es requerido")
                .MinimumLength(4).WithMessage("El nombre de usuario no puede ser menor a 4 caracteres")
                .MaximumLength(20).WithMessage("El Maximo de caracteres es 20")
                .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");
        }

    }
}
