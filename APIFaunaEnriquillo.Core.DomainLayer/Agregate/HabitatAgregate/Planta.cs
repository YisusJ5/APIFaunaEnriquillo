using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.DomainLayer.Common;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.PlantObjects;

namespace APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate
{
    public class Planta : CreationDate
    {
        public Guid? IdPlanta { get; set; }
        public NombreComunPlant NombreComun { get; set; }
        public NombreCientificoPlant NombreCientifico { get; set; }
        public EstadoDeConservacion EstadoDeConservacion { get; set; }
        public EstatusBiogeograficoPlantas EstatusBiogeografico { get; set; }
        public FiloPlant Filo { get; set; }
        public ClasePlant Clase { get; set; }
        public OrdenPlant Orden { get; set; }
        public FamiliaPlant Familia { get; set; }
        public GeneroPlant Genero { get; set; }
        public EspeciePlant Especie { get; set; }
        public SubEspeciePlant SubEspecie { get; set; }
        public string? Observaciones { get; set; }
        public string? DistribucionGeograficaUrl { get; set; }
        public string? ImagenUrl { get; set; }
        public Guid? HabitatId { get; set; }
        public Habitat? Habitat { get; set; }

    }
}
