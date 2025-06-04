using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto;
using FluentValidation;

namespace APIFaunaEnriquillo.Validations.Habitat
{
    public class HabitatInsertDtoValidation : AbstractValidator<HabitatInsertDto>
    {
        public HabitatInsertDtoValidation()
        {
            RuleFor(x => x.NombreComun)
            .NotEmpty().WithMessage("El nombre común es requerido.")
            .MinimumLength(3).WithMessage("El nombre común debe tener al menos 3 caracteres.")
            .MaximumLength(250).WithMessage("El Maximo de caracteres es 250");

            RuleFor(x => x.NombreCientifico)
             .NotEmpty().WithMessage("El nombre científico es requerido.")
             .MinimumLength(4).WithMessage("El nombre científico debe tener al menos 4 caracteres.")
             .MaximumLength(250).WithMessage("El Maximo de caracteres es 250");

            RuleFor(x => x.Descripcion)
               .NotEmpty().WithMessage("La Descripción es requerida.")
               .MinimumLength(3).WithMessage("El nombre común debe tener al menos 3 caracteres.")
               .MaximumLength(500).WithMessage("El Maximo de caracteres es 500");   

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
