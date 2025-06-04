using APIFaunaEnriquillo.Core.AplicationLayer.Dtos;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Auth;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Forgot;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Reset;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Register;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.Core.DomainLayer.Utils;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace APIFaunaEnriquillo.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/account")]
    public class AccountController(
        IAccountService accountService,
        IValidator<RegisterRequest> registerValidator,
        IValidator<ForgotRequest> forgotValidator,
        IValidator<ResetPasswordRequest> resetValidator,
        IValidator<UpdateAccountDto> updateValidator,
        IValidator<AuthRequest> authValidator) : ControllerBase
    {
        [HttpPost("Admins")]
        [EnableRateLimiting("Fixed")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest request)
        {
            var validationResult = await registerValidator.ValidateAsync(request, new CancellationToken());
            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var result = await accountService.RegisterOwnerAsync(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.ErrorMessage);

        }

        [HttpPost("Editores")]
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("Fixed")]
        public async Task<IActionResult> RegisterEditor([FromBody] RegisterRequest request)
        {
            var validationResult = await registerValidator.ValidateAsync(request, new CancellationToken());
            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var result = await accountService.RegisterAccountAsync(request, Roles.Editor);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("Confirm_Account")]
        [EnableRateLimiting("Fixed")]
        public async Task<IActionResult> ConfirmAccountAsync([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await accountService.ConfirmAccountAsync(userId, token);
            if (result.Success)
                return Ok(result.Data);
            return NotFound(result.ErrorMessage);
        }
        [HttpPost("Auth")]
        [EnableRateLimiting("Fixed")]
        public async Task<IActionResult> AuthAsync([FromBody] AuthRequest request)
        {
            var validationResult = await authValidator.ValidateAsync(request, new CancellationToken());
            if (!validationResult.IsValid)
                return BadRequest(validationResult);
            var result = await accountService.AuthAsync(request);
            return result.StatusCode switch
            {
                404 => NotFound(ApiResponse<string>.ErrorResponse($"{request.Email} no encontrado")),
                400 => BadRequest(ApiResponse<string>.ErrorResponse($"Cuenta no confirmada por {request.Email} ")),
                401 => Unauthorized(ApiResponse<string>.ErrorResponse($"Credenciales incorrectas para {request.Email} ")),
                _ => Ok(ApiResponse<AuthResponse>.SuccessResponse(result))
            };

        }

        [HttpPost("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccountAsync([FromRoute] string userId)
        {
           await accountService.RemoveAccountAsync(userId);
            return NoContent();
        }



        [HttpPost("Forgot_Password")]
        [EnableRateLimiting("Fixed")]
        public async  Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotRequest request)
        {
            var validationResult = await forgotValidator.ValidateAsync(request, new CancellationToken());
            if (!validationResult.IsValid)
                return BadRequest(validationResult);
            var result = await accountService.GetForgotPasswordAsync(request);
            if (result.Success)
                return Ok(result.Data);
            return NotFound(result.ErrorMessage);
        }

        [HttpPost("Reset_Password")]
        [EnableRateLimiting("Fixed")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var validationResult = await resetValidator.ValidateAsync(request, new CancellationToken());
            if (!validationResult.IsValid)
                return BadRequest(validationResult);
            var result = await accountService.ResetPasswordAsync(request);
            if (result.Success)
                return Ok(result.Data);
            return NotFound(result.ErrorMessage);
        }

        [HttpGet("{userId}/datails")]
        [Authorize]
        [EnableRateLimiting("Fixed")]

        public async Task<IActionResult> GetDatailsAsync([FromRoute] string userId)
        {
            var result = await accountService.GetAccoundDetailsAsync(userId);
            if (result.Success)
                return Ok(result.Data);
            return NotFound(result.ErrorMessage);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAccountAsync([FromRoute] string id, [FromBody] UpdateAccountDto request)
        {
            var validationResult = await updateValidator.ValidateAsync(request, new CancellationToken());
            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var result = await accountService.UpdateAccountDetailsAsync(request, id);
            if (result.Success)
                return Ok(result.Data);
            return NotFound(result.ErrorMessage);


        }

        [HttpPost("logout")]
        [Authorize]
        [EnableRateLimiting("Fixed")]
        public async Task<IActionResult> LougoutAsync()
        {
            await accountService.LogOutAsync();
            return NoContent();
        }
    }
}
