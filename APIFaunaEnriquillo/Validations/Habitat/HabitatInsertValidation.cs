using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto;
using FluentValidation;

namespace APIFaunaEnriquillo.Validations.Habitat
{
    public class HabitatInsertValidation : AbstractValidator<HabitatInsertDto>
    {
        public HabitatInsertValidation()
        {
            RuleFor(x => x.NombreComun)
            .NotEmpty().WithMessage("El nombre común es requerido.")
            .MinimumLength(3).WithMessage("El nombre común debe tener al menos 3 caracteres.")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
            .MaximumLength(250).WithMessage("El Maximo de caracteres es 250")
            .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
            .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.NombreCientifico)
             .NotEmpty().WithMessage("El nombre científico es requerido.")
             .MinimumLength(4).WithMessage("El nombre científico debe tener al menos 4 caracteres.")
             .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ .-]+$").WithMessage("Solo puede contener letras")
             .MaximumLength(250).WithMessage("El Maximo de caracteres es 250")
             .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.Descripcion)
               .NotEmpty().WithMessage("La Descripción es requerida.")
               .MinimumLength(10).WithMessage("El nombre común debe tener al menos 3 caracteres.")
               .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
               .MaximumLength(500).WithMessage("El Maximo de caracteres es 500")
               .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
               .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
               .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");


            RuleFor(x => x.Clima)
               .NotEmpty().WithMessage("El Clima es requerido.")
               .IsInEnum().WithMessage("Clima no valido.");

            RuleFor(x => x.UbicacionGeografica)
            .NotNull().WithMessage("La Ubicación Geográfica es requerido.");

            RuleFor(x => x.Imagen)
          .NotNull().WithMessage("La Imagen es requerida.");
        }
    }
}
