using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.AnimalesDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.AplicationLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace APIFaunaEnriquillo.Core.AplicationLayer
{
    public static class AddAplication
    {
        public static IServiceCollection AddAplicationLayer(this IServiceCollection services) 
        {

            services.AddScoped<IAnimalService, AnimalService>();
            services.AddScoped<IHabitatService, HabitatService>();
            services.AddScoped<IPlantaService, PlantaService>();

            return services;
        }


    }
}
