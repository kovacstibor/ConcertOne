using System;

namespace ConcertOne.Dal.Behavior
{
    public interface IAuditable
    {
        Guid CreatorId { get; set; }

        DateTime CreationTime { get; set; }

        Guid? LastModifierId { get; set; }

        DateTime? LastModificationTime { get; set; }
    }
}
