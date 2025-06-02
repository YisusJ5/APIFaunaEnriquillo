using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories;
using APIFaunaEnriquillo.InfraestructureLayer.Persistence.Repository;
using APIFaunaEnriquillo.InfrastructureLayer.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIFaunaEnriquillo.Core.DomainLayer
{
    public static class AddPersistence
    {
        public static void AddPersistenceMethod(this IServiceCollection services, IConfiguration configuration)
        {
            #region Connection
            services.AddDbContext<FaunaDbContext>(b =>
            {
                b.UseSqlServer(configuration.GetConnectionString("FaunaEnriquillo"),
                    c => c.MigrationsAssembly(typeof(FaunaDbContext).Assembly.FullName));
            });
            #endregion
            #region Repositories
            services.AddTransient(typeof(ICommonRepository<>), typeof(CommonRepository<>));
            services.AddTransient<IHabitatRepository, HabitatRepository>();
            services.AddTransient<IAnimalRepository, AnimalRepository>();
            services.AddTransient<IPlantaRepository, PlantaRepository>();
            #endregion
        }
    }
}
