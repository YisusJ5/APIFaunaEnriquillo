using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.DomainLayer.Setting;
using APIFaunaEnriquillo.InfraestructureLayer.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIFaunaEnriquillo.InfrastructureLayer.Shared
{
    public static class AddShared
    {

    public static void AddSharedLayer(this IServiceCollection service, IConfiguration configuration)
        {
            #region configuration
            service.Configure<CloudinarySetting>(configuration.GetSection("CloudinarySetting"));
            service.Configure<EmailSetting>(configuration.GetSection("EmailSetting"));


            #endregion

            #region services
            service.AddTransient<ICloudinaryService, CloudinaryService>();
            service.AddTransient<IEmailService, EmailService>();

            #endregion
        }

    }
}
