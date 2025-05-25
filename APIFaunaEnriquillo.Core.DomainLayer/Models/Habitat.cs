using APIFaunaEnriquillo.Core.DomainLayer.Common;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Models
{
    public class Habitat: CreationDate
    {
        public Guid? IdHabitat { get; set; }

        public string? NombreComun { get; set; }
        public string? NombreCientifico { get; set; }

        public Clima Clima { get; set; }

        public string? Descripcion { get; set; }

        public string? UbicacionGeograficaUrl { get; set; }
        public string? ImagenUrl { get; set; }

        public ICollection<Planta>? Plantas { get; set; }
        public ICollection<Animal>? Animales { get; set; }
    }
}
