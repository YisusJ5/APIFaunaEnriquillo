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
    public class AnimalRepository: CommonRepository<Animal>, IAnimalRepository
    {
        public AnimalRepository(FaunaDbContext dbContext): base(dbContext) 
        {
        }

        public async Task<Animal> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Animal>().
                AsTracking().
                Where(animal => animal.NombreComun.ToLower().Contains(commonName.ToLower())).
                FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Animal> FilterByScientificNameAsync(string ScientificName, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Animal>().
                 AsTracking().
                 Where(animal => animal.NombreCientifico.ToLower().Contains(ScientificName.ToLower())).
                 FirstOrDefaultAsync(cancellationToken);
        }
    }
}
