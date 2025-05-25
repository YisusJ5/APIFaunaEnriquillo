using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service
{
    public interface ICloudinaryService
    {
<<<<<<< HEAD
        Task<string> UploadImageAsync(Stream fileStream, string imageName, CancellationToken cancellationToken);   
=======
        Task<string> UploadImageAsync(
            Stream fileStream,
            string imageName,
            CancellationToken cancellationToken
            );
>>>>>>> origin/dev

    }
}
