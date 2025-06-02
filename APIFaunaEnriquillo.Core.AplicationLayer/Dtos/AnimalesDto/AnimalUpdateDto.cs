using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Dtos.AnimalesDto
{
    public sealed record AnimalUpdateDto(
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
        IFormFile? DistribucionGeografica,
        IFormFile? Imagen
        );
}
