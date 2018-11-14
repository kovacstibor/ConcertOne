using ConcertOne.Dal.Behavior;

using System;
using System.Collections.Generic;

namespace ConcertOne.Dal.Entity
{
    public sealed class TicketCategory : IAuditableConcertOneEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<TicketLimit> TicketLimits { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public TicketCategory()
        {
            TicketLimits = new HashSet<TicketLimit>();
        }
    }
}
