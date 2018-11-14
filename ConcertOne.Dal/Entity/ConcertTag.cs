using ConcertOne.Dal.Behavior;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConcertOne.Dal.Entity
{
    public sealed class ConcertTag : IAuditableConcertOneEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ConcertId { get; set; }

        public Concert Concert { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
