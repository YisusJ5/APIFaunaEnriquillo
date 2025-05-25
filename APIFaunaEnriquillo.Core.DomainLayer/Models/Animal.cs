using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.DomainLayer.Common;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;

namespace APIFaunaEnriquillo.Core.DomainLayer.Models
{
    public class Animal: CreationDate
    {
        public Guid? IdAnimal { get; set; }
        public string? NombreComun { get; set; }
        public string? NombreCientifico { get; set; }
        public Dieta Dieta { get; set; }
        public EstadoDeConservacion EstadoDeConservacion { get; set; }
        public FormaDeReproduccion FormaDeReproduccion { get; set; }
        public TipoDesarrolloEmbrionario TipoDesarrolloEmbrionario { get; set; }
        public EstatusBiogeográficoAnimales EstatusBiogeográfico { get; set; }
        public string? Filo { get; set; }
        public string? Clase { get; set; }
        public string? Orden { get; set; }
        public string? Familia { get; set; }
        public string? Genero { get; set; }
        public string? Especie { get; set; }
        public string? SubEspecie { get; set; }
        public string? Observaciones { get; set; }
        public string? DistribucionGeograficaUrl { get; set; }
        public string? ImagenUrl { get; set; }

        public Guid? HabitatId { get; set; }
        public Habitat? Habitat { get; set; }


    }
}
