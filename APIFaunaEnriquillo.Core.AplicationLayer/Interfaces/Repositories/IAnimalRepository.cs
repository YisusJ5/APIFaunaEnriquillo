using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories
{
    public interface IAnimalRepository: ICommonRepository<Animal>
    {
        Task<Animal?> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken);
        Task<Animal?> FilterByScientificNameAsync(string scientificName, CancellationToken cancellationToken);
    }
}
