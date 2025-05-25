using APIFaunaEnriquillo.Core.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repository
{
    public interface IHabitatRepository: ICommonRepository<Habitat>
    {
        Task<Habitat> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken);
        Task<Habitat> FilterByScientificNameAsync(string commonName, CancellationToken cancellationToken);
    }
    
}
