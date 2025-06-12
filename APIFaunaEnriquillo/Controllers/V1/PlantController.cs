using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.AnimalesDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.PlantasDto;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.AplicationLayer.Services;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace APIFaunaEnriquillo.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/plant")]
    public class PlantController(
        
        IPlantaService PlantaService,
        IValidator<PlantaInsertDto> CreateValidator,
        IValidator<PlantaUpdateDto> UpdateValidator


        ) : ControllerBase
    {


        [HttpPost]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Create([FromForm] PlantaInsertDto plantaInsert, CancellationToken cancellationToken)
        {
            var resultValidation = await CreateValidator.ValidateAsync(plantaInsert, cancellationToken);
            if (!resultValidation.IsValid)
                return BadRequest(resultValidation.Errors);

            var result = await PlantaService.CreateAsync(plantaInsert, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }


        [HttpGet("{id}")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await PlantaService.GetById(id, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);
        }

        [HttpGet("Recent")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> GetRecent(CancellationToken cancellationToken)
        {
            var result = await PlantaService.GetRecentAsync(cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }



        [HttpPut("{id}")]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] PlantaUpdateDto plantaUpdateDto, CancellationToken cancellationToken)
        {
            var resultValidation = await UpdateValidator.ValidateAsync(plantaUpdateDto, cancellationToken);
            if (!resultValidation.IsValid)
                return BadRequest(resultValidation.Errors);
            var result = await PlantaService.UpdateAsync(id, plantaUpdateDto, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);



        }

        [HttpDelete("{id}")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await PlantaService.DeleteAsync(id, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

        [HttpGet("Pagination")]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            var result = await PlantaService.GetPageResult(pageNumber, pageSize, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

        [HttpGet("search/commonName/{CommonName}")]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> SearchByCommonName([FromRoute] string CommonName, CancellationToken cancellationToken)
        {
            var result = await PlantaService.FilterByCommonNameAsync(CommonName, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }

        [HttpGet("search/ScientificName/{ScientificName}")]
        [EnableRateLimiting("fixed")]

        public async Task<IActionResult> SearchByScientificName([FromRoute] string ScientificName, CancellationToken cancellationToken)
        {
            var result = await PlantaService.FilterByScientificNameAsync(ScientificName, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error);

        }




    }
}
