using APIFaunaEnriquillo.Core.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repository
{
    public interface IAnimalRepository: ICommonRepository<Animal>
    {
        Task<Animal> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken);
        Task<Animal> FilterByScientificNameAsync(string commonName, CancellationToken cancellationToken);
    }
}
