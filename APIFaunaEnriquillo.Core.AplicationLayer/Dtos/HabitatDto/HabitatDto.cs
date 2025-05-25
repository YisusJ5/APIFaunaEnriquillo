using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto
{

    public sealed record HabitatDto(
         Guid? IdHabitat,
         string? NombreComun,
         string? NombreCientifico,
         Clima Clima,
         string? Descripcion,
         string? UbicacionGeograficaUrl,
         string? ImagenUrl,
         DateTime? CreatedAt,
         DateTime? UpdatedAt
    );
}
