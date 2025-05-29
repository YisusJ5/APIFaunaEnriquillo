using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Auth;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Forgot;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Reset;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Register;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;
using APIFaunaEnriquillo.Core.DomainLayer.Utils;
using CloudinaryDotNet;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service
{
    public interface IAccountService
    {

        Task<AuthResponse> AuthAsync(AuthRequest authRequest);
        Task<ApiResponse<UpdateAccountDto>> UpdateAccountDetailsAsync(UpdateAccountDto updateAccountDto, string id);
        Task<ApiResponse<RegisterResponse>> RegisterOwnerAsync(RegisterRequest registerRequest);
        Task<ApiResponse<RegisterResponse>> RegisterAccountAsync(RegisterRequest registerRequest, Roles roles);
        Task<ApiResponse<string>> ConfirmAccountAsync(string userId, string token);
        Task<ApiResponse<ForgotResponse>> GetForgotPasswordAsync(ForgotRequest forgotRequest);
        Task<ApiResponse<ResetPasswordResponse>> ResetPasswordAsync (ResetPasswordRequest resetPasswordRequest);
        Task <ApiResponse<AccountDto>> GetAccoundDetailsAsync (string userId);
        Task LogOutAsync();
        Task RemoveAccountAsync(string userId);

    }
}
