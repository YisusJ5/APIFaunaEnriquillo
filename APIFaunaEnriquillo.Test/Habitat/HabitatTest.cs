using Moq;
using Microsoft.Extensions.Logging;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.AplicationLayer.Services;
using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
namespace APIFaunaEnriquillo.Test
{
    public class HabitatServiceTests
    {
        [Fact]
        public async Task GetById_ReturnsFailure_WhenNotFound()
        {
            var repoMock = new Mock<IHabitatRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Habitat)null!);

            var loggerMock = new Mock<ILogger<HabitatDto>>();
            var cloudinaryMock = new Mock<ICloudinaryService>();

            var service = new HabitatService(repoMock.Object, loggerMock.Object, cloudinaryMock.Object);

            var result = await service.GetById(Guid.NewGuid(), CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task GetById_ReturnsSuccess_WhenFound()
        {
            
            var habitat = new Habitat
            {
                IdHabitat = Guid.NewGuid(),
                NombreComun = new APIFaunaEnriquillo.Core.DomainLayer.Value_object.HabitatObjects.NombreComunHabitat("Bosque"),
                NombreCientifico = new APIFaunaEnriquillo.Core.DomainLayer.Value_object.HabitatObjects.NombreCientificoHabitat("Forestus"),
                Clima = Clima.Templado,
                Descripcion = "Un bosque templado",
                UbicacionGeograficaUrl = "http://test.com/ubicacion.jpg",
                ImagenUrl = "http://test.com/imagen.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var repoMock = new Mock<IHabitatRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(habitat);

            var loggerMock = new Mock<ILogger<HabitatDto>>();
            var cloudinaryMock = new Mock<ICloudinaryService>();

            var service = new HabitatService(repoMock.Object, loggerMock.Object, cloudinaryMock.Object);

            var result = await service.GetById(habitat.IdHabitat!.Value, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("Bosque", result.Value.NombreComun);
        }
    }
}
