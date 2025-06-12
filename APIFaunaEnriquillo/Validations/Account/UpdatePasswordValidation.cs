using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account;
using FluentValidation;
namespace APIFaunaEnriquillo.Validations.Account
{
    public class UpdatePasswordValidation: AbstractValidator<UpdateAccountDto>
    {
        public UpdatePasswordValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El primer nombre es requerido")
                .MinimumLength(3).WithMessage("El primer nombre no puede ser menor a 3 caracteres");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido")
                .MinimumLength(3).WithMessage("El apellido no puede ser menor a 3 caracteres");
            
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es requerido")
                .MinimumLength(3).WithMessage("El nombre de usuario no puede ser menor a 3 caracteres");
        }

    }
}
