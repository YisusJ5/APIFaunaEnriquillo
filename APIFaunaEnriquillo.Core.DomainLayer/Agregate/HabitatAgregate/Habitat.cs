using APIFaunaEnriquillo.Core.DomainLayer.Common;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.HabitatObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate
{
    public class Habitat: CreationDate, IdAgregateRoot
    {
        public Guid? IdHabitat { get; set; }

        public NombreComunHabitat NombreComun { get; set; }
        public NombreCientificoHabitat NombreCientifico { get; set; }

        public Clima Clima { get; set; }

        public string? Descripcion { get; set; }

        public string? UbicacionGeograficaUrl { get; set; }
        public string? ImagenUrl { get; set; }

        public ICollection<Planta>? Plantas { get; set; }
        public ICollection<Animal>? Animales { get; set; }
    }
}
