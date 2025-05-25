using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Dtos.PlantasDto
{
    public sealed record PlantaUpdateDto(
       string? NombreComun,
        string? NombreCientifico,
        EstadoDeConservacion EstadoDeConservacion,
        EstatusBiogeograficoPlantas EstatusBiogeografico,
        string? Filo,
        string? Clase,
        string? Orden,
        string? Familia,
        string? Genero,
        string? Especie,
        string? SubEspecie,
        string? Observaciones,
        IFormFile? DistribucionGeografica,
        IFormFile? Imagen
        );
}
