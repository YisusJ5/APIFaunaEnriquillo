using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Forgot;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Auth;
using APIFaunaEnriquillo.Validations.Account;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Register;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Password.Reset;
using APIFaunaEnriquillo.ExceptionHandlers;
using APIFaunaEnriquillo.Exceptions;

namespace APIFaunaEnriquillo.Extensions
{
    public static class ExtensionService
    {
        public static void AddSwaggerExtension (this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Fauna Enriquillo",
                    Description =
                        "Api fauna enriquillo",
                    Contact = new OpenApiContact
                    {
                        Name = "Proyecto de tesis José Antonio Y Yaservi Pérez",
                        Email = "enrriquillowiki@gmail.com"
                    }
                });
            });
        }


        public static void AddValidations (this IServiceCollection services) 
        {
            services.AddScoped<IValidator<AuthRequest>, AuthValidation>();
            services.AddScoped<IValidator<ForgotRequest>, ForgotPasswordValidation>();
            services.AddScoped<IValidator<RegisterRequest>, RegisterValidation>();
            services.AddScoped<IValidator<ResetPasswordRequest>, ResetPasswordValidation>();
            services.AddScoped<IValidator<UpdateAccountDto>, UpdatePasswordValidation>();

        }

        public static void AddVersionIng(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
        }


        public static void AddExceptions(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptions>();
            services.AddExceptionHandler<DbUpdateExceptionHandler>();
            services.AddProblemDetails();
        }
        public static void AddLimit(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.OnRejected = async (context, toke) =>
                {
                    await context.HttpContext.Response.WriteAsync("Request limit exceeded. Please try again later", cancellationToken: toke);
                };

                options.AddFixedWindowLimiter("fixed", limiterOptions =>
                {
                    limiterOptions.Window = TimeSpan.FromSeconds(10);
                    limiterOptions.PermitLimit = 5;
                });

                options.AddSlidingWindowLimiter("sliding", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 5;
                    limiterOptions.SegmentsPerWindow = 5;
                    limiterOptions.Window = TimeSpan.FromMinutes(4);
                });

                options.AddTokenBucketLimiter("tokenBucketPolicy", limiterOptions =>
                {
                    limiterOptions.TokenLimit = 10; 
                    limiterOptions.TokensPerPeriod = 2;         
                    limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(5); 
                });
            });
        }



    }
}
