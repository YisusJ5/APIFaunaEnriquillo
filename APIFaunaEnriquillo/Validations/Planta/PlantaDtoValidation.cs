using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.PlantasDto;
using FluentValidation;

namespace APIFaunaEnriquillo.Validations.Planta
{
    public class PlantaDtoValidation : AbstractValidator<PlantaDto>
    {
        public PlantaDtoValidation() 
        {
            RuleFor(x => x.NombreComun)
             .NotEmpty().WithMessage("El nombre común es requerido.")
             .MinimumLength(3).WithMessage("El nombre común debe tener al menos 3 caracteres.")
             .MaximumLength(250).WithMessage("El Maximo de caracteres son de 250");

            RuleFor(x => x.NombreCientifico)
             .NotEmpty().WithMessage("El nombre científico es requerido.")
             .MinimumLength(4).WithMessage("El nombre científico debe tener al menos 4 caracteres.")
             .MaximumLength(250).WithMessage("El Maximo de caracteres son de 250");

            RuleFor(x => x.EstadoDeConservacion)
                .NotEmpty().WithMessage("El estado de conservación es requerido.")
                .IsInEnum().WithMessage("Estado de Conservación no valido.");

            RuleFor(x => x.EstatusBiogeografico)
             .NotEmpty().WithMessage("El Estatus Biogeográfico es requerido.")
             .IsInEnum().WithMessage("Estatus Biogeográfico no valido");

            RuleFor(x => x.Filo)
             .NotEmpty().WithMessage("El Filo es requerido.")
             .MinimumLength(4).WithMessage("El Filo debe tener al menos 4 caracteres.")
             .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200");

            RuleFor(x => x.Clase)
             .NotEmpty().WithMessage("La Clase es requerida.")
             .MinimumLength(4).WithMessage("El nombre científico debe tener al menos 4 caracteres.")
             .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200");

            RuleFor(x => x.Orden)
            .NotEmpty().WithMessage("El Orden es requerido.")
            .MinimumLength(3).WithMessage("El Orden debe tener al menos 3 caracteres.")
            .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200");

            RuleFor(x => x.Familia)
             .NotEmpty().WithMessage("La Familia es requerido.")
             .MinimumLength(4).WithMessage("La Familia debe tener al menos 4 caracteres.")
             .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200");

            RuleFor(x => x.Genero)
            .NotEmpty().WithMessage("El Genero es requerido.")
            .MinimumLength(4).WithMessage("El Genero debe tener al menos 4 caracteres.")
            .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200");

            RuleFor(x => x.Especie)
             .NotEmpty().WithMessage("La Especie es requerida.")
             .MinimumLength(4).WithMessage("La Especie debe tener al menos 4 caracteres.")
             .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200");

            RuleFor(x => x.SubEspecie)
            .NotEmpty().WithMessage("La SubEspecie es requerido.")
            .MinimumLength(3).WithMessage("La SubEspecie debe tener al menos 3 caracteres.")
            .MaximumLength(200).WithMessage("El Maximo de caracteres son de 200");

            RuleFor(x => x.Observaciones)
             .NotEmpty().WithMessage("Las Observaciones son requeridas.")
             .MinimumLength(50).WithMessage("Las Observaciones deben tener al menos 50 caracteres.")
             .MaximumLength(800).WithMessage("El Maximo de caracteres son de 800");

            RuleFor(x => x.DistribucionGeograficaUrl)
            .NotEmpty().WithMessage("La Distribución Geográfica es requerido.");

            RuleFor(x => x.IdHabitat)
             .NotNull().WithMessage("El Habitat es requerida.");

            RuleFor(x => x.ImagenUrl)
            .NotEmpty().WithMessage("La Imagen es requerida.");
        }
    }
}
