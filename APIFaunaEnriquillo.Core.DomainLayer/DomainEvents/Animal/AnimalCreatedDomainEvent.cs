using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.DomainEvents.Animal
{
    public class AnimalCreatedDomainEvent: IDomainEvent
    {
        public Guid AnimalId { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public AnimalCreatedDomainEvent(Guid animalId)
        {
            AnimalId = animalId;
        }


    }
}
