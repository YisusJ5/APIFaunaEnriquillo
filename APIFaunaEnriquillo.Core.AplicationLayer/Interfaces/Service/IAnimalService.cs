using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.AnimalesDto;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service
{
    public interface IAnimalService: ICommonService<AnimalInsertDto, AnimalUpdateDto, AnimalDto>
    {
    }
}
