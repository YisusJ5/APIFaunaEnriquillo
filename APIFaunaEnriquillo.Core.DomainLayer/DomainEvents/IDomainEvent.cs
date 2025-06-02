using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFaunaEnriquillo.Core.DomainLayer.DomainEvents
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }


    }
}
