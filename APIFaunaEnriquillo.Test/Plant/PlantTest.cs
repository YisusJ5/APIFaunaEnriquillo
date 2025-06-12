using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.PlantasDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.AplicationLayer.Services;
using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.PlantObjects;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;

namespace APIFaunaEnriquillo.Test.Plant
{
    public class PlantServiceTests
    {
        [Fact]
        public async Task GetById_ReturnsFailure_WhenNotFound()
        {
            var repoMock = new Mock<IPlantaRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Planta)null!);

            var loggerMock = new Mock<ILogger<PlantaDto>>();
            var cloudinaryMock = new Mock<ICloudinaryService>();

            var service = new PlantaService(repoMock.Object, loggerMock.Object, cloudinaryMock.Object);

            var result = await service.GetById(Guid.NewGuid(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task GetById_ReturnsSuccess_WhenFound()
        {
            var planta = new Planta
            {
                IdPlanta = Guid.NewGuid(),
                NombreComun = new NombreComunPlant("Cactus"),
                NombreCientifico = new NombreCientificoPlant("Cactaceae"),
                EstadoDeConservacion = EstadoDeConservacion.PreocupacionMenor,
                EstatusBiogeografico = EstatusBiogeograficoPlantas.Nativo,
                Filo = new FiloPlant("Tracheophyta"),
                Clase = new ClasePlant("Magnoliopsida"),
                Orden = new OrdenPlant("Caryophyllales"),
                Genero = new GeneroPlant("Carnegiea"),
                Familia = new FamiliaPlant("Cactaceae"),
                Especie = new EspeciePlant("gigantea"),
                SubEspecie = new SubEspeciePlant("SubGigantea"),
                Observaciones = "Planta de zonas áridas",
                DistribucionGeograficaUrl = "http://test.com/distribucion.jpg",
                ImagenUrl = "http://test.com/imagen.jpg",
                HabitatId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var repoMock = new Mock<IPlantaRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(planta);

            var loggerMock = new Mock<ILogger<PlantaDto>>();
            var cloudinaryMock = new Mock<ICloudinaryService>();

            var service = new PlantaService(repoMock.Object, loggerMock.Object, cloudinaryMock.Object);

            var result = await service.GetById(planta.IdPlanta!.Value, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Cactus", result.Value.NombreComun);
        }
    }
}
