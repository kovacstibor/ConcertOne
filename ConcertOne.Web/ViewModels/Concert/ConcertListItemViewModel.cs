using ConcertOne.Bll.Dto.Concert;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace ConcertOne.Web.ViewModels.Concert
{
    public class ConcertListItemViewModel
    {
        [JsonProperty( "Id" )]
        public Guid Id { get; set; }

        [JsonProperty( "Artist" )]
        public string Artist { get; set; }

        [JsonProperty( "Location" )]
        public string Location { get; set; }

        [JsonProperty( "StartTime" )]
        public DateTime StartTime { get; set; }

        [JsonProperty( "Tags" )]
        public List<string> Tags { get; set; }

        [JsonProperty( "AvailableTickets" )]
        public Dictionary<string, int> AvailableTickets { get; set; }

        public ConcertListItemViewModel()
        {
            AvailableTickets = new Dictionary<string, int>();
        }

        public ConcertListItemViewModel( ConcertListItemDto concertListItemDto )
            : this()
        {
            if (concertListItemDto == null)
            {
                throw new ArgumentNullException( nameof( concertListItemDto ) );
            }

            Id = concertListItemDto.Id;
            Artist = concertListItemDto.Artist;
            Location = concertListItemDto.Location;
            StartTime = concertListItemDto.StartTime;
            Tags = concertListItemDto.Tags;
            AvailableTickets = concertListItemDto.AvailableTickets;
        }
    }
}
