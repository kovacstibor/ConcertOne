using System;
using System.Collections.Generic;

namespace ConcertOne.Bll.Dto.Concert
{
    public sealed class ConcertDetailsDto
    {
        public Guid Id { get; set; }

        public string Artist { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public List<string> Tags { get; set; }

        public List<TicketLimitDto> AvailableTickets { get; set; }

        public ConcertDetailsDto()
        {
            Tags = new List<string>();
            AvailableTickets = new List<TicketLimitDto>();
        }
    }
}
