using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.AnimalesDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Repositories;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.AplicationLayer.Pagination;
using APIFaunaEnriquillo.Core.DomainLayer.Agregate.HabitatAgregate;
using APIFaunaEnriquillo.Core.DomainLayer.Utils;
using APIFaunaEnriquillo.Core.DomainLayer.Value_object.HabitatObjects;
using CloudinaryDotNet.Core;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using static APIFaunaEnriquillo.Core.DomainLayer.Utils.Result;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Services
{
    public class HabitatService: IHabitatService
    {
        private readonly IHabitatRepository _repository;
        private readonly ILogger<HabitatDto> _logger;
        private readonly ICloudinaryService _cloudinary;

        public HabitatService(IHabitatRepository repository, ILogger<HabitatDto> logger, ICloudinaryService cloudinaryService)
        {
            _repository = repository;
            _logger = logger;
            _cloudinary = cloudinaryService;
        }

        public async Task<Result.ResultT<PageResult<HabitatDto>>> GetPageResult(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogError("Invalid page parameters: pageNumber and pageSize must be geater than 0. ");
                return ResultT<PageResult<HabitatDto>>.Failure(
                    Error.Failure("400", "Invalid pagination parameters. Both pageNumber and pageSize must be greater than 0.")
                    );
            }


            var entityWithNumber = await _repository.GetPageResultAsync(pageNumber, pageSize, cancellationToken);
            var dto = entityWithNumber.Items.Select(x => new HabitatDto(
                 IdHabitat: x.IdHabitat,
                 NombreComun: x.NombreComun.Value,
                 NombreCientifico: x.NombreCientifico.Value,
                 Clima: x.Clima,
                 Descripcion: x.Descripcion,
                 UbicacionGeograficaUrl: x.UbicacionGeograficaUrl,
                 ImagenUrl: x.ImagenUrl,    
                 CreatedAt: x.CreatedAt,
                 UpdatedAt: x.UpdatedAt
            )).ToList();


            if (!dto.Any())
            {
                _logger.LogError("No register found");
                return ResultT<PageResult<HabitatDto>>.Failure(
                    Error.Failure("404", "The list is empty")
                );
            }

            PageResult<HabitatDto> pageResult = new()
            {
                TotalItems = entityWithNumber.TotalItems,
                CurrentPage = entityWithNumber.CurrentPage,
                TotalPages = entityWithNumber.TotalPages,
                Items = dto,

            };


            _logger.LogInformation("Successfully retrieved {count} Habitat. Page{CurrentPage} of {TotalPages} ", dto.Count, pageResult.CurrentPage, pageResult.TotalPages);
            return ResultT<PageResult<HabitatDto>>.Success(pageResult);



        }

        public async Task<Result.ResultT<HabitatDto>> GetById(Guid Id, CancellationToken cancellationToken)
        {
            var habitat = await _repository.GetByIdAsync(Id, cancellationToken);
            if (habitat == null)
            {

                _logger.LogError("No register planta found");

                return ResultT<HabitatDto>.Failure(Error.Failure("404", "The register habitat could not be found "));
            }

            HabitatDto habitatDto = new(

                   IdHabitat: habitat.IdHabitat,
                   NombreComun: habitat.NombreComun.Value,
                   NombreCientifico: habitat.NombreCientifico.Value,
                   Clima: habitat.Clima,
                   Descripcion: habitat.Descripcion,
                   UbicacionGeograficaUrl: habitat.UbicacionGeograficaUrl,
                   ImagenUrl: habitat.ImagenUrl,
                   CreatedAt: habitat.CreatedAt,
                   UpdatedAt: habitat.UpdatedAt
                );

            _logger.LogInformation("Habitat with ID {IdHabitat} was successfully retrieved ", Id);
            return ResultT<HabitatDto>.Success(habitatDto);
        }

        public async Task<Result.ResultT<HabitatDto>> CreateAsync(HabitatInsertDto EntityInsertDto, CancellationToken cancellationToken)
        {
            if (EntityInsertDto == null)
            {
                _logger.LogError("Invalid parameter: HabitatInsertDto ");
                return ResultT<HabitatDto>.Failure(Error.Failure("404", "The register habitat could not be found "));

            }
            var existCommonName = await _repository.ValidateAsync(h => h.NombreComun.Value == EntityInsertDto.NombreComun);

            if (existCommonName)
            {
                _logger.LogError("Habitat registration failed: A habitat with the name '{NombreComun}' already exists", EntityInsertDto.NombreComun);
                return ResultT<HabitatDto>.Failure(
                    Error.Failure("400", $"A habitat with the Common name '{EntityInsertDto.NombreComun} already exists'. Please check it out ")

                    );
            }

            var existsScientificName = await _repository.ValidateAsync(h => h.NombreCientifico.Value == EntityInsertDto.NombreCientifico);

            if (existsScientificName)
            {
                _logger.LogError("Habitat registration failed: A Habitat with the name '{NombreCientifico}' already exists", EntityInsertDto.NombreCientifico);
                return ResultT<HabitatDto>.Failure(
                    Error.Failure("400", $"A habitat with the Scientific name '{EntityInsertDto.NombreCientifico} already exists'. Please check it out ")

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

            string? UbicacionGeografica = null;
            if (EntityInsertDto.UbicacionGeografica != null)
            {
                using var stream = EntityInsertDto.UbicacionGeografica.OpenReadStream();
                UbicacionGeografica = await _cloudinary.UploadImageAsync(
                    stream,
                    EntityInsertDto.UbicacionGeografica.FileName,
                    cancellationToken
                    );
            }
            Habitat habitatI = new()
            {
                IdHabitat = Guid.NewGuid(),
                NombreComun = new NombreComunHabitat (EntityInsertDto.NombreComun),
                NombreCientifico = new NombreCientificoHabitat(EntityInsertDto.NombreCientifico),
                Clima = EntityInsertDto.Clima,
                Descripcion = EntityInsertDto.Descripcion,
                UbicacionGeograficaUrl = UbicacionGeografica,
                ImagenUrl = Imagen
            };

            await _repository.InsertAsync(habitatI, cancellationToken);

            HabitatDto habitatDto = new(
                IdHabitat: habitatI.IdHabitat,
                NombreComun: habitatI.NombreComun.Value,
                NombreCientifico: habitatI.NombreCientifico.Value,
                Clima: habitatI.Clima,
                Descripcion: habitatI.Descripcion,
                UbicacionGeograficaUrl: UbicacionGeografica,
                ImagenUrl: Imagen,
                CreatedAt: habitatI.CreatedAt,
                UpdatedAt: habitatI.UpdatedAt
            );

            _logger.LogInformation("Successfully retrieved habitat deatails. Id: {IdHabitat}, Common name: {NombreComun}, Scientific name: {NombreCientifico}, Climate: {Clima}, Description: {Descripcion} ",
                habitatDto.IdHabitat, habitatDto.NombreComun, habitatDto.NombreCientifico, habitatDto.Clima, habitatDto.Descripcion);
            return ResultT<HabitatDto>.Success(habitatDto);

        }

        public async Task<Result.ResultT<HabitatDto>> UpdateAsync(Guid Id, HabitatUpdateDto Entity, CancellationToken cancellationToken)
        {
            var habitat = await _repository.GetByIdAsync(Id, cancellationToken);

            if (habitat == null)
            {
                _logger.LogError("no registered habitat found ");
                return ResultT<HabitatDto>.Failure(Error.Failure("404", $"{Id} is already registered"));

            }
            var existCommonName = await _repository.ValidateAsync(h => h.NombreComun.Value == Entity.NombreComun);

            if (existCommonName)
            {
                _logger.LogError("Habitat registration failed: A habitat with the name '{NombreComun}' already exists", Entity.NombreComun);
                return ResultT<HabitatDto>.Failure(
                    Error.Failure("400", $"A habitat with the Id '{Id} already exists'. Please check it out ")

                    );
            }
            var scientificName = await _repository.ValidateAsync(h => h.NombreCientifico.Value == Entity.NombreCientifico);

            if (scientificName)
            {
                _logger.LogError("Habitat registration failed: A habitat with the name '{NombreCientifico}' already exists", Entity.NombreCientifico);
                return ResultT<HabitatDto>.Failure(
                    Error.Failure("400", $"A habitat with the Id '{Id} already exists'. Please check it out ")

                    );
            }

            string? imagen = null;
            if (Entity.Imagen != null)
            {
                using var stream = Entity.Imagen.OpenReadStream();
                imagen = await _cloudinary.UploadImageAsync(stream, Entity.Imagen.FileName, cancellationToken);


            }
            string? ubicacionGeografica = null;
            if (Entity.UbicacionGeografica != null)
            {
                using var stream = Entity.UbicacionGeografica.OpenReadStream();
                ubicacionGeografica = await _cloudinary.UploadImageAsync(stream, Entity.UbicacionGeografica.FileName, cancellationToken);
            }


            habitat.NombreComun = new NombreComunHabitat(Entity.NombreComun);
            habitat.NombreCientifico = new NombreCientificoHabitat(Entity.NombreCientifico);
            habitat.Clima = Entity.Clima;
            habitat.Descripcion = Entity.Descripcion;
            habitat.UbicacionGeograficaUrl = ubicacionGeografica;
            habitat.ImagenUrl = imagen;
            habitat.UpdatedAt = DateTime.Now; 

           await _repository.UpdateAsync(habitat, cancellationToken);

            HabitatDto habitatDto = new
                (
                IdHabitat: habitat.IdHabitat,
                NombreComun: habitat.NombreComun.Value,
                NombreCientifico: habitat.NombreCientifico.Value,
                Clima: habitat.Clima,
                Descripcion: habitat.Descripcion,
                UbicacionGeograficaUrl: ubicacionGeografica,
                ImagenUrl: imagen,
                CreatedAt: habitat.CreatedAt,
                UpdatedAt: habitat.UpdatedAt
                );
            _logger.LogInformation("Successfully retrieved habitat deatails. Id: {IdHabitat}, Common name: {CommonName}, Scientific name: {ScientificName} ",
                habitatDto.IdHabitat, habitatDto.NombreComun, habitatDto.NombreCientifico);

            return ResultT<HabitatDto>.Success(habitatDto);

        }


        public async Task<Result.ResultT<Guid>> DeleteAsync(Guid Id, CancellationToken cancellationToken)
        {
            var habitat = await _repository.GetByIdAsync(Id, cancellationToken);
            if (habitat == null)
            {
                _logger.LogError("No register habitats found");
                return ResultT<Guid>.Failure(Error.Failure("404", $"{Id} is not registered"));

            }

            await _repository.DeleteChangesAsync(habitat, cancellationToken);
            _logger.LogInformation("Successfully retrieved habitat deatails. Id: {IdHabitat}", habitat.IdHabitat);

            return ResultT<Guid>.Success(habitat.IdHabitat ?? Guid.Empty);
        }

        public async Task<Result.ResultT<HabitatDto>> FilterByCommonNameAsync(string commonName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(commonName))
            {
                _logger.LogError("Validation failed: The 'commonNameHabitat' is requiered but was not provided or is empty ");
                return ResultT<HabitatDto>.Failure(Error.Failure("400", "The Habitat name cannot be empty"));
            }
            var existCommonName = await _repository.ValidateAsync(b => b.NombreComun.Value == commonName);

            if (!existCommonName)
            {
                _logger.LogWarning("Habitat search failed: No registered habitat found with the name {commonName}", commonName);
                return ResultT<HabitatDto>.Failure(Error.Failure("404", $"No habitat registered under the name {commonName}"));

            }
            var filterBycommonName = await _repository.FilterByCommonNameAsync(commonName, cancellationToken);
            if (filterBycommonName == null)
            {
                _logger.LogWarning("Habitat search failed: No registered habitat found with the name {commonName} ", commonName);
                return ResultT<HabitatDto>.Failure(Error.Failure("404", "The register habitat could not be found "));
            }

            HabitatDto habitatDto = new(
                IdHabitat: filterBycommonName.IdHabitat,
                NombreComun: filterBycommonName.NombreComun.Value,
                NombreCientifico: filterBycommonName.NombreCientifico.Value,
                Clima: filterBycommonName.Clima,
                Descripcion: filterBycommonName.Descripcion,
                UbicacionGeograficaUrl: filterBycommonName.UbicacionGeograficaUrl,
                ImagenUrl: filterBycommonName.ImagenUrl,
                CreatedAt: filterBycommonName.CreatedAt,
                UpdatedAt: filterBycommonName.UpdatedAt


                );
            _logger.LogInformation("Search successful: Found registered habitat {commonNameHabitat} wit id {IdHabitat}", filterBycommonName.NombreComun, filterBycommonName.IdHabitat);
            return ResultT<HabitatDto>.Success(habitatDto);

        }

        public async Task<Result.ResultT<HabitatDto>> FilterByScientificNameAsync(string scientificName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(scientificName))
            {
                _logger.LogError("Validation failed: The 'scientificName' is requiered but was not provided or is empty ");
                return ResultT<HabitatDto>.Failure(Error.Failure("400", "The Habitat name cannot be empty"));
            }
            var ExistscientificNameHabitat = await _repository.ValidateAsync(b => b.NombreCientifico.Value == scientificName);

            if (!ExistscientificNameHabitat)
            {
                _logger.LogWarning("Habitat search failed: No registered habitat found with the name {scientificName}", scientificName);
                return ResultT<HabitatDto>.Failure(Error.Failure("404", $"No habitat registered under the name {scientificName}"));

            }
            var filterByscientificName = await _repository.FilterByCommonNameAsync(scientificName, cancellationToken);
            if (filterByscientificName == null)
            {
                _logger.LogWarning("Habitat search failed: No registered habitat found with the name {nombreCientifico} ", scientificName);
                return ResultT<HabitatDto>.Failure(Error.Failure("404", "The register habitat could not be found "));
            }


            HabitatDto habitatDto = new(
               IdHabitat: filterByscientificName.IdHabitat,
               NombreComun: filterByscientificName.NombreComun.Value,
               NombreCientifico: filterByscientificName.NombreCientifico.Value,
               Clima: filterByscientificName.Clima,
               Descripcion: filterByscientificName.Descripcion,
               UbicacionGeograficaUrl: filterByscientificName.UbicacionGeograficaUrl,
               ImagenUrl: filterByscientificName.ImagenUrl,
               CreatedAt: filterByscientificName.CreatedAt,
               UpdatedAt: filterByscientificName.UpdatedAt


               );

            _logger.LogInformation("Search successful: Found registered habitat {nombreCientifico} wit id {IdHabitat}", filterByscientificName.NombreCientifico, filterByscientificName.IdHabitat);
            return ResultT<HabitatDto>.Success(habitatDto);


        }

      

       
    }
}
