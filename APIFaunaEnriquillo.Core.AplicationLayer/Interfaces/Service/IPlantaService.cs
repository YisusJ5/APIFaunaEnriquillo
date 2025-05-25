using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIFaunaEnriquillo.Core.AplicationLayer.Dtos.PlantasDto;

namespace APIFaunaEnriquillo.Core.AplicationLayer.Interfaces.Service
{
    public interface IPlantaService: ICommonService<PlantaInsertDto, PlantaUpdateDto, PlantaDto>
    {
    }
}
