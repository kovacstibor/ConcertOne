using ConcertOne.Bll.Dto.Concert;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ConcertOne.Web.ViewModels.Concert
{
    public class ConcertDetailsViewModel
    {
        [JsonProperty( "Id" )]
        public Guid Id { get; set; }

        [JsonProperty( "Artist" )]
        public string Artist { get; set; }

        [JsonProperty( "AvailableTickets" )]
        public List<TicketLimitViewModel> AvailableTickets { get; set; }

        [JsonProperty( "Description" )]
        public string Description { get; set; }

        [JsonProperty( "Location" )]
        public string Location { get; set; }

        [JsonProperty( "StartDate" )]
        public string StartDate { get; set; }

        [JsonProperty( "StartTime" )]
        public string StartTime { get; set; }

        [JsonProperty( "Tags" )]
        public List<string> Tags { get; set; }

        public ConcertDetailsViewModel()
        {
            AvailableTickets = new List<TicketLimitViewModel>();
        }

        public ConcertDetailsViewModel( ConcertDetailsDto concertDetailsDto )
            : this()
        {
            if (concertDetailsDto == null)
            {
                throw new ArgumentNullException( nameof( concertDetailsDto ) );
            }
            Id = concertDetailsDto.Id;
            Artist = concertDetailsDto.Artist;
            Description = concertDetailsDto.Description;
            Location = concertDetailsDto.Location;
            StartDate = concertDetailsDto.StartTime.ToString( "yyyy-MM-dd" );
            StartTime = concertDetailsDto.StartTime.ToString( "HH:mm" );
            Tags = concertDetailsDto.Tags;
            AvailableTickets = concertDetailsDto.AvailableTickets
                                                .Select( at => new TicketLimitViewModel( at ) )
                                                .ToList();
        }
    }
}
