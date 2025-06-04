using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Auth;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Forgot;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace APIFaunaEnriquillo.Validations.Account
{
    public class ForgotPasswordValidation: AbstractValidator<ForgotRequest>
    {
        
        public ForgotPasswordValidation()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email requerido")
                .EmailAddress().WithMessage("Email no válido");

        }

    }
}
