using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.DomainEvents.Habitat
{
    public class HabitatCreatedDomainEvent : IDomainEvent
    {
        public Guid HabitatId { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public HabitatCreatedDomainEvent(Guid habitatId)
        {
            HabitatId = habitatId;
        }
    }
}
