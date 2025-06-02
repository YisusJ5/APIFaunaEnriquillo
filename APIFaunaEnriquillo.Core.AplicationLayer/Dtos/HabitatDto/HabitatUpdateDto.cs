using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto
{
    public sealed record HabitatUpdateDto(
        string NombreComun,
         string NombreCientifico,
         Clima Clima,
         string? Descripcion,
         IFormFile? UbicacionGeografica,
         IFormFile? Imagen
    );
}
