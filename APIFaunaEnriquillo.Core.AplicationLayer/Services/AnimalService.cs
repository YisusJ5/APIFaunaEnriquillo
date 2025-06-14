﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using static APIFaunaEnriquillo.Core.DomainLayer.Utils.Result;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _repository;
        private readonly ILogger<AnimalDto> _logger;
        private readonly ICloudinaryService _cloudinary;

        public AnimalService(IAnimalRepository repository, ILogger<AnimalDto> logger, ICloudinaryService cloudinaryService)
        {

            _repository = repository;
            _logger = logger;
            _cloudinary = cloudinaryService;

        }
        public async Task<Result.ResultT<PageResult<AnimalDto>>> GetPageResult(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0) 
            {
                _logger.LogError("Invalid page parameters: pageNumber and pageSize must be geater than 0. ");
                return ResultT<PageResult<AnimalDto>>.Failure(
                    Error.Failure("400", "Invalid pagination parameters. Both pageNumber and pageSize must be greater than 0.")
                    );
            }

            var entityWithNumber = await _repository.GetPageResultAsync(pageNumber, pageSize, cancellationToken);
            var dto = entityWithNumber.Items.Select(x => new AnimalDto(
                 IdAnimal: x.IdAnimal,
                 NombreComun: x.NombreComun.Value,
                 NombreCientifico: x.NombreCientifico.Value,
                 Dieta:x.Dieta,
                 EstadoDeConservacion: x.EstadoDeConservacion,
                 FormaDeReproduccion: x.FormaDeReproduccion,
                 TipoDesarrolloEmbrionario: x.TipoDesarrolloEmbrionario,
                 EstatusBiogeográfico: x.EstatusBiogeográfico,
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
                return ResultT<PageResult<AnimalDto>>.Failure(
                    Error.Failure("404", "The list is empty")
                );
            }

            PageResult<AnimalDto> pageResult = new()
            {
                TotalItems = entityWithNumber.TotalItems,
                CurrentPage = entityWithNumber.CurrentPage,
                TotalPages = entityWithNumber.TotalPages,
                Items = dto,

            };

            _logger.LogInformation("Successfullt retrieved {count} animals. Page{CurrentPage} of {TotalPages} ", dto.Count, pageResult.CurrentPage, pageResult.TotalPages );
            return ResultT<PageResult<AnimalDto>>.Success(pageResult);
        }
        public async Task<Result.ResultT<AnimalDto>> GetById(Guid Id, CancellationToken cancellationToken)
        {
            var animal = await _repository.GetByIdAsync(Id, cancellationToken);
            if (animal == null)
            {

                _logger.LogError("No register Animal found");

                return ResultT<AnimalDto>.Failure(Error.Failure("404", "The register Animal could not be found "));
            }

            AnimalDto animalDto = new(

                 IdAnimal: animal.IdAnimal,
                 NombreComun: animal.NombreComun.Value,
                 NombreCientifico: animal.NombreCientifico.Value,
                 Dieta: animal.Dieta,
                 EstadoDeConservacion: animal.EstadoDeConservacion,
                 FormaDeReproduccion: animal.FormaDeReproduccion,
                 TipoDesarrolloEmbrionario: animal.TipoDesarrolloEmbrionario,
                 EstatusBiogeográfico: animal.EstatusBiogeográfico,
                 Filo: animal.Filo.Value,
                 Clase: animal.Clase.Value,
                 Orden: animal.Orden.Value,
                 Familia: animal.Familia.Value,
                 Genero: animal.Genero.Value,
                 Especie: animal.Especie.Value,
                 SubEspecie: animal.SubEspecie.Value,
                 Observaciones: animal.Observaciones,
                 IdHabitat: animal.HabitatId,
                 DistribucionGeograficaUrl: animal.DistribucionGeograficaUrl,
                 ImagenUrl: animal.ImagenUrl,
                 CreatedAt: animal.CreatedAt,
                 UpdatedAt: animal.UpdatedAt

                );

            _logger.LogInformation("Animal with ID {IdAnimal} was successfully retrieved ", Id);
            return ResultT<AnimalDto>.Success(animalDto);
        }

        public async Task<ResultT<IEnumerable<AnimalDto>>> GetRecentAsync(CancellationToken cancellationToken)
        {
            var GetRecent = await _repository.GetRecentAsync(cancellationToken);
            if (GetRecent == null || !GetRecent.Any())
            {
                _logger.LogError("No recent registered animal found.");

                return ResultT<IEnumerable<AnimalDto>>.Failure(Error.NotFound("404", "The list is empty"));
            }

            IEnumerable<AnimalDto> AnimalDtos = GetRecent.Select(x => new AnimalDto
            (
                IdAnimal: x.IdAnimal,
                NombreComun: x.NombreComun.Value,
                NombreCientifico: x.NombreCientifico.Value,
                Dieta: x.Dieta,
                EstadoDeConservacion: x.EstadoDeConservacion,
                FormaDeReproduccion: x.FormaDeReproduccion,
                TipoDesarrolloEmbrionario: x.TipoDesarrolloEmbrionario,
                EstatusBiogeográfico: x.EstatusBiogeográfico,
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
            ));

            _logger.LogInformation("Successfully retrieved {Count} recent animals.", AnimalDtos.Count());

            return ResultT<IEnumerable<AnimalDto>>.Success(AnimalDtos);
        }
        public async Task<Result.ResultT<AnimalDto>> CreateAsync(AnimalInsertDto EntityInsertDto, CancellationToken cancellationToken)
        {
            if (EntityInsertDto == null)
            {
                _logger.LogError("Invalid parameter: AnimalInsertDto ");
                return ResultT<AnimalDto>.Failure(Error.Failure("404", "The register Animal could not be found "));

            }
            var existCommonName = await _repository.ValidateAsync(h => h.NombreComun.Value == EntityInsertDto.NombreComun);

            if (existCommonName)
            {
                _logger.LogError("Animal registration failed: A animal with the name '{NombreComun}' already exists", EntityInsertDto.NombreComun);
                return ResultT<AnimalDto>.Failure(
                    Error.Failure("400", $"A Animal with the Common name '{EntityInsertDto.NombreComun} already exists'. Please check it out ")

                    );
            }

            var existsScientificName = await _repository.ValidateAsync(h => h.NombreCientifico.Value == EntityInsertDto.NombreCientifico);

            if (existsScientificName)
            {
                _logger.LogError("Animal registration failed: A Animal with the name '{NombreCientifico}' already exists", EntityInsertDto.NombreCientifico);
                return ResultT<AnimalDto>.Failure(
                    Error.Failure("400", $"A Animal with the Scientific name '{EntityInsertDto.NombreCientifico} already exists'. Please check it out ")

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

            Animal animalI = new()
            {
                IdAnimal = Guid.NewGuid(),
                NombreComun = new NombreComunAnimal (EntityInsertDto.NombreComun),
                NombreCientifico = new NombreCientificoAnimal (EntityInsertDto.NombreCientifico),
                Dieta = EntityInsertDto.Dieta,
                EstadoDeConservacion = EntityInsertDto.EstadoDeConservacion,
                FormaDeReproduccion = EntityInsertDto.FormaDeReproduccion,
                TipoDesarrolloEmbrionario = EntityInsertDto.TipoDesarrolloEmbrionario,
                EstatusBiogeográfico = EntityInsertDto.EstatusBiogeográfico,
                Filo = new FiloAnimal (EntityInsertDto.Filo),
                Clase = new ClaseAnimal (EntityInsertDto.Clase),
                Orden = new OrdenAnimal (EntityInsertDto.Orden),
                Familia = new FamiliaAnimal (EntityInsertDto.Familia),
                Genero = new GeneroAnimal (EntityInsertDto.Genero),
                Especie = new EspecieAnimal (EntityInsertDto.Especie),
                SubEspecie = new SubEspecieAnimal (EntityInsertDto.SubEspecie),
                Observaciones = EntityInsertDto.Observaciones,
                DistribucionGeograficaUrl = DistribucionGeografica,
                ImagenUrl = Imagen,
                HabitatId = EntityInsertDto.HabitatId,
                CreatedAt = DateTime.Now
            };


            await _repository.InsertAsync(animalI, cancellationToken);

            AnimalDto animalDto = new(
                IdAnimal: animalI.IdAnimal,
                NombreComun: animalI.NombreComun.Value,
                NombreCientifico: animalI.NombreCientifico.Value,
                Dieta: animalI.Dieta,
                EstadoDeConservacion: animalI.EstadoDeConservacion,
                FormaDeReproduccion: animalI.FormaDeReproduccion,
                TipoDesarrolloEmbrionario: animalI.TipoDesarrolloEmbrionario,
                EstatusBiogeográfico: animalI.EstatusBiogeográfico,
                Filo: animalI.Filo.Value,
                Clase: animalI.Clase.Value,
                Orden: animalI.Orden.Value,
                Familia: animalI.Familia.Value,
                Genero: animalI.Genero.Value,
                Especie: animalI.Especie.Value,
                SubEspecie: animalI.SubEspecie.Value,
                Observaciones: animalI.Observaciones,
                IdHabitat: animalI.HabitatId,
                DistribucionGeograficaUrl: DistribucionGeografica,
                ImagenUrl: Imagen,
                CreatedAt: animalI.CreatedAt,
                UpdatedAt: animalI.UpdatedAt
            );

            _logger.LogInformation("Animal with ID {IdAnimal} was successfully created ", animalDto.IdAnimal);
            return ResultT<AnimalDto>.Success(animalDto);
        }
        public async Task<Result.ResultT<AnimalDto>> UpdateAsync(Guid Id, AnimalUpdateDto Entity, CancellationToken cancellationToken)
        {
            var animal = await _repository.GetByIdAsync(Id, cancellationToken);

            if (animal == null)
            {
                _logger.LogError("no registered animal found ");
                return ResultT<AnimalDto>.Failure(Error.Failure("404", $"{Id} is already registered"));

            }
            var existCommonName = await _repository.ValidateAsync(h => h.NombreComun.Value == Entity.NombreComun);

            if (existCommonName)
            {
                _logger.LogError("Animal registration failed: A animal with the name '{NombreComun}' already exists", Entity.NombreComun);
                return ResultT<AnimalDto>.Failure(
                    Error.Failure("400", $"A animal with the Id '{Id} already exists'. Please check it out ")

                    );
            }
            var scientificName = await _repository.ValidateAsync(h => h.NombreCientifico.Value == Entity.NombreCientifico);

            if (scientificName)
            {
                _logger.LogError("Animal registration failed: A animal with the name '{NombreCientifico}' already exists", Entity.NombreCientifico);
                return ResultT<AnimalDto>.Failure(
                    Error.Failure("400", $"A animal with the Id '{Id} already exists'. Please check it out ")

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


            animal.NombreComun = new NombreComunAnimal(Entity.NombreComun);
            animal.NombreCientifico = new NombreCientificoAnimal (Entity.NombreCientifico);
            animal.Dieta = Entity.Dieta;
            animal.EstadoDeConservacion = Entity.EstadoDeConservacion;
            animal.FormaDeReproduccion = Entity.FormaDeReproduccion;
            animal.TipoDesarrolloEmbrionario = Entity.TipoDesarrolloEmbrionario;
            animal.EstatusBiogeográfico = Entity.EstatusBiogeográfico;
            animal.Filo = new FiloAnimal (Entity.Filo);
            animal.Clase = new ClaseAnimal (Entity.Clase);
            animal.Orden = new OrdenAnimal (Entity.Orden);
            animal.Familia = new FamiliaAnimal (Entity.Familia);
            animal.Genero = new GeneroAnimal (Entity.Genero);
            animal.Especie = new EspecieAnimal (Entity.Especie);
            animal.SubEspecie = new SubEspecieAnimal (Entity.SubEspecie);
            animal.Observaciones = Entity.Observaciones;
            animal.DistribucionGeograficaUrl = distribucionGeografica;
            animal.ImagenUrl = imagen;
            animal.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(animal, cancellationToken);

            _logger.LogInformation("Animal with the id {IdAnimal} has been successfully created  ", animal.IdAnimal);

            AnimalDto animalDto = new
                (
                IdAnimal: animal.IdAnimal,
                NombreComun: animal.NombreComun.Value,
                NombreCientifico: animal.NombreCientifico.Value,
                Dieta: animal.Dieta,
                EstadoDeConservacion: animal.EstadoDeConservacion,
                FormaDeReproduccion: animal.FormaDeReproduccion,
                TipoDesarrolloEmbrionario: animal.TipoDesarrolloEmbrionario,
                EstatusBiogeográfico: animal.EstatusBiogeográfico,
                Filo: animal.Filo.Value,
                Clase: animal.Clase.Value,
                Orden: animal.Orden.Value,
                Familia: animal.Familia.Value,
                Genero: animal.Genero.Value,
                Especie: animal.Especie.Value,
                SubEspecie: animal.SubEspecie.Value,
                Observaciones: animal.Observaciones,
                DistribucionGeograficaUrl: animal.DistribucionGeograficaUrl,
                ImagenUrl: animal.ImagenUrl,
                IdHabitat: animal.HabitatId,
                CreatedAt: animal.CreatedAt,
                UpdatedAt: animal.UpdatedAt

                );


            return ResultT<AnimalDto>.Success(animalDto);



        }
        public async Task<Result.ResultT<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken)
        {
            var animal = await _repository.GetByIdAsync(Id, cancellationToken);
            if (animal == null)
            {
                _logger.LogError("No register animal found");
                return ResultT<Guid>.Failure(Error.Failure("404", $"{Id} is not registered"));

            }

            await _repository.DeleteChangesAsync(animal, cancellationToken);
            _logger.LogInformation("Successfully retrieved animal deatails. Id: {IdAnimal}", animal.IdAnimal);

            return ResultT<Guid>.Success(animal.IdAnimal ?? Guid.Empty);
        }

        public async Task<Result.ResultT<AnimalDto>> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(commonName))
            {
                _logger.LogError("Validation failed: The 'commonName' is requiered but was not provided or is empty ");
                return ResultT<AnimalDto>.Failure(Error.Failure("400", "The animal name cannot be empty"));
            }
            var existCommonName = await _repository.ValidateAsync(b => b.NombreComun.Value == commonName);

            if (!existCommonName)
            {
                _logger.LogWarning("Animal search failed: No registered animal found with the name {commonName}", commonName);
                return ResultT<AnimalDto>.Failure(Error.Failure("404", $"No animal registered under the name {commonName}"));

            }
            var filterBycommonName = await _repository.FilterByCommonNameAsync(commonName, cancellationToken);
            if (filterBycommonName == null)
            {
                _logger.LogWarning("animal search failed: No registered animal found with the name {commonName} ", commonName);
                return ResultT<AnimalDto>.Failure(Error.Failure("404", "The register animal could not be found "));
            }

            AnimalDto animalDto = new(

                IdAnimal: filterBycommonName.IdAnimal,
                NombreComun: filterBycommonName.NombreComun.Value,
                NombreCientifico: filterBycommonName.NombreCientifico.Value,
                Dieta: filterBycommonName.Dieta,
                EstadoDeConservacion: filterBycommonName.EstadoDeConservacion,
                FormaDeReproduccion: filterBycommonName.FormaDeReproduccion,
                TipoDesarrolloEmbrionario: filterBycommonName.TipoDesarrolloEmbrionario,
                EstatusBiogeográfico: filterBycommonName.EstatusBiogeográfico,
                Filo: filterBycommonName.Filo.Value,
                Clase: filterBycommonName.Clase.Value,
                Orden: filterBycommonName.Orden.Value,
                Familia: filterBycommonName.Familia.Value,
                Genero: filterBycommonName.Genero.Value,
                Especie: filterBycommonName.Especie.Value,
                SubEspecie: filterBycommonName.SubEspecie.Value,
                Observaciones: filterBycommonName.Observaciones,
                DistribucionGeograficaUrl: filterBycommonName.DistribucionGeograficaUrl,
                ImagenUrl: filterBycommonName.ImagenUrl,
                IdHabitat: filterBycommonName.HabitatId,
                CreatedAt: filterBycommonName.CreatedAt,
                UpdatedAt: filterBycommonName.UpdatedAt
                );


            _logger.LogInformation("Search successful: Found registered animal {commonName} wit id {IdAnimal}", filterBycommonName.NombreComun, filterBycommonName.IdAnimal);
            return ResultT<AnimalDto>.Success(animalDto);


        }

        public async Task<Result.ResultT<AnimalDto>> FilterByScientificNameAsync(string scientificName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(scientificName))
            {
                _logger.LogError("Validation failed: The 'scientificName' is requiered but was not provided or is empty ");
                return ResultT<AnimalDto>.Failure(Error.Failure("400", "The animal name cannot be empty"));
            }
            var existeScientificName = await _repository.ValidateAsync(b => b.NombreCientifico.Value == scientificName);

            if (!existeScientificName)
            {
                _logger.LogWarning("Animal search failed: No registered animal found with the name {scientificName}", scientificName);
                return ResultT<AnimalDto>.Failure(Error.Failure("404", $"No animal registered under the name {scientificName}"));

            }
            var filterByScientificName = await _repository.FilterByScientificNameAsync(scientificName, cancellationToken);
            if (filterByScientificName == null)
            {
                _logger.LogWarning("animal search failed: No registered animal found with the name {scientificName} ", scientificName);
                return ResultT<AnimalDto>.Failure(Error.Failure("404", "The register animal could not be found "));
            }

            AnimalDto animalDto = new(

                IdAnimal: filterByScientificName.IdAnimal,
                NombreComun: filterByScientificName.NombreComun.Value,
                NombreCientifico: filterByScientificName.NombreCientifico.Value,
                Dieta: filterByScientificName.Dieta,
                EstadoDeConservacion: filterByScientificName.EstadoDeConservacion,
                FormaDeReproduccion: filterByScientificName.FormaDeReproduccion,
                TipoDesarrolloEmbrionario: filterByScientificName.TipoDesarrolloEmbrionario,
                EstatusBiogeográfico: filterByScientificName.EstatusBiogeográfico,
                Filo: filterByScientificName.Filo.Value,
                Clase: filterByScientificName.Clase.Value,
                Orden: filterByScientificName.Orden.Value,
                Familia: filterByScientificName.Familia.Value,
                Genero: filterByScientificName.Genero.Value,
                Especie: filterByScientificName.Especie.Value,
                SubEspecie: filterByScientificName.SubEspecie.Value,
                Observaciones: filterByScientificName.Observaciones,
                DistribucionGeograficaUrl: filterByScientificName.DistribucionGeograficaUrl,
                ImagenUrl: filterByScientificName.ImagenUrl,
                IdHabitat: filterByScientificName.HabitatId,
                CreatedAt: filterByScientificName.CreatedAt,
                UpdatedAt: filterByScientificName.UpdatedAt
                );


            _logger.LogInformation("Search successful: Found registered animal {scientificName} wit id {IdAnimal}", filterByScientificName.NombreComun, filterByScientificName.IdAnimal);
            return ResultT<AnimalDto>.Success(animalDto);
        }

        
    }
    
}
