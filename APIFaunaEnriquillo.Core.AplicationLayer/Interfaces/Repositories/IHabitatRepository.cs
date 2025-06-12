using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories
{
    public interface IHabitatRepository: ICommonRepository<Habitat>
    {
        Task<Habitat?> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken);
        Task<Habitat?> FilterByScientificNameAsync(string scrientificName, CancellationToken cancellationToken);

        Task<IEnumerable<Habitat>> GetRecentAsync(CancellationToken cancellationToken);
    }
    
}
