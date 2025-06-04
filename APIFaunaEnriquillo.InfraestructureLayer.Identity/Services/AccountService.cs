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
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Register;
using APIFaunaEnriquillo.Core.DomainLayer.Enums;

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

        private async Task<string> SendVerificationEmailUrlAsync(User user)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            return code;
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
            response.IsVerified = user.EmailConfirmed;
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
            var resetLink = $"http://localhost:4200/reset-password?email={Uri.EscapeDataString(forgotRequest.Email)}&token={Uri.EscapeDataString(verification)}";

            await emailSender.SendAsync(new EmailRequestDto
            {
                To = forgotRequest.Email,
                Body = $@"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html dir='ltr' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office' lang='es'>
 <head>
  <meta charset='UTF-8'>
  <meta content='width=device-width, initial-scale=1' name='viewport'>
  <meta name='x-apple-disable-message-reformatting'>
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <meta content='telephone=no' name='format-detection'>
  <title>Restaurar Contraseña</title>
 </head>
 <body class='body' style='width:100%;height:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'>
  <div dir='ltr' class='es-wrapper-color' lang='es' style='background-color:#F1F6EF'>
   <table width='100%' cellspacing='0' cellpadding='0' class='es-wrapper' role='none' style='width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#F1F6EF'>
     <tr>
      <td valign='top' style='padding:0;Margin:0'>
       <table cellpadding='0' cellspacing='0' align='center' class='es-content' role='none' style='width:100%;table-layout:fixed !important'></table>
       <table cellpadding='0' cellspacing='0' align='center' class='es-content' role='none' style='width:100%;table-layout:fixed !important'>
         <tr>
          <td align='center' style='padding:0;Margin:0'>
           <table bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' class='es-content-body' role='none' style='background-color:#FFFFFF;width:600px;border-radius:16px;overflow:hidden;box-shadow:0 10px 25px rgba(0,0,0,0.08);'>
             <tr>
              <td align='left' class='es-m-p0b' style='padding:0;Margin:0;padding-top:15px;padding-right:20px;padding-left:20px'>
               <table cellpadding='0' cellspacing='0' width='100%' role='none'>
                 <tr>
                  <td align='center' valign='top' style='padding:0;Margin:0;width:560px'>
                   <table cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                     <tr>
                      <td align='center' style='padding:0;Margin:0;padding-top:10px;padding-bottom:10px;font-size:0px'>
                        <a target='_blank' href='https://upload.wikimedia.org/wikipedia/commons/thumb/c/c7/Cabritos_2011_12_13Feb_221.JPG/1280px-Cabritos_2011_12_13Feb_221.JPG' style='text-decoration:underline;color:#5C68E2;font-size:14px'>
                          <img src='https://ekxjnkq.stripocdn.email/content/guids/CABINET_9edf6ab6e40ac9330ee90c4b08b854ebc40bfe8569461cff6386603619a5f395/images/cabritos_2011_12_13feb_221.JPG' alt='' width='450' title='' class='adapt-img' style='display:block;font-size:14px;border:0;outline:none;text-decoration:none;border-radius:12px'>
                        </a>
                      </td>
                     </tr>
                     <tr>
                      <td align='center' style='padding-top:10px;'>
                        <p style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:16px;line-height:24px;color:#5c68e2;margin:0 0 10px 0;'>
                          Esperamos que te encuentres bien. Te enviamos este mensaje para ayudarte a restaurar tu contraseña.
                        </p>
                      </td>
                     </tr>
                     <tr>
                      <td align='center' class='es-m-p0r es-m-p0l es-text-2265' style='padding-top:15px;padding-right:40px;padding-bottom:15px;padding-left:40px'>
                        <h1 class='es-m-txt-c es-text-mobile-size-28 es-override-size' style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:35px;font-weight:bold;line-height:42px;color:#333333'>Restaurar Contraseña</h1>
                      </td>
                     </tr>
                     <tr>
                      <td align='center' style='padding-top:10px'>
                        <p style='font-family:arial, helvetica neue, helvetica, sans-serif;line-height:28px;color:#333333;font-size:14px'>Después de dar click en el botón sigue estos pasos:</p>
                        <p style='font-family:arial, helvetica neue, helvetica, sans-serif;line-height:28px;color:#333333;font-size:14px'>1. Coloca tu nueva contraseña</p>
                        <p style='font-family:arial, helvetica neue, helvetica, sans-serif;line-height:28px;color:#333333;font-size:14px'>2. Confirma tu nueva contraseña</p>
                        <p style='font-family:arial, helvetica neue, helvetica, sans-serif;line-height:28px;color:#333333;font-size:14px'>3. Haz click en aceptar</p>
                        <div style='background:#f1f6ef;border-radius:8px;padding:18px 0;margin:18px 0 18px 0;display:inline-block;min-width:220px;'>
                          <span style='font-size:22px;letter-spacing:4px;color:#5c68e2;font-family:Courier New, monospace;font-weight:bold;'>{verification}</span>
                        </div>
                      </td>
                     </tr>
                   </table>
                  </td>
                 </tr>
               </table>
              </td>
             </tr>
             <tr>
              <td align='left' style='padding-right:20px;padding-left:20px;padding-bottom:20px'>
               <table cellpadding='0' cellspacing='0' width='100%' role='none'>
                 <tr>
                  <td align='center' valign='top' style='width:560px'>
                   <table cellpadding='0' cellspacing='0' width='100%' style='border-radius:5px' role='presentation'>
                     <tr>
                      <td align='center' style='padding-top:10px;padding-bottom:10px'>
                        <span class='es-button-border' style='border-style:solid;border-color:#2CB543;background:#5ce277;border-width:0px;display:inline-block;border-radius:6px;width:auto'>
                          <a href='{resetLink}' target='_blank' class='es-button' style='text-decoration:none;color:#FFFFFF;font-size:20px;padding:10px 30px 10px 30px;display:inline-block;background:#5ce277;border-radius:6px;font-family:arial, helvetica neue, helvetica, sans-serif;line-height:24px;width:auto;text-align:center;letter-spacing:0;'>Restaura tu contraseña aquí</a>
                        </span>
                      </td>
                     </tr>
                     <tr>
                      <td align='center' class='es-text-2848' style='padding-top:10px'>
                        <h3 class='es-m-txt-c es-text-mobile-size-18' style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:18px;font-weight:bold;line-height:27px;color:#333333'>Por seguridad este link dejará de ser útil después de 30 minutos.</h3>
                      </td>
                     </tr>
                     <tr>
                      <td align='center' style='padding-top:10px;padding-bottom:10px'>
                        <p style='font-family:arial, helvetica neue, helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px'>Si no hiciste la solicitud por favor ignora este mensaje.</p>
                      </td>
                     </tr>
                   </table>
                  </td>
                 </tr>
               </table>
              </td>
             </tr>
           </table>
          </td>
         </tr>
       </table>
       <table cellpadding='0' cellspacing='0' align='center' class='es-footer' role='none' style='width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top'>
         <tr>
          <td align='center'>
           <table align='center' cellpadding='0' cellspacing='0' class='es-footer-body' style='background-color:transparent;width:600px' role='none'>
             <tr>
              <td align='left' style='padding-right:20px;padding-left:20px;padding-bottom:20px;padding-top:20px'>
               <table cellpadding='0' cellspacing='0' width='100%' role='none'>
                 <tr>
                  <td align='left' style='width:560px'>
                   <table cellpadding='0' cellspacing='0' width='100%' role='presentation'>
                     <tr>
                      <td align='center' style='padding-bottom:35px'>
                        <p style='font-family:arial, helvetica neue, helvetica, sans-serif;line-height:18px;color:#333333;font-size:12px'>Derechos de imagen reservados</p>
                        <p style='font-family:arial, helvetica neue, helvetica, sans-serif;line-height:18px;color:#333333;font-size:12px'>Autor de la imagen: <a href='https://commons.wikimedia.org/wiki/User_talk:Ymleon' style='text-decoration:underline;color:#333333;font-size:12px'>Yolanda M. Leon</a> <br>Bajo la licencia: CC BY-SA 4.0</p>
                      </td>
                     </tr>
                   </table>
                  </td>
                 </tr>
               </table>
              </td>
             </tr>
           </table>
          </td>
         </tr>
       </table>
      </td>
     </tr>
   </table>
  </div>
 </body>
</html>" ,
                Subject = "Recuperación de contraseña 🦎"

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

        public async Task<ApiResponse<RegisterResponse>> RegisterOwnerAsync(RegisterRequest registerRequest)
        {
            RegisterResponse response = new();

            var userSameUsername = await userManager.FindByNameAsync(registerRequest.Username);
            if (userSameUsername != null)
            {
                return ApiResponse<RegisterResponse>.ErrorResponse($"this username {userSameUsername} is already taken");
            }

            var userWithEmail = await userManager.FindByEmailAsync(registerRequest.Email);
            if (userWithEmail != null)
            {
                return ApiResponse<RegisterResponse>.ErrorResponse($"this email {userWithEmail} is already taken");
            }

            User owner = new()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.Username,
                PhoneNumber = registerRequest.PhoneNumber,
                Email = registerRequest.Email
            };


            var result = await userManager.CreateAsync(owner, registerRequest.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(owner, Roles.Admin.ToString());
                response.Email = registerRequest.Email;
                response.Username = registerRequest.Username;
                response.UserId = owner.Id;

                var verification = await SendVerificationEmailUrlAsync(owner);
                await emailSender.SendAsync(new EmailRequestDto
                {
                    To = registerRequest.Email,
                    Body = @$"
<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html dir='ltr' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office' lang='es'>
<head>
  <meta charset='UTF-8'>
  <meta content='width=device-width, initial-scale=1' name='viewport'>
  <meta name='x-apple-disable-message-reformatting'>
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <meta content='telephone=no' name='format-detection'>
  <title>Bienvenido a Fauna Enriquillo</title>
</head>
<body class='body' style='width:100%;height:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'>
  <div dir='ltr' class='es-wrapper-color' lang='es' style='background-color:#F1F6EF'>
    <table width='100%' cellspacing='0' cellpadding='0' class='es-wrapper' role='none' style='width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#F1F6EF'>
      <tr>
        <td valign='top' style='padding:0;Margin:0'>
          <table cellpadding='0' cellspacing='0' align='center' class='es-content' role='none' style='width:100%;table-layout:fixed !important'></table>
          <table cellpadding='0' cellspacing='0' align='center' class='es-content' role='none' style='width:100%;table-layout:fixed !important'>
            <tr>
              <td align='center' style='padding:0;Margin:0'>
                <table bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' class='es-content-body' role='none' style='background-color:#FFFFFF;width:600px;border-radius:16px;overflow:hidden;box-shadow:0 10px 25px rgba(0,0,0,0.08);'>
                  <tr>
                    <td align='center' style='padding:0;Margin:0;padding-top:20px'>
                      <img src='https://upload.wikimedia.org/wikipedia/commons/thumb/6/69/Lago_Enriquillo2.jpg/1280px-Lago_Enriquillo2.jpg' alt='Lago Enriquillo' width='480' style='display:block;border-radius:12px;margin-bottom:20px;max-width:90%;height:auto;'>
                    </td>
                  </tr>
                  <tr>
                    <td align='center' style='padding:0 40px 0 40px;'>
                      <h1 style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:30px;font-weight:bold;line-height:38px;color:#333333;margin-bottom:10px;'>¡Bienvenido a Fauna Enriquillo!</h1>
                      <p style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:16px;line-height:24px;color:#333333;margin:0 0 18px 0;'>
                        Gracias por registrarte y formar parte de nuestra comunidad.<br>
                        Estamos felices de tenerte con nosotros.
                      </p>
                      <p style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:15px;line-height:22px;color:#333333;margin:0 0 10px 0;'>
                        Para activar tu cuenta, utiliza el siguiente código de verificación:
                      </p>
                      <div style='background:#f1f6ef;border-radius:8px;padding:18px 0;margin:0 0 18px 0;'>
                        <span style='font-size:22px;letter-spacing:4px;color:#5c68e2;font-family:Courier New, monospace;font-weight:bold;'>{verification}</span>
                      </div>
                      <p style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:13px;line-height:20px;color:#888;margin:0 0 10px 0;'>
                        Si no solicitaste esta cuenta, puedes ignorar este mensaje.
                      </p>
                    </td>
                  </tr>
                  <tr>
                    <td align='center' style='padding:0 0 20px 0;'>
                      <p style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:12px;line-height:18px;color:#bdbdbd;margin:0;'>
                        © 2025 Fauna Enriquillo. Todos los derechos reservados.
                      </p>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </div>
</body>
</html>
",
                    Subject = "Confirmación de registro 🐊"

                });
            }
            else
            {
                return ApiResponse<RegisterResponse>.ErrorResponse($"An error occurred trying to register the user");
            }
            return ApiResponse<RegisterResponse>.SuccessResponse(response);
        }
            

        public async Task<ApiResponse<RegisterResponse>> RegisterAccountAsync(RegisterRequest registerRequest, Roles roles)
        {
            RegisterResponse response = new();
            var username = await userManager.FindByNameAsync(registerRequest.Username);
            if (username != null)
            {
                return ApiResponse<RegisterResponse>.ErrorResponse($"this user {registerRequest.Username} is already taken");
            }

            var userWithEmail = await userManager.FindByEmailAsync(registerRequest.Email);
            if (userWithEmail != null)
            {
                return ApiResponse<RegisterResponse>.ErrorResponse($"this email {registerRequest.Email} is already taken");
            }

            User user = new()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, registerRequest.Password);
            if (result.Succeeded)
            {
                response.Email = registerRequest.Email;
                response.Username = registerRequest.Username;
                response.UserId = user.Id;

                await userManager.AddToRoleAsync(user, roles.ToString());
                var verification = await SendVerificationEmailUrlAsync(user);
                await emailSender.SendAsync(new EmailRequestDto
                {
                    To = registerRequest.Email,
                    Body = $@"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html dir='ltr' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office' lang='es'>
<head>
  <meta charset='UTF-8'>
  <meta content='width=device-width, initial-scale=1' name='viewport'>
  <meta name='x-apple-disable-message-reformatting'>
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <meta content='telephone=no' name='format-detection'>
  <title>Bienvenido a Fauna Enriquillo</title>
</head>
<body class='body' style='width:100%;height:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'>
  <div dir='ltr' class='es-wrapper-color' lang='es' style='background-color:#F1F6EF'>
    <table width='100%' cellspacing='0' cellpadding='0' class='es-wrapper' role='none' style='width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#F1F6EF'>
      <tr>
        <td valign='top' style='padding:0;Margin:0'>
          <table cellpadding='0' cellspacing='0' align='center' class='es-content' role='none' style='width:100%;table-layout:fixed !important'></table>
          <table cellpadding='0' cellspacing='0' align='center' class='es-content' role='none' style='width:100%;table-layout:fixed !important'>
            <tr>
              <td align='center' style='padding:0;Margin:0'>
                <table bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' class='es-content-body' role='none' style='background-color:#FFFFFF;width:600px;border-radius:16px;overflow:hidden;box-shadow:0 10px 25px rgba(0,0,0,0.08);'>
                  <tr>
                    <td align='center' style='padding:0;Margin:0;padding-top:20px'>
                      <img src='https://upload.wikimedia.org/wikipedia/commons/thumb/6/69/Lago_Enriquillo2.jpg/1280px-Lago_Enriquillo2.jpg' alt='Lago Enriquillo' width='480' style='display:block;border-radius:12px;margin-bottom:20px;max-width:90%;height:auto;'>
                    </td>
                  </tr>
                  <tr>
                    <td align='center' style='padding:0 40px 0 40px;'>
                      <h1 style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:30px;font-weight:bold;line-height:38px;color:#333333;margin-bottom:10px;'>¡Bienvenido a Fauna Enriquillo!</h1>
                      <p style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:16px;line-height:24px;color:#333333;margin:0 0 18px 0;'>
                        Gracias por registrarte y formar parte de nuestra comunidad.<br>
                        Estamos felices de tenerte con nosotros.
                      </p>
                      <p style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:15px;line-height:22px;color:#333333;margin:0 0 10px 0;'>
                        Para activar tu cuenta, utiliza el siguiente código de verificación:
                      </p>
                      <div style='background:#f1f6ef;border-radius:8px;padding:18px 0;margin:0 0 18px 0;'>
                        <span style='font-size:22px;letter-spacing:4px;color:#5c68e2;font-family:Courier New, monospace;font-weight:bold;'>{verification}</span>
                      </div>
                      <p style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:13px;line-height:20px;color:#888;margin:0 0 10px 0;'>
                        Si no solicitaste esta cuenta, puedes ignorar este mensaje.
                      </p>
                    </td>
                  </tr>
                  <tr>
                    <td align='center' style='padding:0 0 20px 0;'>
                      <p style='font-family:arial, helvetica neue, helvetica, sans-serif;font-size:12px;line-height:18px;color:#bdbdbd;margin:0;'>
                        © 2025 Fauna Enriquillo. Todos los derechos reservados.
                      </p>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </div>
</body>
</html>",
                    Subject = "Confirmación de registro 🐊"
                });
            }
            else
            {
                return ApiResponse<RegisterResponse>.ErrorResponse($"An error occurred trying to register the user");
            }
            return ApiResponse<RegisterResponse>.SuccessResponse(response);
        }

        public async Task<ApiResponse<string>> ConfirmAccountAsync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<string>.ErrorResponse($"No account registered with this {userId} user id");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? ApiResponse<string>.SuccessResponse($"Your account has been successfully confirmed!")
                : ApiResponse<string>.ErrorResponse($"An error occurred trying to confirm your account");
        }
    }
}
