using System;

namespace ConcertOne.Common.ServiceInterface
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}
