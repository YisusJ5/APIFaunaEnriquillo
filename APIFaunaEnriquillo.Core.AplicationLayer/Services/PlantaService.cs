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
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repository;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.AplicationLayer.Pagination;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.Core.DomainLayer.Models;
using APIFaunaEnriquillo.Core.DomainLayer.Utils;
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
                   NombreComun: x.NombreComun,
                   NombreCientifico: x.NombreCientifico,
                   EstadoDeConservacion: x.EstadoDeConservacion,
                   EstatusBiogeografico: x.EstatusBiogeografico,
                   Filo: x.Filo,
                   Clase: x.Clase,
                   Orden: x.Orden,
                   Familia: x.Familia,
                   Genero: x.Genero,
                   Especie: x.Especie,
                   SubEspecie: x.SubEspecie,
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
                   NombreComun: planta.NombreComun,
                   NombreCientifico: planta.NombreCientifico,
                   EstadoDeConservacion: planta.EstadoDeConservacion,
                   EstatusBiogeografico: planta.EstatusBiogeografico,
                   Filo: planta.Filo,
                   Clase: planta.Clase,
                   Orden: planta.Orden,
                   Familia: planta.Familia,
                   Genero: planta.Genero,
                   Especie: planta.Especie,
                   SubEspecie: planta.SubEspecie,
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
        public async Task<ResultT<PlantaDto>> CreateAsync(PlantaInsertDto EntityInsertDto, CancellationToken cancellationToken)
        {
            if (EntityInsertDto == null)
            {
                _logger.LogError("Invalid parameter: PlantaInsertDto ");
                return ResultT<PlantaDto>.Failure(Error.Failure("404", "The register Planta could not be found "));

            }
            var existCommonName = await _repository.ValidateAsync(h => h.NombreComun == EntityInsertDto.NombreComun);

            if (existCommonName)
            {
                _logger.LogError("Planta registration failed: A planta with the name '{NombreComun}' already exists", EntityInsertDto.NombreComun);
                return ResultT<PlantaDto>.Failure(
                    Error.Failure("400", $"A Planta with the Common name '{EntityInsertDto.NombreComun} already exists'. Please check it out ")

                    );
            }

            var existsScientificName = await _repository.ValidateAsync(h => h.NombreCientifico == EntityInsertDto.NombreCientifico);

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
                NombreComun = EntityInsertDto.NombreComun,
                NombreCientifico = EntityInsertDto.NombreCientifico,
                EstadoDeConservacion = EntityInsertDto.EstadoDeConservacion,
                EstatusBiogeografico = EntityInsertDto.EstatusBiogeografico,
                Filo = EntityInsertDto.Filo,
                Clase = EntityInsertDto.Clase,
                Orden = EntityInsertDto.Orden,
                Familia = EntityInsertDto.Familia,
                Genero = EntityInsertDto.Genero,
                Especie = EntityInsertDto.Especie,
                SubEspecie = EntityInsertDto.SubEspecie,
                Observaciones = EntityInsertDto.Observaciones,
                DistribucionGeograficaUrl = DistribucionGeografica,
                ImagenUrl = Imagen,

            };


            await _repository.InsertAsync(PlantaI, cancellationToken);


            PlantaDto plantaDto = new(
                   IdPlanta: PlantaI.IdPlanta,
                   NombreComun: PlantaI.NombreComun,
                   NombreCientifico: PlantaI.NombreCientifico,
                   EstadoDeConservacion: PlantaI.EstadoDeConservacion,
                   EstatusBiogeografico: PlantaI.EstatusBiogeografico,
                   Filo: PlantaI.Filo,
                   Clase: PlantaI.Clase,
                   Orden: PlantaI.Orden,
                   Familia: PlantaI.Familia,
                   Genero: PlantaI.Genero,
                   Especie: PlantaI.Especie,
                   SubEspecie: PlantaI.SubEspecie,
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
            var planta = await _repository.GetByIdAsync(Id, cancellationToken);

            if (planta == null)
            {
                _logger.LogError("no registered Planta found ");
                return ResultT<PlantaDto>.Failure(Error.Failure("404", $"{Id} is already registered"));

            }
            var existCommonName = await _repository.ValidateAsync(h => h.NombreComun == Entity.NombreComun);

            if (existCommonName)
            {
                _logger.LogError("Planta registration failed: A planta with the name '{NombreComun}' already exists", Entity.NombreComun);
                return ResultT<PlantaDto>.Failure(
                    Error.Failure("400", $"A Plant with the Id '{Id} already exists'. Please check it out ")

                    );
            }
            var scientificName = await _repository.ValidateAsync(h => h.NombreCientifico == Entity.NombreCientifico);

            if (scientificName)
            {
                _logger.LogError("Planta registration failed: A planta with the name '{NombreCientifico}' already exists", Entity.NombreCientifico);
                return ResultT<PlantaDto>.Failure(
                    Error.Failure("400", $"A planta with the Id '{Id} already exists'. Please check it out ")

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


            planta.NombreComun = Entity.NombreComun;
            planta.NombreCientifico = Entity.NombreCientifico;
            planta.EstadoDeConservacion = Entity.EstadoDeConservacion;
            planta.EstatusBiogeografico = Entity.EstatusBiogeografico;
            planta.Filo = Entity.Filo;
            planta.Clase = Entity.Clase;
            planta.Orden = Entity.Orden;
            planta.Genero = Entity.Genero;
            planta.Familia = Entity.Familia;
            planta.Especie = Entity.Especie;
            planta.SubEspecie = Entity.SubEspecie;
            planta.Observaciones = Entity.Observaciones;
            planta.DistribucionGeograficaUrl = distribucionGeografica;
            planta.ImagenUrl = imagen;
            planta.UpdatedAt = DateTime.Now;

            PlantaDto plantaDto = new(
              IdPlanta: planta.IdPlanta,
              NombreComun: planta.NombreComun,
              NombreCientifico: planta.NombreCientifico,
              EstadoDeConservacion:  planta.EstadoDeConservacion,
              EstatusBiogeografico:  planta.EstatusBiogeografico,
              Filo: planta.Filo,
              Clase: planta.Clase,
              Orden: planta.Orden,
              Genero: planta.Genero,
              Familia: planta.Familia,
              Especie: planta.Especie,
              SubEspecie: planta.SubEspecie,
              Observaciones: planta.Observaciones,
              DistribucionGeograficaUrl:  planta.DistribucionGeograficaUrl,
              IdHabitat: planta.HabitatId,
              ImagenUrl: planta.ImagenUrl,
              CreatedAt: planta.CreatedAt,
              UpdatedAt: planta.UpdatedAt

                );

            _logger.LogInformation("Register plant found");

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
            var existCommonName = await _repository.ValidateAsync(b => b.NombreComun == commonName);

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
             NombreComun: filterBycommonName.NombreComun,
             NombreCientifico: filterBycommonName.NombreCientifico,
             EstadoDeConservacion: filterBycommonName.EstadoDeConservacion,
             EstatusBiogeografico: filterBycommonName.EstatusBiogeografico,
            Filo: filterBycommonName.Filo,
            Clase: filterBycommonName.Clase,
             Orden: filterBycommonName.Orden,
             Genero: filterBycommonName.Genero,
             Familia: filterBycommonName.Familia,
             Especie: filterBycommonName.Especie,
             SubEspecie: filterBycommonName.SubEspecie,
             Observaciones: filterBycommonName.Observaciones,
             DistribucionGeograficaUrl: filterBycommonName.DistribucionGeograficaUrl,
             IdHabitat: filterBycommonName.HabitatId,
             ImagenUrl: filterBycommonName.ImagenUrl,
             CreatedAt: filterBycommonName.CreatedAt,
             UpdatedAt: filterBycommonName.UpdatedAt

               );

            _logger.LogInformation("Search successful: Found registered animal {commonName} wit id {IdAnimal}", filterBycommonName.NombreComun, filterBycommonName.IdPlanta);
            return ResultT<PlantaDto>.Success(plantaDto);

        }

        public async Task<ResultT<PlantaDto>> FilterByScientificNameAsync(string scientificName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(scientificName))
            {
                _logger.LogError("Validation failed: The 'scientificName' is requiered but was not provided or is empty ");
                return ResultT<PlantaDto>.Failure(Error.Failure("400", "The plant name cannot be empty"));
            }
            var existscientificName = await _repository.ValidateAsync(b => b.NombreCientifico == scientificName);

            if (!existscientificName)
            {
                _logger.LogWarning("plant search failed: No registered plant found with the name {scientificName}", scientificName);
                return ResultT<PlantaDto>.Failure(Error.Failure("404", $"No plant registered under the name {scientificName}"));

            }
            var filterByscientificName = await _repository.FilterByCommonNameAsync(scientificName, cancellationToken);
            if (filterByscientificName == null)
            {
                _logger.LogWarning("Plant search failed: No registered plant found with the name {scientificName} ", scientificName);
                return ResultT<PlantaDto>.Failure(Error.Failure("404", "The register plant could not be found "));
            }


            PlantaDto plantaDto = new(
             IdPlanta: filterByscientificName.IdPlanta,
             NombreComun: filterByscientificName.NombreComun,
             NombreCientifico: filterByscientificName.NombreCientifico,
             EstadoDeConservacion: filterByscientificName.EstadoDeConservacion,
             EstatusBiogeografico: filterByscientificName.EstatusBiogeografico,
            Filo: filterByscientificName.Filo,
            Clase: filterByscientificName.Clase,
             Orden: filterByscientificName.Orden,
             Genero: filterByscientificName.Genero,
             Familia: filterByscientificName.Familia,
             Especie: filterByscientificName.Especie,
             SubEspecie: filterByscientificName.SubEspecie,
             Observaciones: filterByscientificName.Observaciones,
             DistribucionGeograficaUrl: filterByscientificName.DistribucionGeograficaUrl,
             IdHabitat: filterByscientificName.HabitatId,
             ImagenUrl: filterByscientificName.ImagenUrl,
             CreatedAt: filterByscientificName.CreatedAt,
             UpdatedAt: filterByscientificName.UpdatedAt

               );

            _logger.LogInformation("Search successful: Found registered animal {NombreCientifico} wit id {IdAnimal}", filterByscientificName.NombreCientifico, filterByscientificName.IdPlanta);
            return ResultT<PlantaDto>.Success(plantaDto);
        }

      

       
    }
}
