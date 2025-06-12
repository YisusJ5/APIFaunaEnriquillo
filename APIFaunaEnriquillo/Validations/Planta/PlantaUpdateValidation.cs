using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.PlantasDto;
using FluentValidation;

namespace APIFaunaEnriquillo.Validations.Planta
{
    public class PlantaUpdateValidation : AbstractValidator<PlantaUpdateDto>
    {
        public PlantaUpdateValidation()
        {
            RuleFor(x => x.NombreComun)
              .NotEmpty().WithMessage("El nombre común es requerido.")
              .MinimumLength(3).WithMessage("El nombre común debe tener al menos 3 caracteres.")
              .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
              .MaximumLength(250).WithMessage("El Maximo de caracteres son de 250")
              .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
              .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
              .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.NombreCientifico)
             .NotEmpty().WithMessage("El nombre científico es requerido.")
             .MinimumLength(4).WithMessage("El nombre científico debe tener al menos 4 caracteres.")
             .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ .-]+$").WithMessage("Solo puede contener letras")
             .MaximumLength(250).WithMessage("El Maximo de caracteres son de 250")
             .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
              .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.EstadoDeConservacion)
                .NotEmpty().WithMessage("El estado de conservación es requerido.")
                .IsInEnum().WithMessage("Estado de Conservación no valido.");

            RuleFor(x => x.EstatusBiogeografico)
             .NotEmpty().WithMessage("El Estatus Biogeográfico es requerido.")
             .IsInEnum().WithMessage("Estatus Biogeográfico no valido");

            RuleFor(x => x.Filo)
             .NotEmpty().WithMessage("El Filo es requerido.")
             .MinimumLength(4).WithMessage("El Filo debe tener al menos 4 caracteres.")
             .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
             .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200")
             .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");


            RuleFor(x => x.Clase)
             .NotEmpty().WithMessage("La Clase es requerida.")
             .MinimumLength(4).WithMessage("El nombre científico debe tener al menos 4 caracteres.")
             .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
             .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200")
             .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.Orden)
            .NotEmpty().WithMessage("El Orden es requerido.")
            .MinimumLength(3).WithMessage("El Orden debe tener al menos 3 caracteres.")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
            .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200")
            .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
            .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
            .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.Familia)
             .NotEmpty().WithMessage("La Familia es requerido.")
             .MinimumLength(4).WithMessage("La Familia debe tener al menos 4 caracteres.")
             .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
             .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200")
             .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.Genero)
            .NotEmpty().WithMessage("El Genero es requerido.")
            .MinimumLength(4).WithMessage("El Genero debe tener al menos 4 caracteres.")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
            .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200")
            .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
            .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
            .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.Especie)
             .NotEmpty().WithMessage("La Especie es requerida.")
             .MinimumLength(4).WithMessage("La Especie debe tener al menos 4 caracteres.")
             .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ .-]+$").WithMessage("Solo puede contener letras")
             .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200")
             .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.SubEspecie)
            .NotEmpty().WithMessage("La SubEspecie es requerido.")
            .MinimumLength(3).WithMessage("La SubEspecie debe tener al menos 3 caracteres.")
            .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ .-]+$").WithMessage("Solo puede contener letras")
            .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200")
            .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
            .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.Observaciones)
             .NotEmpty().WithMessage("Las Observaciones son requeridas.")
             .MinimumLength(50).WithMessage("Las Observaciones deben tener al menos 50 caracteres.")
             .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("Solo puede contener letras")
             .MaximumLength(800).WithMessage("El Maximo de caracteres son de 800")
             .Must(name => !name.StartsWith(" ") && !name.EndsWith(" ")).WithMessage("No puede iniciar ni terminar con espacio.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"\s{2,}")).WithMessage("No puede contener espacios múltiples seguidos.")
             .Must(name => !System.Text.RegularExpressions.Regex.IsMatch(name, @"(.)\1{3,}")).WithMessage("No se permite repetir una letra más de tres veces seguidas.");

            RuleFor(x => x.DistribucionGeografica)
            .NotNull().WithMessage("La Distribución Geográfica es requerido.");

            RuleFor(x => x.Imagen)
            .NotNull().WithMessage("La Imagen es requerida.");
            RuleFor(x => x.DistribucionGeografica)
            .NotNull().WithMessage("La Distribución Geográfica es requerido.");

            RuleFor(x => x.Imagen)
            .NotNull().WithMessage("La Imagen es requerida.");
        }
    }
}
