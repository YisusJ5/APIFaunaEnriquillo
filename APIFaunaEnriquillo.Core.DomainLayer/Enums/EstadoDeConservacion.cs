using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.Enums
{
    public enum EstadoDeConservacion
    {
        Extinto,
        EnPeligroCritico,
        EnPeligro,
        Vulnerable,
        CasiAmenazado,
        PreocupacionMenor,
        DatosInsuficientes,
        NoEvaluado
    }
}
