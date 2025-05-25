using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repository;
using APIFaunaEnriquillo.Core.DomainLayer.Models;
using APIFaunaEnriquillo.InfrastructureLayer.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.InfraestructureLayer.Persistence.Repository
{
    public class HabitatRepository : CommonRepository<Habitat>, IHabitatRepository
    {
        public HabitatRepository(FaunaDbContext dbContext): base(dbContext) { }

        public Task<Habitat> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken)
        {
            return _dbContext.Set<Habitat>().
                AsTracking().
                Where(habitat => habitat.NombreComun.ToLower().Contains(commonName.ToLower())).
                FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Habitat> FilterByScientificNameAsync(string scientificName, CancellationToken cancellationToken)
        {
            return _dbContext.Set<Habitat>().
            AsTracking().
                Where(habitat => habitat.NombreCientifico.ToLower().Contains(scientificName.ToLower())).
                FirstOrDefaultAsync(cancellationToken);
        }

    }
}
