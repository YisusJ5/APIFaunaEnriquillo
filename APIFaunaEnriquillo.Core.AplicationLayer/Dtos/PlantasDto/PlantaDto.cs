using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Dtos.PlantasDto
{
    public sealed record PlantaDto( 
        Guid IdPlanta,
        string NombreComun,
        string NombreCientifico,
        EstadoDeConservacion EstadoDeConservacion,
        EstatusBiogeograficoPlantas EstatusBiogeografico,
        string Filo,
        string Clase,
        string Orden,
        string Familia,
        string Genero,
        string Especie,
        string SubEspecie,
        string Observaciones,
        string DistribucionGeograficaUrl,
        string ImagenUrl,
        DateTime? CreatedAt,
        DateTime? UpdateAt
    );
}
