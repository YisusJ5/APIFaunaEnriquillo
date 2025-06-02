using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.DomainLayer.Common;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects;

namespace APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate
{
    public class Animal: CreationDate
    {
        public Guid? IdAnimal { get; set; }
        public NombreComunAnimal NombreComun { get; set; }
        public NombreCientificoAnimal NombreCientifico { get; set; }
        public Dieta Dieta { get; set; }
        public EstadoDeConservacion EstadoDeConservacion { get; set; }
        public FormaDeReproduccion FormaDeReproduccion { get; set; }
        public TipoDesarrolloEmbrionario TipoDesarrolloEmbrionario { get; set; }
        public EstatusBiogeográficoAnimales EstatusBiogeográfico { get; set; }
        public FiloAnimal Filo { get; set; }
        public ClaseAnimal Clase { get; set; }
        public OrdenAnimal Orden { get; set; }
        public FamiliaAnimal Familia { get; set; }
        public GeneroAnimal Genero { get; set; }
        public EspecieAnimal Especie { get; set; }
        public SubEspecieAnimal SubEspecie { get; set; }
        public string? Observaciones { get; set; }
        public string? DistribucionGeograficaUrl { get; set; }
        public string? ImagenUrl { get; set; }

        public Guid? HabitatId { get; set; }
        public Habitat? Habitat { get; set; }


    }
}
