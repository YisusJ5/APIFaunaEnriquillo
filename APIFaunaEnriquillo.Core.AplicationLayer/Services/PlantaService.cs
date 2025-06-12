using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.AnimalesDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.PlantasDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.AplicationLayer.Pagination;
using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.Core.DomainLayer.Utils;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.AnimalObjects;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.PlantObjects;
using Microsoft.Extensions.Logging;
using static APIFaunaEnriquillo.Core.DomainLayer.Utils.Result;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Services
{
    public class PlantaService : IPlantaService
    {
        private readonly IPlantaRepository _repository;
        private readonly ILogger<PlantaDto> _logger;
        private readonly ICloudinaryService _cloudinary;

        public PlantaService(IPlantaRepository repository, ILogger<PlantaDto>logger, ICloudinaryService cloudinary)
        {
            _repository = repository;
            _logger = logger;
            _cloudinary = cloudinary;
            
        }

        public async Task<ResultT<PageResult<PlantaDto>>> GetPageResult(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogError("Invalid page parameters: pageNumber and pageSize must be geater than 0. ");
                return ResultT<PageResult<PlantaDto>>.Failure(
                    Error.Failure("400", "Invalid pagination parameters. Both pageNumber and pageSize must be greater than 0.")
                    );
            }

            var entityWithNumber = await _repository.GetPageResultAsync(pageNumber, pageSize, cancellationToken);
            var dto = entityWithNumber.Items.Select(x => new PlantaDto(
                   IdPlanta: x.IdPlanta,
                   NombreComun: x.NombreComun.Value,
                   NombreCientifico: x.NombreCientifico.Value,
                   EstadoDeConservacion: x.EstadoDeConservacion,
                   EstatusBiogeografico: x.EstatusBiogeografico,
                   Filo: x.Filo.Value,
                   Clase: x.Clase.Value,
                   Orden: x.Orden.Value,
                   Familia: x.Familia.Value,
                   Genero: x.Genero.Value,
                   Especie: x.Especie.Value,
                   SubEspecie: x.SubEspecie.Value,
                   Observaciones: x.Observaciones,
                   DistribucionGeograficaUrl: x.DistribucionGeograficaUrl,
                   ImagenUrl: x.ImagenUrl,
                   IdHabitat: x.HabitatId,
                   CreatedAt: x.CreatedAt,
                   UpdatedAt: x.UpdatedAt
                 )).ToList();

            if (!dto.Any())
            {
                _logger.LogError("No register found");
                return ResultT<PageResult<PlantaDto>>.Failure(
                    Error.Failure("404", "The list is empty")
                );
            }

            PageResult<PlantaDto> pageResult = new()
            {
                TotalItems = entityWithNumber.TotalItems,
                CurrentPage = entityWithNumber.CurrentPage,
                TotalPages = entityWithNumber.TotalPages,
                Items = dto,

            };

            _logger.LogInformation("Successfully retrieved {count} Planta. Page{CurrentPage} of {TotalPages} ", dto.Count, pageResult.CurrentPage, pageResult.TotalPages);
            return ResultT<PageResult<PlantaDto>>.Success(pageResult);

        }
        public async Task<ResultT<PlantaDto>> GetById(Guid Id, CancellationToken cancellationToken)
        {
            var planta = await _repository.GetByIdAsync(Id, cancellationToken);
            if (planta == null)
            {

                _logger.LogError("No register planta found");

                return ResultT<PlantaDto>.Failure(Error.Failure("404", "The register Planta could not be found "));
            }

            PlantaDto plantaDto = new(

                   IdPlanta: planta.IdPlanta,
                   NombreComun: planta.NombreComun.Value,
                   NombreCientifico: planta.NombreCientifico.Value,
                   EstadoDeConservacion: planta.EstadoDeConservacion,
                   EstatusBiogeografico: planta.EstatusBiogeografico,
                   Filo: planta.Filo.Value,
                   Clase: planta.Clase.Value,
                   Orden: planta.Orden.Value,
                   Familia: planta.Familia.Value,
                   Genero: planta.Genero.Value,
                   Especie: planta.Especie.Value,
                   SubEspecie: planta.SubEspecie.Value,
                   Observaciones: planta.Observaciones,
                   DistribucionGeograficaUrl: planta.DistribucionGeograficaUrl,
                   ImagenUrl: planta.ImagenUrl,
                   IdHabitat: planta.HabitatId,
                   CreatedAt: planta.CreatedAt,
                   UpdatedAt: planta.UpdatedAt
                );

            _logger.LogInformation("Planta with ID {IdPlanta} was successfully retrieved ", Id);
            return ResultT<PlantaDto>.Success(plantaDto);
        }

        public async Task<ResultT<IEnumerable<PlantaDto>>> GetRecentAsync(CancellationToken cancellationToken)
        {
            var GetRecent = await _repository.GetRecentAsync(cancellationToken);
            if (GetRecent == null || !GetRecent.Any())
            {
                _logger.LogError("No recent registered plant found.");

                return ResultT<IEnumerable<PlantaDto>>.Failure(Error.NotFound("404", "The list is empty"));
            }

            IEnumerable<PlantaDto> plantDto = GetRecent.Select(x => new PlantaDto
            (
                 IdPlanta: x.IdPlanta,
             NombreComun: x.NombreComun.Value,
             NombreCientifico: x.NombreCientifico.Value,
             EstadoDeConservacion: x.EstadoDeConservacion,
             EstatusBiogeografico: x.EstatusBiogeografico,
            Filo: x.Filo.Value,
            Clase: x.Clase.Value,
             Orden: x.Orden.Value,
             Genero: x.Genero.Value,
             Familia: x.Familia.Value,
             Especie: x.Especie.Value,
             SubEspecie: x.SubEspecie.Value,
             Observaciones: x.Observaciones,
             DistribucionGeograficaUrl: x.DistribucionGeograficaUrl,
             IdHabitat: x.HabitatId,
             ImagenUrl: x.ImagenUrl,
             CreatedAt: x.CreatedAt,
             UpdatedAt: x.UpdatedAt
            ));

            _logger.LogInformation("Successfully retrieved {Count} recent plants.", plantDto.Count());

            return ResultT<IEnumerable<PlantaDto>>.Success(plantDto);
        }
        public async Task<ResultT<PlantaDto>> CreateAsync(PlantaInsertDto EntityInsertDto, CancellationToken cancellationToken)
        {
            if (EntityInsertDto == null)
            {
                _logger.LogError("Invalid parameter: PlantaInsertDto ");
                return ResultT<PlantaDto>.Failure(Error.Failure("404", "The register Planta could not be found "));

            }
            var existCommonName = await _repository.ValidateAsync(h => h.NombreComun.Value == EntityInsertDto.NombreComun);

            if (existCommonName)
            {
                _logger.LogError("Planta registration failed: A planta with the name '{NombreComun}' already exists", EntityInsertDto.NombreComun);
                return ResultT<PlantaDto>.Failure(
                    Error.Failure("400", $"A Planta with the Common name '{EntityInsertDto.NombreComun} already exists'. Please check it out ")

                    );
            }

            var existsScientificName = await _repository.ValidateAsync(h => h.NombreCientifico.Value == EntityInsertDto.NombreCientifico);

            if (existsScientificName)
            {
                _logger.LogError("Planta registration failed: A Planta with the name '{NombreCientifico}' already exists", EntityInsertDto.NombreCientifico);
                return ResultT<PlantaDto>.Failure(
                    Error.Failure("400", $"A Planta with the Scientific name '{EntityInsertDto.NombreCientifico} already exists'. Please check it out ")

                    );
            }
            string? Imagen = null;
            if (EntityInsertDto.Imagen != null)
            {
                using var stream = EntityInsertDto.Imagen.OpenReadStream();
                Imagen = await _cloudinary.UploadImageAsync(
                    stream,
                    EntityInsertDto.Imagen.FileName,
                    cancellationToken
                    );
            }

            string? DistribucionGeografica = null;
            if (EntityInsertDto.DistribucionGeografica != null)
            {
                using var stream = EntityInsertDto.DistribucionGeografica.OpenReadStream();
                DistribucionGeografica = await _cloudinary.UploadImageAsync(
                    stream,
                    EntityInsertDto.DistribucionGeografica.FileName,
                    cancellationToken
                    );
            }


            Planta PlantaI = new()
            {
                NombreComun = new NombreComunPlant (EntityInsertDto.NombreComun),
                NombreCientifico = new NombreCientificoPlant(EntityInsertDto.NombreCientifico),
                EstadoDeConservacion = EntityInsertDto.EstadoDeConservacion,
                EstatusBiogeografico = EntityInsertDto.EstatusBiogeografico,
                Filo = new FiloPlant(EntityInsertDto.Filo),
                Clase = new ClasePlant(EntityInsertDto.Clase),
                Orden = new OrdenPlant(EntityInsertDto.Orden),
                Familia = new FamiliaPlant(EntityInsertDto.Familia),
                Genero = new GeneroPlant(EntityInsertDto.Genero),
                Especie = new EspeciePlant(EntityInsertDto.Especie),
                SubEspecie = new SubEspeciePlant(EntityInsertDto.SubEspecie),
                Observaciones = EntityInsertDto.Observaciones,
                DistribucionGeograficaUrl = DistribucionGeografica,
                HabitatId = EntityInsertDto.IdHabitat,
                ImagenUrl = Imagen,
                CreatedAt = DateTime.Now

            };


            await _repository.InsertAsync(PlantaI, cancellationToken);


            PlantaDto plantaDto = new(
                   IdPlanta: PlantaI.IdPlanta,
                   NombreComun: PlantaI.NombreComun.Value,
                   NombreCientifico: PlantaI.NombreCientifico.Value,
                   EstadoDeConservacion: PlantaI.EstadoDeConservacion,
                   EstatusBiogeografico: PlantaI.EstatusBiogeografico,
                   Filo: PlantaI.Filo.Value,
                   Clase: PlantaI.Clase.Value,
                   Orden: PlantaI.Orden.Value,
                   Familia: PlantaI.Familia.Value,
                   Genero: PlantaI.Genero.Value,
                   Especie: PlantaI.Especie.Value,
                   SubEspecie: PlantaI.SubEspecie.Value,
                   Observaciones: PlantaI.Observaciones,
                   DistribucionGeograficaUrl: DistribucionGeografica,
                   ImagenUrl: Imagen,
                   IdHabitat: PlantaI.HabitatId,
                   CreatedAt: PlantaI.CreatedAt,
                   UpdatedAt: PlantaI.UpdatedAt
            );

            _logger.LogInformation("Successfully retrieved planta details.Id: {IdPlanta}, Common name: {NombreComun}, Scientific name: { NombreCientifico}, CanservationStatus: {EstadoDeConservacion}", plantaDto.IdPlanta, plantaDto.NombreComun, plantaDto.NombreCientifico, plantaDto.EstadoDeConservacion);

            return ResultT<PlantaDto>.Success(plantaDto);
        }

        public async Task<ResultT<PlantaDto>> UpdateAsync(Guid Id, PlantaUpdateDto Entity, CancellationToken cancellationToken)
        {
            var Plant = await _repository.GetByIdAsync(Id, cancellationToken);

            if (Plant == null)
            {
                _logger.LogError("no registered plant found ");
                return ResultT<PlantaDto>.Failure(Error.Failure("404", $"{Id} is already registered"));

            }
            var existCommonName = await _repository.ValidateAsync(h => h.NombreComun.Value == Entity.NombreComun);

            if (existCommonName)
            {
                _logger.LogError("Plant registration failed: A plant with the name '{NombreComun}' already exists", Entity.NombreComun);
                return ResultT<PlantaDto>.Failure(
                    Error.Failure("400", $"A Plant with the Id '{Id} already exists'. Please check it out ")

                    );
            }
            var scientificName = await _repository.ValidateAsync(h => h.NombreCientifico.Value == Entity.NombreCientifico);

            if (scientificName)
            {
                _logger.LogError("Plant registration failed: A plant with the name '{NombreCientifico}' already exists", Entity.NombreCientifico);
                return ResultT<PlantaDto>.Failure(
                    Error.Failure("400", $"A plant with the Id '{Id} already exists'. Please check it out ")

                    );
            }

            string? imagen = null;
            if (Entity.Imagen != null)
            {
                using var stream = Entity.Imagen.OpenReadStream();
                imagen = await _cloudinary.UploadImageAsync(stream, Entity.Imagen.FileName, cancellationToken);


            }
            string? distribucionGeografica = null;
            if (Entity.DistribucionGeografica != null)
            {
                using var stream = Entity.DistribucionGeografica.OpenReadStream();
                distribucionGeografica = await _cloudinary.UploadImageAsync(stream, Entity.DistribucionGeografica.FileName, cancellationToken);
            }


            Plant.NombreComun = new NombreComunPlant(Entity.NombreComun);
            Plant.NombreCientifico = new NombreCientificoPlant(Entity.NombreCientifico);
            Plant.EstadoDeConservacion = Entity.EstadoDeConservacion;
            Plant.Filo = new FiloPlant(Entity.Filo);
            Plant.Clase = new ClasePlant(Entity.Clase);
            Plant.Orden = new OrdenPlant(Entity.Orden);
            Plant.Familia = new FamiliaPlant(Entity.Familia);
            Plant.Genero = new GeneroPlant(Entity.Genero);
            Plant.Especie = new EspeciePlant(Entity.Especie);
            Plant.SubEspecie = new SubEspeciePlant(Entity.SubEspecie);
            Plant.Observaciones = Entity.Observaciones;
            Plant.DistribucionGeograficaUrl = distribucionGeografica;
            Plant.ImagenUrl = imagen;
            Plant.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(Plant, cancellationToken);

            _logger.LogInformation("Plant with the id {IdPlanta} has been successfully created  ", Plant.IdPlanta);

            PlantaDto plantaDto = new
                (
                IdPlanta: Plant.IdPlanta,
                NombreComun: Plant.NombreComun.Value,
                NombreCientifico: Plant.NombreCientifico.Value,
                EstadoDeConservacion: Plant.EstadoDeConservacion,
                EstatusBiogeografico: Plant.EstatusBiogeografico,
                Filo: Plant.Filo.Value,
                Clase: Plant.Clase.Value,
                Orden: Plant.Orden.Value,
                Familia: Plant.Familia.Value,
                Genero: Plant.Genero.Value,
                Especie: Plant.Especie.Value,
                SubEspecie: Plant.SubEspecie.Value,
                Observaciones: Plant.Observaciones,
                DistribucionGeograficaUrl: Plant.DistribucionGeograficaUrl,
                ImagenUrl: Plant.ImagenUrl,
                IdHabitat: Plant.HabitatId,
                CreatedAt: Plant.CreatedAt,
                UpdatedAt: Plant.UpdatedAt

                );


            return ResultT<PlantaDto>.Success(plantaDto);

        }
        public async Task<ResultT<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken)
        {
            var planta = await _repository.GetByIdAsync(Id, cancellationToken);
            if (planta == null)
            {
                _logger.LogError("No register planta found");
                return ResultT<Guid>.Failure(Error.Failure("404", $"{Id} is not registered"));

            }

            await _repository.DeleteChangesAsync(planta, cancellationToken);
            _logger.LogInformation("Successfully retrieved planta deatails. Id: {IdPlanta}", planta.IdPlanta);

            return ResultT<Guid>.Success(planta.IdPlanta ?? Guid.Empty);
        }

        public async Task<ResultT<PlantaDto>> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(commonName))
            {
                _logger.LogError("Validation failed: The 'commonName' is requiered but was not provided or is empty ");
                return ResultT<PlantaDto>.Failure(Error.Failure("400", "The plant name cannot be empty"));
            }
            var existCommonName = await _repository.ValidateAsync(b => b.NombreComun.Value == commonName);

            if (!existCommonName)
            {
                _logger.LogWarning("plant search failed: No registered plant found with the name {commonName}", commonName);
                return ResultT<PlantaDto>.Failure(Error.Failure("404", $"No plant registered under the name {commonName}"));

            }
            var filterBycommonName = await _repository.FilterByCommonNameAsync(commonName, cancellationToken);
            if (filterBycommonName == null)
            {
                _logger.LogWarning("Plant search failed: No registered plant found with the name {commonName} ", commonName);
                return ResultT<PlantaDto>.Failure(Error.Failure("404", "The register plant could not be found "));
            }


            PlantaDto plantaDto = new(
             IdPlanta: filterBycommonName.IdPlanta,
             NombreComun: filterBycommonName.NombreComun.Value,
             NombreCientifico: filterBycommonName.NombreCientifico.Value,
             EstadoDeConservacion: filterBycommonName.EstadoDeConservacion,
             EstatusBiogeografico: filterBycommonName.EstatusBiogeografico,
            Filo: filterBycommonName.Filo.Value,
            Clase: filterBycommonName.Clase.Value,
             Orden: filterBycommonName.Orden.Value,
             Genero: filterBycommonName.Genero.Value,
             Familia: filterBycommonName.Familia.Value,
             Especie: filterBycommonName.Especie.Value,
             SubEspecie: filterBycommonName.SubEspecie.Value,
             Observaciones: filterBycommonName.Observaciones,
             DistribucionGeograficaUrl: filterBycommonName.DistribucionGeograficaUrl,
             IdHabitat: filterBycommonName.HabitatId,
             ImagenUrl: filterBycommonName.ImagenUrl,
             CreatedAt: filterBycommonName.CreatedAt,
             UpdatedAt: filterBycommonName.UpdatedAt

               );

            _logger.LogInformation("Search successful: Found registered plant {commonName} wit id {IdPlanta}", filterBycommonName.NombreComun, filterBycommonName.IdPlanta);
            return ResultT<PlantaDto>.Success(plantaDto);

        }

        public async Task<ResultT<PlantaDto>> FilterByScientificNameAsync(string scientificName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(scientificName))
            {
                _logger.LogError("Validation failed: The 'scientificName' is requiered but was not provided or is empty ");
                return ResultT<PlantaDto>.Failure(Error.Failure("400", "The plant name cannot be empty"));
            }
            var existscientificName = await _repository.ValidateAsync(b => b.NombreCientifico.Value == scientificName);

            if (!existscientificName)
            {
                _logger.LogWarning("plant search failed: No registered plant found with the name {scientificName}", scientificName);
                return ResultT<PlantaDto>.Failure(Error.Failure("404", $"No plant registered under the name {scientificName}"));

            }
            var filterByscientificName = await _repository.FilterByScientificNameAsync(scientificName, cancellationToken);
            if (filterByscientificName == null)
            {
                _logger.LogWarning("Plant search failed: No registered plant found with the name {scientificName} ", scientificName);
                return ResultT<PlantaDto>.Failure(Error.Failure("404", "The register plant could not be found "));
            }


            PlantaDto plantaDto = new(
             IdPlanta: filterByscientificName.IdPlanta,
             NombreComun: filterByscientificName.NombreComun.Value,
             NombreCientifico: filterByscientificName.NombreCientifico.Value,
             EstadoDeConservacion: filterByscientificName.EstadoDeConservacion,
             EstatusBiogeografico: filterByscientificName.EstatusBiogeografico,
            Filo: filterByscientificName.Filo.Value,
            Clase: filterByscientificName.Clase.Value,
             Orden: filterByscientificName.Orden.Value,
             Genero: filterByscientificName.Genero.Value,
             Familia: filterByscientificName.Familia.Value,
             Especie: filterByscientificName.Especie.Value,
             SubEspecie: filterByscientificName.SubEspecie.Value,
             Observaciones: filterByscientificName.Observaciones,
             DistribucionGeograficaUrl: filterByscientificName.DistribucionGeograficaUrl,
             IdHabitat: filterByscientificName.HabitatId,
             ImagenUrl: filterByscientificName.ImagenUrl,
             CreatedAt: filterByscientificName.CreatedAt,
             UpdatedAt: filterByscientificName.UpdatedAt

               );

            _logger.LogInformation("Search successful: Found registered plant {NombreCientifico} wit id {Idplanta}", filterByscientificName.NombreCientifico, filterByscientificName.IdPlanta);
            return ResultT<PlantaDto>.Success(plantaDto);
        }

        
    }
   
}
