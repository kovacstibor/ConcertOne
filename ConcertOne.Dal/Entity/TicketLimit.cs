using ConcertOne.Dal.Behavior;

using System;
using System.Collections.Generic;

namespace ConcertOne.Dal.Entity
{
    public sealed class TicketLimit : IAuditableConcertOneEntity
    {
        public Guid Id { get; set; }

        public int Limit { get; set; }

        public int UnitPrice { get; set; }

        public Guid? ConcertId { get; set; }

        public Concert Concert { get; set; }

        public Guid? TicketCategoryId { get; set; }

        public TicketCategory TicketCategory { get; set; }

        public ICollection<TicketPurchase> TicketPurchases { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public TicketLimit()
        {
            TicketPurchases = new HashSet<TicketPurchase>();
        }
    }
}
