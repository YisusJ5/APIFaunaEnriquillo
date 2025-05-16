using APIFaunaEnriquillo.Core.DomainLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Models
{
    public class Habitat:CreationDate
    {
        public Guid IdHabitat { get; set; }

        public string Nombre { get; set; }

        public string Clima { get; set; }

        public string Descripcion { get; set; }

        public string UbicacionGeografica { get; set; }
    }
}
