﻿using ConcertOne.Dal.Behavior;

using System;
using System.Collections.Generic;

namespace ConcertOne.Dal.Entity
{
    public sealed class Concert : IAuditableConcertOneEntity
    {
        public Guid Id { get; set; }

        public string Artist { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public string Description { get; set; }

        public ICollection<ConcertTag> ConcertTags { get; set; }

        public ICollection<TicketLimit> TicketLimits { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Concert()
        {
            ConcertTags = new HashSet<ConcertTag>();
            TicketLimits = new HashSet<TicketLimit>();
        }
    }
}
