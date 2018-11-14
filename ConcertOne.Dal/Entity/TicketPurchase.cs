using ConcertOne.Dal.Behavior;
using ConcertOne.Dal.Identity;

using System;

namespace ConcertOne.Dal.Entity
{
    public sealed class TicketPurchase : IAuditableConcertOneEntity
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public User User { get; set; }

        public Guid? TicketLimitId { get; set; }

        public TicketLimit TicketLimit { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
