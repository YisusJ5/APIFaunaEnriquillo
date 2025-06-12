using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories;
using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
using APIFaunaEnriquillo.InfrastructureLayer.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.InfraestructureLayer.Persistence.Repository
{
    public class PlantaRepository:CommonRepository<Planta>, IPlantaRepository
    {
        public PlantaRepository(FaunaDbContext dbcontext) : base(dbcontext) { }

        public Task<Planta?> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken)
        {
            return _dbContext.Set<Planta>().
                AsTracking().
                Where(planta => planta.NombreComun.Value.ToLower().Contains(commonName.ToLower())).
                FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Planta?> FilterByScientificNameAsync(string ScientificName, CancellationToken cancellationToken)
        {
            return _dbContext.Set<Planta>().
                AsTracking().
                Where(planta=> planta.NombreCientifico.Value.ToLower().Contains(ScientificName.ToLower())).
                FirstOrDefaultAsync(cancellationToken);

        }

        public async Task<IEnumerable<Planta>> GetRecentAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Planta>()
           .AsNoTracking()
           .OrderByDescending(r => r.CreatedAt)
           .Take(10)
           .ToListAsync(cancellationToken);
        }

    }
}
