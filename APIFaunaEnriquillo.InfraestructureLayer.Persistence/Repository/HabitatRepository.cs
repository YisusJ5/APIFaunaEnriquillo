using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories;
using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
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

        public Task<Habitat?> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken)
        {
            return _dbContext.Set<Habitat>().
                AsTracking().
                Where(habitat => habitat.NombreComun.Value.ToLower().Contains(commonName.ToLower())).
                FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Habitat?> FilterByScientificNameAsync(string scientificName, CancellationToken cancellationToken)
        {
            return _dbContext.Set<Habitat>().
            AsTracking().
                Where(habitat => habitat.NombreCientifico.Value.ToLower().Contains(scientificName.ToLower())).
                FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Habitat>> GetRecentAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Habitat>()
           .AsNoTracking()
           .OrderByDescending(r => r.CreatedAt)
           .Take(10)
           .ToListAsync(cancellationToken);
        }

    }
}
