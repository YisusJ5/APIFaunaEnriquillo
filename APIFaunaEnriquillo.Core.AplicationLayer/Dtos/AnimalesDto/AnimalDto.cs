using APIFaunaEnriquillo.Core.DomainLayer.Enums;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Dtos.AnimalesDto
{
    public sealed record AnimalDto(
        Guid? IdAnimal,
        string NombreComun,
        string NombreCientifico,
        Dieta Dieta,
        EstadoDeConservacion EstadoDeConservacion,
        FormaDeReproduccion FormaDeReproduccion,
        TipoDesarrolloEmbrionario TipoDesarrolloEmbrionario,
        EstatusBiogeográficoAnimales EstatusBiogeográfico,
        string Filo,
        string Clase,
        string Orden,
        string Familia,
        string Genero,
        string Especie,
        string SubEspecie,
        string? Observaciones,
        string? DistribucionGeograficaUrl,
        string? ImagenUrl,
        Guid? IdHabitat,
        DateTime? CreatedAt,
        DateTime? UpdatedAt
    );
        
}
