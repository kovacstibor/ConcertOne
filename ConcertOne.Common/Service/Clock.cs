using ConcertOne.Common.ServiceInterface;

using System;

namespace ConcertOne.Common.Service
{
    public sealed class Clock : IClock
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
