using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.DomainEvents.Plant
{
    public class PlantCreatedDomainEvent: IDomainEvent
    {
        public Guid PlantId { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public PlantCreatedDomainEvent(Guid plantId)
        {
            PlantId = plantId;
        }
    }
    
}
