using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service;
using APIFaunaEnriquillo.Core.DomainLayer.Setting;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;

namespace APIFaunaEnriquillo.InfraestructureLayer.Shared.Services
{
    public class CloudinaryService(IOptions<CloudinarySetting> cloudinarySetting) : ICloudinaryService
    {
        private CloudinarySetting _cloudinarySetting { get; } = cloudinarySetting.Value;

        public async Task<string> UploadImageAsync(Stream fileStream, string imageName, CancellationToken cancellationToken)
        {
            Cloudinary cloudinary = new Cloudinary(_cloudinarySetting.UrlImage);

            ImageUploadParams image = new()
            {
                File = new FileDescription(imageName, fileStream),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,

            };
            var uploadResult = await cloudinary.UploadAsync(image, cancellationToken);
            if(uploadResult.SecureUrl == null)
            throw new Exception($"Error al subir la imagen: {uploadResult.Error?.Message ?? "No se recibió URL de Cloudinary."}");
            return uploadResult.SecureUrl.ToString();

        }
    }
}
