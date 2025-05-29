using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service
{
    public interface ICodeConfirmation
    {
        Task<string> GenerateCodeConfirmation(string emailAddress);
    }
}
