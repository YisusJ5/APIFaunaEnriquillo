using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repository;
using APIFaunaEnriquillo.Core.DomainLayer.Models;
using APIFaunaEnriquillo.InfrastructureLayer.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.InfraestructureLayer.Persistence.Repository
{
    public class HabitatRepository : CommonRepository<Habitat>, IHabitatRepository
    {
        public HabitatRepository(FaunaDbContext dbContext): base(dbContext) { }

        public Task<Habitat> FilterHabitatName(string nameHabitat, CancellationToken cancellationToken)
        {
            return _dbContext.Set<Habitat>().
                AsTracking().
                Where(habitat => habitat.Nombre.ToLower().Contains(nameHabitat.ToLower())).
                FirstOrDefaultAsync(cancellationToken);
        }
    }
}
