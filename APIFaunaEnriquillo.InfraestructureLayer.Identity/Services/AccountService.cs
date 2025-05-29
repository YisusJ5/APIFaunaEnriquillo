using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Auth;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Forgot;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Reset;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.DomainLayer.Setting;
using APIFaunaEnriquillo.Core.DomainLayer.Utils;
using APIFaunaEnriquillo.InfraestructureLayer.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Jwt;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Email;

namespace APIFaunaEnriquillo.InfraestructureLayer.Shared.Services
{
    public class AccountService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IOptions<JWTSetting> jwtSettings,
    IEmailService emailSender
        ) : IAccountService

    {

        private JWTSetting _jwtSetting { get; } = jwtSettings.Value;


        private string RandomTokenString()
        {
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new Byte[40];
            rng.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes);
        }
        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expired = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        private async Task<JwtSecurityToken> GenerateTokenAsync(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            List<Claim> rolesClaims = new List<Claim>();

            foreach (var role in roles)
            {
                rolesClaims.Add(new Claim("roles", role));
            }

            var claim = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Id", user.Id)
            }
                .Union(userClaims)
                .Union(rolesClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                claims: claim,
                expires: DateTime.Now.AddMinutes(_jwtSetting.DurationInMinutes),
                signingCredentials: signingCredentials
            );
            return jwtSecurityToken;
        }


        private async Task<string> SendForgotPasswordAsync(User user)
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            return code;
        }

        public async Task<AuthResponse> AuthAsync(AuthRequest Request)
        {
            AuthResponse response = new();

            var user = await userManager.FindByEmailAsync(Request.Email);
            if (user == null)
            {
                response.StatusCode = 404;
                return response;
            }
            var result = await signInManager.PasswordSignInAsync(user.UserName, Request.Password, false, false);

            if (!result.Succeeded)
            {
                response.StatusCode = 401;
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.StatusCode = 400;
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateTokenAsync(user);

            response.UserId = user.Id;
            response.Username = user.UserName;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;

            var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.Verification = user.EmailConfirmed;
            response.PhoneNumber = user.PhoneNumber;
            response.JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;

        }

        public async Task<ApiResponse<AccountDto>> GetAccoundDetailsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<AccountDto>.ErrorResponse("User account not found");

            AccountDto account = new()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                Username = user.UserName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
            return ApiResponse<AccountDto>.SuccessResponse(account);
        }
        public async Task<ApiResponse<UpdateAccountDto>> UpdateAccountDetailsAsync(UpdateAccountDto updateAccountDto, string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return ApiResponse<UpdateAccountDto>.ErrorResponse("Account not found");
            }

            user.FirstName = updateAccountDto.FirstName;
            user.LastName = updateAccountDto.LastName;
            user.UserName = updateAccountDto.Username;

            var updateUser = await userManager.UpdateAsync(user);

            UpdateAccountDto accountDto = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName
            };
            return ApiResponse<UpdateAccountDto>.SuccessResponse(accountDto);
        }
        public async Task<ApiResponse<ForgotResponse>> GetForgotPasswordAsync(ForgotRequest forgotRequest)
        {
            ForgotResponse response = new();

            var account = await userManager.FindByEmailAsync(forgotRequest.Email);

            if (account == null)
            {
                return ApiResponse<ForgotResponse>.ErrorResponse("No accounts registered with this email");
            }

            var verification = await SendForgotPasswordAsync(account);
            await emailSender.SendAsync(new EmailRequestDto
            {
                To = forgotRequest.Email,
                Body = $@"" ,
                Subject = "Recuperación de contraseña"

            });

            response.Message = "Se envio el email, chequea tu inbox";
            return ApiResponse<ForgotResponse>.SuccessResponse(response);


        }

        public async Task<ApiResponse<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
        {
            ResetPasswordResponse response = new();
            var account = await userManager.FindByEmailAsync(resetPasswordRequest.Email);

            if (account == null)
            {
                return ApiResponse<ResetPasswordResponse>.ErrorResponse("No accounts registered with this email");
            }

            resetPasswordRequest.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordRequest.Token));

            var result = await userManager.ResetPasswordAsync(account, resetPasswordRequest.Token, resetPasswordRequest.Password);

            if (!result.Succeeded)
            {
                return ApiResponse<ResetPasswordResponse>.ErrorResponse("An Error has occured trying to reset your password");
            }
            response.Message = "Your password has been reset";

            return ApiResponse<ResetPasswordResponse>.SuccessResponse(response);
        }

        public async Task LogOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task RemoveAccountAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
        }

        

    
    }
}
