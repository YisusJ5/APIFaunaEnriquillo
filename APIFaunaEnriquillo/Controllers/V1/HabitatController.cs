using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.HabitatDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace APIFaunaEnriquillo.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/habitat")]
    public class HabitatController(
        IHabitatService habitatService,
        IValidator<HabitatInsertDto> CreateValidator,
        IValidator<HabitatUpdateDto> UpdateValidator

        ) : ControllerBase
    {
        [HttpPost]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> CreateHabitat([FromForm] HabitatInsertDto habitatInsertDto, CancellationToken cancellationToken)
        {
            var resultValidation = await CreateValidator.ValidateAsync(habitatInsertDto, cancellationToken);
            if (!resultValidation.IsValid)
                return BadRequest(resultValidation.Errors);

            var result = await habitatService.CreateAsync(habitatInsertDto, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }


        [HttpGet("{id}")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> GetByIdHabitat([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await habitatService.GetById(id, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);
        }

        [HttpGet("Recent")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> GetRecent(CancellationToken cancellationToken)
        {
            var result = await habitatService.GetRecentAsync(cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }



        [HttpPut("{id}")]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> UpdateHabitat([FromRoute] Guid id, [FromForm] HabitatUpdateDto habitatUpdateDto, CancellationToken cancellationToken)
        {
            var resultValidation = await UpdateValidator.ValidateAsync(habitatUpdateDto, cancellationToken);
            if (!resultValidation.IsValid)
                return BadRequest(resultValidation.Errors);
            var result = await habitatService.UpdateAsync(id, habitatUpdateDto, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);



        }

        [HttpDelete("{id}")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> DeleteHabitat([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await habitatService.DeleteAsync(id, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

        [HttpGet("Pagination")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            var result = await habitatService.GetPageResult(pageNumber, pageSize, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

        [HttpGet("search/commonName/{CommonName}")]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> SearchByCommonName([FromRoute] string CommonName, CancellationToken cancellationToken)
        {
            var result = await habitatService.FilterByCommonNameAsync(CommonName, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

        [HttpGet("search/ScientificName/{ScientificName}")]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> SearchByScientificName([FromRoute] string ScientificName, CancellationToken cancellationToken)
        {
            var result = await habitatService.FilterByScientificNameAsync(ScientificName, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

       


    }
}
