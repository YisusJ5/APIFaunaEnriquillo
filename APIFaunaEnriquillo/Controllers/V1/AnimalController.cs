using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.AnimalesDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.AplicationLayer.Services;
using APIFaunaEnriquillo.Validations.Animal;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace APIFaunaEnriquillo.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/animal")]
    public class AnimalController (
        
        IAnimalService AnimalService,
        IValidator<AnimalInsertDto> CreateValidator,
        IValidator<AnimalUpdateDto> UpdateValidator



        ) : ControllerBase
    {

        [HttpPost]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Create([FromForm] AnimalInsertDto AnimalInsert, CancellationToken cancellationToken)
        {
            var resultValidation = await CreateValidator.ValidateAsync(AnimalInsert, cancellationToken);
            if (!resultValidation.IsValid)
                return BadRequest(resultValidation.Errors);

            var result = await AnimalService.CreateAsync(AnimalInsert, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }


        [HttpGet("{id}")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await AnimalService.GetById(id, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);
        }

        [HttpGet("Recent")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> GetRecent(CancellationToken cancellationToken)
        {
            var result = await AnimalService.GetRecentAsync(cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }



        [HttpPut("{id}")]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] AnimalUpdateDto animalUpdateDto, CancellationToken cancellationToken)
        {
            var resultValidation = await UpdateValidator.ValidateAsync(animalUpdateDto, cancellationToken);
            if (!resultValidation.IsValid)
                return BadRequest(resultValidation.Errors);
            var result = await AnimalService.UpdateAsync(id, animalUpdateDto, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);



        }

        [HttpDelete("{id}")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await AnimalService.DeleteAsync(id, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

        [HttpGet("Pagination")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            var result = await AnimalService.GetPageResult(pageNumber, pageSize, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

        [HttpGet("search/commonName/{CommonName}")]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> SearchByCommonName([FromRoute] string CommonName, CancellationToken cancellationToken)
        {
            var result = await AnimalService.FilterByCommonNameAsync(CommonName, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

        [HttpGet("search/ScientificName/{ScientificName}")]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> SearchByScientificName([FromRoute] string ScientificName, CancellationToken cancellationToken)
        {
            var result = await AnimalService.FilterByScientificNameAsync(ScientificName, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }



    }
}
