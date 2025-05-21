using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Setting
{
    public class JWTSetting
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? DurationInMinutes { get; set; }
    }
}
