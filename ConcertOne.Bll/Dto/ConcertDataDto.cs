using System;
using System.Collections.Generic;

namespace ConcertOne.Bll.Dto
{
    public sealed class ConcertDataDto
    {
        public string Artist { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public string AttachedImageUrl { get; set; }

        public string Description { get; set; }

        public Dictionary<Guid, int> AvailableTickets { get; set; }

        public ConcertDataDto()
        {
            AvailableTickets = new Dictionary<Guid, int>();
        }
    }
}
