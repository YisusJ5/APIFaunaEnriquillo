using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.AnimalesDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.AplicationLayer.Services;
using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;

namespace APIFaunaEnriquillo.Test
{
    public class AnimalServiceTests
    {
        [Fact]
        public async Task GetById_ReturnsFailure_WhenNotFound()
        {
            var repoMock = new Mock<IAnimalRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Animal)null!);

            var loggerMock = new Mock<ILogger<AnimalDto>>();
            var cloudinaryMock = new Mock<ICloudinaryService>();

            var service = new AnimalService(repoMock.Object, loggerMock.Object, cloudinaryMock.Object);

            var result = await service.GetById(Guid.NewGuid(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task GetById_ReturnsSuccess_WhenFound()
        {
            var animal = new Animal
            {
                IdAnimal = Guid.NewGuid(),
                NombreComun = new NombreComunAnimal("Tigre"),
                NombreCientifico = new NombreCientificoAnimal("Panthera tigris"),
                Dieta = Dieta.Carnivoro,
                EstadoDeConservacion = EstadoDeConservacion.EnPeligro,
                FormaDeReproduccion = FormaDeReproduccion.Sexual,
                TipoDesarrolloEmbrionario = TipoDesarrolloEmbrionario.Viviparo,
                EstatusBiogeográfico = EstatusBiogeográficoAnimales.Nativo,
                Filo = new FiloAnimal("Chordata"),
                Clase = new ClaseAnimal("Mammalia"),
                Orden = new OrdenAnimal("Carnivora"),
                Familia = new FamiliaAnimal("Felidae"),
                Genero = new GeneroAnimal("Panthera"),
                Especie = new EspecieAnimal("tigris"),
                SubEspecie = new SubEspecieAnimal("altaica"),
                Observaciones = "Gran felino asiático",
                DistribucionGeograficaUrl = "http://test.com/distribucion.jpg",
                ImagenUrl = "http://test.com/imagen.jpg",
                HabitatId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var repoMock = new Mock<IAnimalRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(animal);

            var loggerMock = new Mock<ILogger<AnimalDto>>();
            var cloudinaryMock = new Mock<ICloudinaryService>();

            var service = new AnimalService(repoMock.Object, loggerMock.Object, cloudinaryMock.Object);

            var result = await service.GetById(animal.IdAnimal!.Value, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Tigre", result.Value.NombreComun);
        }
    }
}
