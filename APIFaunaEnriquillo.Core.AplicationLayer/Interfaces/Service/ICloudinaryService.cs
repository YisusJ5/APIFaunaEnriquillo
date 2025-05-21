using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(Stream fileStream, string imageName, CancellationToken cancellationToken);   

    }
}
