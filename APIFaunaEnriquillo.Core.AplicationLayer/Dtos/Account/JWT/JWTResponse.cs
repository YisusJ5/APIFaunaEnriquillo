using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Dtos.Account.Jwt
{
    public class JWTResponse
    {
        public string? Error { get; set; }
        public bool HasError { get; set; }
    }
}
