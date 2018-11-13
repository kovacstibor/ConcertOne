using ConcertOne.Dal.Behavior;
using ConcertOne.Dal.Identity;

using System;
using System.Collections.Generic;

namespace ConcertOne.Dal.Entity
{
    public sealed class TicketPurchase : IAuditableConcertOneEntity
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public User User { get; set; }

        public Guid? ConcertId { get; set; }

        public Concert Concert { get; set; }

        public Guid CreatorId { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public TicketPurchase()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}
