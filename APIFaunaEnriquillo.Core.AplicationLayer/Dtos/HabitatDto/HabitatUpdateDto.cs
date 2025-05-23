using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto
{
    public sealed record HabitatUpdateDto(
         string Nombre,
         Clima Clima,
         string Descripcion,
         string UbicacionGeografica,
         string Imagen
    );
}
