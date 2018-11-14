using System;
using System.Collections.Generic;

namespace ConcertOne.Bll.Dto.Concert
{
    public sealed class ConcertListItemDto
    {
        public Guid Id { get; set; }

        public string Artist { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public List<string> Tags { get; set; }

        public Dictionary<string, int> AvailableTickets { get; set; }

        public ConcertListItemDto()
        {
            Tags = new List<string>();
            AvailableTickets = new Dictionary<string, int>();
        }
    }
}
